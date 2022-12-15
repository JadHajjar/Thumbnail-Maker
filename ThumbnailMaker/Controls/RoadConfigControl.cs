using Extensions;

using Newtonsoft.Json;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private Rectangle folderRect;

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

				Road = LegacyUtil.LoadRoad(fileName);

				using (var ms = new MemoryStream(Road.SmallThumbnail))
					Image = new Bitmap(ms);

				RoadSpeed = Road.SpeedLimit <= 0F ? Utilities.DefaultSpeedSign(Road.Lanes, Road.RegionType == RegionType.USA).If(x => x == 0, x => "", x => x.ToString()) : Road.SpeedLimit.ToString();
				RoadSize = Road.RoadWidth <= 0F ? Utilities.CalculateRoadSize(Road.Lanes, Road.BufferWidth).If(x => x == 0F, x => "", x => x.ToString("0.#")) : Road.RoadWidth.ToString();

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

			folderRect = new Rectangle(Width - deleteSize.Width - 6, Height / 6 - deleteSize.Height / 6 + 1, deleteSize.Width, deleteSize.Height);
			deleteRect = new Rectangle(Width - deleteSize.Width - 6, Height * 5 / 6 - deleteSize.Height * 5 / 6 + 5, deleteSize.Width, deleteSize.Height);

			var mouse = PointToClient(Cursor.Position);
			var deleteHovered = deleteRect.Contains(mouse);
			var folderHovered = folderRect.Contains(mouse);
			var startX = (int)(55 * UI.UIScale) + 10;

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			e.Graphics.FillRoundedRectangle(new SolidBrush(deleteHovered || folderHovered ? FormDesign.Design.AccentBackColor :
				HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor :
				HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.AccentBackColor, 35) : FormDesign.Design.AccentBackColor), ClientRectangle.Pad(3, 6, 3, 1), 4);
			
			SlickButton.DrawButton(e, deleteRect, string.Empty, UI.Font(8.25F), Properties.Resources.I_Delete, null, deleteHovered ? HoverState : HoverState.Normal, ColorStyle.Red);
			SlickButton.DrawButton(e, folderRect, string.Empty, UI.Font(8.25F), Properties.Resources.I_Folder, null, folderHovered ? HoverState : HoverState.Normal, ColorStyle.Active);

			var bottomY = Height - UI.Font(8.25F).Height - 6;
			var foreColor = !deleteHovered && !folderHovered && HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ForeColor;
			
			e.Graphics.DrawString(Road.Name.RegexRemove("^B?R[B4][RHFP]").Trim()
				, UI.Font(9.75F, FontStyle.Bold)
				, new SolidBrush(foreColor)
				, ClientRectangle.Pad(startX, 10, Width - deleteRect.X - 2, UI.Font(8.25F).Height)
				, new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

			foreColor = foreColor.MergeColor(FormDesign.Design.AccentBackColor, 70);

			var portion = (Width - startX + Width - deleteRect.X - 24 * 6) / 2;

			if (!string.IsNullOrEmpty(RoadSize))
			{
				e.Graphics.DrawImage(Properties.Resources.I_Size.Color(foreColor), new Rectangle(startX + 10, bottomY + (UI.Font(8.25F).Height - 14 )/ 2, 16, 16));

				e.Graphics.DrawString(RoadSize + "m"
					, UI.Font(8.25F)
					, new SolidBrush(foreColor)
					, ClientRectangle.Pad(startX + 29, bottomY, 36, 0));
			}

			if (!string.IsNullOrEmpty(RoadSpeed))
			{
				e.Graphics.DrawImage(Properties.Resources.I_SpeedLimit.Color(foreColor), new Rectangle(startX + 29 + portion, bottomY + (UI.Font(8.25F).Height - 14) / 2, 16, 16));

				e.Graphics.DrawString(RoadSpeed + (Road.RegionType == RegionType.USA ? "mph" : "km/h")
					, UI.Font(8.25F)
					, new SolidBrush(foreColor)
					, ClientRectangle.Pad(startX + 29 + portion + 19, bottomY, 36, 0));
			}

			if (!deleteHovered && !folderHovered)
				DrawFocus(e.Graphics, ClientRectangle.Pad(3, 6, 3, 1), 4);

			e.Graphics.TranslateTransform(7, 3 + (int)(Height - 50 * UI.UIScale) / 2);

			using (var image = new Bitmap(Image, UI.Scale(new Size(55, 50), UI.UIScale)))
			using (var texture = new TextureBrush(image))
				e.Graphics.FillRoundedRectangle(texture, new Rectangle(Point.Empty, UI.Scale(new Size(55, 50), UI.UIScale)), 6);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left && e.Button != MouseButtons.None)
				return;

			if (deleteRect.Contains(e.Location))
			{
				if (MessagePrompt.Show("Are you sure you want to delete this road configuration?", PromptButtons.YesNo, PromptIcons.Question) == DialogResult.Yes)
				{
					File.Delete(FileName);

					Dispose();
				}

				return;
			}

			if (folderRect.Contains(e.Location))
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "explorer",
					Arguments = $"/e, /select, \"{FileName}\""
				});

				return;
			}

			LoadConfiguration?.Invoke(this, Road);
		}
	}
}
