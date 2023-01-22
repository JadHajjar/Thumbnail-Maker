using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Controls
{
	public partial class RoadLane : SlickControl
	{
		private readonly Dictionary<Rectangle, string> _tooltips = new Dictionary<Rectangle, string>();
		private readonly Dictionary<Rectangle, MouseEventHandler> _clickActions = new Dictionary<Rectangle, MouseEventHandler>();
		private bool _dragDropActive;
		private Rectangle grabberRectangle;
		private int scale;

		public RoadLane(ThumbnailLaneInfo lane = null)
		{
			AllowDrop = true;
			Dock = DockStyle.Top;
			Lane = lane ?? new ThumbnailLaneInfo();
		}

		public event EventHandler RoadLaneChanged;

		public static float GlobalSpeed { get; set; }

		public static RoadType RoadType { get; set; }

		public ThumbnailLaneInfo Lane { get; }

		private Color foreColor => _dragDropActive ? Color.FromArgb(200, Lane.Color.GetAccentColor()) : FormDesign.Design.ForeColor;

		private int yIndex => (Height - scale - 7) / 2;

		public static void HandleDragAction(DragEventArgs drgevent, bool drop)
		{
			if (!(drgevent.Data.GetData(typeof(RoadLane)) is RoadLane item))
			{
				return;
			}

			if (drop)
			{
				item.HoverState = HoverState.Normal;
				item._dragDropActive = false;
				item.Invalidate();

				if (item.Lane.Type == LaneType.Curb)
					item.FixCurbOrientation();
			}

			drgevent.Effect = DragDropEffects.Move;

			var yIndex = item.Parent.PointToClient(Cursor.Position).Y + ((ScrollableControl)item.Parent).VerticalScroll.Value;
			var calculatedIndex = item.Parent.Controls.Count - 1 - (int)Math.Floor((double)yIndex / item.Height).Between(0, item.Parent.Controls.Count - 1);

			item.Parent.Controls.SetChildIndex(item, calculatedIndex);
			item.RoadLaneChanged?.Invoke(item, EventArgs.Empty);
		}

		public void FixCurbOrientation()
		{
			var other = Parent.Controls.Where(x => (x as RoadLane).Lane.Type == LaneType.Curb && x != this).FirstOrDefault() as RoadLane;
			var index = Parent.Controls.IndexOf(other);

			if (index < Parent.Controls.IndexOf(this))
			{
				other.Lane.Direction = LaneDirection.Forward;
				Lane.Direction = LaneDirection.Backwards;
			}
			else
			{
				Lane.Direction = LaneDirection.Forward;
				other.Lane.Direction = LaneDirection.Backwards;
			}

			other.RefreshRoad();
		}

		public RoadLane Duplicate()
		{
			return new RoadLane(new ThumbnailLaneInfo(Lane));
		}

		internal void ApplyLaneType()
		{
			if (Lane.Type < LaneType.Bike)
			{
				Lane.Direction = LaneDirection.Both;
			}

			if (Lane.Type.HasFlag(LaneType.Parking))
			{
				Lane.Type = LaneType.Parking;
			}

			if (Lane.Type.HasFlag(LaneType.Filler))
			{
				Lane.Type = LaneType.Filler;
			}

			Lane.ParkingAngle = Lane.Type == LaneType.Parking
				? Parent.Controls.OfType<RoadLane>().FirstOrDefault(x => x != this && x.Lane.Type == LaneType.Parking)?.Lane.ParkingAngle ?? ParkingAngle.Vertical
				: ParkingAngle.Vertical;

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

			if (!Options.Current.AdvancedElevation)
			{
				Lane.Elevation = null;
			}

			RefreshRoad();
		}

		internal void RefreshRoad()
		{
			Invalidate();

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);
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

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (grabberRectangle.Contains(e.Location))
			{
				GrabberClick(this, e);

				return;
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			foreach (var item in _tooltips)
			{
				if (item.Key.Contains(e.Location))
				{
					SlickTip.SetTo(this, item.Value);

					break;
				}
			}

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

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (_dragDropActive)
			{
				return;
			}

			foreach (var item in _clickActions)
			{
				if (item.Key.Contains(e.Location))
				{
					item.Value(this, e);

					return;
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var cursor = HoverState.HasFlag(HoverState.Hovered) ? PointToClient(Cursor.Position) : new Point(-1, -1);

			e.Graphics.Clear(BackColor);

			_clickActions.Clear();
			_tooltips.Clear();

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			if (_dragDropActive)
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Lane.Type == LaneType.Empty ? FormDesign.Design.ActiveColor : Lane.Color), ClientRectangle.Pad(0, 0, 1, 7), 6);
			}
			else
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(0, 0, 1, 7), 6);
			}

			var iconX = DrawIcon(e, cursor);

			if (Lane.Type != LaneType.Empty)
			{
				DrawDecoIcon(e, cursor, ref iconX);
			}

			var leftX = DrawDeleteOrInfoIcon(e, cursor);

			DrawCopyButton(e, cursor, ref leftX);

			DrawLaneWidth(e, cursor, ref leftX);

			if (Options.Current.AdvancedElevation)
			{
				DrawLaneAdvancedElevation(e, cursor, ref leftX);
			}
			else
			{
				DrawLaneElevation(e, cursor, ref leftX);
			}

			DrawLaneDirections(e, cursor, ref leftX);

			DrawParkingDirections(e, cursor, ref leftX);

			DrawLaneSpeed(e, cursor, ref leftX);

			DrawDecorationDirection(e, cursor, ref leftX);

			DrawFillerPadding(e, cursor, ref leftX);

			grabberRectangle = new Rectangle(iconX + 8, 0, leftX - iconX, Height - 4);

			var drawGrabberRect = new Rectangle(scale * 4, 0, Width - ((scale + 2) * 19) - 18, Height - 4);

			e.Graphics.DrawImage(Properties.Resources.I_Grabber.Color(_dragDropActive || grabberRectangle.Contains(cursor) ? FormDesign.Design.ActiveColor : foreColor), drawGrabberRect.CenterR(10, 5));

			_tooltips[grabberRectangle] = $"Hold to drag this lane up or down";
			_tooltips[ClientRectangle] = Lane.Type != LaneType.Curb ? $"{Lane.Type.ToString().FormatWords()} Lane\r\nMiddle-click to clear it" : Lane.Direction == LaneDirection.Forward ? "Right Curb Delimiter" : "Left Curb Delimiter";
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			Height = (int)(35 * UI.UIScale) + 7;
			scale = (int)(30 * UI.UIScale);
		}

		private void DeleteLaneClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			Dispose();

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);
		}

		private void DrawCopyButton(PaintEventArgs e, Point cursor, ref int leftX)
		{
			if (Lane.Type == LaneType.Curb)
			{
				return;
			}

			leftX -= 12 + scale;

			var foreColor = _dragDropActive ? Color.FromArgb(150, Lane.Color.GetAccentColor()) : FormDesign.Design.ForeColor;

			var copyRectangle = new Rectangle(leftX, yIndex, scale, scale);

			if (copyRectangle.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), copyRectangle, 4);
			}

			e.Graphics.DrawImage(Properties.Resources.I_Copy.Color(copyRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), copyRectangle.CenterR(16, 16));

			DrawLine(e, leftX - 6);

			_tooltips[copyRectangle] = "Duplicate this lane, right-click to also flip its direction";
			_clickActions[copyRectangle] = DuplicateLaneClick;
		}

		private void DrawDecoIcon(PaintEventArgs e, Point cursor, ref int iconX)
		{
			var icons = Lane.Decorations.GetValues().Select(x => ResourceManager.GetImage(x, UI.FontScale <= 1.25)).ToList();
			var laneColor = ThumbnailLaneInfo.GetColor(Lane.Decorations);

			iconX += 6;

			var decoRectangle = new Rectangle(iconX, (Height - scale - 7) / 2, Math.Max(scale, icons.Count * scale), scale);

			if (Lane.Decorations == LaneDecoration.None)
			{
				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 1.5F), decoRectangle, 6);
			}
			else
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(decoRectangle.Contains(cursor) ? 50 : 100, laneColor)), decoRectangle, 6);
			}

			var color = laneColor.GetAccentColor();
			foreach (var icon in icons)
			{
				if (Lane.Decorations == LaneDecoration.None)
				{
					icon.Color(FormDesign.Design.ForeColor.MergeColor(FormDesign.Design.AccentColor));
				}

				using (icon)
				{
					e.Graphics.DrawIcon(icon, new Rectangle(iconX, (Height - scale - 7) / 2, scale, scale).Pad(2), UI.FontScale <= 1.25 ? (Size?)null : new Size(scale * 3 / 4, scale * 3 / 4));
				}

				iconX += scale;
			}

			if (decoRectangle.Contains(cursor))
			{
				DrawFocus(e.Graphics, decoRectangle.Pad(-1), HoverState.Focused, 6, Lane.Decorations == LaneDecoration.None ? (Color?)null : laneColor);
			}

			iconX = decoRectangle.X + decoRectangle.Width + 12;

			_clickActions[decoRectangle] = LaneDecoClick;
			_tooltips[decoRectangle] = Lane.Decorations == LaneDecoration.None ? "No Add-ons" : (Lane.Decorations.ToString().FormatWords() + "\r\nMiddle-click to clear it");
		}

		private int DrawDeleteOrInfoIcon(PaintEventArgs e, Point cursor)
		{
			var foreColor = _dragDropActive ? Color.FromArgb(150, Lane.Color.GetAccentColor()) : FormDesign.Design.ForeColor;

			var deleteRectangle = new Rectangle(Width - (Lane.Type != LaneType.Curb ? scale : ((2 * scale) + 12)), yIndex, Lane.Type != LaneType.Curb ? scale : ((2 * scale) + 12), scale);

			e.Graphics.DrawImage((Lane.Type != LaneType.Curb ? Properties.Resources.I_X : Properties.Resources.I_Info).Color(deleteRectangle.Contains(cursor) ? (Lane.Type != LaneType.Curb ? FormDesign.Design.RedColor : FormDesign.Design.ActiveColor) : foreColor), deleteRectangle.CenterR(16, 16));

			if (Lane.Type != LaneType.Curb && deleteRectangle.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(25, FormDesign.Design.RedColor)), ClientRectangle.Pad(0, 0, 1, 7), 6);
			}
			else if (ClientRectangle.Pad(0, 0, 1, 7).Contains(cursor) && base.HoverState.HasFlag(HoverState.Hovered))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(25, Lane.Color)), ClientRectangle.Pad(0, 0, 1, 7), 6);
			}

			if (Lane.Type != LaneType.Curb)
				_clickActions[deleteRectangle] = DeleteLaneClick;
			else
				_clickActions[deleteRectangle] = InfoLaneClick;

			_tooltips[deleteRectangle] = Lane.Type != LaneType.Curb ? "Delete Lane" : "More Info";

			DrawLine(e, deleteRectangle.X - 6);

			return deleteRectangle.X;
		}

		private int DrawIcon(PaintEventArgs e, Point cursor)
		{
			var laneColor = Lane.Color;
			var icons = Lane.Type.GetValues()
				.ToDictionary(x => x, x => ResourceManager.GetImage(x, UI.FontScale <= 1.25))
				.Where(x => x.Value != null)
				.ToList();

			var iconRectangle = new Rectangle(3, (Height - scale - 7) / 2, Math.Max(scale, icons.Count * scale), scale);

			if (Lane.Type == LaneType.Empty)
			{
				e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentColor, 1.5F), iconRectangle, 6);
			}
			else if (Lane.Type == LaneType.Curb)
			{
				if (!_dragDropActive)
				{
					using (var brush = new LinearGradientBrush(ClientRectangle.Pad(-1, -1, 0, 6), Lane.Color, Color.FromArgb(0, Lane.Color), Lane.Direction == LaneDirection.Forward ? 90 : -90))
					using (var pen = new Pen(brush, 2F))
						e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad(0, 0, 1, 7), 6);
				}
			}
			else
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(iconRectangle.Contains(cursor) ? 50 : 100, laneColor)), iconRectangle, 6);
			}

			var iconX = 3;
			var color = laneColor.GetAccentColor();
			foreach (var icon in icons.Select(x => x.Value))
			{
				if (Lane.Type == LaneType.Curb && Lane.Direction == LaneDirection.Backwards)
				{
					icon.RotateFlip(RotateFlipType.RotateNoneFlipX);
				}

				using (icon)
				{
					e.Graphics.DrawIcon(icon, new Rectangle(iconX, (Height - scale - 7) / 2, scale, scale), UI.FontScale <= 1.25 ? (Size?)null : new Size(scale * 3 / 4, scale * 3 / 4));
				}

				iconX += scale;
			}

			if (iconRectangle.Contains(cursor) && Lane.Type != LaneType.Curb)
			{
				DrawFocus(e.Graphics, iconRectangle.Pad(-1), HoverState.Focused, 6, Lane.Type == LaneType.Empty ? (Color?)null : laneColor);
			}

			if (Lane.Type != LaneType.Curb)
			{
				_clickActions[iconRectangle] = LaneTypeClick;
			}

			return iconRectangle.Width + 12;
		}

		private void DrawLaneAdvancedElevation(PaintEventArgs e, Point cursor, ref int leftX)
		{
			leftX -= 12 + (scale * 3);

			var elevationMinusRectangle = new Rectangle(leftX, yIndex, scale, scale);
			var elevationRectangle = new Rectangle(leftX + scale, yIndex, scale, scale);
			var elevationPlusRectangle = new Rectangle(leftX + (scale * 2), yIndex, scale, scale);

			var size = Lane.Elevation ?? GetDefaultElevation(true);
			e.Graphics.DrawString($"{size:0.##}m", new Font(UI.FontFamily, 8.25F), new SolidBrush(elevationRectangle.Contains(cursor) ? FormDesign.Design.RedColor : Lane.Elevation != null ? FormDesign.Design.ActiveColor : foreColor), elevationRectangle.Pad(-10), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });

			if (elevationMinusRectangle.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), elevationMinusRectangle, 4);
			}

			e.Graphics.DrawImage(Properties.Resources.I_Down.Color(elevationMinusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), elevationMinusRectangle.CenterR(16, 16));

			if (elevationPlusRectangle.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), elevationPlusRectangle, 4);
			}

			e.Graphics.DrawImage(Properties.Resources.I_Up.Color(elevationPlusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), elevationPlusRectangle.CenterR(16, 16));

			DrawLine(e, leftX - 6);

			_clickActions[elevationMinusRectangle] = ElevationMinusClick;
			_clickActions[elevationPlusRectangle] = ElevationPlusClick;
			_clickActions[elevationRectangle] = ElevationResetClick;

			_tooltips[elevationMinusRectangle] = "Decrease lane elevation by 0.1\r\n\r\nUse Shift for 1x, Ctrl for 0.25x & Alt for 0.01x";
			_tooltips[elevationPlusRectangle] = "Increase lane elevation by 0.1\r\n\r\nUse Shift for 1x, Ctrl for 0.25x & Alt for 0.01x";
			_tooltips[elevationRectangle] = "Reset lane elevation to " + GetDefaultElevation(true);
		}

		private void DrawLaneDirections(PaintEventArgs e, Point cursor, ref int leftX)
		{
			if (Lane.Type < LaneType.Bike)
			{
				return;
			}

			var directions = new[] { LaneDirection.Both, LaneDirection.Backwards, LaneDirection.Forward };

			leftX -= 12 + (scale * directions.Length);

			// Draw direction buttons
			var ind = 0;
			foreach (var direction in directions)
			{
				var rect = new Rectangle(leftX + (scale * ind++), yIndex, scale, scale);

				if (rect.Contains(cursor))
				{
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), rect, 4);
				}
				else if (Lane.Direction == direction)
				{
					DrawSelection(e, rect);
				}

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

				_tooltips[rect] = "Set this lane's direction to " + direction;
				_clickActions[rect] = (_, __) =>
				{
					Lane.Direction = direction;
					RefreshRoad();
				};
			}

			DrawLine(e, leftX - 6);
		}

		private void DrawDecorationDirection(PaintEventArgs e, Point cursor, ref int leftX)
		{
			if (Lane.Type != LaneType.Filler && Lane.Type != LaneType.Curb)
			{
				return;
			}

			var directions = new[] { PropAngle.Left, PropAngle.Right };

			leftX -= 12 + scale;

			// Draw direction buttons
			var rect = new Rectangle(leftX, yIndex, scale, scale);

			if (rect.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), rect, 4);
			}
			else if (Lane.PropAngle == PropAngle.Left)
			{
				DrawSelection(e, rect);
			}

			e.Graphics.DrawImage(Properties.Resources.Icon_Flip.Color(rect.Contains(cursor) ? FormDesign.Design.ActiveForeColor : Lane.PropAngle == PropAngle.Left ? FormDesign.Design.ActiveColor : foreColor), rect.CenterR(16, 16));

			_tooltips[rect] = "Flips the direction some decorations are facing";
			_clickActions[rect] = (_, __) =>
			{
				Lane.PropAngle = Lane.PropAngle == PropAngle.Left ? PropAngle.Right : PropAngle.Left;
				RefreshRoad();
			};

			DrawLine(e, leftX - 6);
		}

		private void DrawFillerPadding(PaintEventArgs e, Point cursor, ref int leftX)
		{
			if (!Lane.Decorations.HasAnyFlag(LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement))
			{
				Lane.FillerPadding = FillerPadding.Unset;
				return;
			}

			var directions = new[] { FillerPadding.Left, FillerPadding.Right };

			leftX -= 12 + (scale * directions.Length);

			// Draw direction buttons
			var ind = 0;
			foreach (var direction in directions)
			{
				var rect = new Rectangle(leftX + (scale * ind++), yIndex, scale, scale);

				if (rect.Contains(cursor))
				{
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), rect, 4);
				}
				else if (Lane.FillerPadding.HasFlag(direction))
				{
					DrawSelection(e, rect);
				}

				Bitmap icon;

				switch (direction)
				{
					case FillerPadding.Right:
						icon = Properties.Resources.Icon_Curb;
						icon.RotateFlip(RotateFlipType.RotateNoneFlipX);
						break;

					case FillerPadding.Left:
						icon = Properties.Resources.Icon_Curb;
						break;

					default:
						continue;
				}

				e.Graphics.DrawImage(icon.Color(rect.Contains(cursor) ? FormDesign.Design.ActiveForeColor : Lane.FillerPadding.HasFlag(direction) ? FormDesign.Design.ActiveColor : foreColor), rect.CenterR(16, 16));

				_tooltips[rect] = $"{(Lane.FillerPadding.HasFlag(direction) ? "Reset" : "Remove")} padding on the {direction} of the {Lane.Decorations & (LaneDecoration.Grass | LaneDecoration.Gravel | LaneDecoration.Pavement)} filler";
				_clickActions[rect] = (_, __) =>
				{
					if (Lane.FillerPadding.HasFlag(direction))
						Lane.FillerPadding &= ~direction;
					else
						Lane.FillerPadding |= direction;

					RefreshRoad();
				};
			}


			DrawLine(e, leftX - 6);
		}

		private void DrawParkingDirections(PaintEventArgs e, Point cursor, ref int leftX)
		{
			if (Lane.Type != LaneType.Parking)
			{
				return;
			}

			var directions = new[] { ParkingAngle.Vertical, ParkingAngle.Horizontal, ParkingAngle.InvertedDiagonal, ParkingAngle.Diagonal };

			leftX -= 12 + (scale * directions.Length);

			// Draw direction buttons
			var ind = 0;
			foreach (var direction in directions)
			{
				var rect = new Rectangle(leftX + (scale * ind++), yIndex, scale, scale);

				if (rect.Contains(cursor))
				{
					e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), rect, 4);
				}
				else if (Lane.ParkingAngle == direction)
				{
					DrawSelection(e, rect);
				}

				Bitmap icon;

				switch (direction)
				{
					case ParkingAngle.Horizontal:
						icon = Properties.Resources.Icon_Horizontal;
						break;
					case ParkingAngle.Diagonal:
						icon = Properties.Resources.I_Diagonal;
						break;
					case ParkingAngle.InvertedDiagonal:
						icon = Properties.Resources.Icon_IDiagonal;
						break;
					default:
						icon = Properties.Resources.I_Vertical;
						break;
				}

				e.Graphics.DrawImage(icon.Color(rect.Contains(cursor) ? FormDesign.Design.ActiveForeColor : Lane.ParkingAngle == direction ? FormDesign.Design.ActiveColor : foreColor), rect.CenterR(16, 16));

				_tooltips[rect] = "Set the parking angle to " + direction.ToString().FormatWords();
				_clickActions[rect] = (_, __) =>
				{
					foreach (var rl in Parent.Controls.OfType<RoadLane>().Where(x => x.Lane.Type == LaneType.Parking))
					{
						rl.Lane.ParkingAngle = direction;
						rl.Invalidate();
					}

					RefreshRoad();
				};
			}

			DrawLine(e, leftX - 6);
		}

		private void DrawLaneElevation(PaintEventArgs e, Point cursor, ref int leftX)
		{
			leftX -= 12 + (scale * 2);

			var elevationMinusRectangle = new Rectangle(leftX, yIndex, scale, scale);
			var elevationPlusRectangle = new Rectangle(leftX + scale, yIndex, scale, scale);
			var isLow = Lane.Decorations.HasAnyFlag(LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement) ? Lane.Elevation != null : Lane.Elevation == null;

			if (elevationMinusRectangle.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), elevationMinusRectangle, 4);
			}

			e.Graphics.DrawImage(Properties.Resources.I_Down.Color(elevationMinusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : isLow ? FormDesign.Design.ActiveColor : foreColor), elevationMinusRectangle.CenterR(16, 16));

			if (elevationPlusRectangle.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), elevationPlusRectangle, 4);
			}

			e.Graphics.DrawImage(Properties.Resources.I_Up.Color(elevationPlusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : !isLow ? FormDesign.Design.ActiveColor : foreColor), elevationPlusRectangle.CenterR(16, 16));

			DrawSelection(e, isLow ? elevationMinusRectangle : elevationPlusRectangle);

			DrawLine(e, leftX - 6);

			_clickActions[elevationMinusRectangle] = ElevationMinusClick;
			_clickActions[elevationPlusRectangle] = ElevationPlusClick;

			_tooltips[elevationMinusRectangle] = "Set this lane's elevation to a lowered value\r\n\r\nYou can change this behavior in the options";
			_tooltips[elevationPlusRectangle] = "Set this lane's elevation to a higher value\r\n\r\nYou can change this behavior in the options";
		}

		private void DrawLaneSpeed(PaintEventArgs e, Point cursor, ref int leftX)
		{
			if (!Lane.Type.HasAnyFlag(LaneType.Bike, LaneType.Car, LaneType.Bus, LaneType.Tram, LaneType.Trolley, LaneType.Emergency))
			{
				return;
			}

			leftX -= 12 + scale;

			var speedRectangle = new Rectangle(leftX, yIndex, scale, scale);

			if (speedRectangle.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), speedRectangle, 4);
			}

			ThumbnailHandler.DrawSpeedSignSmall(e.Graphics, Options.Current.Region, (int)(Lane.SpeedLimit ?? GlobalSpeed), speedRectangle);

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			e.Graphics.InterpolationMode = InterpolationMode.Bilinear;

			DrawLine(e, leftX - 6);

			_clickActions[speedRectangle] = SpeedLimitClick;
			_tooltips[speedRectangle] = "Change this lane's speed limit\r\nMiddle-click to clear it";
		}

		private void DrawLaneWidth(PaintEventArgs e, Point cursor, ref int leftX)
		{
			leftX -= 12 + (scale * 3);

			var sizeMinusRectangle = new Rectangle(leftX, yIndex, scale, scale);
			var sizeRectangle = new Rectangle(leftX + scale, yIndex, scale, scale);
			var sizePlusRectangle = new Rectangle(leftX + (scale * 2), yIndex, scale, scale);

			e.Graphics.DrawString($"{Lane.LaneWidth:0.##}m", new Font(UI.FontFamily, 8.25F), new SolidBrush(sizeRectangle.Contains(cursor) ? FormDesign.Design.RedColor : Lane.CustomWidth != null ? FormDesign.Design.ActiveColor : foreColor), sizeRectangle.Pad(-10), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });

			if (sizeMinusRectangle.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), sizeMinusRectangle, 4);
			}

			e.Graphics.DrawImage(Properties.Resources.I_Minus.Color(sizeMinusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), sizeMinusRectangle.CenterR(16, 16));

			if (sizePlusRectangle.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), sizePlusRectangle, 4);
			}

			e.Graphics.DrawImage(Properties.Resources.I_Plus.Color(sizePlusRectangle.Contains(cursor) ? FormDesign.Design.ActiveForeColor : foreColor), sizePlusRectangle.CenterR(16, 16));

			DrawLine(e, leftX - 6);

			_clickActions[sizeMinusRectangle] = WidthMinusClick;
			_clickActions[sizePlusRectangle] = WidthPlusClick;
			_clickActions[sizeRectangle] = WidthResetClick;

			_tooltips[sizeMinusRectangle] = "Decrease lane width by 0.1\r\n\r\nUse Shift for 1x, Ctrl for 0.25x & Alt for 0.01x";
			_tooltips[sizePlusRectangle] = "Increase lane width by 0.1\r\n\r\nUse Shift for 1x, Ctrl for 0.25x & Alt for 0.01x";
			_tooltips[sizeRectangle] = "Reset lane width to " + Lane.DefaultLaneWidth();
		}

		private void DrawLine(PaintEventArgs e, int x)
		{
			var foreColor = _dragDropActive ? Color.FromArgb(150, Lane.Color.GetAccentColor()) : FormDesign.Design.AccentColor;

			e.Graphics.DrawLine(new Pen(foreColor), x, 6, x, Height - 13);
		}

		private void DrawSelection(PaintEventArgs e, Rectangle rectangle)
		{
			e.Graphics.DrawLine(new Pen(FormDesign.Design.ActiveColor, 3.5F) { EndCap = System.Drawing.Drawing2D.LineCap.Round }, rectangle.X + 1, rectangle.Y + rectangle.Height - 3, rectangle.X + rectangle.Width - 2, rectangle.Y + rectangle.Height - 3);
		}

		private void DuplicateLaneClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
				return;

			var index = Parent.Controls.IndexOf(this);

			var ctrl = new RoadLane(new ThumbnailLaneInfo(Lane));

			if (e.Button == MouseButtons.Right && Lane.Direction != LaneDirection.Both)
				ctrl.Lane.Direction = ctrl.Lane.Direction == LaneDirection.Forward ? LaneDirection.Backwards : LaneDirection.Forward;

			Parent.Controls.Add(ctrl);
			Parent.Controls.SetChildIndex(ctrl, index);

			RefreshRoad();
		}

		private void ElevationMinusClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			if (Options.Current.AdvancedElevation)
			{
				var change = GetModifierValue();

				Lane.Elevation = (float)Math.Max(GetDefaultElevation(false), Math.Round(((Lane.Elevation ?? GetDefaultElevation(true)) - change) / change) * change);
			}
			else
			{
				if (Lane.Decorations.HasAnyFlag(LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement))
				{
					var defaultElevation = GetDefaultElevation(true);

					Lane.Elevation = defaultElevation == 0 ? -0.3F : (float?)0;
				}
				else
				{
					Lane.Elevation = null;
				}
			}

			RefreshRoad();
		}

		private void ElevationPlusClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			if (Options.Current.AdvancedElevation)
			{
				var change = GetModifierValue();

				Lane.Elevation = (float)Math.Max(GetDefaultElevation(false), Math.Round(((Lane.Elevation ?? GetDefaultElevation(true)) + change) / change) * change);
			}
			else
			{
				if (Lane.Decorations.HasAnyFlag(LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement))
				{
					Lane.Elevation = null;
				}
				else
				{
					var defaultElevation = GetDefaultElevation(true);

					Lane.Elevation = defaultElevation == 0 ? 0.2F : (float?)0;
				}
			}

			RefreshRoad();
		}

		private static float GetModifierValue()
		{
			return ModifierKeys.HasFlag(Keys.Shift | Keys.Control) ? 5F : ModifierKeys.HasFlag(Keys.Shift) ? 1F : ModifierKeys.HasFlag(Keys.Control) ? 0.25F : ModifierKeys.HasFlag(Keys.Alt) ? 0.01F : 0.1F;
		}

		private void ElevationResetClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			Lane.Elevation = null;

			RefreshRoad();
		}

		private float GetDefaultElevation(bool addFiller)
		{
			var index = Parent.Controls.IndexOf(this);
			var leftSidewalk = Parent.Controls.OfType<RoadLane>().FirstOrDefault(x => x.Lane.Type == LaneType.Curb && x.Lane.Direction == LaneDirection.Backwards);
			var rightSidewalk = Parent.Controls.OfType<RoadLane>().LastOrDefault(x => x.Lane.Type == LaneType.Curb && x.Lane.Direction == LaneDirection.Forward);
			var sideWalk = !(leftSidewalk != null && rightSidewalk != null & index < Parent.Controls.IndexOf(leftSidewalk) && index > Parent.Controls.IndexOf(rightSidewalk));

			var @base = sideWalk || RoadType != RoadType.Road ? 0F : -0.3F;

			if (Lane.Elevation == null && addFiller && Lane.Decorations.HasAnyFlag(LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement))
				@base = @base == 0F ? 0.2F : 0F;

			return @base;
		}

		private void GrabberClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			_dragDropActive = true;

			Refresh();

			DoDragDrop(this, DragDropEffects.Move);

			_dragDropActive = false;
			Invalidate();

			if (Lane.Type == LaneType.Curb)
				FixCurbOrientation();
		}

		private void InfoLaneClick(object sender, MouseEventArgs e)
		{
			MessagePrompt.Show($"This is the {(Lane.Direction == LaneDirection.Forward ? "Right" : "Left")} Curb Delimiter;\r\n\r\nIt is used to separate what lanes are on the {(Lane.Direction == LaneDirection.Forward ? "right" : "left")} sidewalk and what lanes are on the asphalt.\r\n\r\nTry moving lanes and see how it affects the road in the preview.", PromptButtons.OK, PromptIcons.Info);
		}

		private void LaneDecoClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				return;

			if (e.Button == MouseButtons.Middle)
				Lane.Decorations = LaneDecoration.None;
			else
				new DecoTypeSelector(this);

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);
		}

		private void LaneTypeClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				return;

			if (e.Button == MouseButtons.Middle)
				Lane.Type = LaneType.Empty;
			else
				new RoadTypeSelector(this);

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);
		}

		private void SpeedLimitClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
				return;

			if (e.Button == MouseButtons.Middle)
				Lane.SpeedLimit = null;
			else
				new LaneSpeedSelector(this);

			RoadLaneChanged?.Invoke(this, EventArgs.Empty);
		}

		private void WidthMinusClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			var change = GetModifierValue();

			Lane.CustomWidth = (float)Math.Max(0.1, Math.Round((Lane.LaneWidth - change) / change) * change);

			RefreshRoad();
		}

		private void WidthPlusClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			var change = GetModifierValue();

			Lane.CustomWidth = (float)Math.Max(0.1, Math.Round((Lane.LaneWidth + change) / change) * change);

			RefreshRoad();
		}

		private void WidthResetClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			Lane.CustomWidth = null;

			RefreshRoad();
		}
	}
}