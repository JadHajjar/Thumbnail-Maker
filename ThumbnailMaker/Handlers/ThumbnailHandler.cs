using Extensions;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using ThumbnailMaker.Domain;

namespace ThumbnailMaker.Handlers
{
	public class ThumbnailHandler
	{
		public RoadType RoadType { get; set; }
		public RegionType RegionType { get; set; }
		public string Speed { get; set; }
		public string RoadSize { get; set; }
		public string CustomText { get; set; }
		public List<LaneInfo> Lanes { get; set; }

		public int Width { get; }
		public int Height { get; }
		public Graphics Graphics { get; }
		public bool Small { get; }

		private Size arrowSize;

		public ThumbnailHandler(Graphics graphics, bool small)
		{
			Graphics = graphics;
			Small = small;
			Width = Small ? 109 : 512;
			Height = Small ? 100 : 512;
		}

		public void Draw()
		{
			Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

			if (Lanes.Count > 0 && Lanes.First().Type == LaneType.Pedestrian)
				Lanes.First().Width = Lanes.First().Width * 2 / 3;

			if (Lanes.Count > 1 && Lanes.Last().Type == LaneType.Pedestrian)
				Lanes.Last().Width = Lanes.Last().Width * 2 / 3;

			var idealWidthModifier = (float)Lanes.Sum(x => x.Width).If(x => x > Width, x => Width / (float)x, _ => 1F);

			Lanes.Foreach(x => x.Width = (int)Math.Floor(x.Width * idealWidthModifier));

			var lanesWidth = Lanes.Sum(x => x.Width);
			var maxWidth = Lanes.Any() ? Lanes.Max(x => x.Width) : 50;

			using (var arrow = ResourceManager.Arrow(Small))
				arrowSize = arrow.Size;

			if (Lanes.Any(HasDoubleArrows))
			{
				arrowSize = new Size((int)((arrowSize.Width / 1.25) * idealWidthModifier), (int)((arrowSize.Height / 1.25) * idealWidthModifier));
			}
			else
			{
				arrowSize = new Size((int)(arrowSize.Width * idealWidthModifier), (int)(arrowSize.Height * idealWidthModifier));
			}

			using (var logo = ResourceManager.Logo(Small))
			{
				var arrowsHeight = Lanes.Any() ? (Lanes.Max(GetArrowRows) * (arrowSize.Height + (Small ? 2 : 8))) : 0;
				var availableSpace = GetAvailableSpace(logo);
				var xIndex = (Width - lanesWidth) / 2;

				foreach (var lane in Lanes)
				{
					var rect = new Rectangle(xIndex, 0, lane.Width, Height);

					DrawLane(lane, rect, availableSpace, arrowsHeight);

					xIndex += lane.Width;
				}

				if (logo != null)
					Graphics.DrawImage(logo, new Rectangle(new Point((Width - logo.Width) / 2, 0), logo.Size));

				DrawBottomContent();
			}
		}

		private Rectangle GetAvailableSpace(Image logo)
		{
			var startingHeight = (logo?.Height ?? 2) - 2;

			if (string.IsNullOrWhiteSpace(RoadSize) && string.IsNullOrWhiteSpace(Speed) && string.IsNullOrWhiteSpace(CustomText))
			{
				var buffer = Small ? 4 : 12;
				return new Rectangle(0, startingHeight + buffer, Width, Height - (startingHeight + buffer * 2));
			}

			return new Rectangle(0, startingHeight, Width, Height - startingHeight - (Small ? 30 : 120));
		}

