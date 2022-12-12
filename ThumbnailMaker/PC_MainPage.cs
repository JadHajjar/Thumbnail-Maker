using Extensions;

using Newtonsoft.Json;

using SlickControls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using ThumbnailMaker.Controls;
using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker
{
	public partial class PC_MainPage : PanelContent
	{
		public PC_MainPage()
		{
			InitializeComponent();

			using (var img = new Bitmap(24, 24))
			using (var g = Graphics.FromImage(img))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.FillRoundedRectangle(Brushes.Black, new Rectangle(0, 0, 23, 23), 6);
				g.DrawImage(B_CopyName.Image.Color(Color.White), new Rectangle(4, 4, 16, 16));

				PB.Cursor = new Cursor(img.GetHicon());
			}

			TB_BufferSize.Text = 0.25F.ToString();

			RB_Europe.Checked = Options.Current.Region == RegionType.Europe;
			RB_Canada.Checked = Options.Current.Region == RegionType.Canada;
			RB_USA.Checked = Options.Current.Region == RegionType.USA;
			RB_Road.Checked = true;

			SlickTip.SetTo(RB_Road, "A normal road with curbs & sidewalks");
			SlickTip.SetTo(RB_Highway, "A flat road with no pavement and highway rules");
			SlickTip.SetTo(RB_Pedestrian, "A flat pedestrian road");

			SlickTip.SetTo(RB_Europe, "Use a European styled speed limit sign, as well as km/h as the speed measurement");
			SlickTip.SetTo(RB_USA, "Use a US styled speed limit sign, as well as mph as the speed measurement");
			SlickTip.SetTo(RB_Canada, "Use a Canadian styled speed limit sign, as well as km/h as the speed measurement");

			SlickTip.SetTo(TB_Size, "Manually specify the total asphalt width of the road");
			SlickTip.SetTo(TB_BufferSize, "Determines how far should the lanes be from the sidewalk");
			SlickTip.SetTo(TB_SpeedLimit, "Manually specify the default speed limit of the road");
			SlickTip.SetTo(TB_CustomText, "Add custom text to the thumbnail");

			SlickTip.SetTo(B_CopyName, "Copy the generated road name into your clipboard");
			SlickTip.SetTo(B_CopyDesc, "Copy the generated road description into your clipboard");
			SlickTip.SetTo(B_SaveThumb, "Saves the thumbnail on your desktop, or to the folder you have copied in your clipboard");
			SlickTip.SetTo(B_Export, "Exports the road configuration to the Road Builder folder to be generated");

			SlickTip.SetTo(B_Options, "Change the colors & icons of lane types as well as other options");
			SlickTip.SetTo(B_DuplicateFlip, "Duplicates the current lanes to the right and flips their direction");
			SlickTip.SetTo(B_FlipLanes, "Flips the whole road to create its opposite variation");
			SlickTip.SetTo(B_AddLane, "Add a new empty lane");

			FormDesign.DesignChanged += FormDesign_DesignChanged;
		}

		private void FormDesign_DesignChanged(FormDesign design)
		{
			RefreshPreview();
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			L_RoadName.Font = UI.Font(9.75F, FontStyle.Bold);

			TLP_Main.ColumnStyles[TLP_Main.ColumnStyles.Count - 1].Width = (float)(276 * UI.UIScale);
			PB.Size = UI.Scale(new Size(256, 256), UI.UIScale);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

			RefreshPreview();
		}

		private List<LaneInfo> GetLanes(bool small) => P_Lanes.Controls.OfType<RoadLane>().Reverse().Select(x => x.GetLane(small)).ToList();

		private void RefreshPreview()
		{
			try
			{
				var lanes = GetLanes(false);

				var img = new Bitmap(512, 512, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				using (var g = Graphics.FromImage(img))
				{
					DrawThumbnail(g, lanes, false);

					PB.Image = img;
				}

				L_RoadName.Text = string.IsNullOrWhiteSpace(TB_RoadName.Text) ? "BRB " + Utilities.IsOneWay(lanes).Switch(true, "1W ", false, string.Empty, string.Empty) + lanes.Select(x => x.GetTitle(lanes)).WhereNotEmpty().ListStrings("+") : TB_RoadName.Text;
				L_RoadName.ForeColor = L_RoadName.Text.Length > 32 ? FormDesign.Design.RedColor : FormDesign.Design.ForeColor;
			}
			catch (Exception ex) { ShowPrompt(ex.Message, "Error", PromptButtons.OK, PromptIcons.Error); }
		}

		private void DrawThumbnail(Graphics graphics, List<LaneInfo> lanes, bool small)
		{
			new ThumbnailHandler(graphics, small)
			{
				RoadSize = TB_Size.Text.IfEmpty(Utilities.CalculateRoadSize(lanes, TB_BufferSize.Text)),
				CustomText = TB_CustomText.Text,
				BufferSize = TB_BufferSize.Text.SmartParseF(),
				PavementWidth = TB_PavementWidth.Text.SmartParseF(),
				AsphaltWidth = TB_Size.Text.SmartParseF(),
				RegionType = GetRegion(),
				RoadType = GetRoadType(),
				Speed = TB_SpeedLimit.Text.IfEmpty(Utilities.DefaultSpeedSign(lanes, RB_USA.Checked)),
				Lanes = new List<LaneInfo>(lanes)
			}.Draw();
		}

		private void RB_CheckedChanged(object sender, EventArgs e)
		{
			if (RB_Road == sender)
				SetupType(RoadType.Road);
			else if (RB_Highway == sender)
				SetupType(RoadType.Highway);
			else if (RB_FlatRoad == sender)
				SetupType(RoadType.Flat);
			else if (RB_Pedestrian == sender)
				SetupType(RoadType.Pedestrian);

			foreach (var item in P_Lanes.Controls.OfType<RoadLane>())
				item.Invalidate();

			RefreshPreview();

			TB_SpeedLimit.LabelText = $"Speed Limit ({(RB_USA.Checked ? "mph" : "km/h")})";

			Options.Current.Region = GetRegion();
			Options.Save();
		}

		private void SetupType(RoadType road)
		{
			if (road == RoadType.Road)
			{
				AddLaneControl(new LaneInfo { Type = LaneType.Pedestrian });
				AddLaneControl(new LaneInfo { Type = LaneType.Sidewalk, Direction = LaneDirection.Backwards });
				AddLaneControl(new LaneInfo { Type = LaneType.Sidewalk, Direction = LaneDirection.Forward });
				AddLaneControl(new LaneInfo { Type = LaneType.Pedestrian });
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			RefreshPreview();
		}

		private void TB_Name_TextChanged(object sender, EventArgs e)
		{
			RefreshPreview();
		}

		private void B_Clear_Click(object sender, EventArgs e)
		{
			TB_RoadName.Text = string.Empty;
			P_Lanes.Controls.Clear(true);

			RefreshPreview();
		}

		private void B_Options_Click(object sender, EventArgs e)
		{
			Form.PushPanel<PC_Options>(null);
		}

		private void B_Add_Click(object sender, EventArgs e)
		{
			var ctrl = new RoadLane(() => RB_Highway.Checked);

			ctrl.RoadLaneChanged += TB_Name_TextChanged;

			P_Lanes.Controls.Add(ctrl);

			ctrl.BringToFront();

			var frm = new RoadTypeSelector(ctrl);

			frm.FormClosed += (s, _) => RefreshPreview();
		}

		private void B_FlipLanes_Click(object sender, EventArgs e)
		{
			foreach (var item in P_Lanes.Controls.OfType<RoadLane>().ToList())
			{
				item.Invalidate();

				item.BringToFront();
			}

			RefreshPreview();
		}

		private void B_DuplicateFlip_Click(object sender, EventArgs e)
		{
			foreach (var item in P_Lanes.Controls.OfType<RoadLane>().ToList())
			{
				var ctrl = item.Duplicate();

				if (ctrl.LaneDirection == LaneDirection.Forward)
					ctrl.LaneDirection = LaneDirection.Backwards;
				else if (ctrl.LaneDirection == LaneDirection.Backwards)
					ctrl.LaneDirection = LaneDirection.Forward;

				ctrl.Dock = DockStyle.Top;

				ctrl.RoadLaneChanged += TB_Name_TextChanged;

				P_Lanes.Controls.Add(ctrl);

				ctrl.BringToFront();
			}

			RefreshPreview();
		}

		private void B_Save_Click(object sender, EventArgs e)
		{
			try
			{
				var folder = Clipboard.ContainsText() && Directory.Exists(Clipboard.GetText()) ? Clipboard.GetText() : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				var matched = false;
				var files = new List<(string, bool)>
				{
					("asset_thumb.png", true),
					("PreviewImage.png", false),
					("snapshot.png", false),
					("thumbnail.png", false),
				};

				if (File.Exists(Path.Combine(folder, "tooltip.png")))
					File.Copy($"{Utilities.Folder}\\Resources\\tooltip.png", Path.Combine(folder, "tooltip.png"), true);

				if (File.Exists(Path.Combine(folder, "asset_tooltip.png")))
					File.Copy($"{Utilities.Folder}\\Resources\\tooltip.png", Path.Combine(folder, "asset_tooltip.png"), true);

				foreach (var item in files)
				{
					if (File.Exists(Path.Combine(folder, item.Item1)))
					{
						save(item.Item1, item.Item2);

						matched = true;
					}
				}

				if (!matched)
					save(GetLanes(false).ListStrings(" + ") + ".png", false);

				void save(string filename, bool small)
				{
					var width = small ? 109 : 512;
					var height = small ? 100 : 512;

					var lanes = GetLanes(small);

					using (var img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
					using (var g = Graphics.FromImage(img))
					{
						DrawThumbnail(g, lanes, small);

						img.Save(Path.Combine(folder, filename), System.Drawing.Imaging.ImageFormat.Png);

						Notification.Create("Thumbnail Saved", "Your thumbnail was saved at:\n" + Path.Combine(folder, filename), PromptIcons.Info, null)
							.Show(Form, 15);
					}
				}
			}
			catch (Exception ex) { ShowPrompt(ex.Message, "Error", PromptButtons.OK, PromptIcons.Error); }
		}

		private void B_CopyDesc_Click(object sender, EventArgs e)
		{
			var desc = Utilities.GetRoadDescription(GetLanes(false), TB_Size.Text, TB_BufferSize.Text, TB_SpeedLimit.Text, RB_USA.Checked);

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
				var appdata = Options.Current.ExportFolder.IfEmpty(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
					, "Colossal Order", "Cities_Skylines", "BlankRoadBuilder", "Roads"));

				Directory.CreateDirectory(appdata);

				var lanes = GetLanes(false);

				if (lanes.Count == 0)
					return;

				if (lanes.Any(x => !x.IsFiller && x.Direction == LaneDirection.None))
				{
					ShowPrompt("You need to specify the direction of all non-filler lanes before you can export this road.", PromptButtons.OK, PromptIcons.Hand);

					return;
				}

				if (lanes.Any(x => x.Type.HasFlag(LaneType.Train)))
				{
					Notification.Create("Train Lanes Detected", "Your road was exported, but it contains train lanes which have no effect.", PromptIcons.Info, null)
						.Show(Form, 15);
				}

				if (lanes.Any(x => x.IsFiller && x.AddStopToFiller && Utilities.GetLaneWidth(x.Type, x) < 2))
				{
					Notification.Create("Invalid Stops Detected", "Your road was exported, some filler lanes that you've added stops to are too small to work.", PromptIcons.Info, null)
						.Show(Form, 15);
				}

				if (L_RoadName.Text.Length > 32)
				{
					Notification.Create("Lane Name too Long", "Your road was exported, but the generated road name is too long for the game.\nPlease keep that in mind", PromptIcons.Info, null)
						.Show(Form, 15);
				}

				var _lanes = GetLanes(false);

				for (var i = 0; i < lanes.Count; i++)
				{
					if (lanes[i].IsFiller || lanes[i].Type == LaneType.Parking || lanes[i].Lanes <= 1)
						continue;

					var bi = lanes[i].Direction == LaneDirection.Both;

					lanes[i].Lanes--;

					if (bi && lanes[i].Lanes == 1)
						lanes[i].Direction = !Options.Current.LHT ? LaneDirection.Forward : LaneDirection.Backwards;

					lanes.Insert(i, new LaneInfo
					{
						Type = lanes[i].Type,
						Direction = bi ? (Options.Current.LHT ? LaneDirection.Forward : LaneDirection.Backwards) : lanes[i].Direction,
						CustomWidth = lanes[i].CustomWidth,
						SpeedLimit = lanes[i].SpeedLimit,
						Elevation = lanes[i].Elevation,
						AddStopToFiller = lanes[i].AddStopToFiller						
					});
				}

				if (Options.Current.LHT)
					lanes.Reverse();

				var roadInfo = new RoadInfo
				{
					Version = 1,
					Name = L_RoadName.Text,
					Description = Utilities.GetRoadDescription(_lanes, TB_Size.Text, TB_BufferSize.Text, TB_SpeedLimit.Text, RB_USA.Checked),
					CustomText = TB_CustomText.Text,
					SmallThumbnail = getImage(true),
					LargeThumbnail = getImage(false),
					TooltipImage = File.ReadAllBytes($"{Utilities.Folder}\\Resources\\tooltip.png"),
					BufferWidth = TB_BufferSize.Text.SmartParseF(0.25f),
					AsphaltWidth = TB_Size.Text.SmartParseF(),
					PavementWidth = TB_PavementWidth.Text.SmartParseF(),
					RegionType = GetRegion(),
					RoadType = GetRoadType(),
					SpeedLimit = TB_SpeedLimit.Text.SmartParseF() * (RB_USA.Checked ? 1.609F : 1F),
					Lanes = lanes,
					LHT = Options.Current.LHT,
					ThumbnailMakerConfig = JsonConvert.SerializeObject(_lanes.Select(x => new TmLane(x)))
				};

				var guid = Guid.NewGuid().ToString();
				var xML = new System.Xml.Serialization.XmlSerializer(typeof(RoadInfo));

				using (var stream = File.Create(Path.Combine(appdata, $"{guid}.xml")))
					xML.Serialize(stream, roadInfo);

				RCC.RefreshConfigs(Path.Combine(appdata, $"{guid}.xml"));
			}
			catch (Exception ex) { ShowPrompt(ex.Message, "Error", PromptButtons.OK, PromptIcons.Error); }

			byte[] getImage(bool small)
			{
				var width = small ? 109 : 512;
				var height = small ? 100 : 512;

				using (var img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
				using (var g = Graphics.FromImage(img))
				{
					DrawThumbnail(g, GetLanes(small), small);

					var converter = new ImageConverter();
					return (byte[])converter.ConvertTo(img, typeof(byte[]));
				}
			}
		}

		private RoadType GetRoadType()
		{
			if (RB_Pedestrian.Checked)
				return RoadType.Pedestrian;

			if (RB_Highway.Checked)
				return RoadType.Highway;

			if (RB_FlatRoad.Checked)
				return RoadType.Flat;

			return RoadType.Road;
		}

		private RegionType GetRegion()
		{
			if (RB_Canada.Checked)
				return RegionType.Canada;

			if (RB_USA.Checked)
				return RegionType.USA;

			return RegionType.Europe;
		}

		private void RCC_LoadConfiguration(object sender, RoadInfo r)
		{
			TB_Size.Text = r.AsphaltWidth == 0 ? string.Empty : r.AsphaltWidth.ToString();
			TB_PavementWidth.Text = r.PavementWidth == 0 ? string.Empty : r.PavementWidth.ToString();
			TB_BufferSize.Text = r.BufferWidth.ToString();
			TB_SpeedLimit.Text = r.SpeedLimit == 0 ? string.Empty : r.SpeedLimit.ToString();
			TB_CustomText.Text = r.CustomText;

			P_Lanes.Controls.Clear(true);

			foreach (var item in r.TmLanes)
			{
				AddLaneControl(item);
			}

			RefreshPreview();
		}

		private void AddLaneControl(LaneInfo item)
		{
			var ctrl = new RoadLane(() => RB_Highway.Checked);

			ctrl.RoadLaneChanged += TB_Name_TextChanged;
			ctrl.LaneType = item.Type;
			ctrl.LaneDirection = item.Direction;
			ctrl.Lanes = item.Lanes;
			ctrl.CustomLaneWidth = item.CustomWidth == 0F ? -1F : item.CustomWidth;
			ctrl.CustomVerticalOffset = item.Elevation == null ? -1F : (float)item.Elevation;
			ctrl.CustomSpeedLimit = item.SpeedLimit == null ? -1F : (float)item.SpeedLimit;
			ctrl.AddStopToFiller = item.AddStopToFiller;

			P_Lanes.Controls.Add(ctrl);

			ctrl.BringToFront();
		}

		private void PB_Click(object sender, EventArgs e)
		{
			try
			{
				var lanes = GetLanes(false);

				using (var img = new Bitmap(512, 512, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
				using (var g = Graphics.FromImage(img))
				{
					DrawThumbnail(g, lanes, false);

					Clipboard.SetDataObject(new Bitmap(img, 256, 256));
				}

				Notification.Create("Thumbnail copied to clipboard", "", PromptIcons.None, () => { }, NotificationSound.None, new Size(240, 32))
					.Show(Form, 5);
			}
			catch (Exception ex) { ShowPrompt(ex.Message, "Error", PromptButtons.OK, PromptIcons.Error); }
		}

		private void L_RoadName_MouseEnter(object sender, EventArgs e)
		{
			L_RoadName.ForeColor = FormDesign.Design.ActiveColor;
		}

		private void L_RoadName_MouseLeave(object sender, EventArgs e)
		{
			L_RoadName.ForeColor = L_RoadName.Text.Length > 32 ? FormDesign.Design.RedColor : FormDesign.Design.ForeColor;
		}

		private void L_RoadName_Click(object sender, EventArgs e)
		{
			L_RoadName.Parent = null;
			B_CopyName.Parent = null;
			TLP_Right.Controls.Add(TB_RoadName, 0, 0);
			TLP_Right.SetColumnSpan(TB_RoadName, 2);
			TB_RoadName.Focus();
			TB_RoadName.MaxLength = 32;
		}

		private void TB_RoadName_Leave(object sender, EventArgs e)
		{
			RefreshPreview();
			TB_RoadName.Parent = null;
			TLP_Right.Controls.Add(L_RoadName, 1, 0);
			TLP_Right.Controls.Add(B_CopyName, 0, 0);
		}

		private void TB_RoadName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.IsInputKey = true;

				TB_RoadName_Leave(sender, e);
			}	
		}

		private void TB_RoadName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.SuppressKeyPress = true;
				e.Handled = true;
			}

			if (TB_RoadName.Text.Length >= 32 && e.KeyData.IsDigitOrLetter())
			{
				e.SuppressKeyPress = true;
				e.Handled = true;
			}
		}
	}
}