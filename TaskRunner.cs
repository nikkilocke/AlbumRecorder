using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbumRecorder {
	/// <summary>
	/// Class to run a task, with an option to cancel
	/// </summary>
	public class TaskRunner {
		Task m_Task;

		public TaskRunner() {
		}

		public TaskRunner(Action<object, Exception> handler) {
			ExceptionThrown += new EventHandler<Exception>(handler);
		}

		/// <summary>
		/// Stop any existing running task, and start a new one
		/// </summary>
		public void Run(Action<Task> action) {
			lock (this) {
				Stop();
				m_Task = new Task(this, action);
			}
		}

		/// <summary>
		/// Stop any existing running task
		/// </summary>
		public void Stop() {
			lock (this) {
				if (m_Task != null) {
					m_Task.Stop = true;
					m_Task = null;
				}
			}
		}

		/// <summary>
		/// Event thrown if there is an exception while running the task
		/// </summary>
		public event EventHandler<Exception> ExceptionThrown;

		/// <summary>
		/// Class to control a single instance of a task
		/// </summary>
		public class Task {

			public Task(TaskRunner runner, Action<Task> action) {
				new System.Threading.Tasks.Task(delegate() {
					try {
						action(this);
					} catch(Exception ex) {
						if (runner.ExceptionThrown != null)
							runner.ExceptionThrown(runner, ex);
					} finally {
						lock (runner) {
							if (runner.m_Task == this) {
								runner.m_Task = null;
							}
						}
					}
				}).Start();
			}

			/// <summary>
			/// Set to true when the task should stop - test this frequently in the action
			/// </summary>
			public bool Stop;

		}
	}

}
