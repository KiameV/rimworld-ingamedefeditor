using System;
using RimWorld;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class HoldOffsetStats
	{
		public Vector3Stats offset;
		public bool flip;
		public bool behind;

		public HoldOffsetStats() { }
		public HoldOffsetStats(HoldOffset s)
		{
			if (s.offset != null)
				this.offset = new Vector3Stats(s.offset);
			this.flip = s.flip;
			this.behind = s.behind;
		}

		public HoldOffset ToHoldOffset()
		{
			var v = new HoldOffset()
			{
				flip = this.flip,
				behind = this.behind,
			};
			if (this.offset != null)
				v.offset = this.offset.ToVector3();
			return v;
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is HoldOffsetStats s)
			{
				return
					object.Equals(this.offset, s.offset) &&
					this.flip == s.flip &&
					this.behind == s.behind;
			}
			return false;
		}

		public override string ToString()
		{
			return
				"HoldOffsetStats" +
				"\noffset: " + offset +
				"\nflip: " + flip +
				"\nbehind: " + behind;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}
