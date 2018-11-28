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
	}
}
