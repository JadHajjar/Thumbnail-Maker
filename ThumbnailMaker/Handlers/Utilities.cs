using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

using ThumbnailMaker.Domain;

using static System.Windows.Forms.AxHost;

namespace ThumbnailMaker.Handlers
{
	public static class Utilities
	{
		public static string Folder => Directory.GetParent(Application.ExecutablePath).FullName;

		public static string GetRoadDescription(List<LaneInfo> lanes, string size, string bufferSize, string speedLimit, bool usa)
		{
			var skip = false;
			var oneWay = IsOneWay(lanes);
			var asymetrical = lanes.Where(x => x.Class.HasFlag(LaneClass.Car) && x.Direction == LaneDirection.Backwards).Sum(x => x.Lanes) != lanes.Where(x => x.Class.HasFlag(LaneClass.Car) && x.Direction == LaneDirection.Forward).Sum(x => x.Lanes) && lanes.Any(x => x.Class.HasFlag(LaneClass.Car) && x.Direction == LaneDirection.Backwards && x.Lanes > 0);
			var laneDescriptors = new List<string>();

			foreach (var lane in lanes)
			{
				if (skip || (lane.Class == LaneClass.Pedestrian && (lane == lanes.First() || lane == lanes.Last())))
				{
					continue;
				}

				var types = LaneInfo.GetLaneTypes(lane.Class).Select(x => x.ToString());
				var name = types.Count() > 1 ? $"Shared {types.ListStrings(" & ")}" : types.First();

				if (lane.Class == LaneClass.Filler)
				{
					laneDescriptors.Add(lane.Lanes > 3 ? "Separator" : "Median");
				}
				else if (lane.InvertedDiagonalParking)
				{
					laneDescriptors.Add("Inverted Diagonal Parking");
				}
				else if (lane.DiagonalParking)
				{
					laneDescriptors.Add("Diagonal Parking");
				}
				else if (lane.HorizontalParking)
				{
					laneDescriptors.Add("Horizontal Parking");
				}
				else if (lane.Direction == LaneDirection.Both && lane.Class != LaneClass.Parking)
				{
					laneDescriptors.Add($"2W {lane.Lanes}L {name}");
				}
				else if (lane.Lanes > 0 && lane.Class != LaneClass.Parking)
				{
					laneDescriptors.Add($"{lane.Lanes}L{lane.Direction.Switch(LaneDirection.Backwards, "B", LaneDirection.Forward, "F", "")} {name}");
				}
				else
				{
					laneDescriptors.Add(name);
				}
			}

			if (string.IsNullOrWhiteSpace(size))
			{
				size = CalculateRoadSize(lanes, bufferSize).If(x => x == 0F, x => "", x => x.ToString("0.#"));
			}

			if (string.IsNullOrWhiteSpace(speedLimit))
			{
				speedLimit = DefaultSpeedSign(lanes, usa).If(x => x == 0, x => "", x => x.ToString());
			}

			var info = (size.Length == 0 ? "" : $"{size}m") +
				(speedLimit.Length == 0 ? "" : $" - {speedLimit}{usa.If("mph", "km/h")}");

			var desc = $"Blank {(asymetrical ? "Asymmetrical " : oneWay.Switch(true, "One-Way ", false, "Two-Way ", string.Empty))}{lanes.Any(x => x.Class.HasFlag(LaneClass.Bike)).If("Bike ")}Road.  " +
				laneDescriptors.WhereNotEmpty().ListStrings(" + ") +
				info.IfEmpty("", $"  ({info})") +
				"  This road comes with no markings, use Intersection Marking Tool to mark it.";
			return desc;
		}

		public static int DefaultSpeedSign(List<LaneInfo> lanes, bool usa)
		{
			return lanes.Any(x => (x.Class & (LaneClass.Car | LaneClass.Bus | LaneClass.Trolley | LaneClass.Emergency | LaneClass.Tram)) != 0) ? usa ? 25 : 40 : 0;
		}

