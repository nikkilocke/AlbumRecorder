using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using Hqub.MusicBrainz.API.Entities;
using ParkSquare.Gracenote;

namespace AlbumRecorder {
	public class AlbumInfo {
		public AlbumInfo() {
			Tracks = new List<Track>();
		}

		public AlbumInfo(string a, Release r, Medium m) : this() {
			Title = r.Title;
			Artist = a;
			if (r.Date != null) {
				Match y = Regex.Match(r.Date, @"\d+");
				if (y.Success)
					Year = uint.Parse(y.Value);
			}
			ReleaseId = r.Id;
			foreach (Hqub.MusicBrainz.API.Entities.Track t in m.Tracks.Items) {
				Tracks.Add(new Track(this, t));
			}
		}

		public AlbumInfo(Album a)
			: this() {
			Title = a.Title;
			Artist = a.Artist;
			Year = (uint)a.Year;
			GracenoteId = a.Id;
			foreach (ParkSquare.Gracenote.Track t in a.Tracks) {
				Tracks.Add(new Track(this, t));
			}
		}

		public AlbumInfo(AlbumInfo copy) {
			if (copy == null)
				return;
			Filename = copy.Filename;
			Title = copy.Title;
			Artist = copy.Artist;
			Year = copy.Year;
			Genre = copy.Genre;
			Publisher = copy.Publisher;
			ReleaseId = copy.ReleaseId;
			GracenoteId = copy.GracenoteId;
			Volume = copy.Volume;
			Tracks = copy.Tracks.Select(t => new Track(t)).ToList();
		}

		public string Filename;
		public string Title;
		public string Artist;
		public uint Year;
		public string Genre;
		public string Publisher;
		public string ReleaseId;
		public string GracenoteId;
		public float Volume = 1;
		[XmlIgnore]
		public Image Art;
		[XmlElement("Art")]
		public byte[] ImageBuffer {
			get {
				if (Art == null)
					return null;
				using (MemoryStream mem = new MemoryStream()) {
					Art.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
					return mem.ToArray();
				}
			}
			set {
				if (value == null) {
					Art = null;
				} else {
					using (MemoryStream mem = new MemoryStream(value)) {
						Art = Image.FromStream(mem);
					}
				}
			}
		}
		public List<Track> Tracks;
		public List<TrackData> TrackData() {
			return Tracks.Select(t => new TrackData(t)).ToList();
		}
		public override string ToString() {
			return string.Format("{0} {1} {2} tracks", Title, Artist, Tracks.Count);
		}
	}

	public class Track {
		public Track() {
			Gap = 2;
		}

		public Track(AlbumInfo a, Hqub.MusicBrainz.API.Entities.Track t) : this() {
			Title = t.Recording.Title;
			Artist = String.Join(", ", t.Recording.Credits.Select(c => c.Artist.Name).ToArray());
			if (string.IsNullOrWhiteSpace(Artist))
				Artist = a.Artist;
			LengthSeconds = (float)t.Length / 1000;
		}

		public Track(AlbumInfo a, ParkSquare.Gracenote.Track t)
			: this() {
			Title = t.Title;
			Artist = t.Artist;
			if (string.IsNullOrWhiteSpace(Artist))
				Artist = a.Artist;
		}

		public Track(Track t) {
			Title = t.Title;
			Artist = t.Artist;
			LengthSeconds = t.LengthSeconds;
			Gap = t.Gap;
		}

		public string Title { get; set; }
		public string Artist { get; set; }
		public string Length {
			get {
				return LengthSeconds.ToTimeSpanString();
			}
			set {
				value.TimeSpanToSeconds(ref LengthSeconds);
			}
		}
		public float LengthSeconds;
		public float Gap;
		public override string ToString() {
			return string.Format("{0} ({1}) {2}", Title, Gap.ToTimeSpanString(), Length);
		}
	}

	public class TrackData {
		public TrackData() {
		}

		public TrackData(Track t) {
			LengthSeconds = t.LengthSeconds;
			Gap = t.Gap;
		}

		public float LengthSeconds;

		public float Gap;
	}
}
