using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Domain
{
	public class RoadInfo
	{
		[XmlAttribute] public int Version { get; set; }
		public string Name { get; set; }
		public string CustomName { get; set; }
		public string CustomDescription { get; set; }
		public string Description { get; set; }
		public string CustomText { get; set; }
		public DateTime DateCreated { get; set; }
		public RoadType RoadType { get; set; }
		public RegionType RegionType { get; set; }
		public TextureType SideTexture { get; set; }
		public BridgeTextureType BridgeSideTexture { get; set; }
		public AsphaltStyle AsphaltStyle { get; set; }
		public float RoadWidth { get; set; }
		public float BufferWidth { get; set; }
		public int SpeedLimit { get; set; }
		public bool LHT { get; set; }
		public bool VanillaWidth { get; set; }
		public bool HighwayRules { get; set; }
		public bool CanCrossLanes { get; set; }
		public List<string> Tags { get; set; }
		public List<LaneInfo> Lanes { get; set; }
		public RoadElevation[] DisabledElevations { get; set; }
		public byte[] SmallThumbnail { get; set; }
		public byte[] LargeThumbnail { get; set; }
		public byte[] TooltipImage { get; set; }

		[XmlIgnore] public float TotalRoadWidth => Math.Max(RoadWidth, Utilities.VanillaWidth(VanillaWidth, Utilities.CalculateRoadSize(Lanes, BufferWidth)));
		[XmlIgnore] public List<string> AutoTags => Utilities.GetAutoTags(this).ToList();
	}
}
