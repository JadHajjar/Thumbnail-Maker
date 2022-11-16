using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using ThumbnailMaker.Domain;

namespace ThumbnailMaker.Controls
{
	public partial class RoadLane : SlickControl
	{
		private bool _dragDropActive;

		public event EventHandler RoadLaneChanged;

		public RoadLane(Func<bool> highwayCheck)
		{
			IsHighWay = highwayCheck;
			Height = 42;
			AllowDrop = true;
		}

		public LaneType LaneType { get => _laneType == LaneType.Car && IsHighWay() ? LaneType.Highway : _laneType; private set => _laneType = value; }
		public LaneDirection LaneDirection { get; private set; }
		public int Lanes { get; private set; }
		public Func<bool> IsHighWay { get; }

		public void SetLaneType(LaneType laneType)
		{
			LaneType = laneType;

			sizeRects.Clear();
			directionRects.Clear();

			if ((LaneType < LaneType.Car || LaneType == LaneType.Parking) && LaneDirection != LaneDirection.None)
				LaneDirection = LaneDirection.None;

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);

			Invalidate();
		}

		private Rectangle iconRectangle;
		private Rectangle deleteRectangle;
		private LaneType _laneType;
		private readonly Dictionary<int, Rectangle> sizeRects = new Dictionary<int, Rectangle>();
		private readonly Dictionary<LaneDirection, Rectangle> directionRects = new Dictionary<LaneDirection, Rectangle>();

