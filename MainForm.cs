using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using Hqub.MusicBrainz.API.Entities;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.CoreAudioApi;
using TagLib;

namespace AlbumRecorder {
	public partial class MainForm : Form {
		/// <summary>
		/// The only instance of the main form
		/// </summary>
		public static MainForm Instance;
		/// <summary>
		/// Album details dialog (single copy, so data is preserved between uses)
		/// </summary>
 		AlbumDetails m_AlbumDetails;
		/// <summary>
		/// Track details dialog (single copy, so data is preserved between uses)
		/// </summary>
		TrackDetails m_TrackDetails;
		/// <summary>
		/// Current filename
		/// </summary>
		string m_Filename;
		/// <summary>
		/// Wav file
		/// </summary>
		AudioFileReader m_Reader;
		/// <summary>
		/// TrackJoin controls - 1 more than the number of tracks (to cover beginning and end of album)
		/// </summary>
		List<TrackJoin> m_Tracks;
		/// <summary>
		/// For Undo button
		/// </summary>
		Stack<UndoAction> UndoStack = new Stack<UndoAction>();
		Stack<UndoAction> RedoStack = new Stack<UndoAction>();
		/// <summary>
		/// List of all controls that were enabled, and are now disabled, when we disable everything (see Enable)
		/// </summary>
		List<Control> m_Enabled;

		public MainForm() {
			Instance = this;
			m_AlbumDetails = new AlbumDetails();
			m_TrackDetails = new TrackDetails();
			InitializeComponent();
		}

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
		/// Set Status label (can call from any thread)
		/// </summary>
		public void Status(string text) {
			Despatch(delegate() { Instance.lblStatus.Text = text; });
		}

		/// <summary>
		/// Set Status label (can call from any thread)
		/// </summary>
		public void Status(string format, params object[] args) {
			Status(string.Format(format, args));
		}

		/// <summary>
		/// Enable or disable all the controls on the form (can call from any thread)
		/// </summary>
		/// <returns>true if controls were not already disabled</returns>
		bool Enable(bool enable) {
			// m_Enabled will not be null if Enable(false) has been called most recently (i.e. controls already disabled)
			bool enabled = m_Enabled == null;
			// Must run in UI thread, as it alters controls
			Despatch(delegate() {
				if (enable) {
					if (!enabled) {
						// Re-enable everything disabled last time
						foreach (Control c in m_Enabled)
							c.Enabled = true;
						m_Enabled = null;
					}
				} else {
					if (enabled) {
						// Disable all controls, and add them to the m_Enabled list
						m_Enabled = new List<Control>();
						DisableControls(this);
					}
				}
			});
			return enabled;
		}

		/// <summary>
		/// Recursively disable all controls whose parent is con, and add them to m_Enabled list
		/// </summary>
		void DisableControls(Control con) {
			foreach (Control c in con.Controls) {
				if (c.Enabled) {
					m_Enabled.Add(c);
					c.Enabled = false;
				}
				DisableControls(c);
			}
		}

		/// <summary>
		/// Read a music file, display the AlbumDetails dialog to get details, display.
		/// Runs in a separate thread.
		/// </summary>
		/// <param name="next">Action to do after reading</param>
		private void readFile(Action next) {
			runTask(delegate() {
				// This is an undoable action - save all data on undo stack first
				StackUndo(new UndoEdit(), true, delegate() {
					loadFile(m_Filename);	// Load file, converting to wav if required
					Despatch(delegate() {
						// Show AlbumDetails dialog (in UI thread)
						if (m_AlbumDetails.ShowDialog() == System.Windows.Forms.DialogResult.OK)
							Program.Album = new AlbumInfo(m_AlbumDetails.Album);
					});
					if (Program.Album == null) {
						// No track info - make a single track for the whole album
						Program.Album = new AlbumInfo();
						Program.Album.Tracks.Add(new Track() {
							LengthSeconds = m_Reader.WaveFormat.BytesToSeconds(m_Reader.Length)
						});
					}
					Program.Album.Filename = m_Filename;
					bool split = false;
					if (Program.Album.Tracks.Count <= 1 || Program.Album.Tracks[0].LengthSeconds == 0) {
						Despatch(delegate() {
							// No track length info - go straight to SplitAlbum (in UI thread)
							SplitAlbum a = new SplitAlbum(m_Reader);
							if (a.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
								Program.Album = new AlbumInfo(Program.Album);
								Program.Album.Tracks = a.Tracks;
								split = true;
							}
						});
					}
					if (!split)
						autoGap();	// Only adjust track starts if SplitAlbum has not already done so
					Despatch(BuildTrackJoins);	// Create TrackJoin controls for tracks (and gap at the end)
					Status(m_Filename);
					if (next != null)
						Despatch(next);
				});
			});
		}

