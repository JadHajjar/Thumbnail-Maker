namespace ThumbnailMaker.Controls
{
	partial class LaneOptionControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.PB_100 = new SlickControls.SlickPictureBox();
			this.PB_512 = new SlickControls.SlickPictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB_100)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PB_512)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 6;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer2, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.PB_100, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.PB_512, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer1, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(403, 70);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// slickSpacer2
			// 
			this.slickSpacer2.Dock = System.Windows.Forms.DockStyle.Left;
			this.slickSpacer2.Location = new System.Drawing.Point(334, 10);
			this.slickSpacer2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this.slickSpacer2.Name = "slickSpacer2";
			this.slickSpacer2.Size = new System.Drawing.Size(1, 50);
			this.slickSpacer2.TabIndex = 4;
			this.slickSpacer2.TabStop = false;
			this.slickSpacer2.Text = "slickSpacer2";
			// 
			// PB_100
			// 
			this.PB_100.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.PB_100.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PB_100.Location = new System.Drawing.Point(308, 27);
			this.PB_100.Margin = new System.Windows.Forms.Padding(10);
			this.PB_100.Name = "PB_100";
			this.PB_100.Size = new System.Drawing.Size(16, 16);
			this.PB_100.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.PB_100.TabIndex = 0;
			this.PB_100.TabStop = false;
			this.PB_100.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_100_Paint);
			this.PB_100.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PB_100_MouseClick);
			// 
			// PB_512
			// 
			this.PB_512.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.PB_512.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PB_512.Location = new System.Drawing.Point(345, 11);
			this.PB_512.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
			this.PB_512.Name = "PB_512";
			this.PB_512.Size = new System.Drawing.Size(48, 48);
			this.PB_512.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PB_512.TabIndex = 0;
			this.PB_512.TabStop = false;
			this.PB_512.Paint += new System.Windows.Forms.PaintEventHandler(this.PB_512_Paint);
			this.PB_512.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PB_100_MouseClick);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBox1.Location = new System.Drawing.Point(10, 19);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
			this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(55, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// slickSpacer1
			// 
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Left;
			this.slickSpacer1.Location = new System.Drawing.Point(297, 10);
			this.slickSpacer1.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(1, 50);
			this.slickSpacer1.TabIndex = 3;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "png";
			this.openFileDialog.Filter = "PNG File|*.png";
			this.openFileDialog.Title = "Select Image";
			// 
			// LaneOptionControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "LaneOptionControl";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(405, 72);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB_100)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PB_512)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private SlickControls.SlickPictureBox PB_100;
		private SlickControls.SlickPictureBox PB_512;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private SlickControls.SlickSpacer slickSpacer2;
		private SlickControls.SlickSpacer slickSpacer1;
	}
}
