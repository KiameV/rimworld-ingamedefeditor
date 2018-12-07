using RimWorld;
using System;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class QualityRangeStats
	{
		public QualityCategory Min;
		public QualityCategory Max;

		public QualityRangeStats() { }
		public QualityRangeStats(QualityRange qr)
		{
			this.Min = qr.min;
			this.Max = qr.max;
		}

		public QualityRange ToQualityRange()
		{
			return new QualityRange(this.Min, this.Max);
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is QualityRangeStats s)
			{
				return this.Min == s.Min && this.Max == s.Max;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override string ToString()
		{
			return
				this.GetType().Name + Environment.NewLine +
				"    Min: " + this.Min + Environment.NewLine +
				"    Max: " + this.Max;
		}
	}
}