		/// <summary>
		/// Adjust track starts to coincide with actual start of music.
		/// Can call from any thread.
		/// </summary>
		private void autoGap() {
			Status("Checking track gaps");
			GapFinder f = new GapFinder(m_Reader);
			float end = 0;	// Silence at end of previous track (or start of recording, at first)
			foreach (Track t in Program.Album.Tracks) {
				float start;
				if (f.FindMusic(end, 60)) {
					// Set track start to start of music
					start = f.Position;
					// Set track gap to length of silence
					t.Gap = start - end;
				} else {
					// No music found in first 60 secs - just default to gap provided
					start = end + t.Gap;
				}
				end = start + t.LengthSeconds;	// Where track end should be, according to Program.Album.Tracks
				// Is there silence shortly after the end of the track?
				if(f.FindSilence(end, 60)
					// If not, go back 30 seconds (or less, subject to MinTrackLength), and look again
					|| f.FindSilence(Math.Max(start + Properties.Settings.Default.MinTrackLength, end - 30), 30)) {
					// Found silence - set correct track end, and length
					end = f.Position;
					t.LengthSeconds = end - start;
				}
			}
		}

		/// <summary>
		/// Load music from filename. If not a wav file, convert to wav (so we can seek accurately).
		/// Can call from any thread.
		/// </summary>
		void loadFile(string filename) {
			Status("Loading {0}", filename);
			AudioFileReader r = new AudioFileReader(filename);
			if (Path.GetExtension(filename).ToLower() != ".wav") {
				string wavfilename = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(filename) + ".wav");
				Status("Transcoding to {0}", wavfilename);
				WaveFileWriter.CreateWaveFile(wavfilename, r);
				r.Dispose();
				r = new AudioFileReader(wavfilename);
				Status("Loading {0}", filename);
			}
			if (m_Reader != null)
				m_Reader.Dispose();
			m_Reader = r;
		}

		/// <summary>
		/// If Program.Album.Filename has changed, load the new file
		/// </summary>
		public void ReloadFile() {
			if (Program.Album == null || Program.Album.Filename == null) {
				// No file - dispose of reader
				if (m_Reader != null)
					m_Reader.Dispose();
				m_Reader = null;
				// And remove track display
				trackPanel.Controls.Clear();
				return;
			}
			if(Program.Album.Filename == m_Filename)
				return;
			loadFile(Program.Album.Filename);
			if (Program.Album.Volume != 0)
				m_Reader.Volume = Program.Album.Volume;
			m_Filename = Program.Album.Filename;
			Status(m_Filename);
		}

		/// <summary>
		/// Build TrackJoin controls to display joins between (and after) tracks.
		/// Must be called from UI thread.
		/// </summary>
		public void BuildTrackJoins() {
			Status("Creating display");
			m_Tracks = new List<TrackJoin>();
			if (Program.Album != null) {
				TrackJoin prev = null;	// TrackJoins need to be linked to previous one (to calculate length, etc.)
				foreach (Track t in Program.Album.Tracks) {
					TrackJoin j = new TrackJoin(prev, t) {
						Dock = DockStyle.Top,
						Name = "Track" + m_Tracks.Count,
					};
					j.waveControl.WaveFile = m_Reader;
					m_Tracks.Add(j);
					// Attach event handlers to tidy up after edits
					j.StartChanged += StartChanged;
					j.GapChanged += GapChanged;
					j.ChangesFinished += ChangesFinished;
					prev = j;
				}
				// Now one for the end of the album
				TrackJoin end = new TrackJoin(prev, null) {
					ShowStart = false,	// No start of next track
					Dock = DockStyle.Top,
					Name = "End"
				};
				end.waveControl.WaveFile = m_Reader;
				m_Tracks.Add(end);
			}
			// Add to panel - reverse causes dock to put them in the right order
			m_Tracks.Reverse();
			trackPanel.Controls.Clear();
			trackPanel.Controls.AddRange(m_Tracks.ToArray());
			m_Tracks.Reverse();
		}

		/// <summary>
		/// Update existing track join display when track lengths and/or gaps have changed
		/// </summary>
		public void UpdateTrackJoins() {
			if (m_Tracks != null) {
				for (int i = 0; i < m_Tracks.Count; i++) {
					m_Tracks[i].UpdateFromTrack();
				}
			}
		}

