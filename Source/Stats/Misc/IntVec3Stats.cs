using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class IntVec3Stats
	{
		public int x = 0, y = 0, z = 0;

		public IntVec3Stats() { }
		public IntVec3Stats(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		public IntVec3Stats(IntVec3 i)
		{
			this.x = i.x;
			this.y = i.y;
			this.z = i.z;
		}

		public IntVec3 ToIntVec3()
		{
			return new IntVec3(this.x, this.y, this.z);
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is IntVec3Stats s)
			{
				return this.x == s.x && this.y == s.y && this.z == s.z;
			}
			return false;
		}

		public override string ToString()
		{
			return this.GetType().Name + " x: " + x + " y: " + y + " z: " + z;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}
