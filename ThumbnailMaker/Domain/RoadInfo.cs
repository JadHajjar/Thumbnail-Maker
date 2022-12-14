using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbnailMaker.Domain
{
	public class RoadInfo
	{
		public int Version { get; set; }
		public List<LaneInfo> Lanes { get; set; }
		public RoadType RoadType { get; set; }
		public RegionType RegionType { get; set; }
		public float RoadWidth { get; set; }
		public float BufferWidth { get; set; }
		public float SpeedLimit { get; set; }
		public bool LHT { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string CustomText { get; set; }
		public byte[] SmallThumbnail { get; set; }
		public byte[] LargeThumbnail { get; set; }
		public byte[] TooltipImage { get; set; }
		public string ThumbnailMakerConfig { get; set; }

		public List<LaneInfo> TmLanes { get; set; }

		public float Width { get; set; }
		public float BufferSize { get; set; }

		public RoadInfo Update()
		{
			TmLanes = JsonConvert.DeserializeObject<TmLane[]>(ThumbnailMakerConfig).Select(x => (LaneInfo)x).ToList();

			if (Version < 1)
			{
				TmLanes.ForEach(TranslateLaneTypesV1);

				if (Width != 0)
					RoadWidth = Width + 6;

				BufferWidth = BufferSize;

				if (!(TmLanes.Count > 0 && TmLanes[0].Class == LaneClass.Pedestrian))
					TmLanes.Insert(0, new LaneInfo { Class = LaneClass.Pedestrian, Direction = LaneDirection.Both });
				else
					TmLanes[0].Decorations = LaneDecoration.None;

				TmLanes.Insert(1, new LaneInfo { Class = LaneClass.Curb, Direction = LaneDirection.Backwards, Lanes = 1 });

				if (!(TmLanes.Count > 2 && TmLanes[TmLanes.Count - 1].Class == LaneClass.Pedestrian))
					TmLanes.Add(new LaneInfo { Class = LaneClass.Pedestrian, Direction = LaneDirection.Both });
				else
					TmLanes[TmLanes.Count - 1].Decorations = LaneDecoration.None;

				TmLanes.Insert(TmLanes.Count - 1, new LaneInfo { Class = LaneClass.Curb, Direction = LaneDirection.Forward, Lanes = 1 });
			}

			return this;
		}

		private void TranslateLaneTypesV1(LaneInfo l)
		{
			var newType = LaneClass.Empty;

			if (l.Type == OLD_LaneType.Empty)
			{
				l.Class = LaneClass.Empty;
				return;
			}

			foreach (var item in Enum
				.GetValues(typeof(OLD_LaneType))
				.Cast<OLD_LaneType>()
				.Where(e => l.Type.HasFlag(e)))
			{
				newType |= ConvertType(item);

				switch (item)
				{
					case OLD_LaneType.Grass:
						l.Decorations = LaneDecoration.Grass;
						break;
					case OLD_LaneType.Pavement:
						l.Decorations = LaneDecoration.Pavement;
						break;
					case OLD_LaneType.Gravel:
						l.Decorations = LaneDecoration.Gravel;
						break;
					case OLD_LaneType.Trees:
						l.Decorations = LaneDecoration.Tree | LaneDecoration.Grass;
						break;

					case OLD_LaneType.Pedestrian:
						l.Decorations = LaneDecoration.TransitStop;
						break;

					case OLD_LaneType.Bike:
					case OLD_LaneType.Bus:
					case OLD_LaneType.Trolley:
						l.Decorations = LaneDecoration.Filler;
						break;
				}
			}

			l.Class = newType;

			if (l.AddStopToFiller)
				l.Decorations |= LaneDecoration.TransitStop;
		}

		internal static LaneClass ConvertType(OLD_LaneType old)
		{
			switch (old)
			{
				case OLD_LaneType.Grass:
					return LaneClass.Filler;
				case OLD_LaneType.Pavement:
					return LaneClass.Filler;
				case OLD_LaneType.Gravel:
					return LaneClass.Filler;
				case OLD_LaneType.Trees:
					return LaneClass.Filler;
				case OLD_LaneType.Pedestrian:
					return LaneClass.Pedestrian;
				case OLD_LaneType.Car:
					return LaneClass.Car;
				case OLD_LaneType.Bike:
					return LaneClass.Bike;
				case OLD_LaneType.Tram:
					return LaneClass.Tram;
				case OLD_LaneType.Bus:
					return LaneClass.Bus;
				case OLD_LaneType.Trolley:
					return LaneClass.Trolley;
				case OLD_LaneType.Emergency:
					return LaneClass.Emergency;
				case OLD_LaneType.Train:
					return LaneClass.Train;
				case OLD_LaneType.Parking:
					return LaneClass.Parking;
			}

			return (LaneClass)(int)old;
		}
	}
}
