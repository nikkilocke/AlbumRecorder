using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlbumRecorder {
	/// <summary>
	/// Edit a time span
	/// </summary>
	public partial class TimeSpanEdit : UserControl {
		/// <summary>
		/// Event when the user changes the value
		/// </summary>
		public class ChangingEvent {
			public ChangingEvent(TimeSpanEdit e) {
				Edit = e;
				PreviousSeconds = e.m_Seconds;
				ModifierKeys = Control.ModifierKeys;
			}
			/// <summary>
			/// The associated TimeSpanEdit
			/// </summary>
			public TimeSpanEdit Edit;
			/// <summary>
			/// Value before change
			/// </summary>
			public float PreviousSeconds;
			/// <summary>
			/// Value before change as a TimeSpan
			/// </summary>
			public TimeSpan Previous {
				get { return new TimeSpan((long)(10000000 * PreviousSeconds)); }
			}
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
				return string.Format("ChangingEvent {0} {1},{2},{3}",
					Edit.Name, PreviousSeconds, Edit.m_Seconds, ModifierKeys);
			}
		}
		/// <summary>
		/// Value
		/// </summary>
		float m_Seconds;
		/// <summary>
		/// Amount to increment when a button is pressed
		/// </summary>
		float m_Increment;
		/// <summary>
		/// Current ChangingEvent
		/// </summary>
		ChangingEvent m_Event;

		public TimeSpanEdit() {
			InitializeComponent();
		}

		/// <summary>
		/// Event when user uses button to change value - repeats while button held down.
		/// Also fired when user uses textBox to change value
		/// </summary>
		public event EventHandler<ChangingEvent> ValueChanging;
		/// <summary>
		/// Event when user has finished changing value (releases button, or tabs off Textbox)
		/// </summary>
		public event EventHandler<ChangingEvent> ValueChanged;

		/// <summary>
		/// Where to report errors
		/// </summary>
		public ErrorProvider ErrorProvider;

		/// <summary>
		/// Value in seconds
		/// </summary>
		public float Seconds {
			get { return m_Seconds; }
			set {
				m_Seconds = value;
				UpdateText();
			}
		}

		/// <summary>
		/// Value as a TimeSpan
		/// </summary>
		public TimeSpan TimeSpan {
			get { return new TimeSpan((long)(10000000 * m_Seconds)); }
			set { Seconds = (float)value.TotalSeconds; }
		}

		/// <summary>
		/// Update the text box with the current value
		/// </summary>
		private void UpdateText() {
			this.textBox1.Text = m_Seconds.ToTimeSpanString();
		}

		/// <summary>
		/// Update the value from the text box
		/// </summary>
		/// <returns>True if the value was valid</returns>
		private bool ParseTextBox() {
			return textBox1.Text.TimeSpanToSeconds(ref m_Seconds);
		}

		/// <summary>
		/// Increase button depressed
		/// </summary>
		private void btnUp_MouseDown(object sender, MouseEventArgs e) {
			// Start new event (before changing value)
			m_Event = new ChangingEvent(this);
			// Set increment for timer
			m_Increment = 0.1f;
			// Increment value
			m_Seconds += m_Increment;
			// And update
			UpdateText();
			// Fire event
			if (ValueChanging != null) ValueChanging(this, m_Event);
			// Start timer while button down
			timer1.Start();
		}

		private void btn_MouseUp(object sender, MouseEventArgs e) {
			// Stop timer
			timer1.Stop();
			// Fire change finished event
			if (ValueChanged != null) ValueChanged(this, m_Event);
			m_Event = null;
		}

		/// <summary>
		/// Decrease button pressed
		/// </summary>
		private void btnDown_MouseDown(object sender, MouseEventArgs e) {
			// Start new event (before changing value)
			m_Event = new ChangingEvent(this);
			// Set increment for timer
			m_Increment = -0.1f;
			// Increment value
			m_Seconds += m_Increment;
			// And update
			UpdateText();
			// Fire event
			if (ValueChanging != null) ValueChanging(this, m_Event);
			// Start timer
			timer1.Start();
		}

		/// <summary>
		/// TextBox value changed (and focus leaving)
		/// </summary>
		private void textBox1_Validating(object sender, CancelEventArgs e) {
			// Start new event (before changing value)
			m_Event = new ChangingEvent(this);
			if (ParseTextBox()) {
				// Update
				UpdateText();
				// Fire both events
				if (ValueChanging != null) ValueChanging(this, m_Event);
				if (ValueChanged != null) ValueChanged(this, m_Event);
			} else {
				e.Cancel = true;
				if (ErrorProvider != null)
					ErrorProvider.SetError(this, "Invalid time");
			}
			m_Event = null;
		}

		/// <summary>
		/// Remove error message after successful validation
		/// </summary>
		private void textBox1_Validated(object sender, EventArgs e) {
			if (ErrorProvider != null)
				ErrorProvider.SetError(this, "");
		}

		/// <summary>
		/// Repeat button event
		/// </summary>
		private void timer1_Tick(object sender, EventArgs e) {
			m_Event.PreviousSeconds = m_Seconds;
			m_Seconds += m_Increment;
			UpdateText();
			if (ValueChanging != null) ValueChanging(this, m_Event);
		}
	}
}
