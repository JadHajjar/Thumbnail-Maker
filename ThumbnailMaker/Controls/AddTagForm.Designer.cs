using System.Windows.Forms;

namespace ThumbnailMaker.Controls
{
	partial class AddTagForm
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
			this.TLP = new System.Windows.Forms.TableLayoutPanel();
			this.TB_Name = new SlickControls.SlickTextBox();
			this.FLP_Tags = new System.Windows.Forms.FlowLayoutPanel();
			this.TLP.SuspendLayout();
			this.SuspendLayout();
			// 
			// TLP
			// 
			this.TLP.AutoSize = true;
			this.TLP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP.ColumnCount = 2;
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.Controls.Add(this.TB_Name, 0, 0);
			this.TLP.Controls.Add(this.FLP_Tags, 0, 2);
			this.TLP.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP.Location = new System.Drawing.Point(0, 0);
			this.TLP.Name = "TLP";
			this.TLP.RowCount = 3;
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.Size = new System.Drawing.Size(200, 56);
			this.TLP.TabIndex = 1;
			// 
			// TB_Name
			// 
			this.TB_Name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(235)))), ((int)(((byte)(243)))));
			this.TB_Name.Dock = System.Windows.Forms.DockStyle.Top;
			this.TB_Name.EnterTriggersClick = false;
			this.TB_Name.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Name.Image = global::ThumbnailMaker.Properties.Resources.I_Add;
			this.TB_Name.LabelText = "Custom Tag";
			this.TB_Name.Location = new System.Drawing.Point(7, 0);
			this.TB_Name.Margin = new System.Windows.Forms.Padding(7, 0, 5, 5);
			this.TB_Name.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Name.MinimumSize = new System.Drawing.Size(50, 0);
			this.TB_Name.Name = "TB_Name";
			this.TB_Name.Padding = new System.Windows.Forms.Padding(4, 20, 4, 4);
			this.TB_Name.Placeholder = "Enter a new tag here";
			this.TB_Name.SelectedText = "";
			this.TB_Name.SelectionLength = 0;
			this.TB_Name.SelectionStart = 0;
			this.TB_Name.Size = new System.Drawing.Size(188, 45);
			this.TB_Name.TabIndex = 1;
			this.TB_Name.IconClicked += new System.EventHandler(this.B_AddTag_Click);
			// 
			// FLP_Tags
			// 
			this.FLP_Tags.AutoSize = true;
			this.FLP_Tags.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP.SetColumnSpan(this.FLP_Tags, 2);
			this.FLP_Tags.Dock = System.Windows.Forms.DockStyle.Top;
			this.FLP_Tags.Location = new System.Drawing.Point(3, 53);
			this.FLP_Tags.Name = "FLP_Tags";
			this.FLP_Tags.Size = new System.Drawing.Size(194, 0);
			this.FLP_Tags.TabIndex = 4;
			this.FLP_Tags.Resize += new System.EventHandler(this.FLP_Tags_Resize);
			// 
			// AddTagForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(200, 180);
			this.Controls.Add(this.TLP);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "AddTagForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "AddTagForm";
			this.TLP.ResumeLayout(false);
			this.TLP.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TableLayoutPanel TLP;
		private SlickControls.SlickTextBox TB_Name;
		private FlowLayoutPanel FLP_Tags;
	}
}