namespace ThumbnailMaker.Controls
{
	partial class RoadConfigContainer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoadConfigContainer));
			this.P_Container = new System.Windows.Forms.Panel();
			this.slickScroll2 = new SlickControls.SlickScroll();
			this.P_Configs = new System.Windows.Forms.FlowLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.slickSpacer4 = new SlickControls.SlickSpacer();
			this.TB_Search = new SlickControls.SlickTextBox();
			this.slickSpacer1 = new SlickControls.SlickSpacer();
			this.FLP_Tags = new System.Windows.Forms.FlowLayoutPanel();
			this.B_ClearFilters = new SlickControls.SlickButton();
			this.FLP_Options = new System.Windows.Forms.FlowLayoutPanel();
			this.B_Folder = new SlickControls.SlickButton();
			this.Loader = new SlickControls.SlickPictureBox();
			this.P_Container.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Loader)).BeginInit();
			this.SuspendLayout();
			// 
			// P_Container
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.P_Container, 3);
			this.P_Container.Controls.Add(this.slickScroll2);
			this.P_Container.Controls.Add(this.P_Configs);
			this.P_Container.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Container.Location = new System.Drawing.Point(7, 114);
			this.P_Container.Margin = new System.Windows.Forms.Padding(0);
			this.P_Container.Name = "P_Container";
			this.P_Container.Size = new System.Drawing.Size(570, 382);
			this.P_Container.TabIndex = 6;
			// 
			// slickScroll2
			// 
			this.slickScroll2.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll2.LinkedControl = this.P_Configs;
			this.slickScroll2.Location = new System.Drawing.Point(565, 0);
			this.slickScroll2.Name = "slickScroll2";
			this.slickScroll2.Size = new System.Drawing.Size(5, 382);
			this.slickScroll2.SmallHandle = true;
			this.slickScroll2.Style = SlickControls.StyleType.Vertical;
			this.slickScroll2.TabIndex = 7;
			this.slickScroll2.TabStop = false;
			this.slickScroll2.Text = "slickScroll2";
			// 
			// P_Configs
			// 
			this.P_Configs.AutoSize = true;
			this.P_Configs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.P_Configs.Location = new System.Drawing.Point(0, 0);
			this.P_Configs.Name = "P_Configs";
			this.P_Configs.Size = new System.Drawing.Size(0, 0);
			this.P_Configs.TabIndex = 6;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer4, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.TB_Search, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.P_Container, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.FLP_Tags, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.B_ClearFilters, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.FLP_Options, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.B_Folder, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(577, 496);
			this.tableLayoutPanel1.TabIndex = 9;
			// 
			// slickSpacer4
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.slickSpacer4, 3);
			this.slickSpacer4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.slickSpacer4.Location = new System.Drawing.Point(14, 113);
			this.slickSpacer4.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
			this.slickSpacer4.Name = "slickSpacer4";
			this.slickSpacer4.Size = new System.Drawing.Size(556, 1);
			this.slickSpacer4.TabIndex = 19;
			this.slickSpacer4.TabStop = false;
			this.slickSpacer4.Text = "slickSpacer4";
			// 
			// TB_Search
			// 
			this.TB_Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(235)))), ((int)(((byte)(243)))));
			this.TB_Search.Dock = System.Windows.Forms.DockStyle.Top;
			this.TB_Search.EnterTriggersClick = false;
			this.TB_Search.Image = ((System.Drawing.Image)(resources.GetObject("TB_Search.Image")));
			this.TB_Search.LabelText = "Search Configurations";
			this.TB_Search.Location = new System.Drawing.Point(10, 10);
			this.TB_Search.Margin = new System.Windows.Forms.Padding(3, 10, 5, 5);
			this.TB_Search.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Search.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Padding = new System.Windows.Forms.Padding(4, 20, 24, 4);
			this.TB_Search.Placeholder = "Type in to search your configurations";
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.Size = new System.Drawing.Size(490, 44);
			this.TB_Search.TabIndex = 3;
			this.TB_Search.Validation = SlickControls.ValidationType.Decimal;
			this.TB_Search.TextChanged += new System.EventHandler(this.TB_Search_TextChanged);
			// 
			// slickSpacer1
			// 
			this.slickSpacer1.Dock = System.Windows.Forms.DockStyle.Left;
			this.slickSpacer1.Location = new System.Drawing.Point(3, 3);
			this.slickSpacer1.Name = "slickSpacer1";
			this.tableLayoutPanel1.SetRowSpan(this.slickSpacer1, 5);
			this.slickSpacer1.Size = new System.Drawing.Size(1, 490);
			this.slickSpacer1.TabIndex = 7;
			this.slickSpacer1.TabStop = false;
			this.slickSpacer1.Text = "slickSpacer1";
			// 
			// FLP_Tags
			// 
			this.FLP_Tags.AutoSize = true;
			this.FLP_Tags.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.SetColumnSpan(this.FLP_Tags, 3);
			this.FLP_Tags.Dock = System.Windows.Forms.DockStyle.Top;
			this.FLP_Tags.Location = new System.Drawing.Point(7, 86);
			this.FLP_Tags.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_Tags.Name = "FLP_Tags";
			this.FLP_Tags.Size = new System.Drawing.Size(570, 0);
			this.FLP_Tags.TabIndex = 8;
			// 
			// B_ClearFilters
			// 
			this.B_ClearFilters.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_ClearFilters.ColorShade = null;
			this.B_ClearFilters.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ClearFilters.HandleUiScale = false;
			this.B_ClearFilters.Image = ((System.Drawing.Image)(resources.GetObject("B_ClearFilters.Image")));
			this.B_ClearFilters.Location = new System.Drawing.Point(544, 14);
			this.B_ClearFilters.Name = "B_ClearFilters";
			this.B_ClearFilters.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_ClearFilters.Size = new System.Drawing.Size(30, 30);
			this.B_ClearFilters.SpaceTriggersClick = true;
			this.B_ClearFilters.TabIndex = 17;
			this.B_ClearFilters.Click += new System.EventHandler(this.B_ClearCurrentlyEdited_Click);
			// 
			// FLP_Options
			// 
			this.FLP_Options.AutoSize = true;
			this.FLP_Options.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.SetColumnSpan(this.FLP_Options, 3);
			this.FLP_Options.Dock = System.Windows.Forms.DockStyle.Top;
			this.FLP_Options.Location = new System.Drawing.Point(7, 59);
			this.FLP_Options.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_Options.Name = "FLP_Options";
			this.FLP_Options.Size = new System.Drawing.Size(570, 0);
			this.FLP_Options.TabIndex = 20;
			// 
			// B_Folder
			// 
			this.B_Folder.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_Folder.ColorShade = null;
			this.B_Folder.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Folder.HandleUiScale = false;
			this.B_Folder.Image = global::ThumbnailMaker.Properties.Resources.I_Folder;
			this.B_Folder.Location = new System.Drawing.Point(508, 14);
			this.B_Folder.Name = "B_Folder";
			this.B_Folder.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Folder.Size = new System.Drawing.Size(30, 30);
			this.B_Folder.SpaceTriggersClick = true;
			this.B_Folder.TabIndex = 17;
			this.B_Folder.Click += new System.EventHandler(this.B_Folder_Click);
			// 
			// Loader
			// 
			this.Loader.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.Loader.Location = new System.Drawing.Point(272, 232);
			this.Loader.Name = "Loader";
			this.Loader.Size = new System.Drawing.Size(32, 32);
			this.Loader.TabIndex = 8;
			this.Loader.TabStop = false;
			// 
			// RoadConfigContainer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.Loader);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "RoadConfigContainer";
			this.Size = new System.Drawing.Size(577, 496);
			this.P_Container.ResumeLayout(false);
			this.P_Container.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Loader)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel P_Container;
		private SlickControls.SlickScroll slickScroll2;
		public System.Windows.Forms.FlowLayoutPanel P_Configs;
		private SlickControls.SlickPictureBox Loader;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private SlickControls.SlickSpacer slickSpacer1;
		private System.Windows.Forms.FlowLayoutPanel FLP_Tags;
		private SlickControls.SlickButton B_ClearFilters;
		private SlickControls.SlickSpacer slickSpacer4;
		private System.Windows.Forms.FlowLayoutPanel FLP_Options;
		internal SlickControls.SlickTextBox TB_Search;
		private SlickControls.SlickButton B_Folder;
	}
}
