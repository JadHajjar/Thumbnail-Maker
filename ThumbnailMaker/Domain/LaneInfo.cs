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
		public LaneClass Class { get; set; }
		public LaneDecorationStyle Decorations { get; set; }
		public float? Elevation { get; set; }
		public float CustomWidth { get; set; }
		public float? SpeedLimit { get; set; }
		public bool AddStopToFiller { get; set; }

		public int FillerSize { get => IsFiller ? 10 * (10 - Math.Min(Lanes, 9)) : 0; set { } }
		public bool DiagonalParking { get => Class == LaneClass.Parking && Lanes == 3; set { } }
		public bool InvertedDiagonalParking { get => Class == LaneClass.Parking && Lanes > 3; set { } }
		public bool HorizontalParking { get => Class == LaneClass.Parking && Lanes == 2; set { } }

		public OLD_LaneType Type { get; set; }

		[XmlIgnore] public int Width { get; set; }
		[XmlIgnore] public int Lanes { get; set; }
		[XmlIgnore] public bool Sidewalk { get; set; }
		[XmlIgnore] public bool IsFiller => Class == LaneClass.Filler;
		[XmlIgnore] public Color Color
		{
			get
			{
				if (Class == (LaneClass.Car | LaneClass.Tram) && !Options.Current.LaneColors.ContainsKey(LaneClass.Car) && !Options.Current.LaneColors.ContainsKey(LaneClass.Tram))
					return Color.FromArgb(66, 185, 212);

				var types = GetLaneTypes(Class);
				var color = GetColor(types[0]);

				for (var i = 1; i < types.Count; i++)
					color = color.MergeColor(GetColor(types[i]), 30);

				return color;
			}
		}

		public static Color GetColor(LaneClass laneType)
		{
			return Options.Current.LaneColors.ContainsKey(laneType) 
				? Options.Current.LaneColors[laneType] 
				: GetDefaultLaneColor(laneType);
		}

		public static Color GetDefaultLaneColor(LaneClass lane)
		{
			var field = lane.GetType().GetField(Enum.GetName(typeof(LaneClass), lane));

			var attribute = Attribute.GetCustomAttribute(field, typeof(LaneIdentityAttribute)) as LaneIdentityAttribute;

			return attribute.DefaultColor;
		}

		public static string GetLaneAbbreviation(LaneClass lane)
		{
			var field = lane.GetType().GetField(Enum.GetName(typeof(LaneClass), lane));

			var attribute = Attribute.GetCustomAttribute(field, typeof(LaneIdentityAttribute)) as LaneIdentityAttribute;

			return attribute.Name;
		}

		public static List<LaneClass> GetLaneTypes(LaneClass laneType)
		{
			if (laneType == LaneClass.Empty)
				return new List<LaneClass> { LaneClass.Empty };

			return Enum
				.GetValues(typeof(LaneClass))
				.Cast<LaneClass>()
				.Where(e => e != LaneClass.Empty && laneType.HasFlag(e))
				.ToList();
		}

		public Brush Brush(bool small)
			=> new LinearGradientBrush(
				new Rectangle(0, 0, small ? 100 : 512, small ? 100 : 512),
				Color.FromArgb(0, Color), Color.FromArgb(255, Color),
				LinearGradientMode.Vertical);

		public List<KeyValuePair<LaneClass, Image>> Icons(bool small)
		{
			return GetLaneTypes(Class)
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

			sb.Append(GetLaneTypes(Class).Select(GetLaneAbbreviation).OrderBy(y => y).ListStrings("-"));

			return sb.ToString();
		}

		public string GetTitle(IEnumerable<LaneInfo> lanes)
		{
			if (Class == LaneClass.Pedestrian && (lanes.First() == this || lanes.Last() == this))
				return string.Empty;

			if (Class == LaneClass.Parking)
				return InvertedDiagonalParking ? "CP" : DiagonalParking ? "DP" : HorizontalParking ? "HP" : "P";

			var laneNames = GetLaneTypes(Class).Select(GetLaneAbbreviation).OrderBy(y => y).ToList();

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