﻿using Extensions;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

using Timer = System.Timers.Timer;

namespace ThumbnailMaker.Controls
{
	public partial class RoadConfigContainer : UserControl
	{
		private readonly Timer _systemWatcher;

		public event System.EventHandler<RoadInfo> LoadConfiguration;

		public RoadConfigContainer()
		{
			InitializeComponent();

			if (Options.Current == null)
				return;

			_systemWatcher = new Timer(1000);
			_systemWatcher.AutoReset = false;
			_systemWatcher.Elapsed += (s, e) => RefreshConfigs();
			_systemWatcher.Start();
		}

		public void RefreshConfigs(string fileToRefresh = null)
		{
			try
			{
				var appdata = Options.Current.ExportFolder.IfEmpty(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
				, "Colossal Order", "Cities_Skylines", "RoadBuilder", "Roads"));

				var controls = P_Configs.Controls.OfType<RoadConfigControl>().ToDictionary(x => x.FileName);
				var files = Directory.Exists(appdata) ? Directory.GetFiles(appdata, "*.xml", SearchOption.AllDirectories) : new string[0];
				var contents = files.ToDictionary(x => x, x =>
				{
					try { return LegacyUtil.LoadRoad(x); }
					catch { return null; }
				});

				this.TryInvoke(() =>
				{
					Loader.Hide();

					P_Configs.SuspendDrawing();
					if (!string.IsNullOrEmpty(fileToRefresh) && controls.ContainsKey(fileToRefresh))
					{
						controls[fileToRefresh].Dispose();
						controls.Remove(fileToRefresh);
					}

					for (var i = 0; i < files.Length; i++)
					{
						if (!controls.ContainsKey(files[i]) && contents[files[i]] != null)
							createControl(files[i]);
						else if (controls.ContainsKey(files[i]) && controls[files[i]].TimeSaved != new FileInfo(files[i]).LastWriteTime)
						{
							controls[files[i]].Dispose();
							createControl(files[i]);
						}
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

					P_Configs.OrderBy(x => (x as RoadConfigControl).TimeSaved);
					P_Configs.ResumeDrawing();
					P_Configs.Parent.PerformLayout();
				});
			}
			catch { }

			_systemWatcher.Start();
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
