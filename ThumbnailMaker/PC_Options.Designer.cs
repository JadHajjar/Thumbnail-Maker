﻿namespace ThumbnailMaker
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
			this.TLP = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.DD_Font = new SlickControls.SlickDropdown();
			this.CB_LHT = new SlickControls.SlickCheckbox();
			this.TB_ExportFolder = new SlickControls.SlickPathTextBox();
			this.B_Theme = new SlickControls.SlickButton();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(198, 26);
			this.base_Text.Text = "Thumbnail Maker - Options";
			// 
			// TLP
			// 
			this.TLP.AutoSize = true;
			this.TLP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP.ColumnCount = 4;
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.TLP.Location = new System.Drawing.Point(0, 0);
			this.TLP.Margin = new System.Windows.Forms.Padding(0);
			this.TLP.Name = "TLP";
			this.TLP.Padding = new System.Windows.Forms.Padding(15, 10, 15, 0);
			this.TLP.RowCount = 8;
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.Size = new System.Drawing.Size(30, 10);
			this.TLP.TabIndex = 8;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
			this.tableLayoutPanel2.Controls.Add(this.slickSpacer1, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.DD_Font, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.CB_LHT, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.TB_ExportFolder, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.B_Theme, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.label1, 1, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 30);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(908, 168);
			this.tableLayoutPanel2.TabIndex = 13;
			// 
			// slickSpacer1
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.slickSpacer1, 2);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(3, 164);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(902, 1);
			this.slickSpacer1.TabIndex = 0;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// DD_Font
			// 
			this.DD_Font.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.DD_Font.Conversion = null;
			this.DD_Font.EnterTriggersClick = false;
			this.DD_Font.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DD_Font.Image = ((System.Drawing.Image)(resources.GetObject("DD_Font.Image")));
			this.DD_Font.Items = null;
			this.DD_Font.LabelText = "Road Size Font";
			this.DD_Font.Location = new System.Drawing.Point(18, 18);
			this.DD_Font.Margin = new System.Windows.Forms.Padding(18, 18, 18, 0);
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
			this.DD_Font.Size = new System.Drawing.Size(417, 40);
			this.DD_Font.TabIndex = 1;
			this.DD_Font.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.DD_Font.Validation = SlickControls.ValidationType.None;
			this.DD_Font.ValidationCustom = null;
			this.DD_Font.ValidationRegex = "";
			this.DD_Font.TextChanged += new System.EventHandler(this.DD_Font_TextChanged);
			// 
			// CB_LHT
			// 
			this.CB_LHT.ActiveColor = null;
			this.CB_LHT.AutoSize = true;
			this.CB_LHT.Center = false;
			this.CB_LHT.Checked = false;
			this.CB_LHT.CheckedText = null;
			this.CB_LHT.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_LHT.DefaultValue = false;
			this.CB_LHT.HideText = false;
			this.CB_LHT.IconSize = 16;
			this.CB_LHT.Image = ((System.Drawing.Image)(resources.GetObject("CB_LHT.Image")));
			this.CB_LHT.Location = new System.Drawing.Point(20, 132);
			this.CB_LHT.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
			this.CB_LHT.Name = "CB_LHT";
			this.CB_LHT.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.CB_LHT.Size = new System.Drawing.Size(127, 26);
			this.CB_LHT.TabIndex = 15;
			this.CB_LHT.Text = "Left-Hand Traffic";
			this.CB_LHT.UncheckedText = null;
			this.CB_LHT.CheckChanged += new System.EventHandler(this.CB_LHT_CheckChanged);
			// 
			// TB_ExportFolder
			// 
			this.TB_ExportFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_ExportFolder.EnterTriggersClick = false;
			this.TB_ExportFolder.FileExtensions = new string[0];
			this.TB_ExportFolder.Folder = true;
			this.TB_ExportFolder.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_ExportFolder.Image = ((System.Drawing.Image)(resources.GetObject("TB_ExportFolder.Image")));
			this.TB_ExportFolder.LabelText = "Custom Export Folder";
			this.TB_ExportFolder.Location = new System.Drawing.Point(18, 76);
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
			this.TB_ExportFolder.Size = new System.Drawing.Size(417, 35);
			this.TB_ExportFolder.TabIndex = 3;
			this.TB_ExportFolder.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_ExportFolder.Validation = SlickControls.ValidationType.Number;
			this.TB_ExportFolder.ValidationCustom = null;
			this.TB_ExportFolder.ValidationRegex = "";
			this.TB_ExportFolder.TextChanged += new System.EventHandler(this.TB_ExportFolder_TextChanged);
			// 
			// B_Theme
			// 
			this.B_Theme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Theme.ColorShade = null;
			this.B_Theme.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Theme.IconSize = 16;
			this.B_Theme.Image = ((System.Drawing.Image)(resources.GetObject("B_Theme.Image")));
			this.B_Theme.Location = new System.Drawing.Point(813, 28);
			this.B_Theme.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.B_Theme.Name = "B_Theme";
			this.B_Theme.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Theme.Size = new System.Drawing.Size(85, 27);
			this.B_Theme.SpaceTriggersClick = true;
			this.B_Theme.TabIndex = 14;
			this.B_Theme.Text = "Theme & UI Scaling";
			this.B_Theme.Click += new System.EventHandler(this.B_Theme_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(709, 128);
			this.label1.Margin = new System.Windows.Forms.Padding(7, 7, 18, 7);
			this.label1.Name = "label1";
			this.tableLayoutPanel2.SetRowSpan(this.label1, 2);
			this.label1.Size = new System.Drawing.Size(181, 26);
			this.label1.TabIndex = 2;
			this.label1.Text = "Right-click on a color to reset it.\r\nMiddle-click on an icon to clear it.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.TLP);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(5, 198);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(908, 427);
			this.panel1.TabIndex = 14;
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.TLP;
			this.slickScroll1.Location = new System.Drawing.Point(907, 198);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(6, 427);
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 15;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			// 
			// PC_Options
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.slickScroll1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tableLayoutPanel2);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_Options";
			this.Size = new System.Drawing.Size(918, 630);
			this.Text = "Thumbnail Maker - Options";
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.Controls.SetChildIndex(this.tableLayoutPanel2, 0);
			this.Controls.SetChildIndex(this.panel1, 0);
			this.Controls.SetChildIndex(this.slickScroll1, 0);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel TLP;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private SlickControls.SlickSpacer slickSpacer1;
		private SlickControls.SlickDropdown DD_Font;
		private System.Windows.Forms.Label label1;
		private SlickControls.SlickPathTextBox TB_ExportFolder;
		private SlickControls.SlickButton B_Theme;
		private SlickControls.SlickCheckbox CB_LHT;
		private System.Windows.Forms.Panel panel1;
		private SlickControls.SlickScroll slickScroll1;
	}
}