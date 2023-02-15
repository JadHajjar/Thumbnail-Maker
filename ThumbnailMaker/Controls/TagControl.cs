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

		protected override void OnPaint(PaintEventArgs e)
		{
			GetColors(out var fore, out var back);
			e.Graphics.Clear(BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (HoverState >= HoverState.Hovered || Selected || InvertSelected)
				e.Graphics.FillRoundedRectangle(Gradient(back), new Rectangle(1, 1, Width - 3, Height - 3), 7);

			if (!HoverState.HasFlag(HoverState.Pressed))
				DrawFocus(e.Graphics, new Rectangle(1, 1, Width - 3, Height - 3), 7, ActiveColor == null ? FormDesign.Design.ActiveColor : ActiveColor());

			if (Loading)
			{
				if (HideText || string.IsNullOrWhiteSpace(Text))
					DrawLoader(e.Graphics, new Rectangle((Width - iconSize) / 2, (int)((Height - iconSize) / 2F), iconSize, iconSize), fore);
				else
					DrawLoader(e.Graphics, new Rectangle(Padding.Left, (int)((Height - iconSize) / 2F), iconSize, iconSize), fore);
			}
			else if ((DesignMode ? Image.SafeColor(fore) : Image.Color(fore)) != null)
			{
				if (HideText || string.IsNullOrWhiteSpace(Text))
					e.Graphics.DrawImage(Image, new Rectangle((Width - iconSize) / 2, (int)((Height - iconSize) / 2F), iconSize, iconSize));
				else
					e.Graphics.DrawImage(Image, new Rectangle(Padding.Left, (int)((Height - iconSize) / 2F), iconSize, iconSize));
			}

			if (!HideText && !string.IsNullOrWhiteSpace(Text))
			{
				var stl = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
				if (Image != null)
					e.Graphics.DrawString(Text, Font, Gradient(fore), new Rectangle(iconSize + 2 * Padding.Left, 0, Width - (iconSize + Padding.Left + Padding.Horizontal), Height), stl);
				else
					e.Graphics.DrawString(Text, Font, Gradient(fore), new Rectangle(Padding.Left, 0, Width - (Padding.Left), Height), stl);
			}
		}
	}
}
