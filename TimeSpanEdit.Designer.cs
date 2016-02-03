namespace AlbumRecorder {
	partial class TimeSpanEdit {
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
			this.components = new System.ComponentModel.Container();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnDown = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.btnUp = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnDown);
			this.panel1.Controls.Add(this.btnUp);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(80, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(20, 20);
			this.panel1.TabIndex = 0;
			// 
			// btnDown
			// 
			this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDown.BackgroundImage = global::AlbumRecorder.Properties.Resources.imgDown;
			this.btnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnDown.Location = new System.Drawing.Point(5, 10);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(10, 10);
			this.btnDown.TabIndex = 1;
			this.btnDown.UseVisualStyleBackColor = true;
			this.btnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnDown_MouseDown);
			this.btnDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(80, 20);
			this.textBox1.TabIndex = 1;
			this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
			this.textBox1.Validated += new System.EventHandler(this.textBox1_Validated);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// btnUp
			// 
			this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUp.BackgroundImage = global::AlbumRecorder.Properties.Resources.imgUp;
			this.btnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnUp.Location = new System.Drawing.Point(5, 0);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(10, 10);
			this.btnUp.TabIndex = 0;
			this.btnUp.UseVisualStyleBackColor = true;
			this.btnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseDown);
			this.btnUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
			// 
			// TimeSpanEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.panel1);
			this.Name = "TimeSpanEdit";
			this.Size = new System.Drawing.Size(100, 20);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Timer timer1;
	}
}
