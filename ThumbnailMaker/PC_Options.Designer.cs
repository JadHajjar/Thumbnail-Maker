namespace ThumbnailMaker
{
	partial class PC_Options
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PC_Options));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.TB_ExportFolder = new SlickControls.SlickPathTextBox();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.DD_Font = new SlickControls.SlickDropdown();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(198, 26);
			this.base_Text.Text = "Thumbnail Maker - Options";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 113);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(15, 10, 15, 0);
			this.tableLayoutPanel1.RowCount = 8;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(908, 512);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.Controls.Add(this.TB_ExportFolder, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.slickSpacer1, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.DD_Font, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label1, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 30);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(908, 83);
			this.tableLayoutPanel2.TabIndex = 13;
			// 
			// TB_ExportFolder
			// 
			this.TB_ExportFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_ExportFolder.EnterTriggersClick = false;
			this.TB_ExportFolder.FileExtensions = new string[0];
			this.TB_ExportFolder.Folder = true;
			this.TB_ExportFolder.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_ExportFolder.Image = ((System.Drawing.Image)(resources.GetObject("TB_ExportFolder.Image")));
			this.TB_ExportFolder.LabelText = "Custom Export Folder";
			this.TB_ExportFolder.Location = new System.Drawing.Point(320, 23);
			this.TB_ExportFolder.Margin = new System.Windows.Forms.Padding(18);
			this.TB_ExportFolder.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_ExportFolder.MaxLength = 32767;
			this.TB_ExportFolder.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_ExportFolder.Name = "TB_ExportFolder";
			this.TB_ExportFolder.Password = false;
			this.TB_ExportFolder.Placeholder = "Default is the Blank Road Generator\'s app data folder";
			this.TB_ExportFolder.ReadOnly = false;
			this.TB_ExportFolder.Required = false;
			this.TB_ExportFolder.SelectAllOnFocus = false;
			this.TB_ExportFolder.SelectedText = "";
			this.TB_ExportFolder.SelectionLength = 0;
			this.TB_ExportFolder.SelectionStart = 0;
			this.TB_ExportFolder.Size = new System.Drawing.Size(266, 35);
			this.TB_ExportFolder.TabIndex = 3;
			this.TB_ExportFolder.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_ExportFolder.Validation = SlickControls.ValidationType.Number;
			this.TB_ExportFolder.ValidationCustom = null;
			this.TB_ExportFolder.ValidationRegex = "";
			// 
			// slickSpacer1
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.slickSpacer1, 3);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(3, 79);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(902, 1);
			this.slickSpacer1.TabIndex = 0;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// DD_Font
			// 
			this.DD_Font.Conversion = null;
			this.DD_Font.EnterTriggersClick = false;
			this.DD_Font.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DD_Font.Image = ((System.Drawing.Image)(resources.GetObject("DD_Font.Image")));
			this.DD_Font.Items = null;
			this.DD_Font.LabelText = "Road Size Font";
			this.DD_Font.Location = new System.Drawing.Point(18, 18);
			this.DD_Font.Margin = new System.Windows.Forms.Padding(18);
			this.DD_Font.MaximumSize = new System.Drawing.Size(9999, 0);
			this.DD_Font.MaxLength = 32767;
			this.DD_Font.MinimumSize = new System.Drawing.Size(50, 0);
			this.DD_Font.Name = "DD_Font";
			this.DD_Font.Password = false;
			this.DD_Font.Placeholder = "Font used for the road\'s size";
			this.DD_Font.ReadOnly = false;
			this.DD_Font.Required = false;
			this.DD_Font.SelectAllOnFocus = false;
			this.DD_Font.SelectedItem = null;
			this.DD_Font.SelectedText = "";
			this.DD_Font.SelectionLength = 0;
			this.DD_Font.SelectionStart = 0;
			this.DD_Font.Size = new System.Drawing.Size(266, 40);
			this.DD_Font.TabIndex = 1;
			this.DD_Font.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.DD_Font.Validation = SlickControls.ValidationType.None;
			this.DD_Font.ValidationCustom = null;
			this.DD_Font.ValidationRegex = "";
			this.DD_Font.TextChanged += new System.EventHandler(this.DD_Font_TextChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(709, 43);
			this.label1.Margin = new System.Windows.Forms.Padding(7, 7, 18, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(181, 26);
			this.label1.TabIndex = 2;
			this.label1.Text = "Right-click on a color to reset it.\r\nMiddle-click on an icon to clear it.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// PC_Options
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.tableLayoutPanel2);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_Options";
			this.Size = new System.Drawing.Size(918, 630);
			this.Text = "Thumbnail Maker - Options";
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel2, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private SlickControls.SlickSpacer slickSpacer1;
		private SlickControls.SlickDropdown DD_Font;
		private System.Windows.Forms.Label label1;
		private SlickControls.SlickPathTextBox TB_ExportFolder;
	}
}