using Extensions;

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
			Class = lane.Class;
			LaneDirection = lane.Direction;
			Lanes = lane.Lanes;
			CustomLaneWidth = lane.CustomWidth;
			CustomVerticalOffset = lane.Elevation;
			CustomSpeedLimit = lane.SpeedLimit;
			AddStopToFiller = lane.AddStopToFiller;
			Decorations = lane.Decorations;
		}

		public OLD_LaneType LaneType { get; set; }
		public LaneClass Class { get; set; }
		public LaneDirection LaneDirection { get; set; }
		public int Lanes { get; set; }
		public float? CustomLaneWidth { get; set; }
		public float? CustomVerticalOffset { get; set; }
		public float? CustomSpeedLimit { get; set; }
		public bool AddStopToFiller { get; set; }
		public LaneDecoration Decorations { get; set; }

		public static explicit operator LaneInfo(TmLane l) => new LaneInfo
		{
			Class = l.Class,
			Type = l.LaneType,
			Direction = l.LaneDirection,
			Lanes = l.Lanes,
			Decorations = l.Decorations,
			AddStopToFiller = l.AddStopToFiller,
			CustomWidth = l.CustomLaneWidth.If(-1, null),
			Elevation = l.CustomVerticalOffset.If(-1, null),
			SpeedLimit = l.CustomSpeedLimit.If(-1, null),
		};
	}
}