		private void DrawLane(LaneInfo lane, Rectangle rect, Rectangle availableSpace, int arrowsHeight)
		{
			Graphics.FillRectangle(lane.Brush(Small), rect);

			var icons = lane.Icons(Small);

			if (icons.Count == 0 || lane.Width < 1)
				return;

			icons = icons.Select(x =>
			{
				var modifier = (float)lane.Width / (Small ? 20 : 100);

				using (x)
					return (Image)new Bitmap(x, Math.Max(1, (int)(x.Width * modifier)), Math.Max(1, (int)(x.Height * modifier)));
			}).ToList();

			var iconsSize = icons.Sum(i => i.Height);
			var arrowRows = GetArrowRows(lane);
			var y = availableSpace.Y + (availableSpace.Height - iconsSize - arrowsHeight) / 2 + arrowsHeight;

			foreach (var icon in icons)
			{
				if (lane.Type == LaneType.Highway && lane.Direction == LaneDirection.Backwards)
					icon.RotateFlip(RotateFlipType.RotateNoneFlipY);

				Graphics.DrawImage(icon, new Rectangle(new Point(rect.X + (rect.Width - icon.Width) / 2, y), icon.Size));

				y += icon.Height;
			}

			if (arrowRows == 0)
				return;

			var arrow = ResourceManager.Arrow(Small);
				
			if (arrow == null)
				return;

			if (lane.Type == LaneType.Parking && lane.Lanes == 2)
				arrow.RotateFlip(lane.Direction == LaneDirection.Backwards ? RotateFlipType.Rotate270FlipNone : RotateFlipType.Rotate90FlipNone);

			if (lane.Type != LaneType.Parking && (lane.Direction == LaneDirection.Backwards || lane.Direction == LaneDirection.Both))
				arrow.RotateFlip(RotateFlipType.RotateNoneFlipY);

			foreach (var arrowRect in GetDirectionArrowRects(lane, new Rectangle(rect.X, 0, rect.Width, availableSpace.Y + arrowsHeight + (availableSpace.Height - iconsSize - arrowsHeight) / 2)))
			{
				if (lane.Type == LaneType.Parking && lane.Lanes > 2)
					DrawDiagonalArrow(lane, arrowRect, arrow);
				else
					Graphics.DrawImage(arrow, arrowRect);

				if (lane.Direction == LaneDirection.Both)
					arrow.RotateFlip(RotateFlipType.RotateNoneFlipY);

				if (lane.Direction == LaneDirection.Both && lane.Lanes == 1)
				{
					Graphics.DrawImage(arrow, new Rectangle(arrowRect.X, arrowRect.Y - arrowRect.Height, arrowRect.Width, arrowRect.Height));
				}
			}

			arrow.Dispose();
		}

		private IEnumerable<Rectangle> GetDirectionArrowRects(LaneInfo lane, Rectangle rect)
		{
			var lanes = lane.Type != LaneType.Parking ? lane.Lanes
				: (lane.Direction == LaneDirection.None ? 0 : 1);

			for (var i = 1; i <= lanes / 2; i++)
			{
				yield return new Rectangle(
					rect.X + (rect.Width / 2 - arrowSize.Width) / 2 + (Small ? 0 : 3),
					rect.Height - arrowSize.Height * i,
					arrowSize.Width,
					arrowSize.Height);

				yield return new Rectangle(
					rect.X + rect.Width / 2 + (rect.Width / 2 - arrowSize.Width) / 2 - (Small ? 1 : 3),
					rect.Height - arrowSize.Height * i,
					arrowSize.Width,
					arrowSize.Height);
			}

			if (lanes % 2 == 1)
			{
				yield return new Rectangle(
					rect.X + (rect.Width - arrowSize.Width) / 2,
					rect.Height - arrowSize.Height * lanes / 2 - arrowSize.Height / 2,
					arrowSize.Width,
					arrowSize.Height);
			}
		}

		private void DrawDiagonalArrow(LaneInfo lane, Rectangle rect, Image arrow)
		{
			if (lane.Direction == LaneDirection.Backwards)
				arrow.RotateFlip(RotateFlipType.RotateNoneFlipY);

			if (!Small || lane.Direction == LaneDirection.Forward)
				rect.Y -= Small ? 2 : 4;
			if (lane.Direction == LaneDirection.Forward)
				rect.X += 1;

			var translate = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

			Graphics.TranslateTransform(translate.X, translate.Y);
			Graphics.RotateTransform(45F);

			Graphics.DrawImage(arrow, new Rectangle(new Point(-rect.Width / 2, -rect.Height / 2), rect.Size));

			Graphics.RotateTransform(-45F);
			Graphics.TranslateTransform(-translate.X, -translate.Y);
		}

