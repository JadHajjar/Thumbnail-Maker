using Extensions;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;

using ThumbnailMaker.Domain;

using static System.Windows.Forms.AxHost;

namespace ThumbnailMaker.Handlers
{
	public class ThumbnailHandler
	{
		public ThumbnailHandler(Graphics graphics, bool small, bool toolTip)
		{
			Graphics = graphics;
			Small = small || toolTip;
			Width = toolTip ? 492 : Small ? 109 : 512;
			Height = toolTip ? 147 : Small ? 100 : 512;
			PixelFactor = Small ? 8 : 30;

			LaneSizeOptions.Refresh();
		}

		public float RoadWidth { get; set; }
		public float AsphaltWidth { get; set; }
		public float BufferSize { get; set; }
		public int Speed { get; set; }
		public string CustomText { get; set; }
		public List<ThumbnailLaneInfo> Lanes { get; set; }
		public RegionType RegionType { get; set; }
		public RoadType RoadType { get; set; }
		public TextureType SideTexture { get; set; }

		public Graphics Graphics { get; }
		public int Height { get; }
		public int Width { get; }
		public bool Small { get; }
		private int PixelFactor { get; }

		private float IdealWidthModifier;
		private Color PavementColor
		{
			get
			{
				switch (SideTexture)
				{
					case TextureType.Pavement:
						return Color.FromArgb(180, 180, 180);
					case TextureType.Gravel:
						return Color.FromArgb(143, 121, 97);
					case TextureType.Ruined:
						return Color.FromArgb(181, 176, 125);
					default:
						return Color.FromArgb(50, 50, 50);
				}
			}
		}

		public void Draw()
		{
			Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

			PrepareLanes();

			IdealWidthModifier = Lanes.Sum(x => x.Width).If(x => x > Width, x => Width / (float)x, _ => 1F);

			Lanes.Foreach(x => x.Width = (int)Math.Floor(x.Width * IdealWidthModifier));

			var lanesWidth = Lanes.Sum(x => x.Width);
			var maxWidth = Lanes.Any() ? Lanes.Max(x => x.Width) : 50;
			var availableSpace = new Rectangle(0, 0, Width, Height - (Small ? 55 : 275));
			var xIndex = (Width - lanesWidth) / 2;
			var laneRects = new Dictionary<ThumbnailLaneInfo, Rectangle>();

			foreach (var lane in Lanes)
			{
				laneRects[lane] = new Rectangle(xIndex, 0, lane.Width, Height);

				xIndex += lane.Width;
			}

			DrawBackground(availableSpace, laneRects);

			using (var logo = ResourceManager.Logo(Small))
			{
				if (logo != null)
					Graphics.DrawImage(logo, new Rectangle(new Point((Width - logo.Width) / 2, 0), logo.Size));

				DrawCustomText(new Rectangle(0, 0, Width, Height).Pad(0, logo?.Height ?? 0, 0, 0));
			}

			foreach (var lane in Lanes.Where(x => (int)x.Type > 0))
				DrawBackground(lane, laneRects[lane], availableSpace.Pad(0, 0, 0, LaneHeight(lane)));

			foreach (var lane in Lanes.Where(x => (int)x.Type > 0))
				DrawLane(lane, laneRects[lane], availableSpace.Pad(0, 0, 0, LaneHeight(lane)), IdealWidthModifier);

			DrawBottomContent();
		}

