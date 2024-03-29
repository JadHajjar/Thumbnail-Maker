﻿using Extensions;

using SlickControls;

using System;
using System.IO;
using System.Windows.Forms;

using ThumbnailMaker.Handlers;

namespace ThumbnailMaker
{
	public partial class MainForm : BasePanelForm
	{
		public MainForm()
		{
			InitializeComponent();

			try
			{ FormDesign.Initialize(this, DesignChanged); }
			catch { }

			try
			{ SetPanel<PC_MainPage>(panelItem1); }
			catch (Exception ex)
			{ MessagePrompt.Show(ex.ToString(), "Error"); }

			var timer = new System.Timers.Timer(1000);

			timer.Elapsed += (s, e) =>
			{
				if (File.Exists(Path.Combine(Utilities.Folder, "Wake")))
				{
					SendKeys.SendWait("%{TAB}");

					this.TryInvoke(this.ShowUp);

					File.Delete(Path.Combine(Utilities.Folder, "Wake"));
				}
			};

			timer.Start();
		}

		protected override void UIChanged()
		{
			base.UIChanged();
		}

		private void panelItem2_OnClick(object sender, MouseEventArgs e)
		{
			SetPanel<PC_Options>(panelItem2);
		}

		private void panelItem1_OnClick(object sender, MouseEventArgs e)
		{
			SetPanel<PC_MainPage>(panelItem1);
		}
	}
}
