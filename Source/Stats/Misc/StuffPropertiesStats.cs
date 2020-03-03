using System;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;
using System.Collections.Generic;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class StuffPropertiesStats
	{
		public string stuffAdjective;
		public float commonality;
		public bool smeltable;
		public bool allowColorGenerators;

		public ColorStats color;

		public DefStat<EffecterDef> constructEffect;
		public DefStat<StuffAppearanceDef> appearance;
		public DefStat<SoundDef> soundImpactStuff;
		public DefStat<SoundDef> soundMeleeHitSharp;
		public DefStat<SoundDef> soundMeleeHitBlunt;

		public List<FloatValueDefStat<StatDef>> statOffsets;
		public List<FloatValueDefStat<StatDef>> statFactors;
		public List<DefStat<StuffCategoryDef>> categories;

		public StuffPropertiesStats() { }
		public StuffPropertiesStats(StuffProperties p)
		{
			this.stuffAdjective = p.stuffAdjective;
			this.commonality = p.commonality;
            // TODO this.smeltable = p.smeltable;
            this.allowColorGenerators = p.allowColorGenerators;

			this.color = new ColorStats(p.color);

			Util.AssignDefStat(p.constructEffect, out this.constructEffect);
			Util.AssignDefStat(p.appearance, out this.appearance);
			Util.AssignDefStat(p.soundImpactStuff, out this.soundImpactStuff);
			Util.AssignDefStat(p.soundMeleeHitSharp, out this.soundMeleeHitSharp);
			Util.AssignDefStat(p.soundMeleeHitBlunt, out this.soundMeleeHitBlunt);
			
			Util.Populate(out this.statOffsets, p.statOffsets, (v) => new FloatValueDefStat<StatDef>(v.stat, v.value));
			Util.Populate(out this.statFactors, p.statFactors, (v) => new FloatValueDefStat<StatDef>(v.stat, v.value));
			this.categories = Util.CreateDefStatList(p.categories);
		}
	}
}
