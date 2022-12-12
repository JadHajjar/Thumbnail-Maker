using Extensions;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ThumbnailMaker.Domain
{
	public class LaneInfo
	{
		public LaneDirection Direction { get; set; }
		public LaneType Type { get; set; }
		public float? Elevation { get; set; }
		public float CustomWidth { get; set; }
		public float? SpeedLimit { get; set; }
		public bool AddStopToFiller { get; set; }

		public int FillerSize { get => IsFiller ? 10 * (10 - Math.Min(Lanes, 9)) : 0; set { } }
		public bool DiagonalParking { get => Type == LaneType.Parking && Lanes == 3; set { } }
		public bool InvertedDiagonalParking { get => Type == LaneType.Parking && Lanes > 3; set { } }
		public bool HorizontalParking { get => Type == LaneType.Parking && Lanes == 2; set { } }

		[XmlIgnore] public int Width { get; set; }
		[XmlIgnore] public int Lanes { get; set; }
		[XmlIgnore] public bool Sidewalk { get; set; }
		[XmlIgnore] public bool IsFiller => GetLaneTypes(Type).All(x => x < LaneType.Car);
		[XmlIgnore] public Color Color
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

		public static Color GetDefaultLaneColor(LaneType lane)
		{
			var field = lane.GetType().GetField(Enum.GetName(typeof(LaneType), lane));

			var attribute = Attribute.GetCustomAttribute(field, typeof(LaneIdentityAttribute)) as LaneIdentityAttribute;

			return attribute.DefaultColor;
		}

		public static string GetLaneAbbreviation(LaneType lane)
		{
			var field = lane.GetType().GetField(Enum.GetName(typeof(LaneType), lane));

			var attribute = Attribute.GetCustomAttribute(field, typeof(LaneIdentityAttribute)) as LaneIdentityAttribute;

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
				Color.FromArgb(0, Color), Color.FromArgb(255, Color),
				LinearGradientMode.Vertical);

		public List<KeyValuePair<LaneType, Image>> Icons(bool small)
		{
			return GetLaneTypes(Type)
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

			if (Lanes > 0)
				sb.Append($"{Lanes}L ");

			sb.Append(GetLaneTypes(Type).Select(GetLaneAbbreviation).OrderBy(y => y).ListStrings("-"));

			return sb.ToString();
		}

		public string GetTitle(IEnumerable<LaneInfo> lanes)
		{
			if (Type == LaneType.Pedestrian && (lanes.First() == this || lanes.Last() == this))
				return string.Empty;

			if (Type == LaneType.Parking)
				return InvertedDiagonalParking ? "CP" : DiagonalParking ? "DP" : HorizontalParking ? "HP" : "P";

			var laneNames = GetLaneTypes(Type).Select(GetLaneAbbreviation).OrderBy(y => y).ToList();

			if (Direction == LaneDirection.Both)
			{
				if (laneNames.Count() > 1 || Lanes == 1)
					return $"2W " + laneNames.ListStrings("/");

				return laneNames[0] + "+" + laneNames[0];
			}

			if (laneNames.All(x => x == "M"))
				return Lanes < -6 ? "XLM" : Lanes < -2 ? "LM" : Lanes > 4 ? "S" : laneNames[0];

			return (Lanes >= 2 ? Lanes.ToString() : "") + laneNames.ListStrings("/");
		}
	}
}