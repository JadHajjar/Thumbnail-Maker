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
		public static Bitmap Arrow(bool small) => GetImage(nameof(Arrow), small);

		public static Bitmap GetRoadType(RoadType type, bool lht, bool small) => GetImage($"RT_{(int)type}" + (lht ? "_LHT" : ""), small);

		public static Bitmap GetImage(LaneType lane, bool small)
		{
			var name = Enum.GetName(typeof(LaneType), lane);

			if (name == null)
				return null;

			var field = lane.GetType().GetField(name);

			var attribute = Attribute.GetCustomAttribute(field, typeof(StyleIdentityAttribute)) as StyleIdentityAttribute;

			if (attribute == null)
				return null;

			return GetImage($"C_{attribute.Id}", small);
		}

		public static Bitmap GetImage(LaneDecoration decorations, bool small)
		{
			var name = Enum.GetName(typeof(LaneDecoration), decorations);

			if (name == null)
				return null;

			var field = decorations.GetType().GetField(name);

			var attribute = Attribute.GetCustomAttribute(field, typeof(StyleIdentityAttribute)) as StyleIdentityAttribute;

			if (attribute == null)
				return null;

			return GetImage($"D_{attribute.Id}", small);
		}

		public static Bitmap GetImage(string name, bool small)
		{
			try
			{
				return (Bitmap)Properties.Resources.ResourceManager.GetObject($"{(small ? "S" : "L")}_{name}", Properties.Resources.Culture);
			}
			catch
			{
				return null;
			}
		}

		public static void SetLogo(bool small, string fileName)
		{
			var path = $"{Utilities.Folder}\\Resources\\{(small ? "S" : "L")}_Logo.png";
			
			if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
			{
				if (File.Exists(path))
					File.Delete(path);
			}
			else
				File.Copy(fileName, path, true);
		}

		public static Image Logo(bool small)
		{
			try
			{
				var path = $"{Utilities.Folder}\\Resources\\{(small ? "S" : "L")}_Logo.png";

				if (!File.Exists(path))
					return null;

				using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					var img = new byte[fileStream.Length];

					fileStream.Read(img, 0, img.Length);
					fileStream.Close();

					return Image.FromStream(new MemoryStream(img));
				}
			}
			catch { return null; }
		}
	}
}
