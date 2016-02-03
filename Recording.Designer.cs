namespace AlbumRecorder {
	partial class Recording {
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
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.cbSource = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtFile = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnPause = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.tbVolume = new System.Windows.Forms.TrackBar();
			this.waveControl1 = new AlbumRecorder.WaveControl();
			((System.ComponentModel.ISupportInitialize)(this.tbVolume)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Source:";
			// 
			// cbSource
			// 
			this.cbSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSource.FormattingEnabled = true;
			this.cbSource.Location = new System.Drawing.Point(63, 10);
			this.cbSource.Name = "cbSource";
			this.cbSource.Size = new System.Drawing.Size(463, 21);
			this.cbSource.TabIndex = 1;
			this.cbSource.SelectedIndexChanged += new System.EventHandler(this.cbSource_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(26, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "File:";
			// 
			// txtFile
			// 
			this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFile.Location = new System.Drawing.Point(63, 38);
			this.txtFile.Name = "txtFile";
			this.txtFile.Size = new System.Drawing.Size(433, 20);
			this.txtFile.TabIndex = 4;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(503, 38);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(23, 20);
			this.btnBrowse.TabIndex = 5;
			this.btnBrowse.Text = "...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnStart
			// 
			this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnStart.Location = new System.Drawing.Point(12, 218);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 6;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnPause
			// 
			this.btnPause.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnPause.Enabled = false;
			this.btnPause.Location = new System.Drawing.Point(237, 218);
			this.btnPause.Name = "btnPause";
			this.btnPause.Size = new System.Drawing.Size(75, 23);
			this.btnPause.TabIndex = 6;
			this.btnPause.Text = "Pause";
			this.btnPause.UseVisualStyleBackColor = true;
			this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
			// 
			// btnStop
			// 
			this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStop.Enabled = false;
			this.btnStop.Location = new System.Drawing.Point(451, 218);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 7;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.DefaultExt = "wav";
			this.saveFileDialog1.Filter = "Wav Files (*.wav)|*.wav";
			// 
			// tbVolume
			// 
			this.tbVolume.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbVolume.Location = new System.Drawing.Point(484, 65);
			this.tbVolume.Maximum = 100;
			this.tbVolume.Name = "tbVolume";
			this.tbVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbVolume.Size = new System.Drawing.Size(45, 143);
			this.tbVolume.TabIndex = 8;
			this.tbVolume.Scroll += new System.EventHandler(this.tbVolume_Scroll);
			// 
			// waveControl1
			// 
			this.waveControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.waveControl1.BackColor = System.Drawing.Color.SkyBlue;
			this.waveControl1.Cursors = null;
			this.waveControl1.LengthSeconds = 0F;
			this.waveControl1.Location = new System.Drawing.Point(12, 64);
			this.waveControl1.Name = "waveControl1";
			this.waveControl1.Size = new System.Drawing.Size(466, 144);
			this.waveControl1.StartSeconds = 0F;
			this.waveControl1.TabIndex = 2;
			this.waveControl1.WaveFile = null;
			// 
			// Recording
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(538, 253);
			this.Controls.Add(this.tbVolume);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnPause);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtFile);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.waveControl1);
			this.Controls.Add(this.cbSource);
			this.Controls.Add(this.label1);
			this.Name = "Recording";
			this.Text = "Recording";
			this.Load += new System.EventHandler(this.Recording_Load);
			((System.ComponentModel.ISupportInitialize)(this.tbVolume)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbSource;
		private WaveControl waveControl1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtFile;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnPause;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.TrackBar tbVolume;
	}
}