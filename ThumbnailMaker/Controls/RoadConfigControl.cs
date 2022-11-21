using Extensions;

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

namespace ThumbnailMaker.Controls
{
	public class RoadConfigControl : SlickControl
	{
		private readonly Rectangle _loadRect;
		private readonly Rectangle _deleteRect;

		public Bitmap Image { get; }
		public RoadInfo Road { get; }
		public string FileName { get; }

		public event System.EventHandler<RoadInfo> LoadConfiguration;

		public RoadConfigControl(string fileName)
		{
			FileName = fileName;

			var x = new XmlSerializer(typeof(RoadInfo));

			using (var stream = File.OpenRead(fileName))
				Road = (RoadInfo)x.Deserialize(stream);

			using (var ms = new MemoryStream(Road.SmallThumbnail))
			using (var bm = new Bitmap(ms))
				Image = new Bitmap(bm, 55, 50);

			Height = 64;
			Dock = DockStyle.Top;

			_loadRect = new Rectangle(75, 33, 85, 26);
			_deleteRect = new Rectangle(170, 33, 85, 26);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(3, 6, 3, 0), 4);

			e.Graphics.DrawString(Road.Name
				, new Font(UI.FontFamily, 9F, FontStyle.Bold)
				, new SolidBrush(FormDesign.Design.ForeColor)
				, ClientRectangle.Pad(65, 12, 3, 0));

			var mouse = PointToClient(Cursor.Position);
			SlickButton.DrawButton(e, _loadRect, "Load", new Font(UI.FontFamily, 8.25F), Properties.Resources.I_Load, null, _loadRect.Contains(mouse) ? HoverState : HoverState.Normal);
			SlickButton.DrawButton(e, _deleteRect, "Delete", new Font(UI.FontFamily, 8.25F), Properties.Resources.I_Delete, null, _deleteRect.Contains(mouse) ? HoverState : HoverState.Normal, ColorStyle.Red);

			e.Graphics.TranslateTransform(7, 10);

			using (var texture = new TextureBrush(Image))
				e.Graphics.FillRoundedRectangle(texture, new Rectangle(0, 0, 55, 50), 6);

			Cursor = _loadRect.Contains(mouse) || _deleteRect.Contains(mouse) ? Cursors.Hand : Cursors.Default;
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (_loadRect.Contains(e.Location))
			{
				LoadConfiguration?.Invoke(this, Road);
			}

			if (_deleteRect.Contains(e.Location))
			{
				File.Delete(FileName);

				Dispose();
			}
		}
	}
}