		protected override void OnPaint(PaintEventArgs e)
		{
			var cursor = PointToClient(Cursor.Position);
			var lane = new LaneInfo
			{
				Type = LaneType,
				Direction = LaneDirection,
				Lanes = Lanes,
			};

			e.Graphics.Clear(BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(0, 0, 1, 7), 6);

			if (_dragDropActive)
				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.ActiveColor, 2F), ClientRectangle.Pad(0, 0, 1, 7), 6);

			e.Graphics.FillEllipse(new SolidBrush(lane.Color), new Rectangle(6, (Height - 24 - 7) / 2, 24, 24));

			if (LaneType == LaneType.Empty)
				e.Graphics.DrawEllipse(new Pen(FormDesign.Design.AccentColor, 1.5F), new Rectangle(6, (Height - 24 - 7) / 2, 24, 24));

			var iconX = 36;
			foreach (var icon in lane.Icons(true).Where(x => x != null))
			{
				using (icon)
					e.Graphics.DrawImage(icon.Color(FormDesign.Design.IconColor), new Rectangle(new Point(iconX, (Height - icon.Height - 7) / 2), icon.Size));

				iconX += 20;
			}

			iconRectangle = new Rectangle(6, (Height - 24 - 7) / 2, iconX - 4, 24);

			if (iconRectangle.Contains(cursor))
				DrawFocus(e.Graphics, iconRectangle.Pad(-4, -4, lane.Icons(true) == null ? 23 : -4, -4), HoverState.Focused, 4);

			deleteRectangle = new Rectangle(Width - 32, (Height - 32 - 7) / 2, 32, 32);

			e.Graphics.DrawImage(Properties.Resources.I_X.Color(deleteRectangle.Contains(cursor) ? FormDesign.Design.RedColor : FormDesign.Design.IconColor), deleteRectangle.CenterR(16, 16));

			e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), Width - 34, 6, Width - 34, Height - 13);

			e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), iconX + 8, 6, iconX + 8, Height - 13);

			if (LaneType == LaneType.Parking)
			{
				for (var i = 3; i >= 0; i--)
				{
					var direction = (LaneDirection)i;

					if (direction == LaneDirection.Both)
						continue;

					directionRects[direction] = new Rectangle(Width - 6 - 32 - (32 * (5 - Math.Max(0, i - 1) + 2) - 16), (Height - 32 - 7) / 2, 32, 32);

					if (directionRects[direction].Contains(cursor))
						e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), directionRects[direction], 4);

					Bitmap icon;
					switch (direction)
					{
						case LaneDirection.Both:
							icon = Properties.Resources.I_2W;
							break;
						case LaneDirection.Forward:
							icon = Properties.Resources.I_1WF;
							break;
						case LaneDirection.Backwards:
							icon = Properties.Resources.I_1WB;
							break;
						default:
							icon = Properties.Resources.I_Unavailable;
							break;
					}

					e.Graphics.DrawImage(icon.Color(directionRects[direction].Contains(cursor) ? FormDesign.Design.ActiveForeColor : LaneDirection == direction ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), directionRects[direction].CenterR(16, 16));
				}

				var icon_ = Properties.Resources.I_Vertical;
				var diagonalRect = new Rectangle(Width - 24 - 32 - (32 * 3) + 10, (Height - 32 - 7) / 2, 32, 32);

				if (diagonalRect.Contains(cursor))
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), diagonalRect, 4);

				e.Graphics.DrawImage(icon_.Color(diagonalRect.Contains(cursor) ? FormDesign.Design.ActiveForeColor : Lanes <= 1 ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), diagonalRect.CenterR(16, 16));
				sizeRects[1] = diagonalRect;

				icon_ = Properties.Resources.Icon_Horizontal;
				diagonalRect.X += 32;

				if (diagonalRect.Contains(cursor))
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), diagonalRect, 4);

				e.Graphics.DrawImage(icon_.Color(diagonalRect.Contains(cursor) ? FormDesign.Design.ActiveForeColor : Lanes == 2 ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), diagonalRect.CenterR(16, 16));
				sizeRects[2] = diagonalRect;

				icon_ = Properties.Resources.I_Diagonal;
				diagonalRect.X += 32;

				if (diagonalRect.Contains(cursor))
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), diagonalRect, 4);

				e.Graphics.DrawImage(icon_.Color(diagonalRect.Contains(cursor) ? FormDesign.Design.ActiveForeColor : Lanes > 2 ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), diagonalRect.CenterR(16, 16));
				sizeRects[3] = diagonalRect;

				e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), Width - 24 - 32 - (32 * 3) + 6, 6, Width - 24 - 32 - (32 * 3) + 6, Height - 13);
				e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), Width - 25 - 32 - (32 * 6), 6, Width - 25 - 32 - (32 * 6), Height - 13);

				e.Graphics.DrawImage(Properties.Resources.I_Grabber.Color(FormDesign.Design.AccentColor), new Rectangle(iconX + 8, 0, (Width - 6 - 32 - (32 * (6) - 16)) - iconX - 8, Height - 4).CenterR(10, 5));

				return;
			}

			if (lane.IsFiller)
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), new Rectangle(Width - 6 - 32 - (32 * 10), (Height - 32 - 7) / 2, 32 * 10, 32), 4);

				for (var i = 9; i >= 0; i--)
				{
					sizeRects[i] = new Rectangle(Width - 6 - 32 - (32 * (9 - i + 1)), (Height - 32 - 7) / 2, 32, 32);

					if (sizeRects[i].Contains(cursor))
						e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), sizeRects[i], 4);

					e.Graphics.DrawString($"{(10 - Math.Min(i, 9)) * 10}%", new Font(UI.FontFamily, 8.25F), new SolidBrush(sizeRects[i].Contains(cursor) ? FormDesign.Design.ActiveForeColor : i == Lanes ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), sizeRects[i], new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
				}

				e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), Width - 6 - 8 - 32 - (32 * 10), 6, Width - 6 - 8 - 32 - (32 * 10), Height - 13);

				e.Graphics.DrawImage(Properties.Resources.I_Grabber.Color(FormDesign.Design.AccentColor), new Rectangle(iconX + 8, 0, Width - 6 - 8 - 32 - (32 * 10) - iconX - 8, Height - 4).CenterR(10, 5));

				return;
			}

			e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), new Rectangle(Width - 6 - 32 - (32 * 8), (Height - 32 - 7) / 2, 32 * 8, 32), 4);

			for (var i = 7; i >= 0; i--)
			{
				sizeRects[i] = new Rectangle(Width - 6 - 32 - (32 * (7 - i + 1)), (Height - 32 - 7) / 2, 32, 32);

				if (sizeRects[i].Contains(cursor))
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), sizeRects[i], 4);

				if (i == 0)
					e.Graphics.DrawImage(Properties.Resources.I_Unavailable.Color(sizeRects[i].Contains(cursor) ? FormDesign.Design.ActiveForeColor : i == Lanes ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), sizeRects[i].CenterR(16, 16));
				else
					e.Graphics.DrawString($"{i}L", new Font(UI.FontFamily, 8.25F), new SolidBrush(sizeRects[i].Contains(cursor) ? FormDesign.Design.ActiveForeColor : i == Lanes ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), sizeRects[i], new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			}

			e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), Width - 6 - 8 - 32 - (32 * 8), 6, Width - 6 - 8 - 32 - (32 * 8), Height - 13);
			e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), Width - 6 - 8 - 16 - 32 - (32 * 12), 6, Width - 6 - 16 - 8 - 32 - (32 * 12), Height - 13);

			e.Graphics.DrawImage(Properties.Resources.I_Grabber.Color(FormDesign.Design.AccentColor), new Rectangle(iconX + 8, 0, Width - 6 - 8 - 16 - 32 - (32 * 12) - iconX - 8, Height - 4).CenterR(10, 5));

			e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), new Rectangle(Width - 6 - 32 - (32 * 12) - 16, (Height - 32 - 7) / 2, 32 * 4, 32), 4);

			for (var i = 3; i >= 0; i--)
			{
				var direction = (LaneDirection)i;
				directionRects[direction] = new Rectangle(Width - 6 - 32 - (32 * (12 - i + 1) - 16), (Height - 32 - 7) / 2, 32, 32);

				if (directionRects[direction].Contains(cursor))
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), directionRects[direction], 4);

				Bitmap icon;
				switch (direction)
				{
					case LaneDirection.Both:
						icon = Properties.Resources.I_2W;
						break;
					case LaneDirection.Forward:
						icon = Properties.Resources.I_1WF;
						break;
					case LaneDirection.Backwards:
						icon = Properties.Resources.I_1WB;
						break;
					default:
						icon = Properties.Resources.I_Unavailable;
						break;
				}

				e.Graphics.DrawImage(icon.Color(directionRects[direction].Contains(cursor) ? FormDesign.Design.ActiveForeColor : LaneDirection == direction ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), directionRects[direction].CenterR(16, 16));
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && !iconRectangle.Contains(e.Location) && !deleteRectangle.Contains(e.Location) && !sizeRects.Any(x => x.Value.Contains(e.Location)) && !directionRects.Any(x => x.Value.Contains(e.Location)))
			{
				_dragDropActive = true;

				Refresh();

				DoDragDrop(this, DragDropEffects.Move);

				_dragDropActive = false;
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (e.Button != MouseButtons.Left || _dragDropActive)
				return;

			if (iconRectangle.Contains(e.Location))
			{
				new RoadTypeSelector(this);
			}

			if (deleteRectangle.Contains(e.Location))
			{
				Dispose();

				RoadLaneChanged?.Invoke(this, EventArgs.Empty);
			}

			foreach (var item in sizeRects)
			{
				if (item.Value.Contains(e.Location))
				{
					Lanes = item.Key;
					RoadLaneChanged?.Invoke(this, EventArgs.Empty);
					Invalidate();

					if (LaneType == LaneType.Parking)
					{
						foreach (var rl in Parent.Controls.OfType<RoadLane>().Where(x => x != this && x.LaneType == LaneType.Parking))
						{
							rl.Lanes = item.Key;
							rl.RoadLaneChanged?.Invoke(this, EventArgs.Empty);
							rl.Invalidate();
						}
					}

					return;
				}
			}

			foreach (var item in directionRects)
			{
				if (item.Value.Contains(e.Location))
				{
					LaneDirection = item.Key;

					switch (LaneDirection)
					{
						case LaneDirection.None:
							if (LaneType != LaneType.Parking)
								Lanes = 0;
							break;
						case LaneDirection.Both:
							if (LaneType != LaneType.Parking)
								Lanes = Math.Max(LaneType != LaneType.Pedestrian ? 2 : 1, Lanes);
							break;
						case LaneDirection.Forward:
						case LaneDirection.Backwards:
							if (LaneType != LaneType.Parking)
								Lanes = Math.Max(1, Lanes);
							break;
					}

					RoadLaneChanged?.Invoke(this, EventArgs.Empty);
					Invalidate();
					return;
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (iconRectangle.Contains(e.Location) || deleteRectangle.Contains(e.Location) || sizeRects.Any(x => x.Value.Contains(e.Location)) || directionRects.Any(x => x.Value.Contains(e.Location)))
			{
				Cursor = Cursors.Hand;
				return;
			}

			Cursor = Cursors.Default;
		}

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			base.OnDragDrop(drgevent);

			HandleDragAction(drgevent, true);
		}

		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			base.OnDragEnter(drgevent);

			HandleDragAction(drgevent, false);
		}

		public static void HandleDragAction(DragEventArgs drgevent, bool drop)
		{
			var item = drgevent.Data.GetData(typeof(RoadLane)) as RoadLane;

			if (item == null)
				return;

			if (drop)
			{
				item._dragDropActive = false;
				item.Invalidate();
			}

			drgevent.Effect = DragDropEffects.Move;

			var yIndex = item.Parent.PointToClient(Cursor.Position).Y + ((ScrollableControl)item.Parent).VerticalScroll.Value;
			var calculatedIndex = item.Parent.Controls.Count - 1 - (int)Math.Floor((double)yIndex / item.Height).Between(0, item.Parent.Controls.Count - 1);

			item.Parent.Controls.SetChildIndex(item, calculatedIndex);
			item.RoadLaneChanged?.Invoke(item, EventArgs.Empty);
		}
	}
}
