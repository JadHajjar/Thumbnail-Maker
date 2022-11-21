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
			this.panel1 = new System.Windows.Forms.Panel();
			this.TB_Size = new SlickControls.SlickTextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.P_Configs = new System.Windows.Forms.Panel();
			this.slickScroll2 = new SlickControls.SlickScroll();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.TB_Size);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			this.panel1.Size = new System.Drawing.Size(266, 51);
			this.panel1.TabIndex = 0;
			// 
			// TB_Size
			// 
			this.TB_Size.Dock = System.Windows.Forms.DockStyle.Top;
			this.TB_Size.EnterTriggersClick = false;
			this.TB_Size.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Size.Image = ((System.Drawing.Image)(resources.GetObject("TB_Size.Image")));
			this.TB_Size.LabelText = "Search Configurations";
			this.TB_Size.Location = new System.Drawing.Point(10, 10);
			this.TB_Size.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Size.MaxLength = 32767;
			this.TB_Size.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_Size.Name = "TB_Size";
			this.TB_Size.Password = false;
			this.TB_Size.Placeholder = "Type in to search your configurations";
			this.TB_Size.ReadOnly = false;
			this.TB_Size.Required = false;
			this.TB_Size.SelectAllOnFocus = false;
			this.TB_Size.SelectedText = "";
			this.TB_Size.SelectionLength = 0;
			this.TB_Size.SelectionStart = 0;
			this.TB_Size.Size = new System.Drawing.Size(246, 41);
			this.TB_Size.TabIndex = 3;
			this.TB_Size.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.TB_Size.Validation = SlickControls.ValidationType.Decimal;
			this.TB_Size.ValidationCustom = null;
			this.TB_Size.ValidationRegex = "";
			this.TB_Size.TextChanged += new System.EventHandler(this.TB_Size_TextChanged);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.slickScroll2);
			this.panel2.Controls.Add(this.P_Configs);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 51);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(266, 229);
			this.panel2.TabIndex = 6;
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
			// slickScroll2
			// 
			this.slickScroll2.Dock = System.Windows.Forms.DockStyle.Right;
			this.slickScroll2.LinkedControl = this.P_Configs;
			this.slickScroll2.Location = new System.Drawing.Point(260, 0);
			this.slickScroll2.Name = "slickScroll2";
			this.slickScroll2.Size = new System.Drawing.Size(6, 229);
			this.slickScroll2.Style = SlickControls.StyleType.Vertical;
			this.slickScroll2.TabIndex = 7;
			this.slickScroll2.TabStop = false;
			this.slickScroll2.Text = "slickScroll2";
			// 
			// RoadConfigContainer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "RoadConfigContainer";
			this.Size = new System.Drawing.Size(266, 280);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private SlickControls.SlickTextBox TB_Size;
		private System.Windows.Forms.Panel panel2;
		private SlickControls.SlickScroll slickScroll2;
		private System.Windows.Forms.Panel P_Configs;
	}
}