		public static float CalculateRoadSize(List<LaneInfo> lanes, string bufferSize)
		{
			lanes = new List<LaneInfo>(lanes);

			if (lanes.Count == 0)
			{
				return 0;
			}

			try
			{
				var size = (bufferSize.SmartParseF() * 2) + lanes.Sum(x => LaneInfo.GetLaneTypes(x.Class).Max(y => GetLaneWidth(y, x)));

				return (float)Math.Round(size, 1);
			}
			catch { throw; }
		}

		public static bool? IsOneWay(List<LaneInfo> lanes)
		{
			var car = lanes.FirstOrDefault(x => x.Class.HasFlag(LaneClass.Car));
			var bus = lanes.FirstOrDefault(x => x.Class.HasFlag(LaneClass.Bus));
			var bike = lanes.FirstOrDefault(x => x.Class.HasFlag(LaneClass.Bike));

			if (car != null)
			{
				return car.Direction != LaneDirection.Both && lanes
					.Where(x => x.Class.HasFlag(LaneClass.Car))
					.All(x => x.Direction == car.Direction);
			}

			if (bus != null)
			{
				return bus.Direction != LaneDirection.Both && lanes
					.Where(x => x.Class.HasFlag(LaneClass.Bus))
					.All(x => x.Direction == bus.Direction);
			}

			return bike != null
				? bike.Direction != LaneDirection.Both && lanes
					.Where(x => x.Class.HasFlag(LaneClass.Bike))
					.All(x => x.Direction == bike.Direction)
				: (bool?)null;
		}

		public static float GetLaneWidth(LaneClass type, LaneInfo lane)
		{
			if (lane.CustomWidth > 0)
			{
				return Math.Max(1, lane.Lanes) * lane.CustomWidth;
			}

			switch (type)
			{
				case LaneClass.Empty:
					return 1;

				case LaneClass.Filler:
					return (float)Math.Round(Math.Ceiling(0.04 * LaneSizeOptions.LaneSizes[type] * lane.FillerSize) / 4, 2);

				case LaneClass.Parking:
					return
						lane.HorizontalParking ? LaneSizeOptions.LaneSizes.HorizontalParkingSize :
						lane.DiagonalParking || lane.InvertedDiagonalParking ? LaneSizeOptions.LaneSizes.DiagonalParkingSize :
						LaneSizeOptions.LaneSizes[type];
			}

			return Math.Max(1, lane.Lanes) * LaneSizeOptions.LaneSizes[type];
		}

		public static void DrawIcon(this Graphics g, Image img, Rectangle rect, Size? size = null)
		{
			if (img == null)
				return;

			rect = rect.CenterR(size??img.Size);

			if (img.Width >= rect.Width || img.Height >= rect.Height)
			{
				if (img.Width >= img.Height)
				{
					var newHeight = img.Height * rect.Width / img.Width;

					rect.Y += (rect.Height - newHeight) / 2;
					rect.Height = newHeight;
				}

				if (img.Width <= img.Height)
				{
					var newWidth = img.Width * rect.Height / img.Height;

					rect.X += (rect.Width - newWidth) / 2;
					rect.Width = newWidth;
				}
			}
			else
				rect = rect.CenterR(img.Size);

			g.DrawImage(img, rect);
		}

		public static bool IsCompatible(this LaneDecorationStyle deco, LaneClass laneClass)
		{
			foreach (var item in LaneInfo.GetLaneTypes(laneClass))
			{
				switch (item)
				{
					case LaneClass.Filler:
					case LaneClass.Curb:
						if (deco.AnyOf(LaneDecorationStyle.StreetLight, LaneDecorationStyle.DoubleStreetLight, LaneDecorationStyle.Filler))
							return false;
						break;

					case LaneClass.Pedestrian:
					case LaneClass.Bike:
					case LaneClass.Car:
					case LaneClass.Tram:
					case LaneClass.Bus:
					case LaneClass.Trolley:
					case LaneClass.Emergency:
					case LaneClass.Train:
					case LaneClass.Parking:
						if (!deco.AnyOf(LaneDecorationStyle.None, LaneDecorationStyle.Filler, LaneDecorationStyle.Grass, LaneDecorationStyle.Gravel, LaneDecorationStyle.Pavement))
							return false;
						break;
				}
			}

			return laneClass != LaneClass.Empty;
		}
	}
}