		/// <summary>
		/// User has change the start of a track, and lock is on, so need to update all subsequent ones
		/// </summary>
		void StartChanged(object sender, TrackJoin.CascadingEvent e) {
			// Keep following track lengths the same
			int i = m_Tracks.IndexOf(e.Track) + 1;
			if (i > 0) {
				while (i < m_Tracks.Count) {
					TrackJoin j = m_Tracks[i++];
					j.End += e.Change;
					j.Start += e.Change;
				}
			}
		}

		/// <summary>
		/// User has change a track gap, and has pressed Ctrl to alter all the others, so need to update all subsequent ones
		/// </summary>
		void GapChanged(object sender, TrackJoin.CascadingEvent e) {
			// Keep following track lengths same, but change all the gaps
			int i = m_Tracks.IndexOf(e.Track) + 1;
			float change = 0;
			if (i > 0) {
				while (i < m_Tracks.Count) {
					TrackJoin j = m_Tracks[i++];
					j.End += change;
					change += e.Change;
					j.Start += change;
				}
			}
		}

		/// <summary>
		/// An edit has finished - adjust following tracks so there is 5 sec before and after start & end
		/// </summary>
		void ChangesFinished(object sender, TrackJoin e) {
			// Undoable action - save current track gaps and lengths on stack
			StackUndo(new UndoMove(), false, delegate() {
				int i = m_Tracks.IndexOf(e) + 1;
				if (i > 0) {
					while (i < m_Tracks.Count) {
						TrackJoin j = m_Tracks[i++];
						j.Adjust(5);
					}
				}
			});
		}

		/// <summary>
		/// Stack an action onto the undo stack.
		/// </summary>
		/// <param name="a">The UndoAction which will reverse this action</param>
		/// <param name="disable">Disable all controls while performing the action</param>
		/// <param name="change">The action which will make the changes</param>
		public void StackUndo(UndoAction a, bool disable, Action change) {
			try {
				if (disable)
					disable = Enable(false);
				UndoStack.Push(a);
				RedoStack.Clear();
				change();	// Make the change
				Despatch(delegate() {
					btnUndo.Enabled = true;
					btnRedo.Enabled = false;
				});
			} finally {
				// Record current state (for next UndoAction)
				Program.CurrentState = new AlbumInfo(Program.Album);
				if (disable)
					Enable(true);
			}
		}

		/// <summary>
		/// The volume setting on the wave file
		/// </summary>
		public float Volume {
			get {
				return m_Reader == null ? 0 : m_Reader.Volume;
			}
			set {
				if (m_Reader != null)
					m_Reader.Volume = value;
				if (Program.Album != null)
					Program.Album.Volume = value;
			}
		}

		/// <summary>
		/// Split a string at commas - if string is whitespace, use deflt instead
		/// </summary>
		string[] split(string source, string deflt) {
			if (string.IsNullOrWhiteSpace(source))
				source = deflt;
			return source.Split(',');
		}

