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
			Dock = DockStyle.Top;
		}

		public LaneType LaneType { get => _laneType == LaneType.Car && IsHighWay() ? LaneType.Highway : _laneType; set => _laneType = value; }
		public LaneDirection LaneDirection { get; set; }
		public int Lanes { get; set; }
		public Func<bool> IsHighWay { get; }
		public float CustomLaneWidth { get; set; } = -1F;
		public float CustomVerticalOffset { get; set; } = -1F;
		public float CustomSpeedLimit { get; set; } = -1F;

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
		private Rectangle grabberRectangle;
		private LaneType _laneType;
		private Rectangle editRectangle;
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

			var iconX = DrawIcon(e, cursor, lane);

			DrawDeleteIcon(e, cursor);

			if (LaneType == LaneType.Parking)
			{
				Draw(e, cursor, iconX,
					new[] { 1, 2, 3 },
					GetParkingDirectionIcon,
					new[] { LaneDirection.None, LaneDirection.Backwards, LaneDirection.Forward });
			}
			else if (lane.IsFiller)
			{
				Draw(e, cursor, iconX,
					new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
					l => new ItemDrawContent { Text = $"{(10 - Math.Min(l, 9)) * 10}%" },
					new LaneDirection[0]);
			}
			else
			{
				Draw(e, cursor, iconX,
					LaneType == LaneType.Pedestrian ? new[] { 0, 1, 2 } : new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },
					l => new ItemDrawContent { Image = l == 0 ? Properties.Resources.I_Unavailable : null, Text = $"{l}L" },
					new[] { LaneDirection.None, LaneDirection.Backwards, LaneDirection.Forward, LaneDirection.Both });
			}
		}

		private void DrawDeleteIcon(PaintEventArgs e, Point cursor)
		{
			deleteRectangle = new Rectangle(Width - 32, (Height - 32 - 7) / 2, 32, 32);

			e.Graphics.DrawImage(Properties.Resources.I_X.Color(deleteRectangle.Contains(cursor) ? FormDesign.Design.RedColor : FormDesign.Design.IconColor), deleteRectangle.CenterR(16, 16));

			DrawLine(e, Width - 34);
		}

		private int DrawIcon(PaintEventArgs e, Point cursor, LaneInfo lane)
		{
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

			DrawLine(e, iconX + 8);
			return iconX;
		}

		private ItemDrawContent GetParkingDirectionIcon(int arg)
		{
			if (arg < 2)
				return new ItemDrawContent { Image = Properties.Resources.I_Vertical };

			if (arg == 2)
				return new ItemDrawContent { Image = Properties.Resources.Icon_Horizontal };

			return new ItemDrawContent { Image = Properties.Resources.I_Diagonal };
		}

		private void DrawLine(PaintEventArgs e, int x)
		{
			e.Graphics.DrawLine(new Pen(FormDesign.Design.AccentColor), x, 6, x, Height - 13);
		}

		private void Draw(PaintEventArgs e, Point cursor, int iconX, int[] lanes, Func<int, ItemDrawContent> drawLane, LaneDirection[] directions)
		{
			// Draw lane buttons
			var i = 0;
			foreach (var l in lanes)
			{
				sizeRects[l] = new Rectangle(Width - (32 * (1 + lanes.Length - i++)) - 6, (Height - 32 - 7) / 2, 32, 32);

				if (sizeRects[l].Contains(cursor))
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), sizeRects[l], 4);

				var content = drawLane(l);

				if (content.Image != null)
					e.Graphics.DrawImage(content.Image.Color(sizeRects[l].Contains(cursor) ? FormDesign.Design.ActiveForeColor : l == Lanes ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), sizeRects[l].CenterR(16, 16));
				else
					e.Graphics.DrawString(content.Text, new Font(UI.FontFamily, 8.25F), new SolidBrush(sizeRects[l].Contains(cursor) ? FormDesign.Design.ActiveForeColor : l == Lanes ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor), sizeRects[l], new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			}

			DrawLine(e, Width - (32 * (1 + lanes.Length)) - 12);

			if (directions.Length > 0)
			{ 
				// Draw direction buttons
				var ind = 0;
				foreach (var direction in directions)
				{
					directionRects[direction] = new Rectangle(Width - (32 * (1 + lanes.Length - ind++ + directions.Length)) - 18, (Height - 32 - 7) / 2, 32, 32);

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

				DrawLine(e, Width - (32 * (1 + lanes.Length + directions.Length)) - 24);
			}

			// Draw direction buttons
			editRectangle = new Rectangle(Width - (32 * (1 + lanes.Length + directions.Length + 1)) - (directions.Any() ? 30 : 18), (Height - 32 - 7) / 2, 32, 32);

			e.Graphics.DrawImage(Properties.Resources.I_Edit.Color(FormDesign.Design.IconColor), editRectangle.CenterR(16, 16));

			if (editRectangle.Contains(cursor))
				DrawFocus(e.Graphics, editRectangle.Pad(0, 1, 0, 1), HoverState.Focused, 4);

			DrawLine(e, editRectangle.X - 6);

			grabberRectangle = new Rectangle(iconX + 8, 0, editRectangle.X - 6 - iconX - 8, Height - 4);

			e.Graphics.DrawImage(Properties.Resources.I_Grabber.Color(grabberRectangle.Contains(cursor) ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), grabberRectangle.CenterR(10, 5));
		}

		struct ItemDrawContent
		{
			public string Text;
			public Bitmap Image;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && !iconRectangle.Contains(e.Location) && !editRectangle.Contains(e.Location) && !deleteRectangle.Contains(e.Location) && !sizeRects.Any(x => x.Value.Contains(e.Location)) && !directionRects.Any(x => x.Value.Contains(e.Location)))
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

				RoadLaneChanged?.Invoke(this, EventArgs.Empty);

				return;
			}

			if (editRectangle.Contains(e.Location))
			{
				new RoadOptionsEditor(this);

				RoadLaneChanged?.Invoke(this, EventArgs.Empty);

				return;
			}

			if (deleteRectangle.Contains(e.Location))
			{
				Dispose();

				RoadLaneChanged?.Invoke(this, EventArgs.Empty);

				return;
			}

			foreach (var item in sizeRects)
			{
				if (item.Value.Contains(e.Location))
				{
					Lanes = item.Key;
					RefreshRoad();

					if (LaneType == LaneType.Parking)
					{
						foreach (var rl in Parent.Controls.OfType<RoadLane>().Where(x => x != this && x.LaneType == LaneType.Parking))
						{
							rl.Lanes = item.Key;
							rl.RefreshRoad();
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

					RefreshRoad();
					return;
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (iconRectangle.Contains(e.Location) || editRectangle.Contains(e.Location) || deleteRectangle.Contains(e.Location) || sizeRects.Any(x => x.Value.Contains(e.Location)) || directionRects.Any(x => x.Value.Contains(e.Location)))
			{
				Cursor = Cursors.Hand;
				return;
			}

			Cursor = grabberRectangle.Contains(e.Location) ? Cursors.SizeAll : Cursors.Default;
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

		public LaneInfo GetLane(bool small)
		{
			return new LaneInfo
			{
				Type = LaneType,
				Direction = LaneDirection,
				Lanes = Lanes,
				CustomWidth = CustomLaneWidth == -1 ? 0 : CustomLaneWidth,
				Elevation = CustomVerticalOffset == -1 ? (float?)null : CustomVerticalOffset,
				SpeedLimit = CustomSpeedLimit == -1 ? (float?)null : CustomSpeedLimit,
				Width = (LaneType < LaneType.Car ? (10 - Math.Min(Lanes, 9)) : 10) * (small ? 2 : 10)
			};
		}

		public RoadLane Duplicate()
		{
			return new RoadLane(IsHighWay)
			{
				LaneType = LaneType,
				LaneDirection = LaneDirection,
				Lanes = Lanes,
				CustomLaneWidth = CustomLaneWidth,
				CustomVerticalOffset = CustomVerticalOffset,
				CustomSpeedLimit = CustomSpeedLimit,
			};
		}

		internal void RefreshRoad()
		{
			Invalidate();

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);
		}

		internal void ApplyLaneType(LaneType previousLaneType)
		{
			if (previousLaneType < LaneType.Car && LaneType >= LaneType.Car)
			{
				Lanes = 0;
			}

			if (LaneType < LaneType.Car)
			{
				LaneDirection = LaneDirection.None;

				if (previousLaneType >= LaneType.Car)
					Lanes = 0;
			}

			if (LaneType == LaneType.Parking)
			{
				Lanes = Parent.Controls.OfType<RoadLane>().FirstOrDefault(x => x != this && x.LaneType == LaneType.Parking)?.Lanes ?? 0;

				if (LaneDirection == LaneDirection.Both)
					LaneDirection = LaneDirection.None;
			}

			if (LaneType >= LaneType.Car)
			{
				LaneType &= ~LaneType.Empty & ~LaneType.Grass & ~LaneType.Gravel & ~LaneType.Pavement;
			}

			if (LaneType.HasFlag(LaneType.Parking))
			{
				LaneType = LaneType.Parking;
				Lanes = Lanes.Between(1, 3);
			}

			if (LaneType.HasFlag(LaneType.Pedestrian))
			{
				Lanes = Lanes.Between(0, 2);
			}

			RefreshRoad();
		}
	}
}
