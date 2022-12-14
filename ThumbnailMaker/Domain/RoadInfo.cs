using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ThumbnailMaker.Domain
{
	public class RoadInfo
	{
		[XmlAttribute] public int Version { get; set; }
		public string Name { get; set; }
		public string CustomName { get; set; }
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
		public List<LaneInfo> Lanes { get; set; }
		public byte[] SmallThumbnail { get; set; }
		public byte[] LargeThumbnail { get; set; }
		public byte[] TooltipImage { get; set; }
	}
}
