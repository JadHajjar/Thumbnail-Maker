using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Domain
{
	public class ThumbnailLaneInfo : LaneInfo
	{ 
		public int Width { get; set; }
		public bool Sidewalk { get; set; }
		public Color Color
		{
			get
			{
				if (Type == (LaneType.Car | LaneType.Tram) && !Options.Current.LaneColors.ContainsKey(LaneType.Car) && !Options.Current.LaneColors.ContainsKey(LaneType.Tram))
					return Color.FromArgb(66, 185, 212);

				var types = GetLaneTypes(Type);
				var color = GetColor(types[0]);

				for (var i = 1; i < types.Count; i++)
					color = color.MergeColor(GetColor(types[i]), 30);

				return color;
			}
		}

		public static Color GetColor(LaneType laneType)
		{
			return Options.Current.LaneColors.ContainsKey(laneType) 
				? Options.Current.LaneColors[laneType] 
				: GetDefaultLaneColor(laneType);
		}

		public static Color GetColor(LaneDecoration deco)
		{
			var types = deco.GetValues().ToList();
			var color = getColor(types[0]);

			for (var i = 1; i < types.Count; i++)
				color = color.MergeColor(getColor(types[i]), 30);

			return color;

			Color getColor(LaneDecoration val)
			{ 
				var field = typeof(LaneDecoration).GetField(Enum.GetName(typeof(LaneDecoration), val));

				var attribute = Attribute.GetCustomAttribute(field, typeof(StyleIdentityAttribute)) as StyleIdentityAttribute;

				return attribute.DefaultColor; 
			}
		}

		public static Color GetDefaultLaneColor(LaneType lane)
		{
			var field = lane.GetType().GetField(Enum.GetName(typeof(LaneType), lane));

			var attribute = Attribute.GetCustomAttribute(field, typeof(StyleIdentityAttribute)) as StyleIdentityAttribute;

			return attribute.DefaultColor;
		}

		public static string GetLaneAbbreviation(LaneType lane)
		{
			var field = lane.GetType().GetField(Enum.GetName(typeof(LaneType), lane));

			var attribute = Attribute.GetCustomAttribute(field, typeof(StyleIdentityAttribute)) as StyleIdentityAttribute;

			return attribute.Name;
		}

		public static List<LaneType> GetLaneTypes(LaneType laneType)
		{
			if (laneType == LaneType.Empty)
				return new List<LaneType> { LaneType.Empty };

			return Enum
				.GetValues(typeof(LaneType))
				.Cast<LaneType>()
				.Where(e => e != LaneType.Empty && laneType.HasFlag(e))
				.ToList();
		}

		public Brush Brush(bool small)
			=> new LinearGradientBrush(
				new Rectangle(0, 0, small ? 100 : 512, small ? 100 : 512),
				Color.FromArgb(100, Color), Color.FromArgb(255, Color),
				LinearGradientMode.Vertical);

		public List<KeyValuePair<LaneType, Image>> Icons(bool small, bool all = false)
		{
			return GetLaneTypes(Type)
			  .Where(x => all || x > LaneType.Curb)
			  .ToDictionary(x => x, x => ResourceManager.GetImage(x, small))
			  .Where(x => x.Value != null)
			  .ToList();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			switch (Direction)
			{
				case LaneDirection.Both:
					sb.Append("2W ");
					break;

				case LaneDirection.Forward:
					sb.Append("1WF ");
					break;

				case LaneDirection.Backwards:
					sb.Append("1WB ");
					break;
			}

			sb.Append(GetLaneTypes(Type).Select(GetLaneAbbreviation).OrderBy(y => y).ListStrings("-"));

			return sb.ToString();
		}

		public string GetTitle(IEnumerable<LaneInfo> lanes)
		{
			if (Type == LaneType.Pedestrian && (lanes.First() == this || lanes.Last() == this))
				return string.Empty;

			if (Type == LaneType.Parking)
			{
				switch (ParkingAngle)
				{
					case ParkingAngle.Vertical: return "P";
					case ParkingAngle.Horizontal: return "HP";
					case ParkingAngle.Diagonal: return "DP";
					case ParkingAngle.InvertedDiagonal: return "CP";
				}
			}

			var laneNames = GetLaneTypes(Type).Select(GetLaneAbbreviation).OrderBy(y => y).ToList();

			return laneNames.ListStrings("/");
			
			//if (Direction == LaneDirection.Both)
			//{
			//	if (laneNames.Count() > 1 || Lanes == 1)
			//		return $"2W " + laneNames.ListStrings("/");

			//	return laneNames[0] + "+" + laneNames[0];
			//}

			//if (laneNames.All(x => x == "M"))
			//	return Lanes < -6 ? "XLM" : Lanes < -2 ? "LM" : Lanes > 4 ? "S" : laneNames[0];

			//return (Lanes >= 2 ? Lanes.ToString() : "") + laneNames.ListStrings("/");
		}

		public ThumbnailLaneInfo()
		{

		}

		public ThumbnailLaneInfo(LaneInfo laneInfo)
		{
			CustomWidth = laneInfo.CustomWidth;
			Decorations = laneInfo.Decorations;
			Direction = laneInfo.Direction;
			Elevation = laneInfo.Elevation;
			ParkingAngle = laneInfo.ParkingAngle;
			SpeedLimit = laneInfo.SpeedLimit;
			Type = laneInfo.Type;
		}

		public LaneInfo AsLaneInfo() => new LaneInfo
		{
			CustomWidth = CustomWidth,
			Decorations = Decorations,
			Direction = Direction,
			Elevation = Elevation,
			ParkingAngle = ParkingAngle,
			SpeedLimit = SpeedLimit,
			Type = Type,
		};
}
}