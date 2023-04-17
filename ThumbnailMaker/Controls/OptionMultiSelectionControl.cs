using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Controls
{
	public class OptionMultiSelectionControl<T> : SlickMultiSelectionDropDown<T> where T : Enum
	{
        public bool Small { get; set; }

        public OptionMultiSelectionControl()
		{
			Items = typeof(T).GetEnumValues().Cast<T>().ToArray();
		}

		protected override void UIChanged()
		{
			if (Small)
			{
				Font = UI.Font(8.25F, FontStyle.Bold);
				Margin = new Padding(3);
				Padding = UI.Scale(new Padding(3), UI.FontScale);
				Size = UI.Scale(new Size(140, 30), UI.UIScale);
				return;
			}

			Font = UI.Font(8.25F, FontStyle.Bold);
			Margin = new Padding(3, 5, 5, 3);
			Padding = UI.Scale(new Padding(3), UI.FontScale);
			Size = UI.Scale(new Size(150, 37), UI.UIScale);
		}

		protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, T item, bool selected)
		{
			e.Graphics.DrawImage((!selected ? Properties.Resources.I_Checked : Properties.Resources.I_Noo).Color(!selected ? FormDesign.Design.ActiveColor : foreColor), rectangle.Pad(Padding).Align(new Size(16, 16), ContentAlignment.MiddleLeft));

			var textRect = new Rectangle(rectangle.X + 16 + Padding.Horizontal, rectangle.Y + (rectangle.Height - Font.Height) / 2, 0, Font.Height);

			textRect.Width = rectangle.Width - textRect.X;

			var text = item.ToString().FormatWords();
			e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), textRect, new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
		}

		protected override void PaintSelectedItems(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IEnumerable<T> items)
		{
			string text;

			switch (items.Count())
			{
				case 3:
					text = "Ground Only";
					break;
				case 0:
					text = "All Elevations";
					break;
				default:
					text = "No " + items.ListStrings(", ");
					break;
			}

			e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), rectangle, new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
		}
	}
}