		private void DrawBackground(Rectangle availableSpace, Dictionary<ThumbnailLaneInfo, Rectangle> laneRects)
		{
			var leftSidewalk = Lanes.FirstOrDefault(x => x.Type == LaneType.Curb && x.Direction == LaneDirection.Backwards);
			var rightSidewalk = Lanes.LastOrDefault(x => x.Type == LaneType.Curb && x.Direction == LaneDirection.Forward);
			var bottomArea = new Rectangle(availableSpace.X, availableSpace.Y + availableSpace.Height, availableSpace.Width, Height - (availableSpace.Y + availableSpace.Height)).Pad(0, 0, 0, 0);

			Graphics.Clear(Color.FromArgb(50, 50, 50));

			Graphics.FillRectangle(new SolidBrush(Color.FromArgb(174, 215, 242)), new Rectangle(0, 0, bottomArea.Width, bottomArea.Y + (RoadType == RoadType.Road ? (int)(0.6F * PixelFactor * IdealWidthModifier) : 0)));

			if (leftSidewalk != null)
			{
				Graphics.FillRectangle(new SolidBrush(PavementColor), new Rectangle(0, bottomArea.Y, laneRects[leftSidewalk].X + laneRects[leftSidewalk].Width, bottomArea.Height));
			}

			if (rightSidewalk != null)
			{
				Graphics.FillRectangle(new SolidBrush(PavementColor), new Rectangle(laneRects[rightSidewalk].X, bottomArea.Y, Width - laneRects[rightSidewalk].X, bottomArea.Height));
			}
		}

		private void DrawLane(ThumbnailLaneInfo lane, Rectangle rect, Rectangle availableSpace, float scale)
		{
			if (rect.Width < 0.5F * PixelFactor * scale)
				rect = rect.Pad(-(int)(0.5F * PixelFactor * scale - rect.Width) / 2, 0, -(int)(0.5F * PixelFactor * scale - rect.Width) / 2, 0);

			var classIcons = lane.Icons(Small);

			var icons = classIcons.OrderByDescending(LaneTypeImportance).Select(x =>
			{
				var standardWidth = PixelFactor * LaneSizeOptions.GetDefaultLaneWidth(x.Key);

				if (rect.Width >= Math.Round(standardWidth * scale) - 1)
					using (x.Value)
						return new Bitmap(x.Value, Math.Max(1, (int)(x.Value.Width * scale)), Math.Max(1, (int)(x.Value.Height * scale)));

				using (x.Value)
					return new Bitmap(x.Value, Math.Max(1, (int)(x.Value.Width * (rect.Width + standardWidth - scale * standardWidth) / standardWidth * scale)), Math.Max(1, (int)(x.Value.Height * rect.Width / standardWidth * scale)));
			}).ToList();

			var arrowY = 0;
			var decoY = availableSpace.Y + availableSpace.Height;

			if (icons.Count > 0)
			{
				var y = availableSpace.Y + availableSpace.Height - icons[0].Height;

				foreach (var icon in icons)
				{
					Graphics.DrawImage(icon, new Rectangle(new Point(rect.X + (rect.Width - icon.Width) / 2, y), icon.Size));

					arrowY = y + icon.Height;

					y += (int)(icon.Height * 2 / scale / 1.25 / icons.Count);
				}

				decoY = arrowY + (Small ? 2 : 8);
				arrowY += (int)((Small ? 1 : 7) * (1 - icons.Count) + (1 - scale) * 20);

				if (DisplaysArrows(lane))
				{
					using (var arrow = ResourceManager.Arrow(Small))
					{
						if (arrow != null)
						{
							var arrowRect = new Rectangle(rect.X, arrowY, rect.Width, (int)(arrow.Height * scale))
								.CenterR(Math.Min(45, rect.Width), Math.Min(45, rect.Width));

							if (lane.ParkingAngle == ParkingAngle.Horizontal)
								arrow.RotateFlip(lane.Direction == LaneDirection.Backwards ? RotateFlipType.Rotate270FlipNone : RotateFlipType.Rotate90FlipNone);
							else if (lane.Direction == LaneDirection.Backwards && !(lane.ParkingAngle == ParkingAngle.Diagonal || lane.ParkingAngle == ParkingAngle.InvertedDiagonal))
								arrow.RotateFlip(RotateFlipType.Rotate180FlipNone);

							if (lane.ParkingAngle == ParkingAngle.Diagonal || lane.ParkingAngle == ParkingAngle.InvertedDiagonal)
								DrawDiagonalArrow(lane, arrowRect, arrow);
							else
								Graphics.DrawImage(arrow, arrowRect);
						}
					}
				}
			}

			var decoIcons = lane.Decorations.GetValues().Select(x => GetDecorationIcon(lane, x, rect, scale)).ToList();

			decoIcons.Insert(0, GetDecorationIcon(lane, GetLightType(lane), rect, scale));

			foreach (var decoIcon in decoIcons)
			{
				if (decoIcon == null)
				{
					continue;
				}

				if (decoY == availableSpace.Y + availableSpace.Height)
					decoY -= decoIcon.Height;

				Graphics.DrawImage(decoIcon, new Rectangle(new Point(rect.X + (rect.Width - decoIcon.Width) / 2, decoY), decoIcon.Size));

				decoY += decoIcon.Height + (Small ? 2 : 8);
			}
		}

