using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.IO;
using Hqub.MusicBrainz.API.Entities;
using ParkSquare.Gracenote;

namespace AlbumRecorder {
	/// <summary>
	/// Edit track and album details
	/// </summary>
	public partial class TrackDetails : Form {
		/// <summary>
		/// List of tracks in DataBinding form
		/// </summary>
		BindingList<Track> m_Tracks;
		/// <summary>
		/// Background task to download cover image
		/// </summary>
		TaskRunner m_Task;
		
		public TrackDetails() {
			InitializeComponent();
			m_Task = new TaskRunner(TaskException);
			// Set output format from settings
			for (int i = 0; i < cbFormat.Items.Count; i++) {
				if (cbFormat.Items[i].ToString() == Properties.Settings.Default.OutputType)
					cbFormat.SelectedIndex = i;
			}
		}

		/// <summary>
		/// Set up dialog with album information
		/// </summary>
		public void SetDetails() {
			if (Program.Album == null)
				return;
			txtArtist.Text = Program.Album.Artist;
			txtTitle.Text = Program.Album.Title;
			txtYear.Text = Program.Album.Year.ToString();
			txtGenre.Text = Program.Album.Genre;
			txtPublisher.Text = Program.Album.Publisher;
			setImage(Program.Album.Art);
			// Copy track information
			m_Tracks = new BindingList<Track>(Program.Album.Tracks.Select(t => new Track(t)).ToList());
			dataGridView1.DataSource = m_Tracks;
			if (Program.Album.Art == null)
				downloadAlbumArt();		// Start art download
		}

		/// <summary>
		/// Copy dialog data back to album
		/// </summary>
		public void GetDetails() {
			Program.Album = new AlbumInfo(Program.Album);
			Program.Album.Artist = txtArtist.Text;
			Program.Album.Title = txtTitle.Text;
			Program.Album.Year = uint.Parse(txtYear.Text);
			Program.Album.Genre = txtGenre.Text;
			Program.Album.Publisher = txtPublisher.Text;
			Program.Album.Tracks = new List<Track>(m_Tracks);
			Program.Album.Art = pictureBox1.Image;
		}

		/// <summary>
		/// Catch exceptions in the task
		/// </summary>
		private void TaskException(object sender, Exception ex) {
			System.Diagnostics.Trace.WriteLine(ex);
			Status(ex.Message);
		}

		/// <summary>
		/// Set Status label (can call from any thread)
		/// </summary>
		void Status(string text) {
			Despatch(delegate() { lblStatus.Text = text; });
		}

		/// <summary>
		/// Set Status label (can call from any thread)
		/// </summary>
		void Status(string format, params object[] args) {
			Status(string.Format(format, args));
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
		/// Start search and download of album art
		/// </summary>
		void downloadAlbumArt() {
			search(txtTitle.Text, txtArtist.Text);
		}

		/// <summary>
		/// Currently selected row of tracks data grid
		/// </summary>
		int selectedRow {
			get {
				return dataGridView1.CurrentCellAddress.Y;
			}
			set {
				dataGridView1.CurrentCell = dataGridView1.Rows[value].Cells[dataGridView1.CurrentCellAddress.X];
			}
		}

		/// <summary>
		/// Search for and download album art in a separate thread
		/// </summary>
		void search(string title, string artist) {
			if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(artist))
				return;		// Not enough information, so give up
			m_Task.Run(delegate(TaskRunner.Task task) {
				if (!string.IsNullOrEmpty(Program.Album.ReleaseId)) {
					// MusicBrainz data - try there first
					Uri art = CoverArtArchive.GetCoverArtUri(Program.Album.ReleaseId);
					if (task.Stop)
						return;
					if (art != null) {
						Status("Downloading cover art from MusicBrainz");
						if(downloadImage(art, task))
							return;
					}
				}
				if (task.Stop)
					return;
				// Not found - try Gracenote
				Status("Searching Gracenote database");
				GracenoteClient Client = new GracenoteClient(Program.GracenoteKey);
				if (task.Stop)
					return;
				SearchResult result = Client.Search(new SearchCriteria() {
					AlbumTitle = title,
					Artist = artist,
					SearchMode = SearchMode.BestMatchWithCoverArt
				});
				var a = result.Albums.FirstOrDefault();
				if (a != null) {
					var i = a.Artwork.FirstOrDefault();
					if (task.Stop)
						return;
					if (i != null) {
						string filename = System.IO.Path.GetTempFileName().Replace(".tmp", ".jpg");
						Status("Downloading album cover");
						i.Download(filename);
						using (Image img = Image.FromFile(filename))
							setImage(img);
						File.Delete(filename);
						Status("");
						return;
					}
				}
				Status("Album not found - drag an image from the web");
				if (task.Stop)
					return;
				System.Diagnostics.Process.Start("http://www.google.co.uk/search?q=" + HttpUtility.UrlEncode("\"" + artist + "\" \"" + title + "\" album cover"));
			});
		}

