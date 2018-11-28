using System;
using UnityEngine;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class Vector2Stats
	{
		public float x = 0, y = 0, z = 0;

		public Vector2Stats() { }
		public Vector2Stats(float x, float y)
		{
			this.x = x;
			this.y = y;
		}
		public Vector2Stats(Vector2 f)
		{
			this.x = f.x;
			this.y = f.y;
		}

		public Vector2 ToVector2()
		{
			return new Vector2(this.x, this.y);
		}
	}
}
