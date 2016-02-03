#define MOVE
#define PLAYER
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Dsp;

namespace AlbumRecorder {
	/// <summary>
	/// Class to display a portion of a wave file
	/// </summary>
	public partial class WaveControl : UserControl {
		/// <summary>
		/// Event5 to indicate when a cursor is moved
		/// </summary>
		public class CursorMovedEventArgs : EventArgs {
			public CursorMovedEventArgs(PositionCursor c) {
				Cursor = c;
				PreviousPosition = c.Position;
				ModifierKeys = Control.ModifierKeys;
			}
			/// <summary>
			/// Cursor which moved
			/// </summary>
			public PositionCursor Cursor;
			/// <summary>
			/// Position before move
			/// </summary>
			public float PreviousPosition;
			/// <summary>
			/// Keys pressed at time
			/// </summary>
			public Keys ModifierKeys;
			/// <summary>
			/// Program.Locked, except when shift key pressed, which reverses it
			/// </summary>
			public bool Locked {
				get {
					bool result = Program.Locked;
					if ((ModifierKeys & Keys.Shift) != 0)
						result = !result;
					return result;
				}
			}
			public override string ToString() {
				return string.Format("CursorMovedEvent {0} {1},{2},{3} {4}",
					Cursor.Label, PreviousPosition, Cursor.Position, ModifierKeys);
			}
		}

		/// <summary>
		/// Cursor which appears on top of waveform
		/// </summary>
		public class PositionCursor {
			/// <summary>
			/// Only display if Active is true
			/// </summary>
			public bool Active = true;
			/// <summary>
			/// Position in seconds (from start of wave file)
			/// </summary>
			public float Position;
			/// <summary>
			/// Text to display next to cursor
			/// </summary>
			public string Label = "";
			/// <summary>
			/// Cursor colour
			/// </summary>
			public Color Color = Color.Red;
			/// <summary>
			/// Event handler when cursor is dragged
			/// </summary>
			public event EventHandler<CursorMovedEventArgs> CursorMoved;
			/// <summary>
			/// Event handler when drag finished
			/// </summary>
			public event EventHandler<CursorMovedEventArgs> CursorMoveFinished;
			/// <summary>
			/// Fire event if required
			/// </summary>
			public void FireCursorMoved(CursorMovedEventArgs args) {
				if (CursorMoved != null)
					CursorMoved(this, args);
			}
			/// <summary>
			/// Fire event if required
			/// </summary>
			public void FireCursorMoveFinished(CursorMovedEventArgs args) {
				if (CursorMoveFinished != null)
					CursorMoveFinished(this, args);
			}
		}
		/// <summary>
		/// Current cursor move event
		/// </summary>
		CursorMovedEventArgs cursorEvent;

		/// <summary>
		/// The wave file
		/// </summary>
		private AudioFileReader m_Wavefile;

		/// <summary>
		/// Boolean for whether the .WAV should draw or not.  So that the control doesnt draw the .WAV until after it is read
		/// </summary>
		private bool			m_DrawWave = false;

		/// <summary>
		/// Start of display
		/// </summary>
		private float m_StartSeconds;

		/// <summary>
		/// Length of display
		/// </summary>
		private float m_LengthSeconds;

		/// <summary>
		/// Buffer containing samples from wave file
		/// </summary>
		private float[] m_Samples;

		/// <summary>
		/// The cursors
		/// </summary>
		private PositionCursor[] m_Cursors;

		/// <summary>
		/// To play a portion of the file
		/// </summary>
		private DirectSoundOut m_Player;

		/// <summary>
		/// So we can stop the player which is playing before starting another
		/// </summary>
		private static DirectSoundOut m_CurrentPlayer;

		/// <summary>
		/// For updating playing now position when playing
		/// </summary>
		private Timer m_Timer = new Timer();

		/// <summary>
		/// Where a potential drag started
		/// </summary>
		private MouseEventArgs m_DragStart;

		/// <summary>
		/// True if definitely dragging
		/// </summary>
		private bool m_Dragging;

		/// <summary>
		/// If true, don't display a wave file, display a section of a recording as it is being recorded
		/// </summary>
		private bool m_BufferOnly;

