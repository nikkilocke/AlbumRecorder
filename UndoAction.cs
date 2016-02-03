using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbumRecorder {
	/// <summary>
	/// Class to undo an action
	/// </summary>
	public abstract class UndoAction {
		/// <summary>
		/// Undo this action
		/// </summary>
		/// <param name="form">Main form</param>
		/// <returns>Redo action</returns>
		abstract public UndoAction Undo(MainForm form);
	}

	/// <summary>
	/// Undo a change to track times & lengths only
	/// </summary>
	public class UndoMove : UndoAction {
		List<TrackData> m_State;

		public UndoMove() {
			// Save current data in constructor
			m_State = Program.CurrentState.TrackData();
		}

		public override UndoAction Undo(MainForm form) {
			// Save current data as redo action
			UndoMove redo = new UndoMove();
			// Restore data saved in constructor
			for (int i = 0; i < m_State.Count; i++) {
				Program.Album.Tracks[i].LengthSeconds = m_State[i].LengthSeconds;
				Program.Album.Tracks[i].Gap = m_State[i].Gap;
			}
			// Update display
			form.UpdateTrackJoins();
			return redo;
		}
	}

	/// <summary>
	/// Undo a change to the album data (e.g. no of tracks, names, etc).
	/// </summary>
	public class UndoEdit : UndoAction {
		AlbumInfo m_State;

		public UndoEdit() {
			// Save current data in constructor
			if (Program.Album != null)
				Program.CurrentState = new AlbumInfo(Program.Album);
			m_State = Program.CurrentState;
		}

		public override UndoAction Undo(MainForm form) {
			// Save current data as redo action
			UndoEdit redo = new UndoEdit();
			// Copy back state saved in constructor
			Program.Album = m_State;
			// Reload file if changed
			form.ReloadFile();
			// Update display
			form.BuildTrackJoins();
			return redo;
		}
	}

	/// <summary>
	/// Undo a change to the volume
	/// </summary>
	public class UndoVolume : UndoAction {
		float m_State;

		public UndoVolume() {
			// Save state in constructor
			m_State = Program.Album.Volume;
		}

		public override UndoAction Undo(MainForm form) {
			// Save current state for redo
			UndoVolume redo = new UndoVolume();
			// Update volume
			form.Volume = m_State;
			return redo;
		}
	}
}
