using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using ThumbnailMaker.Domain;

namespace ThumbnailMaker.Handlers
{
	public class ThumbnailHandler
	{
		public ThumbnailHandler(Graphics graphics, RoadInfo road, bool small, bool toolTip)
		{
			Graphics = graphics;
			Road = road;
			ToolTip = toolTip;
			Small = small || toolTip;
			Width = toolTip ? 492 : Small ? 109 : 512;
			Height = toolTip ? 147 : Small ? 100 : 512;
			PixelFactor = Small ? 8 : 30;

			Lanes = road.Lanes.Select(x => new ThumbnailLaneInfo(x)).ToList();

			LaneSizeOptions.Refresh();
		}

		static ThumbnailHandler()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var pfc = new PrivateFontCollection();
			using (var stream = assembly.GetManifestResourceStream("ThumbnailMaker.Resources.Gogh.ttf"))
			{
				var streamData = new byte[stream.Length];
				stream.Read(streamData, 0, streamData.Length);
				var data = Marshal.AllocCoTaskMem(streamData.Length);
				Marshal.Copy(streamData, 0, data, streamData.Length);
				pfc.AddMemoryFont(data, streamData.Length);
				Marshal.FreeCoTaskMem(data);
			}

			_font = pfc.Families[0];
		}


		public Graphics Graphics { get; }
		public RoadInfo Road { get; }

		public bool ToolTip { get; }
		public int Height { get; }
		public int Width { get; }
		public bool Small { get; }

		private readonly int PixelFactor;
		private readonly List<ThumbnailLaneInfo> Lanes;
		private float IdealWidthModifier;
		private float AsphaltWidth;
		private static readonly FontFamily _font;
		private static FontFamily FontFamily => string.IsNullOrWhiteSpace(Options.Current.TextFont) ? _font : new FontFamily(Options.Current.TextFont);
		private Color PavementColor
		{
			get
			{
				switch (Road.SideTexture)
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

		private static Font GetFont(float size, FontStyle fontStyle = FontStyle.Regular)
		{
			return new Font(FontFamily, size * 4 / 3, fontStyle, GraphicsUnit.Pixel);
		}

		public void Draw()
		{
			if (!Small)
			{
				Graphics.SmoothingMode = SmoothingMode.HighQuality;
			}

			Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

			PrepareLanes();

			IdealWidthModifier = Lanes.Sum(x => x.Width).If(x => x > Width, x => Width / (float)x, _ => 1F);

			Lanes.Foreach(x => x.Width = (int)Math.Floor(x.Width * IdealWidthModifier));

			var lanesWidth = Lanes.Sum(x => x.Width);
			var maxWidth = Lanes.Any() ? Lanes.Max(x => x.Width) : 50;
			var availableSpace = new Rectangle(0, 0, Width, Height - (ToolTip ? 80 : Small ? 60 : 275));
			var xIndex = (Width - lanesWidth) / 2;
			var laneRects = new Dictionary<ThumbnailLaneInfo, Rectangle>();

			foreach (var lane in Lanes)
			{
				laneRects[lane] = new Rectangle(xIndex, 0, lane.Width, Height);

				xIndex += lane.Width;
			}

			DrawBackground(availableSpace, laneRects);

			DrawFillers(availableSpace, laneRects);

			foreach (var lane in Lanes.Where(x => x.Type != LaneType.Empty))
			{
				DrawBackground(lane, laneRects[lane], availableSpace.Pad(0, 0, 0, LaneHeight(lane)));
			}

			foreach (var lane in Lanes.Where(x => x.Type != LaneType.Empty))
			{
				DrawLaneLines(lane, laneRects[lane], availableSpace.Pad(0, 0, 0, LaneHeight(lane)));
			}

			foreach (var lane in Lanes.Where(x => (int)x.Type > 0))
			{
				DrawLane(lane, laneRects[lane], availableSpace.Pad(0, 0, 0, LaneHeight(lane)), IdealWidthModifier);
			}

			using (var logo = ResourceManager.Logo(Small))
			{
				if (logo != null)
				{
					Graphics.DrawImage(logo, new Rectangle(new Point((Width - logo.Width) / 2, Small ? 3 : 10), logo.Size));
				}

				DrawCustomText(new Rectangle(0, 0, Width, Height).Pad(0, (logo?.Height ?? 0).If(x => x > Height / 2, 0), 0, 0));
			}

			DrawBottomContent();
		}

		private void DrawBackground(Rectangle availableSpace, Dictionary<ThumbnailLaneInfo, Rectangle> laneRects)
		{
			var leftSidewalk = Lanes.FirstOrDefault(x => x.Type == LaneType.Curb && x.Direction == LaneDirection.Backwards);
			var rightSidewalk = Lanes.LastOrDefault(x => x.Type == LaneType.Curb && x.Direction == LaneDirection.Forward);
			var bottomArea = new Rectangle(availableSpace.X, availableSpace.Y + availableSpace.Height, availableSpace.Width, Height - (availableSpace.Y + availableSpace.Height)).Pad(0, 0, 0, 0);

			Graphics.Clear(Road.AsphaltStyle == AsphaltStyle.None ? PavementColor : Color.FromArgb(50, 50, 50));

			Graphics.FillRectangle(new SolidBrush(Color.FromArgb(174, 215, 242)), new Rectangle(0, 0, bottomArea.Width, bottomArea.Y - LaneHeight(new ThumbnailLaneInfo())));

			if (!Options.Current.HideSkyInThumb)
			{
				if (Small)
				{
					Graphics.DrawImage(Properties.Resources.L_Clouds_1, new Rectangle((Width / 2) - 50, 0, 16, 16));
					Graphics.DrawImage(Properties.Resources.L_Clouds_2, new Rectangle((Width / 2) + 25, 7, 16, 16));
					Graphics.DrawImage(Properties.Resources.L_Clouds_3, new Rectangle((Width / 2) - 30, 5, 16, 16));
				}
				else
				{
					Graphics.DrawImage(Properties.Resources.L_Clouds_1, new Rectangle(25, 15, 64, 64));
					Graphics.DrawImage(Properties.Resources.L_Clouds_2, new Rectangle(410, 35, 64, 64));
					Graphics.DrawImage(Properties.Resources.L_Clouds_3, new Rectangle(90, 25, 64, 64));
				}
			}

			Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, 174, 215, 242)), new Rectangle(0, 0, bottomArea.Width, bottomArea.Y - LaneHeight(new ThumbnailLaneInfo())));

