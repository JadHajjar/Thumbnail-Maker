using Extensions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

using ThumbnailMaker.Domain;

namespace ThumbnailMaker.Handlers
{
	public static class Utilities
	{
		public static string Folder => Directory.GetParent(Application.ExecutablePath).FullName;

		public static string GetRoadDescription(List<LaneInfo> lanes, string size, string bufferSize, string speedLimit, bool usa)
		{
			var skip = false;
			var oneWay = IsOneWay(lanes);
			var asymetrical = lanes.Where(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Backwards).Sum(x => x.Lanes) != lanes.Where(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Forward).Sum(x => x.Lanes) && lanes.Any(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Backwards && x.Lanes > 0);
			var laneDescriptors = new List<string>();

			foreach (var lane in lanes)
			{
				if (skip || (lane.Type == LaneType.Pedestrian && (lane == lanes.First() || lane == lanes.Last())))
				{
					continue;
				}

				var types = LaneInfo.GetLaneTypes(lane.Type).Select(x => x.ToString());
				var name = types.Count() > 1 ? $"Shared {types.ListStrings(" & ")}" : types.First();

				if (lane.Type < LaneType.Trees)
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
				else if (lane.Direction == LaneDirection.Both && lane.Type != LaneType.Parking)
				{
					laneDescriptors.Add($"2W {lane.Lanes}L {name}");
				}
				else if (lane.Lanes > 0 && lane.Type != LaneType.Parking)
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
				size = CalculateRoadSize(lanes, bufferSize);
			}

			if (string.IsNullOrWhiteSpace(speedLimit))
			{
				speedLimit = DefaultSpeedSign(lanes, usa);
			}

			var info = (size.Length == 0 ? "" : $"{size}m") +
				(speedLimit.Length == 0 ? "" : $" - {speedLimit}{usa.If("mph", "km/h")}");

			var desc = $"Blank {(asymetrical ? "Asymmetrical " : oneWay.Switch(true, "One-Way ", false, "Two-Way ", string.Empty))}{lanes.Any(x => x.Type.HasFlag(LaneType.Bike)).If("Bike ")}Road.  " +
				laneDescriptors.WhereNotEmpty().ListStrings(" + ") +
				info.IfEmpty("", $"  ({info})") +
				"  This road comes with no markings, use Intersection Marking Tool to mark it.";
			return desc;
		}

		public static string DefaultSpeedSign(List<LaneInfo> lanes, bool usa)
		{
			return lanes.Any(x => (x.Type & (LaneType.Car | LaneType.Bus | LaneType.Highway)) != 0) ? usa ? "25" : "40" : string.Empty;
		}

		public static string CalculateRoadSize(List<LaneInfo> lanes, string bufferSize)
		{
			lanes = new List<LaneInfo>(lanes);

			if (lanes.Count == 0)
			{
				return string.Empty;
			}

			var size = (bufferSize.SmartParseF() * 2) + lanes.Sum(x => LaneInfo.GetLaneTypes(x.Type).Max(y => GetLaneWidth(y, x)));

			return Math.Round(size, 1).ToString("0.#");
		}

		public static bool? IsOneWay(List<LaneInfo> lanes)
		{
			var car = lanes.FirstOrDefault(x => x.Type.HasFlag(LaneType.Car));
			var bus = lanes.FirstOrDefault(x => x.Type.HasFlag(LaneType.Bus));
			var bike = lanes.FirstOrDefault(x => x.Type.HasFlag(LaneType.Bike));

			if (car != null)
			{
				return car.Direction != LaneDirection.Both && lanes
					.Where(x => x.Type.HasFlag(LaneType.Car))
					.All(x => x.Direction == car.Direction);
			}

			if (bus != null)
			{
				return bus.Direction != LaneDirection.Both && lanes
					.Where(x => x.Type.HasFlag(LaneType.Bus))
					.All(x => x.Direction == bus.Direction);
			}

			return bike != null
				? bike.Direction != LaneDirection.Both && lanes
					.Where(x => x.Type.HasFlag(LaneType.Bike))
					.All(x => x.Direction == bike.Direction)
				: (bool?)null;
		}

		public static float GetLaneWidth(LaneType type, LaneInfo lane)
		{
			if (lane.CustomWidth > 0)
			{
				return Math.Max(1, lane.Lanes) * lane.CustomWidth;
			}

			switch (type)
			{
				case LaneType.Empty:
				case LaneType.Grass:
				case LaneType.Pavement:
				case LaneType.Gravel:
				case LaneType.Trees:
					return (float)Math.Round(Math.Ceiling(0.04 * LaneSizeOptions.LaneSizes[type] * lane.FillerSize) / 4, 2);

				case LaneType.Parking:
					return
						lane.HorizontalParking ? LaneSizeOptions.LaneSizes.DiagonalParkingSize :
						lane.DiagonalParking || lane.InvertedDiagonalParking ? LaneSizeOptions.LaneSizes.DiagonalParkingSize :
						LaneSizeOptions.LaneSizes[type];
			}

			return Math.Max(1, lane.Lanes) * LaneSizeOptions.LaneSizes[type];
		}
	}
}
