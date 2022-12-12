using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbnailMaker.Domain
{
	public enum LaneDecorationStyle
	{
		[LaneIdentity(5, "M", 72, 161, 73)] None,
		[LaneIdentity(1, "M", 154, 203, 96)]
		Grass,
		[LaneIdentity(2, "M", 99, 102, 107)]
		Pavement,
		[LaneIdentity(3, "M", 79, 61, 55)]
		Gravel,
		[LaneIdentity(4, "M", 72, 161, 73)]
		Tree,
		[LaneIdentity(4, "M", 72, 161, 73)]
		TreeAndGrass,
		[LaneIdentity(5, "M", 72, 161, 73)]
		TreeAndBenches,
		[LaneIdentity(6, "M", 72, 161, 73)]
		Benches,
		[LaneIdentity(7, "M", 72, 161, 73)] 
		FlowerPots,
		[LaneIdentity(8, "M", 72, 161, 73)]
		StreetLight,
		[LaneIdentity(9, "M", 72, 161, 73)]
		DoubleStreetLight,
		[LaneIdentity(10, "M", 72, 161, 73)]
		Bollard,

	}
}
