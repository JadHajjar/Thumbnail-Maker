using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbnailMaker.Domain
{
	public class StyleIdentityAttribute : Attribute
	{
		public StyleIdentityAttribute(int id, string name, int r, int g, int b)
		{
			Id = id;
			Name = name;
			DefaultColor = Color.FromArgb(r, g, b);
		}

		public StyleIdentityAttribute(int id, int r, int g, int b) : this(id, "", r, g, b)
		{ }

		public int Id { get; }
		public string Name { get; }
		public Color DefaultColor { get; }
	}
}
