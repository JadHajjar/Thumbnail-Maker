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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ThumbnailMaker.Handlers
{
	public static class Utilities
	{
		public static string Folder => Directory.GetParent(Application.ExecutablePath).FullName;

		public static string ExportRoad(RoadInfo road, string fileName = null)
		{
			var appdata = Options.Current.ExportFolder.IfEmpty(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
				, "Colossal Order", "Cities_Skylines", "BlankRoadBuilder", "Roads"));
			
			Directory.CreateDirectory(appdata);

			road.Version = LegacyUtil.CURRENT_VERSION;
			road.SpeedLimit = road.SpeedLimit.If(0, DefaultSpeedSign(road.RoadType, road.RegionType == RegionType.USA));
			road.SmallThumbnail = getImage(true, false);
			road.LargeThumbnail = getImage(false, false);
			road.TooltipImage = getImage(true, true);

			if (road.LHT)
				road.Lanes.Reverse();

			var guid = Guid.NewGuid().ToString();
			var xML = new System.Xml.Serialization.XmlSerializer(typeof(RoadInfo));

			using (var stream = File.Create(fileName ?? Path.Combine(appdata, $"{guid}.xml")))
				xML.Serialize(stream, road);

			return fileName ?? Path.Combine(appdata, $"{guid}.xml");

			byte[] getImage(bool small, bool toolTip)
			{
				var width = toolTip ? 492 : small ? 109 : 512;
				var height = toolTip ? 147 : small ? 100 : 512;

				using (var img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
				using (var g = Graphics.FromImage(img))
				{
					new ThumbnailHandler(g, small, toolTip)
					{
						RoadWidth = road.RoadWidth,
						CustomText = road.CustomText,
						BufferSize = Math.Max(0, road.BufferWidth),
						RegionType = road.RegionType,
						RoadType = road.RoadType,
						Speed = road.SpeedLimit,
						Lanes = road.Lanes.Select(x => new ThumbnailLaneInfo(x)).ToList()
					}.Draw();

					return (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]));
				}
			}
		}

		public static string GetRoadDescription<T>(List<T> lanes, RoadType roadType, string size, float bufferSize, int speedLimit, bool usa) where T : LaneInfo
		{
			var skip = false;
			var oneWay = IsOneWay(lanes);
			var asymetrical = lanes.Where(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Backwards).Count() != lanes.Where(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Forward).Count() && lanes.Any(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Backwards);
			var laneDescriptors = new List<string>();

			foreach (var lane in lanes)
			{
				if (skip || (lane.Type == LaneType.Pedestrian && (lane == lanes.First() || lane == lanes.Last())))
				{
					continue;
				}

				var types = lane.Type.GetValues().Select(x => x.ToString());
				var name = types.Count() > 1 ? $"Shared {types.ListStrings(" & ")}" : types.First();

				if (lane.Type == LaneType.Filler)
				{
					laneDescriptors.Add(lane.LaneWidth <= 1 ? "Separator" : "Median");
				}
				else if (lane.Type == LaneType.Parking && lane.ParkingAngle != ParkingAngle.Vertical)
				{
					laneDescriptors.Add(lane.ParkingAngle.ToString().FormatWords() + " Parking");
				}
				else if (lane.Direction == LaneDirection.Both && lane.Type != LaneType.Parking)
				{
					laneDescriptors.Add($"2W {name}");
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

			if (speedLimit > 0)
			{
				speedLimit = DefaultSpeedSign(lanes, roadType, usa);
			}

			var info = (size.Length == 0 ? "" : $"{size}m") +
				(speedLimit == 0 ? "" : $" - {speedLimit}{usa.If("mph", "km/h")}");

			var desc = $"Blank {(asymetrical ? "Asymmetrical " : oneWay.Switch(true, "One-Way ", false, "Two-Way ", string.Empty))}{lanes.Any(x => x.Type.HasFlag(LaneType.Bike)).If("Bike ")}Road.  " +
				laneDescriptors.WhereNotEmpty().ListStrings(" + ") +
				info.IfEmpty("", $"  ({info})") +
				"  This road comes with no markings, use Intersection Marking Tool to mark it.";
			return desc;
		}

		public static int DefaultSpeedSign<T>(List<T> lanes, RoadType type, bool mph) where T : LaneInfo
		{
			return lanes.Any(x => (x.Type & (LaneType.Car | LaneType.Bus | LaneType.Trolley | LaneType.Emergency | LaneType.Tram)) != 0)
				? DefaultSpeedSign(type, mph) : 0;
		}

		public static int DefaultSpeedSign(RoadType type, bool mph)
		{
			switch (type)
			{
				case RoadType.Road:
					return mph ? 25 : 40;
				case RoadType.Highway:
					return mph ? 55 : 80;
				case RoadType.Pedestrian:
					return mph ? 10 : 20;
				case RoadType.Flat:
					return mph ? 15 : 30;
			}

			return 0;
		}

		public static float CalculateRoadSize<T>(List<T> lanes, float bufferSize) where T : LaneInfo
		{
			if (lanes.Count == 0)
			{
				return 0;
			}

			var size = Math.Max(0, bufferSize * 2) + lanes.Sum(x => x.LaneWidth);

			return (float)Math.Round(size, 1);
		}

		public static bool? IsOneWay<T>(List<T> lanes) where T : LaneInfo
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

		public static bool IsCompatible(this LaneDecoration deco, LaneType laneClass)
		{
			foreach (var item in laneClass.GetValues())
			{
				switch (item)
				{
					case LaneType.Filler:
					case LaneType.Curb:
						if (deco.AnyOf(LaneDecoration.Filler))
							return false;
						break;

					case LaneType.Pedestrian:
						if (!deco.AnyOf(LaneDecoration.StreetLight, LaneDecoration.DoubleStreetLight, LaneDecoration.TransitStop, LaneDecoration.None, LaneDecoration.Filler, LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement))
							return false;
						break;

					case LaneType.Bike:
					case LaneType.Car:
					case LaneType.Tram:
					case LaneType.Bus:
					case LaneType.Trolley:
					case LaneType.Emergency:
					case LaneType.Train:
					case LaneType.Parking:
						if (!deco.AnyOf(LaneDecoration.None, LaneDecoration.Filler, LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement))
							return false;
						break;
				}
			}

			return laneClass != LaneType.Empty;
		}

		public static IEnumerable<T> GetValues<T>(this T @enum) where T : Enum
		{
			if (@enum.Equals(default(T)))
			{
				yield return @enum;
				yield break;
			}	

			foreach (T value in Enum.GetValues(typeof(T)))
			{
				if (!value.Equals(default(T)) && @enum.HasFlag(value))
					yield return value;
			}
		}

		public static bool HasAnyFlag<T>(this T @enum, params T[] values) where T : Enum
		{
			foreach (var value in values)
			{
				if (@enum.HasFlag(value))
					return true;
			}

			return false;
		}

		public static bool HasAllFlag<T>(this T @enum, params T[] values) where T : Enum
		{
			foreach (var value in values)
			{
				if (!@enum.HasFlag(value))
					return false;
			}

			return true;
		}
	}
}
