﻿namespace AlbumRecorder {
	partial class WaveControl {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			disposePlayer();
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// WaveControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.SkyBlue;
			this.Name = "WaveControl";
			this.Size = new System.Drawing.Size(616, 98);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.WaveControl_Paint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WaveControl_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WaveControl_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WaveControl_MouseUp);
			this.Resize += new System.EventHandler(this.WaveControl_Resize);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip toolTip;
	}
}
