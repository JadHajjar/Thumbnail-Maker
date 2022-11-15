﻿using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ThumbnailMaker.Domain;

namespace ThumbnailMaker.Controls
{
	public partial class LaneOptionControl : SlickControl
	{
		public LaneOptionControl(LaneType laneType)
		{
			InitializeComponent();
			LaneType = laneType;

			label1.Text = LaneType.ToString().FormatWords();
			pictureBox1.BackColor = LaneInfo.GetColor(LaneType);
			PB_100.Image =  ResourceManager.GetImage(LaneType, true)?.Color(FormDesign.Design.IconColor);
			PB_512.Image = ResourceManager.GetImage(LaneType, false)?.Color(FormDesign.Design.IconColor);
		}

		public LaneType LaneType { get; }

		protected override void DesignChanged(FormDesign design)
		{
			BackColor = design.AccentColor;
			tableLayoutPanel1.BackColor = design.AccentBackColor;
		}

		protected override void UIChanged()
		{
			base.UIChanged();

			label1.Font = UI.Font(10.25F);
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.Clear(tableLayoutPanel1.BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.FillEllipse(new SolidBrush(pictureBox1.BackColor), pictureBox1.ClientRectangle.Pad(1));
		}

		private void PB_512_Paint(object sender, PaintEventArgs e)
		{
			var PB = sender as SlickPictureBox;

			if (PB.Image == null)
			{
				e.Graphics.DrawRectangle(new Pen(PB.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), PB.ClientRectangle.Pad(1));

				return;
			}

			e.Graphics.Clear(tableLayoutPanel1.BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.DrawImage(new Bitmap(PB.Image, new Size(PB.Image.Width * 48 / PB.Image.Height, 48)).Color(PB.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor), new Rectangle(Point.Empty, new Size(48, 48)).CenterR(new Size(PB.Image.Width * 48 / PB.Image.Height, 48)));
		}

		private void PB_100_Paint(object sender, PaintEventArgs e)
		{
			var PB = sender as SlickPictureBox;

			if (PB.Image == null)
			{
				e.Graphics.DrawRectangle(new Pen(PB.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor : FormDesign.Design.AccentColor), PB.ClientRectangle.Pad(1));

				return;
			}

			e.Graphics.Clear(tableLayoutPanel1.BackColor);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.DrawImage(PB.Image.Color(PB.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor), PB.ClientRectangle);
		}

		private void PB_100_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left)
				{
					if (openFileDialog.ShowDialog() != DialogResult.OK)
						return;

					ResourceManager.SetImage(LaneType, sender == PB_100, openFileDialog.FileName);
				}
				else if (e.Button == MouseButtons.Middle)
				{
					if (MessagePrompt.Show($"Are you sure you want to delete the icon for {LaneType.ToString().FormatWords()}?", PromptButtons.YesNo, PromptIcons.Warning) != DialogResult.Yes)
						return;

					ResourceManager.SetImage(LaneType, sender == PB_100, null);
				}

				PB_100.Image = ResourceManager.GetImage(LaneType, true)?.Color(FormDesign.Design.IconColor);
				PB_512.Image = ResourceManager.GetImage(LaneType, false)?.Color(FormDesign.Design.IconColor);
			}
			catch (Exception ex) { MessagePrompt.Show(ex.Message, "Error", PromptButtons.OK, PromptIcons.Error); }
		}

		private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
			{
				Options.Current.LaneColors.Remove(LaneType);
				pictureBox1.BackColor = LaneInfo.GetColor(LaneType);
				Options.Save();
				return;
			}

			var picker = new SlickColorPicker(pictureBox1.BackColor);

			if (picker.ShowDialog() == DialogResult.OK)
			{
				Options.Current.LaneColors[LaneType] = picker.Color;
				pictureBox1.BackColor = LaneInfo.GetColor(LaneType);
				Options.Save();
			}
		}
	}
}