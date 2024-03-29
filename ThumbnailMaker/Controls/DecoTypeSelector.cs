﻿using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Controls
{
	public class DecoTypeSelector : Form
	{
		private readonly RoadLane _roadLane;
		private readonly List<LaneDecoration> values;

		public DecoTypeSelector(RoadLane roadLane)
		{
			_roadLane = roadLane;

			var point = new Point(12, 12);

			values = Enum.GetValues(typeof(LaneDecoration)).Cast<LaneDecoration>().OrderBy(GetOrder).ToList();

			Width = ((roadLane.Lane.Type == LaneType.Pedestrian ? 5 : 7) * 108) + 12;

			foreach (var laneType in values)
			{
				if (!Utilities.IsCompatible(laneType, roadLane.Lane.Type))
				{
					continue;
				}

				if (point.X + 108 > Width)
				{
					point = new Point(12, point.Y + 108);
				}

				point.X += 108;
			}
			if (point.Y == 12)
			{
				Width = point.X;
			}

			Height = point.Y + 108;
			ShowIcon = false;
			ShowInTaskbar = false;
			DoubleBuffered = true;
			ResizeRedraw = true;
			FormBorderStyle = FormBorderStyle.None;
			StartPosition = FormStartPosition.Manual;

			Show(roadLane.FindForm());
		}

		public static int GetOrder(LaneDecoration decorations)
		{
			var name = Enum.GetName(typeof(LaneDecoration), decorations);

			if (name == null)
			{
				return int.MaxValue;
			}

			var field = decorations.GetType().GetField(name);

			if (!(Attribute.GetCustomAttribute(field, typeof(StyleIdentityAttribute)) is StyleIdentityAttribute attribute))
			{
				return int.MaxValue;
			}

			return attribute.Order;
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			Location = new Point(_roadLane.PointToScreen(Point.Empty).X, _roadLane.PointToScreen(Point.Empty).Y + _roadLane.Height);

			if (Location.Y + Height > Screen.FromControl(this).WorkingArea.Height)
			{
				Top = Math.Max(0, Top - (_roadLane.Height + Height));
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

			foreach (var laneType in values)
			{
				if (!Utilities.IsCompatible(laneType, _roadLane.Lane.Type))
				{
					continue;
				}

				var rectangle = new Rectangle(point, new Size(96, 96));

				using (var icon = ResourceManager.GetImage(laneType, false))
				{
					var laneColor = ThumbnailLaneInfo.GetColor(laneType);

					e.Graphics.FillRoundedRectangle(new SolidBrush(_roadLane.Lane.Decorations.HasFlag(laneType) ? Color.FromArgb(200, laneColor) : FormDesign.Design.AccentColor), rectangle, 16);

					if (laneType == LaneDecoration.None ? (_roadLane.Lane.Decorations == LaneDecoration.None) : _roadLane.Lane.Decorations.HasFlag(laneType))
					{
						e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.ActiveColor, 2.5F), rectangle, 16);
					}

					if (icon != null)
					{
						e.Graphics.DrawIcon(laneType == LaneDecoration.None ? icon.Color(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.AccentColor)) : icon
							, rectangle, new Size(80, 80));
					}
					else if (!_roadLane.Lane.Decorations.HasFlag(laneType))
					{
						e.Graphics.DrawRoundedRectangle(new Pen(laneColor, 2.5F), rectangle, 16);
					}

					if (rectangle.Contains(cursor))
					{
						e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(150, laneColor)), rectangle, 16);
						e.Graphics.DrawString(laneType.ToString().FormatWords(), new Font(UI.FontFamily, 11.25F, FontStyle.Bold), new SolidBrush(laneColor.GetAccentColor()), rectangle, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
					}
					else if (!_roadLane.Lane.Decorations.HasFlag(laneType))
					{
						e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(100, FormDesign.Design.AccentColor)), rectangle, 16);
					}
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

			foreach (var laneType in values)
			{
				if (!Utilities.IsCompatible(laneType, _roadLane.Lane.Type))
				{
					continue;
				}

				if (new Rectangle(point, new Size(96, 96)).Contains(e.Location))
				{
					if (laneType == LaneDecoration.None)
					{
						_roadLane.Lane.Decorations = laneType;
					}
					else
					{
						if (laneType <= LaneDecoration.Gravel && !_roadLane.Lane.Decorations.HasFlag(laneType))
						{
							_roadLane.Lane.Decorations &= ~LaneDecoration.Filler & ~LaneDecoration.Grass & ~LaneDecoration.Gravel & ~LaneDecoration.Pavement;
						}

						_roadLane.Lane.Decorations = _roadLane.Lane.Decorations.HasFlag(laneType) ? _roadLane.Lane.Decorations & ~laneType : _roadLane.Lane.Decorations | laneType;
					}
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

			foreach (var laneType in values)
			{
				if (!Utilities.IsCompatible(laneType, _roadLane.Lane.Type))
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
	}
}
