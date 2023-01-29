using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using ThumbnailMaker.Domain;

namespace ThumbnailMaker.Handlers
{
	public static class Utilities
	{
		public static string Folder => Directory.GetParent(Application.ExecutablePath).FullName;

		public static string ExportRoad(RoadInfo road, string fileName = null)
		{
			var appdata = Options.Current.ExportFolder.IfEmpty(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
				, "Colossal Order", "Cities_Skylines", "RoadBuilder", "Roads"));

			Directory.CreateDirectory(appdata);

			road.Version = LegacyUtil.CURRENT_VERSION;
			road.Name = road.CustomName.IfEmpty(GetRoadName(road));
			road.Description = GetRoadDescription(road);
			road.SmallThumbnail = getImage(true, false);
			road.LargeThumbnail = getImage(false, false);
			road.TooltipImage = getImage(true, true);

			if (road.LHT)
			{
				road.Lanes.Reverse();
				road.Lanes.Where(x => x.Type == LaneType.Curb).Foreach(x => x.Direction = x.Direction == LaneDirection.Forward ? LaneDirection.Backwards : LaneDirection.Forward);
			}

			var guid = Guid.NewGuid().ToString();
			var xML = new System.Xml.Serialization.XmlSerializer(typeof(RoadInfo));

			using (var stream = File.Create(Path.Combine(appdata, fileName ?? $"{guid}.xml")))
			{
				xML.Serialize(stream, road);
			}

			return Path.Combine(appdata, fileName ?? $"{guid}.xml");

