using Extensions;

using SlickControls;

using System;
using System.Drawing;
using System.Windows.Forms;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Controls
{
	public class RoadTypeSelector : Form
	{
		private readonly RoadLane _roadLane;
		private readonly Control _control;

		public RoadTypeSelector(RoadLane roadLane, Control control = null)
		{
			_roadLane = roadLane;
			_control = control ?? roadLane;

			var point = new Point(12, 12);

			foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
			{
				if ((laneType & (LaneType.Curb | LaneType.Train)) != 0)
				{
					continue;
				}

				if (point.X + 96 > (5 * 108) + 12)
				{
					point = new Point(12, point.Y + 108);
				}

				point.X += 108;
			}

			Size = new Size((5 * 108) + 12, point.Y + 108);
			ShowIcon = false;
			ShowInTaskbar = false;
			DoubleBuffered = true;
			ResizeRedraw = true;
			FormBorderStyle = FormBorderStyle.None;
			StartPosition = FormStartPosition.Manual;

			Show(roadLane.FindForm());
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			Location = new Point(_control.PointToScreen(Point.Empty).X - (_control is RoadLane ? 0 : Width - _control.Width), _control.PointToScreen(Point.Empty).Y + _control.Height + _control.Margin.Bottom);

			if (Location.Y + Height > Screen.FromControl(this).WorkingArea.Height)
			{
				Top -= _control.Height + Height;
			}
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
			e.Graphics.Clear(FormDesign.Design.AccentBackColor);
			e.Graphics.DrawRectangle(new Pen(FormDesign.Design.ActiveColor), ClientRectangle.Pad(0, 0, 1, 1));
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			var cursor = PointToClient(Cursor.Position);
			var point = new Point(12, 12);

			foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
			{
				if ((laneType & (LaneType.Curb | LaneType.Train)) != 0)
				{
					continue;
				}

				var rectangle = new Rectangle(point, new Size(96, 96));

				e.Graphics.FillRoundedRectangle(new SolidBrush(_roadLane.Lane.Type.HasFlag(laneType) ? Color.FromArgb(175, ThumbnailLaneInfo.GetColor(laneType)) : FormDesign.Design.AccentColor), rectangle, 16);

				if (laneType == LaneType.Empty ? (_roadLane.Lane.Type == LaneType.Empty) : _roadLane.Lane.Type.HasFlag(laneType))
				{
					e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.ActiveColor, 2.5F), rectangle, 16);
				}

				using (var icon = ResourceManager.GetImage(laneType, false))
				{
					if (icon != null)
					{
						e.Graphics.DrawIcon(laneType == LaneType.Empty ? icon.Color(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.AccentColor)) : icon, rectangle, new Size(80, 80));
					}
					else if (!_roadLane.Lane.Type.HasFlag(laneType))
					{
						e.Graphics.DrawRoundedRectangle(new Pen(ThumbnailLaneInfo.GetColor(laneType), 2.5F), rectangle, 16);
					}
				}

				if (rectangle.Contains(cursor))
				{
					e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(150, ThumbnailLaneInfo.GetColor(laneType))), rectangle, 16);
					e.Graphics.DrawString(laneType.ToString(), new Font(UI.FontFamily, 11.25F, FontStyle.Bold), new SolidBrush(ThumbnailLaneInfo.GetColor(laneType).GetAccentColor()), rectangle, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
				}
				else if (!_roadLane.Lane.Type.HasFlag(laneType))
				{
					e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(100, FormDesign.Design.AccentColor)), rectangle, 16);
				}

				point.X += 108;

				if (point.X + 96 > ClientRectangle.Width)
				{
					point = new Point(12, point.Y + 108);
				}
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

			var point = new Point(8, 8);

			foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
			{
				if ((laneType & (LaneType.Curb | LaneType.Train)) != 0)
				{
					continue;
				}

				if (new Rectangle(point, new Size(96, 96)).Contains(e.Location))
				{
					if (!_roadLane.Lane.Type.HasFlag(laneType) && (laneType == LaneType.Empty || laneType == LaneType.Filler || laneType == LaneType.Parking))
					{
						_roadLane.Lane.Type = laneType;
					}
					else
					{
						_roadLane.Lane.Type = _roadLane.Lane.Type.HasFlag(laneType) ? _roadLane.Lane.Type & ~laneType : _roadLane.Lane.Type | laneType;
					}

					if (!_roadLane.Lane.Type.HasAnyFlag(LaneType.Bike, LaneType.Car, LaneType.Bus, LaneType.Tram, LaneType.Trolley, LaneType.Emergency))
						_roadLane.Lane.SpeedLimit = null;

					Invalidate();
					_roadLane.RefreshRoad();
					return;
				}

				point.X += 108;

				if (point.X + 96 > ClientRectangle.Width)
				{
					point = new Point(8, point.Y + 108);
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			Invalidate();

			var point = new Point(8, 8);

			foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
			{
				if ((laneType & (LaneType.Curb | LaneType.Train)) != 0)
				{
					continue;
				}

				if (new Rectangle(point, new Size(96, 96)).Contains(e.Location))
				{
					Cursor = Cursors.Hand;
					return;
				}

				point.X += 108;

				if (point.X + 96 > ClientRectangle.Width)
				{
					point = new Point(8, point.Y + 108);
				}
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

			_roadLane.ApplyLaneType();
		}
	}
}
