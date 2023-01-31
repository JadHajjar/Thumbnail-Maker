using Extensions;

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

			RoadTypeControl = new OptionSelectionControl<RoadType>(t => ResourceManager.GetRoadType(t, false, false)) { Dock = DockStyle.Top };
			GB_RoadType.Controls.Add(RoadTypeControl);
			RegionTypeControl = new OptionSelectionControl<RegionType>((g, r, t) => ThumbnailHandler.DrawSpeedSignSmall(g, t, 20, r)) { Dock = DockStyle.Top };
			GB_Region.Controls.Add(RegionTypeControl);
			SideTextureControl = new OptionSelectionControl<TextureType>(GetTextureIcon) { Dock = DockStyle.Top };
			GB_SideTexture.Controls.Add(SideTextureControl);
			BridgeSideTextureControl = new OptionSelectionControl<BridgeTextureType>(GetTextureIcon) { Dock = DockStyle.Top };
			GB_BridgeSideTexture.Controls.Add(BridgeSideTextureControl);
			AsphaltTextureControl = new OptionSelectionControl<AsphaltStyle>(GetTextureIcon) { Dock = DockStyle.Top };
			GB_AsphaltTexture.Controls.Add(AsphaltTextureControl);

			RoadTypeControl.SelectedValueChanged += (s, e) => SetupType(RoadTypeControl.SelectedValue);
			SideTextureControl.SelectedValueChanged += (s, e) => RefreshPreview();
			AsphaltTextureControl.SelectedValueChanged += (s, e) => RefreshPreview();
			RegionTypeControl.SelectedValueChanged += (s, e) =>
			{
				TB_SpeedLimit.LabelText = $"Speed Limit ({(RegionTypeControl.SelectedValue == RegionType.USA ? "mph" : "km/h")})";

				Options.Current.Region = GetRegion();
				Options.Save();

				RefreshPreview();
			};

			SetupType(RoadTypeControl.SelectedValue);

			using (var img = new Bitmap(65 * 2, 24 * 2))
			using (var g = Graphics.FromImage(img))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				using (var path = new Rectangle(64, 24, 64, 23).RoundedRect(5, false))
				{
					g.FillPath(new SolidBrush(Color.FromArgb(35, 35, 40)), path);
				}

				g.DrawImage(Properties.Resources.I_Copy.Color(Color.White), new Rectangle(7 + 64, 4 + 24, 16, 16));
				g.DrawString("COPY", new Font(UI.FontFamily, 8.25F), Brushes.White, new Rectangle(26 + 64, 0 + 24, 35, 24), new StringFormat
				{
					LineAlignment = StringAlignment.Center,
					Alignment = StringAlignment.Center,
				});
				g.DrawLine(Pens.White, 66, 26, 69, 26);
				g.DrawLine(Pens.White, 66, 26, 66, 29);
				g.DrawLine(Pens.White, 66, 26, 68F, 28);

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
			SlickTip.SetTo(B_AddLane, "Add a new empty lane");
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

		protected override void DesignChanged(FormDesign design)
		{
			base.DesignChanged(design);

			C_CurrentlyEditing.ForeColor = design.ActiveColor;
			L_RoadDesc.ForeColor = design.InfoColor;
			L_NoTags.ForeColor = design.LabelColor;

			RefreshPreview();
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			L_RoadName.Font = UI.Font(9.75F, FontStyle.Bold);
			L_RoadDesc.Font = L_NoTags.Font = UI.Font(7.5F);

			TLP_Main.ColumnStyles[TLP_Main.ColumnStyles.Count - 2].Width = (float)(276 * UI.UIScale);
			PB.Size = UI.Scale(new Size(256, 256), UI.UIScale);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			TB_BufferSize.Text = 0.25F.ToString();
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

		private List<ThumbnailLaneInfo> GetLanes()
		{
			return P_Lanes.Controls.OfType<RoadLane>().Reverse().Select(x => x.Lane).ToList();
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
				var img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				using (var g = Graphics.FromImage(img))
				{
					DrawThumbnail(g, lanes, small || toolTip, toolTip);

					PB.Image = img;
					PB.SizeMode = small && !toolTip ? PictureBoxSizeMode.Normal : PictureBoxSizeMode.Zoom;
				}

				roadInfo.Name = roadInfo.CustomName.IfEmpty(Utilities.GetRoadName(roadInfo));
				roadInfo.Description = roadInfo.CustomDescription.IfEmpty(Utilities.GetRoadDescription(roadInfo, false));

				L_RoadName.Text = roadInfo.Name.Substring(0, Math.Min(32, roadInfo.Name.Length)).Replace(" ", " ").Replace("&", "&&") + (roadInfo.Name.Length > 32 ? ".." : "");
				L_RoadDesc.Text = roadInfo.Description.Substring(0, Math.Min(1024, roadInfo.Description.Length)).Replace("&", "&&") + (roadInfo.Description.Length > 1024 ? ".." : "");

				C_Warnings.SetRoad(roadInfo);

				var speed = string.IsNullOrWhiteSpace(TB_SpeedLimit.Text) ? Utilities.DefaultSpeedSign(lanes, GetRoadType(), RegionTypeControl.SelectedValue == RegionType.USA) : TB_SpeedLimit.Text.SmartParse();

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

		private void DrawThumbnail(Graphics graphics, List<ThumbnailLaneInfo> lanes, bool small, bool tooltip)
		{
			new ThumbnailHandler(graphics, small, tooltip)
			{
				RoadWidth = Utilities.VanillaWidth(Options.Current.VanillaWidths, Math.Max(TB_Size.Text.SmartParseF(), Utilities.CalculateRoadSize(lanes, TB_BufferSize.Text.SmartParseF()))),
				CustomText = TB_CustomText.Text,
				BufferSize = Math.Max(0, TB_BufferSize.Text.SmartParseF()),
				RegionType = GetRegion(),
				RoadType = GetRoadType(),
				LHT = Options.Current.LHT,
				SideTexture = SideTextureControl.SelectedValue,
				AsphaltStyle = AsphaltTextureControl.SelectedValue,
				Speed = string.IsNullOrWhiteSpace(TB_SpeedLimit.Text) ? Utilities.DefaultSpeedSign(lanes, GetRoadType(), RegionTypeControl.SelectedValue == RegionType.USA) : TB_SpeedLimit.Text.SmartParse(),
				Lanes = new List<ThumbnailLaneInfo>(lanes)
			}.Draw();
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
				SideTextureControl.SelectedValue = TextureType.Gravel;
				BridgeSideTextureControl.SelectedValue = BridgeTextureType.Asphalt;
			}
			else
			{
				SideTextureControl.SelectedValue = TextureType.Pavement;
				BridgeSideTextureControl.SelectedValue = BridgeTextureType.Pavement;
			}

			RefreshPreview();
		}

		private void TB_Name_TextChanged(object sender, EventArgs e)
		{
			RefreshPreview();
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

			AsphaltTextureControl.SelectedValue = AsphaltStyle.Asphalt;
			refreshPaused = false;
			SetupType(RoadTypeControl.SelectedValue);
		}

		private void B_Options_Click(object sender, EventArgs e)
		{
			Form.PushPanel<PC_Options>(null);
		}

		private void B_Add_Click(object sender, EventArgs e)
		{
			var ctrl = new RoadLane();

			P_Lanes.Controls.Add(ctrl);

			var rightSidewalk = P_Lanes.Controls.OfType<RoadLane>().LastOrDefault(x => x.Lane.Type == LaneType.Curb && x.Lane.Direction == LaneDirection.Forward);

			if (rightSidewalk != null)
			{
				P_Lanes.Controls.SetChildIndex(ctrl, P_Lanes.Controls.GetChildIndex(rightSidewalk) + 1);
			}
			else
			{
				ctrl.BringToFront();
			}

			var frm = new RoadTypeSelector(ctrl, B_AddLane);

			frm.FormClosed += (s, _) => RefreshPreview();
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

			refreshPaused = false;
			RefreshPreview();
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
					if (File.Exists(Path.Combine(folder, item.Item1)))
					{
						save(item.Item1, item.Item2, item.Item3);

						matched = true;
					}
				}

				if (!matched)
				{
					save(GetLanes().ListStrings(" + ") + ".png", frm.CB_Small.Checked, frm.CB_Tooltip.Checked);
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

						var FileName = Path.Combine(folder, filename);

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

		private void B_CopyDesc_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(TB_RoadDesc.Text))
			{
				Clipboard.SetText(TB_RoadDesc.Text);
				return;
			}

			var roadInfo = new RoadInfo
			{
				CustomName = TB_RoadName.Text,
				CustomText = TB_CustomText.Text,
				BufferWidth = TB_BufferSize.Text.SmartParseF(),
				RoadWidth = TB_Size.Text.SmartParseF(),
				RegionType = GetRegion(),
				RoadType = GetRoadType(),
				SideTexture = SideTextureControl.SelectedValue,
				BridgeSideTexture = BridgeSideTextureControl.SelectedValue,
				AsphaltStyle = AsphaltTextureControl.SelectedValue,
				SpeedLimit = (int)(TB_SpeedLimit.Text.SmartParse() * (RegionTypeControl.SelectedValue == RegionType.USA ? 1.609F : 1F)),
				Lanes = GetLanes().Select(x => x.AsLaneInfo()).ToList(),
				LHT = Options.Current.LHT,
				VanillaWidth = Options.Current.VanillaWidths,
			};

			var desc = Utilities.GetRoadDescription(roadInfo);

			Clipboard.SetText(desc);
		}

		private void B_CopyRoadName_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(L_RoadName.Text);
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

				var file = Utilities.ExportRoad(roadInfo, C_CurrentlyEditing.Road == null ? null : Path.GetFileName(C_CurrentlyEditing.Control.FileName));

				C_CurrentlyEditing.Control?.Dispose();

				RCC.RefreshConfigs();

				C_CurrentlyEditing.SetRoad(RCC.P_Configs.Controls.OfType<RoadConfigControl>().FirstOrDefault(x => x.FileName.Equals(file, StringComparison.InvariantCultureIgnoreCase)));
			}
			catch (Exception ex) { ShowPrompt(ex.Message, "Error", PromptButtons.OK, PromptIcons.Error); }
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
				SideTexture = SideTextureControl.SelectedValue,
				BridgeSideTexture = BridgeSideTextureControl.SelectedValue,
				AsphaltStyle = AsphaltTextureControl.SelectedValue,
				SpeedLimit = TB_SpeedLimit.Text.SmartParse(),
				Lanes = (lanes ?? GetLanes()).Select(x => x.AsLaneInfo()).ToList(),
				LHT = Options.Current.LHT,
				VanillaWidth = Options.Current.VanillaWidths,
				Tags = FLP_Tags.Controls.OfType<TagControl>().Select(x => x.Text).ToList(),
				DateCreated = C_CurrentlyEditing.Road?.DateCreated ?? DateTime.Now,
			};
		}

		private RoadType GetRoadType()
		{
			return RoadTypeControl.SelectedValue;
		}

		private RegionType GetRegion()
		{
			return RegionTypeControl.SelectedValue;
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

				RoadTypeControl.SelectedValue = r.RoadType;
				RegionTypeControl.SelectedValue = r.RegionType;
				SideTextureControl.SelectedValue = r.SideTexture;
				BridgeSideTextureControl.SelectedValue = r.BridgeSideTexture;
				AsphaltTextureControl.SelectedValue = r.AsphaltStyle;

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

		private RoadLane AddLaneControl(ThumbnailLaneInfo item)
		{
			var ctrl = new RoadLane(item);

			P_Lanes.Controls.Add(ctrl);

			ctrl.BringToFront();

			return ctrl;
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

		private void L_RoadName_MouseEnter(object sender, EventArgs e)
		{
			(sender as Label).ForeColor = FormDesign.Design.ActiveColor;
		}

		private void L_RoadName_MouseLeave(object sender, EventArgs e)
		{
			L_RoadName.ForeColor = FormDesign.Design.ForeColor;
			L_RoadDesc.ForeColor = FormDesign.Design.InfoColor;
		}

		private void L_RoadName_Click(object sender, EventArgs e)
		{
			TB_RoadName.Show();
			L_RoadName.Hide();
			B_EditName.Hide();
			TB_RoadName.Focus();
			TB_RoadName.MaxLength = 32;
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

		private void TB_RoadName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
			{
				e.SuppressKeyPress = true;
				e.Handled = true;
			}
		}

		private void P_Lanes_ControlAdded(object sender, ControlEventArgs e)
		{
			if (e.Control is RoadLane roadLane)
			{
				roadLane.RoadLaneChanged += TB_Name_TextChanged;
			}
		}

		private void B_ViewSavedRoads_Click(object sender, EventArgs e)
		{
			B_ViewSavedRoads.Text = RCC.Width.If(0, "Hide Roads", "Load Road");
			B_ViewSavedRoads.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
			RCC.Width = RCC.Width.If(0, 15 + Options.Current.RoadConfigColumns * (12 + (int)(100 * UI.UIScale)), 0);
			RCC.Visible = RCC.Width != 0;

			Form.OnNextIdle(P_Lanes.PerformLayout);
		}

		private void B_AddTag_Click(object sender, EventArgs e)
		{
			var frm = new AddTagForm(RCC.LoadedTags, FLP_Tags.GetControls<TagControl>().Select(x => x.Text)) { Location = FLP_Tags.PointToScreen(Point.Empty), MinimumSize = new Size(TLP_Right.Width - 5, 0) };

			frm.TagAdded += Frm_TagAdded;
			frm.TagRemoved += Frm_TagRemoved;

			frm.Show();
		}

		private void Frm_TagRemoved(object sender, string e)
		{
			FLP_Tags.Controls.Clear(true, x => x is TagControl tc && tc.Text.Equals(e, StringComparison.CurrentCultureIgnoreCase));
		}

		private void Frm_TagAdded(object sender, string e)
		{
			FLP_Tags.Controls.Add(new TagControl(e, false));
		}

		private void FLP_Tags_ControlAdded(object sender, ControlEventArgs e)
		{
			L_NoTags.Visible = FLP_Tags.Controls.Count == 1;
		}

		private void TLP_Buttons_Resize(object sender, EventArgs e)
		{
			foreach (var item in TLP_Buttons.Controls.OfType<SlickButton>())
			{
				item.Text = TLP_Buttons.Width < (650 * UI.FontScale) ? string.Empty : item.Tag.ToString();
			}
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

		private void TB_RoadDesc_Leave(object sender, EventArgs e)
		{
			RefreshPreview();
			TB_RoadDesc.Hide();
			L_RoadDesc.Show();
			B_EditDesc.Show();
		}

		private void TB_RoadDesc_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
			{
				e.SuppressKeyPress = true;
				e.Handled = true;
			}
		}

		private void TB_RoadDesc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
			{
				e.IsInputKey = true;

				TB_RoadDesc_Leave(sender, e);
			}
		}
	}
}