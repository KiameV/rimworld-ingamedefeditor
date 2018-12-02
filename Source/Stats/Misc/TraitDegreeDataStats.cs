using InGameDefEditor.Stats.DefStat;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class TraitDegreeDataStats
	{
		public string label;
		public int degree;
		public float commonality;
		public float socialFightChanceFactor;
		public float marketValueFactorOffset;
		public float randomDiseaseMtbDays;

		public SimpleCurveStats randomMentalStateMtbDaysMoodCurve;

		public DefStat<ThinkTreeDef> thinkTree;
		public DefStat<MentalStateDef> randomMentalState;

		public List<DefStat<MentalStateDef>> disallowedMentalStates;
		public List<DefStat<InspirationDef>> disallowedInspirations;
		public List<DefStat<MentalBreakDef>> theOnlyAllowedMentalBreaks;

		public List<IntValueDefStat<SkillDef>> skillGains;
		public List<FloatValueDefStat<StatDef>> statOffsets;
		public List<FloatValueDefStat<StatDef>> statFactors;

		//[Unsaved]
		//private TraitMentalStateGiver mentalStateGiverInt;
		//public Type mentalStateGiverClass = typeof(TraitMentalStateGiver);

		public TraitDegreeDataStats() { }
		public TraitDegreeDataStats(TraitDegreeData d)
		{
			this.label = d.label;
			this.degree = d.degree;
			this.commonality = d.commonality;
			this.socialFightChanceFactor = d.socialFightChanceFactor;
			this.marketValueFactorOffset = d.marketValueFactorOffset;
			this.randomDiseaseMtbDays = d.randomDiseaseMtbDays;

			if (d.randomMentalStateMtbDaysMoodCurve != null)
				this.randomMentalStateMtbDaysMoodCurve = new SimpleCurveStats(d.randomMentalStateMtbDaysMoodCurve);

			Util.AssignDefStat(d.thinkTree, out this.thinkTree);
			Util.AssignDefStat(d.randomMentalState, out this.randomMentalState);

			this.disallowedMentalStates = Util.CreateDefStatList(d.disallowedMentalStates);
			this.disallowedInspirations = Util.CreateDefStatList(d.disallowedInspirations);
			this.theOnlyAllowedMentalBreaks = Util.CreateDefStatList(d.theOnlyAllowedMentalBreaks);

			Util.Populate(out this.skillGains, d.skillGains, (def, v) => new IntValueDefStat<SkillDef>(def, v));
			Util.Populate(out this.statOffsets, d.statOffsets, (v) => new FloatValueDefStat<StatDef>(v.stat, v.value));
			Util.Populate(out this.statFactors, d.statFactors, (v) => new FloatValueDefStat<StatDef>(v.stat, v.value));
		}

		public bool Initialize()
		{
			Util.InitializeDefStat(this.thinkTree);
			Util.InitializeDefStat(this.randomMentalState);
			Util.InitializeDefStat(this.disallowedMentalStates);
			Util.InitializeDefStat(this.disallowedInspirations);
			Util.InitializeDefStat(this.theOnlyAllowedMentalBreaks);
			Util.InitializeDefStat(this.skillGains);
			if (this.statOffsets != null)
				this.statOffsets.ForEach((v) => v.Initialize());
			if (this.statFactors != null)
				this.statFactors.ForEach((v) => v.Initialize());
			return true;
		}

		public void ApplyStats(TraitDegreeData to)
		{
			to.label = this.label;
			to.degree = this.degree;
			to.commonality = this.commonality;
			to.socialFightChanceFactor = this.socialFightChanceFactor;
			to.marketValueFactorOffset = this.marketValueFactorOffset;
			to.randomDiseaseMtbDays = this.randomDiseaseMtbDays;

			if (to.randomMentalStateMtbDaysMoodCurve != null && 
				this.randomMentalStateMtbDaysMoodCurve != null)
			{
				this.randomMentalStateMtbDaysMoodCurve.ApplyStats(to.randomMentalStateMtbDaysMoodCurve);
			}

			Util.AssignDef(this.thinkTree, out to.thinkTree);
			Util.AssignDef(this.randomMentalState, out to.randomMentalState);

			Util.Populate(out to.disallowedMentalStates, this.disallowedMentalStates, (v) => v.Def);
			Util.Populate(out to.disallowedInspirations, this.disallowedInspirations, (v) => v.Def);
			Util.Populate(out to.theOnlyAllowedMentalBreaks, this.theOnlyAllowedMentalBreaks, (v) => v.Def);

			to.skillGains?.Clear();
			if (this.skillGains != null)
			{
				if (to.skillGains == null)
					to.skillGains = new Dictionary<SkillDef, int>();
				this.skillGains.ForEach(v => to.skillGains.Add(v.Def, v.value));
			}

			to.statOffsets?.Clear();
			Util.Populate(out to.statOffsets, this.statOffsets, (v) => new StatModifier() { stat = v.Def, value = v.value });

			to.statFactors?.Clear();
			Util.Populate(out to.statFactors, this.statFactors, (v) => new StatModifier() { stat = v.Def, value = v.value });
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is TraitDegreeDataStats d)
			{
				return
					object.Equals(this.label, d.label) &&
					this.degree == d.degree &&
					this.commonality == d.commonality &&
					this.socialFightChanceFactor == d.socialFightChanceFactor &&
					this.marketValueFactorOffset == d.marketValueFactorOffset &&
					this.randomDiseaseMtbDays == d.randomDiseaseMtbDays &&
					object.Equals(this.randomMentalStateMtbDaysMoodCurve, d.randomMentalStateMtbDaysMoodCurve) &&
					object.Equals(this.thinkTree, d.thinkTree) &&
					object.Equals(this.randomMentalState, d.randomMentalState) &&
					Util.AreEqual(this.disallowedMentalStates, d.disallowedMentalStates) &&
					Util.AreEqual(this.disallowedInspirations, d.disallowedInspirations) &&
					Util.AreEqual(this.theOnlyAllowedMentalBreaks, d.theOnlyAllowedMentalBreaks) &&
					Util.AreEqual(this.skillGains, d.skillGains, v => v.GetHashCode()) &&
					Util.AreEqual(this.statOffsets, d.statOffsets) &&
					Util.AreEqual(this.statFactors, d.statFactors);
			}
			return false;
		}

		public override string ToString()
		{
			return this.label + " " + this.degree;
		}

		public override int GetHashCode()
		{
			return base.ToString().GetHashCode();
		}
	}
}