		private void DrawBottomContent()
		{
			var speed = !string.IsNullOrWhiteSpace(Speed);
			var size = !string.IsNullOrWhiteSpace(RoadSize);
			var custom = !string.IsNullOrWhiteSpace(CustomText);

			var speedWidth = speed ? GetSpeedWidth() : 0;
			var roadSizeWidth = size ? GetRoadSizeWidth() : 0;

			if (speed)
			{
				if (Small)
					DrawSpeedSignSmall(new Rectangle(size ? (Width / 2 + roadSizeWidth / 2 + (Small ? 3 : 24)) : (Width - speedWidth) / 2, Height - 28, speedWidth, 30));
				else
					DrawSpeedSignLarge(new Rectangle(size ? (Width / 2 + roadSizeWidth / 2 + (Small ? 3 : 24)) : (Width - speedWidth) / 2, Height - 120, speedWidth, 120));
			}

			if (size)
				DrawRoadWidth(new Rectangle(0, Height - (Small ? 30 : 120), Width, (Small ? 30 : 120)));

			if (custom)
				DrawCustomText(new Rectangle(0, Height - (Small ? 30 : 120), size ? ((Width - roadSizeWidth) / 2 - (Small ? 3 : 24)) : speed ? ((Width - speedWidth) / 2 - (Small ? 3 : 24)) : Width, (Small ? 30 : 120)), !speed && !size);
		}

