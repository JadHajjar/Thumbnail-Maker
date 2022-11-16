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

		public int FillerSize { get => IsFiller ? 10 * (10 - Math.Min(Lanes, 9)) : 0; set { } }
		public bool DiagonalParking { get => Type == LaneType.Parking && Lanes > 1; set { } }
		public bool HorizontalParking { get => Type == LaneType.Parking && Lanes == 1; set { } }

		[XmlIgnore]
		public int Width { get; set; }
		[XmlIgnore]
		public int Lanes { get; set; }
		[XmlIgnore]
		public Color Color
		{
			get
			{
				var types = GetLaneTypes(Type);
				var color = GetColor(types[0]);

				for (var i = 1; i < types.Count; i++)
					color = color.MergeColor(GetColor(types[i]), 35);

				return color;
			}
		}
		[XmlIgnore]
		public bool IsFiller => GetLaneTypes(Type).All(x => x < LaneType.Car);

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
				Color.FromArgb(0, Color), Color,
				LinearGradientMode.Vertical);

		public List<Image> Icons(bool small)
		{
			return GetLaneTypes(Type)
			  .Select(x => ResourceManager.GetImage(x, small))
			  .Where(x => x != null)
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

			sb.Append(Type.ToString().Where(char.IsUpper).ListStrings());

			return sb.ToString();
		}

		public string GetTitle()
		{
			if (Type == LaneType.Pedestrian)
				return string.Empty;

			if (Type == LaneType.Parking)
				return Lanes == 0 ? "P" : Lanes == 1 ? "HP" : "DP";

			var laneNames = GetLaneTypes(Type).Select(GetLaneAbbreviation).OrderBy(y => y).ToList();

			if (Direction == LaneDirection.Both)
			{
				if (laneNames.Count() > 1)
					return $"2W " + laneNames.ListStrings("/");

				return laneNames[0] + "+" + laneNames[0];
			}

			if (laneNames.All(x => x == "M"))
				return Lanes > 3 ? "" : laneNames[0];

			return (Lanes >= 2 ? Lanes.ToString() : "") + laneNames.ListStrings("/");
		}
	}
}