			byte[] getImage(bool small, bool toolTip)
			{
				var width = toolTip ? 492 : small ? 109 : 512;
				var height = toolTip ? 147 : small ? 100 : 512;

				using (var img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
				using (var g = Graphics.FromImage(img))
				{
					new ThumbnailHandler(g, small, toolTip)
					{
						RoadWidth = road.TotalRoadWidth,
						CustomText = road.CustomText,
						BufferSize = Math.Max(0, road.BufferWidth),
						RegionType = road.RegionType,
						RoadType = road.RoadType,
						Speed = road.SpeedLimit.If(0, DefaultSpeedSign(road.RoadType, road.RegionType == RegionType.USA)),
						SideTexture = road.SideTexture,
						AsphaltStyle = road.AsphaltStyle,
						LHT = road.LHT,
						Lanes = road.Lanes.Select(x => new ThumbnailLaneInfo(x)).ToList()
					}.Draw();

					return (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]));
				}
			}
		}

		public static IEnumerable<string> GetAutoTags(RoadInfo road)
		{
			if (road?.Lanes == null)
				yield break;

			if (road.Lanes.Any(x => x.Type.HasFlag(LaneType.Tram)))
				yield return "Tram";
			if (road.Lanes.Any(x => x.Type.HasFlag(LaneType.Trolley)))
				yield return "Trolley";
			if (road.Lanes.Any(x => x.Type.HasFlag(LaneType.Bike)))
				yield return "Bike";
			if (road.Lanes.Any(x => x.Type.HasFlag(LaneType.Bus)))
				yield return "Bus";
			if (IsOneWay(road.Lanes) == true)
				yield return "One-Way";
		}

		public static float VanillaWidth(bool vanillaWidths, float value)
		{
			if (!vanillaWidths)
			{
				return value;
			}

			return (float)(16 * Math.Ceiling((value - 1F) / 16D));
		}

		public static string GetRoadName(RoadInfo road)
		{
			var sb = new List<string>();
			var current = (LaneInfo)null;
			var currentCount = 0;
			var _lanes = new List<LaneInfo>(road.Lanes);

			if (_lanes.Count > 1 && _lanes[0].Type == LaneType.Pedestrian && _lanes[1].Type == LaneType.Curb)
			{
				_lanes.RemoveAt(0);
			}

			if (_lanes.Count > 1 && _lanes[_lanes.Count - 1].Type == LaneType.Pedestrian && _lanes[_lanes.Count - 2].Type == LaneType.Curb)
			{
				_lanes.RemoveAt(_lanes.Count - 1);
			}

			foreach (var item in _lanes)
			{
				if (item.Type == LaneType.Curb)
				{
					continue;
				}

				if (current != null && current.Type == item.Type && current.Direction == item.Direction)
				{
					currentCount++;
				}
				else
				{
					if (current != null)
					{
						var title = ThumbnailLaneInfo.GetTitle(current);

						if (!string.IsNullOrEmpty(title))
						{
							sb.Add(currentCount > 1 ? $"{currentCount}{title}" : title);
						}
					}

					current = item;
					currentCount = 1;
				}
			}

			if (current != null)
			{
				var title = ThumbnailLaneInfo.GetTitle(current);

				if (!string.IsNullOrEmpty(title))
				{
					sb.Add(currentCount > 1 ? $"{currentCount}{title}" : title);
				}
			}

			var name = $"RB{road.RoadType.ToString()[0]}";

			if (Options.Current.AddRoadWidthToName)
			{
				var roadSize = road.TotalRoadWidth;

				name += road.VanillaWidth ? $" {roadSize / 8:0}u" : $" {roadSize:0.##}m";
			}

			if (IsOneWay(_lanes) == true)
			{
				name += " 1W";
			}

			return $"{name} {sb.ListStrings("+")}";
		}

		public static string GetRoadDescription(RoadInfo road, bool signature = true)
		{
			var oneWay = IsOneWay(road.Lanes);
			var asymetrical = road.Lanes.Where(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Backwards).Count() != road.Lanes.Where(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Forward).Count() && road.Lanes.Any(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Backwards);
			var sb = new List<string>();
			var lanes = new List<string>();

			if (asymetrical)
			{
				sb.Add("Asymmetrical");
			}

			sb.Add(oneWay.Switch(true, "One-Way", false, "Two-Way", string.Empty));

			if (road.RoadType != RoadType.Road)
			{
				sb.Add($"{road.RoadType}");
			}

			sb.Add($"Road,");

			var current = (LaneInfo)null;
			var currentCount = 0;
			var _lanes = new List<LaneInfo>(road.Lanes);

			if (_lanes.Count > 1 && _lanes[0].Type == LaneType.Pedestrian && _lanes[1].Type == LaneType.Curb)
			{
				_lanes.RemoveAt(0);
			}

			if (_lanes.Count > 1 && _lanes[_lanes.Count - 1].Type == LaneType.Pedestrian && _lanes[_lanes.Count - 2].Type == LaneType.Curb)
			{
				_lanes.RemoveAt(_lanes.Count - 1);
			}

			foreach (var item in _lanes)
			{
				if (item.Type == LaneType.Curb)
				{
					continue;
				}

				if (current != null && current.Type == item.Type && current.Direction == item.Direction && current.Decorations == item.Decorations)
				{
					currentCount++;
				}
				else
				{
					if (current != null)
					{
						lanes.Add(current.ToString(currentCount));
					}

					current = item;
					currentCount = 1;
				}
			}

			if (current != null)
			{
				lanes.Add(current.ToString(currentCount));
			}

			sb.Add(lanes.ListStrings(", "));

			var size = Math.Max(road.RoadWidth, CalculateRoadSize(road.Lanes, road.BufferWidth));
			var info = $"{size}m";

			if (road.SpeedLimit != 0)
			{
				info += $" - {road.SpeedLimit}{(road.RegionType == RegionType.USA ? "mph" : "km/h")}";
			}

			sb.Add($"({info})");

			if (signature)
			{
				sb.Add("This road was automatically generated by Road Builder.");
			}

			return string.Join(" ", sb).RemoveDoubleSpaces().Trim();
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
				//case RoadType.Pedestrian:
				//	return mph ? 10 : 20;
				case RoadType.Flat:
					return mph ? 15 : 30;
			}

			return 0;
		}

		public static float CalculateRoadSize<T>(List<T> sizeLanes, float bufferSize) where T : LaneInfo
			=> CalculateRoadSize<T>(sizeLanes, bufferSize, out _, out _);

		public static float CalculateRoadSize<T>(List<T> sizeLanes, float bufferSize, out float leftPavementWidth, out float rightPavementWidth) where T : LaneInfo
		{
			var leftCurb = sizeLanes.FirstOrDefault(x => x.Type == LaneType.Curb);
			var rightCurb = sizeLanes.LastOrDefault(x => x.Type == LaneType.Curb);

			if (leftCurb == null || rightCurb == null)
			{
				leftPavementWidth = rightPavementWidth = 0;
				return 0;
			}

			leftPavementWidth = sizeLanes.Where(x => sizeLanes.IndexOf(x) <= sizeLanes.IndexOf(leftCurb)).Sum(x => x.LaneWidth);
			rightPavementWidth = sizeLanes.Where(x => sizeLanes.IndexOf(x) >= sizeLanes.IndexOf(rightCurb)).Sum(x => x.LaneWidth);
			var asphaltWidth = sizeLanes.Where(x => sizeLanes.IndexOf(x) > sizeLanes.IndexOf(leftCurb) && sizeLanes.IndexOf(x) < sizeLanes.IndexOf(rightCurb)).Sum(x => x.LaneWidth) + (2 * bufferSize);
			var totalWidth = Math.Max(1.5F, leftPavementWidth) + Math.Max(1.5F, rightPavementWidth) + asphaltWidth;

			return (float)Math.Round(totalWidth, 2);
		}

		public static bool? IsOneWay<T>(IEnumerable<T> lanes) where T : LaneInfo
		{
			var types = new[] { LaneType.Bike, LaneType.Car, LaneType.Bus, LaneType.Tram, LaneType.Trolley, LaneType.Emergency };
			var firstLane = lanes.FirstOrDefault(x => x.Type.HasAnyFlag(types));

			if (firstLane != null)
			{
				return firstLane.Direction != LaneDirection.Both && lanes
					.Where(x => x.Type.HasAnyFlag(types))
					.All(x => x.Direction == firstLane.Direction);
			}

			return null;
		}

		public static void DrawIcon(this Graphics g, Image img, Rectangle rect, Size? size = null)
		{
			if (img == null)
			{
				return;
			}

			if (size != null)
			{
				rect = rect.CenterR((Size)size);
			}

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
			{
				rect = rect.CenterR(img.Size);
			}

			g.DrawImage(img, rect);
		}

		public static bool IsCompatible(this LaneDecoration deco, LaneType laneClass)
		{
			foreach (var item in laneClass.GetValues())
			{
				switch (item)
				{
					case LaneType.Pedestrian:
						if (!deco.AnyOf(LaneDecoration.TrashBin, LaneDecoration.StreetAds, LaneDecoration.StreetLight, LaneDecoration.DoubleStreetLight, LaneDecoration.TransitStop, LaneDecoration.None, LaneDecoration.Filler, LaneDecoration.Grass, LaneDecoration.Gravel, LaneDecoration.Pavement))
						{
							return false;
						}

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
						{
							return false;
						}

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
				{
					yield return value;
				}
			}
		}

		public static bool HasAnyFlag<T>(this T @enum, params T[] values) where T : Enum
		{
			foreach (var value in values)
			{
				if (@enum.HasFlag(value))
				{
					return true;
				}
			}

			return false;
		}

		public static bool HasAllFlag<T>(this T @enum, params T[] values) where T : Enum
		{
			foreach (var value in values)
			{
				if (!@enum.HasFlag(value))
				{
					return false;
				}
			}

			return true;
		}
	}
}
