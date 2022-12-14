using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

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

		public LaneClass LaneType { get; set; }
		public LaneDirection LaneDirection { get; set; }
		public int Lanes { get; set; }
		public Func<bool> IsHighWay { get; }
		public float? CustomLaneWidth { get; set; }
		public float? CustomVerticalOffset { get; set; }
		public float? CustomSpeedLimit { get; set; }
		public bool AddStopToFiller { get; set; }
		public LaneDecoration Decorations { get; internal set; }

		public void SetLaneType(LaneClass laneType)
		{
			LaneType = laneType;

			sizeRects.Clear();
			directionRects.Clear();

			RefreshRoad();
		}

		private Rectangle iconRectangle;
		private Rectangle decoRectangle;
		private Rectangle deleteRectangle;
		private Rectangle grabberRectangle;
		private Rectangle editRectangle;
		private Rectangle sizeRectangle;
		private Rectangle sizeMinusRectangle;
		private Rectangle sizePlusRectangle;
		private readonly Dictionary<int, Rectangle> sizeRects = new Dictionary<int, Rectangle>();
		private readonly Dictionary<LaneDirection, Rectangle> directionRects = new Dictionary<LaneDirection, Rectangle>();

		private int yIndex => (Height - scale - 7) / 2;
		private Color foreColor => _dragDropActive ? Color.FromArgb(200, new LaneInfo { Class = LaneType }.Color.GetAccentColor()) : FormDesign.Design.ForeColor;

		protected override void OnPaint(PaintEventArgs e)
		{
			var cursor = PointToClient(Cursor.Position);
			var lane = new LaneInfo
			{
				Class = LaneType,
				Direction = LaneDirection,
				Lanes = Lanes,
			};

			e.Graphics.Clear(BackColor);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (_dragDropActive)
				e.Graphics.FillRoundedRectangle(new SolidBrush(LaneType == LaneClass.Empty ? FormDesign.Design.ActiveColor : lane.Color), ClientRectangle.Pad(0, 0, 1, 7), 6);
			else
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(0, 0, 1, 7), 6);

			var iconX = DrawIcon(e, cursor, lane);

			if (LaneType != LaneClass.Empty)
				DrawDecoIcon(e, cursor, ref iconX);

			DrawDeleteOrInfoIcon(e, cursor);

			var leftX = deleteRectangle.X;

			DrawLaneWidth(e, cursor, ref leftX);

			DrawLaneElevation(e, cursor, ref leftX);

			DrawLaneDirections(e, cursor, ref leftX);

			DrawLaneSpeed(e, cursor, ref leftX);

			//if (LaneType == LaneClass.Curb)
			//{
			//	e.Graphics.DrawString((LaneDirection == LaneDirection.Forward ? "Right" : "Left") + " curb delimiter", UI.Font(9.75F, FontStyle.Bold), new SolidBrush(FormDesign.Design.ForeColor), ClientRectangle.Pad(4), new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });

			//	Draw(e, cursor, iconX, lane, new int[0], null, new LaneDirection[0]);
			//}
			//else if (LaneType == LaneClass.Parking)
			//{
			//	Draw(e, cursor, iconX, lane,
			//		new[] { 1, 2, 4, 3 },
			//		GetParkingDirectionIcon,
			//		new[] { LaneDirection.None, LaneDirection.Backwards, LaneDirection.Forward });
			//}
			//else if (lane.IsFiller)
			//{
			//	Draw(e, cursor, iconX, lane,
			//		new[] { -10, -8, -6, -4, -2, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
			//		l => new ItemDrawContent { Text = $"{(10 - Math.Min(l, 9)) * 10}%" },
			//		new LaneDirection[0]);
			//}
			//else
			//{
			//	Draw(e, cursor, iconX, lane,
			//		LaneType == LaneClass.Pedestrian ? new[] { 0, 1, 2 } : new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 },
			//		l => new ItemDrawContent { Image = l == 0 ? Properties.Resources.I_Unavailable : null, Text = $"{l}L" },
			//		new[] { LaneDirection.None, LaneDirection.Backwards, LaneDirection.Forward, LaneDirection.Both });
			//}
		}

		private void DrawLaneSpeed(PaintEventArgs e, Point cursor, ref int leftX)
		{
		}

		private void DrawLaneDirections(PaintEventArgs e, Point cursor, ref int leftX)
		{
		}

		private void DrawLaneElevation(PaintEventArgs e, Point cursor, ref int leftX)
		{
		}

		private void DrawLaneWidth(PaintEventArgs e, Point cursor, ref int leftX)
		{
			leftX -= 6 + scale * 3;

			sizeMinusRectangle = new Rectangle(leftX, yIndex, scale, scale);
			sizeRectangle = new Rectangle(leftX + scale, yIndex, scale, scale);
			sizePlusRectangle = new Rectangle(leftX + scale * 2, yIndex, scale, scale);

			var size = CustomLaneWidth ?? LaneInfo.GetLaneTypes(LaneType).Max(y => Utilities.GetLaneWidth(y, new LaneInfo()));
			e.Graphics.DrawString($"{size:0.#}m", new Font(UI.FontFamily, 8.25F), new SolidBrush(sizeRectangle.Contains(cursor) ? FormDesign.Design.RedColor : foreColor), sizeRectangle, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			
			if (sizeMinusRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), sizeMinusRectangle, 4);

			e.Graphics.DrawImage(Properties.Resources.I_Minus.Color(sizeMinusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), sizeMinusRectangle.CenterR(16, 16));

			if (sizePlusRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), sizePlusRectangle, 4);

			e.Graphics.DrawImage(Properties.Resources.I_Plus.Color(sizePlusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), sizePlusRectangle.CenterR(16, 16));

			DrawLine(e, leftX - 3);
		}

		private void DrawDeleteOrInfoIcon(PaintEventArgs e, Point cursor)
		{
			var lane = new LaneInfo { Class = LaneType };
			var foreColor = _dragDropActive ? Color.FromArgb(150, lane.Color.GetAccentColor()) : FormDesign.Design.ForeColor;

			deleteRectangle = new Rectangle(Width - scale, yIndex, scale, scale);

			e.Graphics.DrawImage((LaneType != LaneClass.Curb ? Properties.Resources.I_X : Properties.Resources.I_Info).Color(deleteRectangle.Contains(cursor) ? (LaneType != LaneClass.Curb ? FormDesign.Design.RedColor : FormDesign.Design.ActiveColor) : foreColor), deleteRectangle.CenterR(16, 16));

			if (LaneType != LaneClass.Curb && deleteRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(25, FormDesign.Design.RedColor)), ClientRectangle.Pad(0, 0, 1, 7), 6);

			DrawLine(e, Width - scale - 2);
		}

		private int DrawIcon(PaintEventArgs e, Point cursor, LaneInfo lane)
		{
			var laneColor = lane.Color;
			var icons = lane.Icons(UI.FontScale <= 1.25, true);

			iconRectangle = new Rectangle(3, (Height - scale - 7) / 2, Math.Max(scale, icons.Count * scale), scale);

			if (LaneType == LaneClass.Empty)
				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 1.5F), iconRectangle, 6);
			else
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(iconRectangle.Contains(cursor) ? 50 : 100, laneColor)), iconRectangle, 6);

			var iconX = 3;
			var color = laneColor.GetAccentColor();
			foreach (var icon in icons.Select(x => x.Value))
			{
				if (LaneType == LaneClass.Curb && LaneDirection == LaneDirection.Backwards)
					icon.RotateFlip(RotateFlipType.RotateNoneFlipX);

				using (icon)
					e.Graphics.DrawIcon(icon, new Rectangle(iconX, (Height - scale - 7) / 2, scale, scale), UI.FontScale <= 1.25 ? (Size?)null : new Size(scale * 3 / 4, scale * 3 / 4));

				iconX += scale;
			}

			if (iconRectangle.Contains(cursor) && LaneType != LaneClass.Curb)
				DrawFocus(e.Graphics, iconRectangle.Pad(-1), HoverState.Focused, 6, LaneType == LaneClass.Empty ? (Color?)null : laneColor);

			if (!deleteRectangle.Contains(cursor) && ClientRectangle.Pad(0, 0, 1, 7).Contains(cursor) && HoverState.HasFlag(HoverState.Hovered))
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(25, laneColor)), ClientRectangle.Pad(0, 0, 1, 7), 6);

			return iconRectangle.Width + 12;
		}

		private void DrawDecoIcon(PaintEventArgs e, Point cursor, ref int iconX)
		{
			var icons = Decorations.GetValues().Select(x => ResourceManager.GetImage(x, UI.FontScale <= 1.25)).ToList();
			var laneColor = LaneInfo.GetColor(Decorations);

			iconX += 3;

			decoRectangle = new Rectangle(iconX, (Height - scale - 7) / 2, Math.Max(scale, icons.Count * scale), scale);

			if (Decorations == LaneDecoration.None)
				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 1.5F), decoRectangle, 6);
			else
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(decoRectangle.Contains(cursor) ? 50 : 100, laneColor)), decoRectangle, 6);

			var color = laneColor.GetAccentColor();
			foreach (var icon in icons)
			{
				if (Decorations == LaneDecoration.None)
					icon.Color(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.AccentColor));

				using (icon)
					e.Graphics.DrawIcon(icon, new Rectangle(iconX, (Height - scale - 7) / 2, scale, scale), UI.FontScale <= 1.25 ? (Size?)null : new Size(scale * 3 / 4, scale * 3 / 4));

				iconX += scale;
			}

			if (decoRectangle.Contains(cursor))
				DrawFocus(e.Graphics, decoRectangle.Pad(-1), HoverState.Focused, 6, Decorations == LaneDecoration.None ? (Color?)null : laneColor);

			if (!deleteRectangle.Contains(cursor) && ClientRectangle.Pad(0, 0, 1, 7).Contains(cursor) && HoverState.HasFlag(HoverState.Hovered))
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(25, laneColor)), ClientRectangle.Pad(0, 0, 1, 7), 6);

			iconX = decoRectangle.X + decoRectangle.Width + 12;
		}
		//{
		//	var laneColor = FormDesign.Design.AccentColor;
		//	var icon = ResourceManager.GetImage(Decorations, UI.FontScale <= 1.25);

		//	decoRectangle = new Rectangle(iconX + 3, (Height - scale - 7) / 2, Math.Max(scale, scale), scale);

		//	if (LaneType == LaneClass.Empty)
		//		e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 1.5F), decoRectangle, 6);
		//	else
		//		e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(decoRectangle.Contains(cursor) ? 100 : 200, laneColor)), decoRectangle, 6);

		//	using (icon)
		//		e.Graphics.DrawIcon(icon, decoRectangle, UI.FontScale <= 1.25 ? (Size?)null : new Size(scale * 3 / 4, scale * 3 / 4));

		//	iconX += scale;

		//	if (decoRectangle.Contains(cursor))
		//		DrawFocus(e.Graphics, decoRectangle.Pad(-1), HoverState.Focused, 6);	
		//}

		private ItemDrawContent GetParkingDirectionIcon(int arg)
		{
			if (arg < 2)
				return new ItemDrawContent { Image = Properties.Resources.I_Vertical };

			if (arg == 2)
				return new ItemDrawContent { Image = Properties.Resources.Icon_Horizontal };

			if (arg == 3)
				return new ItemDrawContent { Image = Properties.Resources.I_Diagonal };

			return new ItemDrawContent { Image = Properties.Resources.Icon_IDiagonal };
		}

		private void DrawLine(PaintEventArgs e, int x)
		{
			var lane = new LaneInfo { Class = LaneType };
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
					DrawFocus(e.Graphics, sizeRects[l], HoverState.Focused, 4);

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
						DrawFocus(e.Graphics, directionRects[direction], HoverState.Focused, 4);

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

			//e.Graphics.DrawImage(Properties.Resources.I_Edit.Color(foreColor), editRectangle.CenterR(16, 16));

			if (editRectangle.Contains(cursor))
				DrawFocus(e.Graphics, editRectangle.Pad(0, 1, 0, 1), HoverState.Focused, 4);

			DrawLine(e, editRectangle.X - 6);

			grabberRectangle = new Rectangle(iconX + 8, 0, editRectangle.X - 6 - iconX - 8, Height - 4);

			var drawGrabberRect = new Rectangle(scale * 2, 0, Width - (scale + 2) * 19 - 18, Height - 4);

			e.Graphics.DrawImage(Properties.Resources.I_Grabber.Color(_dragDropActive || grabberRectangle.Contains(cursor) ? FormDesign.Design.ActiveColor : foreColor), drawGrabberRect.CenterR(10, 5));
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

				return;
			}

			base.OnMouseDown(e);

			if (e.Button != MouseButtons.Left || _dragDropActive)
				return;

			if (iconRectangle.Contains(e.Location) && LaneType != LaneClass.Curb)
			{
				new LaneSpeedSelector(this);
				//new RoadTypeSelector(this);

				RoadLaneChanged?.Invoke(this, EventArgs.Empty);

				return;
			}

			if (decoRectangle.Contains(e.Location))
			{
				new DecoTypeSelector(this);

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
				if (LaneType != LaneClass.Curb)
				{
					Dispose();

					RoadLaneChanged?.Invoke(this, EventArgs.Empty);
				}
				else
				{
					MessagePrompt.Show($"This is the {(LaneDirection == LaneDirection.Forward ? "Right" : "Left")} Curb Delimiter;\r\n\r\nIt is used to specify what lanes are on the sidewalk and what lanes are on the asphalt.\r\n\r\nTry moving lanes and see how it affects the road in the preview.", PromptButtons.OK, PromptIcons.Info);
				}

				return;
			}

			if (sizePlusRectangle.Contains(e.Location) || sizeMinusRectangle.Contains(e.Location))
			{
				if (CustomLaneWidth == null)
					CustomLaneWidth = LaneInfo.GetLaneTypes(LaneType).Max(y => Utilities.GetLaneWidth(y, new LaneInfo()));

				CustomLaneWidth = (float)CustomLaneWidth + (sizePlusRectangle.Contains(e.Location) ? 1 : -1)
					* (ModifierKeys.HasFlag(Keys.Shift) ? 5F : ModifierKeys.HasFlag(Keys.Control) ? 1F : 0.1F);

				RefreshRoad();
			}

			if (sizeRectangle.Contains(e.Location))
			{
				CustomLaneWidth = null;
				RefreshRoad();
			}

			foreach (var item in sizeRects)
			{
				if (item.Value.Contains(e.Location))
				{
					Lanes = item.Key;
					RefreshRoad();

					if (LaneType == LaneClass.Parking)
					{
						foreach (var rl in Parent.Controls.OfType<RoadLane>().Where(x => x != this && x.LaneType == LaneClass.Parking))
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
							if (LaneType != LaneClass.Parking)
								Lanes = 0;
							break;
						case LaneDirection.Both:
							if (LaneType != LaneClass.Parking)
								Lanes = Math.Max(LaneType != LaneClass.Pedestrian ? 2 : 1, Lanes);
							break;
						case LaneDirection.Forward:
						case LaneDirection.Backwards:
							if (LaneType != LaneClass.Parking)
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

			if ((LaneType != LaneClass.Curb && iconRectangle.Contains(e.Location)) || deleteRectangle.Contains(e.Location) || decoRectangle.Contains(e.Location) || editRectangle.Contains(e.Location) || sizeRects.Any(x => x.Value.Contains(e.Location)) || directionRects.Any(x => x.Value.Contains(e.Location)))
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
				Class = LaneType,
				Direction = LaneDirection,
				Lanes = Lanes,
				Decorations = Decorations,
				CustomWidth = CustomLaneWidth ?? 0,
				Elevation = CustomVerticalOffset == -1 ? (float?)null : CustomVerticalOffset,
				SpeedLimit = CustomSpeedLimit == -1 ? (float?)null : CustomSpeedLimit,
				AddStopToFiller = AddStopToFiller,
				Width = (LaneType == LaneClass.Curb ? 5 : (LaneType < LaneClass.Car ? (10 - Math.Min(Lanes, 9)) : 10)) * (small ? 2 : 10)
			};
		}

		public RoadLane Duplicate()
		{
			return new RoadLane(IsHighWay)
			{
				LaneType = LaneType,
				LaneDirection = LaneDirection,
				Lanes = Lanes,
				Decorations = Decorations,
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

		internal void ApplyLaneType(LaneClass previousLaneType)
		{
			if (previousLaneType < LaneClass.Bike && LaneType >= LaneClass.Bike)
			{
				Lanes = 0;
			}

			if (LaneType < LaneClass.Bike)
			{
				LaneDirection = LaneDirection.None;

				if (previousLaneType >= LaneClass.Car)
					Lanes = 0;
			}

			if (LaneType == LaneClass.Parking)
			{
				Lanes = Parent.Controls.OfType<RoadLane>().FirstOrDefault(x => x != this && x.LaneType == LaneClass.Parking)?.Lanes ?? 0;

				if (LaneDirection == LaneDirection.Both)
					LaneDirection = LaneDirection.None;
			}

			if (LaneType >= LaneClass.Pedestrian)
			{
				LaneType &= ~LaneClass.Empty & ~LaneClass.Filler;
			}

			if (LaneType.HasFlag(LaneClass.Parking))
			{
				LaneType = LaneClass.Parking;
				Lanes = Lanes.Between(1, 3);
			}

			if (LaneType.HasFlag(LaneClass.Pedestrian))
			{
				Lanes = Lanes.Between(0, 2);
			}

			if ((LaneType < LaneClass.Bike || LaneType == LaneClass.Parking) && LaneDirection != LaneDirection.None)
			{
				LaneDirection = LaneDirection.None;
			}

			if (Decorations == LaneDecoration.None && (LaneType == LaneClass.Bike || LaneType == LaneClass.Bus || LaneType == LaneClass.Trolley))
				Decorations = LaneDecoration.Filler;

			if (!Decorations.IsCompatible(LaneType))
				Decorations = LaneDecoration.None;

			sizeRects.Clear();
			directionRects.Clear();
			iconRectangle = Rectangle.Empty;
			decoRectangle = Rectangle.Empty;
			deleteRectangle = Rectangle.Empty;
			grabberRectangle = Rectangle.Empty;
			editRectangle = Rectangle.Empty;

			RefreshRoad();
		}

		internal void SetDecorations(LaneDecoration laneDecorationStyle)
		{
			Decorations = laneDecorationStyle;

			RefreshRoad();
		}
	}
}
