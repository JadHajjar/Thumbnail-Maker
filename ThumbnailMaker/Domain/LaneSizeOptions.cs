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
		}

		public float HorizontalParkingSize
		{
			get => _horizontalParkingSize;
		}

		public float this[LaneType l]
		{
			get => _sizes[l];
			set
			{
				_sizes[l] = value;

				try
				{
					var appdata = Directory.GetParent(Options.Current.ExportFolder.IfEmpty(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
						, "Colossal Order", "Cities_Skylines", "BlankRoadBuilder", "Roads"))).FullName;

					var xML = new XmlSerializer(typeof(SavedSettings));

					using (var stream = File.Create(Path.Combine(appdata, "LaneSizes.xml")))
						xML.Serialize(stream, new SavedSettings
						{
							DiagonalParkingSize = _diagonalParkingSize,
							HorizontalParkingSize = _horizontalParkingSize,
							LaneTypes = _sizes.Keys.Cast<int>().ToList(),
							LaneSizes = _sizes.Values.ToList(),
						});
				}
				catch { }
			}
		}

		public static LaneSizeOptions LaneSizes { get; set; } = new LaneSizeOptions();

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

						foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
						{
							var index = savedSettings.LaneTypes.IndexOf((int)laneType);

							_sizes[laneType] = index != -1 ? savedSettings.LaneSizes[index] : GetDefaultLaneWidth(laneType);
						}
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
				case LaneType.Empty:
				case LaneType.Grass:
				case LaneType.Pavement:
				case LaneType.Gravel:
					return 3F;

				case LaneType.Trees:
					return 4F;

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

				case LaneType.Highway:
				case LaneType.Train:
					return 4F;

				case LaneType.Sidewalk:
					return 1F;
			}

			return 3F;
		}

		public class SavedSettings
		{
			public float DiagonalParkingSize { get; set; }
			public float HorizontalParkingSize { get; set; }
			public List<int> LaneTypes { get; set; }
			public List<float> LaneSizes { get; set; }
		}
	}
}
