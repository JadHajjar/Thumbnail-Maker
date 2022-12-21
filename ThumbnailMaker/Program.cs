using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using ThumbnailMaker.Controls;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker
{

	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			SlickControls.SlickCursors.Initialize();

			try
			{ Options.Current = ISave.Load<Options>("LaneOptions.tf"); }
			catch { Options.Current = new Options(); }

			if (args.Length > 0 && args[0] == "update")
			{
				try
				{
					var appdata = Options.Current.ExportFolder.IfEmpty(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
						, "Colossal Order", "Cities_Skylines", "BlankRoadBuilder", "Roads"));

					var files = Directory.Exists(appdata) ? Directory.GetFiles(appdata, "*.xml", SearchOption.AllDirectories) : new string[0];

					for (var i = 0; i < files.Length; i++)
					{
						LegacyUtil.LoadRoad(files[i]);

						File.Delete(files[i]);
					}

					DeleteAll(appdata);
				}
				catch { }
				return;
			}

			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm());
			}
			catch(Exception ex)
			{ MessageBox.Show(ex.ToString(), "App failed to start"); }
		}

		public static void DeleteAll(string directory)
		{
			if (!Directory.Exists(directory))
				return;
			foreach (var file in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories))
				File.Delete(file);

			foreach (var file in Directory.GetDirectories(directory, "*", SearchOption.AllDirectories))
				Directory.Delete(file);

			Directory.Delete(directory);
		}
	}
}