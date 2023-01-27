using Extensions;

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
			{
				return 1F;
			}

			if (Type == LaneType.Parking)
			{
				if (ParkingAngle == ParkingAngle.Horizontal)
				{
					return LaneSizeOptions.LaneSizes.HorizontalParkingSize;
				}

				if (ParkingAngle == ParkingAngle.Diagonal || ParkingAngle == ParkingAngle.InvertedDiagonal)
				{
					return LaneSizeOptions.LaneSizes.DiagonalParkingSize;
				}
			}

			return Type.GetValues().Max(x => LaneSizeOptions.LaneSizes[x]);
		}

		public override string ToString()
		{
			return ToString(0);
		}

		public string ToString(int lanes)
		{
			var name = string.Empty;
			var types = Type.GetValues().Select(x => x.ToString());

			if (Type > LaneType.Pedestrian && Type != LaneType.Parking)
			{
				name += Direction.Switch(LaneDirection.Forward, "1WF ", LaneDirection.Backwards, "1WB ", "2W ");
			}

			if (lanes > 1)
			{
				name += $"{lanes}L ";
			}

			name += types.Count() > 1 ? $"Shared {types.ListStrings(" & ")}" : types.First();

			if (Decorations == LaneDecoration.None)
			{
				return name;
			}

			name += " with " + Decorations.GetValues().Select(x => x.ToString().FormatWords()).ListStrings(", ");

			return name;
		}
	}
}