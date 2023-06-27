using Extensions;

using SlickControls;

using System.Drawing;
using System.Windows.Forms;

using ThumbnailMaker.Domain;

namespace ThumbnailMaker.Controls
{
	internal class CurrentlyEditingControl : SlickControl
	{
		public event System.EventHandler<RoadInfo> LoadConfiguration;

		public RoadConfigControl Control { get; private set; }
		public RoadInfo Road { get; private set; }

		internal void SetRoad(RoadConfigControl roadConfigControl)
		{
			Clear();
			Control = roadConfigControl;
			Control.Selected = true;
			Road = Control.Road;
			Visible = true;
			Invalidate();
			SlickTip.SetTo(this, "The road you're currently editing.\r\nExporting will update this road instead of creating a new one");
		}

		internal void Clear()
		{
			if (Control != null)
			{
				Control.Selected = false;
			}

			Control = null;
			Road = null;
			Visible = false;
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			var cancelRect = new Rectangle(Width - 36, 0, 36, Height);
			var undoRect = new Rectangle(0, 0, 36, Height);

			if (cancelRect.Contains(e.Location) && e.Button == MouseButtons.Left)
			{
				Clear();
			}

			if (undoRect.Contains(e.Location) && e.Button == MouseButtons.Left)
			{
				LoadConfiguration?.Invoke(Control, Road);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			var cancelRect = new Rectangle(Width - 36, 0, 36, Height);
			var undoRect = new Rectangle(0, 0, 36, Height);

			Cursor = cancelRect.Contains(e.Location) || undoRect.Contains(e.Location) ? Cursors.Hand : Cursors.Default;

			SlickTip.SetTo(this, cancelRect.Contains(e.Location) ? "Stop editing this road" : undoRect.Contains(e.Location) ? "Reset the changes you've made to this road" : "The road you're currently editing.\r\nExporting will update this road instead of creating a new one");
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (Road == null)
			{
				return;
			}

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			var cancelRect = new Rectangle(Width - 36, 0, 36, Height);
			var undoRect = new Rectangle(0, 0, 36, Height);

			var newHeight = 18 + (int)e.Graphics.Measure(Road.Name.Trim().Replace(" ", " ")
				, UI.Font(8.25F, FontStyle.Bold), ClientRectangle.Pad(cancelRect.Width, 16, cancelRect.Width, 0).Width).Height;

			if (newHeight != Height)
			{
				Height = newHeight;
			}

			if (HoverState.HasFlag(HoverState.Hovered) && cancelRect.Contains(PointToClient(Cursor.Position)))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(75, FormDesign.Design.RedColor)), ClientRectangle.Pad(1), 4);
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.RedColor), cancelRect.Pad(1), 4);
				e.Graphics.DrawImage(Properties.Resources.I_Cancel.Color(FormDesign.Design.ActiveForeColor), cancelRect.CenterR(16, 16));
				e.Graphics.DrawImage(Properties.Resources.I_Reset.Color(FormDesign.Design.IconColor), undoRect.CenterR(16, 16));
			}
			else if (HoverState.HasFlag(HoverState.Hovered) && undoRect.Contains(PointToClient(Cursor.Position)))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(75, FormDesign.Design.GreenColor)), ClientRectangle.Pad(1), 4);
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.GreenColor), undoRect.Pad(1), 4);
				e.Graphics.DrawImage(Properties.Resources.I_Reset.Color(FormDesign.Design.ActiveForeColor), undoRect.CenterR(16, 16));
				e.Graphics.DrawImage(Properties.Resources.I_Cancel.Color(FormDesign.Design.IconColor), cancelRect.CenterR(16, 16));
			}
			else
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(75, FormDesign.Design.ActiveColor)), ClientRectangle.Pad(1), 4);

				e.Graphics.DrawImage(Properties.Resources.I_Reset.Color(FormDesign.Design.IconColor), undoRect.CenterR(16, 16));
				e.Graphics.DrawImage(Properties.Resources.I_Cancel.Color(FormDesign.Design.IconColor), cancelRect.CenterR(16, 16));
			}

			e.Graphics.DrawString("Currently Editing", UI.Font(6.75F), new SolidBrush(FormDesign.Design.InfoColor), ClientRectangle.Pad(1), new StringFormat { Alignment = StringAlignment.Center });

			e.Graphics.DrawString(Road.Name.Trim().Replace(" ", " ")
				, UI.Font(8.25F, FontStyle.Bold)
				, new SolidBrush(FormDesign.Design.ForeColor)
				, ClientRectangle.Pad(cancelRect.Width, 16, cancelRect.Width, 0)
				, new StringFormat { Alignment = StringAlignment.Center });
		}
	}
}
