using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbnailMaker.Domain
{
	public class TmLane
	{
		public TmLane()
		{
		}

		public TmLane(LaneInfo lane)
		{
			LaneType = lane.Type;
			LaneDirection = lane.Direction;
			Lanes = lane.Lanes;
			CustomLaneWidth = lane.CustomWidth <= 0 ? -1 : lane.CustomWidth;
			CustomVerticalOffset = lane.Elevation ?? -1;
			CustomSpeedLimit = lane.SpeedLimit ?? -1;
		}

		public LaneType LaneType { get; set; }
		public LaneDirection LaneDirection { get; set; }
		public int Lanes { get; set; }
		public float CustomLaneWidth { get; set; }
		public float CustomVerticalOffset { get; set; }
		public float CustomSpeedLimit { get; set; }
	}
}
