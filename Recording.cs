using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.CoreAudioApi;

namespace AlbumRecorder {
	/// <summary>
	/// Record incoming wav
	/// </summary>
	public partial class Recording : Form {
		MMDevice m_RecordingDevice;
		IWaveIn waveIn;
		WaveFileWriter writer;
		bool recording;		// True if recording

		public Recording() {
			InitializeComponent();
		}

		/// <summary>
		/// File we are recording into
		/// </summary>
		public string Filename {
			get { return txtFile.Text; }
		}

		/// <summary>
		/// Set/change recording device
		/// </summary>
		void switchRecordingDevice() {
			m_RecordingDevice = (MMDevice)cbSource.Items[cbSource.SelectedIndex];
			// Set volume slider to correct value
			tbVolume.Value = (int)(100 * m_RecordingDevice.AudioEndpointVolume.MasterVolumeLevelScalar);
		}

		/// <summary>
		/// User has changed recording device
		/// </summary>
		private void cbSource_SelectedIndexChanged(object sender, EventArgs e) {
			switchRecordingDevice();
		}

		/// <summary>
		/// Browse recording file name
		/// </summary>
		private void btnBrowse_Click(object sender, EventArgs e) {
			saveFileDialog1.FileName = txtFile.Text;
			saveFileDialog1.InitialDirectory = saveFileDialog1.FileName == "" ? Properties.Settings.Default.RecordingFolder : Path.GetDirectoryName(saveFileDialog1.FileName);
			if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				txtFile.Text = saveFileDialog1.FileName;
			if (Path.GetDirectoryName(saveFileDialog1.FileName) != Properties.Settings.Default.RecordingFolder) {
				// Default setttings keeps track of recording folder
				Properties.Settings.Default.RecordingFolder = Path.GetDirectoryName(saveFileDialog1.FileName);
				Properties.Settings.Default.Save();
			}
		}

		/// <summary>
		/// Start recording
		/// </summary>
		private void btnStart_Click(object sender, EventArgs e) {
			if (waveIn == null) {
				// Start new recording
				waveIn = new WaveIn();
				waveIn.WaveFormat = new WaveFormat();
				waveIn.DataAvailable += OnDataAvailable;
				writer = new WaveFileWriter(txtFile.Text, waveIn.WaveFormat);
				waveControl1.CreateBuffer(waveIn.WaveFormat);
			}
			recording = true;
			waveIn.StartRecording();
			// Disable controls
			cbSource.Enabled = false;
			txtFile.Enabled = false;
			btnBrowse.Enabled = false;
			btnStart.Enabled = false;
			// Enable pause & stop
			btnPause.Enabled = true;
			btnStop.Enabled = true;
		}

		/// <summary>
		/// Have got some incoming data
		/// </summary>
		void OnDataAvailable(object sender, WaveInEventArgs e) {
			if (this.InvokeRequired) {
				this.BeginInvoke(new EventHandler<WaveInEventArgs>(OnDataAvailable), sender, e);
			} else {
				// Save to disk
				writer.Write(e.Buffer, 0, e.BytesRecorded);
				// And display on screen
				waveControl1.AddData(e.Buffer, e.BytesRecorded);
			}
		}

		private void btnPause_Click(object sender, EventArgs e) {
			if (recording) {
				recording = false;
				waveIn.StopRecording();
			} else if (waveIn != null) {
				recording = true;
				waveIn.StartRecording();
			}
		}

		private void btnStop_Click(object sender, EventArgs e) {
			recording = false;
			if (waveIn != null) {
				waveIn.StopRecording();
				waveIn.Dispose();
				waveIn = null;
			}
			if (writer != null) {
				writer.Dispose();
				writer = null;
			}
			cbSource.Enabled = true;
			txtFile.Enabled = true;
			btnBrowse.Enabled = true;
			btnStart.Enabled = true;
			btnPause.Enabled = false;
			btnStop.Enabled = false;
			// Close after recording stopped
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		/// <summary>
		/// Initialise
		/// </summary>
		private void Recording_Load(object sender, EventArgs e) {
			// Fill device dropdown
			var deviceEnum = new MMDeviceEnumerator();
			foreach (var device in deviceEnum.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)) {
				int d = cbSource.Items.Add(device);
				if (m_RecordingDevice == null || device.FriendlyName == Properties.Settings.Default.RecordingDevice) {
					cbSource.SelectedIndex = d;
					switchRecordingDevice();
				}
			}
			// Set default file name
			txtFile.Text = Path.Combine(Properties.Settings.Default.RecordingFolder, string.Format("Recording{0:yyyy-MM-dd-hh-mm}.wav", DateTime.Now));
			// Prepare wave control
			waveControl1.LengthSeconds = 20;
		}

		private void tbVolume_Scroll(object sender, EventArgs e) {
			// Alter recording volume
			m_RecordingDevice.AudioEndpointVolume.MasterVolumeLevelScalar = tbVolume.Value / 100f;
		}

	}
}
