namespace ThumbnailMaker
{
	partial class SaveThumbDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveThumbDialog));
			this.CB_Small = new SlickControls.SlickCheckbox();
			this.CB_Tooltip = new SlickControls.SlickCheckbox();
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Close)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Max)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Min)).BeginInit();
			this.SuspendLayout();
			// 
			// CB_Small
			// 
			this.CB_Small.AutoSize = true;
			this.CB_Small.Checked = false;
			this.CB_Small.CheckedText = null;
			this.CB_Small.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_Small.DefaultValue = false;
			this.CB_Small.Location = new System.Drawing.Point(341, 212);
			this.CB_Small.Name = "CB_Small";
			this.CB_Small.Size = new System.Drawing.Size(126, 26);
			this.CB_Small.TabIndex = 2;
			this.CB_Small.Text = "Small Thumbnail";
			this.CB_Small.UncheckedText = null;
			// 
			// CB_Tooltip
			// 
			this.CB_Tooltip.AutoSize = true;
			this.CB_Tooltip.Checked = false;
			this.CB_Tooltip.CheckedText = null;
			this.CB_Tooltip.Cursor = System.Windows.Forms.Cursors.Hand;
			this.CB_Tooltip.DefaultValue = false;
			this.CB_Tooltip.Location = new System.Drawing.Point(337, 212);
			this.CB_Tooltip.Name = "CB_Tooltip";
			this.CB_Tooltip.Size = new System.Drawing.Size(134, 26);
			this.CB_Tooltip.TabIndex = 3;
			this.CB_Tooltip.Text = "Tooltip Thumbnail";
			this.CB_Tooltip.UncheckedText = null;
			// 
			// SaveThumbDialog
			// 
			this.Controls.Add(this.CB_Tooltip);
			this.Controls.Add(this.CB_Small);
			this.Name = "SaveThumbDialog";
			this.Text = "SaveThumbDialog";
			this.Controls.SetChildIndex(this.base_P_Container, 0);
			this.Controls.SetChildIndex(this.CB_Small, 0);
			this.Controls.SetChildIndex(this.CB_Tooltip, 0);
			((System.ComponentModel.ISupportInitialize)(this.base_PB_Icon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Close)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Max)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.base_B_Min)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public SlickControls.SlickCheckbox CB_Small;
		public SlickControls.SlickCheckbox CB_Tooltip;
	}
}