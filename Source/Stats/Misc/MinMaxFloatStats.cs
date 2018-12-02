using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class MinMaxFloatStats
	{
		public float Min = 0, Max = 0;

		public MinMaxFloatStats() { }
		public MinMaxFloatStats(float min, float max)
		{
			this.Min = min;
			this.Max = max;
		}
		public MinMaxFloatStats(FloatRange f)
		{
			this.Min = f.min;
			this.Max = f.max;
		}

		public FloatRange ToFloatRange()
		{
			return new FloatRange(this.Min, this.Max);
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is MinMaxFloatStats s)
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
