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
			this.SS_Width = new SlickControls.SlickSlider();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.slickSpacer3 = new SlickControls.SlickSpacer();
			this.label3 = new System.Windows.Forms.Label();
			this.slickIcon1 = new SlickControls.SlickIcon();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB_100)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PB_512)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.slickIcon1)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 6;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer3, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer2, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.PB_100, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.PB_512, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer1, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.SS_Width, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label3, 5, 2);
			this.tableLayoutPanel1.Controls.Add(this.slickIcon1, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 1);
			this.tableLayoutPanel1.MaximumSize = new System.Drawing.Size(400, 999);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 110);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// slickSpacer2
			// 
			this.slickSpacer2.Dock = System.Windows.Forms.DockStyle.Left;
			this.slickSpacer2.Location = new System.Drawing.Point(331, 10);
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
			this.PB_100.Location = new System.Drawing.Point(305, 27);
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
			this.PB_512.Location = new System.Drawing.Point(342, 11);
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
			this.slickSpacer1.Location = new System.Drawing.Point(294, 10);
			this.slickSpacer1.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(1, 50);
			this.slickSpacer1.TabIndex = 3;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// SS_Width
			// 
			this.SS_Width.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SS_Width.AnimatedValue = 0;
			this.tableLayoutPanel1.SetColumnSpan(this.SS_Width, 3);
			this.SS_Width.Cursor = System.Windows.Forms.Cursors.Hand;
			this.SS_Width.FromValue = 0.1D;
			this.SS_Width.Location = new System.Drawing.Point(55, 74);
			this.SS_Width.MaxValue = 10D;
			this.SS_Width.MinValue = 0.1D;
			this.SS_Width.Name = "SS_Width";
			this.SS_Width.Padding = new System.Windows.Forms.Padding(14, 8, 14, 8);
			this.SS_Width.Percentage = 0D;
			this.SS_Width.PercFrom = 0D;
			this.SS_Width.PercTo = 0D;
			this.SS_Width.ShowValues = false;
			this.SS_Width.Size = new System.Drawing.Size(273, 33);
			this.SS_Width.SliderStyle = SlickControls.SliderStyle.SingleHorizontal;
			this.SS_Width.TabIndex = 5;
			this.SS_Width.TargetAnimationValue = 0;
			this.SS_Width.ToValue = 0.1D;
			this.SS_Width.Value = 0.1D;
			this.SS_Width.ValueOutput = null;
			this.SS_Width.ValuesChanged += new System.EventHandler(this.SS_Width_ValuesChanged);
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "png";
			this.openFileDialog.Filter = "PNG File|*.png";
			this.openFileDialog.Title = "Select Image";
			// 
			// slickSpacer3
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.slickSpacer3, 6);
			this.slickSpacer3.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer3.Location = new System.Drawing.Point(10, 70);
			this.slickSpacer3.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.slickSpacer3.Name = "slickSpacer3";
			this.slickSpacer3.Size = new System.Drawing.Size(380, 1);
			this.slickSpacer3.TabIndex = 6;
			this.slickSpacer3.TabStop = false;
			this.slickSpacer3.Text = "slickSpacer3";
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(335, 84);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Width:";
			// 
			// slickIcon1
			// 
			this.slickIcon1.ActiveColor = null;
			this.slickIcon1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.slickIcon1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.slickIcon1.Image = global::ThumbnailMaker.Properties.Resources.I_Size;
			this.slickIcon1.Location = new System.Drawing.Point(18, 82);
			this.slickIcon1.Margin = new System.Windows.Forms.Padding(10);
			this.slickIcon1.MinimumIconSize = 18;
			this.slickIcon1.Name = "slickIcon1";
			this.slickIcon1.Size = new System.Drawing.Size(16, 16);
			this.slickIcon1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.slickIcon1.TabIndex = 7;
			this.slickIcon1.TabStop = false;
			this.slickIcon1.Click += new System.EventHandler(this.slickIcon1_Click);
			// 
			// LaneOptionControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "LaneOptionControl";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(402, 112);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB_100)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PB_512)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.slickIcon1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

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
		private SlickControls.SlickSlider SS_Width;
		private SlickControls.SlickSpacer slickSpacer3;
		private System.Windows.Forms.Label label3;
		private SlickControls.SlickIcon slickIcon1;
	}
}
