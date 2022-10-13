using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThumbnailMaker.Controls
{
	public class RoadLaneContainer : Panel
	{
		public string Key { get; }

		public RoadLaneContainer()
		{
			AllowDrop = true;
			MinimumSize = new System.Drawing.Size(0, 22);
		}

		public RoadLaneContainer(string key) : this()
		{
			Key = key;
		}

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			base.OnDragDrop(drgevent);

			RoadLane.HandleDragAction(drgevent, true);
		}

		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			base.OnDragEnter(drgevent);

			RoadLane.HandleDragAction(drgevent, false);
		}
	}
}
