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
		public float? CustomWidth { get; set; }
		public float? Elevation { get; set; }
		public float? SpeedLimit { get; set; }
		public bool AddStopToFiller { get; set; }
		public LaneDecoration Decorations { get; internal set; }

		public void SetLaneType(LaneClass laneType)
		{
			LaneType = laneType;

			RefreshRoad();
		}

		private Rectangle grabberRectangle;
		private readonly Dictionary<Rectangle, MouseEventHandler> _clickActions = new Dictionary<Rectangle, MouseEventHandler>();

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

			_clickActions.Clear();

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (_dragDropActive)
				e.Graphics.FillRoundedRectangle(new SolidBrush(LaneType == LaneClass.Empty ? FormDesign.Design.ActiveColor : lane.Color), ClientRectangle.Pad(0, 0, 1, 7), 6);
			else
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(0, 0, 1, 7), 6);

			var iconX = DrawIcon(e, cursor, lane);

			if (LaneType != LaneClass.Empty)
				DrawDecoIcon(e, cursor, ref iconX);
					
			var leftX = DrawDeleteOrInfoIcon(e, cursor);

			DrawLaneWidth(e, cursor, ref leftX);

			if (Options.Current.AdvancedElevation)
				DrawLaneAdvancedElevation(e, cursor, ref leftX);
			else
				DrawLaneElevation(e, cursor, ref leftX);

			DrawLaneDirections(e, cursor, ref leftX);

			DrawLaneSpeed(e, cursor, ref leftX);

			grabberRectangle = new Rectangle(iconX + 8, 0, leftX-iconX, Height - 4);

			var drawGrabberRect = new Rectangle(scale * 4, 0, Width - (scale + 2) * 19 - 18, Height - 4);

			e.Graphics.DrawImage(Properties.Resources.I_Grabber.Color(_dragDropActive || grabberRectangle.Contains(cursor) ? FormDesign.Design.ActiveColor : foreColor), drawGrabberRect.CenterR(10, 5));
		}

		private void DrawLaneSpeed(PaintEventArgs e, Point cursor, ref int leftX)
		{
			if (!LaneType.HasAnyFlag(LaneClass.Bike, LaneClass.Car, LaneClass.Bus, LaneClass.Tram, LaneClass.Trolley, LaneClass.Emergency))
				return;

			leftX -= 6 + scale;

			var speedRectangle = new Rectangle(leftX, yIndex, scale, scale);

			if (speedRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), speedRectangle, 4);

			ThumbnailHandler.DrawSpeedSignSmall(e.Graphics, Options.Current.Region, (int)(SpeedLimit ?? 0F), speedRectangle);

			_clickActions[speedRectangle] = SpeedLimitClick;
		}

		private void DrawLaneDirections(PaintEventArgs e, Point cursor, ref int leftX)
		{
			var directions = LaneType == LaneClass.Parking ? new[] { LaneDirection.None, LaneDirection.Backwards, LaneDirection.Forward } : LaneType <= LaneClass.Curb ? new LaneDirection[0] : new[] { LaneDirection.None, LaneDirection.Backwards, LaneDirection.Forward, LaneDirection.Both };

			if (directions.Length == 0)
				return;

			leftX -= 6 + scale * directions.Length;

			// Draw direction buttons
			var ind = 0;
			foreach (var direction in directions)
			{
				var rect = new Rectangle(leftX + scale * ind++, yIndex, scale, scale);

				if (rect.Contains(cursor))
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), rect, 4);
				else if (LaneDirection == direction)
					DrawFocus(e.Graphics, rect, HoverState.Focused, 4);

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

				e.Graphics.DrawImage(icon.Color(rect.Contains(cursor) ? FormDesign.Design.ActiveForeColor : LaneDirection == direction ? FormDesign.Design.ActiveColor : foreColor), rect.CenterR(16, 16));

				_clickActions[rect] = (_, __) =>
				{
					LaneDirection = direction;
					RefreshRoad();
				};
			}

			DrawLine(e, leftX - 3);
		}

		private void DrawLaneElevation(PaintEventArgs e, Point cursor, ref int leftX)
		{
			leftX -= 6 + scale * 2;

			var elevationMinusRectangle = new Rectangle(leftX, yIndex, scale, scale);
			var elevationPlusRectangle = new Rectangle(leftX + scale, yIndex, scale, scale);

			if (elevationMinusRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), elevationMinusRectangle, 4);

			e.Graphics.DrawImage(Properties.Resources.I_Down.Color(elevationMinusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), elevationMinusRectangle.CenterR(16, 16));

			if (elevationPlusRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), elevationPlusRectangle, 4);

			e.Graphics.DrawImage(Properties.Resources.I_Up.Color(elevationPlusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), elevationPlusRectangle.CenterR(16, 16));

			DrawFocus(e.Graphics, Elevation == null ? elevationMinusRectangle : elevationPlusRectangle, HoverState.Focused, 4);

			DrawLine(e, leftX - 3);

			_clickActions[elevationMinusRectangle] = ElevationMinusClick;
			_clickActions[elevationPlusRectangle] = ElevationPlusClick;
		}

		private void DrawLaneAdvancedElevation(PaintEventArgs e, Point cursor, ref int leftX)
		{
			leftX -= 6 + scale * 3;

			var elevationMinusRectangle = new Rectangle(leftX, yIndex, scale, scale);
			var elevationRectangle = new Rectangle(leftX + scale, yIndex, scale, scale);
			var elevationPlusRectangle = new Rectangle(leftX + scale * 2, yIndex, scale, scale);

			var size = Elevation ?? GetDefaultElevation();
			e.Graphics.DrawString($"{size:0.#}m", new Font(UI.FontFamily, 8.25F), new SolidBrush(elevationRectangle.Contains(cursor) ? FormDesign.Design.RedColor : foreColor), elevationRectangle, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });

			if (elevationMinusRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), elevationMinusRectangle, 4);

			e.Graphics.DrawImage(Properties.Resources.I_Down.Color(elevationMinusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), elevationMinusRectangle.CenterR(16, 16));

			if (elevationPlusRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), elevationPlusRectangle, 4);

			e.Graphics.DrawImage(Properties.Resources.I_Up.Color(elevationPlusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), elevationPlusRectangle.CenterR(16, 16));

			DrawLine(e, leftX - 3);

			_clickActions[elevationMinusRectangle] = ElevationMinusClick;
			_clickActions[elevationPlusRectangle] = ElevationPlusClick;
			_clickActions[elevationRectangle] = ElevationResetClick;
		}

		private void DrawLaneWidth(PaintEventArgs e, Point cursor, ref int leftX)
		{
			leftX -= 6 + scale * 3;

			var sizeMinusRectangle = new Rectangle(leftX, yIndex, scale, scale);
			var sizeRectangle = new Rectangle(leftX + scale, yIndex, scale, scale);
			var sizePlusRectangle = new Rectangle(leftX + scale * 2, yIndex, scale, scale);

			var size = CustomWidth ?? LaneInfo.GetLaneTypes(LaneType).Max(y => Utilities.GetLaneWidth(y, new LaneInfo()));
			e.Graphics.DrawString($"{size:0.##}m", new Font(UI.FontFamily, 8.25F), new SolidBrush(sizeRectangle.Contains(cursor) ? FormDesign.Design.RedColor : foreColor), sizeRectangle, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			
			if (sizeMinusRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), sizeMinusRectangle, 4);

			e.Graphics.DrawImage(Properties.Resources.I_Minus.Color(sizeMinusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), sizeMinusRectangle.CenterR(16, 16));

			if (sizePlusRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), sizePlusRectangle, 4);

			e.Graphics.DrawImage(Properties.Resources.I_Plus.Color(sizePlusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), sizePlusRectangle.CenterR(16, 16));

			DrawLine(e, leftX - 3);

			_clickActions[sizeMinusRectangle] = WidthMinusClick;
			_clickActions[sizePlusRectangle] = WidthPlusClick;
			_clickActions[sizeRectangle] = WidthResetClick;
		}

		private int DrawDeleteOrInfoIcon(PaintEventArgs e, Point cursor)
		{
			var lane = new LaneInfo { Class = LaneType };
			var foreColor = _dragDropActive ? Color.FromArgb(150, lane.Color.GetAccentColor()) : FormDesign.Design.ForeColor;

			var deleteRectangle = new Rectangle(Width - scale, yIndex, scale, scale);

			e.Graphics.DrawImage((LaneType != LaneClass.Curb ? Properties.Resources.I_X : Properties.Resources.I_Info).Color(deleteRectangle.Contains(cursor) ? (LaneType != LaneClass.Curb ? FormDesign.Design.RedColor : FormDesign.Design.ActiveColor) : foreColor), deleteRectangle.CenterR(16, 16));

			if (LaneType != LaneClass.Curb && deleteRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(25, FormDesign.Design.RedColor)), ClientRectangle.Pad(0, 0, 1, 7), 6);
			else if (ClientRectangle.Pad(0, 0, 1, 7).Contains(cursor) && HoverState.HasFlag(HoverState.Hovered))
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(25, lane.Color)), ClientRectangle.Pad(0, 0, 1, 7), 6);

			DrawLine(e, Width - scale - 2);

			if (LaneType != LaneClass.Curb)
				_clickActions[deleteRectangle] = DeleteLaneClick;
			else
				_clickActions[deleteRectangle] = InfoLaneClick;

			return deleteRectangle.X;
		}

		private int DrawIcon(PaintEventArgs e, Point cursor, LaneInfo lane)
		{
			var laneColor = lane.Color;
			var icons = lane.Icons(UI.FontScale <= 1.25, true);

			var iconRectangle = new Rectangle(3, (Height - scale - 7) / 2, Math.Max(scale, icons.Count * scale), scale);

			if (LaneType == LaneClass.Empty)
				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 1.5F), iconRectangle, 6);
			else
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(iconRectangle.Contains(cursor) && LaneType != LaneClass.Curb ? 50 : 100, laneColor)), iconRectangle, 6);

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

			if (LaneType != LaneClass.Curb)
				_clickActions[iconRectangle] = LaneTypeClick;

			return iconRectangle.Width + 12;
		}

		private void DrawDecoIcon(PaintEventArgs e, Point cursor, ref int iconX)
		{
			var icons = Decorations.GetValues().Select(x => ResourceManager.GetImage(x, UI.FontScale <= 1.25)).ToList();
			var laneColor = LaneInfo.GetColor(Decorations);

			iconX += 3;

			var decoRectangle = new Rectangle(iconX, (Height - scale - 7) / 2, Math.Max(scale, icons.Count * scale), scale);

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

			iconX = decoRectangle.X + decoRectangle.Width + 12;

			_clickActions[decoRectangle] = LaneDecoClick;
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

		private Bitmap GetParkingDirectionIcon(int arg)
		{
			if (arg < 2)
				return Properties.Resources.I_Vertical;

			if (arg == 2)
				return Properties.Resources.Icon_Horizontal;

			if (arg == 3)
				return Properties.Resources.I_Diagonal;

			return Properties.Resources.Icon_IDiagonal;
		}

		private void DrawLine(PaintEventArgs e, int x)
		{
			var lane = new LaneInfo { Class = LaneType };
			var foreColor = _dragDropActive ? Color.FromArgb(150, lane.Color.GetAccentColor()) : FormDesign.Design.AccentColor;

			e.Graphics.DrawLine(new Pen(foreColor), x, 6, x, Height - 13);
		}

		private float GetDefaultElevation()
		{
			var leftSidewalk = Parent.Controls.OfType<RoadLane>().FirstOrDefault(x => x.LaneType == LaneClass.Curb && x.LaneDirection == LaneDirection.Backwards);
			var rightSidewalk = Parent.Controls.OfType<RoadLane>().LastOrDefault(x => x.LaneType == LaneClass.Curb && x.LaneDirection == LaneDirection.Forward);

			if (leftSidewalk != null && rightSidewalk != null)
			{
				var index = Parent.Controls.IndexOf(this);

				if (index < Parent.Controls.IndexOf(leftSidewalk) && index > Parent.Controls.IndexOf(rightSidewalk))
					return -0.3F;
			}

			return 0;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (grabberRectangle.Contains(e.Location))
			{
				GrabberClick(this, e);

				return;
			}

			base.OnMouseDown(e); }

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (e.Button != MouseButtons.Left || _dragDropActive)
				return;

			foreach (var item in _clickActions)
			{
				if (item.Key.Contains(e.Location))
				{
					item.Value(this, e);

					return;
				}
			}
		}

		private void GrabberClick(object sender, MouseEventArgs e)
		{
			_dragDropActive = true;

			Refresh();

			DoDragDrop(this, DragDropEffects.Move);

			_dragDropActive = false;
		}

		private void LaneTypeClick(object sender, MouseEventArgs e)
		{
			new RoadTypeSelector(this);

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);
		}

		private void LaneDecoClick(object sender, MouseEventArgs e)
		{
			new DecoTypeSelector(this);

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);
		}

		private void SpeedLimitClick(object sender, MouseEventArgs e)
		{
			new LaneSpeedSelector(this);

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);
		}

		private void DeleteLaneClick(object sender, MouseEventArgs e)
		{
			Dispose();

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);
		}

		private void InfoLaneClick(object sender, MouseEventArgs e)
		{
			MessagePrompt.Show($"This is the {(LaneDirection == LaneDirection.Forward ? "Right" : "Left")} Curb Delimiter;\r\n\r\nIt is used to seperate what lanes are on the {(LaneDirection == LaneDirection.Forward ? "right" : "left")} sidewalk and what lanes are on the asphalt.\r\n\r\nTry moving lanes and see how it affects the road in the preview.", PromptButtons.OK, PromptIcons.Info);
		}

		private void WidthResetClick(object sender, MouseEventArgs e)
		{
			CustomWidth = null;

			RefreshRoad();
		}

		private void WidthPlusClick(object sender, MouseEventArgs e)
		{
			if (CustomWidth == null)
				CustomWidth = LaneInfo.GetLaneTypes(LaneType).Max(y => Utilities.GetLaneWidth(y, new LaneInfo()));

			CustomWidth = (float)Math.Max(0.1, Math.Round((float)CustomWidth +
				(ModifierKeys.HasFlag(Keys.Shift) ? 5F : ModifierKeys.HasFlag(Keys.Control) ? 1F : ModifierKeys.HasFlag(Keys.Alt) ? 0.01F : 0.1F), 2));

			RefreshRoad();
		}

		private void WidthMinusClick(object sender, MouseEventArgs e)
		{
			if (CustomWidth == null)
				CustomWidth = LaneInfo.GetLaneTypes(LaneType).Max(y => Utilities.GetLaneWidth(y, new LaneInfo()));

			CustomWidth = (float)Math.Max(0.1, Math.Round((float)CustomWidth +
				-(ModifierKeys.HasFlag(Keys.Shift) ? 5F : ModifierKeys.HasFlag(Keys.Control) ? 1F : ModifierKeys.HasFlag(Keys.Alt) ? 0.01F : 0.1F), 2));

			RefreshRoad();
		}

		private void ElevationResetClick(object sender, MouseEventArgs e)
		{
			Elevation = null;

			RefreshRoad();
		}

		private void ElevationPlusClick(object sender, MouseEventArgs e)
		{
			if (Options.Current.AdvancedElevation)
			{
				if (Elevation == null)
					Elevation = -0.3F;

				Elevation = (float)Math.Max(-0.3, Math.Round((float)Elevation +
					(ModifierKeys.HasFlag(Keys.Shift) ? 5F : ModifierKeys.HasFlag(Keys.Control) ? 1F : ModifierKeys.HasFlag(Keys.Alt) ? 0.01F : 0.1F), 2));
			}
			else
			{
				var defaultElevation = GetDefaultElevation();

				if (defaultElevation == 0)
					Elevation = 0.2F;
				else
					Elevation = 0;
			}

			RefreshRoad();
		}

		private void ElevationMinusClick(object sender, MouseEventArgs e)
		{
			if (Options.Current.AdvancedElevation)
			{
				if (Elevation == null)
					Elevation = -0.3F;

				Elevation = (float)Math.Max(-0.3, Math.Round((float)Elevation +
					-(ModifierKeys.HasFlag(Keys.Shift) ? 5F : ModifierKeys.HasFlag(Keys.Control) ? 1F : ModifierKeys.HasFlag(Keys.Alt) ? 0.01F : 0.1F), 2));
			}
			else
			{
				Elevation = null;
			}

			RefreshRoad();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			foreach (var item in _clickActions)
			{
				if (item.Key.Contains(e.Location))
				{
					Cursor = Cursors.Hand;
					return;
				}
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
				CustomWidth = CustomWidth,
				Elevation = Elevation,
				SpeedLimit = SpeedLimit,
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
				CustomWidth = CustomWidth,
				Elevation = Elevation,
				SpeedLimit = SpeedLimit,
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

			RefreshRoad();
		}

		internal void SetDecorations(LaneDecoration laneDecorationStyle)
		{
			Decorations = laneDecorationStyle;

			RefreshRoad();
		}
	}
}
