using System;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class SurpriseAttackPropStats : IInitializable
	{
		public List<ExtraDamageStats> extraMeleeDamages;

		public string Label => "SurpriseAttackPropStats";

		public SurpriseAttackPropStats() { }
		public SurpriseAttackPropStats(SurpriseAttackProps p)
		{
			Util.Populate(out this.extraMeleeDamages, p.extraMeleeDamages, (v) => new ExtraDamageStats(v));
		}

		public bool Initialize()
		{
			if (this.extraMeleeDamages != null)
				this.extraMeleeDamages.ForEach(v => Util.InitializeDefStat(v));
			return true;
		}
	}
}
