﻿using Extensions;

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
		private readonly Dictionary<LaneClass, float> _sizes;
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

		public float this[LaneClass l]
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
							Version = 1,
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
						_sizes = new Dictionary<LaneClass, float>();

						for (var i = 0; i < savedSettings.LaneTypes.Count; i++)
						{
							var newType = savedSettings.Version < 1 ? RoadInfo.ConvertType((OLD_LaneType)savedSettings.LaneTypes[i]) : (LaneClass)savedSettings.LaneTypes[i];

							_sizes[newType] = savedSettings.LaneSizes[i];
						}

						foreach (LaneClass laneType in Enum.GetValues(typeof(LaneClass)))
						{
							if (!_sizes.ContainsKey(laneType))
								_sizes[laneType] = GetDefaultLaneWidth(laneType);
						}
					}

					return;
				}
			}
			catch { }

			_sizes = Enum.GetValues(typeof(LaneClass)).Cast<LaneClass>().ToDictionary(x => x, GetDefaultLaneWidth);
			_diagonalParkingSize = 4F;
			_horizontalParkingSize = 5.5F;
		}

		public static float GetDefaultLaneWidth(LaneClass type)
		{
			switch (type)
			{
				case LaneClass.Filler:
					return 3F;

				case LaneClass.Tram:
				case LaneClass.Car:
				case LaneClass.Bus:
				case LaneClass.Emergency:
					return 3F;

				case LaneClass.Pedestrian:
					return 2F;

				case LaneClass.Bike:
					return 2F;

				case LaneClass.Parking:
					return 2F;

				case LaneClass.Train:
					return 4F;

				case LaneClass.Curb:
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
