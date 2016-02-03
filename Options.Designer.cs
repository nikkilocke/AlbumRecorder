namespace AlbumRecorder {
	partial class Options {
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtMusicFolder = new System.Windows.Forms.TextBox();
			this.btnBrowseMusicFolder = new System.Windows.Forms.Button();
			this.btnBrowseRecordings = new System.Windows.Forms.Button();
			this.txtRecordingsFolder = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtCentre = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtQ = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtEndSilence = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtStartSilence = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblStatus = new System.Windows.Forms.Label();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Music Folder";
			// 
			// txtMusicFolder
			// 
			this.txtMusicFolder.Location = new System.Drawing.Point(117, 10);
			this.txtMusicFolder.Name = "txtMusicFolder";
			this.txtMusicFolder.Size = new System.Drawing.Size(465, 20);
			this.txtMusicFolder.TabIndex = 1;
			// 
			// btnBrowseMusicFolder
			// 
			this.btnBrowseMusicFolder.Location = new System.Drawing.Point(588, 8);
			this.btnBrowseMusicFolder.Name = "btnBrowseMusicFolder";
			this.btnBrowseMusicFolder.Size = new System.Drawing.Size(22, 23);
			this.btnBrowseMusicFolder.TabIndex = 2;
			this.btnBrowseMusicFolder.Text = "...";
			this.btnBrowseMusicFolder.UseVisualStyleBackColor = true;
			this.btnBrowseMusicFolder.Click += new System.EventHandler(this.btnBrowseMusicFolder_Click);
			// 
			// btnBrowseRecordings
			// 
			this.btnBrowseRecordings.Location = new System.Drawing.Point(588, 34);
			this.btnBrowseRecordings.Name = "btnBrowseRecordings";
			this.btnBrowseRecordings.Size = new System.Drawing.Size(22, 23);
			this.btnBrowseRecordings.TabIndex = 5;
			this.btnBrowseRecordings.Text = "...";
			this.btnBrowseRecordings.UseVisualStyleBackColor = true;
			this.btnBrowseRecordings.Click += new System.EventHandler(this.btnBrowseRecordings_Click);
			// 
			// txtRecordingsFolder
			// 
			this.txtRecordingsFolder.Location = new System.Drawing.Point(117, 37);
			this.txtRecordingsFolder.Name = "txtRecordingsFolder";
			this.txtRecordingsFolder.Size = new System.Drawing.Size(465, 20);
			this.txtRecordingsFolder.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(93, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Recordings Folder";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtEndSilence);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.txtStartSilence);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.txtQ);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.txtCentre);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(12, 63);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(598, 83);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Silence Filter";
			// 
			// txtCentre
			// 
			this.txtCentre.Location = new System.Drawing.Point(159, 19);
			this.txtCentre.Name = "txtCentre";
			this.txtCentre.Size = new System.Drawing.Size(124, 20);
			this.txtCentre.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(1, 22);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(124, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Centre Frequency (3500)";
			// 
			// txtQ
			// 
			this.txtQ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtQ.Location = new System.Drawing.Point(449, 19);
			this.txtQ.Name = "txtQ";
			this.txtQ.Size = new System.Drawing.Size(121, 20);
			this.txtQ.TabIndex = 10;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(312, 22);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(98, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Filter width Q (0.25)";
			// 
			// txtEndSilence
			// 
			this.txtEndSilence.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtEndSilence.Location = new System.Drawing.Point(449, 45);
			this.txtEndSilence.Name = "txtEndSilence";
			this.txtEndSilence.Size = new System.Drawing.Size(121, 20);
			this.txtEndSilence.TabIndex = 14;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(312, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(117, 13);
			this.label5.TabIndex = 13;
			this.label5.Text = "Music threshold (0.003)";
			// 
			// txtStartSilence
			// 
			this.txtStartSilence.Location = new System.Drawing.Point(159, 45);
			this.txtStartSilence.Name = "txtStartSilence";
			this.txtStartSilence.Size = new System.Drawing.Size(124, 20);
			this.txtStartSilence.TabIndex = 12;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(1, 48);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(147, 13);
			this.label6.TabIndex = 11;
			this.label6.Text = "Start silence threshold (0.002)";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(16, 155);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(535, 155);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblStatus.ForeColor = System.Drawing.Color.Red;
			this.lblStatus.Location = new System.Drawing.Point(97, 155);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(432, 19);
			this.lblStatus.TabIndex = 9;
			// 
			// folderBrowserDialog1
			// 
			// 
			// Options
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(622, 190);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnBrowseRecordings);
			this.Controls.Add(this.txtRecordingsFolder);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnBrowseMusicFolder);
			this.Controls.Add(this.txtMusicFolder);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Options";
			this.Text = "Options";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtMusicFolder;
		private System.Windows.Forms.Button btnBrowseMusicFolder;
		private System.Windows.Forms.Button btnBrowseRecordings;
		private System.Windows.Forms.TextBox txtRecordingsFolder;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtCentre;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtQ;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtEndSilence;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtStartSilence;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
	}
}