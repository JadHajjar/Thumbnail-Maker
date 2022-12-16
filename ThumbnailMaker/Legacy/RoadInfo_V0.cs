using Extensions;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Legacy
{
	[XmlRoot("RoadInfo")]
	public class RoadInfo_V0
	{
		public List<LaneInfo_V0> Lanes { get; set; }
		public RegionType RegionType { get; set; }
		public float Width { get; set; }
		public float BufferSize { get; set; }
		public float SpeedLimit { get; set; }
		public bool LHT { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string CustomText { get; set; }
		public byte[] SmallThumbnail { get; set; }
		public byte[] LargeThumbnail { get; set; }
		public byte[] TooltipImage { get; set; }
		public string ThumbnailMakerConfig { get; set; }


		public static explicit operator RoadInfo(RoadInfo_V0 road)
		{
			var newRoad = new RoadInfo
			{
				RegionType = road.RegionType,
				RoadWidth = road.Width == 0 ? 0 : (road.Width + 6),
				BufferWidth = road.BufferSize,
				CustomText = road.CustomText,
				Description = road.Description,
				LargeThumbnail = road.LargeThumbnail,
				LHT = road.LHT,
				Name = road.Name,
				RoadType = RoadType.Road,
				SmallThumbnail = road.SmallThumbnail,
				SpeedLimit = (int)road.SpeedLimit,
				TooltipImage = road.TooltipImage,
				Version = LegacyUtil.CURRENT_VERSION,
				Lanes = new List<LaneInfo>()
			};

			var TmLanes = JsonConvert.DeserializeObject<TmLane_V0[]>(road.ThumbnailMakerConfig).Select(x => (LaneInfo_V0)x).ToList();

			foreach (var l in TmLanes)
			{
				if (l.IsFiller || l.Type == LaneType_V0.Parking || l.Lanes <= 1)
				{
					newRoad.Lanes.Add(GenerateLane(l));
					continue;
				}

				for (var i = 0; i < l.Lanes; i++)
				{
					var lane = GenerateLane(l);

					var bi = l.Direction == LaneDirection_V0.Both;

					if (bi && i >= l.Lanes / 2)
						lane.Direction = !road.LHT ? LaneDirection.Forward : LaneDirection.Backwards;
					else if (bi)
						lane.Direction = road.LHT ? LaneDirection.Forward : LaneDirection.Backwards;

					newRoad.Lanes.Add(lane);
				}
			}

			if (!(newRoad.Lanes.Count > 0 && newRoad.Lanes[0].Type == LaneType.Pedestrian))
				newRoad.Lanes.Insert(0, new LaneInfo { Type = LaneType.Pedestrian, Direction = LaneDirection.Both });
			else
				newRoad.Lanes[0].Decorations = LaneDecoration.None;

			newRoad.Lanes.Insert(1, new LaneInfo { Type = LaneType.Curb, Direction = LaneDirection.Backwards });

			if (!(newRoad.Lanes.Count > 2 && newRoad.Lanes[newRoad.Lanes.Count - 1].Type == LaneType.Pedestrian))
				newRoad.Lanes.Add(new LaneInfo { Type = LaneType.Pedestrian, Direction = LaneDirection.Both });
			else
				newRoad.Lanes[newRoad.Lanes.Count - 1].Decorations = LaneDecoration.None;

			newRoad.Lanes.Insert(newRoad.Lanes.Count - 1, new LaneInfo { Type = LaneType.Curb, Direction = LaneDirection.Forward });
			
			return newRoad;
		}

		private static LaneInfo GenerateLane(LaneInfo_V0 l)
		{
			var lane = new LaneInfo
			{
				CustomWidth = l.CustomWidth <= 0 ? (float?)null : l.CustomWidth,
				Elevation = l.Elevation,
				SpeedLimit = l.SpeedLimit,
				Direction = l.Direction == LaneDirection_V0.Forward ? LaneDirection.Forward : l.Direction == LaneDirection_V0.Backwards ? LaneDirection.Backwards : LaneDirection.Both,
				ParkingAngle = l.DiagonalParking ? ParkingAngle.Diagonal : l.InvertedDiagonalParking ? ParkingAngle.InvertedDiagonal : l.HorizontalParking ? ParkingAngle.Horizontal : ParkingAngle.Vertical
			};

			var newType = LaneType.Empty;

			foreach (var item in l.Type.GetValues())
			{
				newType |= ConvertType(item);

				switch (item)
				{
					case LaneType_V0.Grass:
						lane.Decorations |= LaneDecoration.Grass;
						break;
					case LaneType_V0.Pavement:
						lane.Decorations |= LaneDecoration.Pavement;
						break;
					case LaneType_V0.Gravel:
						lane.Decorations |= LaneDecoration.Gravel;
						break;
					case LaneType_V0.Trees:
						lane.Decorations |= LaneDecoration.Tree | LaneDecoration.Grass;
						break;

					case LaneType_V0.Pedestrian:
						lane.Decorations |= LaneDecoration.TransitStop;
						break;

					case LaneType_V0.Bike:
					case LaneType_V0.Bus:
					case LaneType_V0.Trolley:
						lane.Decorations |= LaneDecoration.Filler;
						break;
				}
			}

			lane.Type = newType;

			if (l.AddStopToFiller)
				lane.Decorations |= LaneDecoration.TransitStop;

			return lane;
		}

		internal static LaneType ConvertType(LaneType_V0 old)
		{
			switch (old)
			{
				case LaneType_V0.Empty:
					return LaneType.Filler;
				case LaneType_V0.Grass:
					return LaneType.Filler;
				case LaneType_V0.Pavement:
					return LaneType.Filler;
				case LaneType_V0.Gravel:
					return LaneType.Filler;
				case LaneType_V0.Trees:
					return LaneType.Filler;
				case LaneType_V0.Pedestrian:
					return LaneType.Pedestrian;
				case LaneType_V0.Car:
					return LaneType.Car;
				case LaneType_V0.Bike:
					return LaneType.Bike;
				case LaneType_V0.Tram:
					return LaneType.Tram;
				case LaneType_V0.Bus:
					return LaneType.Bus;
				case LaneType_V0.Trolley:
					return LaneType.Trolley;
				case LaneType_V0.Emergency:
					return LaneType.Emergency;
				case LaneType_V0.Train:
					return LaneType.Train;
				case LaneType_V0.Parking:
					return LaneType.Parking;
			}

			return (LaneType)(int)old;
		}
	}

	public class TmLane_V0
	{
		public TmLane_V0()
		{
		}

		public TmLane_V0(LaneInfo_V0 lane)
		{
			LaneType = lane.Type;
			LaneDirection = lane.Direction;
			Lanes = lane.Lanes;
			CustomLaneWidth = lane.CustomWidth <= 0 ? -1 : lane.CustomWidth;
			CustomVerticalOffset = lane.Elevation ?? -1;
			CustomSpeedLimit = lane.SpeedLimit ?? -1;
			AddStopToFiller = lane.AddStopToFiller;
		}

		public LaneType_V0 LaneType { get; set; }
		public LaneDirection_V0 LaneDirection { get; set; }
		public int Lanes { get; set; }
		public float CustomLaneWidth { get; set; }
		public float CustomVerticalOffset { get; set; }
		public float CustomSpeedLimit { get; set; }
		public bool AddStopToFiller { get; set; }


		public static explicit operator LaneInfo_V0(TmLane_V0 l) => new LaneInfo_V0
		{
			Type = l.LaneType,
			Direction = l.LaneDirection,
			Lanes = l.Lanes,
			AddStopToFiller = l.AddStopToFiller,
			CustomWidth = l.CustomLaneWidth == -1 ? 0 : l.CustomLaneWidth,
			Elevation = l.CustomVerticalOffset == -1 ? (float?)null : l.CustomLaneWidth,
			SpeedLimit = l.CustomSpeedLimit == -1 ? (float?)null : l.CustomSpeedLimit
		};
	}

	public class LaneInfo_V0
	{
		public LaneDirection_V0 Direction { get; set; }
		public LaneType_V0 Type { get; set; }
		public float? Elevation { get; set; }
		public float CustomWidth { get; set; }
		public float? SpeedLimit { get; set; }
		public bool AddStopToFiller { get; set; }

		public bool DiagonalParking { get => Type == LaneType_V0.Parking && Lanes == 3; set { } }
		public bool InvertedDiagonalParking { get => Type == LaneType_V0.Parking && Lanes > 3; set { } }
		public bool HorizontalParking { get => Type == LaneType_V0.Parking && Lanes == 2; set { } }

		public int Lanes { get; set; }
		public bool IsFiller => Type.GetValues().All(x => x < LaneType_V0.Car);

		public static List<LaneType_V0> GetLaneTypes(LaneType_V0 laneType)
		{
			if (laneType == LaneType_V0.Empty)
				return new List<LaneType_V0> { LaneType_V0.Empty };

			return Enum
				.GetValues(typeof(LaneType_V0))
				.Cast<LaneType_V0>()
				.Where(e => e != LaneType_V0.Empty && laneType.HasFlag(e))
				.ToList();
		}
	}

	public enum LaneDirection_V0
	{
		None = 0,
		Both = 1,
		Backwards = 2,
		Forward = 3,
	}

	[Flags]
	public enum LaneType_V0
	{
		[StyleIdentity(0, "M", 0, 0, 0)]
		Empty = 0,

		[StyleIdentity(1, "M", 154, 203, 96)]
		Grass = 1,

		[StyleIdentity(2, "M", 99, 102, 107)]
		Pavement = 2,

		[StyleIdentity(3, "M", 79, 61, 55)]
		Gravel = 4,

		[StyleIdentity(4, "M", 72, 161, 73)]
		Trees = 8,

		[StyleIdentity(5, "C", 66, 132, 212)]
		Car = 16,

		[StyleIdentity(6, "Ped", 92, 97, 102)]
		Pedestrian = 32,

		[StyleIdentity(7, "C", 41, 153, 151)]
		Highway = 64,

		[StyleIdentity(8, "B", 74, 205, 151)]
		Bike = 128,

		[StyleIdentity(9, "T", 230, 210, 122)]
		Tram = 256,

		[StyleIdentity(10, "Bus", 170, 62, 48)]
		Bus = 512,

		[StyleIdentity(11, "Trolley", 184, 70, 55)]
		Trolley = 1024,

		[StyleIdentity(12, "EV", 222, 75, 109)]
		Emergency = 2048,

		[StyleIdentity(13, "Train", 194, 146, 74)]
		Train = 4096,

		[StyleIdentity(14, "P", 74, 89, 161)]
		Parking = 8192
	}
}
