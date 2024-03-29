﻿using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using ThumbnailMaker.Controls;
using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker
{
	public partial class PC_MainPage : PanelContent
	{
		private bool refreshPaused = true;

		public PC_MainPage()
		{
			InitializeComponent();

			RoadTypeControl = new OptionSelectionControl<RoadType>(t => ResourceManager.GetRoadType(t, false, false)) { Dock = DockStyle.Top, Text = "Road Type" };
			TLP_TopControls.Controls.Add(RoadTypeControl, 0, 0);
			RegionTypeControl = new OptionSelectionControl<RegionType>((g, r, t) => ThumbnailHandler.DrawSpeedSignSmall(g, t, 20, r)) { Dock = DockStyle.Top, Text = "Speed Sign Region" };
			TLP_TopControls.Controls.Add(RegionTypeControl, 1, 0);
			SideTextureControl = new OptionSelectionControl<TextureType>(GetTextureIcon) { Dock = DockStyle.Top, Text = "Ground Side-Texture" };
			TLP_TopControls.Controls.Add(SideTextureControl, 2, 0);
			BridgeSideTextureControl = new OptionSelectionControl<BridgeTextureType>(GetTextureIcon) { Dock = DockStyle.Top, Text = "Bridge Side-Texture" };
			TLP_TopControls.Controls.Add(BridgeSideTextureControl, 3, 0);
			AsphaltTextureControl = new OptionSelectionControl<AsphaltStyle>(GetTextureIcon) { Dock = DockStyle.Top, Text = "Asphalt Style" };
			TLP_TopControls.Controls.Add(AsphaltTextureControl, 4, 0);
			ElevationTypeControl = new OptionMultiSelectionControl<RoadElevation>() { Dock = DockStyle.Top, Text = "Elevations" };
			TLP_TopControls.Controls.Add(ElevationTypeControl, 5, 0);

			RegionTypeControl.SelectedItem = Options.Current.Region;

			RoadTypeControl.SelectedItemChanged += (s, e) => SetupType(RoadTypeControl.SelectedItem);
			SideTextureControl.SelectedItemChanged += (s, e) => RefreshPreview();
			AsphaltTextureControl.SelectedItemChanged += (s, e) => RefreshPreview();
			RegionTypeControl.SelectedItemChanged += (s, e) =>
			{
				TB_SpeedLimit.LabelText = $"Speed Limit ({(RegionTypeControl.SelectedItem == RegionType.USA ? "mph" : "km/h")})";

				RoadLane.GlobalSpeed = int.MinValue;
				Options.Current.Region = GetRegion();
				Options.Save();

				RefreshPreview();
			};

			SetupType(RoadTypeControl.SelectedItem);

			var size = UI.Scale(new Size(65, 24), UI.WindowsScale);
			using (var img = new Bitmap(size.Width * 2, size.Height * 2))
			using (var g = Graphics.FromImage(img))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				using (var path = new Rectangle(size.Width - 1, size.Height, size.Width - 1, size.Height - 1).RoundedRect((int)(5*UI.WindowsScale), false))
				{
					g.FillPath(new SolidBrush(Color.FromArgb(35, 35, 40)), path);
				}

				g.DrawImage(Properties.Resources.I_Copy.Color(Color.White), new Rectangle(7 + size.Width, 4 + size.Height, 16, 16));
				g.DrawString("COPY", new Font(UI.FontFamily, 8.25F), Brushes.White, new Rectangle(size.Width + 23, 0 + size.Height, size.Width - 23, size.Height), new StringFormat
				{
					LineAlignment = StringAlignment.Center,
					Alignment = StringAlignment.Center,
				});
				g.DrawLine(Pens.White, size.Width + 1, size.Height + 2, size.Width + 6, size.Height + 2);
				g.DrawLine(Pens.White, size.Width + 1, size.Height + 2, size.Width + 1, size.Height + 7);
				g.DrawLine(Pens.White, size.Width + 1, size.Height + 2, size.Width + 5, size.Height + 6);

				PB.Cursor = L_RoadDesc.Cursor = L_RoadName.Cursor = new Cursor(img.GetHicon());
			}

			SlickTip.SetTo(TB_Size, "Manually specify the total road width, which includes the asphalt and pavement");
			SlickTip.SetTo(TB_BufferSize, "Represents the distance between the sidewalks and the lanes next to them");
			SlickTip.SetTo(TB_SpeedLimit, "Manually specify the default speed limit of the road");
			SlickTip.SetTo(TB_CustomText, "Add custom text to the thumbnail");

			SlickTip.SetTo(L_RoadName, "Copy the generated road name into your clipboard");
			SlickTip.SetTo(L_RoadDesc, "Copy the generated road description into your clipboard");
			SlickTip.SetTo(B_EditName, "Manually change the road's name");
			SlickTip.SetTo(B_EditDesc, "Manually change the road's description");
			SlickTip.SetTo(B_SaveThumb, "Saves the displayed thumbnail on your computer");
			SlickTip.SetTo(B_ViewSavedRoads, "Show/Hide the saved roads panel");
			SlickTip.SetTo(B_Export, "Exports the road configuration to the Road Builder folder to be generated");
			SlickTip.SetTo(B_AddTag, "Add a custom tag to this road");

			SlickTip.SetTo(B_Options, "Change the colors & icons of lane types as well as other options");
			SlickTip.SetTo(B_DuplicateFlip, "Duplicates the current lanes to the right and flips their direction");
			SlickTip.SetTo(B_FlipLanes, "Flips the whole road to create its opposite variation");
			SlickTip.SetTo(B_AddLane, "Add a new empty lane\r\nRight click: positions at the start\r\nMiddle click: positions it at the end\r\nLeft click: positions it at right side of the asphalt");
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Control | Keys.F))
			{
				if (RCC.Parent.Width == 0)
					B_ViewSavedRoads_Click(null, null);
				else
					RCC.TB_Search.Focus();
			}

			if (keyData == (Keys.Control | Keys.Tab))
			{
				B_ViewSavedRoads_Click(null, null);
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private RoadLane AddLaneControl(ThumbnailLaneInfo item)
		{
			var ctrl = new RoadLane(item);

			P_Lanes.Controls.Add(ctrl);

			ctrl.BringToFront();

			return ctrl;
		}

		private void B_AddLane_MouseClick(object sender, MouseEventArgs e)
		{
			var ctrl = new RoadLane();

			P_Lanes.Controls.Add(ctrl);

			var rightSidewalk = P_Lanes.Controls.OfType<RoadLane>().LastOrDefault(x => x.Lane.Type == LaneType.Curb && x.Lane.Direction == LaneDirection.Forward);

			if (rightSidewalk != null && e.Button != MouseButtons.Right && e.Button != MouseButtons.Middle)
			{
				P_Lanes.Controls.SetChildIndex(ctrl, P_Lanes.Controls.GetChildIndex(rightSidewalk) + 1);
			}
			else if (e.Button == MouseButtons.Middle)
			{
				ctrl.BringToFront();
			}
			else
			{
				ctrl.SendToBack();
			}

			var frm = new RoadTypeSelector(ctrl, B_AddLane);

			frm.FormClosed += (s, _) => RefreshPreview();
		}

		private void B_AddTag_Click(object sender, EventArgs e)
		{
			var frm = new AddTagForm(RCC.LoadedTags, FLP_Tags.GetControls<TagControl>().Select(x => x.Text)) { Location = FLP_Tags.PointToScreen(new Point(3, 4)), MinimumSize = new Size(TLP_Right.Width - 5, 0), MaximumSize = new Size(TLP_Right.Width - 5, 999) };

			frm.TagAdded += Frm_TagAdded;
			frm.TagRemoved += Frm_TagRemoved;

			frm.Show();
		}

		private void B_Clear_Click(object sender, EventArgs e)
		{
			C_CurrentlyEditing.Clear();
			refreshPaused = true;
			TB_RoadName.Text = string.Empty;
			TB_RoadDesc.Text = string.Empty;
			TB_Size.Text = string.Empty;
			TB_SpeedLimit.Text = string.Empty;
			P_Lanes.Controls.Clear(true);

			ElevationTypeControl.ResetValue();
			AsphaltTextureControl.SelectedItem = AsphaltStyle.Asphalt;
			refreshPaused = false;
			SetupType(RoadTypeControl.SelectedItem);
		}

		private void B_CopyDesc_Click(object sender, EventArgs e)
		{
			var roadInfo = GetRoadInfo();
			var desc = Utilities.GetRoadDescription(roadInfo);

			Clipboard.SetText(desc.Substring(0, Math.Min(1024, desc.Length)));
		}

		private void B_CopyRoadName_Click(object sender, EventArgs e)
		{
			var roadInfo = GetRoadInfo();
			var name = Utilities.GetRoadName(roadInfo);

			Clipboard.SetText(name.Substring(0, Math.Min(32, name.Length)));
		}

		private void B_DuplicateFlip_Click(object sender, EventArgs e)
		{
			refreshPaused = true;
			var leftSidewalk = P_Lanes.Controls.OfType<RoadLane>().FirstOrDefault(x => x.Lane.Type == LaneType.Curb && x.Lane.Direction == LaneDirection.Backwards);
			var rightSidewalk = P_Lanes.Controls.OfType<RoadLane>().LastOrDefault(x => x.Lane.Type == LaneType.Curb && x.Lane.Direction == LaneDirection.Forward);
			var lanes = P_Lanes.Controls.OfType<RoadLane>().Where(x =>
			{
				if (leftSidewalk != null && P_Lanes.Controls.IndexOf(x) >= P_Lanes.Controls.IndexOf(leftSidewalk))
				{
					return false;
				}

				if (rightSidewalk != null && P_Lanes.Controls.IndexOf(x) <= P_Lanes.Controls.IndexOf(rightSidewalk))
				{
					return false;
				}

				return true;
			}).ToList();
			var leftlanes = P_Lanes.Controls.OfType<RoadLane>().Where(x =>
			{
				if (leftSidewalk != null && P_Lanes.Controls.IndexOf(x) >= P_Lanes.Controls.IndexOf(leftSidewalk))
				{
					return true;
				}

				return false;
			}).ToList();

			if (lanes.FirstOrDefault()?.Lane.Type == LaneType.Filler)
			{
				lanes.RemoveAt(0);
			}

			foreach (var item in lanes)
			{
				var ctrl = item.Duplicate();

				if (ctrl.Lane.Direction == LaneDirection.Forward)
				{
					ctrl.Lane.Direction = LaneDirection.Backwards;
				}
				else if (ctrl.Lane.Direction == LaneDirection.Backwards)
				{
					ctrl.Lane.Direction = LaneDirection.Forward;
				}

				ctrl.Dock = DockStyle.Top;

				P_Lanes.Controls.Add(ctrl);

				if (rightSidewalk != null)
				{
					P_Lanes.Controls.SetChildIndex(ctrl, P_Lanes.Controls.IndexOf(rightSidewalk) + 1);
				}
				else
				{
					ctrl.BringToFront();
				}
			}

			if (rightSidewalk != null && P_Lanes.Controls.IndexOf(rightSidewalk) <= 1)
			{
				while (P_Lanes.Controls.IndexOf(rightSidewalk) >= 0)
				{
					P_Lanes.Controls.RemoveAt(0);
				}

				foreach (var item in leftlanes)
				{
					var ctrl = item.Duplicate();

					if (ctrl.Lane.Direction == LaneDirection.Forward)
					{
						ctrl.Lane.Direction = LaneDirection.Backwards;
					}
					else if (ctrl.Lane.Direction == LaneDirection.Backwards)
					{
						ctrl.Lane.Direction = LaneDirection.Forward;
					}

					ctrl.Dock = DockStyle.Top;

					P_Lanes.Controls.Add(ctrl);

					P_Lanes.Controls.SetChildIndex(ctrl, 0);
				}
			}

			refreshPaused = false;
			RefreshPreview();
		}

		private void B_EditDesc_Click(object sender, EventArgs e)
		{
			TB_RoadDesc.MultiLine = true;
			TB_RoadDesc.MaxLength = 1024;
			TB_RoadDesc.Height = (int)(120 * UI.FontScale);
			TB_RoadDesc.Show();
			L_RoadDesc.Hide();
			B_EditDesc.Hide();
			TB_RoadDesc.Focus();
		}

		private void B_Export_Click(object sender, EventArgs e)
		{
			try
			{
				var lanes = GetLanes();

				if (lanes.Count == 0)
				{
					return;
				}

				var roadInfo = GetRoadInfo(lanes);

				if (roadInfo.Lanes.FindIndex(x => x.Type == LaneType.Curb) + 1 == roadInfo.Lanes.FindLastIndex(x => x.Type == LaneType.Curb))
				{
					ShowPrompt("Road must have at least one lane on the asphalt to be exported", "Can't export road", PromptButtons.OK, PromptIcons.Info);
					return;
				}

				var file = Utilities.ExportRoad(roadInfo, C_CurrentlyEditing.Road == null ? null : Path.GetFileName(C_CurrentlyEditing.Control.FileName));

				C_CurrentlyEditing.Control?.Dispose();

				RCC.RefreshConfigs();

				C_CurrentlyEditing.SetRoad(RCC.P_Configs.Controls.OfType<RoadConfigControl>().FirstOrDefault(x => x.FileName.Equals(file, StringComparison.InvariantCultureIgnoreCase)));
			}
			catch (Exception ex) { ShowPrompt(ex.Message, "Error", PromptButtons.OK, PromptIcons.Error); }
		}

		private void B_FlipLanes_Click(object sender, EventArgs e)
		{
			refreshPaused = true;
			foreach (var item in P_Lanes.Controls.OfType<RoadLane>().ToList())
			{
				item.BringToFront();

				if (item.Lane.Type == LaneType.Curb)
				{
					item.Lane.Direction = item.Lane.Direction == LaneDirection.Forward ? LaneDirection.Backwards : LaneDirection.Forward;
				}

				item.Invalidate();
			}

			refreshPaused = false;
			RefreshPreview();
		}

		private void B_Options_Click(object sender, EventArgs e)
		{
			Form.PushPanel<PC_Options>(null);
		}

		private void B_Save_Click(object sender, EventArgs e)
		{
			try
			{
				var frm = new SaveThumbDialog(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

				if (frm.ShowDialog() != DialogResult.OK)
				{
					return;
				}

				var folder = frm.SelectedPath;
				var matched = false;
				var files = new List<(string, bool, bool)>
				{
					("asset_thumb.png", true, false),
					("PreviewImage.png", false, false),
					("snapshot.png", false, false),
					("thumbnail.png", false, false),
					("tooltip.png", true, true),
					("asset_tooltip.png", true, true),
				};

				foreach (var item in files)
				{
					if (CrossIO.FileExists(CrossIO.Combine(folder, item.Item1)))
					{
						save(item.Item1, item.Item2, item.Item3);

						matched = true;
					}
				}

				if (!matched)
				{
					save(L_RoadName.Text + ".png", frm.CB_Small.Checked, frm.CB_Tooltip.Checked);
				}

				void save(string filename, bool small, bool toolTip)
				{
					var width = toolTip ? 492 : small ? 109 : 512;
					var height = toolTip ? 147 : small ? 100 : 512;

					var lanes = GetLanes();

					using (var img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
					using (var g = Graphics.FromImage(img))
					{
						DrawThumbnail(g, lanes, small, toolTip);

						var FileName = CrossIO.Combine(folder, filename.EscapeFileName());

						img.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);

						Notification.Create("Thumbnail Saved", "Your thumbnail was saved at:\n" + FileName, PromptIcons.Info, () =>
						{
							Process.Start(new ProcessStartInfo
							{
								FileName = "explorer",
								Arguments = $"/e, /select, \"{FileName}\""
							});
						}).Show(Form, 15);
					}
				}
			}
			catch (Exception ex) { ShowPrompt(ex.Message, "Error", PromptButtons.OK, PromptIcons.Error); }
		}

		private void B_ViewSavedRoads_Click(object sender, EventArgs e)
		{
			B_ViewSavedRoads.Text = RCC.Parent.Width.If(0, "Hide Roads", "Load Road");
			B_ViewSavedRoads.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
			RCC.Parent.Width = RCC.Parent.Width.If(0, 15 + RCC.Parent.Padding.Horizontal + (Options.Current.RoadConfigColumns * (12 + (int)(100 * UI.UIScale))), 0);
			RCC.Parent.Visible = RCC.Parent.Width != 0;

			Form.OnNextIdle(P_Lanes.PerformLayout);
			RCC.TB_Search.Focus();
		}

		private void C_CurrentlyEditing_VisibleChanged(object sender, EventArgs e)
		{
			B_Export.Text = C_CurrentlyEditing.Visible ? "Update the current configuration" : "Export configuration to Road Builder";
		}

		private void DrawThumbnail(Graphics graphics, List<ThumbnailLaneInfo> lanes, bool small, bool tooltip)
		{
			new ThumbnailHandler(graphics, GetRoadInfo(), small, tooltip).Draw();
		}

		private void FLP_Tags_ControlAdded(object sender, ControlEventArgs e)
		{
			L_NoTags.Visible = FLP_Tags.Controls.Count == 1;

			RefreshPreview();
		}

		private void Frm_TagAdded(object sender, string e)
		{
			if (string.IsNullOrWhiteSpace(e))
			{
				return;
			}

			FLP_Tags.Controls.Add(new TagControl(e.Trim(), false));

			RefreshPreview();
		}

		private void Frm_TagRemoved(object sender, string e)
		{
			FLP_Tags.Controls.Clear(true, x => x is TagControl tc && tc.Text.Equals(e, StringComparison.CurrentCultureIgnoreCase));

			RefreshPreview();
		}

		private List<ThumbnailLaneInfo> GetLanes()
		{
			return P_Lanes.Controls.OfType<RoadLane>().Reverse().Select(x => x.Lane).ToList();
		}

		private RegionType GetRegion()
		{
			return RegionTypeControl.SelectedItem;
		}

		private RoadInfo GetRoadInfo(List<ThumbnailLaneInfo> lanes = null)
		{
			return new RoadInfo
			{
				CustomName = TB_RoadName.Text,
				CustomDescription = TB_RoadDesc.Text,
				CustomText = TB_CustomText.Text,
				BufferWidth = TB_BufferSize.Text.SmartParseF(),
				RoadWidth = TB_Size.Text.SmartParseF(),
				RegionType = GetRegion(),
				RoadType = GetRoadType(),
				SideTexture = SideTextureControl.SelectedItem,
				BridgeSideTexture = BridgeSideTextureControl.SelectedItem,
				AsphaltStyle = AsphaltTextureControl.SelectedItem,
				SpeedLimit = TB_SpeedLimit.Text.SmartParse(),
				Lanes = (lanes ?? GetLanes()).Select(x => x.AsLaneInfo()).ToList(),
				LHT = Options.Current.LHT,
				VanillaWidth = Options.Current.VanillaWidths,
				Tags = FLP_Tags.Controls.OfType<TagControl>().Select(x => x.Text).ToList(),
				DateCreated = C_CurrentlyEditing.Road?.DateCreated ?? DateTime.Now,
				DisabledElevations = ElevationTypeControl.SelectedItems.ToArray(),
				CanCrossLanes = CB_CanCrossLanes.Checked,
				HighwayRules = CB_HighwayRules.Checked,
			};
		}

		private RoadType GetRoadType()
		{
			return RoadTypeControl.SelectedItem;
		}

		private Image GetTextureIcon(TextureType texture)
		{
			switch (texture)
			{
				case TextureType.Pavement:
					return Properties.Resources.L_D_2;
				case TextureType.Gravel:
					return Properties.Resources.L_D_3;
				case TextureType.Ruined:
					return Properties.Resources.I_Ruined;
				case TextureType.Asphalt:
					return Properties.Resources.I_Asphalt;
			}

			return null;
		}

		private Image GetTextureIcon(BridgeTextureType texture)
		{
			switch (texture)
			{
				case BridgeTextureType.Pavement:
					return Properties.Resources.L_D_2;
				case BridgeTextureType.Asphalt:
					return Properties.Resources.I_Asphalt;
			}

			return null;
		}

		private Image GetTextureIcon(AsphaltStyle style)
		{
			switch (style)
			{
				case AsphaltStyle.None:
					return Properties.Resources.L_C_0;
				case AsphaltStyle.Asphalt:
					return Properties.Resources.I_Asphalt;
			}

			return null;
		}

		private void L_RoadName_Click(object sender, EventArgs e)
		{
			TB_RoadName.Show();
			L_RoadName.Hide();
			B_EditName.Hide();
			TB_RoadName.Focus();
			TB_RoadName.MaxLength = 32;
		}

		private void L_RoadName_MouseEnter(object sender, EventArgs e)
		{
			(sender as Label).ForeColor = FormDesign.Design.ActiveColor;
		}

		private void L_RoadName_MouseLeave(object sender, EventArgs e)
		{
			L_RoadName.ForeColor = FormDesign.Design.ForeColor;
			L_RoadDesc.ForeColor = FormDesign.Design.InfoColor;
		}

		private void P_Lanes_ControlAdded(object sender, ControlEventArgs e)
		{
			if (e.Control is RoadLane roadLane)
			{
				roadLane.RoadLaneChanged += TB_Name_TextChanged;
			}
		}

		private void PB_Click(object sender, EventArgs e)
		{
			try
			{
				var lanes = GetLanes();

				var toolTip = false;
				var small = false;
				var width = toolTip ? 492 : small ? 109 : 512;
				var height = toolTip ? 147 : small ? 100 : 512;

				using (var img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
				using (var g = Graphics.FromImage(img))
				{
					DrawThumbnail(g, lanes, small, toolTip);

					Clipboard.SetDataObject(small || toolTip ? new Bitmap(img) : new Bitmap(img, 256, 256));
				}

				Notification.Create("Thumbnail copied to clipboard", "", PromptIcons.None, () => { }, NotificationSound.None, new Size(240, 32))
					.Show(Form, 5);
			}
			catch (Exception ex) { ShowPrompt(ex.Message, "Error", PromptButtons.OK, PromptIcons.Error); }
		}

		private void RCC_LoadConfiguration(object sender, RoadInfo r)
		{
			try
			{
				refreshPaused = true;
				P_Lanes.SuspendDrawing();
				P_Lanes.Controls.Clear(true);

				TB_Size.Text = r.RoadWidth == 0 ? string.Empty : r.RoadWidth.ToString();
				TB_BufferSize.Text = r.BufferWidth.ToString();
				TB_SpeedLimit.Text = r.SpeedLimit == 0 ? string.Empty : r.SpeedLimit.ToString();
				TB_CustomText.Text = r.CustomText;
				TB_RoadName.Text = r.CustomName;
				TB_RoadDesc.Text = r.CustomDescription;

				foreach (var lane in Options.Current.LHT ? (r.Lanes as IEnumerable<LaneInfo>).Reverse() : r.Lanes)
				{
					AddLaneControl(new ThumbnailLaneInfo(lane)
					{
						Direction = (Options.Current.LHT && lane.Type == LaneType.Curb) ? lane.Direction == LaneDirection.Forward ? LaneDirection.Backwards : LaneDirection.Forward
							: lane.Direction
					});
				}

				RoadTypeControl.SelectedItem = r.RoadType;
				RegionTypeControl.SelectedItem = r.RegionType;
				SideTextureControl.SelectedItem = r.SideTexture;
				BridgeSideTextureControl.SelectedItem = r.BridgeSideTexture;
				AsphaltTextureControl.SelectedItem = r.AsphaltStyle;

				ElevationTypeControl.ResetValue();
				foreach (var item in r.DisabledElevations??new RoadElevation[0])
				{
					ElevationTypeControl.Select(item);
				}

				CB_CanCrossLanes.Checked = r.CanCrossLanes;
				CB_HighwayRules.Checked = r.HighwayRules;

				FLP_Tags.Controls.Clear(true, x => x is TagControl);

				if (r.Tags?.Any() ?? false)
				{
					FLP_Tags.Controls.AddRange(r.Tags.Select(x => new TagControl(x, false)).ToArray());
				}

				P_Lanes.Controls.OfType<RoadLane>().FirstOrDefault(x => x.Lane.Type == LaneType.Curb)?.FixCurbOrientation();

				C_CurrentlyEditing.SetRoad(sender as RoadConfigControl);
			}
			catch (Exception ex)
			{
				ShowPrompt(ex.Message, "Failed to load this configuration", PromptButtons.OK, PromptIcons.Error);
			}
			finally
			{
				P_Lanes.ResumeDrawing();
				refreshPaused = false;
				RefreshPreview();
			}
		}

		private void RefreshPreview()
		{
			if (refreshPaused)
			{
				return;
			}

			try
			{
				var lanes = GetLanes();

				var toolTip = false;
				var small = false;
				var width = toolTip ? 492 : small ? 109 : 512;
				var height = toolTip ? 147 : small ? 100 : 512;

				var roadInfo = GetRoadInfo(lanes);
				using (var img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
				using (var g = Graphics.FromImage(img))
				{
					DrawThumbnail(g, lanes, small || toolTip, toolTip);

					var actualImg = new Bitmap(PB.Width, PB.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
					using (var g2 = Graphics.FromImage(actualImg))
					{
						g2.SmoothingMode = SmoothingMode.HighQuality;
						g2.DrawRoundedImage(img, new Rectangle(1, 1, PB.Width - 2, PB.Height - 2), TLP_Right.Padding.Left+1);
						g2.DrawRoundedRectangle(new Pen(FormDesign.Design.AccentBackColor, 1.5F), new Rectangle(1, 1, PB.Width - 2, PB.Height - 2), TLP_Right.Padding.Left);
					}

					PB.Image = actualImg;
					PB.SizeMode = small && !toolTip ? PictureBoxSizeMode.Normal : PictureBoxSizeMode.Normal;
				}

				roadInfo.Name = roadInfo.CustomName.IfEmpty(Utilities.GetRoadName(roadInfo));
				roadInfo.Description = roadInfo.CustomDescription.IfEmpty(Utilities.GetRoadDescription(roadInfo, false));

				L_RoadName.Text = roadInfo.Name.Substring(0, Math.Min(32, roadInfo.Name.Length)).Replace(" ", " ").Replace("&", "&&") + (roadInfo.Name.Length > 32 ? ".." : "");
				L_RoadDesc.Text = roadInfo.Description.Substring(0, Math.Min(1024, roadInfo.Description.Length)).Replace("&", "&&") + (roadInfo.Description.Length > 1024 ? ".." : "");

				C_Warnings.SetRoad(roadInfo);

				var speed = string.IsNullOrWhiteSpace(TB_SpeedLimit.Text) ? Utilities.DefaultSpeedSign(lanes, GetRoadType(), RegionTypeControl.SelectedItem == RegionType.USA) : TB_SpeedLimit.Text.SmartParse();

				if (speed != RoadLane.GlobalSpeed || RoadLane.RoadType != GetRoadType())
				{
					RoadLane.GlobalSpeed = speed;
					RoadLane.RoadType = GetRoadType();

					foreach (Control item in P_Lanes.Controls)
					{
						item.Invalidate();
					}
				}
			}
			catch (Exception ex) { ShowPrompt(ex.Message, "Error", PromptButtons.OK, PromptIcons.Error); }
		}

		private void SetupType(RoadType road)
		{
			if (refreshPaused)
			{
				return;
			}

			var leftSidewalk = P_Lanes.Controls.OfType<RoadLane>().FirstOrDefault(x => x.Lane.Type == LaneType.Curb && x.Lane.Direction == LaneDirection.Backwards);
			var rightSidewalk = P_Lanes.Controls.OfType<RoadLane>().LastOrDefault(x => x.Lane.Type == LaneType.Curb && x.Lane.Direction == LaneDirection.Forward);

			if (leftSidewalk == null)
			{
				AddLaneControl(new ThumbnailLaneInfo { Type = LaneType.Curb, Direction = LaneDirection.Backwards }).SendToBack();
			}

			if (rightSidewalk == null)
			{
				AddLaneControl(new ThumbnailLaneInfo { Type = LaneType.Curb, Direction = LaneDirection.Forward });
			}

			var first = P_Lanes.Controls[0] as RoadLane;
			var last = P_Lanes.Controls[P_Lanes.Controls.Count - 1] as RoadLane;

			if (road == RoadType.Highway)
			{
				if (first.Lane.Type == LaneType.Pedestrian && first.Lane.Decorations == LaneDecoration.None)
				{
					first.Dispose();
				}

				if (last.Lane.Type == LaneType.Pedestrian && last.Lane.Decorations == LaneDecoration.None)
				{
					last.Dispose();
				}
			}
			else
			{
				if (first.Lane.Type == LaneType.Curb)
				{
					AddLaneControl(new ThumbnailLaneInfo { Type = LaneType.Pedestrian }).SendToBack();
				}

				if (last.Lane.Type == LaneType.Curb)
				{
					AddLaneControl(new ThumbnailLaneInfo { Type = LaneType.Pedestrian });
				}
			}

			if (road == RoadType.Highway)
			{
				SideTextureControl.SelectedItem = TextureType.Gravel;
				BridgeSideTextureControl.SelectedItem = BridgeTextureType.Asphalt;
				CB_HighwayRules.Checked = true;
			}
			else
			{
				SideTextureControl.SelectedItem = TextureType.Pavement;
				BridgeSideTextureControl.SelectedItem = BridgeTextureType.Pavement;
				CB_HighwayRules.Checked = false;
			}

			RefreshPreview();
		}

		private void TB_Name_TextChanged(object sender, EventArgs e)
		{
			if (sender == TB_BufferSize)
			{ Options.Current.BufferSize = TB_BufferSize.Text; Options.Save(); }

			RefreshPreview();
		}

		private void TB_RoadDesc_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
			{
				e.SuppressKeyPress = true;
				e.Handled = true;
			}
		}

		private void TB_RoadDesc_Leave(object sender, EventArgs e)
		{
			RefreshPreview();
			TB_RoadDesc.Hide();
			L_RoadDesc.Show();
			B_EditDesc.Show();
		}

		private void TB_RoadDesc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
			{
				e.IsInputKey = true;

				TB_RoadDesc_Leave(sender, e);
			}
		}

		private void TB_RoadName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
			{
				e.SuppressKeyPress = true;
				e.Handled = true;
			}
		}

		private void TB_RoadName_Leave(object sender, EventArgs e)
		{
			RefreshPreview();
			TB_RoadName.Hide();
			L_RoadName.Show();
			B_EditName.Show();
		}

		private void TB_RoadName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
			{
				e.IsInputKey = true;

				TB_RoadName_Leave(sender, e);
			}
		}

		private void TB_SpeedLimit_IconClicked(object sender, EventArgs e)
		{
			var rl = new RoadLane
			{
				Visible = false,
				Parent = this
			};

			new LaneSpeedSelector(rl)
			{
				Location = TB_SpeedLimit.PointToScreen(new Point(0, TB_SpeedLimit.Height + 2))
			}
			.FormClosed += (s, _) =>
			{
				if (rl.Lane.SpeedLimit != null)
				{
					TB_SpeedLimit.Text = rl.Lane.SpeedLimit?.ToString();
				}

				rl.Dispose();
			};
		}

		private void TLP_Buttons_Resize(object sender, EventArgs e)
		{
			foreach (var item in TLP_Buttons.Controls.OfType<SlickButton>())
			{
				item.Text = TLP_Buttons.Width < (650 * UI.FontScale) ? string.Empty : item.Tag.ToString();
			}
		}

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			C_CurrentlyEditing.ForeColor = design.ActiveColor;
			L_NoTags.ForeColor = design.LabelColor;
			L_RoadName.ForeColor = design.ForeColor;
			L_RoadDesc.ForeColor = design.InfoColor;

			RefreshPreview();
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			TB_BufferSize.Text = Options.Current.BufferSize;
			refreshPaused = false;
			SetupType(GetRoadType());
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			if (Visible)
			{
				RefreshPreview();
			}
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			L_RoadName.Font = UI.Font(9.75F, FontStyle.Bold);
			L_RoadDesc.Font = L_NoTags.Font = UI.Font(7.5F);

			TLP_TopControls.Padding= TLP_TopControls.Margin=
			TLP_Right.Padding = RCC.Parent.Padding = TLP_Right.Margin = RCC.Parent.Margin = UI.Scale(new Padding(5), UI.FontScale);

			TLP_Right.Width = (int)(250 * UI.FontScale);
		}

		private void PB_SizeChanged(object sender, EventArgs e)
		{
			PB.Height = PB.Width;
		}
	}
}