using System;
using UnityEngine;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class Vector3Stats
	{
		public float x = 0, y = 0, z = 0;

		public Vector3Stats() { }
		public Vector3Stats(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		public Vector3Stats(Vector3 f)
		{
			this.x = f.x;
			this.y = f.y;
			this.z = f.z;
		}

		public Vector3 ToVector3()
		{
			return new Vector3(this.x, this.y, this.z);
		}
	}
}
