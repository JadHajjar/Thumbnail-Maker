using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
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
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			label1.Font = UI.Font(9.75F, FontStyle.Bold);
			L_RoadName.Font = UI.Font(9.75F, FontStyle.Bold);
		}

		private void PB_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.Black);

			var lanes = GetLanes(false);

			new ThumbnailHandler(e.Graphics)
			{
				RoadSize = TB_Size.Text.IfEmpty(CalculateRoadSize(lanes)),
				CustomText = TB_CustomText.Text,
				Small = false,
				HighWay = RB_Highway.Checked,
				Europe = RB_Europe.Checked,
				USA = RB_USA.Checked,
				Canada = RB_Canada.Checked,
				Speed = TB_SpeedLimit.Text.IfEmpty(DefaultSpeedSign(lanes)),
				Lanes = lanes
			}.Draw();

			L_RoadName.Text = "BR4 " + IsOneWay(lanes).Switch(true, "1W ", false, string.Empty, string.Empty) + lanes.Select(x => x.GetTitle()).WhereNotEmpty().ListStrings("+");
		}

		private void RB_CheckedChanged(object sender, EventArgs e)
		{
			PB.Size = new Size(512, 512);
			PB.Invalidate();

			foreach (var item in P_Lanes.Controls.OfType<RoadLane>())
				item.Invalidate();
		}

		private void TB_Name_TextChanged(object sender, EventArgs e)
		{
			PB.Invalidate();
		}

		private void B_Clear_Click(object sender, EventArgs e)
		{
			P_Lanes.Controls.Clear(true);

			PB.Invalidate();
		}

		private List<LaneInfo> GetLanes(bool small) => P_Lanes.Controls.OfType<RoadLane>().Reverse().Select(x => new LaneInfo
		{
			Type = x.LaneType,
			Direction = x.LaneDirection,
			Lanes = x.Lanes,
			Width = (x.LaneType < LaneType.Car ? (10 - Math.Min(x.Lanes, 9)) : 10) * (small ? 2 : 10)
		}).ToList();

		private void B_Save_Click(object sender, EventArgs e)
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
				File.Copy("Resources\\tooltip.png", Path.Combine(folder, "tooltip.png"), true);

			if (File.Exists(Path.Combine(folder, "asset_tooltip.png")))
				File.Copy("Resources\\tooltip.png", Path.Combine(folder, "asset_tooltip.png"), true);

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
					new ThumbnailHandler(g)
					{
						RoadSize = TB_Size.Text.IfEmpty(CalculateRoadSize(lanes)),
						CustomText = TB_CustomText.Text,
						Small = small,
						HighWay = RB_Highway.Checked,
						Europe = RB_Europe.Checked,
						USA = RB_USA.Checked,
						Canada = RB_Canada.Checked,
						Speed = TB_SpeedLimit.Text.IfEmpty(DefaultSpeedSign(lanes)),
						Lanes = lanes
					}.Draw();

					img.Save(Path.Combine(folder, filename), System.Drawing.Imaging.ImageFormat.Png);
				}
			}
		}

		private string DefaultSpeedSign(List<LaneInfo> lanes)
		{
			if (lanes.Any(x => (x.Type & (LaneType.Car | LaneType.Bus | LaneType.Highway)) != 0))
				return RB_USA.Checked ? "25" : "40";

			return string.Empty;
		}

		private string CalculateRoadSize(List<LaneInfo> lanes)
		{
			if (lanes.Count == 0)
				return string.Empty;

			var size = (TB_BufferSize.Text.SmartParseF() * 2 + lanes.Sum(x => LaneInfo.GetLaneTypes(x.Type).Max(y => GetLaneWidth(y, x))));

			return (Math.Ceiling(size * 2) / 2F).ToString("0.#");
		}

		private static float GetLaneWidth(LaneType type, LaneInfo lane)
		{
			switch (type)
			{
				case LaneType.Empty:
				case LaneType.Grass:
				case LaneType.Pavement:
				case LaneType.Gravel:
					return 0.03F * lane.FillerSize;

				case LaneType.Trees:
					return 0.04F * lane.FillerSize;

				case LaneType.Tram:
				case LaneType.Car:
				case LaneType.Trolley:
				case LaneType.Emergency:
					return 3F * lane.Lanes;

				case LaneType.Pedestrian:
				case LaneType.Bike:
					return 2F * lane.Lanes;

				case LaneType.Parking:
					return lane.DiagonalParking ? 3.5F : lane.HorizontalParking ? 5F : 2F;

				case LaneType.Highway:
				case LaneType.Bus:
				case LaneType.Train:
					return 4F * lane.Lanes;
			}

			return 3F * lane.Lanes;
		}

		private void B_Add_Click(object sender, EventArgs e)
		{
			var ctrl = new RoadLane(() => RB_Highway.Checked)
			{
				Dock = DockStyle.Top
			};

			ctrl.RoadLaneChanged += TB_Name_TextChanged;

			P_Lanes.Controls.Add(ctrl);

			ctrl.BringToFront();

			var frm = new RoadTypeSelector(ctrl);

			frm.FormClosed += (s, _) => PB.Invalidate();
		}

		private void B_Options_Click(object sender, EventArgs e)
		{
			Form.PushPanel<PC_Options>(null);
		}

		private void B_CopyDesc_Click(object sender, EventArgs e)
		{
			var desc = GetRoadDescription();

			Clipboard.SetText(desc);
		}

		private string GetRoadDescription()
		{
			var lanes = GetLanes(true);

			var skip = false;
			var oneWay = IsOneWay(lanes);
			var asymetrical = lanes.Where(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Backwards).Sum(x => x.Lanes) != lanes.Where(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Forward).Sum(x => x.Lanes) && lanes.Any(x => x.Type.HasFlag(LaneType.Car) && x.Direction == LaneDirection.Backwards && x.Lanes > 0);
			var laneDescriptors = new List<string>();

			foreach (var lane in lanes)
			{
				if (skip || lane.Type == LaneType.Pedestrian)
					continue;

				var types = LaneInfo.GetLaneTypes(lane.Type).Select(x => x.ToString());
				var name = types.Count() > 1 ? $"Shared {types.ListStrings(" & ")}" : types.First();

				if (lane.Type < LaneType.Trees)
					laneDescriptors.Add(lane.Lanes > 3 ? "Separator" : "Median");
				else if (lane.Direction == LaneDirection.Both && lane.Type != LaneType.Parking)
					laneDescriptors.Add($"2W {lane.Lanes}L {name}");
				else if (lane.Lanes > 0 && lane.Type != LaneType.Parking)
					laneDescriptors.Add($"{lane.Lanes}L{lane.Direction.Switch(LaneDirection.Backwards, "B", LaneDirection.Forward, "F", "")} {name}");
				else
					laneDescriptors.Add(name);
			}

			var info = (TB_Size.Text.Length == 0 ? "" : $"{TB_Size.Text}m") +
				(TB_SpeedLimit.Text.Length == 0 ? "" : $" - {TB_SpeedLimit.Text}{RB_USA.Checked.If("mph", "km/h")}");

			var desc = $"Blank {(asymetrical ? "Asymmetrical " : oneWay.Switch(true, "One-Way ", false, "Two-Way ", string.Empty))}{lanes.Any(x => x.Type.HasFlag(LaneType.Bike)).If("Bike ")}Road.  " +
				laneDescriptors.WhereNotEmpty().ListStrings(" + ") +
				info.IfEmpty("", $"  ({info})") +
				"  This road comes with no markings, use Intersection Marking Tool to mark it.";
			return desc;
		}

		private bool? IsOneWay(List<LaneInfo> lanes)
		{
			var car = lanes.FirstOrDefault(x => x.Type.HasFlag(LaneType.Car));
			var bus = lanes.FirstOrDefault(x => x.Type.HasFlag(LaneType.Bus));
			var bike = lanes.FirstOrDefault(x => x.Type.HasFlag(LaneType.Bike));

			if (car != null)
			{
				return car.Direction != LaneDirection.Both && lanes
					.Where(x => x.Type.HasFlag(LaneType.Car))
					.All(x => x.Direction == car.Direction);
			}

			if (bus != null)
			{
				return bus.Direction != LaneDirection.Both && lanes
					.Where(x => x.Type.HasFlag(LaneType.Bus))
					.All(x => x.Direction == bus.Direction);
			}

			if (bike != null)
			{
				return bike.Direction != LaneDirection.Both && lanes
					.Where(x => x.Type.HasFlag(LaneType.Bike))
					.All(x => x.Direction == bike.Direction);
			}

			return null;
		}

		private void slickButton3_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(L_RoadName.Text);
		}

		private void B_Export_Click(object sender, EventArgs e)
		{
			var appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
				, "Colossal Order", "Cities_Skylines", "BlankRoadBuilder", "Roads");

			Directory.CreateDirectory(appdata);

			var lanes = GetLanes(false);

			if (lanes.Count == 0)				
				return;

			if (lanes.Any(x => !x.IsFiller && x.Direction == LaneDirection.None))
			{
				ShowPrompt("You need to specify the direction of all non-filler lanes before you can export this road.", PromptButtons.OK, PromptIcons.Hand);

				return;
			}

			if (!RB_Highway.Checked)
			{
				if (lanes[0].Type == LaneType.Pedestrian)
					lanes.RemoveAt(0);

				if (lanes.Count > 0 && lanes[lanes.Count - 1].Type == LaneType.Pedestrian)
					lanes.RemoveAt(lanes.Count - 1);
			}

			if (lanes.Count == 0)
				return;

			for (var i = 0; i < lanes.Count; i++)
			{
				if (lanes[i].IsFiller || lanes[i].Type == LaneType.Parking || lanes[i].Lanes <= 1)
					continue;

				var bi = lanes[i].Direction == LaneDirection.Both;

				lanes[i].Lanes--;

				if (bi && lanes[i].Lanes == 1)
					lanes[i].Direction = LaneDirection.Forward;

				lanes.Insert(i, new LaneInfo
				{
					Type = lanes[i].Type,
					Direction = bi ? LaneDirection.Backwards : lanes[i].Direction
				});
			}

			var roadInfo = new RoadInfo
			{
				Name = L_RoadName.Text,
				Description = GetRoadDescription(),
				SmallThumbnail = getImage(true),
				LargeThumbnail = getImage(false),
				BufferSize = TB_BufferSize.Text.SmartParseF(0.25f),
				Width = TB_Size.Text.SmartParseF(),
				Highway = RB_Highway.Checked,
				SpeedLimit = TB_SpeedLimit.Text.SmartParseF() * (RB_USA.Checked ? 1.609F : 1F),
				Lanes = lanes
			};

			var xML = new System.Xml.Serialization.XmlSerializer(typeof(RoadInfo));

			using (var stream = File.Create(Path.Combine(appdata, "road.xml")))
				xML.Serialize(stream, roadInfo);

			byte[] getImage(bool small)
			{
				var width = small ? 109 : 512;
				var height = small ? 100 : 512;

				var _lanes = GetLanes(small);

				using (var img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
				using (var g = Graphics.FromImage(img))
				{
					new ThumbnailHandler(g)
					{
						RoadSize = TB_Size.Text.IfEmpty(CalculateRoadSize(_lanes)),
						CustomText = TB_CustomText.Text,
						Small = small,
						HighWay = RB_Highway.Checked,
						Europe = RB_Europe.Checked,
						USA = RB_USA.Checked,
						Canada = RB_Canada.Checked,
						Speed = TB_SpeedLimit.Text.IfEmpty(DefaultSpeedSign(_lanes)),
						Lanes = _lanes
					}.Draw();

					var converter = new ImageConverter();
					return (byte[])converter.ConvertTo(img, typeof(byte[]));
				}
			}
		}
	}
}