using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AlbumRecorder {
	/// <summary>
	/// Alow user to alter default settings
	/// </summary>
	public partial class Options : Form {
		public Options() {
			InitializeComponent();
			txtMusicFolder.Text = Properties.Settings.Default.MusicFolder;
			txtRecordingsFolder.Text = Properties.Settings.Default.RecordingFolder;
			txtCentre.Text = Properties.Settings.Default.SilenceFilterCentre.ToString();
			txtQ.Text = Properties.Settings.Default.SilenceFilterQ.ToString();
			txtStartSilence.Text = Properties.Settings.Default.StartSilenceThreshold.ToString();
			txtEndSilence.Text = Properties.Settings.Default.EndSilenceThreshold.ToString();
		}


		/// <summary>
		/// Check folder exists, throw if it doesn't
		/// </summary>
		/// <param name="desc">For errors</param>
		private void checkFolderExists(string folder, string desc) {
			if (!Directory.Exists(folder))
				throw new ApplicationException(desc + " folder '" + folder + "' does not exist");
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}

		private void btnOK_Click(object sender, EventArgs e) {
			try {
				checkFolderExists(txtMusicFolder.Text, "Music");
				Properties.Settings.Default.MusicFolder = txtMusicFolder.Text;
				checkFolderExists(txtRecordingsFolder.Text, "Recordings");
				Properties.Settings.Default.RecordingFolder = txtRecordingsFolder.Text;
				Properties.Settings.Default.SilenceFilterCentre = txtCentre.Text.ToFloat("Centre Frequency");
				Properties.Settings.Default.SilenceFilterQ = txtQ.Text.ToFloat("Filter width (Q)");
				Properties.Settings.Default.StartSilenceThreshold = txtStartSilence.Text.ToFloat("Start silence threshold");
				Properties.Settings.Default.EndSilenceThreshold = txtEndSilence.Text.ToFloat("Start Music threshold");
				Properties.Settings.Default.Save();
				DialogResult = System.Windows.Forms.DialogResult.OK;
				Close();
			} catch (Exception ex) {
				lblStatus.Text = ex.Message;
				Properties.Settings.Default.Reload();
			}
		}

		private void btnBrowseMusicFolder_Click(object sender, EventArgs e) {
			folderBrowserDialog1.SelectedPath = txtMusicFolder.Text;
			folderBrowserDialog1.Description = "Select Music Folder";
			if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				txtMusicFolder.Text = folderBrowserDialog1.SelectedPath;
		}

		private void btnBrowseRecordings_Click(object sender, EventArgs e) {
			folderBrowserDialog1.SelectedPath = txtRecordingsFolder.Text;
			folderBrowserDialog1.Description = "Select Recordings Folder";
			if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				txtRecordingsFolder.Text = folderBrowserDialog1.SelectedPath;
		}
	}
}
