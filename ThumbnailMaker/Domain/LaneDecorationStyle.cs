using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbnailMaker.Domain
{
	public enum LaneDecorationStyle
	{
		[LaneIdentity(0, "M", 72, 161, 73)] 
		None = 0,
		[LaneIdentity(12, "M", 72, 161, 73)]
		Filler = 1,
		[LaneIdentity(1, "M", 154, 203, 96)]
		Grass = 2,
		[LaneIdentity(2, "M", 99, 102, 107)]
		Pavement = 3,
		[LaneIdentity(3, "M", 79, 61, 55)]
		Gravel = 4,
		[LaneIdentity(4, "M", 72, 161, 73)]
		Tree = 5,
		[LaneIdentity(5, "M", 72, 161, 73)]
		TreeAndGrass = 6,
		[LaneIdentity(6, "M", 72, 161, 73)]
		TreeAndBenches = 7,
		[LaneIdentity(7, "M", 72, 161, 73)]
		Benches = 8,
		[LaneIdentity(8, "M", 72, 161, 73)] 
		FlowerPots = 9,
		[LaneIdentity(9, "M", 72, 161, 73)]
		StreetLight = 10,
		[LaneIdentity(10, "M", 72, 161, 73)]
		DoubleStreetLight = 11,
		[LaneIdentity(11, "M", 72, 161, 73)]
		Bollard = 12
	}
}
