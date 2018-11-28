using System;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class LifeStageAgeStats
	{
		public float minAge;

		public DefStat<LifeStageDef> def;
		public DefStat<SoundDef> soundCall;
		public DefStat<SoundDef> soundAngry;
		public DefStat<SoundDef> soundWounded;
		public DefStat<SoundDef> soundDeath;

		public LifeStageAgeStats() { }
		public LifeStageAgeStats(LifeStageAge a)
		{
			Util.AssignDefStat(a.def, out this.def);
			Util.AssignDefStat(a.soundCall, out this.soundCall);
			Util.AssignDefStat(a.soundAngry, out this.soundAngry);
			Util.AssignDefStat(a.soundWounded, out this.soundWounded);
			Util.AssignDefStat(a.soundDeath, out this.soundDeath);
		}
	}
}
