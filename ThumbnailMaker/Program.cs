using Extensions;

using System;
using System.Windows.Forms;

using ThumbnailMaker.Handlers;

namespace ThumbnailMaker
{

	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			SlickControls.SlickCursors.Initialize();

			try
			{ Options.Current = ISave.Load<Options>("LaneOptions.tf"); }
			catch { Options.Current = new Options(); }

			if (Environment.OSVersion.Version.Major == 6)
				SetProcessDPIAware();

			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm());
			}
			catch (Exception ex)
			{ MessageBox.Show(ex.ToString(), "App failed to start"); }
		}

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();
	}
}