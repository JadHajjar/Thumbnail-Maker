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

namespace ThumbnailMaker.Controls
{
	public class RoadTypeSelector : Form
	{
		private readonly RoadLane _roadLane;

		public RoadTypeSelector(RoadLane roadLane)
		{
			_roadLane = roadLane;

			var point = new Point(12, 12);

			foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
			{
				if (laneType == LaneType.Highway)
					continue;

				point.X += 108;

				if (point.X + 96 > 5 * 108 + 12)
					point = new Point(12, point.Y + 108);
			}

			Size = new Size(5 * 108 + 12, point.Y + 108);
			ShowIcon = false;
			ShowInTaskbar = false;
			DoubleBuffered = true;
			ResizeRedraw = true;
			FormBorderStyle = FormBorderStyle.None;
			StartPosition = FormStartPosition.Manual;
			Location = roadLane.FindForm().Bounds.Center(Size);
			Show(roadLane.FindForm());
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

			var point = new Point(12, 12);

			foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
			{
				if (laneType == LaneType.Highway)
					continue;

				e.Graphics.FillRoundedRectangle(new SolidBrush(_roadLane.LaneType.HasFlag(laneType) ? LaneInfo.GetColor(laneType) : FormDesign.Design.AccentColor), new Rectangle(point, new Size(96, 96)), 16);

				if (laneType == LaneType.Empty)
					e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 2.5F), new Rectangle(point, new Size(96, 96)), 16);
				
				using (var icon = ResourceManager.GetImage(laneType, false))
					if (icon != null)
						e.Graphics.DrawImage(new Bitmap(icon, new Size(icon.Width * 84 / icon.Height, 84)).Color(_roadLane.LaneType.HasFlag(laneType) ? LaneInfo.GetColor(laneType).GetAccentColor(): LaneInfo.GetColor(laneType)), new Rectangle(point, new Size(96, 96)).CenterR(new Size(icon.Width * 84 / icon.Height, 84)));
				else if (!_roadLane.LaneType.HasFlag(laneType))
						e.Graphics.DrawRoundedRectangle(new Pen(LaneInfo.GetColor(laneType), 2.5F), new Rectangle(point, new Size(96, 96)), 16);

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

			foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
			{
				if (laneType == LaneType.Highway)
					continue;

				if (new Rectangle(point, new Size(96, 96)).Contains(e.Location))
				{
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

			var point = new Point(8, 8);

			foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
			{
				if (laneType == LaneType.Highway)
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
	}
}
