using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

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

		public static Image GetImage(string name, bool small)
		{
			try
			{
				var fileName = $"{Utilities.Folder}\\Resources\\{(small ? 100 : 512)}_{name}.png";

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

			SetImage(attribute.Id.ToString(), small, fileName);
		}

		public static void SetLogo(LaneType lane, bool small, string fileName) => SetImage(nameof(Logo), small, fileName);

		public static void SetArrow(LaneType lane, bool small, string fileName) => SetImage(nameof(Arrow), small, fileName);

		public static void SetImage(string name, bool small, string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
			{
				if (File.Exists($"{Utilities.Folder}\\Resources\\{(small ? 100 : 512)}_{name}.png"))
					File.Delete($"{Utilities.Folder}\\Resources\\{(small ? 100 : 512)}_{name}.png");
			}
			else
				File.Copy(fileName, $"{Utilities.Folder}\\Resources\\{(small ? 100 : 512)}_{name}.png", true);
		}
	}
}
