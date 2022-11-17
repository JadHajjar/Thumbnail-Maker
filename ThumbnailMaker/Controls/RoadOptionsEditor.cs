﻿using Extensions;

using SlickControls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ThumbnailMaker.Domain;
using ThumbnailMaker.Handlers;

namespace ThumbnailMaker.Controls
{
	public partial class RoadOptionsEditor : BaseForm
	{
		private readonly RoadLane _roadLane;

		public RoadOptionsEditor(RoadLane roadLane)
		{
			InitializeComponent();

			var lane = roadLane.GetLane(false);
			var width = LaneInfo.GetLaneTypes(lane.Type).Max(x => Utilities.GetLaneWidth(x, lane));

			TB_LaneWidth.Placeholder = $"Calculated width is  {width}";

			_roadLane = roadLane;
			if (_roadLane.CustomLaneWidth != -1)
				TB_LaneWidth.Text = _roadLane.CustomLaneWidth.ToString();
			if (_roadLane.CustomVerticalOffset != -1)
				TB_Vertical.Text = _roadLane.CustomVerticalOffset.ToString();

			Show(roadLane.FindForm());
		}

		private void B_Apply_Click(object sender, EventArgs e)
		{
			_roadLane.CustomLaneWidth = TB_LaneWidth.Text.SmartParseF(-1);
			_roadLane.CustomVerticalOffset = TB_Vertical.Text.SmartParseF(-1);

			Close();
		}

		private void RoadOptionsEditor_Deactivate(object sender, EventArgs e)
		{
			Close();
		}
	}
}