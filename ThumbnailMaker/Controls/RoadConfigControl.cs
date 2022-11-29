﻿using Extensions;

using Newtonsoft.Json;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Controls
{
	public class RoadConfigControl : SlickControl
	{
		private Rectangle deleteRect;

		public Bitmap Image { get; }
		public RoadInfo Road { get; }
		public string FileName { get; }
		public DateTime TimeSaved { get; }
		public string RoadSpeed { get; }
		public string RoadSize { get; }

		public event System.EventHandler<RoadInfo> LoadConfiguration;

		public RoadConfigControl(string fileName, out bool valid)
		{
			try
			{
				FileName = fileName;
				TimeSaved = new FileInfo(fileName).LastWriteTime;

				var xml = new XmlSerializer(typeof(RoadInfo));

				using (var stream = File.OpenRead(fileName))
					Road = (RoadInfo)xml.Deserialize(stream);

			using (var ms = new MemoryStream(Road.SmallThumbnail))
			using (var bm = new Bitmap(ms))
				Image = new Bitmap(bm, UI.Scale(new Size(55, 50), UI.UIScale));

				if (!string.IsNullOrWhiteSpace(Road.ThumbnailMakerConfig))
				{
					var lanes = JsonConvert.DeserializeObject<TmLane[]>(Road.ThumbnailMakerConfig).Select(x => (LaneInfo)x).ToList();

					RoadSpeed = Road.SpeedLimit <= 0F ? Utilities.DefaultSpeedSign(lanes, Road.RegionType == RegionType.USA) : Road.SpeedLimit.ToString();
					RoadSize = Road.Width <= 0F ? Utilities.CalculateRoadSize(lanes, Road.BufferSize.ToString()) : Road.Width.ToString();
				}

				Height = 64;
				Dock = DockStyle.Top;
				Cursor = Cursors.Hand;

				valid = true;
			}
			catch { valid = false; }
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			Height = (int)(64 * UI.UIScale);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var deleteSize = UI.Scale(new Size(26, 26), UI.UIScale);

			deleteRect = new Rectangle(Width - deleteSize.Width - 6, Height / 2 - deleteSize.Height / 2 + 2, deleteSize.Width, deleteSize.Height);

			var mouse = PointToClient(Cursor.Position);
			var deleteHovered = deleteRect.Contains(mouse);

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			e.Graphics.FillRoundedRectangle(new SolidBrush(deleteHovered ? FormDesign.Design.AccentBackColor :
				HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor :
				HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.AccentBackColor, 35) : FormDesign.Design.AccentBackColor), ClientRectangle.Pad(3, 6, 3, 1), 4);
			
			SlickButton.DrawButton(e, deleteRect, string.Empty, UI.Font(8.25F), Properties.Resources.I_Delete, null, deleteHovered ? HoverState : HoverState.Normal, ColorStyle.Red);

			var foreColor = !deleteHovered && HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ForeColor;
			
			e.Graphics.DrawString(Road.Name.Substring(4)
				, UI.Font(9.75F, FontStyle.Bold)
				, new SolidBrush(foreColor)
				, ClientRectangle.Pad(65, 10, 36, 20)
				, new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

			foreColor = foreColor.MergeColor(FormDesign.Design.AccentBackColor, 70);

			if (!string.IsNullOrEmpty(RoadSize))
			{
				e.Graphics.DrawImage(Properties.Resources.I_Size.Color(foreColor), new Rectangle(70, 44, 16, 16));

				e.Graphics.DrawString(RoadSize + "m"
					, UI.Font(8.25F)
					, new SolidBrush(foreColor)
					, ClientRectangle.Pad(88, 44, 36, 0));
			}

			if (!string.IsNullOrEmpty(RoadSpeed))
			{
				e.Graphics.DrawImage(Properties.Resources.I_SpeedLimit.Color(foreColor), new Rectangle(130, 44, 16, 16));

				e.Graphics.DrawString(RoadSpeed + (Road.RegionType == RegionType.USA ? "mph" : "km/h")
					, UI.Font(8.25F)
					, new SolidBrush(foreColor)
					, ClientRectangle.Pad(148, 44, 36, 0));
			}

			if (!deleteHovered)
				DrawFocus(e.Graphics, ClientRectangle.Pad(3, 6, 3, 1), 4);

			e.Graphics.TranslateTransform(7, 10);

			using (var texture = new TextureBrush(Image))
				e.Graphics.FillRoundedRectangle(texture, new Rectangle(Point.Empty, UI.Scale(new Size(55, 50), UI.UIScale)), 6);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (deleteRect.Contains(e.Location))
			{
				if (MessagePrompt.Show("Are you sure you want to delete this road configuration?", PromptButtons.YesNo, PromptIcons.Question) == DialogResult.Yes)
				{
					File.Delete(FileName);

					Dispose();
				}

				return;
			}

			LoadConfiguration?.Invoke(this, Road);
		}
	}
}