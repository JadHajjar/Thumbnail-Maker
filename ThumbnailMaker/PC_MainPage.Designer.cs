using ThumbnailMaker.Domain;

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
			this.roundedPanel1 = new SlickControls.RoundedPanel();
			this.RCC = new ThumbnailMaker.Controls.RoadConfigContainer();
			this.TLP_Right = new SlickControls.RoundedTableLayoutPanel();
			this.TB_RoadName = new SlickControls.SlickTextBox();
			this.TB_RoadDesc = new SlickControls.SlickTextBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer4 = new SlickControls.SlickSpacer();
			this.C_Warnings = new ThumbnailMaker.Controls.WarningsControl();
			this.B_ViewSavedRoads = new SlickControls.SlickButton();
			this.B_Export = new SlickControls.SlickButton();
			this.B_SaveThumb = new SlickControls.SlickButton();
			this.C_CurrentlyEditing = new ThumbnailMaker.Controls.CurrentlyEditingControl();
			this.L_RoadName = new System.Windows.Forms.Label();
			this.L_RoadDesc = new System.Windows.Forms.Label();
			this.B_EditDesc = new SlickControls.SlickButton();
			this.B_EditName = new SlickControls.SlickButton();
			this.PB = new System.Windows.Forms.PictureBox();
			this.FLP_Tags = new System.Windows.Forms.FlowLayoutPanel();
			this.L_NoTags = new System.Windows.Forms.Label();
			this.B_AddTag = new SlickControls.SlickButton();
			this.TLP_TopControls = new SlickControls.RoundedGroupTableLayoutPanel();
			this.TB_Size = new SlickControls.SlickTextBox();
			this.TB_BufferSize = new SlickControls.SlickTextBox();
			this.TB_CustomText = new SlickControls.SlickTextBox();
			this.TB_SpeedLimit = new SlickControls.SlickTextBox();
			this.CB_HighwayRules = new SlickControls.SlickCheckbox();
			this.CB_CanCrossLanes = new SlickControls.SlickCheckbox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.slickScroll1 = new SlickControls.SlickScroll();
			this.P_Lanes = new ThumbnailMaker.Controls.RoadLaneContainer();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.TLP_Buttons = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer3 = new SlickControls.SlickSpacer();
			this.B_FlipLanes = new SlickControls.SlickButton();
			this.B_DuplicateFlip = new SlickControls.SlickButton();
			this.B_AddLane = new SlickControls.SlickButton();
			this.B_ClearLines = new SlickControls.SlickButton();
			this.B_Options = new SlickControls.SlickButton();
			this.TLP_Main.SuspendLayout();
			this.roundedPanel1.SuspendLayout();
			this.TLP_Right.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB)).BeginInit();
			this.FLP_Tags.SuspendLayout();
			this.TLP_TopControls.SuspendLayout();
			this.panel1.SuspendLayout();
			this.TLP_Buttons.SuspendLayout();
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
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.Controls.Add(this.roundedPanel1, 4, 0);
			this.TLP_Main.Controls.Add(this.TLP_Right, 3, 0);
			this.TLP_Main.Controls.Add(this.TLP_TopControls, 0, 0);
			this.TLP_Main.Controls.Add(this.panel1, 0, 4);
			this.TLP_Main.Controls.Add(this.slickSpacer1, 0, 3);
			this.TLP_Main.Controls.Add(this.TLP_Buttons, 0, 2);
			this.TLP_Main.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Main.Location = new System.Drawing.Point(5, 30);
			this.TLP_Main.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Main.Name = "TLP_Main";
			this.TLP_Main.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.TLP_Main.RowCount = 5;
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.Size = new System.Drawing.Size(1181, 574);
			this.TLP_Main.TabIndex = 0;
			// 
			// roundedPanel1
			// 
			this.roundedPanel1.AddOutline = true;
			this.roundedPanel1.Controls.Add(this.RCC);
			this.roundedPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.roundedPanel1.Location = new System.Drawing.Point(1179, 3);
			this.roundedPanel1.Name = "roundedPanel1";
			this.TLP_Main.SetRowSpan(this.roundedPanel1, 5);
			this.roundedPanel1.Size = new System.Drawing.Size(0, 568);
			this.roundedPanel1.TabIndex = 19;
			this.roundedPanel1.Visible = false;
			// 
			// RCC
			// 
			this.RCC.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RCC.Location = new System.Drawing.Point(0, 0);
			this.RCC.Margin = new System.Windows.Forms.Padding(0);
			this.RCC.Name = "RCC";
			this.RCC.Size = new System.Drawing.Size(0, 568);
			this.RCC.TabIndex = 16;
			this.RCC.LoadConfiguration += new System.EventHandler<ThumbnailMaker.Domain.RoadInfo>(this.RCC_LoadConfiguration);
			// 
			// TLP_Right
			// 
			this.TLP_Right.AddOutline = true;
			this.TLP_Right.ColumnCount = 2;
			this.TLP_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Right.Controls.Add(this.TB_RoadName, 0, 1);
			this.TLP_Right.Controls.Add(this.TB_RoadDesc, 0, 3);
			this.TLP_Right.Controls.Add(this.tableLayoutPanel2, 0, 7);
			this.TLP_Right.Controls.Add(this.L_RoadName, 0, 2);
			this.TLP_Right.Controls.Add(this.L_RoadDesc, 0, 4);
			this.TLP_Right.Controls.Add(this.B_EditDesc, 1, 4);
			this.TLP_Right.Controls.Add(this.B_EditName, 1, 2);
			this.TLP_Right.Controls.Add(this.PB, 0, 0);
			this.TLP_Right.Controls.Add(this.FLP_Tags, 0, 5);
			this.TLP_Right.Controls.Add(this.B_AddTag, 1, 5);
			this.TLP_Right.Dock = System.Windows.Forms.DockStyle.Right;
			this.TLP_Right.Location = new System.Drawing.Point(899, 0);
			this.TLP_Right.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Right.Name = "TLP_Right";
			this.TLP_Right.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.TLP_Right.RowCount = 8;
			this.TLP_Main.SetRowSpan(this.TLP_Right, 5);
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.Size = new System.Drawing.Size(276, 574);
			this.TLP_Right.TabIndex = 17;
			// 
			// TB_RoadName
			// 
			this.TB_RoadName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TLP_Right.SetColumnSpan(this.TB_RoadName, 2);
			this.TB_RoadName.EnterTriggersClick = false;
			this.TB_RoadName.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_RoadName.Image = ((System.Drawing.Image)(resources.GetObject("TB_RoadName.Image")));
			this.TB_RoadName.LabelText = "Custom Road Name";
			this.TB_RoadName.Location = new System.Drawing.Point(10, 262);
			this.TB_RoadName.Margin = new System.Windows.Forms.Padding(10, 0, 5, 0);
			this.TB_RoadName.Name = "TB_RoadName";
			this.TB_RoadName.Padding = new System.Windows.Forms.Padding(4, 20, 24, 4);
			this.TB_RoadName.Placeholder = "Replaces the generated road name";
			this.TB_RoadName.SelectedText = "";
			this.TB_RoadName.SelectionLength = 0;
			this.TB_RoadName.SelectionStart = 0;
			this.TB_RoadName.Size = new System.Drawing.Size(256, 43);
			this.TB_RoadName.TabIndex = 0;
			this.TB_RoadName.Visible = false;
			this.TB_RoadName.IconClicked += new System.EventHandler(this.TB_RoadName_Leave);
			this.TB_RoadName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_RoadName_KeyDown);
			this.TB_RoadName.Leave += new System.EventHandler(this.TB_RoadName_Leave);
			this.TB_RoadName.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TB_RoadName_PreviewKeyDown);
			// 
			// TB_RoadDesc
			// 
			this.TB_RoadDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TLP_Right.SetColumnSpan(this.TB_RoadDesc, 2);
			this.TB_RoadDesc.EnterTriggersClick = false;
			this.TB_RoadDesc.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_RoadDesc.Image = ((System.Drawing.Image)(resources.GetObject("TB_RoadDesc.Image")));
			this.TB_RoadDesc.LabelText = "Custom Road Description";
			this.TB_RoadDesc.Location = new System.Drawing.Point(10, 341);
			this.TB_RoadDesc.Margin = new System.Windows.Forms.Padding(10, 0, 5, 0);
			this.TB_RoadDesc.Name = "TB_RoadDesc";
			this.TB_RoadDesc.Padding = new System.Windows.Forms.Padding(4, 20, 24, 4);
			this.TB_RoadDesc.Placeholder = "Replaces the generated road description";
			this.TB_RoadDesc.SelectedText = "";
			this.TB_RoadDesc.SelectionLength = 0;
			this.TB_RoadDesc.SelectionStart = 0;
			this.TB_RoadDesc.Size = new System.Drawing.Size(256, 43);
			this.TB_RoadDesc.TabIndex = 2;
			this.TB_RoadDesc.Visible = false;
			this.TB_RoadDesc.IconClicked += new System.EventHandler(this.TB_RoadDesc_Leave);
			this.TB_RoadDesc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_RoadDesc_KeyDown);
			this.TB_RoadDesc.Leave += new System.EventHandler(this.TB_RoadDesc_Leave);
			this.TB_RoadDesc.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TB_RoadDesc_PreviewKeyDown);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.TLP_Right.SetColumnSpan(this.tableLayoutPanel2, 2);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.slickSpacer4, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.C_Warnings, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.B_ViewSavedRoads, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.B_Export, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.B_SaveThumb, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.C_CurrentlyEditing, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 457);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this.tableLayoutPanel2.RowCount = 5;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(271, 117);
			this.tableLayoutPanel2.TabIndex = 15;
			// 
			// slickSpacer4
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.slickSpacer4, 2);
			this.slickSpacer4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.slickSpacer4.Location = new System.Drawing.Point(7, 0);
			this.slickSpacer4.Margin = new System.Windows.Forms.Padding(7, 0, 7, 7);
			this.slickSpacer4.Name = "slickSpacer4";
			this.slickSpacer4.Size = new System.Drawing.Size(257, 1);
			this.slickSpacer4.TabIndex = 0;
			this.slickSpacer4.TabStop = false;
			this.slickSpacer4.Text = "slickSpacer4";
			// 
			// C_Warnings
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.C_Warnings, 2);
			this.C_Warnings.Cursor = System.Windows.Forms.Cursors.Help;
			this.C_Warnings.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.C_Warnings.Location = new System.Drawing.Point(3, 11);
			this.C_Warnings.Name = "C_Warnings";
			this.C_Warnings.Size = new System.Drawing.Size(265, 13);
			this.C_Warnings.TabIndex = 19;
			this.C_Warnings.Visible = false;
			// 
			// B_ViewSavedRoads
			// 
			this.B_ViewSavedRoads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.B_ViewSavedRoads.AutoSize = true;
			this.B_ViewSavedRoads.ColorShade = null;
			this.B_ViewSavedRoads.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ViewSavedRoads.Image = ((System.Drawing.Image)(resources.GetObject("B_ViewSavedRoads.Image")));
			this.B_ViewSavedRoads.Location = new System.Drawing.Point(168, 82);
			this.B_ViewSavedRoads.Name = "B_ViewSavedRoads";
			this.B_ViewSavedRoads.Size = new System.Drawing.Size(100, 27);
			this.B_ViewSavedRoads.SpaceTriggersClick = true;
			this.B_ViewSavedRoads.TabIndex = 2;
			this.B_ViewSavedRoads.Text = "Show Roads";
			this.B_ViewSavedRoads.Click += new System.EventHandler(this.B_ViewSavedRoads_Click);
			// 
			// B_Export
			// 
			this.B_Export.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Export.AutoSize = true;
			this.B_Export.ColorShade = null;
			this.tableLayoutPanel2.SetColumnSpan(this.B_Export, 2);
			this.B_Export.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Export.Image = ((System.Drawing.Image)(resources.GetObject("B_Export.Image")));
			this.B_Export.Location = new System.Drawing.Point(3, 49);
			this.B_Export.Name = "B_Export";
			this.B_Export.Size = new System.Drawing.Size(265, 27);
			this.B_Export.SpaceTriggersClick = true;
			this.B_Export.TabIndex = 1;
			this.B_Export.Text = "Export configuration to Road Builder";
			this.B_Export.Click += new System.EventHandler(this.B_Export_Click);
			// 
			// B_SaveThumb
			// 
			this.B_SaveThumb.AutoSize = true;
			this.B_SaveThumb.ColorShade = null;
			this.B_SaveThumb.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_SaveThumb.Image = ((System.Drawing.Image)(resources.GetObject("B_SaveThumb.Image")));
			this.B_SaveThumb.Location = new System.Drawing.Point(3, 82);
			this.B_SaveThumb.Name = "B_SaveThumb";
			this.B_SaveThumb.Size = new System.Drawing.Size(100, 27);
			this.B_SaveThumb.SpaceTriggersClick = true;
			this.B_SaveThumb.TabIndex = 3;
			this.B_SaveThumb.Text = "Save Thumb.";
			this.B_SaveThumb.Click += new System.EventHandler(this.B_Save_Click);
			// 
			// C_CurrentlyEditing
			// 
			this.tableLayoutPanel2.SetColumnSpan(this.C_CurrentlyEditing, 2);
			this.C_CurrentlyEditing.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.C_CurrentlyEditing.Location = new System.Drawing.Point(3, 30);
			this.C_CurrentlyEditing.Name = "C_CurrentlyEditing";
			this.C_CurrentlyEditing.Size = new System.Drawing.Size(265, 13);
			this.C_CurrentlyEditing.TabIndex = 7;
			this.C_CurrentlyEditing.Visible = false;
			this.C_CurrentlyEditing.LoadConfiguration += new System.EventHandler<ThumbnailMaker.Domain.RoadInfo>(this.RCC_LoadConfiguration);
			this.C_CurrentlyEditing.VisibleChanged += new System.EventHandler(this.C_CurrentlyEditing_VisibleChanged);
			// 
			// L_RoadName
			// 
			this.L_RoadName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_RoadName.AutoSize = true;
			this.L_RoadName.Location = new System.Drawing.Point(10, 316);
			this.L_RoadName.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
			this.L_RoadName.Name = "L_RoadName";
			this.L_RoadName.Size = new System.Drawing.Size(36, 13);
			this.L_RoadName.TabIndex = 14;
			this.L_RoadName.Tag = "NoMouseDown";
			this.L_RoadName.Text = "Lanes";
			this.L_RoadName.Click += new System.EventHandler(this.B_CopyRoadName_Click);
			this.L_RoadName.MouseEnter += new System.EventHandler(this.L_RoadName_MouseEnter);
			this.L_RoadName.MouseLeave += new System.EventHandler(this.L_RoadName_MouseLeave);
			// 
			// L_RoadDesc
			// 
			this.L_RoadDesc.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_RoadDesc.AutoSize = true;
			this.L_RoadDesc.Location = new System.Drawing.Point(10, 395);
			this.L_RoadDesc.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
			this.L_RoadDesc.Name = "L_RoadDesc";
			this.L_RoadDesc.Size = new System.Drawing.Size(36, 13);
			this.L_RoadDesc.TabIndex = 14;
			this.L_RoadDesc.Tag = "NoMouseDown";
			this.L_RoadDesc.Text = "Lanes";
			this.L_RoadDesc.Click += new System.EventHandler(this.B_CopyDesc_Click);
			this.L_RoadDesc.MouseEnter += new System.EventHandler(this.L_RoadName_MouseEnter);
			this.L_RoadDesc.MouseLeave += new System.EventHandler(this.L_RoadName_MouseLeave);
			// 
			// B_EditDesc
			// 
			this.B_EditDesc.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_EditDesc.AutoSize = true;
			this.B_EditDesc.ColorShade = null;
			this.B_EditDesc.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_EditDesc.HandleUiScale = false;
			this.B_EditDesc.Image = ((System.Drawing.Image)(resources.GetObject("B_EditDesc.Image")));
			this.B_EditDesc.Location = new System.Drawing.Point(238, 387);
			this.B_EditDesc.Name = "B_EditDesc";
			this.B_EditDesc.Size = new System.Drawing.Size(30, 30);
			this.B_EditDesc.SpaceTriggersClick = true;
			this.B_EditDesc.TabIndex = 3;
			this.B_EditDesc.Click += new System.EventHandler(this.B_EditDesc_Click);
			// 
			// B_EditName
			// 
			this.B_EditName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_EditName.AutoSize = true;
			this.B_EditName.ColorShade = null;
			this.B_EditName.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_EditName.HandleUiScale = false;
			this.B_EditName.Image = ((System.Drawing.Image)(resources.GetObject("B_EditName.Image")));
			this.B_EditName.Location = new System.Drawing.Point(238, 308);
			this.B_EditName.Name = "B_EditName";
			this.B_EditName.Size = new System.Drawing.Size(30, 30);
			this.B_EditName.SpaceTriggersClick = true;
			this.B_EditName.TabIndex = 1;
			this.B_EditName.Click += new System.EventHandler(this.L_RoadName_Click);
			// 
			// PB
			// 
			this.TLP_Right.SetColumnSpan(this.PB, 2);
			this.PB.Dock = System.Windows.Forms.DockStyle.Top;
			this.PB.Location = new System.Drawing.Point(3, 3);
			this.PB.Name = "PB";
			this.PB.Size = new System.Drawing.Size(265, 256);
			this.PB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PB.TabIndex = 2;
			this.PB.TabStop = false;
			this.PB.SizeChanged += new System.EventHandler(this.PB_SizeChanged);
			this.PB.Click += new System.EventHandler(this.PB_Click);
			this.PB.MouseEnter += new System.EventHandler(this.TB_Name_TextChanged);
			this.PB.MouseLeave += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// FLP_Tags
			// 
			this.FLP_Tags.AutoSize = true;
			this.FLP_Tags.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.FLP_Tags.Controls.Add(this.L_NoTags);
			this.FLP_Tags.Dock = System.Windows.Forms.DockStyle.Top;
			this.FLP_Tags.Location = new System.Drawing.Point(0, 420);
			this.FLP_Tags.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_Tags.Name = "FLP_Tags";
			this.FLP_Tags.Padding = new System.Windows.Forms.Padding(7, 2, 3, 0);
			this.FLP_Tags.Size = new System.Drawing.Size(235, 24);
			this.FLP_Tags.TabIndex = 3;
			this.FLP_Tags.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.FLP_Tags_ControlAdded);
			this.FLP_Tags.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.FLP_Tags_ControlAdded);
			// 
			// L_NoTags
			// 
			this.L_NoTags.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_NoTags.AutoSize = true;
			this.L_NoTags.Location = new System.Drawing.Point(10, 11);
			this.L_NoTags.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
			this.L_NoTags.Name = "L_NoTags";
			this.L_NoTags.Size = new System.Drawing.Size(90, 13);
			this.L_NoTags.TabIndex = 14;
			this.L_NoTags.Tag = "NoMouseDown";
			this.L_NoTags.Text = "No Custom Tags";
			// 
			// B_AddTag
			// 
			this.B_AddTag.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_AddTag.AutoSize = true;
			this.B_AddTag.ColorShade = null;
			this.B_AddTag.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_AddTag.HandleUiScale = false;
			this.B_AddTag.Image = global::ThumbnailMaker.Properties.Resources.I_AddTag;
			this.B_AddTag.Location = new System.Drawing.Point(238, 423);
			this.B_AddTag.Name = "B_AddTag";
			this.B_AddTag.Size = new System.Drawing.Size(30, 30);
			this.B_AddTag.SpaceTriggersClick = true;
			this.B_AddTag.TabIndex = 4;
			this.B_AddTag.Click += new System.EventHandler(this.B_AddTag_Click);
			// 
			// TLP_TopControls
			// 
			this.TLP_TopControls.AddOutline = true;
			this.TLP_TopControls.AutoSize = true;
			this.TLP_TopControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_TopControls.ColumnCount = 6;
			this.TLP_Main.SetColumnSpan(this.TLP_TopControls, 3);
			this.TLP_TopControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.TLP_TopControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.TLP_TopControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.TLP_TopControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.TLP_TopControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.TLP_TopControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.TLP_TopControls.Controls.Add(this.TB_Size, 0, 1);
			this.TLP_TopControls.Controls.Add(this.TB_BufferSize, 1, 1);
			this.TLP_TopControls.Controls.Add(this.TB_CustomText, 3, 1);
			this.TLP_TopControls.Controls.Add(this.TB_SpeedLimit, 2, 1);
			this.TLP_TopControls.Controls.Add(this.CB_HighwayRules, 5, 1);
			this.TLP_TopControls.Controls.Add(this.CB_CanCrossLanes, 4, 1);
			this.TLP_TopControls.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_TopControls.Image = ((System.Drawing.Image)(resources.GetObject("TLP_TopControls.Image")));
			this.TLP_TopControls.Location = new System.Drawing.Point(5, 0);
			this.TLP_TopControls.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_TopControls.Name = "TLP_TopControls";
			this.TLP_TopControls.Padding = new System.Windows.Forms.Padding(5, 34, 5, 5);
			this.TLP_TopControls.RowCount = 2;
			this.TLP_Main.SetRowSpan(this.TLP_TopControls, 2);
			this.TLP_TopControls.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_TopControls.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_TopControls.Size = new System.Drawing.Size(894, 87);
			this.TLP_TopControls.TabIndex = 13;
			this.TLP_TopControls.Text = "Road Settings";
			// 
			// TB_Size
			// 
			this.TB_Size.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_Size.EnterTriggersClick = false;
			this.TB_Size.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Size.Image = ((System.Drawing.Image)(resources.GetObject("TB_Size.Image")));
			this.TB_Size.LabelText = "Road Size (m)";
			this.TB_Size.Location = new System.Drawing.Point(8, 44);
			this.TB_Size.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
			this.TB_Size.Name = "TB_Size";
			this.TB_Size.Padding = new System.Windows.Forms.Padding(4, 20, 24, 4);
			this.TB_Size.Placeholder = "Custom road width";
			this.TB_Size.SelectedText = "";
			this.TB_Size.SelectionLength = 0;
			this.TB_Size.SelectionStart = 0;
			this.TB_Size.Size = new System.Drawing.Size(134, 35);
			this.TB_Size.TabIndex = 0;
			this.TB_Size.Validation = SlickControls.ValidationType.Decimal;
			this.TB_Size.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// TB_BufferSize
			// 
			this.TB_BufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_BufferSize.EnterTriggersClick = false;
			this.TB_BufferSize.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_BufferSize.Image = ((System.Drawing.Image)(resources.GetObject("TB_BufferSize.Image")));
			this.TB_BufferSize.LabelText = "Buffer Size (m)";
			this.TB_BufferSize.Location = new System.Drawing.Point(155, 44);
			this.TB_BufferSize.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
			this.TB_BufferSize.Name = "TB_BufferSize";
			this.TB_BufferSize.Padding = new System.Windows.Forms.Padding(4, 20, 24, 4);
			this.TB_BufferSize.Placeholder = "Space after the curb";
			this.TB_BufferSize.SelectedText = "";
			this.TB_BufferSize.SelectionLength = 0;
			this.TB_BufferSize.SelectionStart = 0;
			this.TB_BufferSize.Size = new System.Drawing.Size(134, 35);
			this.TB_BufferSize.TabIndex = 1;
			this.TB_BufferSize.Validation = SlickControls.ValidationType.Decimal;
			this.TB_BufferSize.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// TB_CustomText
			// 
			this.TB_CustomText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_CustomText.EnterTriggersClick = false;
			this.TB_CustomText.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_CustomText.Image = ((System.Drawing.Image)(resources.GetObject("TB_CustomText.Image")));
			this.TB_CustomText.LabelText = "Custom Text";
			this.TB_CustomText.Location = new System.Drawing.Point(449, 44);
			this.TB_CustomText.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
			this.TB_CustomText.Name = "TB_CustomText";
			this.TB_CustomText.Padding = new System.Windows.Forms.Padding(4, 20, 24, 4);
			this.TB_CustomText.Placeholder = "Extra display text on the thumbnail";
			this.TB_CustomText.SelectedText = "";
			this.TB_CustomText.SelectionLength = 0;
			this.TB_CustomText.SelectionStart = 0;
			this.TB_CustomText.Size = new System.Drawing.Size(134, 35);
			this.TB_CustomText.TabIndex = 3;
			this.TB_CustomText.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// TB_SpeedLimit
			// 
			this.TB_SpeedLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_SpeedLimit.EnterTriggersClick = false;
			this.TB_SpeedLimit.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_SpeedLimit.Image = ((System.Drawing.Image)(resources.GetObject("TB_SpeedLimit.Image")));
			this.TB_SpeedLimit.LabelText = "Global Speed Limit";
			this.TB_SpeedLimit.Location = new System.Drawing.Point(302, 44);
			this.TB_SpeedLimit.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
			this.TB_SpeedLimit.Name = "TB_SpeedLimit";
			this.TB_SpeedLimit.Padding = new System.Windows.Forms.Padding(4, 20, 24, 4);
			this.TB_SpeedLimit.Placeholder = "Default speed limit";
			this.TB_SpeedLimit.SelectedText = "";
			this.TB_SpeedLimit.SelectionLength = 0;
			this.TB_SpeedLimit.SelectionStart = 0;
			this.TB_SpeedLimit.Size = new System.Drawing.Size(134, 35);
			this.TB_SpeedLimit.TabIndex = 2;
			this.TB_SpeedLimit.Validation = SlickControls.ValidationType.Number;
			this.TB_SpeedLimit.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			this.TB_SpeedLimit.IconClicked += new System.EventHandler(this.TB_SpeedLimit_IconClicked);
			// 
			// CB_HighwayRules
			// 
			this.CB_HighwayRules.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CB_HighwayRules.AutoSize = false;
			this.CB_HighwayRules.Checked = false;
			this.CB_HighwayRules.CheckedText = null;
			this.CB_HighwayRules.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_HighwayRules.DefaultValue = false;
			this.CB_HighwayRules.EnterTriggersClick = false;
			this.CB_HighwayRules.Location = new System.Drawing.Point(743, 50);
			this.CB_HighwayRules.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
			this.CB_HighwayRules.Name = "CB_HighwayRules";
			this.CB_HighwayRules.Size = new System.Drawing.Size(95, 22);
			this.CB_HighwayRules.SpaceTriggersClick = true;
			this.CB_HighwayRules.TabIndex = 4;
			this.CB_HighwayRules.Text = "Highway Rules";
			this.CB_HighwayRules.UncheckedText = null;
			// 
			// CB_CanCrossLanes
			// 
			this.CB_CanCrossLanes.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CB_CanCrossLanes.AutoSize = false;
			this.CB_CanCrossLanes.Checked = false;
			this.CB_CanCrossLanes.CheckedText = null;
			this.CB_CanCrossLanes.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_CanCrossLanes.DefaultValue = false;
			this.CB_CanCrossLanes.EnterTriggersClick = false;
			this.CB_CanCrossLanes.Location = new System.Drawing.Point(596, 50);
			this.CB_CanCrossLanes.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
			this.CB_CanCrossLanes.Name = "CB_CanCrossLanes";
			this.CB_CanCrossLanes.Size = new System.Drawing.Size(103, 22);
			this.CB_CanCrossLanes.SpaceTriggersClick = true;
			this.CB_CanCrossLanes.TabIndex = 4;
			this.CB_CanCrossLanes.Text = "Can Cross Lanes";
			this.CB_CanCrossLanes.UncheckedText = null;
			// 
			// panel1
			// 
			this.TLP_Main.SetColumnSpan(this.panel1, 3);
			this.panel1.Controls.Add(this.slickScroll1);
			this.panel1.Controls.Add(this.P_Lanes);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(5, 140);
			this.panel1.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(894, 434);
			this.panel1.TabIndex = 1;
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.P_Lanes;
			this.slickScroll1.Location = new System.Drawing.Point(888, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(6, 434);
			this.slickScroll1.SmallHandle = true;
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
			this.P_Lanes.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.P_Lanes.Size = new System.Drawing.Size(5, 22);
			this.P_Lanes.TabIndex = 0;
			this.P_Lanes.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.P_Lanes_ControlAdded);
			// 
			// slickSpacer1
			// 
			this.TLP_Main.SetColumnSpan(this.slickSpacer1, 3);
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
			this.slickSpacer1.Location = new System.Drawing.Point(8, 128);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(888, 1);
			this.slickSpacer1.TabIndex = 9;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// TLP_Buttons
			// 
			this.TLP_Buttons.AutoSize = true;
			this.TLP_Buttons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Buttons.ColumnCount = 7;
			this.TLP_Main.SetColumnSpan(this.TLP_Buttons, 3);
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Buttons.Controls.Add(this.slickSpacer3, 3, 0);
			this.TLP_Buttons.Controls.Add(this.B_FlipLanes, 5, 0);
			this.TLP_Buttons.Controls.Add(this.B_DuplicateFlip, 4, 0);
			this.TLP_Buttons.Controls.Add(this.B_AddLane, 6, 0);
			this.TLP_Buttons.Controls.Add(this.B_ClearLines, 2, 0);
			this.TLP_Buttons.Controls.Add(this.B_Options, 0, 0);
			this.TLP_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Buttons.Location = new System.Drawing.Point(5, 87);
			this.TLP_Buttons.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this.TLP_Buttons.Name = "TLP_Buttons";
			this.TLP_Buttons.RowCount = 1;
			this.TLP_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Buttons.Size = new System.Drawing.Size(894, 33);
			this.TLP_Buttons.TabIndex = 0;
			this.TLP_Buttons.Resize += new System.EventHandler(this.TLP_Buttons_Resize);
			// 
			// slickSpacer3
			// 
			this.slickSpacer3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.slickSpacer3.Location = new System.Drawing.Point(604, 3);
			this.slickSpacer3.Name = "slickSpacer3";
			this.slickSpacer3.Size = new System.Drawing.Size(1, 27);
			this.slickSpacer3.TabIndex = 19;
			this.slickSpacer3.TabStop = false;
			this.slickSpacer3.Text = "slickSpacer3";
			// 
			// B_FlipLanes
			// 
			this.B_FlipLanes.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_FlipLanes.AutoSize = true;
			this.B_FlipLanes.ColorShade = null;
			this.B_FlipLanes.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_FlipLanes.Image = global::ThumbnailMaker.Properties.Resources.I_2W;
			this.B_FlipLanes.Location = new System.Drawing.Point(709, 3);
			this.B_FlipLanes.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.B_FlipLanes.Name = "B_FlipLanes";
			this.B_FlipLanes.Size = new System.Drawing.Size(81, 27);
			this.B_FlipLanes.SpaceTriggersClick = true;
			this.B_FlipLanes.TabIndex = 1;
			this.B_FlipLanes.Tag = "Flip Lanes";
			this.B_FlipLanes.Text = "Flip Lanes";
			this.B_FlipLanes.Click += new System.EventHandler(this.B_FlipLanes_Click);
			// 
			// B_DuplicateFlip
			// 
			this.B_DuplicateFlip.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_DuplicateFlip.AutoSize = true;
			this.B_DuplicateFlip.ColorShade = null;
			this.B_DuplicateFlip.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_DuplicateFlip.Image = ((System.Drawing.Image)(resources.GetObject("B_DuplicateFlip.Image")));
			this.B_DuplicateFlip.Location = new System.Drawing.Point(615, 3);
			this.B_DuplicateFlip.Margin = new System.Windows.Forms.Padding(5, 3, 10, 3);
			this.B_DuplicateFlip.Name = "B_DuplicateFlip";
			this.B_DuplicateFlip.Size = new System.Drawing.Size(81, 27);
			this.B_DuplicateFlip.SpaceTriggersClick = true;
			this.B_DuplicateFlip.TabIndex = 2;
			this.B_DuplicateFlip.Tag = "Duplicate Flip Lanes";
			this.B_DuplicateFlip.Text = "Duplicate Flip Lanes";
			this.B_DuplicateFlip.Click += new System.EventHandler(this.B_DuplicateFlip_Click);
			// 
			// B_AddLane
			// 
			this.B_AddLane.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_AddLane.AutoSize = true;
			this.B_AddLane.ColorShade = null;
			this.B_AddLane.ColorStyle = Extensions.ColorStyle.Green;
			this.B_AddLane.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_AddLane.Image = ((System.Drawing.Image)(resources.GetObject("B_AddLane.Image")));
			this.B_AddLane.Location = new System.Drawing.Point(803, 3);
			this.B_AddLane.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.B_AddLane.Name = "B_AddLane";
			this.B_AddLane.Size = new System.Drawing.Size(81, 27);
			this.B_AddLane.SpaceTriggersClick = true;
			this.B_AddLane.TabIndex = 0;
			this.B_AddLane.Tag = "Add Lane";
			this.B_AddLane.Text = "Add Lane";
			this.B_AddLane.MouseClick += new System.Windows.Forms.MouseEventHandler(this.B_AddLane_MouseClick);
			// 
			// B_ClearLines
			// 
			this.B_ClearLines.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.B_ClearLines.AutoSize = true;
			this.B_ClearLines.ColorShade = null;
			this.B_ClearLines.ColorStyle = Extensions.ColorStyle.Red;
			this.B_ClearLines.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ClearLines.Image = ((System.Drawing.Image)(resources.GetObject("B_ClearLines.Image")));
			this.B_ClearLines.Location = new System.Drawing.Point(514, 3);
			this.B_ClearLines.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
			this.B_ClearLines.Name = "B_ClearLines";
			this.B_ClearLines.Size = new System.Drawing.Size(81, 27);
			this.B_ClearLines.SpaceTriggersClick = true;
			this.B_ClearLines.TabIndex = 3;
			this.B_ClearLines.Tag = "Clear Lanes";
			this.B_ClearLines.Text = "Clear Road";
			this.B_ClearLines.Click += new System.EventHandler(this.B_Clear_Click);
			// 
			// B_Options
			// 
			this.B_Options.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_Options.AutoSize = true;
			this.B_Options.ColorShade = null;
			this.B_Options.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Options.Image = ((System.Drawing.Image)(resources.GetObject("B_Options.Image")));
			this.B_Options.Location = new System.Drawing.Point(3, 3);
			this.B_Options.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.B_Options.Name = "B_Options";
			this.B_Options.Size = new System.Drawing.Size(85, 27);
			this.B_Options.SpaceTriggersClick = true;
			this.B_Options.TabIndex = 4;
			this.B_Options.Tag = "Options";
			this.B_Options.Text = "Options";
			this.B_Options.Click += new System.EventHandler(this.B_Options_Click);
			// 
			// PC_MainPage
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.TLP_Main);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "PC_MainPage";
			this.Padding = new System.Windows.Forms.Padding(5, 30, 0, 0);
			this.Size = new System.Drawing.Size(1186, 604);
			this.Text = "Thumbnail Maker";
			this.Controls.SetChildIndex(this.TLP_Main, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.roundedPanel1.ResumeLayout(false);
			this.TLP_Right.ResumeLayout(false);
			this.TLP_Right.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB)).EndInit();
			this.FLP_Tags.ResumeLayout(false);
			this.FLP_Tags.PerformLayout();
			this.TLP_TopControls.ResumeLayout(false);
			this.TLP_TopControls.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.TLP_Buttons.ResumeLayout(false);
			this.TLP_Buttons.PerformLayout();
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
		private System.Windows.Forms.TableLayoutPanel TLP_Buttons;
		private SlickControls.SlickButton B_Options;
		private SlickControls.SlickButton B_SaveThumb;
		private SlickControls.RoundedGroupTableLayoutPanel TLP_TopControls;
		private SlickControls.SlickTextBox TB_SpeedLimit;
		private SlickControls.SlickTextBox TB_CustomText;
		private SlickControls.RoundedTableLayoutPanel TLP_Right;
		private SlickControls.SlickButton B_EditName;
		private System.Windows.Forms.Label L_RoadName;
		private SlickControls.SlickTextBox TB_BufferSize;
		private SlickControls.SlickButton B_ClearLines;
		private SlickControls.SlickButton B_FlipLanes;
		private SlickControls.SlickButton B_DuplicateFlip;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private SlickControls.SlickButton B_Export;
		private SlickControls.SlickSpacer slickSpacer3;
		private Controls.RoadConfigContainer RCC;
		private SlickControls.SlickTextBox TB_RoadName;
		private Controls.OptionSelectionControl<RoadType> RoadTypeControl;
		private Controls.OptionSelectionControl<RegionType> RegionTypeControl;
		private Controls.OptionSelectionControl<TextureType> SideTextureControl;
		private Controls.OptionSelectionControl<BridgeTextureType> BridgeSideTextureControl;
		private Controls.OptionSelectionControl<AsphaltStyle> AsphaltTextureControl;
		private Controls.OptionMultiSelectionControl<RoadElevation> ElevationTypeControl;
		private Controls.CurrentlyEditingControl C_CurrentlyEditing;
		private System.Windows.Forms.Label L_RoadDesc;
		private SlickControls.SlickButton B_EditDesc;
		private SlickControls.SlickButton B_ViewSavedRoads;
		private SlickControls.SlickButton B_AddTag;
		private System.Windows.Forms.FlowLayoutPanel FLP_Tags;
		private System.Windows.Forms.Label L_NoTags;
		private SlickControls.SlickSpacer slickSpacer4;
		private Controls.WarningsControl C_Warnings;
		private SlickControls.SlickTextBox TB_RoadDesc;
		private SlickControls.RoundedPanel roundedPanel1;
		private SlickControls.SlickCheckbox CB_HighwayRules;
		private SlickControls.SlickCheckbox CB_CanCrossLanes;
	}
}