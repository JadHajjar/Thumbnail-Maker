namespace ThumbnailMaker
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.panelItem1 = new SlickControls.PanelItem();
			this.panelItem2 = new SlickControls.PanelItem();
			this.base_P_Container.SuspendLayout();
			this.SuspendLayout();
			// 
			// base_P_Content
			// 
			this.base_P_Content.Size = new System.Drawing.Size(1137, 612);
			// 
			// base_P_SideControls
			// 
			this.base_P_SideControls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(129)))), ((int)(((byte)(150)))));
			this.base_P_SideControls.Location = new System.Drawing.Point(0, 342);
			// 
			// base_P_Container
			// 
			this.base_P_Container.Size = new System.Drawing.Size(1139, 614);
			// 
			// panelItem1
			// 
			this.panelItem1.ForceReopen = false;
			this.panelItem1.Group = "Tabs";
			this.panelItem1.Highlighted = false;
			this.panelItem1.Icon = ((System.Drawing.Bitmap)(resources.GetObject("panelItem1.Icon")));
			this.panelItem1.Selected = true;
			this.panelItem1.Text = "Main";
			this.panelItem1.OnClick += new System.Windows.Forms.MouseEventHandler(this.panelItem1_OnClick);
			// 
			// panelItem2
			// 
			this.panelItem2.ForceReopen = false;
			this.panelItem2.Group = "Tabs";
			this.panelItem2.Highlighted = false;
			this.panelItem2.Icon = ((System.Drawing.Bitmap)(resources.GetObject("panelItem2.Icon")));
			this.panelItem2.Selected = false;
			this.panelItem2.Text = "Options";
			this.panelItem2.OnClick += new System.Windows.Forms.MouseEventHandler(this.panelItem2_OnClick);
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1150, 625);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(58)))), ((int)(((byte)(69)))));
			this.FormIcon = ((System.Drawing.Image)(resources.GetObject("$this.FormIcon")));
			this.HideMenu = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IconBounds = new System.Drawing.Rectangle(3, 21, 14, 42);
			this.MaximizeBox = true;
			this.MaximizedBounds = new System.Drawing.Rectangle(0, 0, 1920, 1032);
			this.MinimizeBox = true;
			this.Name = "MainForm";
			this.SidebarItems = new SlickControls.PanelItem[] {
        this.panelItem1,
        this.panelItem2};
			this.Text = "Thumbnail Maker";
			this.base_P_Container.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private SlickControls.PanelItem panelItem1;
		private SlickControls.PanelItem panelItem2;
	}
}