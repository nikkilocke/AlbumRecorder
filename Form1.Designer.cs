namespace AlbumRecorder {
	partial class Form1 {
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.tracks = new System.Windows.Forms.ListView();
			this.btnOpen = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.lblStatus = new System.Windows.Forms.TextBox();
			this.btnAlbum = new System.Windows.Forms.Button();
			this.waveControl1 = new AlbumRecorder.WaveControl();
			this.label1 = new System.Windows.Forms.Label();
			this.txtStart = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtLength = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "wav;.mpg";
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.ReadOnlyChecked = true;
			this.openFileDialog1.Title = "Open recording";
			// 
			// tracks
			// 
			this.tracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tracks.Location = new System.Drawing.Point(1, -1);
			this.tracks.Name = "tracks";
			this.tracks.Size = new System.Drawing.Size(719, 295);
			this.tracks.TabIndex = 0;
			this.tracks.UseCompatibleStateImageBehavior = false;
			// 
			// btnOpen
			// 
			this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOpen.Location = new System.Drawing.Point(5, 319);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(71, 28);
			this.btnOpen.TabIndex = 1;
			this.btnOpen.Text = "Open";
			this.btnOpen.UseVisualStyleBackColor = true;
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(634, 318);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 28);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblStatus.Location = new System.Drawing.Point(0, 293);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(719, 20);
			this.lblStatus.TabIndex = 3;
			// 
			// btnAlbum
			// 
			this.btnAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAlbum.Location = new System.Drawing.Point(95, 318);
			this.btnAlbum.Name = "btnAlbum";
			this.btnAlbum.Size = new System.Drawing.Size(71, 28);
			this.btnAlbum.TabIndex = 4;
			this.btnAlbum.Text = "Album info";
			this.btnAlbum.UseVisualStyleBackColor = true;
			this.btnAlbum.Click += new System.EventHandler(this.btnAlbum_Click);
			// 
			// waveControl1
			// 
			this.waveControl1.BackColor = System.Drawing.Color.SkyBlue;
			this.waveControl1.Location = new System.Drawing.Point(1, -1);
			this.waveControl1.Name = "waveControl1";
			this.waveControl1.Size = new System.Drawing.Size(719, 268);
			this.waveControl1.TabIndex = 5;
			this.waveControl1.WaveFile = null;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 274);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(29, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Start";
			// 
			// txtStart
			// 
			this.txtStart.Location = new System.Drawing.Point(48, 273);
			this.txtStart.Name = "txtStart";
			this.txtStart.Size = new System.Drawing.Size(47, 20);
			this.txtStart.TabIndex = 7;
			this.txtStart.Text = "0";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(126, 274);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "Length";
			// 
			// txtLength
			// 
			this.txtLength.Location = new System.Drawing.Point(172, 271);
			this.txtLength.Name = "txtLength";
			this.txtLength.Size = new System.Drawing.Size(58, 20);
			this.txtLength.TabIndex = 9;
			this.txtLength.Text = "10";
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(254, 271);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(39, 23);
			this.btnGo.TabIndex = 10;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(722, 352);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtLength);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtStart);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.waveControl1);
			this.Controls.Add(this.btnAlbum);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnOpen);
			this.Controls.Add(this.tracks);
			this.Name = "Form1";
			this.Text = "Album Recorder";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ListView tracks;
		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox lblStatus;
		private System.Windows.Forms.Button btnAlbum;
		private AlbumRecorder.WaveControl waveControl1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtStart;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtLength;
		private System.Windows.Forms.Button btnGo;
	}
}

