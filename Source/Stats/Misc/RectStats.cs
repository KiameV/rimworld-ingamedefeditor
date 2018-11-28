using System;
using UnityEngine;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class RectStats
	{
		public float x, y, width, height;

		public RectStats() { }
		public RectStats(float x, float y, float width, float height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}
		public RectStats(Rect r)
		{
			this.x = r.x;
			this.y = r.y;
			this.width = r.width;
			this.height = r.height;
		}

		public Rect ToRect()
		{
			return new Rect(this.x, this.y, this.width, this.height);
		}
	}
}
