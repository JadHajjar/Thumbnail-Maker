using Extensions;
using SlickControls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThumbnailMaker.Controls
{
	public partial class AddTagForm : Form
	{
		public event System.EventHandler<string> TagAdded;
		public event System.EventHandler<string> TagRemoved;

		public AddTagForm()
		{
			InitializeComponent();

			BackColor = FormDesign.Design.ActiveColor;
			ForeColor = FormDesign.Design.ForeColor;
			TLP.BackColor = FormDesign.Design.BackColor;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Enter)
			{
				B_AddTag_Click(null, null);
				return true;
			}

			if (keyData == Keys.Escape)
			{
				Close();
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);

			Close();
		}

		private void B_AddTag_Click(object sender, EventArgs e)
		{
			foreach (var item in FLP_Tags.Controls.OfType<TagControl>().Where(x => x.Text.Equals(TB_Name.Text, StringComparison.CurrentCultureIgnoreCase)))
			{
				item.Dispose();
			}

			var ctrl = new TagControl(TB_Name.Text, true);

			FLP_Tags.Controls.Add(ctrl);
			FLP_Tags.OrderBy(x => x.Text);

			ctrl.SelectionChanged += Ctrl_Selected;
			ctrl.Disposed += Ctrl_Disposed;

			ctrl.Selected = true;

			TB_Name.Text = string.Empty;
		}

		private void Ctrl_Disposed(object sender, EventArgs e)
		{
			TagRemoved?.Invoke(this, (sender as Control).Text);
		}

		private void Ctrl_Selected(object sender, EventArgs e)
		{
			if ((sender as TagControl).Selected)
				TagAdded?.Invoke(this, (sender as Control).Text);
			else
				TagRemoved?.Invoke(this, (sender as Control).Text);
		}
	}
}
