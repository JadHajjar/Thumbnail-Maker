using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Controls
{
	public class LaneSpeedSelector : Form
	{
		private readonly RoadLane _roadLane;
		private readonly LaneDecoration _previousLaneType;

		public LaneSpeedSelector(RoadLane roadLane)
		{
			_roadLane = roadLane;
			_previousLaneType = roadLane.Decorations;

			var point = new Point(12, 12);

			foreach (var item in GetSpeedValues())
			{
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

		private IEnumerable<int> GetSpeedValues()
		{
			yield return -1;

			if (Options.Current.Region == RegionType.USA)
			{
				for (var i = 10; i <= 100; i += 5)
				{
					yield return i;
				}

				yield break;
			}

			for (var i = 10; i <= 140; i += 10)
			{
				yield return i;
			}
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			Location = new Point(_roadLane.PointToScreen(Point.Empty).X+ _roadLane.Width-Width, _roadLane.PointToScreen(Point.Empty).Y + _roadLane.Height);

			if (Location.Y + Height > Screen.FromControl(this).WorkingArea.Height)
				Top -= _roadLane.Height + Height;
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

			foreach (var speed in GetSpeedValues())
			{
				var rectangle = new Rectangle(point, new Size(96, 96));

				var laneColor = FormDesign.Design.AccentBackColor;

				e.Graphics.FillRoundedRectangle(new SolidBrush(_roadLane.SpeedLimit == speed ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), rectangle, 16);

				//if (laneType == LaneDecorationStyle.None)
				//	e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 2.5F), rectangle, 16);

				ThumbnailHandler.DrawSpeedSignLarge(e.Graphics, Options.Current.Region, speed, rectangle);
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

				if (!rectangle.Contains(cursor) && _roadLane.SpeedLimit != speed)
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

			foreach (var speed in GetSpeedValues())
			{
				if (new Rectangle(point, new Size(96, 96)).Contains(e.Location))
				{
					_roadLane.SpeedLimit = speed;

					_roadLane.RefreshRoad();

					Close();
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

			foreach (var speed in GetSpeedValues())
			{
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

			//_roadLane.ApplyLaneType(_previousLaneType);
		}
	}
}
