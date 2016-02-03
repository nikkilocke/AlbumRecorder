namespace AlbumRecorder {
	partial class MainForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.trackPanel = new System.Windows.Forms.Panel();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.lblStatus = new System.Windows.Forms.Label();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnOpen = new System.Windows.Forms.ToolStripButton();
			this.btnAlbum = new System.Windows.Forms.ToolStripButton();
			this.btnTrackInfo = new System.Windows.Forms.ToolStripButton();
			this.btnSaveProject = new System.Windows.Forms.ToolStripButton();
			this.btnSave = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnUndo = new System.Windows.Forms.ToolStripButton();
			this.btnRedo = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.btnSplit = new System.Windows.Forms.ToolStripButton();
			this.btnNormalize = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.btnLock = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.btnRecord = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.btnOptions = new System.Windows.Forms.ToolStripButton();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// trackPanel
			// 
			this.trackPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackPanel.AutoScroll = true;
			this.trackPanel.Location = new System.Drawing.Point(0, 28);
			this.trackPanel.Name = "trackPanel";
			this.trackPanel.Size = new System.Drawing.Size(719, 295);
			this.trackPanel.TabIndex = 0;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.AddExtension = false;
			this.openFileDialog1.Filter = "Album Recorder Projects|*.AlbumRecorder|Wave Files|*.wav|MP3 Files|*.mp3";
			this.openFileDialog1.ReadOnlyChecked = true;
			this.openFileDialog1.Title = "Open recording";
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblStatus.Location = new System.Drawing.Point(0, 329);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(719, 20);
			this.lblStatus.TabIndex = 3;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpen,
            this.btnAlbum,
            this.btnTrackInfo,
            this.btnSaveProject,
            this.btnSave,
            this.toolStripSeparator1,
            this.btnUndo,
            this.btnRedo,
            this.toolStripSeparator4,
            this.btnSplit,
            this.btnNormalize,
            this.toolStripSeparator2,
            this.btnLock,
            this.toolStripSeparator3,
            this.btnRecord,
            this.toolStripSeparator5,
            this.btnOptions});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(722, 25);
			this.toolStrip1.TabIndex = 6;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnOpen
			// 
			this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOpen.Image = global::AlbumRecorder.Properties.Resources.imgOpen;
			this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(23, 22);
			this.btnOpen.Text = "Open";
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnAlbum
			// 
			this.btnAlbum.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnAlbum.Image = global::AlbumRecorder.Properties.Resources.imgSearch;
			this.btnAlbum.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnAlbum.Name = "btnAlbum";
			this.btnAlbum.Size = new System.Drawing.Size(23, 22);
			this.btnAlbum.Text = "Album Info Search";
			this.btnAlbum.Click += new System.EventHandler(this.btnAlbum_Click);
			// 
			// btnTrackInfo
			// 
			this.btnTrackInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnTrackInfo.Image = global::AlbumRecorder.Properties.Resources.imgTracks;
			this.btnTrackInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnTrackInfo.Name = "btnTrackInfo";
			this.btnTrackInfo.Size = new System.Drawing.Size(23, 22);
			this.btnTrackInfo.Text = "Track information";
			this.btnTrackInfo.Click += new System.EventHandler(this.btnTrackInfo_Click);
			// 
			// btnSaveProject
			// 
			this.btnSaveProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSaveProject.Image = global::AlbumRecorder.Properties.Resources.imgSaveProject;
			this.btnSaveProject.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSaveProject.Name = "btnSaveProject";
			this.btnSaveProject.Size = new System.Drawing.Size(23, 22);
			this.btnSaveProject.Text = "Save Project";
			this.btnSaveProject.Click += new System.EventHandler(this.btnSaveProject_Click);
			// 
			// btnSave
			// 
			this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSave.Image = global::AlbumRecorder.Properties.Resources.imgSave;
			this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(23, 22);
			this.btnSave.Text = "Save tracks to music folder";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnUndo
			// 
			this.btnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnUndo.Enabled = false;
			this.btnUndo.Image = global::AlbumRecorder.Properties.Resources.imgUndo;
			this.btnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnUndo.Name = "btnUndo";
			this.btnUndo.Size = new System.Drawing.Size(23, 22);
			this.btnUndo.Text = "Undo";
			this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
			// 
			// btnRedo
			// 
			this.btnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRedo.Enabled = false;
			this.btnRedo.Image = global::AlbumRecorder.Properties.Resources.imgRedo;
			this.btnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRedo.Name = "btnRedo";
			this.btnRedo.Size = new System.Drawing.Size(23, 22);
			this.btnRedo.Text = "Redo";
			this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// btnSplit
			// 
			this.btnSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSplit.Image = global::AlbumRecorder.Properties.Resources.imgSplit;
			this.btnSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSplit.Name = "btnSplit";
			this.btnSplit.Size = new System.Drawing.Size(23, 22);
			this.btnSplit.Text = "Split album into tracks";
			this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
			// 
			// btnNormalize
			// 
			this.btnNormalize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnNormalize.Image = global::AlbumRecorder.Properties.Resources.Normalize;
			this.btnNormalize.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNormalize.Name = "btnNormalize";
			this.btnNormalize.Size = new System.Drawing.Size(23, 22);
			this.btnNormalize.Text = "Normalize";
			this.btnNormalize.Click += new System.EventHandler(this.btnNormalize_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// btnLock
			// 
			this.btnLock.Checked = true;
			this.btnLock.CheckOnClick = true;
			this.btnLock.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnLock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnLock.Image = global::AlbumRecorder.Properties.Resources.imgLocked;
			this.btnLock.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnLock.Name = "btnLock";
			this.btnLock.Size = new System.Drawing.Size(23, 22);
			this.btnLock.Text = "Unlock track lengths";
			this.btnLock.ToolTipText = "Unlock track lengths";
			this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// btnRecord
			// 
			this.btnRecord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnRecord.Image = global::AlbumRecorder.Properties.Resources.imgRecord;
			this.btnRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRecord.Name = "btnRecord";
			this.btnRecord.Size = new System.Drawing.Size(23, 22);
			this.btnRecord.Text = "Record";
			this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// btnOptions
			// 
			this.btnOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOptions.Image = global::AlbumRecorder.Properties.Resources.imgOptions;
			this.btnOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOptions.Name = "btnOptions";
			this.btnOptions.Size = new System.Drawing.Size(23, 22);
			this.btnOptions.Text = "Options";
			this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "imgLocked.png");
			this.imageList1.Images.SetKeyName(1, "imgUnlocked.png");
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(722, 352);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.trackPanel);
			this.Controls.Add(this.lblStatus);
			this.Name = "MainForm";
			this.Text = "Album Recorder";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel trackPanel;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnLock;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolStripButton btnNormalize;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStripButton btnAlbum;
		private System.Windows.Forms.ToolStripButton btnTrackInfo;
		private System.Windows.Forms.ToolStripButton btnSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton btnRecord;
		private System.Windows.Forms.ToolStripButton btnUndo;
		private System.Windows.Forms.ToolStripButton btnRedo;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton btnSaveProject;
		private System.Windows.Forms.ToolStripButton btnSplit;
		private System.Windows.Forms.ToolStripButton btnOptions;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
	}
}