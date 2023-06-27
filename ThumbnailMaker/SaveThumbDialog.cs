using SlickControls;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ThumbnailMaker
{
	public partial class SaveThumbDialog : IoForm
	{
		public SaveThumbDialog(string startingFolder = null) : base(true, new IOSelectionDialog() { StartingFolder = startingFolder })
		{
			InitializeComponent();

			OnNextIdle(SetUpCheckboxes);
		}

		private void SetUpCheckboxes()
		{
			CB_Small.Location = new Point(10, (P_CustomPanel.Height - CB_Small.Height) / 2 - (int)(3*UI.FontScale));
			CB_Tooltip.Location = new Point(10 + CB_Small.Width, CB_Small.Top);

			P_CustomPanel.Controls.Add(CB_Small);
			P_CustomPanel.Controls.Add(CB_Tooltip);
		}
	}
}
