using Extensions;

using SlickControls;

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Controls
{
	internal class WarningsControl : SlickControl
	{
		private struct Warning
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
			var roadSize = Utilities.CalculateRoadSize(Road.Lanes, Road.BufferWidth, out var leftP, out var rightP);

			if (Road.Lanes.Any(x => x.Type.HasFlag(LaneType.Train)))
			{
				yield return new Warning(TraceLevel.Error, "Train lanes aren't supported", "Your road contains train lanes which are not supported in-game currently.");
			}

			if (leftP < 1.5F || rightP < 1.5F)
			{
				yield return new Warning(TraceLevel.Info, "Road size doesn't match your lanes", "The road size is being rounded up to accommodate for the minimum sidewalk width of 1.5m for each side.");
				yield return new Warning(TraceLevel.Warning, "Sidewalk width is too small", "The minimum sidewalk width is 1.5m, Road Builder will automatically round up your sidewalks.");
			}

			if (roadSize > Road.RoadWidth && Road.RoadWidth > 0)
			{
				yield return new Warning(TraceLevel.Warning, "Road size is larger than your custom width", "The total size of the lanes is larger than the custom width you've used, the custom width will be ignored.");
			}

			if (Road.Lanes.Any(x => x.Decorations.HasFlag(LaneDecoration.TransitStop) && x.LaneWidth <= 1.5F))
			{
				yield return new Warning(TraceLevel.Warning, "Invalid transit stops detected", "Some filler lanes that you've added stops to are too small to work properly.");
			}

			if (Road.Description.Length > 1024)
			{
				yield return new Warning(TraceLevel.Warning, "Road description is too long", "The maximum length for a road description is 500 characters, your road description currently exceeds that.");
			}

			if (Road.Name.Length > 32)
			{
				yield return new Warning(TraceLevel.Error, "Road name is too long", "The maximum length for a road name is 32 characters, your road name currently exceeds that.");
			}

			if (Road.Lanes.Count > 100)
			{
				yield return new Warning(TraceLevel.Error, "Lane limit exceeded", "There are too many lanes in this road for it to work in-game.");
			}

			if (Road.TotalRoadWidth > 100)
			{
				yield return new Warning(TraceLevel.Error, "Road width is too high", "This road is too large to work properly in-game in-game.");
			}

			if (IsCurbCentered())
			{
				yield return new Warning(TraceLevel.Error, "Curb is too close to the center", "Intersections won't work if the curb is too close to the middle of the whole road.");
			}
		}

		private bool IsCurbCentered()
		{
			if (Road.Lanes.IndexOf(Road.Lanes.First(x => x.Type == LaneType.Curb)) + 1 == Road.Lanes.IndexOf(Road.Lanes.Last(x => x.Type == LaneType.Curb)))
			{
				return false;
			}

			var total = Road.Lanes.Sum(x => x.LaneWidth);
			var ind = total / -2F;
			var Lanes = Road.Lanes.ToDictionary(x => ind += x.LaneWidth);
			var leftIndex = Lanes.First(x => x.Value.Type == LaneType.Curb).Key + Road.BufferWidth;
			var rightIndex = Lanes.Last(x => x.Value.Type == LaneType.Curb).Key + 2 * Road.BufferWidth - Lanes.Last(x => x.Value.Type == LaneType.Curb).Value.LaneWidth;

			return leftIndex.IsWithin(-1.5F, int.MaxValue) || rightIndex.IsWithin(int.MinValue, 1.5F);
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
			{
				return;
			}

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			WarningRecs.Clear();

			var y = 3;
			foreach (var w in Warnings.OrderByDescending(x => x.Level))
			{
				var h = (int)e.Graphics.MeasureString(w.Title, UI.Font(8.25F), Width - 36).Height;
				var fore = FormDesign.Design.ForeColor;

				switch (w.Level)
				{
					case TraceLevel.Error:
						e.Graphics.DrawImage(Properties.Resources.I_Stop.Color(fore = FormDesign.Design.RedColor), new Rectangle(0, y, 36, h).CenterR(16, 16));
						break;
					case TraceLevel.Warning:
						e.Graphics.DrawImage(Properties.Resources.I_Warning.Color(fore = FormDesign.Design.YellowColor), new Rectangle(0, y, 36, h).CenterR(16, 16));
						break;
					case TraceLevel.Info:
						e.Graphics.DrawImage(Properties.Resources.I_Info.Color(fore = FormDesign.Design.IconColor), new Rectangle(0, y, 36, h).CenterR(16, 16));
						break;
				}

				e.Graphics.DrawString(w.Title, UI.Font(8.25F), new SolidBrush(HoverState.HasFlag(HoverState.Hovered) && new Rectangle(0, y, Width, h).Contains(PointToClient(Cursor.Position)) ? fore : FormDesign.Design.ForeColor), new Rectangle(36, y, Width - 36, h));

				WarningRecs[new Rectangle(0, y, Width, h)] = w;

				y += h + 6;
			}

			Height = y;
		}
	}
}
