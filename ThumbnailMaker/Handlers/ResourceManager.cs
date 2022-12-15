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

		public static Image GetRoadType(RoadType type, bool small) => GetImage($"RT_{(int)type}", small);

		public static Image GetImage(LaneType lane, bool small)
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

		public static Image GetImage(LaneDecoration decorations, bool small)
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

		public static Image GetImage(string name, bool small)
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

		public static void SetImage(LaneType lane, bool small, string fileName)
		{
			var field = lane.GetType().GetField(Enum.GetName(typeof(LaneType), lane));

			var attribute = Attribute.GetCustomAttribute(field, typeof(StyleIdentityAttribute)) as StyleIdentityAttribute;

			//SetImage(attribute.Id.ToString(), small, fileName);
		}

		public static void SetLogo(LaneType lane, bool small, string fileName) { }// => SetImage(nameof(Logo), small, fileName);
	}
}
