using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.CoreAudioApi;

namespace AlbumRecorder {
	/// <summary>
	/// Split an album into tracks at silences
	/// </summary>
	public partial class SplitAlbum : Form {
		AudioFileReader m_Reader;
		TaskRunner m_Task;

		public SplitAlbum() {
			InitializeComponent();
			if (Program.Album != null && Program.Album.Tracks.Count > 2) {
				// Default no of tracks is existing number
				txtTracks.Text = Program.Album.Tracks.Count.ToString();
			}
			// Load parameters from settings
			txtMinTrackLength.Text = Properties.Settings.Default.MinTrackLength.ToTimeSpanString();
			txtCentre.Text = Properties.Settings.Default.SilenceFilterCentre.ToString();
			txtQ.Text = Properties.Settings.Default.SilenceFilterQ.ToString();
			txtStartSilence.Text = Properties.Settings.Default.StartSilenceThreshold.ToString();
			txtEndSilence.Text = Properties.Settings.Default.EndSilenceThreshold.ToString();
			m_Task = new TaskRunner(TaskException);
		}

		public SplitAlbum(AudioFileReader reader)
			: this() {
			m_Reader = reader;
		}

		/// <summary>
		/// Number of tracks expected
		/// </summary>
		public int TrackCount;

		/// <summary>
		/// Found tracks
		/// </summary>
		public List<Track> Tracks;

		/// <summary>
		/// Do an action in the UI thread
		/// </summary>
		public void Despatch(Action a) {
			if (InvokeRequired) {
				Invoke(a);
			} else
				a();
		}

		/// <summary>
		/// Catch exceptions in the task
		/// </summary>
		private void TaskException(object sender, Exception ex) {
			System.Diagnostics.Trace.WriteLine(ex);
			Despatch(delegate() {
				lblStatus.Text = ex.Message;
			});
		}

		/// <summary>
		/// Set Status & Progress bar value
		/// </summary>
		void Progress(string status, int value) {
			Despatch(delegate() {
				lblStatus.Text = status;
				progressBar1.Value = value;
				progressBar1.Refresh();
			});
		}

		/// <summary>
		/// Information about a silence
		/// </summary>
		class GapInfo {
			public float Start;
			public float End;
			public float AverageVolume;
		}

		private void btnOK_Click(object sender, EventArgs e) {
			try {
				// Update settings from controls
				TrackCount = txtTracks.Text.ToInt(2, "Track count");
				float m = 0;
				if(!txtMinTrackLength.Text.TimeSpanToSeconds(ref m))
					throw new ApplicationException("Invalid min track length");
				Properties.Settings.Default.MinTrackLength = m;
				Properties.Settings.Default.SilenceFilterCentre = txtCentre.Text.ToFloat("Centre Frequency");
				Properties.Settings.Default.SilenceFilterQ = txtQ.Text.ToFloat("Filter width (Q)");
				Properties.Settings.Default.StartSilenceThreshold = txtStartSilence.Text.ToFloat("Start silence threshold");
				Properties.Settings.Default.EndSilenceThreshold = txtEndSilence.Text.ToFloat("Start Music threshold");
				Properties.Settings.Default.Save();
				btnOK.Enabled = false;	// Disable OK until finished
				m_Task.Run(delegate(TaskRunner.Task t) {
					Progress("Finding silences between tracks", 0);
					// Get list of potential gaps
					float length = m_Reader.WaveFormat.BytesToSeconds(m_Reader.Length);	// Length of file in secs
					List<GapInfo> gaps = new List<GapInfo>();	// Found silences
					GapFinder f = new GapFinder(m_Reader);
					float start = 0;	// Start of track
					Program.Trace("Album length {0}", length.ToTimeSpanString());
					while (!t.Stop && f.FindMusic(start, -1)) {
						// Found some music - add info about the gap
						gaps.Add(new GapInfo() {
							Start = start,
							End = f.Position,
							AverageVolume = f.AverageVolume
						});
						string status = "Track " + gaps.Count + " at " + f.Position.ToTimeSpanString();
						Program.Trace(status);
						Progress(status, (int)(100 * f.Position / length));
						if (f.FindSilence(f.Position, -1)) {
							// Found a silence
							while (!t.Stop && f.Position - start < Properties.Settings.Default.MinTrackLength) {
								// Track is too small - find music, then another silence
								Progress(status, (int)(100 * f.Position / length));
								if (!f.FindMusic(f.Position, -1))
									break;
								Progress(status, (int)(100 * f.Position / length));
								if (!f.FindSilence(f.Position, -1))
									break;
							}
							Program.Trace("Track " + gaps.Count + " ends at " + f.Position.ToTimeSpanString());
							start = f.Position;
						} else {
							Program.Trace("Album ends at " + f.Position.ToTimeSpanString());
							break;
						}
						Progress("Track " + gaps.Count + " ends at " + f.Position.ToTimeSpanString(), (int)(100 * f.Position / length));
					}
					// And the gap at the end
					gaps.Add(new GapInfo() {
						Start = start,
						End = length,
						AverageVolume = f.AverageVolume
					});
					if (!t.Stop) {
						if (gaps.Count < TrackCount + 1) {
							Despatch(delegate() {
								lblStatus.Text = "Only " + (gaps.Count - 1) + " Tracks found";
								Refresh();
							});
						} else {
							// Eliminate loudest gaps (but not first or last)
							while (gaps.Count > TrackCount + 1) {
								float vol = float.MinValue;
								int loudest = 0;
								for (int i = 1; i < gaps.Count - 2; i++) {
									if (gaps[i].AverageVolume > vol) {
										vol = gaps[i].AverageVolume;
										loudest = i;
									}
								}
								gaps.RemoveAt(loudest);
							}
						}
					}
					if (!t.Stop) {
						// Now create/update track information
						Tracks = new List<Track>();
						for (int i = 1; i < gaps.Count; i++) {
							GapInfo last = gaps[i - 1];
							// Copy existing track, or create new one
							Track trk = Program.Album != null && i <= Program.Album.Tracks.Count ?
								new Track(Program.Album.Tracks[i - 1]) : new Track();
							trk.Gap = last.End - last.Start;
							trk.LengthSeconds = gaps[i].Start - last.End;
							Tracks.Add(trk);
						}
					}
					if (!t.Stop) {
						// Have finished = close dialog with OK
						Despatch(delegate() {
							btnOK.Enabled = true;
							DialogResult = System.Windows.Forms.DialogResult.OK;
							Close();
						});
					}
				});
			} catch(Exception ex) {
				lblStatus.Text = ex.Message;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			m_Task.Stop();
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}

	}
}
