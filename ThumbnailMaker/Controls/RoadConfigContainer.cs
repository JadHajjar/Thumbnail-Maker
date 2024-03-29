﻿using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

using Timer = System.Timers.Timer;

namespace ThumbnailMaker.Controls
{
	public partial class RoadConfigContainer : UserControl
	{
		public static RoadConfigContainer _instance;
		private readonly OptionSelectionControl<RoadTypeFilter> RoadTypeControl;
		private readonly OptionSelectionControl<RoadSizeFilter> RoadSizeControl;

		public List<string> LoadedTags { get; private set; }
		public List<string> AutoTags { get; private set; }

		public event System.EventHandler<RoadInfo> LoadConfiguration;

		public RoadConfigContainer()
		{
			InitializeComponent();

			RoadTypeControl = new OptionSelectionControl<RoadTypeFilter>((_) => Properties.Resources.I_RoadType.Color(FormDesign.Design.ForeColor)) { Small = true };
			RoadSizeControl = new OptionSelectionControl<RoadSizeFilter>((_) => Properties.Resources.I_Size.Color(FormDesign.Design.ForeColor)) { Small = true };

			RoadTypeControl.SelectedItemChanged += (s, e) => TB_Search_TextChanged(null, null);
			RoadSizeControl.SelectedItemChanged += (s, e) => TB_Search_TextChanged(null, null);

			FLP_Options.Controls.Add(RoadTypeControl);
			FLP_Options.Controls.Add(RoadSizeControl);

			SlickTip.SetTo(RoadTypeControl, "Filter roads by their type");
			SlickTip.SetTo(RoadSizeControl, "Filter roads by their width");
			SlickTip.SetTo(B_Folder, "Open the folder containing all the road configurations");
			SlickTip.SetTo(B_ClearFilters, "Clear all filters");

			if (Options.Current == null)
			{
				return;
			}

			_instance = this;

			Loader.Loading = true;

			var _systemWatcher = new Timer(1000)
			{
				AutoReset = false
			};
			_systemWatcher.Elapsed += (s, e) => RefreshConfigs(s as Timer);
			_systemWatcher.Start();
		}

		public void RefreshConfigs(Timer timer = null)
		{
			try
			{
				var appdata = Options.Current.ExportFolder.IfEmpty(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
				, "Colossal Order", "Cities_Skylines", "RoadBuilder", "Roads"));

				var controls = GetControlDictionary();
				var files = Directory.Exists(appdata) ? Directory.GetFiles(appdata, "*.xml", SearchOption.AllDirectories) : new string[0];
				var contents = files.ToDictionary(x => x, x =>
				{
					try
					{ return LegacyUtil.LoadRoad(x); }
					catch { return null; }
				});

				foreach (var item in contents.ToList())
				{
					if (item.Value == null)
					{
						contents.Remove(item.Key);
					}
				}

				LoadedTags = contents.Values.SelectMany(x => x.Tags).Distinct((x, y) => x.Equals(y, StringComparison.CurrentCultureIgnoreCase)).ToList();
				AutoTags = contents.Values.SelectMany(x => x.AutoTags).Distinct((x, y) => x.Equals(y, StringComparison.CurrentCultureIgnoreCase)).ToList();

				this.TryInvoke(() =>
				{
					Loader.Hide();

					P_Configs.SuspendDrawing();

					try
					{
						for (var i = 0; i < files.Length; i++)
						{
							if (!controls.ContainsKey(files[i]) && contents[files[i]] != null)
							{
								createControl(files[i]);
							}
						}

						foreach (var item in controls)
						{
							if (!files.Any(x => x == item.Key))
							{
								item.Value.Dispose();
							}
						}

						foreach (var item in AutoTags.Concat(LoadedTags))
						{
							if (!FLP_Tags.GetControls<TagControl>().Any(x => x.Text.Equals(item, StringComparison.CurrentCultureIgnoreCase)))
							{
								var ctrl = new TagControl(item, true);
								ctrl.SelectionChanged += TB_Search_TextChanged;
								FLP_Tags.Controls.Add(ctrl);
							}
						}

						foreach (var item in FLP_Tags.Controls.OfType<TagControl>())
						{
							if (!LoadedTags.Contains(item.Text, StringComparer.CurrentCultureIgnoreCase) && !AutoTags.Contains(item.Text, StringComparer.CurrentCultureIgnoreCase))
							{
								item.Dispose();
							}
						}

						void createControl(string file)
						{
							var ctrl = new RoadConfigControl(file, contents[file], out var valid);

							if (!valid)
							{
								return;
							}

							ctrl.LoadConfiguration += Ctrl_LoadConfiguration;

							P_Configs.Controls.Add(ctrl);

							ctrl.BringToFront();
						}

						switch (LaneSizeOptions.LaneSizes.SortMode)
						{
							case RoadSortMode.DateCreated:
								P_Configs.OrderByDescending(x => (x as RoadConfigControl).Road.DateCreated);
								break;
							case RoadSortMode.RoadName:
								P_Configs.OrderBy(x => $"{(int)(x as RoadConfigControl).Road.RoadType}{(x as RoadConfigControl).Road.Name.RegexRemove("^RB[RHFP] ").RegexRemove(@"\d+([.,]\d+)?[um] ")}");
								break;
							case RoadSortMode.RoadTypeAndSize:
								P_Configs.OrderBy(x => (x as RoadConfigControl).Road.TotalRoadWidth + 10000 * (int)(x as RoadConfigControl).Road.RoadType);
								break;
						}
					}
					finally
					{
						P_Configs.ResumeDrawing();
						P_Configs.Parent?.PerformLayout();
					}
				});
			}
			catch { }

