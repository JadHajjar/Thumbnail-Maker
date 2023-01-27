using Extensions;

using Newtonsoft.Json.Serialization;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ThumbnailMaker.Domain;

using static System.Windows.Forms.AxHost;

namespace ThumbnailMaker.Controls
{
	internal class WarningsControl : SlickControl
	{
		struct Warning
		{
			public string Title;
			public string Message;
			public TraceLevel Level;

			public Warning(TraceLevel level, string title, string message)
			{
				Level = level;
				Title = title;
				Message = message;
			}
		}

		public RoadInfo Road { get; private set; }
		private List<Warning> Warnings { get; set; }
		private Dictionary<Rectangle, Warning> WarningRecs { get; set; } = new Dictionary<Rectangle, Warning>();

		public WarningsControl()
		{
			Cursor = Cursors.Help;
		}

		internal void SetRoad(RoadInfo road)
		{
			Road = road;
			Warnings = GetWarnings().ToList();
			Visible = Warnings.Any();
			Invalidate();
		}

		private IEnumerable<Warning> GetWarnings()
		{
			if (Road.Lanes.Any(x => x.Type.HasFlag(LaneType.Train)))
			{
				yield return new Warning(TraceLevel.Error, "Train lanes aren't supported", "Your road contains train lanes which are not supported in-game currently.");
			}

			if (Road.Lanes.Any(x => x.Decorations.HasFlag(LaneDecoration.TransitStop) && x.LaneWidth < 2))
			{
				yield return new Warning(TraceLevel.Warning, "Invalid transit stops detected", "Some filler lanes that you've added stops to are too small to work.");
			}

			if (Road.Name.Length > 32)
			{
				yield return new Warning(TraceLevel.Error, "Road name too long", "The maximum length for a road name is 32 charachters, your road name currently exceeds that.");
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			foreach (var item in WarningRecs)
			{
				if (item.Key.Contains(e.Location))
				{
					SlickTip.SetTo(this, item.Value.Message);
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (Road == null)
				return;

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			WarningRecs.Clear();

			var y = 3;
			foreach (var w in Warnings)
			{
				var h = (int)e.Graphics.MeasureString(w.Title, UI.Font(8.25F), Width - 36).Height;

				switch (w.Level)
				{
					case TraceLevel.Error:
						e.Graphics.DrawImage(Properties.Resources.I_Stop.Color(FormDesign.Design.RedColor), new Rectangle(0, y, 36, h).CenterR(16, 16));
						break;
					case TraceLevel.Warning:
						e.Graphics.DrawImage(Properties.Resources.I_Warning.Color(FormDesign.Design.YellowColor), new Rectangle(0, y, 36, h).CenterR(16, 16));
						break;
					case TraceLevel.Info:
						e.Graphics.DrawImage(Properties.Resources.I_Info.Color(FormDesign.Design.IconColor), new Rectangle(0, y, 36, h).CenterR(16, 16));
						break;
				}

				e.Graphics.DrawString(w.Title, UI.Font(8.25F), new SolidBrush(FormDesign.Design.ForeColor), new Rectangle(36, y, Width - 36, h));

				WarningRecs[new Rectangle(0, y, Width, h)] = w;

				y += h + 6;
			}

			Height = y;
		}
	}
}
