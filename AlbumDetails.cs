using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hqub.MusicBrainz.API.Entities;
using ParkSquare.Gracenote;

namespace AlbumRecorder {
	/// <summary>
	/// Search Internet databases for album details
	/// </summary>
	public partial class AlbumDetails : Form {
		TaskRunner m_Task;

		public AlbumDetails() {
			InitializeComponent();
			m_Task = new TaskRunner(TaskException);
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
		/// Set the status label (can call from any thread)
		/// </summary>
		public void Status(string text) {
			Despatch(delegate() { lblStatus.Text = text; });
		}

		/// <summary>
		/// The currently selected album
		/// </summary>
		public AlbumInfo Album {
			get {
				return Results.SelectedItems.Count > 0 ? (AlbumInfo)Results.SelectedItems[0].Tag : null;
			}
		}

		/// <summary>
		/// Catch exceptions in the task
		/// </summary>
		private void TaskException(object sender, Exception ex) {
			System.Diagnostics.Trace.WriteLine(ex);
			Status(ex.Message);
		}

		/// <summary>
		/// Change Cancel button to Select if an album is selected
		/// </summary>
		private void Results_SelectedIndexChanged(object sender, EventArgs e) {
			btnCancel.Text = Results.SelectedItems.Count > 0 ? "Select" : "Cancel";
		}

		/// <summary>
		/// Cancel/Select button clicked
		/// </summary>
		private void btnCancel_Click(object sender, EventArgs e) {
			AlbumInfo a = Album;
			if (a != null && (Program.Album == null || Program.Album.ReleaseId != a.ReleaseId || Program.Album.GracenoteId != a.GracenoteId)) {
				// A different album is selected
				DialogResult = System.Windows.Forms.DialogResult.OK;
			} else {
				// No change, or nothing selected
				DialogResult = System.Windows.Forms.DialogResult.Cancel;
			}
			Close();
		}

		/// <summary>
		/// Search button clicked
		/// </summary>
		private void btnOK_Click(object sender, EventArgs e) {
			m_Task.Stop();			// Stop any existing search
			Results.Items.Clear();
			string artist = txtArtist.Text;
			string title = txtTitle.Text;
			m_Task.Run(async delegate(TaskRunner.Task t) {
				int count = 0;			// No of items added
				int insertPoint = 0;	// Where to insert close matches
				try {
					// Build query
					List<string> b = new List<string>();
					if (!string.IsNullOrWhiteSpace(artist))
						b.Add("artist:\"" + artist.Replace("\"", "") + "\"");
					if (!string.IsNullOrWhiteSpace(title))
						b.Add("release:\"" + title.Replace("\"", "") + "\"");
					if (b.Count == 0) {
						Status("No artist or title provided");
						return;
					}
					Status("Searching MusicBrainz database");
					// Get list of releases
					foreach (Release r in (await Release.SearchAsync(string.Join(" AND ", b.ToArray()))).Items) {
						if (t.Stop)
							return;
						try {
							// Get recordings for release (i.e. list of Mediums/Media)
							Release rel = await Release.GetAsync(r.Id, "recordings");
							foreach (Medium m in rel.MediumList.Items) {
								if (t.Stop)
									return;
								// Artists
								string art = string.Join(", ", r.Credits.Select(c => c.Artist.Name).ToArray());
								ListViewItem item = new ListViewItem(new string[] { art, r.Title, r.Date, m.Tracks.Items.Count + ":" + string.Join(",", m.Tracks.Items.Select(tr => tr.Recording.Title)) });
								// Make an AlbumInfo
								item.Tag = new AlbumInfo(art, rel, m);
								if (SoundEx.Equals(artist, art) && SoundEx.Equals(title, r.Title)) {
									// Close match - put near top of list
									Despatch(delegate() { Results.Items.Insert(insertPoint, item); });
									insertPoint++;
								} else {
									// Not so close - put at end
									Despatch(delegate() { Results.Items.Add(item); });
								}
								count++;
							}
						} catch (Hqub.MusicBrainz.API.HttpClientException) {
							// Ignore these exceptions - they seem to happen all the time
						}
					}
				} catch (Hqub.MusicBrainz.API.HttpClientException) {
					// Ignore these exceptions - they seem to happen all the time
				}
				if (count == 0 && !t.Stop) {
					// Nothing found - try Gracenote (NB - no track lengths here)
					Status("Searching Gracenote database");
					GracenoteClient Client = new GracenoteClient(Program.GracenoteKey);
					SearchResult result = Client.Search(new SearchCriteria() {
						AlbumTitle = title,
						Artist = artist
					});
					foreach (Album a in result.Albums) {
						if (t.Stop)
							return;
						ListViewItem item = new ListViewItem(new string[] { a.Artist, a.Title, a.Year.ToString(), a.Tracks.Count() + ":" + string.Join(",", a.Tracks.Select(tr => tr.Title)) });
						item.Tag = new AlbumInfo(a);
						if (SoundEx.Equals(artist, a.Artist) && SoundEx.Equals(title, a.Title))
							Despatch(delegate() { Results.Items.Add(item); });
						else {
							Despatch(delegate() { Results.Items.Insert(insertPoint, item); });
							insertPoint++;
						}
						count++;
					}
				}
				if (!t.Stop) {
					if (count > 0) {
						// Resize columns to fit data
						Invoke((Action)delegate() { Results.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent); });
						Status("Search complete");
					} else
						Status("Nothing found");
				}
			});
		}

		/// <summary>
		/// Stop any search when form closes
		/// </summary>
		private void AlbumDetails_FormClosing(object sender, FormClosingEventArgs e) {
			m_Task.Stop();
		}

		/// <summary>
		/// Reset Cancel button text on entry
		/// </summary>
		private void AlbumDetails_Load(object sender, EventArgs e) {
			btnCancel.Text = "Cancel";
		}

	}
}
