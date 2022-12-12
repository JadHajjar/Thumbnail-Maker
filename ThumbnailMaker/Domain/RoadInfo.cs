using Newtonsoft.Json;

using System;
using System.Collections.Generic;
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
		public float AsphaltWidth { get; set; }
		public float PavementWidth { get; set; }
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
				AsphaltWidth = Width;
				BufferWidth = BufferSize;

				if (!(TmLanes.Count > 0 && TmLanes[0].Type == LaneType.Pedestrian))
					TmLanes.Insert(0, new LaneInfo { Type = LaneType.Pedestrian, Direction = LaneDirection.Both });

				TmLanes.Insert(1, new LaneInfo { Type = LaneType.Sidewalk, Direction = LaneDirection.Backwards, Lanes = 1 });

				if (!(TmLanes.Count > 2 && TmLanes[TmLanes.Count - 1].Type == LaneType.Pedestrian))
					TmLanes.Add(new LaneInfo { Type = LaneType.Pedestrian, Direction = LaneDirection.Both });

				TmLanes.Insert(TmLanes.Count - 1, new LaneInfo { Type = LaneType.Sidewalk, Direction = LaneDirection.Forward, Lanes = 1 });
			}

			return this;
		}
	}
}
