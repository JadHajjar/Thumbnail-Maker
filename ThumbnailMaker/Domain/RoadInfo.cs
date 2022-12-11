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

		public RoadInfo Update()
		{
			if (Version < 1)
			{

			}

			return this;
		}
	}
}
