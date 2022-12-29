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
		public bool Buffer { get; set; }
		public Color Color
		{
			get
			{
				if (Type == (LaneType.Car | LaneType.Tram) && !Options.Current.LaneColors.ContainsKey(LaneType.Car) && !Options.Current.LaneColors.ContainsKey(LaneType.Tram))
					return Color.FromArgb(66, 185, 212);

				var types = Type.GetValues().ToList();
				var color = GetColor(types[0]);

				for (var i = 1; i < types.Count; i++)
					color = color.MergeColor(GetColor(types[i]), 30);

				return color;
			}
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
			FillerPadding = laneInfo.FillerPadding;
			PropAngle = laneInfo.PropAngle;
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
			FillerPadding = FillerPadding,
			PropAngle = PropAngle
		};

		public static Color GetColor(LaneType laneType)
		{
			return Options.Current.LaneColors.ContainsKey(laneType)
				? Options.Current.LaneColors[laneType]
				: GetDefaultLaneColor(laneType);
		}

		public static string GetLaneAbbreviation(LaneType lane)
		{
			var field = lane.GetType().GetField(Enum.GetName(typeof(LaneType), lane));

			var attribute = Attribute.GetCustomAttribute(field, typeof(StyleIdentityAttribute)) as StyleIdentityAttribute;

			return attribute.Name;
		}

		public static string GetTitle(LaneInfo lane)
		{
			if (lane.Type == LaneType.Parking)
			{
				switch (lane.ParkingAngle)
				{
					case ParkingAngle.Vertical:
						return "P";
					case ParkingAngle.Horizontal:
						return "HP";
					case ParkingAngle.Diagonal:
						return "DP";
					case ParkingAngle.InvertedDiagonal:
						return "CP";
				}
			}

			var laneNames = lane.Type.GetValues().Select(GetLaneAbbreviation).OrderBy(y => y).ToList();

			return laneNames.ListStrings("/");
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
	}
}