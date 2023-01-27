using SlickControls;

using System.Drawing;

namespace ThumbnailMaker
{
	public partial class SaveThumbDialog : IOSelectionForm
	{
		public SaveThumbDialog(string startingFolder = null) : base(true, new string[0], startingFolder)
		{
			InitializeComponent();

			CB_Small.Location = new Point(10, (P_CustomPanel.Height - CB_Small.Height) / 2);
			CB_Tooltip.Location = new Point(10 + CB_Small.Width, (P_CustomPanel.Height - CB_Tooltip.Height) / 2);

			P_CustomPanel.Visible = true;
			P_CustomPanel.Controls.Add(CB_Small);
			P_CustomPanel.Controls.Add(CB_Tooltip);
		}
	}
}