		/// <summary>
		/// True if a mouse click stopped playback
		/// </summary>
		private bool m_PlayerJustStopped;

		/// <summary>
		/// The playing now position (in samples from start of file)
		/// </summary>
		private int m_SampleCount = -1;

		public WaveControl() {
			InitializeComponent();
			// set up double buffering
			SetStyle(System.Windows.Forms.ControlStyles.UserPaint|System.Windows.Forms.ControlStyles.AllPaintingInWmPaint|System.Windows.Forms.ControlStyles.DoubleBuffer, true);
			m_Timer.Tick += TimerTick;
		}

		public AudioFileReader WaveFile {
			get {
				return m_Wavefile;
			}
			set {
				m_Wavefile = value;
				m_DrawWave = m_Wavefile != null;
				m_BufferOnly = false;
				if (m_DrawWave) {
					WaveFormat = m_Wavefile.WaveFormat;
					LoadSamples();
				}
				Invalidate();
			}
		}

		/// <summary>
		/// Colour of seconds scale
		/// </summary>
		public Color ScaleColor = Color.White;

		/// <summary>
		/// WaveFormat relating to WaveFile or recording
		/// </summary>
		public WaveFormat WaveFormat { get; private set; }

		/// <summary>
		/// The cursors
		/// </summary>
		public PositionCursor[] Cursors {
			get { return m_Cursors; }
			set {
				m_Cursors = value;
				Invalidate();
			}
		}