		/// <summary>
		/// Save an album to the music folder
		/// </summary>
		void saveAlbum() {
			try {
				Enable(false);
				// Need Media Foundation Api for encoding
				NAudio.MediaFoundation.MediaFoundationApi.Startup();
				// Folder for artist
				string folder = Path.Combine(Properties.Settings.Default.MusicFolder, filename(Program.Album.Artist));
				Directory.CreateDirectory(folder);
				// Folder under that for album
				folder = Path.Combine(folder, filename(Program.Album.Title));
				Directory.CreateDirectory(folder);
				float start = 0;	// Where track starts
				// Artist names separated by commas, not &
				string artist = Program.Album.Artist.Replace(" & ", ",").Replace(" and ", ",");
				for (int t = 0; t < Program.Album.Tracks.Count; t++) {
					Track trk = Program.Album.Tracks[t];
					start += trk.Gap;
					string name = trk.Title;
					// Filename for track
					string dest = Path.Combine(folder, string.Format("{0:00} - {1}", t + 1, filename(name))) + Properties.Settings.Default.OutputType;
					Status("Saving {0} {1}", t + 1, name);
					ExtractWaveProvider p = new ExtractWaveProvider(m_Reader, start, trk.LengthSeconds);
					start += trk.LengthSeconds;
					switch (Properties.Settings.Default.OutputType) {
						case ".wma":
							MediaFoundationEncoder.EncodeToWma(p, dest);
							break;
						case ".mp3":
							MediaFoundationEncoder.EncodeToMp3(p, dest);
							break;
					}
					// Album art picture, in TagLib format, to add to all tracks
					Picture pic = null;
					if (Program.Album.Art != null) {
						// Also save as AlbumArtLarge.jpg
						string path = Path.Combine(folder, "AlbumArtLarge.jpg");
						Program.Album.Art.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
						pic = new Picture(path);
						pic.Type = PictureType.FrontCover;
					}
					using (TagLib.File d = TagLib.File.Create(dest)) {
						d.Tag.Album = Program.Album.Title;
						d.Tag.AlbumArtists = split(artist, "");
						d.Tag.Performers = split(trk.Artist, artist);
						d.Tag.Genres = split(Program.Album.Genre, "");
						d.Tag.Title = name;
						d.Tag.Track = (uint)t + 1;
						d.Tag.Year = Program.Album.Year;
						// Must do publisher, or Media Player will screw it up
						// but TagLib-sharp does not support it directly yet
						switch (Properties.Settings.Default.OutputType) {
							case ".wma":
								(d.Tag as TagLib.Asf.Tag).SetDescriptorString(Program.Album.Publisher, "WM/Publisher");
								break;
							case ".mp3":
								(d.Tag as TagLib.Id3v2.Tag).SetTextFrame("TPUB", Program.Album.Publisher);
								break;
						}
						if (pic != null)
							d.Tag.Pictures = new Picture [] { pic};
						d.Save();
					}
				}
				Status("Save complete");
			} finally {
				NAudio.MediaFoundation.MediaFoundationApi.Shutdown();
				Enable(true);
			}
		}

		/// <summary>
		/// Convert any string into a legal filename
		/// </summary>
		string filename(string source) {
			return System.Text.RegularExpressions.Regex.Replace(source, "[^A-Za-z0-9 _!%&()-]", "").Trim();
		}

		/// <summary>
		/// Show the TrackDetails dialog, and update the display if the user pressed OK
		/// </summary>
		/// <returns>True if the user pressed OK</returns>
		bool amendTrackDetails() {
			m_TrackDetails.SetDetails();
			if (m_TrackDetails.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return false;
			// Undoable - save all data on stack first
			StackUndo(new UndoEdit(), true, delegate() {
				m_TrackDetails.GetDetails();
				BuildTrackJoins();
			});
			return true;
		}

		/// <summary>
		/// Do an action in another thread, catching any exceptions and reporting them
		/// </summary>
		void runTask(Action a) {
			new Task(delegate() {
				try {
					a();
				} catch (Exception ex) {
					Program.Trace(ex.ToString());
					Status(ex.Message);
				}
			}).Start();
		}

		/// <summary>
		/// Open tool button pressed
		/// </summary>
		private void btnOpen_Click(object sender, EventArgs e) {
			openFileDialog1.InitialDirectory = openFileDialog1.FileName == "" ? Properties.Settings.Default.RecordingFolder : Path.GetDirectoryName(openFileDialog1.FileName);
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				try {
					if (openFileDialog1.FileName.ToLower().EndsWith(".albumrecorder")) {
						// This is a project - reload, then load the music file
						runTask(delegate() {
							// Undoable action - save everything first
							StackUndo(new UndoEdit(), true, delegate() {
								using (TextReader r = new StreamReader(openFileDialog1.FileName)) {
									Program.Album = (AlbumInfo)new XmlSerializer(typeof(AlbumInfo)).Deserialize(r);
								}
								Program.Album.Filename = openFileDialog1.FileName.Substring(0, openFileDialog1.FileName.Length - 14);
								ReloadFile();
								Despatch(BuildTrackJoins);
							});
						});
					} else {
						// It's just a music file
						m_Filename = openFileDialog1.FileName;
						readFile(null);	// Loads file in a separate thread
					}
				} catch (Exception ex) {
					Program.Trace(ex.ToString());
					Status(ex.Message);
				}
			}
		}

		/// <summary>
		/// Album Details button clicked
		/// </summary>
		private void btnAlbum_Click(object sender, EventArgs e) {
			if (m_AlbumDetails.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				// Undoable - save all data first
				StackUndo(new UndoEdit(), false, delegate() {
					string filename = Program.Album == null ? null : Program.Album.Filename;
					// Copy selected album details to Program.Album
					Program.Album = new AlbumInfo(m_AlbumDetails.Album);
					Program.Album.Filename = filename;
				});
				BuildTrackJoins();	// And display result
			}
		}

