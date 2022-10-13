using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using ThumbnailMaker.Controls;
using ThumbnailMaker.Domain;

namespace ThumbnailMaker
{
	public partial class PC_Options : PanelContent
	{
		public PC_Options()
		{
			InitializeComponent();

			foreach (LaneType laneType in Enum.GetValues(typeof(LaneType)))
			{
				if (laneType == LaneType.Empty)
					continue;

				var ctrl = new LaneOptionControl(laneType);

				tableLayoutPanel1.Controls.Add(ctrl);
			}

			using (var fontsCollection = new InstalledFontCollection())
			{
				DD_Font.Items = fontsCollection.Families;
				DD_Font.Conversion = (x) => (x as FontFamily)?.Name ?? x?.ToString();
			}

			DD_Font.FontDropdown = true;
			DD_Font.SelectedItem = Options.Current.SizeFont;
		}


		protected override void UIChanged()
		{
			base.UIChanged();

			label1.Font = UI.Font(8.25F, FontStyle.Italic);
		}

		private void DD_Font_TextChanged(object sender, EventArgs e)
		{
			if (new Font(DD_Font.Text, 8.25F).FontFamily.Name.Equals(DD_Font.Text, StringComparison.CurrentCultureIgnoreCase))
			{
				Options.Current.SizeFont = DD_Font.Text;
				Options.Save();
			}
		}
	}
}