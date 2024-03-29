﻿using Extensions;

using SlickControls;

using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Controls
{
	public class RoadConfigControl : SlickControl, IAnimatable
	{
		private bool _selected;

		public Bitmap Image { get; private set; }
		public RoadInfo Road { get; private set; }
		public string FileName { get; }
		public int AnimatedValue { get; set; }
		public int TargetAnimationValue { get; set; }
		public bool Selected { get => _selected; set { _selected = value; Invalidate(); } }

		public event System.EventHandler<RoadInfo> LoadConfiguration;

		public RoadConfigControl(string fileName, RoadInfo road, out bool valid)
		{
			try
			{
				FileName = fileName;
				Road = road;
				Margin = new Padding(6);
				Cursor = Cursors.Hand;

				SlickTip.SetTo(this, road.Description);

				valid = true;
			}
			catch { valid = false; }
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			Size = UI.UIScale == 1 ? new Size(100, 142) : UI.Scale(new Size(98, 138), UI.UIScale);

			using (var ms = new MemoryStream(UI.UIScale > 1 ? Road.LargeThumbnail : Road.SmallThumbnail))
			{
				Image = new Bitmap(ms);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SetUp(BackColor);

			var foreColor = HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : FormDesign.Design.ForeColor;

			if (HoverState.HasFlag(HoverState.Pressed))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), new Rectangle(Point.Empty, new Size(Width - 1, Height - 2)), 6);
			}

			var selectedHeight = 0;

			if (Selected)
			{
				selectedHeight = (int)e.Graphics.Measure("SELECTED", UI.Font(7F, FontStyle.Bold)).Height + Padding.Top;

				e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), new Rectangle(Point.Empty, new Size(Width - 1, Width - 2 + selectedHeight)).Pad(0, 10, 0, 0), 6);

				e.Graphics.DrawString("SELECTED"
					, UI.Font(7F, FontStyle.Bold)
					, new SolidBrush(foreColor)
					, new Rectangle(Point.Empty, new Size(Width - 1, Width - 2 + selectedHeight))
					, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far });
			}

			e.Graphics.DrawString(Road.Name.Trim().Replace(" ", " ")
				, UI.Font(7.5F, FontStyle.Bold)
				, new SolidBrush(foreColor)
				, new Rectangle(3, Width + 2, Width - 6, Height - Width + 3).Pad(0, (Selected ? selectedHeight : 0),0,0).AlignToFontSize(UI.Font(7.5F, FontStyle.Bold), ContentAlignment.TopCenter)
				, new StringFormat { Alignment = StringAlignment.Center });

			Height = Width + 6 + (Selected ? selectedHeight : 0) +
				(int)e.Graphics.Measure(Road.Name.Trim().Replace(" ", " ")
				, UI.Font(7.5F, FontStyle.Bold)
				, Width - 6).Height;

			e.Graphics.DrawRoundedImage(Image, new Rectangle(Point.Empty, new Size(Width - 1, Width - 2)), 6);

			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb((byte)(AnimatedValue * 1.75), BackColor)), ClientRectangle);

			if (AnimatedValue > 0)
			{
				e.Graphics.DrawBannersOverImage(this, ClientRectangle.Pad(-3), Road.Tags.Concat(Road.AutoTags).Select(x => new Banner(x, BannerStyle.Text, Properties.Resources.I_Tag)).ToList(), 7f, AnimatedValue / 100.0);
			}
		}

		protected override void OnHoverStateChanged()
		{
			base.OnHoverStateChanged();
			TargetAnimationValue = HoverState.HasFlag(HoverState.Hovered) && !HoverState.HasFlag(HoverState.Pressed) ? 100 : 0;
			if (TargetAnimationValue != AnimatedValue)
			{
				AnimationHandler.Animate(this, TargetAnimationValue.If(0, 0.7, 1.35));
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				LoadConfiguration?.Invoke(this, Road);
				return;
			}

			if (e.Button == MouseButtons.Right)
			{
				SlickToolStrip.Show(FindForm() as SlickForm, PointToScreen(e.Location),
					new SlickStripItem("Load configuration", () => LoadConfiguration?.Invoke(this, Road), Properties.Resources.I_Load),
					new SlickStripItem(""),
					new SlickStripItem("Copy thumbnail", CopyThumbnail, Properties.Resources.I_Copy),
					new SlickStripItem("Copy file", CopyFile, Properties.Resources.I_Copy),
					new SlickStripItem("Open file location", OpenFileLocation, Properties.Resources.I_Folder),
					new SlickStripItem(""),
					new SlickStripItem("Refresh thumbnail", RefreshThumbnail, Properties.Resources.I_Refresh),
					new SlickStripItem(""),
					new SlickStripItem("Delete configuration", DeleteConfiguration, Properties.Resources.I_Delete));
				return;
			}

			LoadConfiguration?.Invoke(this, Road);
		}

		private void CopyThumbnail()
		{
			using (var ms = new MemoryStream(Road.LargeThumbnail))
			using (var img = new Bitmap(ms))
			{
				Clipboard.SetDataObject(new Bitmap(img, 256, 256));

				Notification.Create("Thumbnail copied to clipboard", "", PromptIcons.None, () => { }, NotificationSound.None, new Size(240, 32))
					.Show(FindForm(), 5);
			}
		}

		private void DeleteConfiguration()
		{
			if (MessagePrompt.Show("Are you sure you want to delete this road configuration?\r\nYou won't be able to recover it afterwards.", PromptButtons.YesNo, PromptIcons.Question) == DialogResult.Yes)
			{
				File.Delete(FileName);

				Dispose();
			}
		}

		private void RefreshThumbnail()
		{
			Road = LegacyUtil.LoadRoad(FileName);

			Utilities.ExportRoad(Road, Path.GetFileName(FileName));

			UIChanged();

			Invalidate();

			Notification.Create("Regenerated this road's thumbnail", "", PromptIcons.None, () => { }, NotificationSound.None, new Size(260, 32))
				.Show(FindForm(), 5);
		}

		private void OpenFileLocation()
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "explorer",
				Arguments = $"/e, /select, \"{FileName}\""
			});
		}

		private void CopyFile()
		{
			Clipboard.SetFileDropList(new System.Collections.Specialized.StringCollection() { FileName });
		}
	}
}
