using System;
using UnityEngine;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class ColorStats
	{
		public float r, g, b, a;
		public ColorStats() { }
		public ColorStats(float r, float g, float b, float a = 1f)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
		public ColorStats(Color c)
		{
			this.r = c.r;
			this.g = c.g;
			this.b = c.b;
			this.a = c.a;
		}

		public Color ToColor()
		{
			return new Color(this.r, this.g, this.b, this.a);
		}
	}
}
