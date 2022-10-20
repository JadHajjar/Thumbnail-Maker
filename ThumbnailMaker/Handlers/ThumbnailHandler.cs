using Extensions;

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
		private bool _small;

		public bool Small
		{
			get => _small; set
			{
				_small = value;
				Width = Small ? 109 : 512;
				Height = Small ? 100 : 512;
			}
		}

		public bool HighWay { get; set; }
		public bool Europe { get; set; }
		public bool USA { get; set; }
		public bool Canada { get; set; }
		public string Speed { get; set; }
		public string RoadSize { get; set; }
		public string CustomText { get; set; }
		public List<LaneInfo> Lanes { get; set; }

		public int Width { get; private set; }
		public int Height { get; private set; }
		public Graphics Graphics { get; }

		public ThumbnailHandler(Graphics graphics)
		{
			Graphics = graphics;
		}

		public void Draw()
		{
			Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

			using (var logo = ResourceManager.Logo(Small))
			{
				var idealWidth = Lanes.Sum(x => x.Width);

				if (idealWidth > Width)
				{
					while ((idealWidth = Lanes.Sum(x => x.Width)) > Width)
					{
						Lanes.Foreach(x => x.Width -= Width / 100);

						if (idealWidth == Lanes.Sum(x => x.Width))
							break;
					}
				}

				var arrowHeight = ResourceManager.Arrow(Small).Width;
				var dualArrows = Lanes.Any(x => x.Lanes > 1 || x.Direction == LaneDirection.Both);
				var maxWidth = Lanes.Max(x => x.Width);

				arrowHeight = Math.Min(arrowHeight, (maxWidth - (dualArrows ? 2 : 1) * (Small ? 2 : 8)) / (dualArrows ? 2 : 1));

				var arrowsHeight = GetArrowsHeight(arrowHeight);
				var availableSpace = GetAvailableSpace(logo == null ? 0 : (logo.Height + 2)).Pad(0, arrowsHeight, 0, 0);
				var xIndex = (Width - idealWidth) / 2;

				for (var i = 0; i < Lanes.Count; i++)
				{
					var lane = Lanes[i];
					var rect = new Rectangle(xIndex, 0, lane.Width, Height);

					DrawLane(lane, rect, availableSpace, arrowsHeight);

					xIndex += lane.Width;
				}

				if (logo != null)
					Graphics.DrawImage(logo, new Rectangle(new Point((Width - logo.Width) / 2, 0), logo.Size));

				DrawBottomLinesAndLength();
			}
		}

		private int GetArrowsHeight(int arrowHeight)
		{
			if (Lanes.Count == 0)
				return 0;

			var maxLanes = Lanes.Max(l => l.IsFiller || l.Direction == LaneDirection.None ? 0 : l.Lanes);

			if (maxLanes % 2 == 1)
				return arrowHeight * maxLanes / 2 + arrowHeight / 2 + 3;

			return arrowHeight * maxLanes / 2 + 3;
		}

		private void DrawLane(LaneInfo lane, Rectangle rect, Rectangle availableSpace, int arrowSize)
		{
			Graphics.FillRectangle(lane.Brush(Small), rect);

			var icons = lane.Icons(Small);

			if (icons.Count == 0 || lane.Width < 1)
				return;

			var arrow = ResourceManager.Arrow(Small);

			//if (HighWay)
			//	rect = rect.Pad(0, Small ? 16 : 50, 0, 0);
			//else
			//	rect = rect.Pad(0, Small ? 32 : 120, 0, 0);

			rect = new Rectangle(rect.X, availableSpace.Y, rect.Width, availableSpace.Height);

			var y = rect.Center(0, icons.Sum(i => i.Height)).Y;

			for (var i = 0; i < icons.Count; i++)
			{
				var icon = icons[i];

				if (icon.Width > lane.Width)
					icon = new Bitmap(icon, new Size(lane.Width, icon.Height * lane.Width / icon.Width));

				if (lane.Type == LaneType.Highway && lane.Direction == LaneDirection.Backwards)
					icon.RotateFlip(RotateFlipType.RotateNoneFlipY);

				Graphics.DrawImage(icon, new Rectangle(new Point(rect.X + (rect.Width - icon.Width) / 2, y), icon.Size));

				y += icon.Height;
			}

			if (lane.Direction == LaneDirection.None || lane.Type == LaneType.Highway)
				return;

			//rect = rect.Pad(0, (icon.Width - icon.Height), 0, 0);


			if (arrow == null)
				return;

			using (arrow)
			{
				if (lane.Direction == LaneDirection.Backwards || lane.Direction == LaneDirection.Both)
					arrow.RotateFlip(RotateFlipType.RotateNoneFlipY);

				foreach (var arrowRect in GetDirectionArrowRects(lane, new Rectangle(rect.X, 0, rect.Width, rect.Center(0, icons.Sum(i => i.Height)).Y), arrowSize))
				{
					Graphics.DrawImage(arrow, arrowRect);

					if (lane.Direction == LaneDirection.Both)
						arrow.RotateFlip(RotateFlipType.RotateNoneFlipY);
				}
			}
		}

		private IEnumerable<Rectangle> GetDirectionArrowRects(LaneInfo lane, Rectangle rect, int iconSize)
		{
			var mainIconSize = Small ? 16 : 96;

			for (var i = 1; i <= lane.Lanes / 2; i++)
			{
				yield return new Rectangle(
					rect.X + (rect.Width / 2 - iconSize) / 2,
					rect.Height - iconSize * i - 3,
					iconSize,
					iconSize);

				yield return new Rectangle(
					rect.X + rect.Width / 2 + (rect.Width / 2 - iconSize) / 2,
					rect.Height - iconSize * i - 3,
					iconSize,
					iconSize);
			}

			if (lane.Lanes % 2 == 1)
			{
				yield return new Rectangle(
					rect.X + (rect.Width - iconSize) / 2,
					rect.Height - iconSize * lane.Lanes / 2 - iconSize / 2 - 3,
					iconSize,
					iconSize);
			}
		}

		private Rectangle GetAvailableSpace(int startingHeight)
		{
			var widthBuffer = Small ? 8 : 32;

			if (string.IsNullOrWhiteSpace(RoadSize) && string.IsNullOrWhiteSpace(Speed) && string.IsNullOrWhiteSpace(CustomText))
				return new Rectangle(0, startingHeight + widthBuffer / 2, Width, Height - (startingHeight + widthBuffer / 2));

			return new Rectangle(0, startingHeight + widthBuffer / 2, Width, Height - (startingHeight + widthBuffer / 2) - (Small ? 30 : 100));
		}

		private void DrawBottomLinesAndLength()
		{
			var speed = !string.IsNullOrWhiteSpace(Speed);
			var size = !string.IsNullOrWhiteSpace(RoadSize);
			var custom = !string.IsNullOrWhiteSpace(CustomText);

			var speedWidth = speed ? GetSpeedWidth() : 0;
			var roadSizeWidth = size ? GetRoadSizeWidth() : 0;

			if (speed)
			{
				if (Small)
					DrawSpeedSignSmall(new Rectangle(size ? (Width / 2 + roadSizeWidth / 2 + (Small ? 3 : 24)) : (Width - speedWidth) / 2, Height - 30, speedWidth, 30));
				else
					DrawSpeedSignLarge(new Rectangle(size ? (Width / 2 + roadSizeWidth / 2 + (Small ? 3 : 24)) : (Width - speedWidth) / 2, Height - 120, speedWidth, 120));
			}

			if (size)
				DrawRoadWidth(new Rectangle(0, Height - (Small ? 30 : 120), Width, (Small ? 30 : 120)));

			if (custom)
				DrawCustomText(new Rectangle(0, Height - (Small ? 30 : 120), size ?( (Width - roadSizeWidth) / 2- (Small ? 3 : 24)) : speed ? ((Width - speedWidth) / 2- (Small ? 3 : 24)) : Width, (Small ? 30 : 120)), !speed &&!size);
		}

		private void DrawCustomText(Rectangle containerRect, bool center)
		{
			Graphics.DrawString(CustomText, new Font("Segoe UI", Small ? 9F : 38F, FontStyle.Bold), new SolidBrush(Color.FromArgb(180, 0, 0, 0)), containerRect.Pad(0, Small ? 3 : 10, Small ? -1 : -3, Small ? -1 : -3), new StringFormat { Alignment = center? StringAlignment.Center: StringAlignment.Far, LineAlignment = StringAlignment.Center });
			Graphics.DrawString(CustomText, new Font("Segoe UI", Small ? 9F : 38F, FontStyle.Bold), Brushes.White, containerRect.Pad(0, Small ? 3 : 10, 0, 0), new StringFormat { Alignment = center? StringAlignment.Center: StringAlignment.Far, LineAlignment = StringAlignment.Center });
		}

		private int GetRoadSizeWidth()
		{
			var sizeSize = (int)Graphics.MeasureString(RoadSize + "m", new Font(Options.Current.SizeFont, Small ? 9.75F : 38F, FontStyle.Bold)).Width;

			return sizeSize + (Small ? 4 : 38);
		}

		private void DrawRoadWidth(Rectangle containerRect)
		{
			var sizeSize = (int)(Graphics.MeasureString(RoadSize + "m", new Font(Options.Current.SizeFont, Small ? 9.75F : 38F, FontStyle.Bold)).Width);
			var rect = containerRect.CenterR(sizeSize + (Small ? 4 : 38), Small ? 20 : 80);

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

			Graphics.DrawString(RoadSize + "m", new Font(Options.Current.SizeFont, Small ? 9.75F : 38F, FontStyle.Bold), Brushes.White, rect.Pad(0, 0, 0, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
		}

		private int GetSpeedWidth()
		{
			if (Small)
			{
				if (Europe)
					return 24;

				if (USA)
					return 20;

				if (Canada)
					return 22;
			}
			else
			{
				if (Europe)
					return 100;

				if (USA)
					return 70;

				if (Canada)
					return 85;
			}

			return 0;
		}

		private void DrawSpeedSignLarge(Rectangle containerRect)
		{
			Graphics.SmoothingMode = SmoothingMode.HighQuality;

			if (Europe)
			{
				var rect = containerRect.CenterR(80, 80);

				Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				Graphics.FillEllipse(Brushes.Black, rect);
				Graphics.FillEllipse(Brushes.White, rect.Pad(1));
				Graphics.FillEllipse(new SolidBrush(Color.FromArgb(207, 30, 22)), rect.Pad(3));
				Graphics.FillEllipse(Brushes.White, rect.Pad(12));
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 24F : 30F), Brushes.Black, rect.Pad(4, 8, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			}
			else if (USA)
			{
				var rect = containerRect.CenterR(56, 80);

				Graphics.FillRoundedRectangle(Brushes.White, rect, 7);
				Graphics.FillRoundedRectangle(Brushes.Black, rect.Pad(2), 7);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(4), 7);
				Graphics.DrawString("SPEED", new Font(Options.Current.SizeFont, 11f), Brushes.Black, rect.Pad(3, 8, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("LIMIT", new Font(Options.Current.SizeFont, 11f), Brushes.Black, rect.Pad(3, 22, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 20F : 25F), Brushes.Black, rect.Pad(3, 0, 0, Speed.Length > 2 ? 1 : -3), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}
			else if (Canada)
			{
				var rect = containerRect.CenterR(68, 80);

				Graphics.FillRoundedRectangle(Brushes.White, rect, 7);
				Graphics.FillRoundedRectangle(Brushes.Black, rect.Pad(1), 7);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(3, 3, 3, 23), 7);
				Graphics.DrawString("MAXIMUM", new Font(Options.Current.SizeFont, 8.25f), Brushes.Black, rect.Pad(3, 7, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("km/ h", new Font(Options.Current.SizeFont, 12f), Brushes.White, rect.Pad(3, 28, 0, 0), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 24F : 28F), Brushes.Black, rect.Pad(3, 0, 0, Speed.Length > 2 ? 16 : 12), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}

			Graphics.SmoothingMode = SmoothingMode.Default;
		}

		private void DrawSpeedSignSmall(Rectangle containerRect)
		{
			Graphics.SmoothingMode = SmoothingMode.HighQuality;

			if (Europe)
			{
				var rect = containerRect.CenterR(24, 24);

				Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				Graphics.FillEllipse(Brushes.White, rect);
				Graphics.FillEllipse(new SolidBrush(Color.FromArgb(207, 30, 22)), rect.Pad(1));
				Graphics.FillEllipse(Brushes.White, rect.Pad(4));
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 7F : 8F), Brushes.Black, rect.Pad(1, 2, 0, 0), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
			}
			else if (USA)
			{
				var rect = containerRect.CenterR(20, 24);

				Graphics.FillRoundedRectangle(Brushes.Black, rect, 2);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(1), 2);
				Graphics.DrawString("SPEED", new Font(Options.Current.SizeFont, 3.5f), Brushes.Black, rect.Pad(1, 2, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("LIMIT", new Font(Options.Current.SizeFont, 3.5f), Brushes.Black, rect.Pad(1, 6, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 6.5F : 8F), Brushes.Black, rect.Pad(3, 0, 0, Speed.Length > 2 ? 1 : -1), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}
			else if (Canada)
			{
				var rect = containerRect.CenterR(22, 24);

				Graphics.FillRoundedRectangle(Brushes.Black, rect, 2);
				Graphics.FillRoundedRectangle(Brushes.White, rect.Pad(1, 1, 1, 7), 2);
				Graphics.DrawString("MAXIMUM", new Font(Options.Current.SizeFont, 3f), Brushes.Black, rect.Pad(0, 2, 0, 0), new StringFormat { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center });
				Graphics.DrawString("km/ h", new Font(Options.Current.SizeFont, 4.5f), Brushes.White, rect.Pad(1, 0, 0, -1), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
				Graphics.DrawString(Speed, new Font(Options.Current.SizeFont, Speed.Length > 2 ? 7F : 8F), Brushes.Black, rect.Pad(2, 0, 0, Speed.Length > 2 ? 5 : 4), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}

			Graphics.SmoothingMode = SmoothingMode.Default;
		}
	}
}