		/// <summary>
		/// The start of the display in seconds
		/// </summary>
		public float StartSeconds {
			get { return m_StartSeconds; }
			set {
				m_StartSeconds = value;
				if (m_Wavefile != null) {
					LoadSamples();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// The length of the display in seconds
		/// </summary>
		public float LengthSeconds {
			get { return m_LengthSeconds; }
			set {
				m_LengthSeconds = value;
				if (m_DrawWave) {
					LoadSamples();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// When displaying a recording, send new recorded data here
		/// </summary>
		/// <param name="data">Buffer of data</param>
		/// <param name="count">Number of bytes</param>
		public void AddData(byte[] data, int count) {
			int offset = 0;
			int bytesPerSample = WaveFormat.BitsPerSample / 8;
			int samples = count / bytesPerSample;
			int samplesPerPixel = (int)(m_Samples.Length / (Width * WaveFormat.Channels));
			if (samples > m_Samples.Length) {
				// More data supplied than needed - only use part of it
				offset = bytesPerSample * (samples - m_Samples.Length);
				samples = m_Samples.Length;
			}
			if (m_SampleCount + samples > m_Samples.Length) {
				// Need to shift array to left
				int shift = m_SampleCount + samples - m_Samples.Length;
				// Make sure it shifts a whole number of pixels, to reduce display flashing
				int remainder = shift % samplesPerPixel;
				if (remainder != 0)
					shift += samplesPerPixel - remainder;
				Array.Copy(m_Samples, shift, m_Samples, 0, m_Samples.Length - shift);
				Array.Clear(m_Samples, m_Samples.Length - shift, shift);
				m_SampleCount -= shift;
			}
			while(offset < count) {
				// Extract samples from the buffer
				if (WaveFormat.BitsPerSample == 16) {
					m_Samples[m_SampleCount++] = BitConverter.ToInt16(data, offset) / 32768f;
					offset += 2;
				} else if (WaveFormat.BitsPerSample == 24) {
					m_Samples[m_SampleCount++] = (((sbyte)data[offset + 2] << 16) | (data[offset + 1] << 8) | data[offset]) / 8388608f;
					offset += 3;
				} else if (WaveFormat.BitsPerSample == 32 && WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat) {
					m_Samples[m_SampleCount++] = BitConverter.ToSingle(data, offset);
					offset += 4;
				} else if (WaveFormat.BitsPerSample == 32) {
					m_Samples[m_SampleCount++] = BitConverter.ToInt32(data, offset) / (Int32.MaxValue + 1f);
					offset += 4;
				} else {
					throw new InvalidOperationException("Unsupported bit depth");
				}
			}
			Invalidate();
		}

		/// <summary>
		/// Create buffer for displaying recording as it happens
		/// </summary>
		/// <param name="format">WaveFormat of recording data</param>
		public void CreateBuffer(WaveFormat format) {
			m_Wavefile = null;
			WaveFormat = format;
			m_BufferOnly = true;
			LoadSamples();
			m_DrawWave = true;
		}

		/// <summary>
		/// Resize sample buffer, and reload samples if displaying a wave file
		/// </summary>
		private void LoadSamples() {
			if (WaveFormat != null) {
				int length = (int)WaveFormat.SecondsToSamples(m_LengthSeconds);
				m_Samples = new float[length];
				m_SampleCount = 0;
				if (m_Wavefile != null) {
					m_Wavefile.Position = WaveFormat.SecondsToBytes(m_StartSeconds);
					m_Wavefile.Read(m_Samples, 0, length);
				}
			}
		}

		/// <summary>
		/// Dispose of player
		/// </summary>
		private void disposePlayer() {
			if (m_Player != null) {
				try {
					m_Player.Dispose();
				} catch {
				}
				m_Player = null;
			}
		}

		/// <summary>
		/// Stop current player
		/// </summary>
		private void stopPlayer() {
			if (m_CurrentPlayer != null) {
				// Stop playing
				if (m_CurrentPlayer.PlaybackState == PlaybackState.Playing) {
					m_CurrentPlayer.Stop();
					m_PlayerJustStopped = true;
				}
				m_CurrentPlayer = null;
			}
		}

		/// <summary>
		/// Catch event when playback stopped
		/// </summary>
		private void PlaybackStopped(object sender, StoppedEventArgs e) {
			disposePlayer();
			m_SampleCount = -1;
			m_Timer.Stop();
			Refresh();
		}

		/// <summary>
		/// Keep track of playing now cursor
		/// </summary>
		private void TimerTick(object sender, EventArgs e) {
			Refresh();
		}

		/// <summary>
		/// Event when player plays another sample
		/// </summary>
		private void SampleCounter(object sender, SampleEventArgs e) {
			m_SampleCount++;
		}

		/// <summary>
		/// Standard paint event
		/// </summary>
		private void WaveControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			float pixelsPerSecond = Width / m_LengthSeconds;
			Graphics grfx = e.Graphics;
			using (Pen pen = new Pen(ForeColor)) {
				if (Width > 50) {
					// Only display scale if control big enough
					int count = Width / (int)grfx.MeasureString("999:99", Font).Width;	// Max no of labels which will fit
					int tick = (int)(10 * m_LengthSeconds / count);		// Distance in seconds between labels
					// Now round to a factor of 10
					for (int t = 1; ; t *= 10) {
						if (tick < t) {
							tick = t;
							break;
						}
					}
					// Will measure in tenths of a second
					int s = (int)(10 * m_StartSeconds);
					// Number of ticks to display
					count = (int)(10 * m_LengthSeconds / tick);
					// S is now position of first tick
					s = s + tick - s % tick;
					using (Pen w = new Pen(ScaleColor)) {
						using (SolidBrush b = new SolidBrush(ScaleColor)) {
							// Draw line at top for ticks to descend from
							grfx.DrawLine(w, 0, 0, Width, 0);
							for (int i = 0; i < count; i++, s += tick) {
								float secs = s / 10.0f;		// Position of tick in seconds
								float x = (secs - m_StartSeconds) * pixelsPerSecond;	// In pixels
								grfx.DrawLine(w, x, 0, x, 0 + Height / 10);
								if (s % 10 == 0) {
									// Label whole seconds only
									string str = secs.ToTimeSpanString().Replace(".0", "");
									grfx.DrawString(str, Font, b, x + 1, 1);
								}
							}
						}
					}
				}
				if (m_DrawWave)
					Draw(e, pen);	// Draw the wave file
				if (m_Cursors != null && m_Cursors.Length > 0) {
					// Draw the cursors
					foreach (PositionCursor f in m_Cursors) {
						if (f.Active && f.Position >= m_StartSeconds && f.Position < m_StartSeconds + m_LengthSeconds) {
							float x = (f.Position - m_StartSeconds) * pixelsPerSecond;
							using (Pen p = new Pen(f.Color))
								grfx.DrawLine(p, x, 0, x, Height);
							using (SolidBrush b = new SolidBrush(f.Color)) {
								grfx.DrawString(f.Label, Font, b, x + 1, 0);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Draw the sample buffer
		/// </summary>
		private void Draw(PaintEventArgs pea, Pen pen ) {
			Graphics grfx = pea.Graphics;
			// Origin is centre for stereo, at bottom for mono
			int origin = WaveFormat.Channels > 1 ? Height / 2 : Height;
			grfx.DrawLine(pen, 0, origin, Width, origin);
			int samplesPerPixel = (int)(m_Samples.Length / (Width * WaveFormat.Channels));
			int samples = 0;	// Samples processed for this pixel
			float x = 0;		// Position to draw
			float ch1 = 0, ch2 = 0;	// Stereo channels max value over course of 1 pixel
			for (int i = 0; i < m_Samples.Length; i += WaveFormat.Channels) {
				ch1 = Math.Max(ch1, Math.Abs(m_Samples[i]));
				if (WaveFormat.Channels > 1)
					ch2 = Math.Max(ch2, Math.Abs(m_Samples[i + 1]));
				if (++samples == samplesPerPixel) {
					// ch1, ch2 in range 0-1
					if (WaveFormat.Channels == 1) {
						// Set values so ch1 (bottom of line) reaches bottom, and ch2 represents mono signal above it
						ch2 = ch1 * 2 - 1;
						ch1 = 1;
					}
					// Convert to 0-2
					ch1 = ch1 + 1;	// Will be 2 for mono
					ch2 = 1 - ch2;
					grfx.DrawLine(pen, x, ch1 * Height / 2, x, ch2 * Height / 2); 
					ch1 = ch2 = 0;
					samples = 0;
					x++;
				}
			}
			// If playing, show current position
			if ((m_Player != null && m_Player.PlaybackState == PlaybackState.Playing) || m_BufferOnly) {
				x = (float)(m_SampleCount - m_StartSeconds * WaveFormat.SampleRate) / samplesPerPixel;
				using (Pen p = new Pen(Color.White))
					grfx.DrawLine(p, x, 0, x, Height);
			}
		}

		private void WaveControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			stopPlayer();
			if (e.Button == MouseButtons.Left) {
				cursorEvent = null;
				m_DragStart = e;
				if (m_Cursors != null) {
					// Work out which cursor we are close to
					float pixelsPerSecond = Width / m_LengthSeconds;
					int l = -1, r = -1;
					for (int i = 0; i < m_Cursors.Length; i++) {
						PositionCursor c = m_Cursors[i];
						if (!c.Active)
							continue;
						float x = (c.Position - m_StartSeconds) * pixelsPerSecond;	// Position of cursor in pixels
						if (Math.Abs(x - e.X) < 10) {
							// We are close to this cursor
							if (x < e.X) {
								// Cursor to left of mouse
								if (l == -1 || c.Position >= m_Cursors[l].Position)
									l = i;	// It's the closest one to the mouse, or same position but larger cursor no
							} else {
								// Cursor to right of mouse
								if (r == -1 || c.Position < m_Cursors[r].Position)
									r = i;	// It's the closest one to the mouse or same position but smaller cursor no
							}
						}
					}
					int csr = -1;		// Cursor we want to drag
					if(l == -1) {
						// No cursor to left
						if(r == -1)	// Or right either
							return;
						csr = r;	// Choose one to right
					} else if (r == -1) {
						// No cursor to right
						csr = l;	// Choose one to left
					} else {
						// Cursors on both sides - choose closest
						float lx = (m_Cursors[l].Position - m_StartSeconds) * pixelsPerSecond;
						float rx = (m_Cursors[r].Position - m_StartSeconds) * pixelsPerSecond;
						if (rx - e.X < e.X - lx)
							csr = r;
						else
							csr = l;
					}
					// Set up cursor event for cursor
					cursorEvent = new CursorMovedEventArgs(m_Cursors[csr]);
					m_Dragging = false;	// Not dragging yet
				}
			} else if (e.Button == System.Windows.Forms.MouseButtons.Right) {
				// Right click displays filtered average volume in a tooltip
				try {
					float pixelsPerSecond = Width / m_LengthSeconds;
					float p = m_StartSeconds + e.X / pixelsPerSecond;	// Position of click in seconds
					m_Wavefile.Position = WaveFormat.SecondsToBytes(p);
					FilteredSampleProvider f = new FilteredSampleProvider(m_Wavefile, Properties.Settings.Default.SilenceFilterCentre, Properties.Settings.Default.SilenceFilterQ);
					// Display average volume of block at this position
					float avg = f.AvgBlockVolume();
					string tip = string.Format("pos={0} level={1:#0.0####}", p.ToTimeSpanString(), avg);
					Program.Trace(tip);
					toolTip.Show(tip, this);
				} catch {
				}
			}
		}

		/// <summary>
		/// Handle drag
		/// </summary>
		private void WaveControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				if (cursorEvent != null) {
					// Have hold of a cursor
					if (Math.Abs(e.X - m_DragStart.X) > 5 || Math.Abs(e.Y - m_DragStart.Y) > 5)
						m_Dragging = true;		// Have moved it 5 pixels or more
					if (m_Dragging) {
						float pixelsPerSecond = Width / m_LengthSeconds;
						// Update the event and fire it
						cursorEvent.PreviousPosition = cursorEvent.Cursor.Position;
						cursorEvent.Cursor.Position = m_StartSeconds + e.X / pixelsPerSecond;
						cursorEvent.Cursor.FireCursorMoved(cursorEvent);
						Refresh();	// Redisplay the cursor
					}
				}
			}
		}

		private void WaveControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left) {
				stopPlayer();
				if (m_Dragging) {
					if (cursorEvent != null) {
						// Have finished dragging - fire event
						cursorEvent.Cursor.FireCursorMoveFinished(cursorEvent);
						cursorEvent = null;
					}
					m_Dragging = false;
				} else if(!m_PlayerJustStopped) {
					if (m_Wavefile != null) {
						if (m_DragStart == null || Math.Abs(e.X - m_DragStart.X) > 5 || Math.Abs(e.Y - m_DragStart.Y) > 5)
							return;		// Ignore everything except click
						float start = m_StartSeconds;					// Where to start playing
						float end = m_StartSeconds + m_LengthSeconds;	// and end
						float pixelsPerSecond = Width / m_LengthSeconds;
						if (m_Cursors != null) {
							// Play only between cursors
							for (int i = 0; i < m_Cursors.Length; i++) {
								PositionCursor c = m_Cursors[i];
								if (!c.Active)
									continue;
								float x = (c.Position - m_StartSeconds) * pixelsPerSecond;
								if (x <= e.X)
									start = Math.Max(start, c.Position);
								else
									end = Math.Min(end, c.Position);
							}
						}
						// If click not near start, start from click position
						float sx = (start - m_StartSeconds) * pixelsPerSecond;
						if (e.X - sx > 20)
							start = m_StartSeconds + e.X / pixelsPerSecond;
						m_Wavefile.Position = WaveFormat.SecondsToBytes(start);
						NotifyingSampleProvider p = new NotifyingSampleProvider(new OffsetSampleProvider(m_Wavefile) {
							Take = new TimeSpan((long)(10000000 * (end - start)))
						});
						p.Sample += SampleCounter;		// To keep track of samples played, for play position cursor
						m_SampleCount = (int)(start * WaveFormat.SampleRate);	// Play position, in samples
						m_Timer.Interval = (int)(1000 / pixelsPerSecond);		// Should fire about every pixel
						m_Timer.Start();
						m_CurrentPlayer = m_Player = new NAudio.Wave.DirectSoundOut();
						m_Player.PlaybackStopped += PlaybackStopped;
						if(Control.ModifierKeys == Keys.Control)	// Play filtered sound
							m_Player.Init(new FilteredSampleProvider(p, Properties.Settings.Default.SilenceFilterCentre, Properties.Settings.Default.SilenceFilterQ));
						else {
								// Play wave file
						}
							m_Player.Init(p);
						m_Player.Play();
						Refresh();
					}
				}
			}
			m_PlayerJustStopped = false;
		}

		/// <summary>
		/// Ensure redraw when resized
		/// </summary>
		private void WaveControl_Resize(object sender, EventArgs e) {
			Invalidate();
		}
	}

}
