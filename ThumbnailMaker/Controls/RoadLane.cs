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

		public RoadLane(ThumbnailLaneInfo lane = null)
		{
			AllowDrop = true;
			Dock = DockStyle.Top;
			Lane = lane ?? new ThumbnailLaneInfo();
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			Height = (int)(35 * UI.UIScale) + 7;
			scale = (int)(30 * UI.UIScale);
		}

		private Rectangle grabberRectangle;
		private readonly Dictionary<Rectangle, MouseEventHandler> _clickActions = new Dictionary<Rectangle, MouseEventHandler>();

		private int yIndex => (Height - scale - 7) / 2;
		private Color foreColor => _dragDropActive ? Color.FromArgb(200, Lane.Color.GetAccentColor()) : FormDesign.Design.ForeColor;

		public ThumbnailLaneInfo Lane { get; }
		public static float GlobalSpeed { get; set; }

		protected override void OnPaint(PaintEventArgs e)
		{
			var cursor = PointToClient(Cursor.Position);

			e.Graphics.Clear(BackColor);

			_clickActions.Clear();

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (_dragDropActive)
				e.Graphics.FillRoundedRectangle(new SolidBrush(Lane.Type== LaneType.Empty ? FormDesign.Design.ActiveColor : Lane.Color), ClientRectangle.Pad(0, 0, 1, 7), 6);
			else
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(0, 0, 1, 7), 6);

			var iconX = DrawIcon(e, cursor);

			if (Lane.Type!= LaneType.Empty)
				DrawDecoIcon(e, cursor, ref iconX);
					
			var leftX = DrawDeleteOrInfoIcon(e, cursor);

			DrawCopyButton(e, cursor, ref leftX);

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
			if (!Lane.Type.HasAnyFlag(LaneType.Bike, LaneType.Car, LaneType.Bus, LaneType.Tram, LaneType.Trolley, LaneType.Emergency))
				return;

			leftX -= 6 + scale;

			var speedRectangle = new Rectangle(leftX, yIndex, scale, scale);

			if (speedRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), speedRectangle, 4);

			ThumbnailHandler.DrawSpeedSignSmall(e.Graphics, Options.Current.Region, (int)(Lane.SpeedLimit ?? GlobalSpeed), speedRectangle);

			_clickActions[speedRectangle] = SpeedLimitClick;
		}

		private void DrawLaneDirections(PaintEventArgs e, Point cursor, ref int leftX)
		{
			var directions = Lane.Type== LaneType.Parking ? new[] { LaneDirection.None, LaneDirection.Backwards, LaneDirection.Forward } : Lane.Type<= LaneType.Curb ? new LaneDirection[0] : new[] { LaneDirection.None, LaneDirection.Backwards, LaneDirection.Forward, LaneDirection.Both };

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
				else if (Lane.Direction == direction)
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

				e.Graphics.DrawImage(icon.Color(rect.Contains(cursor) ? FormDesign.Design.ActiveForeColor : Lane.Direction == direction ? FormDesign.Design.ActiveColor : foreColor), rect.CenterR(16, 16));

				_clickActions[rect] = (_, __) =>
				{
					Lane.Direction = direction;
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

			DrawFocus(e.Graphics, Lane.Elevation == null ? elevationMinusRectangle : elevationPlusRectangle, HoverState.Focused, 4);

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

			var size = Lane.Elevation ?? GetDefaultElevation();
			e.Graphics.DrawString($"{size:0.##}m", new Font(UI.FontFamily, 8.25F), new SolidBrush(elevationRectangle.Contains(cursor) ? FormDesign.Design.RedColor : foreColor), elevationRectangle, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });

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

			e.Graphics.DrawString($"{Lane.LaneWidth:0.##}m", new Font(UI.FontFamily, 8.25F), new SolidBrush(sizeRectangle.Contains(cursor) ? FormDesign.Design.RedColor : foreColor), sizeRectangle, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			
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

		private void DrawCopyButton(PaintEventArgs e, Point cursor, ref int leftX)
		{
			if (this.Lane.Type== LaneType.Curb)
				return;

			var foreColor = _dragDropActive ? Color.FromArgb(150, Lane.Color.GetAccentColor()) : FormDesign.Design.ForeColor;

			var copyRectangle = new Rectangle(leftX + scale * 2, yIndex, scale, scale);

			if (copyRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), copyRectangle, 4);

			e.Graphics.DrawImage(Properties.Resources.I_Copy.Color(copyRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), copyRectangle.CenterR(16, 16));

			DrawLine(e, leftX - 3);

			_clickActions[copyRectangle] = DuplicateLaneClick;
		}

		private int DrawDeleteOrInfoIcon(PaintEventArgs e, Point cursor)
		{
			var foreColor = _dragDropActive ? Color.FromArgb(150, Lane.Color.GetAccentColor()) : FormDesign.Design.ForeColor;

			var deleteRectangle = new Rectangle(Width - scale, yIndex, scale, scale);

			e.Graphics.DrawImage((this.Lane.Type!= LaneType.Curb ? Properties.Resources.I_X : Properties.Resources.I_Info).Color(deleteRectangle.Contains(cursor) ? (this.Lane.Type!= LaneType.Curb ? FormDesign.Design.RedColor : FormDesign.Design.ActiveColor) : foreColor), deleteRectangle.CenterR(16, 16));

			if (this.Lane.Type!= LaneType.Curb && deleteRectangle.Contains(cursor))
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(25, FormDesign.Design.RedColor)), ClientRectangle.Pad(0, 0, 1, 7), 6);
			else if (ClientRectangle.Pad(0, 0, 1, 7).Contains(cursor) && base.HoverState.HasFlag(HoverState.Hovered))
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(25, Lane.Color)), ClientRectangle.Pad(0, 0, 1, 7), 6);

			DrawLine(e, Width - scale - 2);

			if (this.Lane.Type!= LaneType.Curb)
				_clickActions[deleteRectangle] = DeleteLaneClick;
			else
				_clickActions[deleteRectangle] = InfoLaneClick;

			return deleteRectangle.X;
		}

		private int DrawIcon(PaintEventArgs e, Point cursor)
		{
			var laneColor = Lane.Color;
			var icons = Lane.Icons(UI.FontScale <= 1.25, true);

			var iconRectangle = new Rectangle(3, (Height - scale - 7) / 2, Math.Max(scale, icons.Count * scale), scale);

			if (this.Lane.Type== LaneType.Empty)
				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 1.5F), iconRectangle, 6);
			else
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(iconRectangle.Contains(cursor) && this.Lane.Type!= LaneType.Curb ? 50 : 100, laneColor)), iconRectangle, 6);

			var iconX = 3;
			var color = laneColor.GetAccentColor();
			foreach (var icon in icons.Select(x => x.Value))
			{
				if (Lane.Type== LaneType.Curb && Lane.Direction == LaneDirection.Backwards)
					icon.RotateFlip(RotateFlipType.RotateNoneFlipX);

				using (icon)
					e.Graphics.DrawIcon(icon, new Rectangle(iconX, (Height - scale - 7) / 2, scale, scale), UI.FontScale <= 1.25 ? (Size?)null : new Size(scale * 3 / 4, scale * 3 / 4));

				iconX += scale;
			}

			if (iconRectangle.Contains(cursor) && this.Lane.Type!= LaneType.Curb)
				DrawFocus(e.Graphics, iconRectangle.Pad(-1), HoverState.Focused, 6, this.Lane.Type== LaneType.Empty ? (Color?)null : laneColor);

			if (this.Lane.Type!= LaneType.Curb)
				_clickActions[iconRectangle] = LaneTypeClick;

			return iconRectangle.Width + 12;
		}

		private void DrawDecoIcon(PaintEventArgs e, Point cursor, ref int iconX)
		{
			var icons = Lane.Decorations.GetValues().Select(x => ResourceManager.GetImage(x, UI.FontScale <= 1.25)).ToList();
			var laneColor = ThumbnailLaneInfo.GetColor(Lane.Decorations);

			iconX += 3;

			var decoRectangle = new Rectangle(iconX, (Height - scale - 7) / 2, Math.Max(scale, icons.Count * scale), scale);

			if (Lane.Decorations == LaneDecoration.None)
				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 1.5F), decoRectangle, 6);
			else
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(decoRectangle.Contains(cursor) ? 50 : 100, laneColor)), decoRectangle, 6);

			var color = laneColor.GetAccentColor();
			foreach (var icon in icons)
			{
				if (Lane.Decorations == LaneDecoration.None)
					icon.Color(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.AccentColor));

				using (icon)
					e.Graphics.DrawIcon(icon, new Rectangle(iconX, (Height - scale - 7) / 2, scale, scale), UI.FontScale <= 1.25 ? (Size?)null : new Size(scale * 3 / 4, scale * 3 / 4));

				iconX += scale;
			}

			if (decoRectangle.Contains(cursor))
				DrawFocus(e.Graphics, decoRectangle.Pad(-1), HoverState.Focused, 6, Lane.Decorations == LaneDecoration.None ? (Color?)null : laneColor);

			iconX = decoRectangle.X + decoRectangle.Width + 12;

			_clickActions[decoRectangle] = LaneDecoClick;
		}

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
			var foreColor = _dragDropActive ? Color.FromArgb(150, Lane.Color.GetAccentColor()) : FormDesign.Design.AccentColor;

			e.Graphics.DrawLine(new Pen(foreColor), x, 6, x, Height - 13);
		}

		private float GetDefaultElevation()
		{
			var leftSidewalk = Parent.Controls.OfType<RoadLane>().FirstOrDefault(x => x.Lane.Type== LaneType.Curb && x.Lane.Direction == LaneDirection.Backwards);
			var rightSidewalk = Parent.Controls.OfType<RoadLane>().LastOrDefault(x => x.Lane.Type== LaneType.Curb && x.Lane.Direction == LaneDirection.Forward);

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
			MessagePrompt.Show($"This is the {(Lane.Direction == LaneDirection.Forward ? "Right" : "Left")} Curb Delimiter;\r\n\r\nIt is used to separate what lanes are on the {(Lane.Direction == LaneDirection.Forward ? "right" : "left")} sidewalk and what lanes are on the asphalt.\r\n\r\nTry moving lanes and see how it affects the road in the preview.", PromptButtons.OK, PromptIcons.Info);
		}

		private void WidthResetClick(object sender, MouseEventArgs e)
		{
			Lane.CustomWidth = null;

			RefreshRoad();
		}

		private void WidthPlusClick(object sender, MouseEventArgs e)
		{
			var change = ModifierKeys.HasFlag(Keys.Shift) ? 5F : ModifierKeys.HasFlag(Keys.Control) ? 1F : ModifierKeys.HasFlag(Keys.Alt) ? 0.01F : 0.1F;

			Lane.CustomWidth = (float)Math.Max(0.1, Math.Round(Lane.LaneWidth + change, 2));

			RefreshRoad();
		}

		private void WidthMinusClick(object sender, MouseEventArgs e)
		{
			var change = ModifierKeys.HasFlag(Keys.Shift) ? 5F : ModifierKeys.HasFlag(Keys.Control) ? 1F : ModifierKeys.HasFlag(Keys.Alt) ? 0.01F : 0.1F;

			Lane.CustomWidth = (float)Math.Max(0.1, Math.Round(Lane.LaneWidth - change, 2));

			RefreshRoad();
		}

		private void ElevationResetClick(object sender, MouseEventArgs e)
		{
			Lane.Elevation = null;

			RefreshRoad();
		}

		private void ElevationPlusClick(object sender, MouseEventArgs e)
		{
			if (Options.Current.AdvancedElevation)
			{
				var change = ModifierKeys.HasFlag(Keys.Shift) ? 5F : ModifierKeys.HasFlag(Keys.Control) ? 1F : ModifierKeys.HasFlag(Keys.Alt) ? 0.01F : 0.1F;
				var defaultElevation = GetDefaultElevation();

				Lane.Elevation = (float)Math.Max(defaultElevation, Math.Round(Lane.Elevation ?? defaultElevation + change, 2));
			}
			else
			{
				var defaultElevation = GetDefaultElevation();

				if (defaultElevation == 0)
					Lane.Elevation = 0.2F;
				else
					Lane.Elevation = 0;
			}

			RefreshRoad();
		}

		private void ElevationMinusClick(object sender, MouseEventArgs e)
		{
			if (Options.Current.AdvancedElevation)
			{
				var change = ModifierKeys.HasFlag(Keys.Shift) ? 5F : ModifierKeys.HasFlag(Keys.Control) ? 1F : ModifierKeys.HasFlag(Keys.Alt) ? 0.01F : 0.1F;
				var defaultElevation = GetDefaultElevation();
				
				Lane.Elevation = (float)Math.Max(defaultElevation, Math.Round(Lane.Elevation ?? defaultElevation - change, 2));
			}
			else
			{
				Lane.Elevation = null;
			}

			RefreshRoad();
		}

		private void DuplicateLaneClick(object sender, MouseEventArgs e)
		{
			throw new NotImplementedException();
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

		public RoadLane Duplicate() => new RoadLane(Lane);

		internal void RefreshRoad()
		{
			Invalidate();

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);
		}

		internal void ApplyLaneType()
		{
			if (Lane.Type < LaneType.Bike)
			{
				Lane.Direction = LaneDirection.None;
			}

			if (Lane.Type.HasFlag(LaneType.Parking))
			{
				Lane.Type = LaneType.Parking;
			}

			if (Lane.Type.HasFlag(LaneType.Filler))
			{
				Lane.Type = LaneType.Filler;
			}

			if (Lane.Type == LaneType.Parking)
			{
				Lane.ParkingAngle = Parent.Controls.OfType<RoadLane>().FirstOrDefault(x => x != this && x.Lane.Type == LaneType.Parking)?.Lane.ParkingAngle ?? ParkingAngle.Vertical;
			}
			else
			{
				Lane.ParkingAngle = ParkingAngle.Vertical;
			}

			if (Lane.Decorations == LaneDecoration.None && (Lane.Type == LaneType.Bike || Lane.Type == LaneType.Bus || Lane.Type == LaneType.Trolley))
			{
				Lane.Decorations = LaneDecoration.Filler;
			}

			foreach (var decoration in Lane.Decorations.GetValues())
			{
				if (!decoration.IsCompatible(Lane.Type))
				{
					Lane.Decorations &= ~decoration;
				}
			}

			RefreshRoad();
		}
	}
}
