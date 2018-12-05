using System;
using System.Text;
using UnityEngine;
using Verse;

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

		public override bool Equals(object obj)
		{
			if (obj != null && 
				obj is ColorStats s)
			{
				return
					this.r == s.r &&
					this.g == s.g &&
					this.b == s.b &&
					this.a == s.a;
			}
			return false;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(r.ToString());
			sb.Append(",");
			sb.Append(g.ToString());
			sb.Append(",");
			sb.Append(b.ToString());
			sb.Append(",");
			sb.Append(a.ToString());
			return sb.ToString();
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}