			if (leftSidewalk != null)
			{
				Graphics.FillRectangle(new SolidBrush(PavementColor), new Rectangle(0, bottomArea.Y, laneRects[leftSidewalk].X + laneRects[leftSidewalk].Width - (int)(PixelFactor * Road.BufferWidth), bottomArea.Height));
			}

			if (rightSidewalk != null)
			{
				Graphics.FillRectangle(new SolidBrush(PavementColor), new Rectangle(laneRects[rightSidewalk].X + (int)(PixelFactor * Road.BufferWidth), bottomArea.Y, Width - laneRects[rightSidewalk].X - (int)(PixelFactor * Road.BufferWidth), bottomArea.Height));
			}
		}

		private void DrawFillers(Rectangle availableSpace, Dictionary<ThumbnailLaneInfo, Rectangle> laneRects)
		{
			var x = 0;
			var w = 0;
			var elevation = 0;
			var filler = LaneDecoration.None;
			var lefpadding = FillerPadding.Unset;
			var rightpadding = FillerPadding.Unset;

			foreach (var lane in Lanes.Where(l => !l.Buffer))
			{
				var laneFiller = lane.Decorations & (LaneDecoration.Grass | LaneDecoration.Gravel | LaneDecoration.Pavement);

				if (laneFiller == filler && LaneHeight(lane) == elevation)
				{
					rightpadding = lane.FillerPadding;
					w += laneRects[lane].Width;
					continue;
				}

				if (filler != LaneDecoration.None)
				{
					var bottomArea = new Rectangle(x, availableSpace.Y + availableSpace.Height, w, Height - (availableSpace.Y + availableSpace.Height)).Pad(0, -elevation, 0, 0);
					var fillArea = bottomArea.Pad(lefpadding.HasFlag(FillerPadding.Left) ? -1 : Small ? 2 : 5, 0, rightpadding.HasFlag(FillerPadding.Right) ? -1 : Small ? 2 : 5, 0);

					Graphics.FillRectangle(new SolidBrush(Color.FromArgb(160, 160, 160)), fillArea);
					Graphics.FillRectangle(new SolidBrush(ThumbnailLaneInfo.GetColor(filler & (LaneDecoration.Grass | LaneDecoration.Gravel | LaneDecoration.Pavement))), Small ? fillArea.Pad(2, 0, 2, 0) : fillArea.Pad(6, 0, 6, 0));
				}

				filler = laneFiller;
				x = laneRects[lane].X;
				w = laneRects[lane].Width;
				elevation = LaneHeight(lane);
				lefpadding = rightpadding = lane.FillerPadding;
			}

			if (filler != LaneDecoration.None)
			{
				var bottomArea = new Rectangle(x, availableSpace.Y + availableSpace.Height, w, Height - (availableSpace.Y + availableSpace.Height)).Pad(0, -elevation, 0, 0);
				var fillArea = bottomArea.Pad(lefpadding.HasFlag(FillerPadding.Left) ? -1 : Small ? 2 : 5, 0, rightpadding.HasFlag(FillerPadding.Right) ? -1 : Small ? 2 : 5, 0);

				Graphics.FillRectangle(new SolidBrush(Color.FromArgb(160, 160, 160)), fillArea);
				Graphics.FillRectangle(new SolidBrush(ThumbnailLaneInfo.GetColor(filler & (LaneDecoration.Grass | LaneDecoration.Gravel | LaneDecoration.Pavement))), Small ? fillArea.Pad(2, 0, 2, 0) : fillArea.Pad(6, 0, 6, 0));
			}
		}

		private void DrawLane(ThumbnailLaneInfo lane, Rectangle rect, Rectangle availableSpace, float scale)
		{
			if (rect.Width < 0.5F * PixelFactor * scale)
			{
				rect = rect.Pad(-(int)((0.5F * PixelFactor * scale) - rect.Width) / 2, 0, -(int)((0.5F * PixelFactor * scale) - rect.Width) / 2, 0);
			}

			var classIcons = lane.Type.GetValues().Where(x => x > LaneType.Curb)
				.ToDictionary(x => x, x => ResourceManager.GetImage(x, Small))
				.Where(x => x.Value != null)
				.ToList();

			var icons = classIcons.OrderByDescending(LaneTypeImportance).Select(x =>
			{
				var standardWidth = PixelFactor * LaneSizeOptions.GetDefaultLaneWidth(x.Key);

				if (rect.Width >= Math.Round(standardWidth * scale) - 1)
				{
					using (x.Value)
					{
						return new Bitmap(x.Value, Math.Max(1, (int)(x.Value.Width * scale)), Math.Max(1, (int)(x.Value.Height * scale)));
					}
				}

				using (x.Value)
				{
					return new Bitmap(x.Value, Math.Max(1, (int)(x.Value.Width * (rect.Width + standardWidth - (scale * standardWidth)) / standardWidth * scale)), Math.Max(1, (int)(x.Value.Height * (rect.Width + standardWidth - (scale * standardWidth)) / standardWidth * scale)));
				}
			}).ToList();

			var arrowY = 0;
			var decoY = availableSpace.Y + availableSpace.Height;

			if (icons.Count > 0)
			{
				var y = availableSpace.Y + availableSpace.Height - icons[0].Height;

				foreach (var icon in icons)
				{
					Graphics.DrawImage(icon, new Rectangle(new Point(rect.X + ((rect.Width - icon.Width) / 2), y), icon.Size));

					arrowY = y + icon.Height;

					y += (int)(icon.Height * 2 / scale / 1.25 / icons.Count);
				}

				decoY = arrowY + (Small ? -4 : 8);
				arrowY += (int)(((Small ? 2 : 7) * (1 - icons.Count)) + (scale * (Small ? 6 : 20)));

				if (DisplaysArrows(lane))
				{
					using (var arrow = ResourceManager.Arrow(Small))
					{
						if (arrow != null)
						{
							var arrowRect = new Rectangle(rect.X, arrowY, rect.Width, (int)Math.Min(Small ? 16 : 45, arrow.Height * scale))
								.CenterR(Math.Min(Small ? 16 : 45, rect.Width), Math.Min(Small ? 16 : 45, rect.Width));

							if (lane.ParkingAngle == ParkingAngle.Horizontal)
							{
								arrow.RotateFlip(lane.Direction == LaneDirection.Backwards ? RotateFlipType.Rotate270FlipNone : RotateFlipType.Rotate90FlipNone);
							}
							else if (lane.Direction == LaneDirection.Backwards && !(lane.ParkingAngle == ParkingAngle.Diagonal || lane.ParkingAngle == ParkingAngle.InvertedDiagonal))
							{
								arrow.RotateFlip(RotateFlipType.Rotate180FlipNone);
							}

							if (lane.ParkingAngle == ParkingAngle.Diagonal || lane.ParkingAngle == ParkingAngle.InvertedDiagonal)
							{
								DrawDiagonalArrow(lane, arrowRect, arrow);
							}
							else
							{
								Graphics.DrawImage(arrow, arrowRect);
							}
						}
					}
				}
			}

			var decoIcons = lane.Decorations.GetValues().Select(x => GetDecorationIcon(lane, x, rect, scale)).ToList();

			decoIcons.Insert(0, GetDecorationIcon(lane, GetLightType(lane), rect, scale));

			var first = icons.Count == 0;
			foreach (var decoIcon in decoIcons)
			{
				if (decoIcon == null)
				{
					continue;
				}

				if (first)
				{
					decoY -= decoIcon.Height;
				}

				Graphics.DrawImage(decoIcon, new Rectangle(new Point(rect.X + ((rect.Width - decoIcon.Width) / 2), decoY), decoIcon.Size));

				if (!first)
				{
					decoY += (Height - availableSpace.Height - ((ToolTip ? 36 : Small ? 30 : 120) / 2)) / decoIcons.Count;
				}
				else
				{
					decoY += decoIcon.Height;
				}

				first = false;
			}
		}

		private void DrawBackground(ThumbnailLaneInfo lane, Rectangle rect, Rectangle availableSpace)
		{
			var bottomArea = new Rectangle(rect.X, availableSpace.Y + availableSpace.Height, rect.Width, Height - (availableSpace.Y + availableSpace.Height));

			if (lane.Type == LaneType.Curb)
			{
				bottomArea = bottomArea.Pad(lane.Direction == LaneDirection.Forward ? (int)(PixelFactor * Road.BufferWidth) : 0, 0, lane.Direction == LaneDirection.Backwards ? (int)(PixelFactor * Road.BufferWidth) : 0, 0);
			}

			if (!lane.Decorations.HasAnyFlag(LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement))
			{
				Graphics.FillRectangle(new SolidBrush(lane.Sidewalk || Road.AsphaltStyle == AsphaltStyle.None ? PavementColor : Color.FromArgb(50, 50, 50)), bottomArea);

				if (lane.Type == LaneType.Curb && Road.RoadType != RoadType.Highway)
				{
					Graphics.FillRectangle(new SolidBrush(Color.FromArgb(130, 130, 130)), lane.Direction == LaneDirection.Backwards ? new Rectangle(bottomArea.X + bottomArea.Width - (Small ? 2 : 7), bottomArea.Y, Small ? 2 : 7, bottomArea.Height) : new Rectangle(bottomArea.X, bottomArea.Y, Small ? 2 : 7, bottomArea.Height));
				}
			}

			if (Options.Current.ShowLaneColorsOnThumbnail && lane.Type != LaneType.Curb && (!lane.Sidewalk || lane.Type != LaneType.Pedestrian) && rect.Width > 0)
			{
				Graphics.FillRectangle(SlickControls.SlickControl.Gradient(rect, Color.FromArgb(175, lane.Color), 2), rect);
			}

			//if (lane.Decorations.HasAnyFlag(LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement))
			//{
			//	var fillArea = bottomArea.Pad(lane.FillerPadding.HasFlag(FillerPadding.Left)? -1:Small?2:5,0, lane.FillerPadding.HasFlag(FillerPadding.Right) ? -1 : Small ? 2 : 5, 0);// new Rectangle(rect.X, availableSpace.Y + availableSpace.Height - (lane.Sidewalk || lane.Type != LaneType.Filler ? 0 : (int)(0.3F * PixelFactor)), rect.Width, Height);

			//	Graphics.FillRectangle(new SolidBrush(Color.FromArgb(160, 160, 160)), fillArea);
			//	Graphics.FillRectangle(new SolidBrush(ThumbnailLaneInfo.GetColor(lane.Decorations & (LaneDecoration.Grass | LaneDecoration.Gravel | LaneDecoration.Pavement))), Small ? fillArea.Pad(2, 0, 2, 0) : fillArea.Pad(6, 0, 6, 0));
			//}

			if (lane.Decorations.HasFlag(LaneDecoration.Filler))
			{
				Graphics.FillRectangle(new SolidBrush(Color.FromArgb(125, lane.Color)), bottomArea);
			}

			if (lane.Decorations.HasFlag(LaneDecoration.Barrier))
			{
				var barrierWidth = Math.Min(PixelFactor * 12 / 10, bottomArea.Width);
				var barrierRect = bottomArea.Pad((bottomArea.Width - barrierWidth) / 2, -PixelFactor, (bottomArea.Width - barrierWidth) / 2, 0);

				Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 130, 135)), barrierRect.Pad(0, PixelFactor, 0, 0));

				Graphics.SetClip(barrierRect.Pad(PixelFactor * 25 / 100, 0, PixelFactor * 25 / 100, 0));

				Graphics.FillRectangle(new SolidBrush(Color.FromArgb(201, 204, 212)), barrierRect);

				using (var pen = new Pen(Color.FromArgb(213, 157, 37), Small ? 2.5F : 6))
				{
					var w = 4 + (PixelFactor * 8 / 10);
					var x = -2 + barrierRect.X + ((barrierRect.Width - w) / 2);
					var y = barrierRect.Y + (w / 4);
					while (y < barrierRect.Y + barrierRect.Height)
					{
						Graphics.DrawLine(pen, x, y, x + w, y + (w * 3 / 4));

						y += Small ? (w - 2) : w;
					}
				}

				Graphics.ResetClip();

				Graphics.DrawLine(new Pen(Color.FromArgb(60, 60, 60), Small ? 1.5F : 4) { DashPattern = new[] { 4F*IdealWidthModifier, 5F * IdealWidthModifier, 4F * IdealWidthModifier, 10F * IdealWidthModifier } }, barrierRect.X + (barrierRect.Width / 2), barrierRect.Y, barrierRect.X + (barrierRect.Width / 2), barrierRect.Y + barrierRect.Height);
			}

			if (!Small)
			{
				var sizeRect = rect.Pad(-10, 365, -10, (ToolTip ? 36 : Small ? 30 : 120) - 20);
				
				if (Options.Current.DisplayCustomLaneSpeedsOnThumbnail && lane.SpeedLimit != null)
				{
					using (var img = new Bitmap(80, 80, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
					using (var g = Graphics.FromImage(img))
					{
						DrawSpeedSignLarge(g, Road.RegionType, (int)lane.SpeedLimit, new Rectangle(0, 0, 80, 80));

						Graphics.DrawImage(img, sizeRect.Pad(0, -rect.Width - (int)(35F * Math.Min(1F, lane.LaneWidth / 3F) * IdealWidthModifier), 0, 0).CenterR(rect.Width * 75 / 100, rect.Width * 75 / 100));
					}
				}

				if (Options.Current.DisplayLaneWidths)
				{
					Graphics.DrawString($"{lane.LaneWidth:0.##}m", GetFont(30F * Math.Min(0.9F, lane.LaneWidth / 3F) * IdealWidthModifier), new SolidBrush(lane.Sidewalk ? Color.FromArgb(50, 50, 50) : Color.FromArgb(230, 230, 230)), sizeRect, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
				}
			}
		}

		private void DrawLaneLines(ThumbnailLaneInfo lane, Rectangle rect, Rectangle availableSpace)
		{
			var bottomArea = new Rectangle(rect.X, availableSpace.Y + availableSpace.Height, rect.Width, Height - (availableSpace.Y + availableSpace.Height));
			var leftLane = Lanes.Previous(lane);
			var rightLane = Lanes.Next(lane);

			if (lane.Type.HasFlag(LaneType.Tram) || lane.Type.HasFlag(LaneType.Train))
			{
				if (!Small)
				{
					Graphics.DrawLine(new Pen(Color.FromArgb(175, 150, 150, 150), 4F), rect.X + (rect.Width / 4), bottomArea.Y, rect.X + (rect.Width / 4), bottomArea.Y + bottomArea.Height);
					Graphics.DrawLine(new Pen(Color.FromArgb(175, 150, 150, 150), 4F), rect.X + (3 * rect.Width / 4), bottomArea.Y, rect.X + (3 * rect.Width / 4), bottomArea.Y + bottomArea.Height);
				}

				Graphics.DrawLine(new Pen(Small ? Color.FromArgb(175, 150, 150, 150) : Color.FromArgb(175, 255, 255, 255), 1F), rect.X + (rect.Width / 4), bottomArea.Y, rect.X + (rect.Width / 4), bottomArea.Y + bottomArea.Height);
				Graphics.DrawLine(new Pen(Small ? Color.FromArgb(175, 150, 150, 150) : Color.FromArgb(175, 255, 255, 255), 1F), rect.X + (3 * rect.Width / 4), bottomArea.Y, rect.X + (3 * rect.Width / 4), bottomArea.Y + bottomArea.Height);
			}

			if (leftLane != null)
			{
				if (lane.Type >= LaneType.Bike && leftLane.Type == lane.Type && lane.Direction != leftLane.Direction)
				{
					Graphics.DrawLine(new Pen(Color.FromArgb(Small ? 175 : 255, 213, 157, 37), Small ? 1F : 2F), rect.X + (Small ? 0 : 3), bottomArea.Y, rect.X + (Small ? 0 : 3), bottomArea.Y + bottomArea.Height);
				}

				if (lane.Type >= LaneType.Bike && leftLane.Type != lane.Type)
				{
					Graphics.DrawLine(new Pen(Color.FromArgb(Small ? 175 : 255, 200, 200, 200), Small ? 1F : 2F), rect.X, bottomArea.Y, rect.X, bottomArea.Y + bottomArea.Height);
				}

				if (lane.Type == LaneType.Car && leftLane.Type == LaneType.Car && leftLane.Direction == lane.Direction)
				{
					Graphics.DrawLine(new Pen(Color.FromArgb(Small ? 175 : 255, 200, 200, 200), (Small ? 1F : 2F)) { DashPattern = Small ? new[] { 11F * IdealWidthModifier, 7F * IdealWidthModifier } : new[] { 15F * IdealWidthModifier, 8F * IdealWidthModifier } }, rect.X, bottomArea.Y, rect.X, bottomArea.Y + bottomArea.Height);
				}
			}

			if (rightLane != null)
			{
				if (lane.Type >= LaneType.Bike && rightLane.Type == lane.Type && lane.Direction != rightLane.Direction)
				{
					Graphics.DrawLine(new Pen(Color.FromArgb(Small ? 175 : 255, 213, 157, 37), Small ? 1F : 2F), rect.X + rect.Width - (Small ? 0 : 3), bottomArea.Y, rect.X + rect.Width - (Small ? 0 : 3), bottomArea.Y + bottomArea.Height);
				}

				if (lane.Type >= LaneType.Bike && rightLane.Type != lane.Type)
				{
					Graphics.DrawLine(new Pen(Color.FromArgb(Small ? 175 : 255, 200, 200, 200), Small ? 1F : 2F), rect.X + rect.Width, bottomArea.Y, rect.X + rect.Width, bottomArea.Y + bottomArea.Height);
				}

				if (lane.Type == LaneType.Car && rightLane.Type == LaneType.Car && rightLane.Direction == lane.Direction)
				{
					Graphics.DrawLine(new Pen(Color.FromArgb(Small ? 175 : 255, 200, 200, 200), Small ? 1F : 2F) { DashPattern = Small ? new[] { 11F * IdealWidthModifier, 7F * IdealWidthModifier } : new[] { 15F * IdealWidthModifier, 8F * IdealWidthModifier } }, rect.X + rect.Width, bottomArea.Y, rect.X + rect.Width, bottomArea.Y + bottomArea.Height);
				}
			}
		}

		private Image GetDecorationIcon(ThumbnailLaneInfo lane, LaneDecoration style, Rectangle rect, float scale)
		{
			if (style == LaneDecoration.Barrier || style == LaneDecoration.BusBay || style == LaneDecoration.Grass || style == LaneDecoration.Gravel || style == LaneDecoration.Pavement || style == LaneDecoration.Filler || style == LaneDecoration.None)
			{
				return null;
			}

			var decorationIcon = ResourceManager.GetImage(style, Small);

			if (decorationIcon == null)
			{
				return null;
			}

			var flip = lane.Position < 0;

			if (lane.Decorations.HasFlag(style) && lane.PropAngle == PropAngle.Left)
			{
				flip = !flip;
			}

			if (style == LaneDecoration.TransitStop)
			{
				var index = Lanes.IndexOf(lane);
				var left = index == 0 ? null : Lanes[index - 1];
				var right = index == (Lanes.Count - 1) ? null : Lanes[index + 1];

				flip = !(left != null && (left.Direction == LaneDirection.Forward || right == null || !right.Type.HasAnyFlag(LaneType.Car, LaneType.Bus, LaneType.Trolley, LaneType.Tram)) && left.Type.HasAnyFlag(LaneType.Car, LaneType.Bus, LaneType.Trolley, LaneType.Tram));
			}

			if (flip)
			{
				decorationIcon.RotateFlip(RotateFlipType.RotateNoneFlipX);
			}

			var standardWidth = PixelFactor * GetStandardWidth(style);

			if (rect.Width >= Math.Round(standardWidth * scale) - 1)
			{
				using (decorationIcon)
				{
					return new Bitmap(decorationIcon, Math.Max(1, (int)(decorationIcon.Width * scale)), Math.Max(1, (int)(decorationIcon.Height * scale)));
				}
			}

			using (decorationIcon)
			{
				return new Bitmap(decorationIcon, Math.Max(1, (int)(decorationIcon.Width * (rect.Width + standardWidth - (scale * standardWidth)) / standardWidth * scale)), Math.Max(1, (int)(decorationIcon.Height * (rect.Width + standardWidth - (scale * standardWidth)) / standardWidth * scale)));
			}
		}

		private void DrawDiagonalArrow(ThumbnailLaneInfo lane, Rectangle rect, Image arrow)
		{
			if (lane.Direction == LaneDirection.Backwards)
			{
				arrow.RotateFlip(RotateFlipType.RotateNoneFlipY);
			}

			var translate = new Point(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));

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

		private int LaneTypeImportance(KeyValuePair<LaneType, Bitmap> x)
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
			Lanes.ForEach(lane =>
			{
				lane.Width = (int)(PixelFactor * lane.LaneWidth);
				lane.Sidewalk = false;
			});

			var leftSidewalk = Lanes.FirstOrDefault(x => x.Type == LaneType.Curb && x.Direction == LaneDirection.Backwards);
			var rightSidewalk = Lanes.LastOrDefault(x => x.Type == LaneType.Curb && x.Direction == LaneDirection.Forward);

			if (leftSidewalk != null && rightSidewalk != null && Lanes.IndexOf(rightSidewalk) == (Lanes.IndexOf(leftSidewalk) + 1))
			{
				Lanes.Insert(Lanes.IndexOf(rightSidewalk), new ThumbnailLaneInfo { Width = 90 });
			}

			if (leftSidewalk != null)
			{
				for (var i = 0; i <= Lanes.IndexOf(leftSidewalk); i++)
				{
					Lanes[i].Sidewalk = true;
				}

				leftSidewalk.Width += (int)(PixelFactor * Road.BufferWidth);
			}

			if (rightSidewalk != null)
			{
				for (var i = Lanes.IndexOf(rightSidewalk); i < Lanes.Count; i++)
				{
					Lanes[i].Sidewalk = true;
				}

				rightSidewalk.Width += (int)(PixelFactor * Road.BufferWidth);
			}

			AsphaltWidth = (Road.BufferWidth * 2) + Lanes.Where(x => !x.Sidewalk).Sum(lane => lane.LaneWidth);

			var ind = AsphaltWidth / -2;

			foreach (var lane in Lanes)
			{
				if (lane.Sidewalk)
				{
					lane.Position = ind;
				}
				else
				{
					lane.Position = ind + (lane.LaneWidth / 2);
					ind += lane.LaneWidth;
				}
			}
		}

		private LaneDecoration GetLightType(ThumbnailLaneInfo lane)
		{
			if (Lanes.Any(x => x.Decorations.HasAnyFlag(LaneDecoration.LampPost, LaneDecoration.StreetLight, LaneDecoration.DoubleStreetLight)))
			{
				return LaneDecoration.None;
			}

			var centerLane = GetCenterLane();

			if (centerLane == lane)
			{
				return LaneDecoration.DoubleStreetLight;
			}

			if (lane.Type == LaneType.Curb)
			{
				if (lane.Direction == LaneDirection.Forward && (centerLane == null || AsphaltWidth >= 30F))
				{
					return LaneDecoration.StreetLight;
				}

				if (lane.Direction == LaneDirection.Backwards && AsphaltWidth >= (centerLane == null ? 20F : 30F))
				{
					return LaneDecoration.StreetLight;
				}
			}

			return LaneDecoration.None;
		}

		private ThumbnailLaneInfo GetCenterLane()
		{
			return Lanes.Where(x => x.Type == LaneType.Filler && x.LaneWidth >= 1.5F && Math.Abs(x.Position) <= 3F)
				.OrderBy(x => Math.Abs(x.Position))
				.FirstOrDefault();
		}

		private bool DisplaysArrows(ThumbnailLaneInfo lane)
		{
			return !lane.Type.AnyOf(LaneType.Curb, LaneType.Pedestrian, LaneType.Filler)
				&& lane.Direction != LaneDirection.Both;
		}

		private int LaneHeight(ThumbnailLaneInfo lane)
		{
			var @base = lane.Elevation ?? (lane.Sidewalk || Road.RoadType != RoadType.Road ? 0F : -0.3F);

			if (lane.Elevation == null && lane.Decorations.HasAnyFlag(LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement))
			{
				@base = @base == 0F ? 0.2F : 0F;
			}

			return (int)(@base * (Small ? 40 : 70) * IdealWidthModifier);
		}

		#region BottomContents
		private void DrawBottomContent()
		{
			var width = ToolTip ? 150 : Small ? 109 : 512;
			var height = ToolTip ? 36 : Small ? 30 : 120;

			if (Road.TotalRoadWidth > 0)
			{
				DrawRoadWidth(new Rectangle(0, Height - height, Width, height));
			}

			var speedLimit = Road.SpeedLimit.If(0, Utilities.DefaultSpeedSign(Road.Lanes, Road.RoadType, Road.RegionType == RegionType.USA));
			
			if (speedLimit > 0)
			{
				if (Small)
				{
					DrawSpeedSignSmall(Graphics, Road.RegionType, speedLimit, new Rectangle(((Width + width) / 2) - height, Height - height, height, height));
				}
				else
				{
					DrawSpeedSignLarge(Graphics, Road.RegionType, speedLimit, new Rectangle(((Width + width) / 2) - height, Height - height, height, height));
				}
			}

			DrawRoadIcon(new Rectangle((Width - width) / 2, Height - height, height, height));
		}

		private void DrawCustomText(Rectangle containerRect)
		{
			containerRect = containerRect.Pad(0, Small ? 3 : 10, 0, containerRect.Height * 2 / 3 + containerRect.Y);
			var font = GetFont(ToolTip ? 12.75F : Small ? 9F : 38F, FontStyle.Bold);

			while (font.Size > 1F)
			{
				if (Graphics.Measure(Road.CustomText, font, containerRect.Width).Height <= containerRect.Height)
				{
					break;
				}

				font = new Font(font.FontFamily, font.Size - 1, font.Style, GraphicsUnit.Pixel);
			}

			if (!Small)
			{
				Graphics.DrawString(Road.CustomText, font, Brushes.White, new Rectangle(containerRect.X + 2, containerRect.Y + 2, containerRect.Width, containerRect.Height), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near });
			}

			Graphics.DrawString(Road.CustomText, font, new SolidBrush(Color.FromArgb(40, 40, 40)), containerRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near });
		}

		private void DrawRoadIcon(Rectangle containerRect)
		{
			using (var icon = ResourceManager.GetRoadType(Road.RoadType, Road.LHT, Small))
			{
				if (icon != null)
				{
					Graphics.DrawImage(icon, containerRect.Pad(0, Small ? 4 : 20, 0, 0).CenterR(icon.Size));
				}
			}
		}

		private void DrawRoadWidth(Rectangle containerRect)
		{
			var text = Options.Current.UseGameUnitsForWidth && Road.TotalRoadWidth % 8 == 0 ? $"{Road.TotalRoadWidth / 8:0}U" : $"{Road.TotalRoadWidth:0.##}m";
			var sizeSize = (int)Graphics.Measure(text, GetFont(ToolTip ? 14F : Small ? 11F : 40F, FontStyle.Bold)).Width;
			var rect = containerRect.Pad(0, Small ? 4 : 14, 0, 0).CenterR(sizeSize + (Small ? 4 : 38), ToolTip ? 30 : Small ? 20 : 80);

			Graphics.SmoothingMode = SmoothingMode.HighQuality;
			Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(150, 49, 49, 54)), rect, Small ? 2 : 8);
			Graphics.SmoothingMode = SmoothingMode.Default;

			using (var sizePen = new Pen(Color.FromArgb(170, 221, 170, 98), Small ? 1F : 3F))
			{
				for (int i = 0, x = rect.X + (Small ? 4 : 8) + ((rect.Width - (Small ? 8 : 16)) % (Small ? 3 : 8) / 2); x < rect.X + rect.Width - (Small ? 4 : 8); x += Small ? 3 : 8, i++)
				{
					Graphics.DrawLine(sizePen, x, rect.Top + rect.Height, x, rect.Top + rect.Height - ((i % 5 == 0 ? 2 : 1) * (Small ? 3 : 8)));
				}
			}

			Graphics.DrawString(text, GetFont(ToolTip ? 14F : Small ? 10.5F : 40F, FontStyle.Bold), Brushes.Black, rect.Pad(Small ? 2 : 4, Small ? 2 : 4, 0, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
			Graphics.DrawString(text, GetFont(ToolTip ? 14F : Small ? 10.5F : 40F, FontStyle.Bold), Brushes.White, rect.Pad(0, 0, 0, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}

		public static void DrawSpeedSignLarge(Graphics Graphics, RegionType RegionType, int Speed, Rectangle containerRect)
		{
			Speed = Speed.Between(-1, 999);

			Graphics.SmoothingMode = SmoothingMode.HighQuality;
			Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

			if (RegionType == RegionType.Europe)
			{
				var rect = containerRect.CenterR(80, 80);

				Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				//Graphics.FillEllipse(Brushes.Black, rect);
				Graphics.FillEllipse(Brushes.White, rect.Pad(1));
				Graphics.FillEllipse(new SolidBrush(Color.FromArgb(207, 30, 22)), rect.Pad(3));
				Graphics.FillEllipse(Brushes.White, rect.Pad(12));
				if (Speed > 0)
				{
					Graphics.DrawString(Speed.ToString(), GetFont(Speed > 99 ? 20F : 25F, FontStyle.Bold), Brushes.Black, Speed > 99 ? rect.Pad(0, 7, -3, -1) : rect.Pad(4, 8, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
				}
			}
			else if (RegionType == RegionType.Australia)
			{
				var rect = containerRect.CenterR(60, 80);

				Graphics.FillRoundedRectangle(Brushes.White, rect, 7);
				Graphics.DrawEllipse(new Pen(Color.FromArgb(207, 30, 22), 6F), containerRect.CenterR(55, 55));
				if (Speed > 0)
				{
					Graphics.DrawString(Speed.ToString(), GetFont(Speed > 99 ? 18F : 23F, FontStyle.Bold), Brushes.Black, rect.Pad(0,0,0, Speed > 99 ? -4 : -6), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
				}
			}
			else if (RegionType == RegionType.USA)
			{
				var rect = containerRect.CenterR(56, 80);

				Graphics.FillRoundedRectangle(Brushes.White, rect, 7);
				Graphics.FillRoundedRectangle(Brushes.Black, rect.Pad(2), 7);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(4), 7);
				Graphics.DrawString("SPEED", GetFont(10F), Brushes.Black, rect.Pad(3 - 5, 8, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("LIMIT", GetFont(10f), Brushes.Black, rect.Pad(3 - 5, 24, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				if (Speed > 0)
				{
					Graphics.DrawString(Speed.ToString(), GetFont(Speed > 99 ? 18F : 24F, FontStyle.Bold), Brushes.Black, rect.Pad(0, 0, -3, Speed > 99 ? 1 : -6), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				}
			}
			else if (RegionType == RegionType.Canada)
			{
				var rect = containerRect.CenterR(68, 80);

				//Graphics.FillRoundedRectangle(Brushes.White, rect, 7);
				Graphics.FillRoundedRectangle(Brushes.Black, rect.Pad(1), 7);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(3, 3, 3, 23), 7);
				Graphics.DrawString("MAXIMUM", GetFont(8f), Brushes.Black, rect.Pad(3, 7, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("km/ h", GetFont(12f), Brushes.White, rect.Pad(3, 30, 0, -2), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				if (Speed > 0)
				{
					Graphics.DrawString(Speed.ToString(), GetFont(Speed > 99 ? 24F : 28F, FontStyle.Bold), Brushes.Black, rect.Pad(-2, 0, -5, Speed > 99 ? 14 : 10), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				}
			}

			Graphics.SmoothingMode = SmoothingMode.Default;
		}

		public static void DrawSpeedSignSmall(Graphics Graphics, RegionType RegionType, int Speed, Rectangle containerRect)
		{
			Speed = Speed.Between(-1, 999);

			Graphics.SmoothingMode = SmoothingMode.HighQuality;
			Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

			if (RegionType == RegionType.Europe)
			{
				var rect = containerRect.CenterR(24, 24);

				Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				Graphics.FillEllipse(Brushes.White, rect);
				Graphics.FillEllipse(new SolidBrush(Color.FromArgb(207, 30, 22)), rect.Pad(1));
				Graphics.FillEllipse(Brushes.White, rect.Pad(4));
				if (Speed > 0)
				{
					Graphics.DrawString(Speed.ToString(), GetFont(Speed > 99 ? 6F : 7.75F), Brushes.Black, rect.Pad(-1, Speed > 99 ? 2 : 0, -3, -1), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
				}
			}
			else if (RegionType == RegionType.Australia)
			{
				var rect = containerRect.CenterR(20, 24);

				Graphics.FillRoundedRectangle(Brushes.White, rect, 2);
				Graphics.DrawEllipse(new Pen(Color.FromArgb(207, 30, 22), 2.5F), containerRect.CenterR(19, 19));
				if (Speed > 0)
				{
					Graphics.DrawString(Speed.ToString(), GetFont(Speed > 99 ? 6F : 7.75F), Brushes.Black, rect.Pad(Speed > 99 ? 0 :2,0,0, Speed > 99 ? -2 : -1), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
				}
			}
			else if (RegionType == RegionType.USA)
			{
				var rect = containerRect.CenterR(20, 24);

				Graphics.FillRoundedRectangle(Brushes.Black, rect, 2);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(1), 2);
				Graphics.DrawString("SPEED", GetFont(3f), Brushes.Black, rect.Pad(1 - 5, 2, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("LIMIT", GetFont(3f), Brushes.Black, rect.Pad(1 - 5, 6, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				if (Speed > 0)
				{
					Graphics.DrawString(Speed.ToString(), GetFont(Speed > 99 ? 6F : 7.75F), Brushes.Black, rect.Pad(-1, 0, -3, Speed > 99 ? 1 : -1), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				}
			}
			else if (RegionType == RegionType.Canada)
			{
				var rect = containerRect.CenterR(22, 24);

				Graphics.FillRoundedRectangle(Brushes.Black, rect, 2);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(1, 1, 1, 7), 2);
				Graphics.DrawString("MAXIMUM", GetFont(2f), Brushes.Black, rect.Pad(0, 2, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("km/ h", GetFont(4f), Brushes.White, rect.Pad(1, 0, 0, 0), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				if (Speed > 0)
				{
					Graphics.DrawString(Speed.ToString(), GetFont(Speed > 99 ? 6.5F : 8.25F), Brushes.Black, rect.Pad(-1, 0, -3, Speed > 99 ? 5 : 4), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				}
			}

			Graphics.SmoothingMode = SmoothingMode.Default;
		}
		#endregion
	}
}
