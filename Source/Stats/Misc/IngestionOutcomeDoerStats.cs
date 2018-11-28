using System;
using RimWorld;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class IngestionOutcomeDoerStats
	{
		public string Name;
		public float chance;
		public bool doToGeneratedPawnIfAddicted;

		public IngestionOutcomeDoerStats() { }
		public IngestionOutcomeDoerStats(IngestionOutcomeDoer d)
		{
			this.Name = d.GetType().Name;
			this.chance = d.chance;
			this.doToGeneratedPawnIfAddicted = d.doToGeneratedPawnIfAddicted;
		}
	}
}
