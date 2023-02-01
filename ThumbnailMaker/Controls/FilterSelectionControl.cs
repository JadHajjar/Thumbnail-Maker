using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ThumbnailMaker.Controls
{
	public class FilterSelectionControl<T> : SlickControl where T : Enum
	{
		private T _selectedValue;

		public Type EnumType { get; private set; }
		public Image Icon { get; set; }
		public T SelectedValue
		{
			get => _selectedValue;
			set
			{
				_selectedValue = value;

				Invalidate();

				SelectedValueChanged?.Invoke(this, value);
			}
		}

		public event System.EventHandler<T> SelectedValueChanged;

		public FilterSelectionControl()
		{
			Cursor = Cursors.Hand;
			EnumType = typeof(T);
			TabStop = false;
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			Height = (int)(24 * UI.FontScale);

			using (var g = CreateGraphics())
			{
				Width = Height + 3 + (int)EnumType.GetEnumNames().Max(x => g.MeasureString(x.FormatWords(), UI.Font(8.25F, FontStyle.Bold)).Width);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (EnumType == null)
			{
				return;
			}

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			var rect = ClientRectangle;
			var hoverState = HoverState;

			var back = hoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : hoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ButtonColor : FormDesign.Design.AccentBackColor;
			var fore = hoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : hoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ButtonForeColor : FormDesign.Design.ForeColor;

			e.Graphics.FillRoundedRectangle(new SolidBrush(back), rect.Pad(1), 4);

			var iconRect = new Rectangle(0, 0, rect.Height, rect.Height).CenterR(16, 16);

			e.Graphics.DrawImage(Icon.Color(fore), iconRect);

			e.Graphics.DrawString(_selectedValue.ToString().FormatWords(), UI.Font(8.25F, FontStyle.Bold), new SolidBrush(fore), rect.Pad(rect.Height, 0, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center });
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.Button == MouseButtons.Left)
			{
				new FilterSelectionForm<T>(this);
			}
		}
	}

	public class FilterSelectionForm<T> : Form where T : Enum
	{
		private readonly FilterSelectionControl<T> _control;

		public FilterSelectionForm(FilterSelectionControl<T> control)
		{
			_control = control;

			ShowIcon = false;
			ShowInTaskbar = false;
			DoubleBuffered = true;
			ResizeRedraw = true;
			FormBorderStyle = FormBorderStyle.None;
			StartPosition = FormStartPosition.Manual;
			MinimumSize = Size.Empty;

			if (_control.Parent is GroupBox)
			{
				Location = new Point(_control.Parent.PointToScreen(Point.Empty).X, _control.PointToScreen(Point.Empty).Y);
				Size = new Size(_control.Parent.Width, GetValues().Count() * (_control.Height + 6));
			}
			else
			{
				Location = new Point(_control.PointToScreen(Point.Empty).X - _control.Margin.Left, _control.PointToScreen(Point.Empty).Y - 3);
				Size = new Size(_control.Width + _control.Margin.Horizontal, GetValues().Count() * (_control.Height + 6) + 3);
			}

			if (_control.FindForm() is SlickForm form)
			{
				form.CurrentFormState = FormState.ForcedFocused;
			}

			Show(_control.FindForm());
		}

		private IEnumerable<T> GetValues()
		{
			return typeof(T).GetEnumValues().Cast<T>();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape || keyData == Keys.Enter)
			{
				Close();
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(_control.Parent.BackColor);
			e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor), new Rectangle(0, 0, Width - 1, Height - 1), 4);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			var cursor = PointToClient(Cursor.Position);
			var y = 3;

			foreach (var item in GetValues())
			{
				var rect = new Rectangle(3, y, _control.Width, _control.Height);
				var hoverState = _control.SelectedValue.Equals(item) ? HoverState.Pressed : rect.Contains(cursor) ? HoverState.Hovered : HoverState.Normal;
				var back = hoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : hoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.AccentBackColor : Color.Empty;
				var fore = hoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ForeColor;

				e.Graphics.FillRoundedRectangle(new SolidBrush(back), rect.Pad(1), 4);

				var iconRect = new Rectangle(3, y, rect.Height, rect.Height).CenterR(16, 16);

				if (hoverState == HoverState.Pressed)
				{
					e.Graphics.DrawImage(Properties.Resources.I_Checked.Color(fore), iconRect);
				}
				else
				{
					e.Graphics.DrawEllipse(new Pen(fore, 1.5F), iconRect.Pad(2));
				}

				e.Graphics.DrawString(item.ToString().FormatWords(), UI.Font(8.25F, FontStyle.Bold), new SolidBrush(fore), rect.Pad(rect.Height, 0, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center });

				y += rect.Height + 6;
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.Button == MouseButtons.Right)
			{
				Close();
			}

			if (e.Button != MouseButtons.Left)
			{
				return;
			}

			var y = 0;

			foreach (var item in GetValues())
			{
				var rect = new Rectangle(3, y, _control.Width, _control.Height);

				if (rect.Contains(e.Location))
				{
					_control.SelectedValue = item;

					Close();
					return;
				}

				y += rect.Height + 6;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			Invalidate();

			var y = 0;

			foreach (var item in GetValues())
			{
				var rect = new Rectangle(3, y, _control.Width, _control.Height);

				if (rect.Contains(e.Location))
				{
					Cursor = Cursors.Hand;
					return;
				}

				y += rect.Height + 6;
			}

			Cursor = Cursors.Default;
		}

		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);

			Close();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			if (_control.FindForm() is SlickForm form)
			{
				form.CurrentFormState = FormState.NormalFocused;
			}
		}
	}
}
