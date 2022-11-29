﻿using System;
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
			AddStopToFiller = lane.AddStopToFiller;
		}

		public LaneType LaneType { get; set; }
		public LaneDirection LaneDirection { get; set; }
		public int Lanes { get; set; }
		public float CustomLaneWidth { get; set; }
		public float CustomVerticalOffset { get; set; }
		public float CustomSpeedLimit { get; set; }
		public bool AddStopToFiller { get; set; }


		public static explicit operator LaneInfo(TmLane l) => new LaneInfo
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
}