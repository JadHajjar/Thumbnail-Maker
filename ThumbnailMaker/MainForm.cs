using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ThumbnailMaker.Handlers;

namespace ThumbnailMaker
{
	public partial class MainForm : BasePanelForm
	{
		public MainForm()
		{
			InitializeComponent();

			FormDesign.Initialize(this, DesignChanged);

			SetPanel<PC_MainPage>(panelItem1);

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
			var bounds = Bounds;
			base.UIChanged();
			Bounds = bounds;
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
