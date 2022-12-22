using System.Linq;
using System.Xml.Serialization;

using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Domain
{
	public class LaneInfo
	{
		public LaneType Type { get; set; }
		public LaneDirection Direction { get; set; }
		public LaneDecoration Decorations { get; set; }
		public ParkingAngle ParkingAngle { get; set; }
		public PropAngle PropAngle { get; set; }
		public FillerPadding FillerPadding { get; set; }

		public float? Elevation { get; set; }
		public float? CustomWidth { get; set; }
		public float? SpeedLimit { get; set; }

		[XmlIgnore] public float LaneWidth => CustomWidth ?? DefaultLaneWidth();

		public float DefaultLaneWidth()
		{
			if (Type == LaneType.Empty)
				return 1F;

			if (Type == LaneType.Parking)
			{
				if (ParkingAngle == ParkingAngle.Horizontal)
					return LaneSizeOptions.LaneSizes.HorizontalParkingSize;

				if (ParkingAngle == ParkingAngle.Diagonal || ParkingAngle == ParkingAngle.InvertedDiagonal)
					return LaneSizeOptions.LaneSizes.DiagonalParkingSize;
			}

			return Type.GetValues().Max(x => LaneSizeOptions.LaneSizes[x]);
		}
	}
}