		/// <summary>
		/// Download an album art image from the web
		/// </summary>
		/// <returns>True if successful</returns>
		bool downloadImage(Uri art, TaskRunner.Task task) {
			try {
				using (WebClient webClient = new WebClient()) {
					byte[] data = webClient.DownloadData(art);
					if (task.Stop)
						return true;
					using (MemoryStream mem = new MemoryStream(data)) {
						using (Image img = Image.FromStream(mem))
							if(!task.Stop)
								setImage(img);
					}
				}
				return true;
			} catch (Exception ex) {
				System.Diagnostics.Trace.WriteLine(ex);
				Status("{1} at {0}", art, ex.Message);
			}
			return false;
		}

		/// <summary>
		/// Put an image in the PictureBox, sized to 200 x 198
		/// </summary>
		void setImage(Image img) {
			Despatch(delegate() {
				pictureBox1.Image = img == null ? null : img.GetThumbnailImage(200, 198, delegate() { return false; }, new IntPtr());
			});
		}

		private void btnOK_Click(object sender, EventArgs e) {
			// Save output format for next time
			Properties.Settings.Default.OutputType = cbFormat.Text;
			Properties.Settings.Default.Save();
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		/// <summary>
		/// Delete a track
		/// </summary>
		private void btnDelete_Click(object sender, EventArgs e) {
			int s = selectedRow;
			if (s >= 0)
				m_Tracks.RemoveAt(s);
		}

		/// <summary>
		/// Move track up
		/// </summary>
		private void btnUp_Click(object sender, EventArgs e) {
			int s = selectedRow;
			if (s > 0) {
				Track t = m_Tracks[s];
				m_Tracks.RemoveAt(s);
				m_Tracks.Insert(s - 1, t);
				selectedRow = s - 1;
			}
		}

		/// <summary>
		/// Move track down
		/// </summary>
		private void btnDown_Click(object sender, EventArgs e) {
			int s = selectedRow;
			if (s >= 0 && s < m_Tracks.Count - 1) {
				Track t = m_Tracks[s];
				m_Tracks.RemoveAt(s);
				m_Tracks.Insert(s + 1, t);
				selectedRow = s + 1;
			}
		}

		/// <summary>
		/// Click on picture box starts art download
		/// </summary>
		private void pictureBox1_Click(object sender, EventArgs e) {
			if (pictureBox1.Image == null)
				downloadAlbumArt();
		}

		/// <summary>
		/// Types of dragged object we can accept
		/// </summary>
		enum DropType {
			Invalid,
			Url,
			File
		};

		/// <summary>
		/// Find out the type of object being dragged
		/// </summary>
		/// <param name="result">Dropped item as text</param>
		DropType checkDrag(DragEventArgs e, out string result) {
			if (e.Data.GetDataPresent(DataFormats.Text)) {
				result = (string)e.Data.GetData(DataFormats.Text);
				if (result.StartsWith("http://") || result.StartsWith("https://")) {
					return DropType.Url;
				}
			} else if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (files.Length == 1) {
					result = files[0];
					switch (Path.GetExtension(result).ToLower()) {
						case ".png":
						case ".jpg":
						case ".gif":
						case ".bmp":
							if (File.Exists(result))
								return DropType.File;
							break;
					}
				}
			}
			result = null;
			return DropType.Invalid;
		}

		/// <summary>
		/// Dropped image onto form
		/// </summary>
		private void TrackDetails_DragDrop(object sender, DragEventArgs e) {
			string dropped;
			switch (checkDrag(e, out dropped)) {
				case DropType.Url:
					// Download it
					m_Task.Run(delegate(TaskRunner.Task task) {
						downloadImage(new Uri(dropped), task);
					});
					break;
				case DropType.File:
					// Open it
					try {
						using (Image img = Image.FromFile(dropped)) {
							setImage(img.GetThumbnailImage(200, 198, delegate() { return false; }, new IntPtr()));
						}
					} catch (Exception ex) {
						Status(ex.Message);
					}
					break;
			}
		}

		/// <summary>
		/// Dragged something onto form
		/// </summary>
		private void TrackDetails_DragEnter(object sender, DragEventArgs e) {
			// Program.Trace(string.Join("\r\n", e.Data.GetFormats(true)));
			string dropped;
			if(checkDrag(e, out dropped) != DropType.Invalid)
				e.Effect = DragDropEffects.Copy;
		}

		/// <summary>
		/// Stop any download task on exit
		/// </summary>
		private void TrackDetails_FormClosing(object sender, FormClosingEventArgs e) {
			m_Task.Stop();
		}
	}
}
