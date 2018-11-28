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
	}
}
