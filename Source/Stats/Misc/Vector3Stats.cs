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

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is Vector3Stats s)
			{
				return this.x == s.x && this.y == s.y && this.z == s.z;
			}
			return false;
		}

		public override string ToString()
		{
			return x + " " + y + " " + z;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}
