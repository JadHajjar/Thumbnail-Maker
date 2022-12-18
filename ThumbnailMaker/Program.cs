using Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThumbnailMaker
{

	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			SlickControls.SlickCursors.Initialize();

			try
			{ Options.Current = ISave.Load<Options>("LaneOptions.tf"); }
			catch { Options.Current = new Options(); }

			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm());
			}
			catch(Exception ex)
			{ MessageBox.Show(ex.ToString(), "App failed to start"); }
		}
	}
}