namespace AlbumRecorder {
	partial class TrackJoin {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblTrackName = new System.Windows.Forms.Label();
			this.pnlBottom = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.waveControl = new AlbumRecorder.WaveControl();
			this.txtStart = new AlbumRecorder.TimeSpanEdit();
			this.txtGap = new AlbumRecorder.TimeSpanEdit();
			this.txtLength = new AlbumRecorder.TimeSpanEdit();
			this.txtEnd = new AlbumRecorder.TimeSpanEdit();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.pnlBottom.SuspendLayout();
			this.pnlTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblTrackName);
			this.panel1.Controls.Add(this.pnlBottom);
			this.panel1.Controls.Add(this.pnlTop);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(546, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(202, 100);
			this.panel1.TabIndex = 1;
			// 
			// lblTrackName
			// 
			this.lblTrackName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblTrackName.Location = new System.Drawing.Point(0, 22);
			this.lblTrackName.Name = "lblTrackName";
			this.lblTrackName.Size = new System.Drawing.Size(202, 58);
			this.lblTrackName.TabIndex = 2;
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.txtStart);
			this.pnlBottom.Controls.Add(this.label4);
			this.pnlBottom.Controls.Add(this.txtGap);
			this.pnlBottom.Controls.Add(this.label3);
			this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBottom.Location = new System.Drawing.Point(0, 80);
			this.pnlBottom.Name = "pnlBottom";
			this.pnlBottom.Size = new System.Drawing.Size(202, 20);
			this.pnlBottom.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(101, 3);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(29, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Start";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(0, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(27, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Gap";
			// 
			// pnlTop
			// 
			this.pnlTop.Controls.Add(this.txtLength);
			this.pnlTop.Controls.Add(this.label2);
			this.pnlTop.Controls.Add(this.txtEnd);
			this.pnlTop.Controls.Add(this.label1);
			this.pnlTop.Controls.Add(this.label5);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 0);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(202, 22);
			this.pnlTop.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(101, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Length";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(0, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(26, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "End";
			// 
			// waveControl
			// 
			this.waveControl.BackColor = System.Drawing.Color.SkyBlue;
			this.waveControl.Cursors = null;
			this.waveControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.waveControl.LengthSeconds = 10F;
			this.waveControl.Location = new System.Drawing.Point(0, 0);
			this.waveControl.Name = "waveControl";
			this.waveControl.Size = new System.Drawing.Size(546, 100);
			this.waveControl.StartSeconds = 0F;
			this.waveControl.TabIndex = 0;
			this.waveControl.WaveFile = null;
			// 
			// txtStart
			// 
			this.txtStart.Location = new System.Drawing.Point(147, 0);
			this.txtStart.Name = "txtStart";
			this.txtStart.Seconds = 0F;
			this.txtStart.Size = new System.Drawing.Size(60, 18);
			this.txtStart.TabIndex = 3;
			this.txtStart.TimeSpan = System.TimeSpan.Parse("00:00:00");
			// 
			// txtGap
			// 
			this.txtGap.Location = new System.Drawing.Point(33, 0);
			this.txtGap.Name = "txtGap";
			this.txtGap.Seconds = 0F;
			this.txtGap.Size = new System.Drawing.Size(60, 18);
			this.txtGap.TabIndex = 1;
			this.txtGap.TimeSpan = System.TimeSpan.Parse("00:00:00");
			// 
			// txtLength
			// 
			this.txtLength.Location = new System.Drawing.Point(147, 0);
			this.txtLength.Name = "txtLength";
			this.txtLength.Seconds = 0F;
			this.txtLength.Size = new System.Drawing.Size(60, 18);
			this.txtLength.TabIndex = 3;
			this.txtLength.TimeSpan = System.TimeSpan.Parse("00:00:00");
			// 
			// txtEnd
			// 
			this.txtEnd.Location = new System.Drawing.Point(33, 0);
			this.txtEnd.Name = "txtEnd";
			this.txtEnd.Seconds = 0F;
			this.txtEnd.Size = new System.Drawing.Size(60, 18);
			this.txtEnd.TabIndex = 1;
			this.txtEnd.TimeSpan = System.TimeSpan.Parse("00:00:00");
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label5.Location = new System.Drawing.Point(0, 20);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(202, 2);
			this.label5.TabIndex = 4;
			// 
			// TrackJoin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.waveControl);
			this.Controls.Add(this.panel1);
			this.Name = "TrackJoin";
			this.Size = new System.Drawing.Size(748, 100);
			this.panel1.ResumeLayout(false);
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.pnlTop.ResumeLayout(false);
			this.pnlTop.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public WaveControl waveControl;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel pnlBottom;
		private System.Windows.Forms.Panel pnlTop;
		public System.Windows.Forms.Label lblTrackName;
		private System.Windows.Forms.Label label1;
		private TimeSpanEdit txtEnd;
		private TimeSpanEdit txtStart;
		private System.Windows.Forms.Label label4;
		private TimeSpanEdit txtGap;
		private System.Windows.Forms.Label label3;
		private TimeSpanEdit txtLength;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;

	}
}
