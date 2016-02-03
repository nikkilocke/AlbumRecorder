using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hqub.MusicBrainz.API.Entities;
using NAudio.Wave;

namespace AlbumRecorder {
	public partial class Form1 : Form {
		public static Form1 Instance;
		AlbumDetails Details;
		string _filename;
		AudioFileReader _reader;
		

		public Form1() {
			Instance = this;
			Details = new AlbumDetails();
			InitializeComponent();
		}

		public static void Despatch(Action a) {
			if (Instance.InvokeRequired) {
				Instance.Invoke(a);
			} else
				a();
		}

		public static void Status(string text) {
			Despatch(delegate() { Instance.lblStatus.Text = text; });
		}

		public static void Status(string format, params object[] args) {
			Status(string.Format(format, args));
		}

		private void readFile() {
			Status("Loading {0}", _filename);
			waveControl1.WaveFile = null;
			if (_reader != null)
				_reader.Dispose();
			_reader = new AudioFileReader(_filename);
			Details.ShowDialog();
			waveControl1.StartSeconds = float.Parse(txtStart.Text);
			waveControl1.LengthSeconds = float.Parse(txtLength.Text);
			waveControl1.Cursors = new WaveControl.PositionCursor[] { 
				new WaveControl.PositionCursor() { 
					Position = waveControl1.StartSeconds + waveControl1.LengthSeconds / 2,
					Label = "Start"
				} 
			};
			waveControl1.WaveFile = _reader;
			Status(_filename);
		}

		private void btnOpen_Click(object sender, EventArgs e) {
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				_filename = openFileDialog1.FileName;
				readFile();
			}
		}

		private void Form1_Load(object sender, EventArgs e) {
		}

		private void btnAlbum_Click(object sender, EventArgs e) {
			Details.ShowDialog();
		}

		private void btnGo_Click(object sender, EventArgs e) {
			waveControl1.StartSeconds = float.Parse(txtStart.Text);
			waveControl1.LengthSeconds = float.Parse(txtLength.Text);
		}
	}

}
