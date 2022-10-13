using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ThumbnailMaker.Domain;

namespace ThumbnailMaker
{
	public class ResourceManager
	{
		public static Image Logo(bool small) => GetImage(nameof(Logo), small);

		public static Image Arrow(bool small) => GetImage(nameof(Arrow), small);

		public static Image GetImage(LaneType lane, bool small)
		{
			var field = lane.GetType().GetField(Enum.GetName(typeof(LaneType), lane));

			var attribute = Attribute.GetCustomAttribute(field, typeof(LaneIdentityAttribute)) as LaneIdentityAttribute;

			return GetImage(attribute.Id.ToString(), small);
		}

		private static Image GetImage(string name, bool small)
		{
			try
			{
				var fileName = $"Resources\\{(small ? 100 : 512)}_{name}.png";

				if (!File.Exists(fileName))
				{
					return null;
				}

				using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
				{
					var img = new byte[fileStream.Length];

					fileStream.Read(img, 0, img.Length);
					fileStream.Close();

					return Image.FromStream(new MemoryStream(img));
				}
			}
			catch
			{ }

			return null;
		}

		public static void SetImage(LaneType lane, bool small, string fileName)
		{
			var field = lane.GetType().GetField(Enum.GetName(typeof(LaneType), lane));

			var attribute = Attribute.GetCustomAttribute(field, typeof(LaneIdentityAttribute)) as LaneIdentityAttribute;

			if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
			{
				if (File.Exists($"Resources\\{(small ? 100 : 512)}_{attribute.Id}.png"))
					File.Delete($"Resources\\{(small ? 100 : 512)}_{attribute.Id}.png");
			}
			else
				File.Copy(fileName, $"Resources\\{(small ? 100 : 512)}_{attribute.Id}.png", true);
		}
	}
}
