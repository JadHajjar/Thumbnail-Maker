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
		public string SizeFont { get; set; } = "Adobe Gothic Std B";

		public static void Save()
		{
			Current.Save(Current.Name);
		}
	}
}
