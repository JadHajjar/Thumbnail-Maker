using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
		private readonly FilterSelectionControl<RoadTypeFilter> RoadTypeControl;
		private readonly FilterSelectionControl<RoadSize> RoadSizeControl;

		public List<string> LoadedTags { get; private set; }
		public List<string> AutoTags { get; private set; }

		public event System.EventHandler<RoadInfo> LoadConfiguration;

		public RoadConfigContainer()
		{
			InitializeComponent();

			RoadTypeControl = new FilterSelectionControl<RoadTypeFilter>(GetRoadTypeIcon) { Dock = DockStyle.Top };
			TLP_Options.Controls.Add(RoadTypeControl, 0, 0);

			RoadSizeControl = new FilterSelectionControl<RoadSize>(GetRoadSizeIcon) { Dock = DockStyle.Top };
			TLP_Options.Controls.Add(RoadSizeControl, 1, 0);

			RoadTypeControl.SelectedValueChanged += (s, e) => TB_Search_TextChanged(null, null);
			RoadSizeControl.SelectedValueChanged += (s, e) => TB_Search_TextChanged(null, null);

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

		private Image GetRoadTypeIcon(RoadTypeFilter roadType)
		{
			if (roadType == RoadTypeFilter.AnyRoadType)
				return Properties.Resources.L_D_0.Color(FormDesign.Design.IconColor);

			return ResourceManager.GetRoadType((RoadType)((int)roadType - 1), false, false);
		}

		private Image GetRoadSizeIcon(RoadSize roadType)
		{
			Color color;

			switch (roadType)
			{
				case RoadSize.Tiny: color = Color.FromArgb(82, 168, 164); break;
				case RoadSize.Small: color = Color.FromArgb(66, 153, 104); break;
				case RoadSize.Medium: color = Color.FromArgb(66, 98, 153); break;
				case RoadSize.Large: color = Color.FromArgb(173, 155, 80); break;
				case RoadSize.VeryLarge: color = Color.FromArgb(173, 94, 80); break;
				default: return Properties.Resources.L_D_0.Color(FormDesign.Design.IconColor);
			}

			return Properties.Resources.I_RoadSize.Color(color);
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
						contents.Remove(item.Key);
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

						switch (Options.Current.RoadSortMode)
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
						P_Configs.Parent.PerformLayout();
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
			var selectedTags = FLP_Tags.GetControls<TagControl>().Where(x => x.Selected).Select(x => x.Text).ToList();

			P_Configs.SuspendDrawing();
			foreach (var item in P_Configs.Controls.OfType<RoadConfigControl>().ToList())
			{
				item.Visible = !selectedTags.Any(x => !item.Road.Tags.Concat(item.Road.AutoTags).Any(y => y.Equals(x, StringComparison.CurrentCultureIgnoreCase)))
					&& (string.IsNullOrWhiteSpace(TB_Search.Text)
					|| item.Road.Name.SearchCheck(TB_Search.Text)
					|| item.Road.Description.SearchCheck(TB_Search.Text))
					&& (RoadTypeControl.SelectedValue == RoadTypeFilter.AnyRoadType || item.Road.RoadType == (RoadType)((int)RoadTypeControl.SelectedValue - 1))
					&& (RoadSizeControl.SelectedValue == RoadSize.AnyRoadSize || Match(item.Road, RoadSizeControl.SelectedValue));
			}
			P_Configs.ResumeDrawing();
		}

		private bool Match(RoadInfo road, RoadSize selectedValue)
		{
			var width = road.TotalRoadWidth;

			switch (selectedValue)
			{
				case RoadSize.Tiny:
					return width.IsWithin(0, 16.01F);
				case RoadSize.Small:
					return width.IsWithin(16.01F, 24.01F);
				case RoadSize.Medium:
					return width.IsWithin(24.01F, 32.01F);
				case RoadSize.Large:
					return width.IsWithin(32.01F, 48.01F);
				case RoadSize.VeryLarge:
					return width.IsWithin(48.01F, int.MaxValue);
				default:
					return true;
			}
		}

		private void B_ClearCurrentlyEdited_Click(object sender, EventArgs e)
		{
			foreach (var item in FLP_Tags.Controls.OfType<TagControl>())
			{
				item.Release();
			}

			if (string.IsNullOrWhiteSpace(TB_Search.Text))
				TB_Search_TextChanged(null, null);
			else
				TB_Search.Text = string.Empty;
		}
	}
}
