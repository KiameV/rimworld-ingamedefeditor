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

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is MinMaxIntStats s)
			{
				return this.Min == s.Min && this.Max == s.Max;
			}
			return false;
		}

		public override string ToString()
		{
			return this.GetType().Name + " Min: " + Min + " Max: " + Max;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}
