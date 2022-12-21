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
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker
{
	public partial class PC_Options : PanelContent
	{
		public PC_Options()
		{
			InitializeComponent();

			PB_Small.Image = ResourceManager.Logo(true);
			PB_Large.Image = ResourceManager.Logo(false);

			foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
			{
				var ctrl = new LaneOptionControl(laneType);

				TLP.Controls.Add(ctrl);
			}

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

			SlickTip.SetTo(B_ReExport, "Re-generates all of your exported roads' thumbnails, useful in case you changed color or style options");
			SlickTip.SetTo(B_Theme, "Change the theme colors and UI scaling of the App");
			SlickTip.SetTo(CB_LHT, "Automatically flips the road once exported so it has the correct directions when using it in LHT mode");
			SlickTip.SetTo(CB_ColoredLanes, "Adds a colored background to the lanes on the thumbnail");
			SlickTip.SetTo(CB_AdvancedElevartion, "Allows you to manually set the elevation value of a lane");
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

		private void PB_Logo_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					ResourceManager.SetLogo(sender == PB_Small, openFileDialog.FileName);
				}
				else return;
			}
			else
				ResourceManager.SetLogo(sender == PB_Small, null);

			PB_Small.Image = ResourceManager.Logo(true);
			PB_Large.Image = ResourceManager.Logo(false);
		}

		private void PB_Small_Paint(object sender, PaintEventArgs e)
		{
			var pb = sender as SlickPictureBox;

			if (pb.Image == null)
			{
				e.Graphics.DrawIcon(Properties.Resources.I_EditImage.Color(pb.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor), pb.ClientRectangle);
			}
		}

		private void B_ReExport_Click(object sender, EventArgs e)
		{
			if (B_ReExport.Loading)
				return;

			var appdata = Options.Current.ExportFolder.IfEmpty(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
				, "Colossal Order", "Cities_Skylines", "RoadBuilder", "Roads"));
			
			B_ReExport.Loading = true;
			
			new Action(() =>
			{
				var files = Directory.Exists(appdata) ? Directory.GetFiles(appdata, "*.xml", SearchOption.AllDirectories) : new string[0];
				var contents = files.ToDictionary(x => x, x =>
				{
					try
					{
						var road = LegacyUtil.LoadRoad(x);

						Utilities.ExportRoad(road, Path.GetFileName(x));

						return road;
					}
					catch { return null; }
				});

				B_ReExport.Loading = false;

				MessagePrompt.Show($"Thumbnail generation complete, {contents.Count(x => x.Value != null)} roads rebuilt." +
					(contents.Any(x => x.Value == null) ? "\r\nSome roads failed to be rebuilt.":""), PromptButtons.OK, PromptIcons.Info);
			}).RunInBackground();
		}
	}
}