		private void DrawRoadWidth(Rectangle containerRect)
		{
			var sizeSize = (int)(Graphics.MeasureString(RoadSize + "m", new Font(Options.Current.SizeFont, Small ? 13F : 50F, FontStyle.Bold, GraphicsUnit.Pixel)).Width);
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

			Graphics.DrawString(RoadSize + "m", new Font(Options.Current.SizeFont, Small ? 13F : 50F, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.White, rect.Pad(0, 0, 0, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}

		private void DrawSpeedSignLarge(Rectangle containerRect)
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
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 32F : 40F, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(4, 8, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			}
			else if (RegionType == RegionType.USA)
			{
				var rect = containerRect.CenterR(56, 80);

				Graphics.FillRoundedRectangle(Brushes.White, rect, 7);
				Graphics.FillRoundedRectangle(Brushes.Black, rect.Pad(2), 7);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(4), 7);
				Graphics.DrawString("SPEED", new Font(Options.Current.SizeFont, 14.6f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(3-5, 8, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("LIMIT", new Font(Options.Current.SizeFont, 14.6f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(3-5, 22, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 26.6F : 33.3F, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(3, 0, 0, Speed.Length > 2 ? 1 : -3), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}
			else if (RegionType == RegionType.Canada)
			{
				var rect = containerRect.CenterR(68, 80);

				Graphics.FillRoundedRectangle(Brushes.White, rect, 7);
				Graphics.FillRoundedRectangle(Brushes.Black, rect.Pad(1), 7);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(3, 3, 3, 23), 7);
				Graphics.DrawString("MAXIMUM", new Font(Options.Current.SizeFont, 11f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(3, 7, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("km/ h", new Font(Options.Current.SizeFont, 16f, GraphicsUnit.Pixel), Brushes.White, rect.Pad(3, 28, 0, 0), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 32F : 37.3F, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(3, 0, 0, Speed.Length > 2 ? 16 : 12), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}

			Graphics.SmoothingMode = SmoothingMode.Default;
		}

		private void DrawSpeedSignSmall(Rectangle containerRect)
		{
			Graphics.SmoothingMode = SmoothingMode.HighQuality;

			if (RegionType == RegionType.Europe)
			{
				var rect = containerRect.CenterR(24, 24);

				Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				Graphics.FillEllipse(Brushes.White, rect);
				Graphics.FillEllipse(new SolidBrush(Color.FromArgb(207, 30, 22)), rect.Pad(1));
				Graphics.FillEllipse(Brushes.White, rect.Pad(4));
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 9.3F : 10.6F, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(1, 2, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			}
			else if (RegionType == RegionType.USA)
			{
				var rect = containerRect.CenterR(20, 24);

				Graphics.FillRoundedRectangle(Brushes.Black, rect, 2);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(1), 2);
				Graphics.DrawString("SPEED", new Font(Options.Current.SizeFont, 4.6f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(1-5, 2, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("LIMIT", new Font(Options.Current.SizeFont, 4.6f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(1-5, 6, -5, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 8.6F : 10.6F, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(3, 0, 0, Speed.Length > 2 ? 1 : -1), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}
			else if (RegionType == RegionType.Canada)
			{
				var rect = containerRect.CenterR(22, 24);

				Graphics.FillRoundedRectangle(Brushes.Black, rect, 2);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(1, 1, 1, 7), 2);
				Graphics.DrawString("MAXIMUM", new Font(Options.Current.SizeFont, 4f, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(0, 2, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("km/ h", new Font(Options.Current.SizeFont, 6f, GraphicsUnit.Pixel), Brushes.White, rect.Pad(1, 0, 0, -1), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 9.3F : 10.6F, GraphicsUnit.Pixel), Brushes.Black, rect.Pad(2, 0, 0, Speed.Length > 2 ? 5 : 4), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}

			Graphics.SmoothingMode = SmoothingMode.Default;
		}

		private void DrawCustomText(Rectangle containerRect, bool center)
		{
			Graphics.DrawString(CustomText, new Font("Segoe UI", Small ? 9F : 38F, FontStyle.Bold), new SolidBrush(Color.FromArgb(180, 0, 0, 0)), containerRect.Pad(0, Small ? 3 : 10, Small ? -1 : -3, Small ? -1 : -3), new StringFormat { Alignment = center ? StringAlignment.Center : StringAlignment.Far, LineAlignment = StringAlignment.Center });
			Graphics.DrawString(CustomText, new Font("Segoe UI", Small ? 9F : 38F, FontStyle.Bold), Brushes.White, containerRect.Pad(0, Small ? 3 : 10, 0, 0), new StringFormat { Alignment = center ? StringAlignment.Center : StringAlignment.Far, LineAlignment = StringAlignment.Center });
		}

		private int GetRoadSizeWidth()
		{
			var sizeSize = (int)Graphics.MeasureString(RoadSize + "m", new Font(Options.Current.SizeFont, Small ? 13F : 50F, FontStyle.Bold, GraphicsUnit.Pixel)).Width;

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

		private bool HasDoubleArrows(LaneInfo lane)
		{
			if (lane.IsFiller
				|| lane.Type == LaneType.Parking
				|| lane.Direction == LaneDirection.None
				|| lane.Type == LaneType.Highway)
			{
				return false;
			}

			return lane.Lanes > 1;
		}

		private int GetArrowRows(LaneInfo lane)
		{
			if (lane.IsFiller || lane.Direction == LaneDirection.None || lane.Type == LaneType.Highway)
			{
				return 0;
			}

			if (lane.Type == LaneType.Pedestrian && lane.Lanes < 2 && (lane.Direction == LaneDirection.Both || lane.Direction == LaneDirection.None))
			{
				return 0;
			}

			if (lane.Type == LaneType.Parking)
			{
				return lane.Lanes > 1 ? 1 : 0;
			}

			if (lane.Lanes == 1 && lane.Direction == LaneDirection.Both)
			{
				return 2;
			}

			return (int)Math.Ceiling(lane.Lanes / 2F);
		}
	}
}
