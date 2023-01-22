using Extensions;

using SlickControls;

using System;
using System.Drawing;
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
			pictureBox1.BackColor = ThumbnailLaneInfo.GetColor(LaneType);
			PB_512.Image = ResourceManager.GetImage(LaneType, false);
			SS_Width.Value = LaneSizeOptions.LaneSizes[laneType];

			if (laneType == LaneType.Parking)
			{
				SlickTip.SetTo(slickIcon1, "Reset vertical parking width to " + LaneSizeOptions.GetDefaultLaneWidth(laneType));
				SlickTip.SetTo(slickIcon2, "Reset diagonal parking width to 5");
				SlickTip.SetTo(slickIcon3, "Reset horizontal parking width to 6");

				slickIcon1.Image = Properties.Resources.I_Vertical;
				slickIcon2.Image = Properties.Resources.I_Diagonal;
				slickIcon3.Image = Properties.Resources.Icon_Horizontal;

				slickSlider1.Value = LaneSizeOptions.LaneSizes.DiagonalParkingSize;
				slickSlider2.Value = LaneSizeOptions.LaneSizes.HorizontalParkingSize;

				slickIcon2.Visible = slickSlider1.Visible = slickSlider2.Visible = label4.Visible = label2.Visible = true;
			}
			else
			{
				SlickTip.SetTo(slickIcon1, "Reset default width to " + LaneSizeOptions.GetDefaultLaneWidth(laneType));
			}
		}

		public LaneOptionControl(string name)
		{
			InitializeComponent();
			LaneType--;

			label1.Text = name;
			pictureBox1.Visible = false;
			PB_512.Image = ResourceManager.GetImage(name, false)?.Color(FormDesign.Design.IconColor);
			PropertyName = name;

			SS_Width.Visible = slickIcon1.Visible = label3.Visible = slickSpacer3.Visible = false;
		}

		public LaneType LaneType { get; }
		public string PropertyName { get; }

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

			if (PropertyName == "Logo")
			{
				e.Graphics.DrawImage(PB.Image.Color(PB.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor), new Rectangle(Point.Empty, new Size(16, 16)).CenterR(new Size(16, PB.Image.Height * 16 / PB.Image.Width)));
			}
			else
			{
				e.Graphics.DrawImage(PB.Image.Color(PB.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor), PB.ClientRectangle);
			}
		}

		private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
			{
				Options.Current.LaneColors.Remove(LaneType);
				pictureBox1.BackColor = ThumbnailLaneInfo.GetColor(LaneType);
				Options.Save();
				return;
			}

			var picker = new SlickColorPicker(pictureBox1.BackColor);

			if (picker.ShowDialog() == DialogResult.OK)
			{
				Options.Current.LaneColors[LaneType] = picker.Color;
				pictureBox1.BackColor = ThumbnailLaneInfo.GetColor(LaneType);
				Options.Save();
			}
		}

		private void SS_Width_ValuesChanged(object sender, EventArgs e)
		{
			LaneSizeOptions.LaneSizes[LaneType] = (float)Math.Round(SS_Width.Value, 1);

			label3.Text = $"{Math.Round(SS_Width.Value, 1):0.#}m";
		}

		private void slickIcon1_Click(object sender, EventArgs e)
		{
			SS_Width.Value = LaneSizeOptions.GetDefaultLaneWidth(LaneType);
		}

		private void slickSlider1_ValuesChanged(object sender, EventArgs e)
		{
			LaneSizeOptions.LaneSizes.DiagonalParkingSize = (float)Math.Round(slickSlider1.Value, 1);

			label2.Text = $"{Math.Round(slickSlider1.Value, 1):0.#}m";
		}

		private void slickSlider2_ValuesChanged(object sender, EventArgs e)
		{
			LaneSizeOptions.LaneSizes.HorizontalParkingSize = (float)Math.Round(slickSlider2.Value, 1);

			label4.Text = $"{Math.Round(slickSlider2.Value, 1):0.#}m";
		}

		private void slickIcon2_Click(object sender, EventArgs e)
		{
			slickSlider1.Value = 5F;
		}

		private void slickIcon3_Click(object sender, EventArgs e)
		{
			slickSlider2.Value = 6F;
		}
	}
}
