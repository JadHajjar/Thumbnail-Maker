using System;
using System.Drawing;

namespace ThumbnailMaker.Domain
{
	[Flags]
	public enum LaneClass
	{
		[StyleIdentity(0, 50, 50, 50)]
		Empty = 0,

		[StyleIdentity(1, "M", 130, 204, 96)]
		Filler = 1,

		[StyleIdentity(2, 200, 200, 200)]
		Curb = 2,

		[StyleIdentity(3, "Ped", 92, 97, 102)]
		Pedestrian = 4,

		[StyleIdentity(4, "B", 74, 205, 151)]
		Bike = 16,

		[StyleIdentity(5, "C", 66, 132, 212)]
		Car = 32,

		[StyleIdentity(6, "T", 230, 210, 122)]
		Tram = 64,

		[StyleIdentity(7, "Bus", 170, 62, 48)]
		Bus = 128,

		[StyleIdentity(8, "TBus", 184, 70, 55)]
		Trolley = 256,

		[StyleIdentity(9, "EV", 222, 75, 109)]
		Emergency = 512,

		[StyleIdentity(10, "Train", 194, 146, 74)]
		Train = 1024,

		[StyleIdentity(11, "P", 74, 89, 161)]
		Parking = 2048,
	}

	#region Old

	[Flags]
	public enum OLD_LaneType
	{
		[StyleIdentity(0, "M", 0, 0, 0)]
		Empty = 0,

		[StyleIdentity(1, "M", 154, 203, 96)]
		Grass = 1,

		[StyleIdentity(2, "M", 99, 102, 107)]
		Pavement = 2,

		[StyleIdentity(3, "M", 79, 61, 55)]
		Gravel = 4,

		[StyleIdentity(4, "M", 72, 161, 73)]
		Trees = 8,

		[StyleIdentity(5, "C", 66, 132, 212)]
		Car = 16,

		[StyleIdentity(6, "Ped", 92, 97, 102)]
		Pedestrian = 32,

		[StyleIdentity(7, "C", 41, 153, 151)]
		Highway = 64,

		[StyleIdentity(8, "B", 74, 205, 151)]
		Bike = 128,

		[StyleIdentity(9, "T", 230, 210, 122)]
		Tram = 256,

		[StyleIdentity(10, "Bus", 170, 62, 48)]
		Bus = 512,

		[StyleIdentity(11, "Trolley", 184, 70, 55)]
		Trolley = 1024,

		[StyleIdentity(12, "EV", 222, 75, 109)]
		Emergency = 2048,

		[StyleIdentity(13, "Train", 194, 146, 74)]
		Train = 4096,

		[StyleIdentity(14, "P", 74, 89, 161)]
		Parking = 8192
	}
	#endregion
}