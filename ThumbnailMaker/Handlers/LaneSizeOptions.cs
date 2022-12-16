using Extensions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ThumbnailMaker.Domain
{
	public class LaneSizeOptions
	{
		private readonly Dictionary<LaneType, float> _sizes;
		private float _diagonalParkingSize;
		private float _horizontalParkingSize;

		public float DiagonalParkingSize
		{
			get => _diagonalParkingSize; 
			set
			{
				_diagonalParkingSize = value;

				Save();
			}
		}

		public float HorizontalParkingSize
		{
			get => _horizontalParkingSize;
			set
			{
				_horizontalParkingSize = value;

				Save();
			}
		}

		public float this[LaneType l]
		{
			get => _sizes[l];
			set
			{
				_sizes[l] = value;

				Save();
			}
		}

		private void Save()
		{
			try
			{
				var appdata = Directory.GetParent(Options.Current.ExportFolder.IfEmpty(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
					, "Colossal Order", "Cities_Skylines", "BlankRoadBuilder", "Roads"))).FullName;

				var xML = new XmlSerializer(typeof(SavedSettings));

				using (var stream = File.Create(Path.Combine(appdata, "LaneSizes.xml")))
					xML.Serialize(stream, new SavedSettings
					{
						Version = 1,
						DiagonalParkingSize = _diagonalParkingSize,
						HorizontalParkingSize = _horizontalParkingSize,
						LaneTypes = _sizes.Keys.Cast<int>().ToList(),
						LaneSizes = _sizes.Values.ToList(),
					});
			}
			catch { }
		}

		public static LaneSizeOptions LaneSizes { get; private set; } = new LaneSizeOptions();

		public static void Refresh() => LaneSizes = new LaneSizeOptions();

		public LaneSizeOptions()
		{
			try
			{
				var appdata = Directory.GetParent(Options.Current.ExportFolder.IfEmpty(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
					, "Colossal Order", "Cities_Skylines", "BlankRoadBuilder", "Roads"))).FullName;

				if (File.Exists(Path.Combine(appdata, "LaneSizes.xml")))
				{
					var x = new XmlSerializer(typeof(SavedSettings));

					using (var stream = File.OpenRead(Path.Combine(appdata, "LaneSizes.xml")))
					{
						var savedSettings = (SavedSettings)x.Deserialize(stream);

						_diagonalParkingSize = savedSettings.DiagonalParkingSize;
						_horizontalParkingSize = savedSettings.HorizontalParkingSize;
						_sizes = new Dictionary<LaneType, float>();

						for (var i = 0; i < savedSettings.LaneTypes.Count; i++)
						{
							var newType = savedSettings.Version >= 1 ? (LaneType)savedSettings.LaneTypes[i] : Legacy.RoadInfo_V0.ConvertType((Legacy.LaneType_V0)savedSettings.LaneTypes[i]);

							_sizes[newType] = savedSettings.LaneSizes[i];
						}

						foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
						{
							if (!_sizes.ContainsKey(laneType))
								_sizes[laneType] = GetDefaultLaneWidth(laneType);
						}

						if (savedSettings.Version < 1)
							Save();
					}

					return;
				}
			}
			catch { }

			_sizes = Enum.GetValues(typeof(LaneType)).Cast<LaneType>().ToDictionary(x => x, GetDefaultLaneWidth);
			_diagonalParkingSize = 4F;
			_horizontalParkingSize = 5.5F;
		}

		public static float GetDefaultLaneWidth(LaneType type)
		{
			switch (type)
			{
				case LaneType.Filler:
					return 3F;

				case LaneType.Tram:
				case LaneType.Car:
				case LaneType.Bus:
				case LaneType.Emergency:
					return 3F;

				case LaneType.Pedestrian:
					return 2F;

				case LaneType.Bike:
					return 2F;

				case LaneType.Parking:
					return 2F;

				case LaneType.Train:
					return 4F;

				case LaneType.Curb:
				case LaneType.Empty:
					return 1F;
			}

			return 3F;
		}

		public class SavedSettings
		{
			public int Version { get; set; }
			public float DiagonalParkingSize { get; set; }
			public float HorizontalParkingSize { get; set; }
			public List<int> LaneTypes { get; set; }
			public List<float> LaneSizes { get; set; }
		}
	}
}
