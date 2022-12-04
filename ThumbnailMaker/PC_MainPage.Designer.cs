namespace ThumbnailMaker
{
	partial class PC_MainPage
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
			Extensions.FormDesign.DesignChanged -= FormDesign_DesignChanged;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PC_MainPage));
			this.TLP_Main = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.TLP_Right = new System.Windows.Forms.TableLayoutPanel();
			this.RCC = new ThumbnailMaker.Controls.RoadConfigContainer();
			this.PB = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.B_CopyDesc = new SlickControls.SlickButton();
			this.B_Export = new SlickControls.SlickButton();
			this.B_SaveThumb = new SlickControls.SlickButton();
			this.L_RoadName = new System.Windows.Forms.Label();
			this.B_CopyName = new SlickControls.SlickButton();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.slickGroupBox1 = new SlickControls.SlickGroupBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.RB_Canada = new SlickControls.SlickRadioButton();
			this.RB_Europe = new SlickControls.SlickRadioButton();
			this.RB_USA = new SlickControls.SlickRadioButton();
			this.groupBox2 = new SlickControls.SlickGroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.RB_Road = new SlickControls.SlickRadioButton();
			this.RB_FlatRoad = new SlickControls.SlickRadioButton();
			this.RB_Pedestrian = new SlickControls.SlickRadioButton();
			this.RB_Highway = new SlickControls.SlickRadioButton();
			this.TB_Size = new SlickControls.SlickTextBox();
			this.TB_BufferSize = new SlickControls.SlickTextBox();
			this.TB_SpeedLimit = new SlickControls.SlickTextBox();
			this.TB_CustomText = new SlickControls.SlickTextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.P_Lanes = new ThumbnailMaker.Controls.RoadLaneContainer();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer3 = new SlickControls.SlickSpacer();
			this.B_FlipLanes = new SlickControls.SlickButton();
			this.B_DuplicateFlip = new SlickControls.SlickButton();
			this.B_AddLane = new SlickControls.SlickButton();
			this.B_ClearLines = new SlickControls.SlickButton();
			this.B_Options = new SlickControls.SlickButton();
			this.TB_RoadName = new SlickControls.SlickTextBox();
			this.TLP_Main.SuspendLayout();
			this.TLP_Right.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.slickGroupBox1.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_Text
			// 
			this.base_Text.Size = new System.Drawing.Size(130, 26);
			this.base_Text.Text = "Thumbnail Maker";
			// 
			// TLP_Main
			// 
			this.TLP_Main.ColumnCount = 5;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 276F));
			this.TLP_Main.Controls.Add(this.slickSpacer2, 3, 0);
			this.TLP_Main.Controls.Add(this.TLP_Right, 4, 0);
			this.TLP_Main.Controls.Add(this.tableLayoutPanel6, 0, 0);
			this.TLP_Main.Controls.Add(this.panel1, 0, 4);
			this.TLP_Main.Controls.Add(this.slickSpacer1, 0, 3);
			this.TLP_Main.Controls.Add(this.tableLayoutPanel4, 0, 2);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(5, 30);
			this.TLP_Main.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
			this.TLP_Main.RowCount = 5;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Main.Size = new System.Drawing.Size(1176, 569);
			this.TLP_Main.TabIndex = 8;
			// 
			// slickSpacer2
			// 
			this.slickSpacer2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.slickSpacer2.Location = new System.Drawing.Point(894, 3);
			this.slickSpacer2.Name = "slickSpacer2";
			this.TLP_Main.SetRowSpan(this.slickSpacer2, 5);
			this.slickSpacer2.Size = new System.Drawing.Size(1, 563);
			this.slickSpacer2.TabIndex = 18;
			this.slickSpacer2.TabStop = false;
			this.slickSpacer2.Text = "slickSpacer2";
			// 
			// TLP_Right
			// 
			this.TLP_Right.ColumnCount = 2;
			this.TLP_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Right.Controls.Add(this.RCC, 0, 4);
			this.TLP_Right.Controls.Add(this.PB, 0, 2);
			this.TLP_Right.Controls.Add(this.tableLayoutPanel2, 0, 3);
			this.TLP_Right.Controls.Add(this.L_RoadName, 1, 0);
			this.TLP_Right.Controls.Add(this.B_CopyName, 0, 0);
			this.TLP_Right.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Right.Location = new System.Drawing.Point(900, 0);
			this.TLP_Right.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Right.Name = "TLP_Right";
			this.TLP_Right.RowCount = 5;
			this.TLP_Main.SetRowSpan(this.TLP_Right, 5);
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Right.Size = new System.Drawing.Size(276, 569);
			this.TLP_Right.TabIndex = 17;
			// 
			// RCC
			// 
			this.RCC.AutoSize = true;
			this.TLP_Right.SetColumnSpan(this.RCC, 2);
			this.RCC.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RCC.Location = new System.Drawing.Point(0, 374);
			this.RCC.Margin = new System.Windows.Forms.Padding(0);
			this.RCC.Name = "RCC";
			this.RCC.Size = new System.Drawing.Size(276, 195);
			this.RCC.TabIndex = 16;
			this.RCC.LoadConfiguration += new System.EventHandler<ThumbnailMaker.Domain.RoadInfo>(this.RCC_LoadConfiguration);
			// 
			// PB
			// 
			this.PB.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.TLP_Right.SetColumnSpan(this.PB, 2);
			this.PB.Location = new System.Drawing.Point(15, 46);
			this.PB.Margin = new System.Windows.Forms.Padding(10, 10, 0, 0);
			this.PB.Name = "PB";
			this.PB.Size = new System.Drawing.Size(256, 256);
			this.PB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PB.TabIndex = 2;
			this.PB.TabStop = false;
			this.PB.Click += new System.EventHandler(this.PB_Click);
			this.PB.MouseEnter += new System.EventHandler(this.TB_Name_TextChanged);
			this.PB.MouseLeave += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.TLP_Right.SetColumnSpan(this.tableLayoutPanel2, 2);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.B_CopyDesc, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.B_Export, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.B_SaveThumb, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 305);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(270, 66);
			this.tableLayoutPanel2.TabIndex = 15;
			// 
			// B_CopyDesc
			// 
			this.B_CopyDesc.ColorShade = null;
			this.B_CopyDesc.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_CopyDesc.IconSize = 16;
			this.B_CopyDesc.Image = ((System.Drawing.Image)(resources.GetObject("B_CopyDesc.Image")));
			this.B_CopyDesc.Location = new System.Drawing.Point(3, 3);
			this.B_CopyDesc.Name = "B_CopyDesc";
			this.B_CopyDesc.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_CopyDesc.Size = new System.Drawing.Size(125, 27);
			this.B_CopyDesc.SpaceTriggersClick = true;
			this.B_CopyDesc.TabIndex = 6;
			this.B_CopyDesc.Text = "Copy Desc.";
			this.B_CopyDesc.Click += new System.EventHandler(this.B_CopyDesc_Click);
			// 
			// B_Export
			// 
			this.B_Export.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Export.ColorShade = null;
			this.tableLayoutPanel2.SetColumnSpan(this.B_Export, 2);
			this.B_Export.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Export.IconSize = 16;
			this.B_Export.Image = ((System.Drawing.Image)(resources.GetObject("B_Export.Image")));
			this.B_Export.Location = new System.Drawing.Point(3, 36);
			this.B_Export.Name = "B_Export";
			this.B_Export.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_Export.Size = new System.Drawing.Size(264, 27);
			this.B_Export.SpaceTriggersClick = true;
			this.B_Export.TabIndex = 5;
			this.B_Export.Text = "Export configuration to Road Builder";
			this.B_Export.Click += new System.EventHandler(this.B_Export_Click);
			// 
			// B_SaveThumb
			// 
			this.B_SaveThumb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.B_SaveThumb.ColorShade = null;
			this.B_SaveThumb.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_SaveThumb.IconSize = 16;
			this.B_SaveThumb.Image = ((System.Drawing.Image)(resources.GetObject("B_SaveThumb.Image")));
			this.B_SaveThumb.Location = new System.Drawing.Point(167, 3);
			this.B_SaveThumb.Name = "B_SaveThumb";
			this.B_SaveThumb.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_SaveThumb.Size = new System.Drawing.Size(100, 27);
			this.B_SaveThumb.SpaceTriggersClick = true;
			this.B_SaveThumb.TabIndex = 5;
			this.B_SaveThumb.Text = "Save Thumb.";
			this.B_SaveThumb.Click += new System.EventHandler(this.B_Save_Click);
			// 
			// L_RoadName
			// 
			this.L_RoadName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_RoadName.AutoSize = true;
			this.L_RoadName.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.L_RoadName.Location = new System.Drawing.Point(46, 11);
			this.L_RoadName.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
			this.L_RoadName.Name = "L_RoadName";
			this.L_RoadName.Size = new System.Drawing.Size(36, 13);
			this.L_RoadName.TabIndex = 14;
			this.L_RoadName.Tag = "NoMouseDown";
			this.L_RoadName.Text = "Lanes";
			this.L_RoadName.Click += new System.EventHandler(this.L_RoadName_Click);
			this.L_RoadName.MouseEnter += new System.EventHandler(this.L_RoadName_MouseEnter);
			this.L_RoadName.MouseLeave += new System.EventHandler(this.L_RoadName_MouseLeave);
			// 
			// B_CopyName
			// 
			this.B_CopyName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_CopyName.ColorShade = null;
			this.B_CopyName.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_CopyName.HandleUiScale = false;
			this.B_CopyName.IconSize = 16;
			this.B_CopyName.Image = ((System.Drawing.Image)(resources.GetObject("B_CopyName.Image")));
			this.B_CopyName.Location = new System.Drawing.Point(3, 3);
			this.B_CopyName.Name = "B_CopyName";
			this.B_CopyName.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_CopyName.Size = new System.Drawing.Size(30, 30);
			this.B_CopyName.SpaceTriggersClick = true;
			this.B_CopyName.TabIndex = 14;
			this.B_CopyName.Click += new System.EventHandler(this.B_CopyRoadName_Click);
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel6.ColumnCount = 4;
			this.TLP_Main.SetColumnSpan(this.tableLayoutPanel6, 3);
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.77778F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.77778F));
			this.tableLayoutPanel6.Controls.Add(this.slickGroupBox1, 1, 0);
			this.tableLayoutPanel6.Controls.Add(this.groupBox2, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.TB_Size, 2, 0);
			this.tableLayoutPanel6.Controls.Add(this.TB_BufferSize, 3, 0);
			this.tableLayoutPanel6.Controls.Add(this.TB_SpeedLimit, 2, 1);
			this.tableLayoutPanel6.Controls.Add(this.TB_CustomText, 3, 1);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(15, 0);
			this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 3;
			this.TLP_Main.SetRowSpan(this.tableLayoutPanel6, 2);
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.Size = new System.Drawing.Size(875, 155);
			this.tableLayoutPanel6.TabIndex = 13;
			// 
			// slickGroupBox1
			// 
			this.slickGroupBox1.AutoSize = true;
			this.slickGroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.slickGroupBox1.Controls.Add(this.tableLayoutPanel5);
			this.slickGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickGroupBox1.Icon = ((System.Drawing.Image)(resources.GetObject("slickGroupBox1.Icon")));
			this.slickGroupBox1.Location = new System.Drawing.Point(197, 3);
			this.slickGroupBox1.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.slickGroupBox1.Name = "slickGroupBox1";
			this.tableLayoutPanel6.SetRowSpan(this.slickGroupBox1, 3);
			this.slickGroupBox1.Size = new System.Drawing.Size(181, 117);
			this.slickGroupBox1.TabIndex = 16;
			this.slickGroupBox1.TabStop = false;
			this.slickGroupBox1.Text = "Region";
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel5.ColumnCount = 1;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel5.Controls.Add(this.RB_Canada, 0, 2);
			this.tableLayoutPanel5.Controls.Add(this.RB_Europe, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.RB_USA, 0, 1);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 18);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 2;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.Size = new System.Drawing.Size(175, 96);
			this.tableLayoutPanel5.TabIndex = 2;
			// 
			// RB_Canada
			// 
			this.RB_Canada.ActiveColor = null;
			this.RB_Canada.AutoSize = true;
			this.RB_Canada.Center = false;
			this.RB_Canada.Checked = false;
			this.RB_Canada.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RB_Canada.Data = null;
			this.RB_Canada.EnterTriggersClick = false;
			this.RB_Canada.HideText = false;
			this.RB_Canada.IconSize = 16;
			this.RB_Canada.Image = ((System.Drawing.Image)(resources.GetObject("RB_Canada.Image")));
			this.RB_Canada.Location = new System.Drawing.Point(3, 67);
			this.RB_Canada.Name = "RB_Canada";
			this.RB_Canada.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.RB_Canada.Size = new System.Drawing.Size(81, 26);
			this.RB_Canada.SpaceTriggersClick = true;
			this.RB_Canada.TabIndex = 2;
			this.RB_Canada.Text = "Canada";
			this.RB_Canada.CheckChanged += new System.EventHandler(this.RB_CheckedChanged);
			// 
			// RB_Europe
			// 
			this.RB_Europe.ActiveColor = null;
			this.RB_Europe.AutoSize = true;
			this.RB_Europe.Center = false;
			this.RB_Europe.Checked = true;
			this.RB_Europe.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RB_Europe.Data = null;
			this.RB_Europe.EnterTriggersClick = false;
			this.RB_Europe.HideText = false;
			this.RB_Europe.IconSize = 16;
			this.RB_Europe.Image = ((System.Drawing.Image)(resources.GetObject("RB_Europe.Image")));
			this.RB_Europe.Location = new System.Drawing.Point(3, 3);
			this.RB_Europe.Name = "RB_Europe";
			this.RB_Europe.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.RB_Europe.Size = new System.Drawing.Size(78, 26);
			this.RB_Europe.SpaceTriggersClick = true;
			this.RB_Europe.TabIndex = 0;
			this.RB_Europe.Text = "Europe";
			this.RB_Europe.CheckChanged += new System.EventHandler(this.RB_CheckedChanged);
			// 
			// RB_USA
			// 
			this.RB_USA.ActiveColor = null;
			this.RB_USA.AutoSize = true;
			this.RB_USA.Center = false;
			this.RB_USA.Checked = false;
			this.RB_USA.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RB_USA.Data = null;
			this.RB_USA.EnterTriggersClick = false;
			this.RB_USA.HideText = false;
			this.RB_USA.IconSize = 16;
			this.RB_USA.Image = ((System.Drawing.Image)(resources.GetObject("RB_USA.Image")));
			this.RB_USA.Location = new System.Drawing.Point(3, 35);
			this.RB_USA.Name = "RB_USA";
			this.RB_USA.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.RB_USA.Size = new System.Drawing.Size(64, 26);
			this.RB_USA.SpaceTriggersClick = true;
			this.RB_USA.TabIndex = 1;
			this.RB_USA.Text = "USA";
			this.RB_USA.CheckChanged += new System.EventHandler(this.RB_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.AutoSize = true;
			this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox2.Controls.Add(this.tableLayoutPanel3);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Icon = ((System.Drawing.Image)(resources.GetObject("groupBox2.Icon")));
			this.groupBox2.Location = new System.Drawing.Point(3, 3);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.groupBox2.Name = "groupBox2";
			this.tableLayoutPanel6.SetRowSpan(this.groupBox2, 3);
			this.groupBox2.Size = new System.Drawing.Size(181, 149);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Type";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.RB_Road, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.RB_FlatRoad, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.RB_Pedestrian, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.RB_Highway, 0, 2);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 18);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 4;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(175, 128);
			this.tableLayoutPanel3.TabIndex = 2;
			// 
			// RB_Road
			// 
			this.RB_Road.ActiveColor = null;
			this.RB_Road.AutoSize = true;
			this.RB_Road.Center = false;
			this.RB_Road.Checked = true;
			this.RB_Road.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RB_Road.Data = null;
			this.RB_Road.EnterTriggersClick = false;
			this.RB_Road.HideText = false;
			this.RB_Road.IconSize = 16;
			this.RB_Road.Image = ((System.Drawing.Image)(resources.GetObject("RB_Road.Image")));
			this.RB_Road.Location = new System.Drawing.Point(3, 3);
			this.RB_Road.Name = "RB_Road";
			this.RB_Road.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.RB_Road.Size = new System.Drawing.Size(69, 26);
			this.RB_Road.SpaceTriggersClick = true;
			this.RB_Road.TabIndex = 0;
			this.RB_Road.Text = "Road";
			this.RB_Road.CheckChanged += new System.EventHandler(this.RB_CheckedChanged);
			// 
			// RB_FlatRoad
			// 
			this.RB_FlatRoad.ActiveColor = null;
			this.RB_FlatRoad.AutoSize = true;
			this.RB_FlatRoad.Center = false;
			this.RB_FlatRoad.Checked = false;
			this.RB_FlatRoad.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RB_FlatRoad.Data = null;
			this.RB_FlatRoad.Enabled = false;
			this.RB_FlatRoad.EnterTriggersClick = false;
			this.RB_FlatRoad.HideText = false;
			this.RB_FlatRoad.IconSize = 16;
			this.RB_FlatRoad.Image = ((System.Drawing.Image)(resources.GetObject("RB_FlatRoad.Image")));
			this.RB_FlatRoad.Location = new System.Drawing.Point(3, 35);
			this.RB_FlatRoad.Name = "RB_FlatRoad";
			this.RB_FlatRoad.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.RB_FlatRoad.Size = new System.Drawing.Size(90, 26);
			this.RB_FlatRoad.SpaceTriggersClick = true;
			this.RB_FlatRoad.TabIndex = 3;
			this.RB_FlatRoad.Text = "Flat Road";
			this.RB_FlatRoad.CheckChanged += new System.EventHandler(this.RB_CheckedChanged);
			// 
			// RB_Pedestrian
			// 
			this.RB_Pedestrian.ActiveColor = null;
			this.RB_Pedestrian.AutoSize = true;
			this.RB_Pedestrian.Center = false;
			this.RB_Pedestrian.Checked = false;
			this.RB_Pedestrian.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RB_Pedestrian.Data = null;
			this.RB_Pedestrian.Enabled = false;
			this.RB_Pedestrian.EnterTriggersClick = false;
			this.RB_Pedestrian.HideText = false;
			this.RB_Pedestrian.IconSize = 16;
			this.RB_Pedestrian.Image = ((System.Drawing.Image)(resources.GetObject("RB_Pedestrian.Image")));
			this.RB_Pedestrian.Location = new System.Drawing.Point(3, 99);
			this.RB_Pedestrian.Name = "RB_Pedestrian";
			this.RB_Pedestrian.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.RB_Pedestrian.Size = new System.Drawing.Size(96, 26);
			this.RB_Pedestrian.SpaceTriggersClick = true;
			this.RB_Pedestrian.TabIndex = 2;
			this.RB_Pedestrian.Text = "Pedestrian";
			// 
			// RB_Highway
			// 
			this.RB_Highway.ActiveColor = null;
			this.RB_Highway.AutoSize = true;
			this.RB_Highway.Center = false;
			this.RB_Highway.Checked = false;
			this.RB_Highway.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RB_Highway.Data = null;
			this.RB_Highway.Enabled = false;
			this.RB_Highway.EnterTriggersClick = false;
			this.RB_Highway.HideText = false;
			this.RB_Highway.IconSize = 16;
			this.RB_Highway.Image = ((System.Drawing.Image)(resources.GetObject("RB_Highway.Image")));
			this.RB_Highway.Location = new System.Drawing.Point(3, 67);
			this.RB_Highway.Name = "RB_Highway";
			this.RB_Highway.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.RB_Highway.Size = new System.Drawing.Size(86, 26);
			this.RB_Highway.SpaceTriggersClick = true;
			this.RB_Highway.TabIndex = 1;
			this.RB_Highway.Text = "Highway";
			this.RB_Highway.CheckChanged += new System.EventHandler(this.RB_CheckedChanged);
			// 
			// TB_Size
			// 
			this.TB_Size.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_Size.EnterTriggersClick = false;
			this.TB_Size.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Size.Image = ((System.Drawing.Image)(resources.GetObject("TB_Size.Image")));
			this.TB_Size.LabelText = "Road Size (m)";
			this.TB_Size.Location = new System.Drawing.Point(388, 10);
			this.TB_Size.Margin = new System.Windows.Forms.Padding(0, 10, 10, 15);
			this.TB_Size.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Size.MaxLength = 32767;
			this.TB_Size.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_Size.Name = "TB_Size";
			this.TB_Size.Password = false;
			this.TB_Size.Placeholder = "Size including buffer";
			this.TB_Size.ReadOnly = false;
			this.TB_Size.Required = false;
			this.TB_Size.SelectAllOnFocus = false;
			this.TB_Size.SelectedText = "";
			this.TB_Size.SelectionLength = 0;
			this.TB_Size.SelectionStart = 0;
			this.TB_Size.Size = new System.Drawing.Size(233, 35);
			this.TB_Size.TabIndex = 2;
			this.TB_Size.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_Size.Validation = SlickControls.ValidationType.Decimal;
			this.TB_Size.ValidationCustom = null;
			this.TB_Size.ValidationRegex = "";
			this.TB_Size.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// TB_BufferSize
			// 
			this.TB_BufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_BufferSize.EnterTriggersClick = false;
			this.TB_BufferSize.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_BufferSize.Image = ((System.Drawing.Image)(resources.GetObject("TB_BufferSize.Image")));
			this.TB_BufferSize.LabelText = "Buffer Size (m)";
			this.TB_BufferSize.Location = new System.Drawing.Point(631, 10);
			this.TB_BufferSize.Margin = new System.Windows.Forms.Padding(0, 10, 10, 15);
			this.TB_BufferSize.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_BufferSize.MaxLength = 32767;
			this.TB_BufferSize.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_BufferSize.Name = "TB_BufferSize";
			this.TB_BufferSize.Password = false;
			this.TB_BufferSize.Placeholder = "Space from the curb";
			this.TB_BufferSize.ReadOnly = false;
			this.TB_BufferSize.Required = false;
			this.TB_BufferSize.SelectAllOnFocus = false;
			this.TB_BufferSize.SelectedText = "";
			this.TB_BufferSize.SelectionLength = 0;
			this.TB_BufferSize.SelectionStart = 0;
			this.TB_BufferSize.Size = new System.Drawing.Size(234, 35);
			this.TB_BufferSize.TabIndex = 2;
			this.TB_BufferSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_BufferSize.Validation = SlickControls.ValidationType.Decimal;
			this.TB_BufferSize.ValidationCustom = null;
			this.TB_BufferSize.ValidationRegex = "";
			this.TB_BufferSize.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// TB_SpeedLimit
			// 
			this.TB_SpeedLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_SpeedLimit.EnterTriggersClick = false;
			this.TB_SpeedLimit.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_SpeedLimit.Image = ((System.Drawing.Image)(resources.GetObject("TB_SpeedLimit.Image")));
			this.TB_SpeedLimit.LabelText = "Speed Limit";
			this.TB_SpeedLimit.Location = new System.Drawing.Point(388, 60);
			this.TB_SpeedLimit.Margin = new System.Windows.Forms.Padding(0, 0, 10, 3);
			this.TB_SpeedLimit.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_SpeedLimit.MaxLength = 32767;
			this.TB_SpeedLimit.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_SpeedLimit.Name = "TB_SpeedLimit";
			this.TB_SpeedLimit.Password = false;
			this.TB_SpeedLimit.Placeholder = "Speed of car lanes";
			this.TB_SpeedLimit.ReadOnly = false;
			this.TB_SpeedLimit.Required = false;
			this.TB_SpeedLimit.SelectAllOnFocus = false;
			this.TB_SpeedLimit.SelectedText = "";
			this.TB_SpeedLimit.SelectionLength = 0;
			this.TB_SpeedLimit.SelectionStart = 0;
			this.TB_SpeedLimit.Size = new System.Drawing.Size(233, 35);
			this.TB_SpeedLimit.TabIndex = 2;
			this.TB_SpeedLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_SpeedLimit.Validation = SlickControls.ValidationType.Number;
			this.TB_SpeedLimit.ValidationCustom = null;
			this.TB_SpeedLimit.ValidationRegex = "";
			this.TB_SpeedLimit.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// TB_CustomText
			// 
			this.TB_CustomText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_CustomText.EnterTriggersClick = false;
			this.TB_CustomText.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_CustomText.Image = ((System.Drawing.Image)(resources.GetObject("TB_CustomText.Image")));
			this.TB_CustomText.LabelText = "Text";
			this.TB_CustomText.Location = new System.Drawing.Point(631, 60);
			this.TB_CustomText.Margin = new System.Windows.Forms.Padding(0, 0, 10, 3);
			this.TB_CustomText.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_CustomText.MaxLength = 32767;
			this.TB_CustomText.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_CustomText.Name = "TB_CustomText";
			this.TB_CustomText.Password = false;
			this.TB_CustomText.Placeholder = "Extra display text";
			this.TB_CustomText.ReadOnly = false;
			this.TB_CustomText.Required = false;
			this.TB_CustomText.SelectAllOnFocus = false;
			this.TB_CustomText.SelectedText = "";
			this.TB_CustomText.SelectionLength = 0;
			this.TB_CustomText.SelectionStart = 0;
			this.TB_CustomText.Size = new System.Drawing.Size(234, 35);
			this.TB_CustomText.TabIndex = 2;
			this.TB_CustomText.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_CustomText.Validation = SlickControls.ValidationType.None;
			this.TB_CustomText.ValidationCustom = null;
			this.TB_CustomText.ValidationRegex = "";
			this.TB_CustomText.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// panel1
			// 
			this.TLP_Main.SetColumnSpan(this.panel1, 3);
			this.panel1.Controls.Add(this.slickScroll1);
			this.panel1.Controls.Add(this.P_Lanes);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(15, 215);
			this.panel1.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(875, 354);
			this.panel1.TabIndex = 8;
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.P_Lanes;
			this.slickScroll1.Location = new System.Drawing.Point(861, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(14, 354);
			this.slickScroll1.Style = SlickControls.StyleType.Vertical;
			this.slickScroll1.TabIndex = 1;
			this.slickScroll1.TabStop = false;
			this.slickScroll1.Text = "slickScroll1";
			// 
			// P_Lanes
			// 
			this.P_Lanes.AllowDrop = true;
			this.P_Lanes.AutoSize = true;
			this.P_Lanes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Lanes.Location = new System.Drawing.Point(0, 0);
			this.P_Lanes.MinimumSize = new System.Drawing.Size(0, 22);
			this.P_Lanes.Name = "P_Lanes";
			this.P_Lanes.Size = new System.Drawing.Size(0, 22);
			this.P_Lanes.TabIndex = 0;
			// 
			// slickSpacer1
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer1, 3);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(18, 203);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(869, 1);
			this.slickSpacer1.TabIndex = 9;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel4.ColumnCount = 7;
			this.TLP_Main.SetColumnSpan(this.tableLayoutPanel4, 3);
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.slickSpacer3, 3, 0);
			this.tableLayoutPanel4.Controls.Add(this.B_FlipLanes, 5, 0);
			this.tableLayoutPanel4.Controls.Add(this.B_DuplicateFlip, 4, 0);
			this.tableLayoutPanel4.Controls.Add(this.B_AddLane, 6, 0);
			this.tableLayoutPanel4.Controls.Add(this.B_ClearLines, 2, 0);
			this.tableLayoutPanel4.Controls.Add(this.B_Options, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(15, 162);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 7, 0, 5);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(875, 33);
			this.tableLayoutPanel4.TabIndex = 2;
			// 
			// slickSpacer3
			// 
			this.slickSpacer3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.slickSpacer3.Location = new System.Drawing.Point(585, 3);
			this.slickSpacer3.Name = "slickSpacer3";
			this.slickSpacer3.Size = new System.Drawing.Size(1, 27);
			this.slickSpacer3.TabIndex = 19;
			this.slickSpacer3.TabStop = false;
			this.slickSpacer3.Text = "slickSpacer3";
			// 
			// B_FlipLanes
			// 
			this.B_FlipLanes.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_FlipLanes.ColorShade = null;
			this.B_FlipLanes.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_FlipLanes.IconSize = 16;
			this.B_FlipLanes.Image = global::ThumbnailMaker.Properties.Resources.I_2W;
			this.B_FlipLanes.Location = new System.Drawing.Point(690, 3);
			this.B_FlipLanes.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.B_FlipLanes.Name = "B_FlipLanes";
			this.B_FlipLanes.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_FlipLanes.Size = new System.Drawing.Size(81, 27);
			this.B_FlipLanes.SpaceTriggersClick = true;
			this.B_FlipLanes.TabIndex = 18;
			this.B_FlipLanes.Text = "Flip Lanes";
			this.B_FlipLanes.Click += new System.EventHandler(this.B_FlipLanes_Click);
			// 
			// B_DuplicateFlip
			// 
			this.B_DuplicateFlip.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_DuplicateFlip.ColorShade = null;
			this.B_DuplicateFlip.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_DuplicateFlip.IconSize = 16;
			this.B_DuplicateFlip.Image = ((System.Drawing.Image)(resources.GetObject("B_DuplicateFlip.Image")));
			this.B_DuplicateFlip.Location = new System.Drawing.Point(596, 3);
			this.B_DuplicateFlip.Margin = new System.Windows.Forms.Padding(5, 3, 10, 3);
			this.B_DuplicateFlip.Name = "B_DuplicateFlip";
			this.B_DuplicateFlip.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_DuplicateFlip.Size = new System.Drawing.Size(81, 27);
			this.B_DuplicateFlip.SpaceTriggersClick = true;
			this.B_DuplicateFlip.TabIndex = 17;
			this.B_DuplicateFlip.Text = "Duplicate Flip Lanes";
			this.B_DuplicateFlip.Click += new System.EventHandler(this.B_DuplicateFlip_Click);
			// 
			// B_AddLane
			// 
			this.B_AddLane.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_AddLane.ColorShade = null;
			this.B_AddLane.ColorStyle = Extensions.ColorStyle.Green;
			this.B_AddLane.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_AddLane.IconSize = 16;
			this.B_AddLane.Image = ((System.Drawing.Image)(resources.GetObject("B_AddLane.Image")));
			this.B_AddLane.Location = new System.Drawing.Point(784, 3);
			this.B_AddLane.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.B_AddLane.Name = "B_AddLane";
			this.B_AddLane.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_AddLane.Size = new System.Drawing.Size(81, 27);
			this.B_AddLane.SpaceTriggersClick = true;
			this.B_AddLane.TabIndex = 13;
			this.B_AddLane.Text = "Add Lane";
			this.B_AddLane.Click += new System.EventHandler(this.B_Add_Click);
			// 
			// B_ClearLines
			// 
			this.B_ClearLines.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_ClearLines.ColorShade = null;
			this.B_ClearLines.ColorStyle = Extensions.ColorStyle.Red;
			this.B_ClearLines.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ClearLines.IconSize = 16;
			this.B_ClearLines.Image = ((System.Drawing.Image)(resources.GetObject("B_ClearLines.Image")));
			this.B_ClearLines.Location = new System.Drawing.Point(495, 3);
			this.B_ClearLines.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
			this.B_ClearLines.Name = "B_ClearLines";
			this.B_ClearLines.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_ClearLines.Size = new System.Drawing.Size(81, 27);
			this.B_ClearLines.SpaceTriggersClick = true;
			this.B_ClearLines.TabIndex = 16;
			this.B_ClearLines.Text = "Clear Lanes";
			this.B_ClearLines.Click += new System.EventHandler(this.B_Clear_Click);
			// 
			// B_Options
			// 
			this.B_Options.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_Options.ColorShade = null;
			this.B_Options.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Options.IconSize = 16;
			this.B_Options.Image = ((System.Drawing.Image)(resources.GetObject("B_Options.Image")));
			this.B_Options.Location = new System.Drawing.Point(3, 3);
			this.B_Options.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.B_Options.Name = "B_Options";
			this.B_Options.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Options.Size = new System.Drawing.Size(85, 27);
			this.B_Options.SpaceTriggersClick = true;
			this.B_Options.TabIndex = 13;
			this.B_Options.Text = "Options";
			this.B_Options.Click += new System.EventHandler(this.B_Options_Click);
			// 
			// TB_RoadName
			// 
			this.TB_RoadName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_RoadName.EnterTriggersClick = false;
			this.TB_RoadName.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_RoadName.Image = null;
			this.TB_RoadName.LabelText = "Custom Road Name";
			this.TB_RoadName.Location = new System.Drawing.Point(477, 285);
			this.TB_RoadName.Margin = new System.Windows.Forms.Padding(0, 0, 10, 3);
			this.TB_RoadName.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_RoadName.MaxLength = 32767;
			this.TB_RoadName.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_RoadName.Name = "TB_RoadName";
			this.TB_RoadName.Password = false;
			this.TB_RoadName.Placeholder = "Replaces the generated road name";
			this.TB_RoadName.ReadOnly = false;
			this.TB_RoadName.Required = false;
			this.TB_RoadName.SelectAllOnFocus = false;
			this.TB_RoadName.SelectedText = "";
			this.TB_RoadName.SelectionLength = 0;
			this.TB_RoadName.SelectionStart = 0;
			this.TB_RoadName.Size = new System.Drawing.Size(233, 35);
			this.TB_RoadName.TabIndex = 13;
			this.TB_RoadName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_RoadName.Validation = SlickControls.ValidationType.Number;
			this.TB_RoadName.ValidationCustom = null;
			this.TB_RoadName.ValidationRegex = "";
			this.TB_RoadName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_RoadName_KeyDown);
			this.TB_RoadName.Leave += new System.EventHandler(this.TB_RoadName_Leave);
			this.TB_RoadName.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TB_RoadName_PreviewKeyDown);
			// 
			// PC_MainPage
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.TLP_Main);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Name = "PC_MainPage";
			this.Size = new System.Drawing.Size(1186, 604);
			this.Text = "Thumbnail Maker";
			this.Controls.SetChildIndex(this.TLP_Main, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.TLP_Right.ResumeLayout(false);
			this.TLP_Right.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.slickGroupBox1.ResumeLayout(false);
			this.slickGroupBox1.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel TLP_Main;
		private System.Windows.Forms.PictureBox PB;
		private SlickControls.SlickTextBox TB_Size;
		private System.Windows.Forms.Panel panel1;
		private SlickControls.SlickScroll slickScroll1;
		private ThumbnailMaker.Controls.RoadLaneContainer P_Lanes;
		private SlickControls.SlickButton B_AddLane;
		private SlickControls.SlickSpacer slickSpacer1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private SlickControls.SlickButton B_Options;
		private SlickControls.SlickGroupBox groupBox2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private SlickControls.SlickRadioButton RB_Road;
		private SlickControls.SlickRadioButton RB_Highway;
		private SlickControls.SlickButton B_SaveThumb;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private SlickControls.SlickGroupBox slickGroupBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private SlickControls.SlickRadioButton RB_Europe;
		private SlickControls.SlickRadioButton RB_USA;
		private SlickControls.SlickTextBox TB_SpeedLimit;
		private SlickControls.SlickTextBox TB_CustomText;
		private SlickControls.SlickRadioButton RB_Canada;
		private SlickControls.SlickButton B_CopyDesc;
		private System.Windows.Forms.TableLayoutPanel TLP_Right;
		private SlickControls.SlickButton B_CopyName;
		private System.Windows.Forms.Label L_RoadName;
		private SlickControls.SlickTextBox TB_BufferSize;
		private SlickControls.SlickButton B_ClearLines;
		private SlickControls.SlickButton B_FlipLanes;
		private SlickControls.SlickButton B_DuplicateFlip;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private SlickControls.SlickSpacer slickSpacer2;
		private SlickControls.SlickButton B_Export;
		private SlickControls.SlickRadioButton RB_Pedestrian;
		private SlickControls.SlickSpacer slickSpacer3;
		private Controls.RoadConfigContainer RCC;
		private SlickControls.SlickTextBox TB_RoadName;
		private SlickControls.SlickRadioButton RB_FlatRoad;
	}
}