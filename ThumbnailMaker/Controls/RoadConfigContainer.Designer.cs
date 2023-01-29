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
			this.B_ClearCurrentlyEdited = new SlickControls.SlickButton();
			this.Loader = new SlickControls.SlickPictureBox();
			this.TLP_Options = new System.Windows.Forms.TableLayoutPanel();
			this.P_Container.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Loader)).BeginInit();
			this.SuspendLayout();
			// 
			// P_Container
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.P_Container, 2);
			this.P_Container.Controls.Add(this.slickScroll2);
			this.P_Container.Controls.Add(this.P_Configs);
			this.P_Container.Dock = System.Windows.Forms.DockStyle.Fill;
			this.P_Container.Location = new System.Drawing.Point(7, 55);
			this.P_Container.Margin = new System.Windows.Forms.Padding(0);
			this.P_Container.Name = "P_Container";
			this.P_Container.Size = new System.Drawing.Size(570, 441);
			this.P_Container.TabIndex = 6;
			// 
			// slickScroll2
			// 
			this.slickScroll2.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll2.LinkedControl = this.P_Configs;
			this.slickScroll2.Location = new System.Drawing.Point(564, 0);
			this.slickScroll2.Name = "slickScroll2";
			this.slickScroll2.Size = new System.Drawing.Size(6, 441);
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
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer4, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.TB_Search, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.P_Container, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.slickSpacer1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.FLP_Tags, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.B_ClearCurrentlyEdited, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.TLP_Options, 1, 1);
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
			this.tableLayoutPanel1.SetColumnSpan(this.slickSpacer4, 2);
			this.slickSpacer4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.slickSpacer4.Location = new System.Drawing.Point(14, 54);
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
			this.TB_Search.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Search.Image = ((System.Drawing.Image)(resources.GetObject("TB_Search.Image")));
			this.TB_Search.LabelText = "Search Configurations";
			this.TB_Search.Location = new System.Drawing.Point(17, 10);
			this.TB_Search.Margin = new System.Windows.Forms.Padding(10, 10, 10, 5);
			this.TB_Search.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Search.MaxLength = 32767;
			this.TB_Search.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_Search.Name = "TB_Search";
			this.TB_Search.Password = false;
			this.TB_Search.Placeholder = "Type in to search your configurations";
			this.TB_Search.ReadOnly = false;
			this.TB_Search.Required = false;
			this.TB_Search.SelectAllOnFocus = false;
			this.TB_Search.SelectedText = "";
			this.TB_Search.SelectionLength = 0;
			this.TB_Search.SelectionStart = 0;
			this.TB_Search.Size = new System.Drawing.Size(506, 39);
			this.TB_Search.TabIndex = 3;
			this.TB_Search.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_Search.Validation = SlickControls.ValidationType.Decimal;
			this.TB_Search.ValidationCustom = null;
			this.TB_Search.ValidationRegex = "";
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
			this.tableLayoutPanel1.SetColumnSpan(this.FLP_Tags, 2);
			this.FLP_Tags.Dock = System.Windows.Forms.DockStyle.Top;
			this.FLP_Tags.Location = new System.Drawing.Point(7, 54);
			this.FLP_Tags.Margin = new System.Windows.Forms.Padding(0);
			this.FLP_Tags.Name = "FLP_Tags";
			this.FLP_Tags.Size = new System.Drawing.Size(570, 0);
			this.FLP_Tags.TabIndex = 8;
			// 
			// B_ClearCurrentlyEdited
			// 
			this.B_ClearCurrentlyEdited.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.B_ClearCurrentlyEdited.ColorShade = null;
			this.B_ClearCurrentlyEdited.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_ClearCurrentlyEdited.HandleUiScale = false;
			this.B_ClearCurrentlyEdited.IconSize = 16;
			this.B_ClearCurrentlyEdited.Image = global::ThumbnailMaker.Properties.Resources.I_Cancel;
			this.B_ClearCurrentlyEdited.Location = new System.Drawing.Point(540, 12);
			this.B_ClearCurrentlyEdited.Margin = new System.Windows.Forms.Padding(7);
			this.B_ClearCurrentlyEdited.Name = "B_ClearCurrentlyEdited";
			this.B_ClearCurrentlyEdited.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_ClearCurrentlyEdited.Size = new System.Drawing.Size(30, 30);
			this.B_ClearCurrentlyEdited.SpaceTriggersClick = true;
			this.B_ClearCurrentlyEdited.TabIndex = 17;
			this.B_ClearCurrentlyEdited.Click += new System.EventHandler(this.B_ClearCurrentlyEdited_Click);
			// 
			// Loader
			// 
			this.Loader.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.Loader.Loading = true;
			this.Loader.Location = new System.Drawing.Point(272, 232);
			this.Loader.Name = "Loader";
			this.Loader.Size = new System.Drawing.Size(32, 32);
			this.Loader.TabIndex = 8;
			this.Loader.TabStop = false;
			// 
			// TLP_Options
			// 
			this.TLP_Options.AutoSize = true;
			this.TLP_Options.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP_Options.ColumnCount = 2;
			this.tableLayoutPanel1.SetColumnSpan(this.TLP_Options, 2);
			this.TLP_Options.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Options.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TLP_Options.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP_Options.Location = new System.Drawing.Point(7, 54);
			this.TLP_Options.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.TLP_Options.Name = "TLP_Options";
			this.TLP_Options.RowCount = 1;
			this.TLP_Options.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP_Options.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP_Options.Size = new System.Drawing.Size(570, 0);
			this.TLP_Options.TabIndex = 20;
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
		private SlickControls.SlickTextBox TB_Search;
		private System.Windows.Forms.Panel P_Container;
		private SlickControls.SlickScroll slickScroll2;
		public System.Windows.Forms.FlowLayoutPanel P_Configs;
		private SlickControls.SlickPictureBox Loader;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private SlickControls.SlickSpacer slickSpacer1;
		private System.Windows.Forms.FlowLayoutPanel FLP_Tags;
		private SlickControls.SlickButton B_ClearCurrentlyEdited;
		private SlickControls.SlickSpacer slickSpacer4;
		private System.Windows.Forms.TableLayoutPanel TLP_Options;
	}
}
