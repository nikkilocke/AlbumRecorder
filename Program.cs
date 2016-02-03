using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;

namespace AlbumRecorder {
	static class Program {
		public const string GracenoteKey = "1032852198-E5015C394A16665EE980474BCE5A113A";

		public static AlbumInfo Album;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			if (string.IsNullOrWhiteSpace(Properties.Settings.Default.MusicFolder)) {
				// Default music folder is My Music
				Properties.Settings.Default.MusicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
				Properties.Settings.Default.Save();
			}
			if(string.IsNullOrWhiteSpace(Properties.Settings.Default.RecordingFolder)) {
				// Default Recording folder is TEMP folder
				Properties.Settings.Default.RecordingFolder = Path.GetTempPath();
				Properties.Settings.Default.Save();
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		/// <summary>
		/// Saved Album Info
		/// </summary>
		public static AlbumInfo CurrentState;

		/// <summary>
		/// True if Locked icon is showing - locks subsequent track lengths when moving start and end of track
		/// </summary>
		public static bool Locked = true;

		/// <summary>
		/// Helper function for tracing
		/// </summary>
		public static void Trace(string message) {
			System.Diagnostics.Trace.WriteLine(message);
		}

		/// <summary>
		/// Helper function for tracing
		/// </summary>
		public static void Trace(string format, params object[] args) {
			Trace(string.Format(format, args));
		}

		/// <summary>
		/// Convert seconds to a TimeSpan string [mm:]ss.t
		/// </summary>
		public static string ToTimeSpanString(this float self) {
			StringBuilder b = new StringBuilder();
			float s = Math.Abs(self);
			int ws = (int)Math.Floor(s);
			if (self < 0) {
				b.Append('-');
			}
			int m = (int)ws / 60;
			s -= m * 60;
			b.AppendFormat(m == 0 ? "{0:#0.0}" : "{1}:{0:00.0}", s, m);
			return b.ToString();
		}

		/// <summary>
		/// Convert TimeSpan string [mm:]ss[.t] to seconds
		/// </summary>
		public static bool TimeSpanToSeconds(this string self, ref float seconds) {
			Match m = Regex.Match(self, @"^\s*-?(?:(\d+):)?(\d+(?:\.\d*)?)\s*$");
			if (m.Success) {
				try {
					float s = float.Parse(m.Groups[2].Value);
					if (!string.IsNullOrEmpty(m.Groups[1].Value))
						s += 60 * int.Parse(m.Groups[1].Value);
					seconds = s;
					return true;
				} catch {
				}
			}
			return false;
		}

		/// <summary>
		/// Convert wave file byte count to seconds
		/// </summary>
		public static float BytesToSeconds(this WaveFormat f, long bytes) {
			return (float)f.SamplesToSeconds(bytes) * 8 / f.BitsPerSample;
		}

		/// <summary>
		/// Convert wave file sample count to seconds
		/// </summary>
		public static float SamplesToSeconds(this WaveFormat f, long samples) {
			return (float)samples / (f.SampleRate * f.Channels);
		}

		/// <summary>
		/// Convert seconds to wave file byte count
		/// </summary>
		public static long SecondsToBytes(this WaveFormat f, float seconds) {
			return f.SecondsToSamples(seconds) * f.BitsPerSample / 8;
		}

		/// <summary>
		/// Convert seconds to wave file sample count
		/// </summary>
		public static long SecondsToSamples(this WaveFormat f, float seconds) {
			long l = (long)(seconds * f.SampleRate * f.Channels);
			l -= l % f.BlockAlign;
			return l;
		}

		/// <summary>
		/// Parse a float value >= 0
		/// Throws if invalid
		/// </summary>
		/// <param name="desc">Description (for errors)</param>
		public static float ToFloat(this string self, string desc) {
			float r;
			try {
				r = float.Parse(self);
			} catch {
				throw new ApplicationException(desc + " '" + self + "' invalid");
			}
			if (r < 0)
				throw new ApplicationException(desc + " must be >= 0");
			return r;
		}

		/// <summary>
		/// Parse int from string. Throw if invalid.
		/// </summary>
		/// <param name="self">Input string</param>
		/// <param name="min">Min allowed value</param>
		/// <param name="desc">For errors</param>
		/// <returns></returns>
		public static int ToInt(this string self, int min, string desc) {
			int r;
			try {
				r = int.Parse(self);
			} catch {
				throw new ApplicationException(desc + " '" + self + "' invalid");
			}
			if (r < min)
				throw new ApplicationException(desc + " must be > " + (min - 1));
			return r;
		}

	}

}
