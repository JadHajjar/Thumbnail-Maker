using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using ThumbnailMaker.Controls;
using ThumbnailMaker.Domain;

namespace ThumbnailMaker
{
	public partial class PC_Options : PanelContent
	{
		public PC_Options()
		{
			InitializeComponent();

			foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
			{
				//if (laneType == LaneType.Train)
				//	continue;

				var ctrl = new LaneOptionControl(laneType);

				TLP.Controls.Add(ctrl);
			}

			//TLP.Controls.Add(new LaneOptionControl("Arrow"));
			//TLP.Controls.Add(new LaneOptionControl("Logo"));

			using (var fontsCollection = new InstalledFontCollection())
			{
				DD_Font.Items = fontsCollection.Families;
				DD_Font.Conversion = (x) => (x as FontFamily)?.Name ?? x?.ToString();
			}

			DD_Font.FontDropdown = true;
			DD_Font.SelectedItem = Options.Current.TextFont;

			TB_ExportFolder.Text = Options.Current.ExportFolder;
			CB_LHT.Checked = Options.Current.LHT;
			CB_ColoredLanes.Checked = Options.Current.ShowLaneColorsOnThumbnail;
			CB_AdvancedElevartion.Checked = Options.Current.AdvancedElevation;
		}


		protected override void UIChanged()
		{
			base.UIChanged();
		}

		private void DD_Font_TextChanged(object sender, EventArgs e)
		{
			Options.Current.TextFont = DD_Font.Text;
			Options.Save();
		}

		private void B_Theme_Click(object sender, EventArgs e)
		{
			Form.PushPanel<PC_ThemeChanger>(null);
		}

		private void TB_ExportFolder_TextChanged(object sender, EventArgs e)
		{
			Options.Current.ExportFolder = TB_ExportFolder.Text;
			Options.Save();
		}

		private void CB_LHT_CheckChanged(object sender, EventArgs e)
		{
			Options.Current.LHT = CB_LHT.Checked;
			Options.Current.ShowLaneColorsOnThumbnail = CB_ColoredLanes.Checked;
			Options.Current.AdvancedElevation = CB_AdvancedElevartion.Checked;
			Options.Save();
		}
	}
}