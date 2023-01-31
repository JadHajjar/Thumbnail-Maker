using Extensions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ThumbnailMaker.Controls
{
	public partial class AddTagForm : Form
	{
		public List<string> LoadedTags { get; }

		public event System.EventHandler<string> TagAdded;
		public event System.EventHandler<string> TagRemoved;

		public AddTagForm(List<string> loadedTags, IEnumerable<string> currentTags)
		{
			InitializeComponent();

			BackColor = FormDesign.Design.ActiveColor;
			ForeColor = FormDesign.Design.ForeColor;
			TLP.BackColor = FormDesign.Design.BackColor;

			LoadedTags = loadedTags;

			foreach (var item in loadedTags.Concat(currentTags).Distinct((x, y) => x.Equals(y, StringComparison.CurrentCultureIgnoreCase)))
			{
				var ctrl = new TagControl(item, true) { Selected = currentTags.Contains(item, StringComparer.CurrentCultureIgnoreCase) };

				FLP_Tags.Controls.Add(ctrl);
				FLP_Tags.OrderBy(x => x.Text);

				ctrl.SelectionChanged += Ctrl_Selected;
				ctrl.Disposed += Ctrl_Disposed;
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Enter)
			{
				if (string.IsNullOrWhiteSpace(TB_Name.Text))
				{
					Close();
				}
				else
				{
					B_AddTag_Click(null, null);
				}

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
			if (string.IsNullOrWhiteSpace(TB_Name.Text))
			{
				return;
			}

			foreach (var item in FLP_Tags.Controls.OfType<TagControl>().Where(x => x.Text.Equals(TB_Name.Text, StringComparison.CurrentCultureIgnoreCase)))
			{
				item.Dispose();
			}

			var ctrl = new TagControl(TB_Name.Text.Trim(), true) { Selected = true };

			FLP_Tags.Controls.Add(ctrl);
			FLP_Tags.OrderBy(x => x.Text);

			ctrl.SelectionChanged += Ctrl_Selected;
			ctrl.Disposed += Ctrl_Disposed;

			TagAdded?.Invoke(this, ctrl.Text);

			TB_Name.Text = string.Empty;
		}

		private void Ctrl_Disposed(object sender, EventArgs e)
		{
			if (!Disposing)
			{
				TagRemoved?.Invoke(this, (sender as Control).Text);
			}
		}

		private void Ctrl_Selected(object sender, EventArgs e)
		{
			if (!Disposing)
			{
				if ((sender as TagControl).Selected)
				{
					TagAdded?.Invoke(this, (sender as Control).Text);
				}
				else
				{
					TagRemoved?.Invoke(this, (sender as Control).Text);
				}
			}
		}

		private void FLP_Tags_Resize(object sender, EventArgs e)
		{
			Height = FLP_Tags.Height + FLP_Tags.Top;
		}
	}
}
