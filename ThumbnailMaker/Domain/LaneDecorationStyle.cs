using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbnailMaker.Domain
{
	public enum LaneDecorationStyle
	{
		[LaneIdentity(0, 50, 50, 50)] 
		None = 0,
		[LaneIdentity(12)]
		Filler = 1,
		[LaneIdentity(1, 154, 203, 96)]
		Grass = 2,
		[LaneIdentity(2, 99, 102, 107)]
		Pavement = 3,
		[LaneIdentity(3, 79, 61, 55)]
		Gravel = 4,
		[LaneIdentity(4)]
		Tree = 5,
		[LaneIdentity(5, 72, 161, 73)]
		TreeAndGrass = 6,
		[LaneIdentity(6)]
		TreeAndBenches = 7,
		[LaneIdentity(7)]
		Benches = 8,
		[LaneIdentity(8)] 
		FlowerPots = 9,
		[LaneIdentity(9)]
		StreetLight = 10,
		[LaneIdentity(10)]
		DoubleStreetLight = 11,
		[LaneIdentity(11)]
		Bollard = 12
	}
}
