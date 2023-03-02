using System;
using System.Drawing;
using System.IO;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker
{
	public class ResourceManager
	{
		public static Bitmap Arrow(bool small)
		{
			return GetImage(nameof(Arrow), small);
		}

		public static Bitmap GetRoadType(RoadType type, bool lht, bool small)
		{
			return GetImage($"RT_{(int)type}" + (lht ? "_LHT" : ""), small);
		}

		public static Bitmap GetImage(LaneType lane, bool small)
		{
			var name = Enum.GetName(typeof(LaneType), lane);

			if (name == null)
			{
				return null;
			}

			var field = lane.GetType().GetField(name);


			if (!(Attribute.GetCustomAttribute(field, typeof(StyleIdentityAttribute)) is StyleIdentityAttribute attribute))
			{
				return null;
			}

			return GetImage($"C_{attribute.Id}", small);
		}

		public static Bitmap GetImage(LaneDecoration decorations, bool small)
		{
			var name = Enum.GetName(typeof(LaneDecoration), decorations);

			if (name == null)
			{
				return null;
			}

			var field = decorations.GetType().GetField(name);

			if (!(Attribute.GetCustomAttribute(field, typeof(StyleIdentityAttribute)) is StyleIdentityAttribute attribute))
			{
				return null;
			}

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
			var path = Path.Combine(Utilities.Folder, "Resources", $"{(small ? "S" : "L")}_Logo.png");

			Directory.CreateDirectory(Path.Combine(Utilities.Folder, "Resources"));

			if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			else
			{
				File.Copy(fileName, path, true);
			}
		}

		public static Image Logo(bool small)
		{
			try
			{
				var path = $"{Utilities.Folder}\\Resources\\{(small ? "S" : "L")}_Logo.png";

				if (!File.Exists(path))
				{
					return null;
				}

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
