using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Controls
{
	public class RoadTypeSelector : Form
	{
		private readonly RoadLane _roadLane;
		private readonly Control _control;
		private readonly LaneClass _previousLaneType;

		public RoadTypeSelector(RoadLane roadLane, Control control = null)
		{
			_roadLane = roadLane;
			_control = control ?? roadLane;
			_previousLaneType = roadLane.LaneType;

			var point = new Point(12, 12);

			foreach (LaneClass laneType in Enum.GetValues(typeof(LaneClass)))
			{
				if ((laneType & (LaneClass.Curb | LaneClass.Train)) != 0)
					continue;

				if (point.X + 96 > 5 * 108 + 12)
					point = new Point(12, point.Y + 108);

				point.X += 108;
			}

			Size = new Size(5 * 108 + 12, point.Y + 108);
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
				Top -= _control.Height + Height;
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

			var cursor = PointToClient(Cursor.Position);
			var point = new Point(12, 12);

			foreach (LaneClass laneType in Enum.GetValues(typeof(LaneClass)))
			{
				if ((laneType & (LaneClass.Curb | LaneClass.Train)) != 0)
					continue;

				var rectangle = new Rectangle(point, new Size(96, 96));

				e.Graphics.FillRoundedRectangle(new SolidBrush(_roadLane.LaneType.HasFlag(laneType) && laneType != LaneClass.Empty ? Color.FromArgb(175, LaneInfo.GetColor(laneType)) : FormDesign.Design.AccentColor), rectangle, 16);

				if (laneType == LaneClass.Empty)
					e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 2.5F), rectangle, 16);

				using (var icon = ResourceManager.GetImage(laneType, false))
				{
					if (icon != null)
						e.Graphics.DrawIcon(icon, rectangle, new Size(80, 80));
					else if (!_roadLane.LaneType.HasFlag(laneType))
						e.Graphics.DrawRoundedRectangle(new Pen(LaneInfo.GetColor(laneType), 2.5F), rectangle, 16);
				}

				if (rectangle.Contains(cursor))
				{
					e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(150, LaneInfo.GetColor(laneType))), rectangle, 16);
					e.Graphics.DrawString(laneType.ToString(), new Font(UI.FontFamily, 11.25F, FontStyle.Bold), new SolidBrush(LaneInfo.GetColor(laneType).GetAccentColor()), rectangle, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
				}
				else if (!_roadLane.LaneType.HasFlag(laneType))
					e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(100, FormDesign.Design.AccentColor)), rectangle, 16);

				point.X += 108;

				if (point.X + 96 > ClientRectangle.Width)
					point = new Point(12, point.Y + 108);
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.Button == MouseButtons.Right)
				Close();

			if (e.Button != MouseButtons.Left)
				return;

			var point = new Point(8, 8);

			foreach (LaneClass laneType in Enum.GetValues(typeof(LaneClass)))
			{
				if ((laneType & (LaneClass.Curb | LaneClass.Train)) != 0)
					continue;

				if (new Rectangle(point, new Size(96, 96)).Contains(e.Location))
				{
					if (laneType == LaneClass.Empty || laneType == LaneClass.Filler || laneType == LaneClass.Parking)
						_roadLane.SetLaneType(laneType);
					else
						_roadLane.SetLaneType(_roadLane.LaneType.HasFlag(laneType) ? _roadLane.LaneType & ~laneType : _roadLane.LaneType | laneType);
				
					Invalidate();
					return;
				}

				point.X += 108;

				if (point.X + 96 > ClientRectangle.Width)
					point = new Point(8, point.Y + 108);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			Invalidate();

			var point = new Point(8, 8);

			foreach (LaneClass laneType in Enum.GetValues(typeof(LaneClass)))
			{
				if ((laneType & (LaneClass.Curb | LaneClass.Train)) != 0)
					continue;

				if (new Rectangle(point, new Size(96, 96)).Contains(e.Location))
				{
					Cursor = Cursors.Hand;
					return;
				}

				point.X += 108;

				if (point.X + 96 > ClientRectangle.Width)
					point = new Point(8, point.Y + 108);
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

			_roadLane.ApplyLaneType(_previousLaneType);
		}
	}
}
