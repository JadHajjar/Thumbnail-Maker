using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ThumbnailMaker.Domain;

namespace ThumbnailMaker
{
	public class Options : ISave
	{
		public override string Name => "LaneOptions.tf";
		public static Options Current { get; set; }

		public Dictionary<LaneType, Color> LaneColors { get; set; } = new Dictionary<LaneType, Color>();
		public string TextFont { get; set; }
		public string ExportFolder { get; set; }
		public RegionType Region { get; set; }
		public bool LHT { get; set; }
		public bool ShowLaneColorsOnThumbnail { get; set; }
		public bool AdvancedElevation { get; set; }
		public bool VanillaWidths { get; set; }

		public static void Save()
		{
			try
			{ Current.Save(Current.Name); }
			catch { }
		}
	}
}
