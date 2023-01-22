using Extensions;

using System;
using System.Collections.Generic;
using System.Data;
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

		public event System.EventHandler<RoadInfo> LoadConfiguration;

		public RoadConfigContainer()
		{
			InitializeComponent();

			if (Options.Current == null)
				return;

			_instance = this;

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

				this.TryInvoke(() =>
				{
					Loader.Hide();

					P_Configs.SuspendDrawing();

					try
					{
						for (var i = 0; i < files.Length; i++)
						{
							if (!controls.ContainsKey(files[i]) && contents[files[i]] != null)
								createControl(files[i]);
						}

						foreach (var item in controls)
						{
							if (!files.Any(x => x == item.Key))
								item.Value.Dispose();
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

						P_Configs.OrderBy(x => (x as RoadConfigControl).Road.DateCreated);
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

		private void TB_Size_TextChanged(object sender, EventArgs e)
		{
			foreach (var item in P_Configs.Controls.OfType<RoadConfigControl>().ToList())
			{
				item.Visible = string.IsNullOrWhiteSpace(TB_Size.Text)
					|| item.Road.Name.SearchCheck(TB_Size.Text)
					|| item.Road.Description.SearchCheck(TB_Size.Text);
			}
		}
	}
}
