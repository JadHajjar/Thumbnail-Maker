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
		private int scale;

		public event EventHandler RoadLaneChanged;

		public RoadLane(Func<bool> highwayCheck)
		{
			IsHighWay = highwayCheck;
			AllowDrop = true;
			Dock = DockStyle.Top;
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			Height = (int)(35 * UI.UIScale) + 7;
			scale = (int)(30 * UI.UIScale);
		}

		public LaneType LaneType { get => _laneType == LaneType.Car && IsHighWay() ? LaneType.Highway : _laneType; set => _laneType = value; }
		public LaneDirection LaneDirection { get; set; }
		public int Lanes { get; set; }
		public Func<bool> IsHighWay { get; }
		public float CustomLaneWidth { get; set; } = -1F;
		public float CustomVerticalOffset { get; set; } = -1F;
		public float CustomSpeedLimit { get; set; } = -1F;
		public bool AddStopToFiller { get; set; }

		public void SetLaneType(LaneType laneType)
		{
			LaneType = laneType;

			sizeRects.Clear();
			directionRects.Clear();

			RefreshRoad();
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


			if (_dragDropActive)
				e.Graphics.FillRoundedRectangle(new SolidBrush(LaneType == LaneType.Empty ? FormDesign.Design.ActiveColor : lane.Color), ClientRectangle.Pad(0, 0, 1, 7), 6);
			else
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(0, 0, 1, 7), 6);

			var iconX = DrawIcon(e, cursor, lane);

			DrawDeleteIcon(e, cursor);

			if (LaneType == LaneType.Parking)
			{
				Draw(e, cursor, iconX, lane,
					new[] { 1, 2, 3 },
					GetParkingDirectionIcon,
					new[] { LaneDirection.None, LaneDirection.Backwards, LaneDirection.Forward });
			}
			else if (lane.IsFiller)
			{
				Draw(e, cursor, iconX, lane,
					new[] { -10, -8, -6, -4, -2, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
					l => new ItemDrawContent { Text = $"{(10 - Math.Min(l, 9)) * 10}%" },
					new LaneDirection[0]);
			}
			else if (LaneType == LaneType.Pedestrian && Parent.Controls.IndexOf(this).AnyOf(0, Parent.Controls.Count - 1))
			{
				grabberRectangle = new Rectangle(iconX + 8, 0, Width - 34 - iconX - 8, Height - 4);

				e.Graphics.DrawImage(Properties.Resources.I_Grabber.Color(grabberRectangle.Contains(cursor) ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), grabberRectangle.CenterR(10, 5));
			}
			else
			{
				Draw(e, cursor, iconX, lane,
					LaneType == LaneType.Pedestrian ? new[] { 0, 1, 2 } : new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },
					l => new ItemDrawContent { Image = l == 0 ? Properties.Resources.I_Unavailable : null, Text = $"{l}L" },
					new[] { LaneDirection.None, LaneDirection.Backwards, LaneDirection.Forward, LaneDirection.Both });
			}
		}

		private void DrawDeleteIcon(PaintEventArgs e, Point cursor)
		{
			var lane = new LaneInfo { Type = LaneType };
			var foreColor = _dragDropActive ? Color.FromArgb(150, lane.Color.GetAccentColor()) : FormDesign.Design.ForeColor;

			deleteRectangle = new Rectangle(Width - scale, (Height - scale - 7) / 2, scale, scale);

			e.Graphics.DrawImage(Properties.Resources.I_X.Color(deleteRectangle.Contains(cursor) ? FormDesign.Design.RedColor : foreColor), deleteRectangle.CenterR(16, 16));

			if (deleteRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(25, FormDesign.Design.RedColor)), ClientRectangle.Pad(0, 0, 1, 7), 6);

			DrawLine(e, Width - scale - 2);
		}

		private int DrawIcon(PaintEventArgs e, Point cursor, LaneInfo lane)
		{
			var laneColor = lane.Color;
			var icons = lane.Icons(UI.FontScale <= 1.25).Where(x => x != null).ToList();

			iconRectangle = new Rectangle(3, (Height - scale - 7) / 2, Math.Max(scale, icons.Count * scale), scale);

			if (LaneType == LaneType.Empty)
				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 1.5F), iconRectangle, 6);
			else
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(iconRectangle.Contains(cursor) ? 100 : 200, laneColor)), iconRectangle, 6);

			var iconX = 3;
			var color = laneColor.GetAccentColor();
			foreach (var icon in icons)
			{
				using (icon)
					e.Graphics.DrawImage(icon.Color(color), new Rectangle(iconX, (Height - scale - 7) / 2, scale, scale).CenterR(UI.FontScale <= 1.25 ? icon.Size : new Size(scale * 2 / 3, scale * 2 / 3)));

				iconX += scale;
			}

			if (iconRectangle.Contains(cursor))
				DrawFocus(e.Graphics, iconRectangle.Pad(-1), HoverState.Focused, 6, LaneType == LaneType.Empty ? (Color?)null : laneColor);

			if (!deleteRectangle.Contains(cursor) && ClientRectangle.Pad(0, 0, 1, 7).Contains(cursor) && HoverState.HasFlag(HoverState.Hovered))
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(25, laneColor)), ClientRectangle.Pad(0, 0, 1, 7), 6);

			return iconRectangle.Width+12;
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
			var lane = new LaneInfo { Type = LaneType };
			var foreColor = _dragDropActive ? Color.FromArgb(150, lane.Color.GetAccentColor()) : FormDesign.Design.AccentColor;

			e.Graphics.DrawLine(new Pen(foreColor), x, 6, x, Height - 13);
		}

		private void Draw(PaintEventArgs e, Point cursor, int iconX, LaneInfo lane, int[] lanes, Func<int, ItemDrawContent> drawLane, LaneDirection[] directions)
		{
			var foreColor = _dragDropActive ? Color.FromArgb(200, lane.Color.GetAccentColor()) : FormDesign.Design.ForeColor;

			// Draw lane buttons
			var i = 0;
			foreach (var l in lanes)
			{
				sizeRects[l] = new Rectangle(Width - ((scale + 2) * (1 + lanes.Length - i++)) - 6, (Height - scale - 7) / 2, scale, scale);

				if (sizeRects[l].Contains(cursor))
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), sizeRects[l], 4);
				else if (l == Lanes)
					e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(_dragDropActive ? 255 : 200, FormDesign.Design.ActiveForeColor)), sizeRects[l], 4);

				var content = drawLane(l);

				if (content.Image != null)
					e.Graphics.DrawImage(content.Image.Color(sizeRects[l].Contains(cursor) ? FormDesign.Design.ActiveForeColor : l == Lanes ? FormDesign.Design.ActiveColor : foreColor), sizeRects[l].CenterR(16, 16));
				else
					e.Graphics.DrawString(content.Text, new Font(UI.FontFamily, 8.25F), new SolidBrush(sizeRects[l].Contains(cursor) ? FormDesign.Design.ActiveForeColor : l == Lanes ? FormDesign.Design.ActiveColor : foreColor), sizeRects[l].Pad(-3), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			}

			DrawLine(e, Width - ((scale + 2) * (1 + lanes.Length)) - 12);

			if (directions.Length > 0)
			{ 
				// Draw direction buttons
				var ind = 0;
				foreach (var direction in directions)
				{
					directionRects[direction] = new Rectangle(Width - ((scale + 2) * (1 + lanes.Length - ind++ + directions.Length)) - 18, (Height - scale - 7) / 2, scale, scale);

					if (directionRects[direction].Contains(cursor))
						e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), directionRects[direction], 4);
					else if (LaneDirection == direction)
						e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(_dragDropActive ? 255 : 200, FormDesign.Design.ActiveForeColor)), directionRects[direction], 4);

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

					e.Graphics.DrawImage(icon.Color(directionRects[direction].Contains(cursor) ? FormDesign.Design.ActiveForeColor : LaneDirection == direction ? FormDesign.Design.ActiveColor : foreColor), directionRects[direction].CenterR(16, 16));
				}

				DrawLine(e, Width - ((scale + 2) * (1 + lanes.Length + directions.Length)) - 24);
			}

			// Draw direction buttons
			editRectangle = new Rectangle(Width - ((scale + 2) * (1 + lanes.Length + directions.Length + 1)) - (directions.Any() ? 30 : 18), (Height - scale - 7) / 2, scale, scale);

			e.Graphics.DrawImage(Properties.Resources.I_Edit.Color(foreColor), editRectangle.CenterR(16, 16));

			if (editRectangle.Contains(cursor))
				DrawFocus(e.Graphics, editRectangle.Pad(0, 1, 0, 1), HoverState.Focused, 4);

			DrawLine(e, editRectangle.X - 6);

			grabberRectangle = new Rectangle(iconX + 8, 0, editRectangle.X - 6 - iconX - 8, Height - 4);

			e.Graphics.DrawImage(Properties.Resources.I_Grabber.Color(_dragDropActive || grabberRectangle.Contains(cursor) ? FormDesign.Design.ActiveColor : foreColor), grabberRectangle.CenterR(10, 5));
		}

		struct ItemDrawContent
		{
			public string Text;
			public Bitmap Image;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && grabberRectangle.Contains(e.Location))
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
				AddStopToFiller = AddStopToFiller,
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
				AddStopToFiller = AddStopToFiller,
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
				LaneType &= ~LaneType.Empty & ~LaneType.Grass & ~LaneType.Gravel & ~LaneType.Pavement & ~LaneType.Trees;
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

			if ((LaneType < LaneType.Car || LaneType == LaneType.Parking) && LaneDirection != LaneDirection.None)
			{
				LaneDirection = LaneDirection.None;
			}

			sizeRects.Clear();
			directionRects.Clear();

			RefreshRoad();
		}
	}
}
