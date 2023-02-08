using Extensions;

using SlickControls;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ThumbnailMaker.Controls
{
	internal class TagControl : SlickLabel
	{
		public bool InvertSelected { get; private set; }

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
					InvertSelected = false;
					SelectionChanged?.Invoke(this, EventArgs.Empty);
				}
				else
				{
					Dispose();
				}
			}
			else if (Display && e.Button == MouseButtons.Right)
			{
				InvertSelected = !InvertSelected;
				Selected = false;
				SelectionChanged?.Invoke(this, EventArgs.Empty);
			}
			else if (Display && e.Button == MouseButtons.Middle)
			{
				InvertSelected = false;
				Selected = false;
				SelectionChanged?.Invoke(this, EventArgs.Empty);
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

		protected override void GetColors(out Color fore, out Color back)
		{
			if (InvertSelected)
			{
				fore = ActiveColor == null ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ActiveForeColor.Tint(ActiveColor());
				back = ActiveColor == null ? FormDesign.Design.RedColor : ActiveColor();
			}
			else
			{
				base.GetColors(out fore, out back);
			}
		}
	}
}
