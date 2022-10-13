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
			label2.Font = UI.Font(9.75F, FontStyle.Bold);
		}

		private void PB_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(Color.Black);

			var lanes = GetLanes(RB_100.Checked);

			new ThumbnailHandler(e.Graphics)
			{
				RoadSize = TB_Size.Text,
				CustomText = TB_CustomText.Text,
				Small = RB_100.Checked,
				HighWay = RB_Highway.Checked,
				Europe = RB_Europe.Checked,
				USA = RB_USA.Checked,
				Canada = RB_Canada.Checked,
				Speed = TB_SpeedLimit.Text,
				Lanes = lanes
			}.Draw();

			label2.Text = "BR4 " + IsOneWay(lanes).Switch(true, "1W ", false, string.Empty, string.Empty) + lanes.Select(x => x.GetTitle()).WhereNotEmpty().ListStrings("+");
		}

		private void RB_CheckedChanged(object sender, EventArgs e)
		{
			PB.Size = RB_100.Checked ? new Size(109, 100) : new Size(512, 512);
			PB.Invalidate();

			foreach (var item in panel2.Controls.OfType<RoadLane>())
				item.Invalidate();
		}

		private void TB_Name_TextChanged(object sender, EventArgs e)
		{
			PB.Invalidate();
		}

		private List<LaneInfo> GetLanes(bool small) => panel2.Controls.OfType<RoadLane>().Reverse().Select(x => new LaneInfo
		{
			Type = x.LaneType,
			Direction = x.LaneDirection,
			Lanes = x.Lanes,
			Width = (x.LaneType < LaneType.Car ? (10 - Math.Min(x.Lanes, 9)) : 10) * (small ? 2 : 10)
		}).ToList();

		private void B_Save_Click(object sender, EventArgs e)
		{
			var matched = false;
			var files = new List<(string, bool)>
			{
				("asset_thumb.png", true),
				("PreviewImage.png", false),
				("snapshot.png", false),
				("thumbnail.png", false),
			};

			if (File.Exists(Path.Combine(TB_Path.Text, "tooltip.png")))
				File.Copy("Resources\\tooltip.png", Path.Combine(TB_Path.Text, "tooltip.png"), true);

			if (File.Exists(Path.Combine(TB_Path.Text, "asset_tooltip.png")))
				File.Copy("Resources\\tooltip.png", Path.Combine(TB_Path.Text, "asset_tooltip.png"), true);

			foreach (var item in files)
			{
				if (File.Exists(Path.Combine(TB_Path.Text, item.Item1)))
				{
					save(item.Item1, item.Item2);

					matched = true;
				}
			}

			if (!matched)
				save(GetLanes(RB_100.Checked).ListStrings(" + ") + ".png", RB_100.Checked);

			void save(string filename, bool small)
			{
				var width = small ? 109 : 512;
				var height = small ? 100 : 512;

				using (var img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
				using (var g = Graphics.FromImage(img))
				{
					new ThumbnailHandler(g)
					{
						RoadSize = TB_Size.Text,
						CustomText = TB_CustomText.Text,
						Small = small,
						HighWay = RB_Highway.Checked,
						Europe = RB_Europe.Checked,
						USA = RB_USA.Checked,
						Canada = RB_Canada.Checked,
						Speed = TB_SpeedLimit.Text,
						Lanes = GetLanes(small)
					}.Draw();

					img.Save(Path.Combine(TB_Path.Text, filename), System.Drawing.Imaging.ImageFormat.Png);
				}
			}
		}

		private void slickButton1_Click(object sender, EventArgs e)
		{
			var ctrl = new RoadLane(() => RB_Highway.Checked)
			{
				Dock = DockStyle.Top
			};

			ctrl.RoadLaneChanged += TB_Name_TextChanged;

			panel2.Controls.Add(ctrl);

			ctrl.BringToFront();

			var frm = new RoadTypeSelector(ctrl);

			frm.FormClosed += (s, _) => PB.Invalidate();
		}

		private void slickButton2_Click(object sender, EventArgs e)
		{
			Form.PushPanel<PC_Options>(null);
		}

		private void B_CopyDesc_Click(object sender, EventArgs e)
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
					laneDescriptors.Add(lane.Lanes > 3 ? "Seperator" : "Median");
				else if (lane.Direction == LaneDirection.Both)
					laneDescriptors.Add($"2W {lane.Lanes}L {name}");
				else if (lane.Lanes > 0)
					laneDescriptors.Add($"{lane.Lanes}L{lane.Direction.Switch(LaneDirection.Backwards, "B", LaneDirection.Forward, "F", "")} {name}");
				else
					laneDescriptors.Add(name);
			}

			var info = (TB_Size.Text.Length == 0 ? "" : $"{TB_Size.Text}m") +
				(TB_SpeedLimit.Text.Length == 0 ? "" : $" - {TB_SpeedLimit.Text}{RB_USA.Checked.If("mph", "km/h")}");

			Clipboard.SetText($"Blank {(asymetrical ? "Asymmetrical " : oneWay.Switch(true, "One-Way ", false, "Two-Way ", string.Empty))}{lanes.Any(x => x.Type.HasFlag(LaneType.Bike)).If("Bike ")}Road.  " +
				laneDescriptors.WhereNotEmpty().ListStrings(" + ") +
				info.IfEmpty("", $"  ({info})") +
				"  This road comes with no markings, use Intersection Marking Tool to mark it.");
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
			Clipboard.SetText(label2.Text);
		}
	}
}