		/// <summary>
		/// Save button clicked
		/// </summary>
		private void btnSave_Click(object sender, EventArgs e) {
			try {
				// Display track details dialog first, so they can make sure it is all filled in
				if (amendTrackDetails()) {
					btnSaveProject_Click(sender, e);	// Save the project file
					runTask(saveAlbum);					// And the album
				}
			} catch (Exception ex) {
				System.Diagnostics.Trace.WriteLine(ex);
				Status(ex.Message);
			}
		}

		/// <summary>
		/// Track Details button clicked
		/// </summary>
		private void btnTrackInfo_Click(object sender, EventArgs e) {
			amendTrackDetails();
		}

		/// <summary>
		/// Lock button clicked
		/// </summary>
		private void btnLock_Click(object sender, EventArgs e) {
			Program.Locked = btnLock.Checked;
			btnLock.Image = imageList1.Images[btnLock.Checked ? 0 : 1];
			btnLock.Text = btnLock.ToolTipText = btnLock.Checked ? "Unlock track lengths" : "Lock track lengths";
		}

		/// <summary>
		/// Normalize button clicked
		/// </summary>
		private void btnNormalize_Click(object sender, EventArgs e) {
			if (m_Reader == null)
				return;
			// Show dialog
			Normalize n = new Normalize(m_Reader);
			if (n.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				// Adjust volume - undoable
				StackUndo(new UndoVolume(), false, delegate() {
					Status("New volume level {0:##0}%", 100 * n.Volume);
					Volume = n.Volume;
				});
			}
		}

		/// <summary>
		/// Record button clicked
		/// </summary>
		private void btnRecord_Click(object sender, EventArgs e) {
			// Show dialog
			Recording r = new Recording();
			if (r.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				FileInfo f = new FileInfo(r.Filename);
				if (f.Exists && f.Length > 0) {
					// Have recorded something - load it in
					m_Filename = r.Filename;
					readFile(delegate() {
						// Then offer to normalize it
						btnNormalize_Click(sender, e);
					});
				}
			}
		}

		/// <summary>
		/// Undo button clicked
		/// </summary>
		private void btnUndo_Click(object sender, EventArgs e) {
			if (UndoStack.Count > 0) {
				// Undo returns an action to redo itself
				UndoAction r = UndoStack.Pop().Undo(this);
				RedoStack.Push(r);
				btnUndo.Enabled = UndoStack.Count > 0;
				btnRedo.Enabled = true;
			}
		}

		/// <summary>
		/// Redo button clicked
		/// </summary>
		private void btnRedo_Click(object sender, EventArgs e) {
			if (RedoStack.Count > 0) {
				// Undo returns an action to redo itself
				UndoAction r = RedoStack.Pop().Undo(this);
				UndoStack.Push(r);
				btnRedo.Enabled = RedoStack.Count > 0;
				btnUndo.Enabled = true;
			}

		}

		/// <summary>
		/// Save project button clicked
		/// </summary>
		private void btnSaveProject_Click(object sender, EventArgs e) {
			if (m_Filename == null)
				return;
			try {
				// Save AlbumInfo as an XML file
				using (TextWriter w = new StreamWriter(m_Filename + ".AlbumRecorder.tmp", false)) {
					new XmlSerializer(typeof(AlbumInfo)).Serialize(w, Program.Album);
				}
				try {
					System.IO.File.Delete(m_Filename + ".AlbumRecorder");
				} catch {
				}
				System.IO.File.Move(m_Filename + ".AlbumRecorder.tmp", m_Filename + ".AlbumRecorder");
			} catch (Exception ex) {
				System.Diagnostics.Trace.WriteLine(ex);
				Status(ex.Message);
			}

		}

		/// <summary>
		/// Split tracks button clicked
		/// </summary>
		private void btnSplit_Click(object sender, EventArgs e) {
			if (m_Reader == null)
				return;
			// Show dialog
			SplitAlbum a = new SplitAlbum(m_Reader);
			if (a.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				// Undoable - save everything
				StackUndo(new UndoEdit(), true, delegate() {
					// Copy data
					Program.Album = new AlbumInfo(Program.Album);
					// And use new track info
					Program.Album.Tracks = a.Tracks;
					BuildTrackJoins();
				});
			}
		}

		/// <summary>
		/// Options/Settings button clicked
		/// </summary>
		private void btnOptions_Click(object sender, EventArgs e) {
			new Options().ShowDialog();
		}

		/// <summary>
		/// Dispose of m_Reader on form close
		/// </summary>
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (m_Reader != null) {
				m_Reader.Dispose();
				m_Reader = null;
			}
		}
	}

}
