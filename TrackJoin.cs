using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlbumRecorder {
	/// <summary>
	/// Control to show the gap between tracks (and at start and end), along with the track details
	/// between the gaps
	/// </summary>
	public partial class TrackJoin : UserControl {
		/// <summary>
		/// Event to propagate start/end changes to later tracks, so lengths stay constant
		/// </summary>
		public class CascadingEvent {
			public CascadingEvent(TrackJoin track, float change) {
				Track = track;
				Change = change;
			}
			public TrackJoin Track;
			public float Change;
		}
		/// <summary>
		/// Start of wave display
		/// </summary>
		private float m_DisplayStart;
		/// <summary>
		/// End of previous track (start of gap)
		/// </summary>
		private float m_End;
		/// <summary>
		/// Start of following track (end of gap)
		/// </summary>
		private float m_Start;
		/// <summary>
		/// Whether to show End cursor - not on gap at start of album
		/// </summary>
		private bool m_ShowEnd;
		/// <summary>
		/// Whether to show Start cursor - not on gap at end of album
		/// </summary>
		private bool m_ShowStart;
		// Constants showing order of WaveControl cursors
		const int EndCursor = 0;
		const int StartCursor = 1;

		public TrackJoin() {
			InitializeComponent();
			m_ShowStart = true;
			m_ShowEnd = true;
			waveControl.LengthSeconds = 10;
			waveControl.Cursors = new WaveControl.PositionCursor[] {
					new WaveControl.PositionCursor() { Label = "End", Position = End },
					new WaveControl.PositionCursor() { Label = "Start", Position = Start }
				};
			// Catch events when user drags the cursors
			waveControl.Cursors[0].CursorMoved += EndCursorMoved;
			waveControl.Cursors[0].CursorMoveFinished += MoveFinished;
			waveControl.Cursors[1].CursorMoved += StartCursorMoved;
			waveControl.Cursors[1].CursorMoveFinished += MoveFinished;
			// Catch events when user alters the TimeSpanEdits
			txtEnd.ValueChanging += EndChanging;
			txtEnd.ValueChanged += ChangeFinished;
			txtStart.ValueChanging += StartChanging;
			txtStart.ValueChanged += ChangeFinished;
			txtGap.ValueChanging += GapChanging;
			txtGap.ValueChanged += ChangeFinished;
			txtLength.ValueChanging += LengthChanging;
			txtLength.ValueChanged += ChangeFinished;
		}

		/// <summary>
		/// Constructor used by MainForm when making list of tracks.
		/// Sets everything inteligently according to prev
		/// </summary>
		/// <param name="prev">Previous track TrackJoin</param>
		/// <param name="t">Track</param>
		public TrackJoin(TrackJoin prev, Track t) : this() {
			Previous = prev;
			ShowEnd = prev != null;	// No end cursor on first track
			Track = t;
			UpdateFromTrack();
		}
		
		/// <summary>
		/// Event when start cursor is moved
		/// </summary>
		public event EventHandler<CascadingEvent> StartChanged;
		/// <summary>
		/// Event when gap is changed
		/// </summary>
		public event EventHandler<CascadingEvent> GapChanged;
		/// <summary>
		/// Event when changes have finished (e.g. drag has finished, or button is released)
		/// </summary>
		public event EventHandler<TrackJoin> ChangesFinished;

		/// <summary>
		/// TrackJoin for previous track
		/// </summary>
		public TrackJoin Previous;

		/// <summary>
		/// Track info
		/// </summary>
		public Track Track;

		/// <summary>
		/// Where WaveControl display starts, in seconds
		/// </summary>
		public float DisplayStart {
			get { return m_DisplayStart; }
			set {
				m_DisplayStart = value;
				waveControl.StartSeconds = value;
			}
		}

		/// <summary>
		/// End cursor position, in seconds
		/// </summary>
		public float End {
			get { return m_End; }
			set {
				end = value;
				waveControl.Cursors[EndCursor].Position = value;
				waveControl.Invalidate();
			}
		}

		/// <summary>
		/// End cursor position - only updates data, not WaveControl
		/// </summary>
		private float end {
			get { return m_End; }
			set {
				Gap -= value - m_End;
				txtEnd.Seconds = m_End = value;
				if (Previous != null)
					txtLength.Seconds = Previous.Track.LengthSeconds = m_End - Previous.m_Start;
			}
		}

		/// <summary>
		/// Start cursor position, in seconds
		/// </summary>
		public float Start {
			get { return m_Start; }
			set {
				start = value;
				waveControl.Cursors[StartCursor].Position = value;
				waveControl.Invalidate();
			}
		}

		/// <summary>
		/// Start cursor position - only updates data, not WaveControl
		/// </summary>
		private float start {
			get { return m_Start; }
			set {
				Gap += value - m_Start;
				txtStart.Seconds = m_Start = value;
			}
		}

		/// <summary>
		/// Length of track (calculated from Start of Previous and End of this)
		/// </summary>
		public float Length {
			get { return Previous == null ? 0 : Previous.Track.LengthSeconds; }
		}

		/// <summary>
		/// Gap between tracks
		/// </summary>
		public float Gap {
			get { return Track == null ? 0 : Track.Gap; }
			set { if(Track != null) txtGap.Seconds = Track.Gap = value; }
		}

		/// <summary>
		/// Update all data from Track (and Previous) because it has changed
		/// </summary>
		public void UpdateFromTrack() {
			if (Track != null) {
				lblTrackName.Text = Track.Title;
				txtGap.Seconds = Track.Gap;
			}
			waveControl.Cursors[EndCursor].Position = txtEnd.Seconds = m_End = Previous == null ? 0 : Previous.Start + Previous.Track.LengthSeconds;
			waveControl.Cursors[StartCursor].Position = txtStart.Seconds = m_Start = m_End + Gap;
			waveControl.Invalidate();
			txtLength.Seconds = Previous == null || Previous.Track == null ? 0 : Previous.Track.LengthSeconds;
			Adjust(5);
		}

		/// <summary>
		/// Whether to show End cursor and fields (not on first track)
		/// </summary>
		public bool ShowEnd {
			get { return m_ShowEnd; }
			set {
				m_ShowEnd = value;
				pnlTop.Visible = value;
				waveControl.Cursors[EndCursor].Active = value;
				waveControl.Invalidate();
			}
		}

		/// <summary>
		/// Whether to show Start cursor and fields (not on gap at end)
		/// </summary>
		public bool ShowStart {
			get { return m_ShowStart; }
			set {
				m_ShowStart = value;
				pnlBottom.Visible = value;
				waveControl.Cursors[StartCursor].Active = value;
				waveControl.Invalidate();
			}
		}

		/// <summary>
		/// Adjust WaveCursor display so there is 5 secs either side of cursors
		/// </summary>
		/// <param name="margin"></param>
		public void Adjust(int margin) {
			float start = Math.Max(0, Math.Min(waveControl.Cursors[0].Position, waveControl.Cursors[1].Position) - margin);
			float end = Math.Max(waveControl.Cursors[0].Position, waveControl.Cursors[1].Position) + margin;
			waveControl.StartSeconds = start;
			waveControl.LengthSeconds = end - start;
		}

		/// <summary>
		/// Event handler when Start cursor is dragged
		/// </summary>
		void StartCursorMoved(object sender, WaveControl.CursorMovedEventArgs e) {
			// Adjust values (WaveControl already done, as it called us)
			start = e.Cursor.Position;
			float change = e.Cursor.Position - e.PreviousPosition;
			if (change != 0) {
				if (e.Locked && StartChanged != null) {
					// Keep following track lengths the same
					StartChanged(this, new CascadingEvent(this, change));
				}
				if ((e.ModifierKeys & Keys.Control) != 0 && GapChanged != null) {
					// Change all the gaps
					GapChanged(this, new CascadingEvent(this, change));
				}
			}
		}

		/// <summary>
		/// Event handler when End cursor is dragged
		/// </summary>
		void EndCursorMoved(object sender, WaveControl.CursorMovedEventArgs e) {
			// Adjust values (WaveControl already done, as it called us)
			end = e.Cursor.Position;
			float change = e.Cursor.Position - e.PreviousPosition;
			if (change != 0) {
				if ((e.ModifierKeys & Keys.Control) != 0 && GapChanged != null) {
					// Change all the gaps
					GapChanged(this, new CascadingEvent(this, -change));
				} else if (e.Locked) {
					// Move start cursor for next track as well
					Start += change;
					if (StartChanged != null) {
						// Keep following track lengths the same
						StartChanged(this, new CascadingEvent(this, change));
					}
				}
			}
		}

		/// <summary>
		/// Event handler when drag has finished
		/// </summary>
		void MoveFinished(object sender, WaveControl.CursorMovedEventArgs e) {
			Adjust(5);
			if (ChangesFinished != null)
				ChangesFinished(this, this);
		}

		/// <summary>
		/// Event handler when Start TimeSpanEdit is changing
		/// </summary>
		void StartChanging(object sender, TimeSpanEdit.ChangingEvent e) {
			Start = e.Edit.Seconds;
			float change = e.Edit.Seconds - e.PreviousSeconds;
			if (change != 0 && e.Locked) {
				if (StartChanged != null) {
					// Keep following track lengths the same
					StartChanged(this, new CascadingEvent(this, change));
				}
			}
		}

		/// <summary>
		/// Event handler when End TimeSpanEdit is changing
		/// </summary>
		void EndChanging(object sender, TimeSpanEdit.ChangingEvent e) {
			End = e.Edit.Seconds;
			float change = e.Edit.Seconds - e.PreviousSeconds;
			if (change != 0 && e.Locked) {
				// Move start cursor for next track as well
				Start += change;
				if (StartChanged != null) {
					// Keep following track lengths the same
					StartChanged(this, new CascadingEvent(this, change));
				}
			}
		}

		/// <summary>
		/// Event handler when Gap TimeSpanEdit is changing
		/// </summary>
		void GapChanging(object sender, TimeSpanEdit.ChangingEvent e) {
			float change = e.Edit.Seconds - e.PreviousSeconds;
			if (change != 0) {
				// Save current gap
				float seconds = e.Edit.Seconds;
				// Move start
				Start += change;
				if (e.Locked && StartChanged != null) {
					// Keep following track lengths the same
					StartChanged(this, new CascadingEvent(this, change));
				}
				if ((e.ModifierKeys & Keys.Control) != 0 && GapChanged != null) {
					// Change all the gaps
					GapChanged(this, new CascadingEvent(this, change));
				}
				// Restore current gap
				e.Edit.Seconds = seconds;
			}
		}

		/// <summary>
		/// Event handler when Length TimeSpanEdit is changing
		/// </summary>
		void LengthChanging(object sender, TimeSpanEdit.ChangingEvent e) {
			float change = e.Edit.Seconds - e.PreviousSeconds;
			if (change != 0) {
				// Save current length
				float seconds = e.Edit.Seconds;
				// Update end (to make track longer)
				End += change;
				if (e.Locked) {
					Start += change;
					if (StartChanged != null) {
						// Keep following track lengths the same
						StartChanged(this, new CascadingEvent(this, change));
					}
				}
				// Restore current length
				e.Edit.Seconds = seconds;
			}
		}

		/// <summary>
		/// Event handler when TimeSpanEdit changes finished
		/// </summary>
		void ChangeFinished(object sender, TimeSpanEdit.ChangingEvent e) {
			Adjust(5);
			if (ChangesFinished != null)
				ChangesFinished(this, this);
		}

	}
}