		private void DrawBackground(ThumbnailLaneInfo lane, Rectangle rect, Rectangle availableSpace)
		{
			var leftLane = Lanes.Previous(lane);
			var rightLane = Lanes.Next(lane);
			var bottomArea = new Rectangle(rect.X, availableSpace.Y + availableSpace.Height, rect.Width, Height - (availableSpace.Y + availableSpace.Height));

			Graphics.FillRectangle(new SolidBrush(lane.Sidewalk ? PavementColor : Color.FromArgb(50, 50, 50)), bottomArea);

			if (lane.Type == LaneType.Curb && RoadType != RoadType.Highway)
				Graphics.FillRectangle(new SolidBrush(Color.FromArgb(130, 130, 130)), lane.Direction == LaneDirection.Backwards ? new Rectangle(bottomArea.X + bottomArea.Width - (Small ? 3 : 7), bottomArea.Y, Small ? 3 : 7, bottomArea.Height) : new Rectangle(bottomArea.X, bottomArea.Y, Small ? 3 : 7, bottomArea.Height));

			if (Options.Current.ShowLaneColorsOnThumbnail && lane.Type != LaneType.Curb && (!lane.Sidewalk || lane.Type != LaneType.Pedestrian))
			{
				Graphics.FillRectangle(SlickControls.SlickControl.Gradient(rect, Color.FromArgb(175, lane.Color), 2), rect);
			}

			if (lane.Decorations.HasAnyFlag(LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement))
			{
				var fillArea = new Rectangle(rect.X, availableSpace.Y + availableSpace.Height - (lane.Sidewalk || lane.Type != LaneType.Filler ? 0 : (int)(0.3F * PixelFactor)), rect.Width, Height);

				Graphics.FillRectangle(new SolidBrush(Color.FromArgb(160, 160, 160)), fillArea.Pad(3, 0, 3, 0));
				Graphics.FillRectangle(new SolidBrush(ThumbnailLaneInfo.GetColor(lane.Decorations & (LaneDecoration.Grass | LaneDecoration.Gravel | LaneDecoration.Pavement))), fillArea.Pad(5, 0, 5, 0));
			}

			if (lane.Decorations.HasFlag(LaneDecoration.Filler))
				Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, lane.Color)), bottomArea);

			if (Small)
				return;

			if (lane.Type.HasFlag(LaneType.Tram) || lane.Type.HasFlag(LaneType.Train))
			{
				Graphics.DrawLine(new Pen(Color.FromArgb(175, 150, 150, 150), 4F), rect.X + rect.Width / 4, bottomArea.Y, rect.X + rect.Width / 4, bottomArea.Y + bottomArea.Height);
				Graphics.DrawLine(new Pen(Color.FromArgb(175, 150, 150, 150), 4F), rect.X + 3 * rect.Width / 4, bottomArea.Y, rect.X + 3 * rect.Width / 4, bottomArea.Y + bottomArea.Height);

				Graphics.DrawLine(new Pen(Color.FromArgb(175, 255, 255, 255), 1F), rect.X + rect.Width / 4, bottomArea.Y, rect.X + rect.Width / 4, bottomArea.Y + bottomArea.Height);
				Graphics.DrawLine(new Pen(Color.FromArgb(175, 255, 255, 255), 1F), rect.X + 3 * rect.Width / 4, bottomArea.Y, rect.X + 3 * rect.Width / 4, bottomArea.Y + bottomArea.Height);
			}

			if (leftLane != null)
			{
				if (lane.Type >= LaneType.Bike && leftLane.Type == lane.Type && lane.Direction != leftLane.Direction)
					Graphics.DrawLine(new Pen(Color.FromArgb(213, 157, 37), 2F), rect.X + 4, bottomArea.Y, rect.X + 4, bottomArea.Y + bottomArea.Height);

				if (lane.Type >= LaneType.Bike && leftLane.Type != lane.Type)
					Graphics.DrawLine(new Pen(Color.FromArgb(200, 200, 200), 2F), rect.X, bottomArea.Y, rect.X, bottomArea.Y + bottomArea.Height);
				
				if (lane.Type == LaneType.Car && leftLane.Type == LaneType.Car && leftLane.Direction == lane.Direction)
					Graphics.DrawLine(new Pen(Color.FromArgb(200, 200, 200), 2F) { DashPattern = new[] { 15F, 6F } }, rect.X, bottomArea.Y, rect.X, bottomArea.Y + bottomArea.Height);
			}

			if (rightLane != null)
			{
				if (lane.Type >= LaneType.Bike && rightLane.Type == lane.Type && lane.Direction != rightLane.Direction)
					Graphics.DrawLine(new Pen(Color.FromArgb(213, 157, 37), 2F), rect.X + rect.Width - 4, bottomArea.Y, rect.X + rect.Width - 4, bottomArea.Y + bottomArea.Height);
			
				if (lane.Type >= LaneType.Bike && rightLane.Type != lane.Type)
					Graphics.DrawLine(new Pen(Color.FromArgb(200, 200, 200), 2F), rect.X + rect.Width, bottomArea.Y, rect.X + rect.Width, bottomArea.Y + bottomArea.Height);

				if (lane.Type == LaneType.Car && rightLane.Type == LaneType.Car && rightLane.Direction == lane.Direction)
					Graphics.DrawLine(new Pen(Color.FromArgb(200, 200, 200), 2F) { DashPattern = new[] { 15F, 6F } }, rect.X + rect.Width, bottomArea.Y, rect.X + rect.Width, bottomArea.Y + bottomArea.Height);
			}
		}

		private Image GetDecorationIcon(ThumbnailLaneInfo lane, LaneDecoration style, Rectangle rect, float scale)
		{
			if (style == LaneDecoration.Grass || style == LaneDecoration.Gravel || style == LaneDecoration.Pavement || style == LaneDecoration.Filler || style == LaneDecoration.None)
				return null;

			var decorationIcon = ResourceManager.GetImage(style, Small);

			if (decorationIcon == null)
			{
				return null;
			}

			var flip = Lanes.IndexOf(lane) < Lanes.Count / 2;

			if (lane.Decorations.HasFlag(style) && lane.PropAngle == PropAngle.Left)
				flip = !flip;

			if (style == LaneDecoration.TransitStop)
			{
				var index = Lanes.IndexOf(lane);
				var left = index == 0 ? null : Lanes[index - 1];
				var right = index == (Lanes.Count - 1) ? null : Lanes[index + 1];

				flip = left != null && (left.Direction == LaneDirection.Forward || right == null || !right.Type.HasAnyFlag(LaneType.Car, LaneType.Bus, LaneType.Trolley, LaneType.Tram)) && left.Type.HasAnyFlag(LaneType.Car, LaneType.Bus, LaneType.Trolley, LaneType.Tram);
			}

			if (flip)
				decorationIcon.RotateFlip(RotateFlipType.RotateNoneFlipX);

			var standardWidth = PixelFactor * GetStandardWidth(style);

			if (rect.Width >= Math.Round(standardWidth * scale) - 1)
				using (decorationIcon)
					return new Bitmap(decorationIcon, Math.Max(1, (int)(decorationIcon.Width * scale)), Math.Max(1, (int)(decorationIcon.Height * scale)));

			using (decorationIcon)
				return new Bitmap(decorationIcon, Math.Max(1, (int)(decorationIcon.Width * (rect.Width + standardWidth - scale * standardWidth) / standardWidth * scale)), Math.Max(1, (int)(decorationIcon.Height * rect.Width / standardWidth * scale)));
		}

		private void DrawDiagonalArrow(ThumbnailLaneInfo lane, Rectangle rect, Image arrow)
		{
			if (lane.Direction == LaneDirection.Backwards)
				arrow.RotateFlip(RotateFlipType.RotateNoneFlipY);

			var translate = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

			Graphics.TranslateTransform(translate.X, translate.Y);
			Graphics.RotateTransform(lane.ParkingAngle == ParkingAngle.InvertedDiagonal ? -45F : 45F);

			Graphics.DrawImage(arrow, new Rectangle(new Point(-rect.Width / 2, -rect.Height / 2), rect.Size));

			Graphics.RotateTransform(lane.ParkingAngle == ParkingAngle.InvertedDiagonal ? 45F : -45F);
			Graphics.TranslateTransform(-translate.X, -translate.Y);
		}

		private float GetStandardWidth(LaneDecoration decorations)
		{
			switch (decorations)
			{
				case LaneDecoration.Benches:
					return 1F;
				case LaneDecoration.FlowerPots:
					return 0.5F;
				case LaneDecoration.Bollard:
					return 0.25F;
				case LaneDecoration.Hedge:
					return 1.5F;
				case LaneDecoration.TrashBin:
					return 0.5F;
			}

			return 1F;
		}

		private int LaneTypeImportance(KeyValuePair<LaneType, Image> x)
		{
			switch (x.Key)
			{
				case LaneType.Train:
					return 21;
				case LaneType.Tram:
					return 20;
				case LaneType.Emergency:
					return 19;
				case LaneType.Trolley:
					return 18;
				case LaneType.Bus:
					return 17;
				case LaneType.Car:
					return 16;
				case LaneType.Bike:
					return 15;
				case LaneType.Parking:
					return 14;
				case LaneType.Pedestrian:
					return 13;
				default:
					return 0;
			}
		}

		private void PrepareLanes()
		{
			Lanes.ForEach(lane => { lane.Width = (int)(PixelFactor * lane.LaneWidth); lane.Sidewalk = false; });

			var leftSidewalk = Lanes.FirstOrDefault(x => x.Type == LaneType.Curb && x.Direction == LaneDirection.Backwards);
			var rightSidewalk = Lanes.LastOrDefault(x => x.Type == LaneType.Curb && x.Direction == LaneDirection.Forward);

			if (leftSidewalk != null && rightSidewalk != null && Lanes.IndexOf(rightSidewalk) == (Lanes.IndexOf(leftSidewalk) + 1))
				Lanes.Insert(Lanes.IndexOf(rightSidewalk), new ThumbnailLaneInfo { Width = 90 });

			if (leftSidewalk != null)
			{
				for (var i = 0; i <= Lanes.IndexOf(leftSidewalk); i++)
					Lanes[i].Sidewalk = true;

				Lanes.Insert(Lanes.IndexOf(leftSidewalk) + 1, new ThumbnailLaneInfo { Width = (int)(PixelFactor * BufferSize) });
			}
			else
				Lanes.Insert(0, new ThumbnailLaneInfo { Width = (int)(PixelFactor * BufferSize) });

			if (rightSidewalk != null)
			{
				for (var i = Lanes.IndexOf(rightSidewalk); i < Lanes.Count; i++)
					Lanes[i].Sidewalk = true;

				Lanes.Insert(Lanes.IndexOf(rightSidewalk), new ThumbnailLaneInfo { Width = (int)(PixelFactor * BufferSize) });
			}
			else
				Lanes.Insert(Lanes.Count, new ThumbnailLaneInfo { Width = (int)(PixelFactor * BufferSize) });

			AsphaltWidth = Lanes.Where(x => !x.Sidewalk).Sum(lane => lane.LaneWidth);
		}

		private LaneDecoration GetLightType(ThumbnailLaneInfo lane)
		{
			var centerLane = GetCenterLane();

			if (centerLane == lane)
				return LaneDecoration.DoubleStreetLight;

			if (lane.Type == LaneType.Curb)
			{
				if (lane.Direction == LaneDirection.Forward && (centerLane == null || AsphaltWidth >= 30F))
					return LaneDecoration.StreetLight;

				if (lane.Direction == LaneDirection.Backwards && AsphaltWidth >= (centerLane == null ? 20F : 30F))
					return LaneDecoration.StreetLight;
			}

			return LaneDecoration.None;
		}

		private ThumbnailLaneInfo GetCenterLane()
		{
			var stoppableVehicleLanes = LaneType.Car | LaneType.Bus | LaneType.Trolley | LaneType.Tram;
			var fillerLanes = Lanes.Where(x => x.Type == LaneType.Filler && x.LaneWidth >= 2);
			var validLanes = fillerLanes.Where(lane =>
			{
				var index = Lanes.IndexOf(lane);
				var left = index == 0 ? null : Lanes[index - 1];
				var right = index == (Lanes.Count - 1) ? null : Lanes[index + 1];

				return ((left?.Type ?? LaneType.Empty) & stoppableVehicleLanes) != 0 && ((right?.Type ?? LaneType.Empty) & stoppableVehicleLanes) != 0 && left.Direction != right.Direction;
			});

			return validLanes.OrderBy(x => Math.Abs(Lanes.Count / 2 - Lanes.IndexOf(x))).FirstOrDefault();
		}

		private bool DisplaysArrows(ThumbnailLaneInfo lane)
		{
			if (lane.Type.AnyOf(LaneType.Curb, LaneType.Pedestrian, LaneType.Filler)
				|| lane.Direction == LaneDirection.Both)
			{
				return false;
			}

			return true;
		}

		private int LaneHeight(ThumbnailLaneInfo lane)
		{
			var @base = lane.Elevation ?? (lane.Sidewalk || RoadType != RoadType.Road ? 0F : -0.3F);

			return (int)(2 * @base * PixelFactor * IdealWidthModifier);
		}

		#region BottomContents
		private void DrawBottomContent()
		{
			var speed = Speed > 0;
			var size = RoadWidth > 0;

			var speedWidth = speed ? GetSpeedWidth() : 0;
			var roadSizeWidth = size ? GetRoadSizeWidth() : 0;

			if (speed)
			{
				if (Small)
					DrawSpeedSignSmall(Graphics, RegionType, Speed, new Rectangle(size ? (Width / 2 + roadSizeWidth / 2 + (Small ? 3 : 24)) : (Width - speedWidth) / 2, Height - 28, speedWidth, 30));
				else
					DrawSpeedSignLarge(Graphics, RegionType, Speed, new Rectangle(size ? (Width / 2 + roadSizeWidth / 2 + (Small ? 3 : 24)) : (Width - speedWidth) / 2, Height - 120, speedWidth, 120));
			}

			if (size)
				DrawRoadWidth(new Rectangle(0, Height - (Small ? 30 : 120), Width, (Small ? 30 : 120)));

			DrawRoadIcon(new Rectangle(0, Height - (Small ? 30 : 120), size ? ((Width - roadSizeWidth) / 2 - (Small ? 3 : 24)) : speed ? ((Width - speedWidth) / 2 - (Small ? 3 : 24)) : Width, (Small ? 30 : 120)));
		}

		private void DrawCustomText(Rectangle containerRect)
		{
			if(!Small)
			Graphics.DrawString(CustomText, new Font(Options.Current.SizeFont, Small ? 9F : 38F, FontStyle.Bold), Brushes.White, containerRect.Pad(0, Small ? 3 : 10, Small ? -1 : -3, Small ? -1 : -3), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near });
			Graphics.DrawString(CustomText, new Font(Options.Current.SizeFont, Small ? 9F : 38F, FontStyle.Bold), new SolidBrush(Color.FromArgb(40, 40, 40)), containerRect.Pad(0, Small ? 3 : 10, 0, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near });
		}

		private void DrawRoadIcon(Rectangle containerRect)
		{
			using (var icon = ResourceManager.GetRoadType(RoadType, Small))
				if (icon != null)
					Graphics.DrawImage(icon, containerRect.Pad(0, Small ? 4 : 20, 0, 0).CenterR(icon.Size));
		}

		private void DrawRoadWidth(Rectangle containerRect)
		{
			var sizeSize = (int)(Graphics.MeasureString($"{RoadWidth:0.#}m", new Font(Options.Current.SizeFont, Small ? 13F : 50F, FontStyle.Bold, GraphicsUnit.Pixel)).Width);
			var rect = containerRect.Pad(0, Small ? 4 : 14, 0, 0).CenterR(sizeSize + (Small ? 4 : 38), Small ? 20 : 80);

			Graphics.SmoothingMode = SmoothingMode.HighQuality;
			Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(200, 49, 49, 54)), rect, Small ? 2 : 8);
			Graphics.SmoothingMode = SmoothingMode.Default;

			using (var sizePen = new Pen(Color.FromArgb(170, 221, 170, 98), Small ? 1F : 3F))
			{
				for (int i = 0, x = rect.X + (Small ? 4 : 8) + (rect.Width - (Small ? 8 : 16)) % (Small ? 3 : 8) / 2; x < rect.X + rect.Width - (Small ? 4 : 8); x += Small ? 3 : 8, i++)
				{
					Graphics.DrawLine(sizePen, x, rect.Top + rect.Height, x, rect.Top + rect.Height - (i % 5 == 0 ? 2 : 1) * (Small ? 3 : 8));
				}
			}

			Graphics.DrawString($"{RoadWidth:0.#}m", new Font(Options.Current.SizeFont, Small ? 13F : 50F, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.White, rect.Pad(0, 0, 0, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}

		public static void DrawSpeedSignLarge(Graphics Graphics, RegionType RegionType, int Speed, Rectangle containerRect)
		{
			Graphics.SmoothingMode = SmoothingMode.HighQuality;

			if (RegionType == RegionType.Europe)
			{
				var rect = containerRect.CenterR(80, 80);

				Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				Graphics.FillEllipse(Brushes.Black, rect);
				Graphics.FillEllipse(Brushes.White, rect.Pad(1));
				Graphics.FillEllipse(new SolidBrush(Color.FromArgb(207, 30, 22)), rect.Pad(3));
				Graphics.FillEllipse(Brushes.White, rect.Pad(12));
				if (Speed > 0)
					Graphics.DrawString(Speed.ToString(), new Font(Options.Current.SizeFont, Speed > 99 ? 30F : 40F, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, Speed > 99 ? rect.Pad(-3, 4, -3, 0) : rect.Pad(4, 8, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			}
			else if (RegionType == RegionType.USA)
			{
				var rect = containerRect.CenterR(56, 80);

				Graphics.FillRoundedRectangle(Brushes.White, rect, 7);
				Graphics.FillRoundedRectangle(Brushes.Black, rect.Pad(2), 7);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(4), 7);
				Graphics.DrawString("SPEED", new Font(Options.Current.SizeFont, 14.6f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(3-5, 8, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("LIMIT", new Font(Options.Current.SizeFont, 14.6f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(3-5, 22, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				if (Speed > 0)
					Graphics.DrawString(Speed.ToString(), new Font(Options.Current.SizeFont, Speed > 99 ? 25F : 33.3F, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(0, 0, -3, Speed > 99 ? 1 : -3), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}
			else if (RegionType == RegionType.Canada)
			{
				var rect = containerRect.CenterR(68, 80);

				Graphics.FillRoundedRectangle(Brushes.White, rect, 7);
				Graphics.FillRoundedRectangle(Brushes.Black, rect.Pad(1), 7);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(3, 3, 3, 23), 7);
				Graphics.DrawString("MAXIMUM", new Font(Options.Current.SizeFont, 11f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(3, 7, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("km/ h", new Font(Options.Current.SizeFont, 16f, GraphicsUnit.Pixel), Brushes.White, rect.Pad(3, 28, 0, 0), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				if (Speed > 0)
					Graphics.DrawString(Speed.ToString(), new Font(Options.Current.SizeFont, Speed > 99 ? 30F : 37.3F, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(0, 0, -3, Speed > 99 ? 20 : 14), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}

			Graphics.SmoothingMode = SmoothingMode.Default;
		}

		public static void DrawSpeedSignSmall(Graphics Graphics, RegionType RegionType, int Speed, Rectangle containerRect)
		{
			Graphics.SmoothingMode = SmoothingMode.HighQuality;

			if (RegionType == RegionType.Europe)
			{
				var rect = containerRect.CenterR(24, 24);

				Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				Graphics.FillEllipse(Brushes.White, rect);
				Graphics.FillEllipse(new SolidBrush(Color.FromArgb(207, 30, 22)), rect.Pad(1));
				Graphics.FillEllipse(Brushes.White, rect.Pad(4));
				if (Speed > 0)
					Graphics.DrawString(Speed.ToString(), new Font(Options.Current.SizeFont, Speed > 99 ? 9.3F : 10.6F, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(-1, 2, -3, 0), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			}
			else if (RegionType == RegionType.USA)
			{
				var rect = containerRect.CenterR(20, 24);

				Graphics.FillRoundedRectangle(Brushes.Black, rect, 2);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(1), 2);
				Graphics.DrawString("SPEED", new Font(Options.Current.SizeFont, 4.6f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(1 - 5, 2, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("LIMIT", new Font(Options.Current.SizeFont, 4.6f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(1 - 5, 6, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				if (Speed > 0)
					Graphics.DrawString(Speed.ToString(), new Font(Options.Current.SizeFont, Speed > 99 ? 8.6F : 10.6F, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(-1, 0, -3, Speed > 99 ? 1 : -1), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}
			else if (RegionType == RegionType.Canada)
			{
				var rect = containerRect.CenterR(22, 24);

				Graphics.FillRoundedRectangle(Brushes.Black, rect, 2);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(1, 1, 1, 7), 2);
				Graphics.DrawString("MAXIMUM", new Font(Options.Current.SizeFont, 4f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(0, 2, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("km/ h", new Font(Options.Current.SizeFont, 6f, GraphicsUnit.Pixel), Brushes.White, rect.Pad(1, 0, 0, -1), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				if (Speed > 0)
					Graphics.DrawString(Speed.ToString(), new Font(Options.Current.SizeFont, Speed > 99 ? 9.3F : 10.6F, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(-1, 0, -3, Speed > 99 ? 5 : 4), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}

			Graphics.SmoothingMode = SmoothingMode.Default;
		}

		private int GetRoadSizeWidth()
		{
			var sizeSize = (int)Graphics.MeasureString($"{RoadWidth:0.#}m", new Font(Options.Current.SizeFont, Small ? 13F : 50F, FontStyle.Bold, GraphicsUnit.Pixel)).Width;

			return sizeSize + (Small ? 4 : 38);
		}

		private int GetSpeedWidth()
		{
			if (Small)
			{
				if (RegionType == RegionType.Europe)
					return 24;

				if (RegionType == RegionType.USA)
					return 20;

				if (RegionType == RegionType.Canada)
					return 22;
			}
			else
			{
				if (RegionType == RegionType.Europe)
					return 100;

				if (RegionType == RegionType.USA)
					return 70;

				if (RegionType == RegionType.Canada)
					return 85;
			}

			return 0;
		}
#endregion
	}
}
