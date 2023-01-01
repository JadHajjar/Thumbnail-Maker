using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Security.AccessControl;
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

		public LaneSpeedSelector(RoadLane roadLane)
		{
			_roadLane = roadLane;

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
				Top = Math.Max(0, Top - (_roadLane.Height + Height));
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
			e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

			var cursor = PointToClient(Cursor.Position);
			var point = new Point(12, 12);

			var rectangle = new Rectangle(point, new Size(96, 96));
			var laneColor = ThumbnailLaneInfo.GetColor(LaneDecoration.None);

			e.Graphics.FillRoundedRectangle(new SolidBrush(_roadLane.Lane.SpeedLimit == null ? laneColor : FormDesign.Design.AccentColor), rectangle, 16);

			if (_roadLane.Lane.SpeedLimit == null)
				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.ActiveColor, 2.5F), rectangle, 16);

			using(var icon = ResourceManager.GetImage(LaneDecoration.None, false))
			if (icon != null)
			{
				e.Graphics.DrawIcon(_roadLane.Lane.SpeedLimit == null ? icon.Color(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.AccentColor)) : icon
					, rectangle, new Size(80, 80));
			}

			if (rectangle.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(150, laneColor)), rectangle, 16);
				e.Graphics.DrawString("Default Speed Limit", new Font(UI.FontFamily, 11.25F, FontStyle.Bold), new SolidBrush(laneColor.GetAccentColor()), rectangle, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			}
			else if (_roadLane.Lane.SpeedLimit != null)
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(100, FormDesign.Design.AccentColor)), rectangle, 16);
			
			point.X += 108;

			foreach (var speed in GetSpeedValues().Skip(1))
			{
				rectangle = new Rectangle(point, new Size(96, 96));

				laneColor = FormDesign.Design.AccentBackColor;

				e.Graphics.FillRoundedRectangle(new SolidBrush((_roadLane.Lane.SpeedLimit ?? -1) == speed ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), rectangle, 16);

				ThumbnailHandler.DrawSpeedSignLarge(e.Graphics, Options.Current.Region, speed, rectangle);
				e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

				if (!rectangle.Contains(cursor) && (_roadLane.Lane.SpeedLimit ?? -1) != speed)
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
					_roadLane.Lane.SpeedLimit = speed == -1 ? (float?)null : speed;

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
	}
}
