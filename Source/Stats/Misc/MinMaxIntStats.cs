using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class MinMaxIntStats
	{
		public int Min = 0, Max = 0;

		public MinMaxIntStats() { }
		public MinMaxIntStats(int min, int max)
		{
			this.Min = min;
			this.Max = max;
		}
		public MinMaxIntStats(IntRange i)
		{
			this.Min = i.min;
			this.Max = i.max;
		}

		public IntRange ToIntRange()
		{
			return new IntRange(this.Min, this.Max);
		}
	}
}
