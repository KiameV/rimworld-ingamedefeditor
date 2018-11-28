using System;
using RimWorld;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class HoldOffsetSetStats
	{
		public HoldOffsetStats northDefault;
		public HoldOffsetStats east;
		public HoldOffsetStats south;
		public HoldOffsetStats west;

		public HoldOffsetSetStats() { }
		public HoldOffsetSetStats(HoldOffsetSet s)
		{
			if (s.northDefault != null)
				this.northDefault = new HoldOffsetStats(s.northDefault);
			if (s.east != null)
				this.east = new HoldOffsetStats(s.east);
			if (s.south != null)
				this.south = new HoldOffsetStats(s.south);
			if (s.west != null)
				this.west = new HoldOffsetStats(s.west);
		}
	}
}
