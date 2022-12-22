using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbnailMaker.Domain
{
	[Flags]
	public enum FillerPadding
	{
		Unset = 0,
		Left = 1,
		Right = 2,
	}
}