			timer?.Start();
		}

		private Dictionary<string, RoadConfigControl> GetControlDictionary()
		{
			var dic = new Dictionary<string, RoadConfigControl>();

			foreach (var item in P_Configs.Controls.OfType<RoadConfigControl>().ToList())
			{
				if (dic.ContainsKey(item.FileName))
				{
					item.TryInvoke(item.Dispose);
				}
				else
				{
					dic.Add(item.FileName, item);
				}
			}

			return dic;
		}

		private void Ctrl_LoadConfiguration(object sender, Domain.RoadInfo e)
		{
			LoadConfiguration?.Invoke(sender, e);
		}

		private void TB_Search_TextChanged(object sender, EventArgs e)
		{
			var selectedTags = FLP_Tags.GetControls<TagControl>().Where(x => x.Selected && !x.InvertSelected).Select(x => x.Text).ToList();
			var invertedTags = FLP_Tags.GetControls<TagControl>().Where(x => x.Selected && x.InvertSelected).Select(x => x.Text).ToList();

			P_Configs.SuspendDrawing();
			foreach (var item in P_Configs.Controls.OfType<RoadConfigControl>().ToList())
			{
				item.Visible = 
					!selectedTags.Any(x => !item.Road.Tags.Concat(item.Road.AutoTags).Any(y => y.Equals(x, StringComparison.CurrentCultureIgnoreCase)))
					&& !invertedTags.Any(x => item.Road.Tags.Concat(item.Road.AutoTags).Any(y => y.Equals(x, StringComparison.CurrentCultureIgnoreCase)))
					&& (string.IsNullOrWhiteSpace(TB_Search.Text)
					|| item.Road.Name.SearchCheck(TB_Search.Text)
					|| item.Road.Description.SearchCheck(TB_Search.Text))
					&& (RoadTypeControl.SelectedItem == RoadTypeFilter.AnyRoadType || item.Road.RoadType == (RoadType)((int)RoadTypeControl.SelectedItem - 1))
					&& (RoadSizeControl.SelectedItem == RoadSizeFilter.AnyRoadSize || Match(item.Road, RoadSizeControl.SelectedItem));
			}
			P_Configs.ResumeDrawing();
		}

		private bool Match(RoadInfo road, RoadSizeFilter selectedValue)
		{
			var width = road.TotalRoadWidth;

			switch (selectedValue)
			{
				case RoadSizeFilter.Tiny:
					return width <= 16F;
				case RoadSizeFilter.Small:
					return width > 16F && width <= 24F;
				case RoadSizeFilter.Medium:
					return width > 24F && width <= 32F;
				case RoadSizeFilter.Large:
					return width > 32F && width <= 48F;
				case RoadSizeFilter.VeryLarge:
					return width > 48F;
				default:
					return true;
			}
		}

		private void B_ClearCurrentlyEdited_Click(object sender, EventArgs e)
		{
			foreach (var item in FLP_Tags.Controls.OfType<TagControl>())
			{
				item.Selected = false;
			}

			if (!string.IsNullOrWhiteSpace(TB_Search.Text))
			{
				TB_Search.Text = string.Empty;
			}

			RoadSizeControl.SelectedItem = RoadSizeFilter.AnyRoadSize;
			RoadTypeControl.SelectedItem = RoadTypeFilter.AnyRoadType;
		}

		private void B_Folder_Click(object sender, EventArgs e)
		{
			var appdata = Options.Current.ExportFolder.IfEmpty(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
			, "Colossal Order", "Cities_Skylines", "RoadBuilder", "Roads"));

			Process.Start(appdata);
		}
	}
}
