using SlickControls;

using System;
using System.Windows.Forms;

namespace ThumbnailMaker.Controls
{
	internal class TagControl : SlickLabel
	{
		public event EventHandler SelectionChanged;

		public TagControl(string tag, bool display)
		{
			Image = Properties.Resources.I_Tag;
			Display = display;
			Text = tag;
			Cursor = Cursors.Hand;
			Margin = new Padding(1);
			Padding = new Padding(5);

			SlickTip.SetTo(this, Display ? "Filer by this tag" : "Remove this tag");
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.Button == MouseButtons.Left)
			{
				if (Display)
				{
					Selected = !Selected;
					SelectionChanged?.Invoke(this, EventArgs.Empty);
				}
				else
				{
					Dispose();
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (Display && Selected && disposing)
			{
				SelectionChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			if (!Display)
			{
				Image = Properties.Resources.I_RemoveTag;
			}

			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if (!Display)
			{
				Image = Properties.Resources.I_Tag;
			}

			base.OnMouseLeave(e);
		}
	}
}
