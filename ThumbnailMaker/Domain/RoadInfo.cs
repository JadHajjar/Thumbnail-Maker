using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbnailMaker.Domain
{
	public class RoadInfo
	{
		public List<LaneInfo> Lanes { get; set; }
		public float Width { get; set; }
		public float BufferSize { get; set; }
		public float SpeedLimit { get; set; }
		public bool Highway { get; set; }
		public float Elevation { get; set; } = -0.3F;
		public string Name { get; set; }
		public string Description { get; set; }
		public byte[] SmallThumbnail { get; set; }
		public byte[] LargeThumbnail { get; set; }
	}
}
