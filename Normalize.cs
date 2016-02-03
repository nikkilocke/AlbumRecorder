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
	/// Normalize volume
	/// </summary>
	public partial class Normalize : Form {
		private AudioFileReader m_Reader;
		TaskRunner m_Task;
	
		public Normalize(AudioFileReader reader) {
			m_Reader = reader;
			Volume = 1;
			m_Task = new TaskRunner();
			InitializeComponent();
		}

		/// <summary>
		/// Normalize (runs in separate thread)
		/// </summary>
		void normalize(TaskRunner.Task t) {
			float oldVolume = m_Reader.Volume;
			try {
				Volume = 1;
				float max = 0;	// Max volume in file
				float[] buffer = new float[m_Reader.WaveFormat.BlockAlign];
				// Number of buffers to read
				long total = m_Reader.Length / (m_Reader.WaveFormat.BlockAlign * m_Reader.WaveFormat.BitsPerSample / 8);
				long count = 0;	// Number read so far
				int pos = 0;	// Progress bar position (0-100)
				m_Reader.Position = 0;
				m_Reader.Volume = 1;
				ProgressBar(0);
				while (!t.Stop && m_Reader.Read(buffer, 0, m_Reader.WaveFormat.BlockAlign) == m_Reader.WaveFormat.BlockAlign) {
					count++;
					// Calculate progress bar position
					int p = (int)(100 * count / total);
					if (p != pos) {
						// Only update if it has changed a whole unit
						pos = p;
						ProgressBar(pos);
					}
					// Find max volume in this buffer
					for (int i = 0; i < m_Reader.WaveFormat.BlockAlign; i++)
						max = Math.Max(max, Math.Abs(buffer[i]));
				}
				if (!t.Stop) {
					Volume = Properties.Settings.Default.NormalizeLevel / max;
					Despatch(delegate() {
						// Have finished - can close dialog
						DialogResult = System.Windows.Forms.DialogResult.OK;
						Close();
					});
				}
			} catch {
			} finally {
				m_Reader.Volume = oldVolume;
			}
			btnOK.Enabled = true;
		}

		/// <summary>
		/// Do an action in the UI thread
		/// </summary>
		void Despatch(Action a) {
			if (InvokeRequired) {
				Invoke(a);
			} else
				a();
		}

		/// <summary>
		/// Set Progress bar value
		/// </summary>
		void ProgressBar(int value) {
			Despatch(delegate() {
				progressBar1.Value = value;
				progressBar1.Refresh();
			});
		}

		/// <summary>
		/// Calculated desirable volume
		/// </summary>
		public float Volume { get; private set; }

		/// <summary>
		/// Set normalize level from settings
		/// </summary>
		private void Normalize_Load(object sender, EventArgs e) {
			txtLevel.Text = Properties.Settings.Default.NormalizeLevel.ToString();
		}

		/// <summary>
		/// Cancel stops any existing task, and exits
		/// </summary>
		private void btnCancel_Click(object sender, EventArgs e) {
			m_Task.Stop();
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}

		/// <summary>
		/// OK starts a normalize run, and only exits if it runs to completion
		/// </summary>
		private void btnOK_Click(object sender, EventArgs e) {
			try {
				Properties.Settings.Default.NormalizeLevel = float.Parse(txtLevel.Text);
				Properties.Settings.Default.Save();
			} catch {
				txtLevel.Text = Properties.Settings.Default.NormalizeLevel.ToString();
			}
			m_Task.Run(normalize);
			btnOK.Enabled = false;
		}
	}
}
