using Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ThumbnailMaker.Domain;

namespace ThumbnailMaker.Handlers
{
	public static class Utilities
	{
		public static string GetRoadDescription(List<LaneInfo> lanes, string size, string bufferSize, string speedLimit, bool usa)
		{
			var skip = false;
			var oneWay = IsOneWay(lanes);
			var asymetrical = lanes.Where(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Backwards).Sum(x => x.Lanes) != lanes.Where(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Forward).Sum(x => x.Lanes) && lanes.Any(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Backwards && x.Lanes > 0);
			var laneDescriptors = new List<string>();

			foreach (var lane in lanes)
			{
				if (skip || lane.Type == LaneType.Pedestrian)
					continue;

				var types = LaneInfo.GetLaneTypes(lane.Type).Select(x => x.ToString());
				var name = types.Count() > 1 ? $"Shared {types.ListStrings(" & ")}" : types.First();

				if (lane.Type < LaneType.Trees)
					laneDescriptors.Add(lane.Lanes > 3 ? "Separator" : "Median");
				else if (lane.Direction == LaneDirection.Both && lane.Type != LaneType.Parking)
					laneDescriptors.Add($"2W {lane.Lanes}L {name}");
				else if (lane.Lanes > 0 && lane.Type != LaneType.Parking)
					laneDescriptors.Add($"{lane.Lanes}L{lane.Direction.Switch(LaneDirection.Backwards, "B", LaneDirection.Forward, "F", "")} {name}");
				else
					laneDescriptors.Add(name);
			}

			if (string.IsNullOrWhiteSpace(size))
				size = CalculateRoadSize(lanes, bufferSize);

			if (string.IsNullOrWhiteSpace(speedLimit))
				speedLimit = DefaultSpeedSign(lanes, usa);

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
			if (lanes.Any(x => (x.Type & (LaneType.Car | LaneType.Bus | LaneType.Highway)) != 0))
				return usa ? "25" : "40";

			return string.Empty;
		}

		public static string CalculateRoadSize(List<LaneInfo> lanes, string bufferSize)
		{
			if (lanes.Count == 0)
				return string.Empty;

			var size = (bufferSize.SmartParseF() * 2 + lanes.Sum(x => LaneInfo.GetLaneTypes(x.Type).Max(y => Utilities.GetLaneWidth(y, x))));

			return (Math.Ceiling(size * 2) / 2F).ToString("0.#");
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

			if (bike != null)
			{
				return bike.Direction != LaneDirection.Both && lanes
					.Where(x => x.Type.HasFlag(LaneType.Bike))
					.All(x => x.Direction == bike.Direction);
			}

			return null;
		}

		public static float GetLaneWidth(LaneType type, LaneInfo lane)
		{
			if (lane.CustomWidth > 0)
				return lane.CustomWidth;

			switch (type)
			{
				case LaneType.Empty:
				case LaneType.Grass:
				case LaneType.Pavement:
				case LaneType.Gravel:
					return 0.03F * lane.FillerSize;

				case LaneType.Trees:
					return 0.04F * lane.FillerSize;

				case LaneType.Tram:
				case LaneType.Car:
				case LaneType.Trolley:
				case LaneType.Emergency:
					return 3F * lane.Lanes;

				case LaneType.Pedestrian:
				case LaneType.Bike:
					return 2F * lane.Lanes;

				case LaneType.Parking:
					return lane.DiagonalParking ? 4F : lane.HorizontalParking ? 5F : 2F;

				case LaneType.Highway:
				case LaneType.Bus:
				case LaneType.Train:
					return 4F * lane.Lanes;
			}

			return 3F * lane.Lanes;
		}
	}
}
