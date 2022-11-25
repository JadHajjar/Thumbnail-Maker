namespace ThumbnailMaker.Controls
{
	partial class RoadOptionsEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoadOptionsEditor));
			this.TB_LaneWidth = new SlickControls.SlickTextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.TB_Speed = new SlickControls.SlickTextBox();
			this.TB_Vertical = new SlickControls.SlickTextBox();
			this.B_Apply = new SlickControls.SlickButton();
			this.CB_BusStop = new SlickControls.SlickCheckbox();
			this.base_P_Content.SuspendLayout();
			this.base_P_Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Content
			// 
			this.base_P_Content.Controls.Add(this.tableLayoutPanel1);
			this.base_P_Content.Location = new System.Drawing.Point(1, 28);
			this.base_P_Content.Size = new System.Drawing.Size(345, 248);
			// 
			// base_P_Controls
			// 
			this.base_P_Controls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(63)))), ((int)(((byte)(79)))));
			this.base_P_Controls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(202)))), ((int)(((byte)(218)))));
			this.base_P_Controls.Location = new System.Drawing.Point(1, 30);
			// 
			// base_P_Top_Spacer
			// 
			this.base_P_Top_Spacer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(63)))), ((int)(((byte)(79)))));
			this.base_P_Top_Spacer.Size = new System.Drawing.Size(345, 3);
			// 
			// base_P_Container
			// 
			this.base_P_Container.Size = new System.Drawing.Size(347, 277);
			// 
			// base_PB_Icon
			// 
			this.base_PB_Icon.Image = global::ThumbnailMaker.Properties.Resources.I_Edit;
			// 
			// TB_LaneWidth
			// 
			this.TB_LaneWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_LaneWidth.EnterTriggersClick = false;
			this.TB_LaneWidth.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_LaneWidth.Image = null;
			this.TB_LaneWidth.LabelText = "Lane Width";
			this.TB_LaneWidth.Location = new System.Drawing.Point(15, 15);
			this.TB_LaneWidth.Margin = new System.Windows.Forms.Padding(15);
			this.TB_LaneWidth.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_LaneWidth.MaxLength = 32767;
			this.TB_LaneWidth.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_LaneWidth.Name = "TB_LaneWidth";
			this.TB_LaneWidth.Password = false;
			this.TB_LaneWidth.Placeholder = null;
			this.TB_LaneWidth.ReadOnly = false;
			this.TB_LaneWidth.Required = false;
			this.TB_LaneWidth.SelectAllOnFocus = false;
			this.TB_LaneWidth.SelectedText = "";
			this.TB_LaneWidth.SelectionLength = 0;
			this.TB_LaneWidth.SelectionStart = 0;
			this.TB_LaneWidth.Size = new System.Drawing.Size(315, 40);
			this.TB_LaneWidth.TabIndex = 0;
			this.TB_LaneWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_LaneWidth.Validation = SlickControls.ValidationType.Decimal;
			this.TB_LaneWidth.ValidationCustom = null;
			this.TB_LaneWidth.ValidationRegex = "";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.TB_Speed, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.TB_LaneWidth, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.TB_Vertical, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.B_Apply, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.CB_BusStop, 0, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(345, 248);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// TB_Speed
			// 
			this.TB_Speed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_Speed.EnterTriggersClick = false;
			this.TB_Speed.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Speed.Image = null;
			this.TB_Speed.LabelText = "Lane Speed Limit";
			this.TB_Speed.Location = new System.Drawing.Point(15, 125);
			this.TB_Speed.Margin = new System.Windows.Forms.Padding(15, 0, 15, 15);
			this.TB_Speed.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Speed.MaxLength = 32767;
			this.TB_Speed.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_Speed.Name = "TB_Speed";
			this.TB_Speed.Password = false;
			this.TB_Speed.Placeholder = "Default speed is based on the global value";
			this.TB_Speed.ReadOnly = false;
			this.TB_Speed.Required = false;
			this.TB_Speed.SelectAllOnFocus = false;
			this.TB_Speed.SelectedText = "";
			this.TB_Speed.SelectionLength = 0;
			this.TB_Speed.SelectionStart = 0;
			this.TB_Speed.Size = new System.Drawing.Size(315, 40);
			this.TB_Speed.TabIndex = 2;
			this.TB_Speed.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_Speed.Validation = SlickControls.ValidationType.Decimal;
			this.TB_Speed.ValidationCustom = null;
			this.TB_Speed.ValidationRegex = "";
			// 
			// TB_Vertical
			// 
			this.TB_Vertical.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_Vertical.EnterTriggersClick = false;
			this.TB_Vertical.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Vertical.Image = null;
			this.TB_Vertical.LabelText = "Lane Vertical Offset";
			this.TB_Vertical.Location = new System.Drawing.Point(15, 70);
			this.TB_Vertical.Margin = new System.Windows.Forms.Padding(15, 0, 15, 15);
			this.TB_Vertical.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Vertical.MaxLength = 32767;
			this.TB_Vertical.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_Vertical.Name = "TB_Vertical";
			this.TB_Vertical.Password = false;
			this.TB_Vertical.Placeholder = "Default offset is  -0.3 for normal roads, 0 for the rest";
			this.TB_Vertical.ReadOnly = false;
			this.TB_Vertical.Required = false;
			this.TB_Vertical.SelectAllOnFocus = false;
			this.TB_Vertical.SelectedText = "";
			this.TB_Vertical.SelectionLength = 0;
			this.TB_Vertical.SelectionStart = 0;
			this.TB_Vertical.Size = new System.Drawing.Size(315, 40);
			this.TB_Vertical.TabIndex = 1;
			this.TB_Vertical.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_Vertical.Validation = SlickControls.ValidationType.Decimal;
			this.TB_Vertical.ValidationCustom = null;
			this.TB_Vertical.ValidationRegex = "";
			// 
			// B_Apply
			// 
			this.B_Apply.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.B_Apply.ColorShade = null;
			this.B_Apply.Cursor = System.Windows.Forms.Cursors.Hand;
			this.B_Apply.IconSize = 16;
			this.B_Apply.Image = ((System.Drawing.Image)(resources.GetObject("B_Apply.Image")));
			this.B_Apply.Location = new System.Drawing.Point(122, 231);
			this.B_Apply.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
			this.B_Apply.Name = "B_Apply";
			this.B_Apply.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.B_Apply.Size = new System.Drawing.Size(100, 14);
			this.B_Apply.SpaceTriggersClick = true;
			this.B_Apply.TabIndex = 4;
			this.B_Apply.Text = "Apply";
			this.B_Apply.Click += new System.EventHandler(this.B_Apply_Click);
			// 
			// CB_BusStop
			// 
			this.CB_BusStop.ActiveColor = null;
			this.CB_BusStop.AutoSize = true;
			this.CB_BusStop.Center = false;
			this.CB_BusStop.Checked = false;
			this.CB_BusStop.CheckedText = null;
			this.CB_BusStop.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_BusStop.HideText = false;
			this.CB_BusStop.IconSize = 16;
			this.CB_BusStop.Image = ((System.Drawing.Image)(resources.GetObject("CB_BusStop.Image")));
			this.CB_BusStop.Location = new System.Drawing.Point(15, 180);
			this.CB_BusStop.Margin = new System.Windows.Forms.Padding(15, 0, 15, 15);
			this.CB_BusStop.Name = "CB_BusStop";
			this.CB_BusStop.Padding = new System.Windows.Forms.Padding(7, 5, 7, 5);
			this.CB_BusStop.Size = new System.Drawing.Size(149, 26);
			this.CB_BusStop.TabIndex = 3;
			this.CB_BusStop.Text = "Add Stop to this lane";
			this.CB_BusStop.UncheckedText = null;
			// 
			// RoadOptionsEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(358, 288);
			this.FormIcon = global::ThumbnailMaker.Properties.Resources.I_Edit;
			this.MaximizeBox = false;
			this.MaximizedBounds = new System.Drawing.Rectangle(0, 0, 1920, 1032);
			this.MinimizeBox = false;
			this.Name = "RoadOptionsEditor";
			this.SeemlessBar = true;
			this.Text = "Edit Lane Properties";
			this.Deactivate += new System.EventHandler(this.RoadOptionsEditor_Deactivate);
			this.base_P_Content.ResumeLayout(false);
			this.base_P_Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private SlickControls.SlickTextBox TB_Vertical;
		private SlickControls.SlickTextBox TB_LaneWidth;
		private SlickControls.SlickButton B_Apply;
		private SlickControls.SlickTextBox TB_Speed;
		private SlickControls.SlickCheckbox CB_BusStop;
	}
}