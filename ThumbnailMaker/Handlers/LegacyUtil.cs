using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Legacy;

namespace ThumbnailMaker.Handlers
{
	internal class LegacyUtil
	{
		public const int CURRENT_VERSION = 1;

		public static int ExtractVersion(string xmlData)
		{
			try
			{
				var document = new XmlDocument();

				document.Load(xmlData);

				var version = document.DocumentElement.Attributes["Version"]?.Value;

				return version == null ? 0 : int.Parse(version);
			}
			catch { return 0; }
		}

		public static RoadInfo LoadRoad(string fileName)
		{
			var version = ExtractVersion(fileName);

			XmlSerializer xml;

			switch (version)
			{
				case CURRENT_VERSION: 
					xml = new XmlSerializer(typeof(RoadInfo));

					using (var stream = File.OpenRead(fileName))
						return (RoadInfo)xml.Deserialize(stream);

				case 0: 
					xml = new XmlSerializer(typeof(RoadInfo_V0));

					RoadInfo road;

					using (var stream = File.OpenRead(fileName))
						road = (RoadInfo)(RoadInfo_V0)xml.Deserialize(stream);

					try
					{
						Utilities.ExportRoad(road, fileName);

						File.Delete(fileName);
					}
					catch { }

					return road;
				default:
					throw new Exception("Unsupported Road Version");
			}

		}
	}
}
