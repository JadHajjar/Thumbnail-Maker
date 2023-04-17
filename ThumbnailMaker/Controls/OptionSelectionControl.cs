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
	public class OptionSelectionControl<T> : SlickSelectionDropDown<T> where T : Enum
	{
		private readonly Action<Graphics, Rectangle, T> _customDraw;
        public bool Small { get; set; }

        public OptionSelectionControl(Action<Graphics, Rectangle, T> customDraw)
		{
			_customDraw = customDraw;
			Items = typeof(T).GetEnumValues().Cast<T>().ToArray();
		}

		public OptionSelectionControl(Func<T, Image> image) : this((g, r, t) =>
		{
			g.DrawIcon(image(t), r, r.Size);
		})
		{ }

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

		protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, T item)
		{
			var iconSize = UI.Scale(new Size(18, 18), UI.UIScale);

			if (rectangle.Width > 33 * UI.FontScale * 3)
			{
				_customDraw(e.Graphics, rectangle.Pad(Padding).Align(iconSize, ContentAlignment.MiddleLeft), item);

				var textRect = new Rectangle(rectangle.X + iconSize.Width + Padding.Horizontal, rectangle.Y + (rectangle.Height - Font.Height) / 2, 0, Font.Height);

				textRect.Width = rectangle.Width - textRect.X;

				var text = item.ToString().FormatWords();
				e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), textRect, new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
			}
			else
			{
				_customDraw(e.Graphics, rectangle.CenterR(iconSize), item);
			}
		}
	}
}