using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Stats
{
	[Serializable]
	public class ThoughtDefStats : DefStat<ThoughtDef>, IParentStat
    {
        public int stackLimit;
        public float stackedEffectMultiplier;
        public float durationDays;
        public bool invert;
        public bool validWhileDespawned;
        public int requiredTraitsDegree = -2147483648;
        public bool nullifiedIfNotColonist;
        public bool showBubble;
        public int stackLimitForSameOtherPawn;
        public float lerpOpinionToZeroAfterDurationPct;
        public float maxCumulatedOpinionOffset;

		public DefStat<ThoughtDef> nextThought;
		public DefStat<HediffDef> hediff;
		public DefStat<GameConditionDef> gameCondition;
        public DefStat<StatDef> effectMultiplyingStat;
        public DefStat<ThoughtDef> thoughtToMake;
        public DefStat<TaleDef> taleDef;

        public List<DefStat<TraitDef>> nullifyingTraits = new List<DefStat<TraitDef>>();
        public List<DefStat<TaleDef>> nullifyingOwnTales = new List<DefStat<TaleDef>>();
        public List<DefStat<TraitDef>> requiredTraits = new List<DefStat<TraitDef>>();

        public List<ThoughtStageStats> stages = new List<ThoughtStageStats>();

		public string UniqueKey => base.defName;

		public ThoughtDefStats() : base() { }
        public ThoughtDefStats(ThoughtDef d) : base(d)
        {
            this.stackLimit = d.stackLimit;
            this.stackedEffectMultiplier = d.stackedEffectMultiplier;
            this.durationDays = d.durationDays;
            this.invert = d.invert;
            this.validWhileDespawned = d.validWhileDespawned;
            this.requiredTraitsDegree = d.requiredTraitsDegree;
            this.nullifiedIfNotColonist = d.nullifiedIfNotColonist;
            this.showBubble = d.showBubble;
            this.stackLimitForSameOtherPawn = d.stackLimitForSameOtherPawn;
            this.lerpOpinionToZeroAfterDurationPct = d.lerpOpinionToZeroAfterDurationPct;
            this.maxCumulatedOpinionOffset = d.maxCumulatedOpinionOffset;

			Util.AssignDefStat(d.hediff, out this.hediff);
			Util.AssignDefStat(d.gameCondition, out this.gameCondition);
            Util.AssignDefStat(d.nextThought, out this.nextThought);
            Util.AssignDefStat(d.effectMultiplyingStat, out this.effectMultiplyingStat);
            Util.AssignDefStat(d.thoughtToMake, out this.thoughtToMake);
            Util.AssignDefStat(d.taleDef, out this.taleDef);

            if (d.nullifyingTraits == null)
                d.nullifyingTraits = new List<TraitDef>();
            d.nullifyingTraits.ForEach((TraitDef td) => this.nullifyingTraits.Add(new DefStat<TraitDef>(td)));

            if (d.nullifyingOwnTales == null)
                d.nullifyingOwnTales = new List<TaleDef>();
            d.nullifyingOwnTales.ForEach((TaleDef td) => this.nullifyingOwnTales.Add(new DefStat<TaleDef>(td)));

            if (d.requiredTraits == null)
                d.requiredTraits = new List<TraitDef>();
            d.requiredTraits.ForEach((TraitDef td) => this.requiredTraits.Add(new DefStat<TraitDef>(td)));

            if (d.stages == null)
                d.stages = new List<ThoughtStage>();
            d.stages.ForEach((ThoughtStage ts) => this.stages.Add(new ThoughtStageStats(ts)));
        }

        public void ApplyStats(object def)
        {
            if (def is ThoughtDef to)
            {
                to.stackLimit = this.stackLimit;
                to.stackedEffectMultiplier = this.stackedEffectMultiplier;
                to.durationDays = this.durationDays;
                to.invert = this.invert;
                to.validWhileDespawned = this.validWhileDespawned;
                to.requiredTraitsDegree = this.requiredTraitsDegree;
                to.nullifiedIfNotColonist = this.nullifiedIfNotColonist;
                to.showBubble = this.showBubble;
                to.stackLimitForSameOtherPawn = this.stackLimitForSameOtherPawn;
                to.lerpOpinionToZeroAfterDurationPct = this.lerpOpinionToZeroAfterDurationPct;
                to.maxCumulatedOpinionOffset = this.maxCumulatedOpinionOffset;

				Util.AssignDef(this.hediff, out to.hediff);
				Util.AssignDef(this.gameCondition, out to.gameCondition);
                Util.AssignDef(this.nextThought, out to.nextThought);
                Util.AssignDef(this.effectMultiplyingStat, out to.effectMultiplyingStat);
                Util.AssignDef(this.thoughtToMake, out to.thoughtToMake);
                Util.AssignDef(this.taleDef, out to.taleDef);

                to.nullifyingTraits.Clear();
                this.nullifyingTraits.ForEach((DefStat<TraitDef> s) => to.nullifyingTraits.Add(s.Def));

                to.nullifyingOwnTales.Clear();
                this.nullifyingOwnTales.ForEach((DefStat<TaleDef> s) => to.nullifyingOwnTales.Add(s.Def));

                to.requiredTraits.Clear();
                this.requiredTraits.ForEach((DefStat<TraitDef> s) => to.requiredTraits.Add(s.Def));
                
                this.stages.ForEach((ThoughtStageStats s) => s.ApplyStats(to.stages));
            }
        }

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;
			Util.InitializeDefStat(this.hediff);
			Util.InitializeDefStat(this.gameCondition);
            Util.InitializeDefStat(this.nextThought);
            Util.InitializeDefStat(this.effectMultiplyingStat);
            Util.InitializeDefStat(this.thoughtToMake);
            Util.InitializeDefStat(this.taleDef);

            foreach (var v in this.nullifyingTraits)
                if (!v.Initialize())
                    Log.Warning("Failed to initialize DefStat " + v.defName);

            foreach (var v in this.nullifyingOwnTales)
                if (!v.Initialize())
                    Log.Warning("Failed to initialize DefStat " + v.defName);

            foreach (var v in this.requiredTraits)
                if (!v.Initialize())
                    Log.Warning("Failed to initialize DefStat " + v.defName);
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) &&
                obj is ThoughtDefStats s)
            {
                return
                    this.stackLimit == s.stackLimit &&
                    this.stackedEffectMultiplier == s.stackedEffectMultiplier &&
                    this.durationDays == s.durationDays &&
                    this.invert == s.invert &&
                    this.validWhileDespawned == s.validWhileDespawned &&
                    this.requiredTraitsDegree == s.requiredTraitsDegree &&
                    this.nullifiedIfNotColonist == s.nullifiedIfNotColonist &&
                    this.showBubble == s.showBubble &&
                    this.stackLimitForSameOtherPawn == s.stackLimitForSameOtherPawn &&
                    this.lerpOpinionToZeroAfterDurationPct == s.lerpOpinionToZeroAfterDurationPct &&
                    Util.FloatsRoughlyEqual(this.maxCumulatedOpinionOffset, s.maxCumulatedOpinionOffset) &&
					Util.AreEqual(this.hediff, s.hediff) &&
					Util.AreEqual(this.gameCondition, s.gameCondition) &&
                    Util.AreEqual(this.nextThought, s.nextThought) &&
                    Util.AreEqual(this.effectMultiplyingStat, s.effectMultiplyingStat) &&
                    Util.AreEqual(this.thoughtToMake, s.thoughtToMake) &&
                    Util.AreEqual(this.taleDef, s.taleDef) &&
                    Util.AreEqual(this.nullifyingTraits, s.nullifyingTraits) &&
                    Util.AreEqual(this.nullifyingOwnTales, s.nullifyingOwnTales) &&
                    Util.AreEqual(this.requiredTraits, s.requiredTraits) &&
                    Util.AreEqual(this.stages, s.stages, null);
            }
            return false;
        }
    }
}
