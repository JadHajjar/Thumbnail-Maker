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
			this.RCC = new ThumbnailMaker.Controls.RoadConfigContainer();
			this.slickSpacer2 = new SlickControls.SlickSpacer();
			this.TLP_Right = new System.Windows.Forms.TableLayoutPanel();
			this.B_Export = new SlickControls.SlickButton();
			this.slickSpacer4 = new SlickControls.SlickSpacer();
			this.B_AddTag = new SlickControls.SlickButton();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.B_ViewSavedRoads = new SlickControls.SlickButton();
			this.B_SaveThumb = new SlickControls.SlickButton();
			this.L_CurrentlyEditing = new System.Windows.Forms.Label();
			this.L_RoadName = new System.Windows.Forms.Label();
			this.L_RoadDesc = new System.Windows.Forms.Label();
			this.B_EditDesc = new SlickControls.SlickButton();
			this.B_EditName = new SlickControls.SlickButton();
			this.PB = new System.Windows.Forms.PictureBox();
			this.FLP_Tags = new System.Windows.Forms.FlowLayoutPanel();
			this.L_NoTags = new System.Windows.Forms.Label();
			this.B_ClearCurrentlyEdited = new SlickControls.SlickButton();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.GB_AsphaltTexture = new SlickControls.SlickGroupBox();
			this.GB_Region = new SlickControls.SlickGroupBox();
			this.GB_RoadType = new SlickControls.SlickGroupBox();
			this.GB_SideTexture = new SlickControls.SlickGroupBox();
			this.TB_Size = new SlickControls.SlickTextBox();
			this.GB_BridgeSideTexture = new SlickControls.SlickGroupBox();
			this.TB_BufferSize = new SlickControls.SlickTextBox();
			this.TB_CustomText = new SlickControls.SlickTextBox();
			this.TB_SpeedLimit = new SlickControls.SlickTextBox();
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
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB)).BeginInit();
			this.FLP_Tags.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
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
			this.TLP_Main.ColumnCount = 6;
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 276F));
			this.TLP_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Main.Controls.Add(this.RCC, 5, 0);
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
			this.TLP_Main.Size = new System.Drawing.Size(1181, 574);
			this.TLP_Main.TabIndex = 8;
			// 
			// RCC
			// 
			this.RCC.Dock = System.Windows.Forms.DockStyle.Right;
			this.RCC.Location = new System.Drawing.Point(1182, 0);
			this.RCC.Margin = new System.Windows.Forms.Padding(0);
			this.RCC.Name = "RCC";
			this.TLP_Main.SetRowSpan(this.RCC, 5);
			this.RCC.Size = new System.Drawing.Size(0, 574);
			this.RCC.TabIndex = 16;
			this.RCC.LoadConfiguration += new System.EventHandler<ThumbnailMaker.Domain.RoadInfo>(this.RCC_LoadConfiguration);
			// 
			// slickSpacer2
			// 
			this.slickSpacer2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.slickSpacer2.Location = new System.Drawing.Point(899, 3);
			this.slickSpacer2.Name = "slickSpacer2";
			this.TLP_Main.SetRowSpan(this.slickSpacer2, 5);
			this.slickSpacer2.Size = new System.Drawing.Size(1, 568);
			this.slickSpacer2.TabIndex = 18;
			this.slickSpacer2.TabStop = false;
			this.slickSpacer2.Text = "slickSpacer2";
			// 
			// TLP_Right
			// 
			this.TLP_Right.ColumnCount = 2;
			this.TLP_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP_Right.Controls.Add(this.B_Export, 0, 6);
			this.TLP_Right.Controls.Add(this.slickSpacer4, 0, 4);
			this.TLP_Right.Controls.Add(this.B_AddTag, 1, 3);
			this.TLP_Right.Controls.Add(this.tableLayoutPanel2, 0, 7);
			this.TLP_Right.Controls.Add(this.L_CurrentlyEditing, 0, 5);
			this.TLP_Right.Controls.Add(this.L_RoadName, 0, 1);
			this.TLP_Right.Controls.Add(this.L_RoadDesc, 0, 2);
			this.TLP_Right.Controls.Add(this.B_EditDesc, 1, 2);
			this.TLP_Right.Controls.Add(this.B_EditName, 1, 1);
			this.TLP_Right.Controls.Add(this.PB, 0, 0);
			this.TLP_Right.Controls.Add(this.FLP_Tags, 0, 3);
			this.TLP_Right.Controls.Add(this.B_ClearCurrentlyEdited, 1, 5);
			this.TLP_Right.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP_Right.Location = new System.Drawing.Point(905, 0);
			this.TLP_Right.Margin = new System.Windows.Forms.Padding(0);
			this.TLP_Right.Name = "TLP_Right";
			this.TLP_Right.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.TLP_Right.RowCount = 8;
			this.TLP_Main.SetRowSpan(this.TLP_Right, 5);
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP_Right.Size = new System.Drawing.Size(276, 574);
			this.TLP_Right.TabIndex = 17;
			// 
			// B_Export
			// 
			this.B_Export.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.B_Export.ColorShade = null;
			this.TLP_Right.SetColumnSpan(this.B_Export, 2);
			this.B_Export.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Export.IconSize = 16;
			this.B_Export.Image = ((System.Drawing.Image)(resources.GetObject("B_Export.Image")));
			this.B_Export.Location = new System.Drawing.Point(3, 496);
			this.B_Export.Name = "B_Export";
			this.B_Export.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_Export.Size = new System.Drawing.Size(265, 27);
			this.B_Export.SpaceTriggersClick = true;
			this.B_Export.TabIndex = 5;
			this.B_Export.Text = "Export configuration to Road Builder";
			this.B_Export.Click += new System.EventHandler(this.B_Export_Click);
			// 
			// slickSpacer4
			// 
			this.slickSpacer4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TLP_Right.SetColumnSpan(this.slickSpacer4, 3);
			this.slickSpacer4.Location = new System.Drawing.Point(7, 449);
			this.slickSpacer4.Margin = new System.Windows.Forms.Padding(7);
			this.slickSpacer4.Name = "slickSpacer4";
			this.slickSpacer4.Size = new System.Drawing.Size(257, 1);
			this.slickSpacer4.TabIndex = 18;
			this.slickSpacer4.TabStop = false;
			this.slickSpacer4.Text = "slickSpacer4";
			// 
			// B_AddTag
			// 
			this.B_AddTag.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_AddTag.ColorShade = null;
			this.B_AddTag.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_AddTag.HandleUiScale = false;
			this.B_AddTag.IconSize = 16;
			this.B_AddTag.Image = global::ThumbnailMaker.Properties.Resources.I_AddTag;
			this.B_AddTag.Location = new System.Drawing.Point(238, 341);
			this.B_AddTag.Name = "B_AddTag";
			this.B_AddTag.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_AddTag.Size = new System.Drawing.Size(30, 30);
			this.B_AddTag.SpaceTriggersClick = true;
			this.B_AddTag.TabIndex = 16;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.TLP_Right.SetColumnSpan(this.tableLayoutPanel2, 2);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.B_ViewSavedRoads, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.B_SaveThumb, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 536);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 5);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(271, 33);
			this.tableLayoutPanel2.TabIndex = 15;
			// 
			// B_ViewSavedRoads
			// 
			this.B_ViewSavedRoads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.B_ViewSavedRoads.ColorShade = null;
			this.B_ViewSavedRoads.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ViewSavedRoads.IconSize = 16;
			this.B_ViewSavedRoads.Image = ((System.Drawing.Image)(resources.GetObject("B_ViewSavedRoads.Image")));
			this.B_ViewSavedRoads.Location = new System.Drawing.Point(168, 3);
			this.B_ViewSavedRoads.Name = "B_ViewSavedRoads";
			this.B_ViewSavedRoads.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_ViewSavedRoads.Size = new System.Drawing.Size(100, 27);
			this.B_ViewSavedRoads.SpaceTriggersClick = true;
			this.B_ViewSavedRoads.TabIndex = 5;
			this.B_ViewSavedRoads.Text = "Load Road";
			this.B_ViewSavedRoads.Click += new System.EventHandler(this.B_ViewSavedRoads_Click);
			// 
			// B_SaveThumb
			// 
			this.B_SaveThumb.ColorShade = null;
			this.B_SaveThumb.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_SaveThumb.IconSize = 16;
			this.B_SaveThumb.Image = ((System.Drawing.Image)(resources.GetObject("B_SaveThumb.Image")));
			this.B_SaveThumb.Location = new System.Drawing.Point(3, 3);
			this.B_SaveThumb.Name = "B_SaveThumb";
			this.B_SaveThumb.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.B_SaveThumb.Size = new System.Drawing.Size(100, 27);
			this.B_SaveThumb.SpaceTriggersClick = true;
			this.B_SaveThumb.TabIndex = 5;
			this.B_SaveThumb.Text = "Save Thumb.";
			this.B_SaveThumb.Click += new System.EventHandler(this.B_Save_Click);
			// 
			// L_CurrentlyEditing
			// 
			this.L_CurrentlyEditing.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_CurrentlyEditing.AutoSize = true;
			this.L_CurrentlyEditing.Location = new System.Drawing.Point(10, 468);
			this.L_CurrentlyEditing.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
			this.L_CurrentlyEditing.Name = "L_CurrentlyEditing";
			this.L_CurrentlyEditing.Size = new System.Drawing.Size(63, 13);
			this.L_CurrentlyEditing.TabIndex = 7;
			this.L_CurrentlyEditing.Text = "Last Edited";
			this.L_CurrentlyEditing.Visible = false;
			this.L_CurrentlyEditing.Click += new System.EventHandler(this.L_CurrentlyEditing_Click);
			// 
			// L_RoadName
			// 
			this.L_RoadName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_RoadName.AutoSize = true;
			this.L_RoadName.Location = new System.Drawing.Point(10, 277);
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
			this.L_RoadDesc.Location = new System.Drawing.Point(10, 313);
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
			this.B_EditDesc.ColorShade = null;
			this.B_EditDesc.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_EditDesc.HandleUiScale = false;
			this.B_EditDesc.IconSize = 16;
			this.B_EditDesc.Image = ((System.Drawing.Image)(resources.GetObject("B_EditDesc.Image")));
			this.B_EditDesc.Location = new System.Drawing.Point(238, 305);
			this.B_EditDesc.Name = "B_EditDesc";
			this.B_EditDesc.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_EditDesc.Size = new System.Drawing.Size(30, 30);
			this.B_EditDesc.SpaceTriggersClick = true;
			this.B_EditDesc.TabIndex = 14;
			// 
			// B_EditName
			// 
			this.B_EditName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_EditName.ColorShade = null;
			this.B_EditName.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_EditName.HandleUiScale = false;
			this.B_EditName.IconSize = 16;
			this.B_EditName.Image = ((System.Drawing.Image)(resources.GetObject("B_EditName.Image")));
			this.B_EditName.Location = new System.Drawing.Point(238, 269);
			this.B_EditName.Name = "B_EditName";
			this.B_EditName.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_EditName.Size = new System.Drawing.Size(30, 30);
			this.B_EditName.SpaceTriggersClick = true;
			this.B_EditName.TabIndex = 14;
			this.B_EditName.Click += new System.EventHandler(this.L_RoadName_Click);
			// 
			// PB
			// 
			this.PB.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.TLP_Right.SetColumnSpan(this.PB, 2);
			this.PB.Location = new System.Drawing.Point(10, 5);
			this.PB.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
			this.PB.Name = "PB";
			this.PB.Size = new System.Drawing.Size(256, 256);
			this.PB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PB.TabIndex = 2;
			this.PB.TabStop = false;
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
			this.FLP_Tags.Location = new System.Drawing.Point(0, 338);
			this.FLP_Tags.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_Tags.Name = "FLP_Tags";
			this.FLP_Tags.Size = new System.Drawing.Size(235, 25);
			this.FLP_Tags.TabIndex = 17;
			// 
			// L_NoTags
			// 
			this.L_NoTags.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.L_NoTags.AutoSize = true;
			this.L_NoTags.Location = new System.Drawing.Point(10, 12);
			this.L_NoTags.Margin = new System.Windows.Forms.Padding(10, 12, 3, 0);
			this.L_NoTags.Name = "L_NoTags";
			this.L_NoTags.Size = new System.Drawing.Size(89, 13);
			this.L_NoTags.TabIndex = 14;
			this.L_NoTags.Tag = "NoMouseDown";
			this.L_NoTags.Text = "No Custom Tags";
			// 
			// B_ClearCurrentlyEdited
			// 
			this.B_ClearCurrentlyEdited.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_ClearCurrentlyEdited.ColorShade = null;
			this.B_ClearCurrentlyEdited.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ClearCurrentlyEdited.HandleUiScale = false;
			this.B_ClearCurrentlyEdited.IconSize = 16;
			this.B_ClearCurrentlyEdited.Image = global::ThumbnailMaker.Properties.Resources.I_Cancel;
			this.B_ClearCurrentlyEdited.Location = new System.Drawing.Point(238, 460);
			this.B_ClearCurrentlyEdited.Name = "B_ClearCurrentlyEdited";
			this.B_ClearCurrentlyEdited.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_ClearCurrentlyEdited.Size = new System.Drawing.Size(30, 30);
			this.B_ClearCurrentlyEdited.SpaceTriggersClick = true;
			this.B_ClearCurrentlyEdited.TabIndex = 16;
			this.B_ClearCurrentlyEdited.Visible = false;
			this.B_ClearCurrentlyEdited.Click += new System.EventHandler(this.L_CurrentlyEditing_Click);
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel6.ColumnCount = 5;
			this.TLP_Main.SetColumnSpan(this.tableLayoutPanel6, 3);
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel6.Controls.Add(this.GB_AsphaltTexture, 4, 0);
			this.tableLayoutPanel6.Controls.Add(this.GB_Region, 1, 0);
			this.tableLayoutPanel6.Controls.Add(this.GB_RoadType, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.GB_SideTexture, 2, 0);
			this.tableLayoutPanel6.Controls.Add(this.TB_Size, 0, 1);
			this.tableLayoutPanel6.Controls.Add(this.GB_BridgeSideTexture, 3, 0);
			this.tableLayoutPanel6.Controls.Add(this.TB_BufferSize, 1, 1);
			this.tableLayoutPanel6.Controls.Add(this.TB_CustomText, 3, 1);
			this.tableLayoutPanel6.Controls.Add(this.TB_SpeedLimit, 2, 1);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(15, 0);
			this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 3;
			this.TLP_Main.SetRowSpan(this.tableLayoutPanel6, 2);
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.Size = new System.Drawing.Size(880, 74);
			this.tableLayoutPanel6.TabIndex = 13;
			// 
			// GB_AsphaltTexture
			// 
			this.GB_AsphaltTexture.AutoSize = true;
			this.GB_AsphaltTexture.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GB_AsphaltTexture.Dock = System.Windows.Forms.DockStyle.Top;
			this.GB_AsphaltTexture.Icon = ((System.Drawing.Image)(resources.GetObject("GB_AsphaltTexture.Icon")));
			this.GB_AsphaltTexture.Location = new System.Drawing.Point(707, 3);
			this.GB_AsphaltTexture.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.GB_AsphaltTexture.MinimumSize = new System.Drawing.Size(60, 20);
			this.GB_AsphaltTexture.Name = "GB_AsphaltTexture";
			this.GB_AsphaltTexture.Size = new System.Drawing.Size(163, 20);
			this.GB_AsphaltTexture.TabIndex = 20;
			this.GB_AsphaltTexture.TabStop = false;
			this.GB_AsphaltTexture.Text = "Asphalt Texture";
			// 
			// GB_Region
			// 
			this.GB_Region.AutoSize = true;
			this.GB_Region.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GB_Region.Dock = System.Windows.Forms.DockStyle.Top;
			this.GB_Region.Icon = ((System.Drawing.Image)(resources.GetObject("GB_Region.Icon")));
			this.GB_Region.Location = new System.Drawing.Point(179, 3);
			this.GB_Region.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.GB_Region.MinimumSize = new System.Drawing.Size(60, 20);
			this.GB_Region.Name = "GB_Region";
			this.GB_Region.Size = new System.Drawing.Size(163, 20);
			this.GB_Region.TabIndex = 16;
			this.GB_Region.TabStop = false;
			this.GB_Region.Text = "Speed Sign Region";
			// 
			// GB_RoadType
			// 
			this.GB_RoadType.AutoSize = true;
			this.GB_RoadType.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GB_RoadType.Dock = System.Windows.Forms.DockStyle.Top;
			this.GB_RoadType.Icon = ((System.Drawing.Image)(resources.GetObject("GB_RoadType.Icon")));
			this.GB_RoadType.Location = new System.Drawing.Point(3, 3);
			this.GB_RoadType.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.GB_RoadType.MinimumSize = new System.Drawing.Size(60, 20);
			this.GB_RoadType.Name = "GB_RoadType";
			this.GB_RoadType.Size = new System.Drawing.Size(163, 20);
			this.GB_RoadType.TabIndex = 16;
			this.GB_RoadType.TabStop = false;
			this.GB_RoadType.Text = "Road Type";
			// 
			// GB_SideTexture
			// 
			this.GB_SideTexture.AutoSize = true;
			this.GB_SideTexture.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GB_SideTexture.Dock = System.Windows.Forms.DockStyle.Top;
			this.GB_SideTexture.Icon = ((System.Drawing.Image)(resources.GetObject("GB_SideTexture.Icon")));
			this.GB_SideTexture.Location = new System.Drawing.Point(355, 3);
			this.GB_SideTexture.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.GB_SideTexture.MinimumSize = new System.Drawing.Size(60, 20);
			this.GB_SideTexture.Name = "GB_SideTexture";
			this.GB_SideTexture.Size = new System.Drawing.Size(163, 20);
			this.GB_SideTexture.TabIndex = 18;
			this.GB_SideTexture.TabStop = false;
			this.GB_SideTexture.Text = "Ground Side Texture";
			// 
			// TB_Size
			// 
			this.TB_Size.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_Size.EnterTriggersClick = false;
			this.TB_Size.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Size.Image = ((System.Drawing.Image)(resources.GetObject("TB_Size.Image")));
			this.TB_Size.LabelText = "Road Size (m)";
			this.TB_Size.Location = new System.Drawing.Point(3, 36);
			this.TB_Size.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
			this.TB_Size.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Size.MaxLength = 32767;
			this.TB_Size.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_Size.Name = "TB_Size";
			this.TB_Size.Password = false;
			this.TB_Size.Placeholder = "Custom road width";
			this.TB_Size.ReadOnly = false;
			this.TB_Size.Required = false;
			this.TB_Size.SelectAllOnFocus = false;
			this.TB_Size.SelectedText = "";
			this.TB_Size.SelectionLength = 0;
			this.TB_Size.SelectionStart = 0;
			this.TB_Size.Size = new System.Drawing.Size(163, 35);
			this.TB_Size.TabIndex = 2;
			this.TB_Size.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_Size.Validation = SlickControls.ValidationType.Decimal;
			this.TB_Size.ValidationCustom = null;
			this.TB_Size.ValidationRegex = "";
			this.TB_Size.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// GB_BridgeSideTexture
			// 
			this.GB_BridgeSideTexture.AutoSize = true;
			this.GB_BridgeSideTexture.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GB_BridgeSideTexture.Dock = System.Windows.Forms.DockStyle.Top;
			this.GB_BridgeSideTexture.Icon = ((System.Drawing.Image)(resources.GetObject("GB_BridgeSideTexture.Icon")));
			this.GB_BridgeSideTexture.Location = new System.Drawing.Point(531, 3);
			this.GB_BridgeSideTexture.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
			this.GB_BridgeSideTexture.MinimumSize = new System.Drawing.Size(60, 20);
			this.GB_BridgeSideTexture.Name = "GB_BridgeSideTexture";
			this.GB_BridgeSideTexture.Size = new System.Drawing.Size(163, 20);
			this.GB_BridgeSideTexture.TabIndex = 19;
			this.GB_BridgeSideTexture.TabStop = false;
			this.GB_BridgeSideTexture.Text = "Bridge Side Texture";
			// 
			// TB_BufferSize
			// 
			this.TB_BufferSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_BufferSize.EnterTriggersClick = false;
			this.TB_BufferSize.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_BufferSize.Image = ((System.Drawing.Image)(resources.GetObject("TB_BufferSize.Image")));
			this.TB_BufferSize.LabelText = "Buffer Size (m)";
			this.TB_BufferSize.Location = new System.Drawing.Point(179, 36);
			this.TB_BufferSize.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
			this.TB_BufferSize.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_BufferSize.MaxLength = 32767;
			this.TB_BufferSize.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_BufferSize.Name = "TB_BufferSize";
			this.TB_BufferSize.Password = false;
			this.TB_BufferSize.Placeholder = "Space from the curb to the next lane";
			this.TB_BufferSize.ReadOnly = false;
			this.TB_BufferSize.Required = false;
			this.TB_BufferSize.SelectAllOnFocus = false;
			this.TB_BufferSize.SelectedText = "";
			this.TB_BufferSize.SelectionLength = 0;
			this.TB_BufferSize.SelectionStart = 0;
			this.TB_BufferSize.Size = new System.Drawing.Size(163, 35);
			this.TB_BufferSize.TabIndex = 2;
			this.TB_BufferSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_BufferSize.Validation = SlickControls.ValidationType.Decimal;
			this.TB_BufferSize.ValidationCustom = null;
			this.TB_BufferSize.ValidationRegex = "";
			this.TB_BufferSize.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// TB_CustomText
			// 
			this.TB_CustomText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel6.SetColumnSpan(this.TB_CustomText, 2);
			this.TB_CustomText.EnterTriggersClick = false;
			this.TB_CustomText.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_CustomText.Image = ((System.Drawing.Image)(resources.GetObject("TB_CustomText.Image")));
			this.TB_CustomText.LabelText = "Text";
			this.TB_CustomText.Location = new System.Drawing.Point(531, 36);
			this.TB_CustomText.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
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
			this.TB_CustomText.Size = new System.Drawing.Size(339, 35);
			this.TB_CustomText.TabIndex = 2;
			this.TB_CustomText.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_CustomText.Validation = SlickControls.ValidationType.None;
			this.TB_CustomText.ValidationCustom = null;
			this.TB_CustomText.ValidationRegex = "";
			this.TB_CustomText.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// TB_SpeedLimit
			// 
			this.TB_SpeedLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_SpeedLimit.EnterTriggersClick = false;
			this.TB_SpeedLimit.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_SpeedLimit.Image = ((System.Drawing.Image)(resources.GetObject("TB_SpeedLimit.Image")));
			this.TB_SpeedLimit.LabelText = "Speed Limit";
			this.TB_SpeedLimit.Location = new System.Drawing.Point(355, 36);
			this.TB_SpeedLimit.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
			this.TB_SpeedLimit.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_SpeedLimit.MaxLength = 32767;
			this.TB_SpeedLimit.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_SpeedLimit.Name = "TB_SpeedLimit";
			this.TB_SpeedLimit.Password = false;
			this.TB_SpeedLimit.Placeholder = "Default speed of car lanes";
			this.TB_SpeedLimit.ReadOnly = false;
			this.TB_SpeedLimit.Required = false;
			this.TB_SpeedLimit.SelectAllOnFocus = false;
			this.TB_SpeedLimit.SelectedText = "";
			this.TB_SpeedLimit.SelectionLength = 0;
			this.TB_SpeedLimit.SelectionStart = 0;
			this.TB_SpeedLimit.Size = new System.Drawing.Size(163, 35);
			this.TB_SpeedLimit.TabIndex = 2;
			this.TB_SpeedLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_SpeedLimit.Validation = SlickControls.ValidationType.Number;
			this.TB_SpeedLimit.ValidationCustom = null;
			this.TB_SpeedLimit.ValidationRegex = "";
			this.TB_SpeedLimit.TextChanged += new System.EventHandler(this.TB_Name_TextChanged);
			// 
			// panel1
			// 
			this.TLP_Main.SetColumnSpan(this.panel1, 3);
			this.panel1.Controls.Add(this.slickScroll1);
			this.panel1.Controls.Add(this.P_Lanes);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(15, 134);
			this.panel1.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(880, 440);
			this.panel1.TabIndex = 8;
			// 
			// slickScroll1
			// 
			this.slickScroll1.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll1.LinkedControl = this.P_Lanes;
			this.slickScroll1.Location = new System.Drawing.Point(874, 0);
			this.slickScroll1.Name = "slickScroll1";
			this.slickScroll1.Size = new System.Drawing.Size(6, 440);
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
			this.slickSpacer1.Location = new System.Drawing.Point(18, 122);
			this.slickSpacer1.Name = "slickSpacer1";
			this.slickSpacer1.Size = new System.Drawing.Size(874, 1);
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
			this.tableLayoutPanel4.Location = new System.Drawing.Point(15, 81);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 7, 0, 5);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(880, 33);
			this.tableLayoutPanel4.TabIndex = 2;
			// 
			// slickSpacer3
			// 
			this.slickSpacer3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.slickSpacer3.Location = new System.Drawing.Point(590, 3);
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
			this.B_FlipLanes.Location = new System.Drawing.Point(695, 3);
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
			this.B_DuplicateFlip.Location = new System.Drawing.Point(601, 3);
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
			this.B_AddLane.Location = new System.Drawing.Point(789, 3);
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
			this.B_ClearLines.Location = new System.Drawing.Point(500, 3);
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
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "PC_MainPage";
			this.Padding = new System.Windows.Forms.Padding(5, 30, 0, 0);
			this.Size = new System.Drawing.Size(1186, 604);
			this.Text = "Thumbnail Maker";
			this.Controls.SetChildIndex(this.TLP_Main, 0);
			this.Controls.SetChildIndex(this.base_Text, 0);
			this.TLP_Main.ResumeLayout(false);
			this.TLP_Main.PerformLayout();
			this.TLP_Right.ResumeLayout(false);
			this.TLP_Right.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PB)).EndInit();
			this.FLP_Tags.ResumeLayout(false);
			this.FLP_Tags.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
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
		private SlickControls.SlickButton B_SaveThumb;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private SlickControls.SlickGroupBox GB_Region;
		private SlickControls.SlickTextBox TB_SpeedLimit;
		private SlickControls.SlickTextBox TB_CustomText;
		private System.Windows.Forms.TableLayoutPanel TLP_Right;
		private SlickControls.SlickButton B_EditName;
		private System.Windows.Forms.Label L_RoadName;
		private SlickControls.SlickTextBox TB_BufferSize;
		private SlickControls.SlickButton B_ClearLines;
		private SlickControls.SlickButton B_FlipLanes;
		private SlickControls.SlickButton B_DuplicateFlip;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private SlickControls.SlickSpacer slickSpacer2;
		private SlickControls.SlickButton B_Export;
		private SlickControls.SlickSpacer slickSpacer3;
		private Controls.RoadConfigContainer RCC;
		private SlickControls.SlickTextBox TB_RoadName;
		private Controls.OptionSelectionControl<RoadType> RoadTypeControl;
		private Controls.OptionSelectionControl<RegionType> RegionTypeControl;
		private Controls.OptionSelectionControl<TextureType> SideTextureControl;
		private Controls.OptionSelectionControl<BridgeTextureType> BridgeSideTextureControl;
		private Controls.OptionSelectionControl<AsphaltStyle> AsphaltTextureControl;
		private SlickControls.SlickGroupBox GB_RoadType;
		private SlickControls.SlickGroupBox GB_SideTexture;
		private SlickControls.SlickGroupBox GB_BridgeSideTexture;
		private SlickControls.SlickGroupBox GB_AsphaltTexture;
		private System.Windows.Forms.Label L_CurrentlyEditing;
		private System.Windows.Forms.Label L_RoadDesc;
		private SlickControls.SlickButton B_EditDesc;
		private SlickControls.SlickButton B_ViewSavedRoads;
		private SlickControls.SlickButton B_AddTag;
		private System.Windows.Forms.FlowLayoutPanel FLP_Tags;
		private System.Windows.Forms.Label L_NoTags;
		private SlickControls.SlickButton B_ClearCurrentlyEdited;
		private SlickControls.SlickSpacer slickSpacer4;
	}
}