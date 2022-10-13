using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThumbnailMaker
{
	public partial class MainForm : BasePanelForm
	{
		public MainForm()
		{
			InitializeComponent();

			FormDesign.Initialize(this, DesignChanged);

			SetPanel<PC_MainPage>(panelItem1);
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
