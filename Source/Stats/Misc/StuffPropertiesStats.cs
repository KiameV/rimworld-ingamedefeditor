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
		//public string stuffAdjective;
		public float commonality;
		public bool allowColorGenerators;

		public ColorStats color;

		public DefStat<EffecterDef> constructEffect;
		public DefStat<StuffAppearanceDef> appearance;
		public DefStat<SoundDef> soundImpactStuff;
		public DefStat<SoundDef> soundMeleeHitSharp;
		public DefStat<SoundDef> soundMeleeHitBlunt;

		public List<FloatValueDefStat<StatDef>> statOffsets;
		public List<FloatValueDefStat<StatDef>> statFactors;
		//public List<DefStat<StuffCategoryDef>> categories;

		public StuffPropertiesStats() { }
		public StuffPropertiesStats(StuffProperties p)
		{
			this.commonality = p.commonality;
			this.allowColorGenerators = p.allowColorGenerators;
			this.color = new ColorStats(p.color);

			Util.AssignDefStat(p.constructEffect, out this.constructEffect);
			Util.AssignDefStat(p.appearance, out this.appearance);
			Util.AssignDefStat(p.soundImpactStuff, out this.soundImpactStuff);
			Util.AssignDefStat(p.soundMeleeHitSharp, out this.soundMeleeHitSharp);
			Util.AssignDefStat(p.soundMeleeHitBlunt, out this.soundMeleeHitBlunt);

			Util.Populate(out this.statOffsets, p.statOffsets, (v) => new FloatValueDefStat<StatDef>(v.stat, v.value));
			Util.Populate(out this.statFactors, p.statFactors, (v) => new FloatValueDefStat<StatDef>(v.stat, v.value));
		}

		public bool Initialize()
		{
			Util.InitializeDefStat(this.constructEffect);
			Util.InitializeDefStat(this.appearance);
			Util.InitializeDefStat(this.soundImpactStuff);
			Util.InitializeDefStat(this.soundMeleeHitSharp);
			Util.InitializeDefStat(this.soundMeleeHitBlunt);
			if (this.statOffsets != null)
				this.statOffsets.ForEach((v) => v.Initialize());
			if (this.statFactors != null)
				this.statFactors.ForEach((v) => v.Initialize());
			return true;
		}

		public void ApplyStats(StuffProperties to)
		{
			to.commonality = this.commonality;
			to.allowColorGenerators = this.allowColorGenerators;
			to.color = this.color.ToColor();

			Util.AssignDef(this.constructEffect, out to.constructEffect);
			Util.AssignDef(this.appearance, out to.appearance);
			Util.AssignDef(this.soundImpactStuff, out to.soundImpactStuff);
			Util.AssignDef(this.soundMeleeHitSharp, out to.soundMeleeHitSharp);
			Util.AssignDef(this.soundMeleeHitBlunt, out to.soundMeleeHitBlunt);

			to.statOffsets?.Clear();
			Util.Populate(out to.statOffsets, this.statOffsets, (v) => new StatModifier() { stat = v.Def, value = v.value });

			to.statFactors?.Clear();
			Util.Populate(out to.statFactors, this.statFactors, (v) => new StatModifier() { stat = v.Def, value = v.value });
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is StuffPropertiesStats d)
			{
				return
					this.commonality == d.commonality &&
					this.allowColorGenerators == d.allowColorGenerators &&
					this.color.Equals(d.color) &&
					Util.AreEqual(this.constructEffect, d.constructEffect) &&
					Util.AreEqual(this.appearance, d.appearance) &&
					Util.AreEqual(this.soundImpactStuff, d.soundImpactStuff) &&
					Util.AreEqual(this.soundMeleeHitSharp, d.soundMeleeHitSharp) &&
					Util.AreEqual(this.soundMeleeHitBlunt, d.soundMeleeHitBlunt) &&
					Util.AreEqual(this.statOffsets, d.statOffsets) &&
					Util.AreEqual(this.statFactors, d.statFactors);
			}
			return false;
		}

		public override string ToString()
		{
			return this.commonality + " " + this.allowColorGenerators;
		}

		public override int GetHashCode()
		{
			return base.ToString().GetHashCode();
		}
	}
}