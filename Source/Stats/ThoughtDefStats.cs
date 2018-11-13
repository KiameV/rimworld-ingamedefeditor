using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats
{
    class ThoughtDefStats : DefStat<ThoughtDef>, IParentStat
    {
        public int stackLimit;
        public float stackedEffectMultiplier;
        public float durationDays;
        public bool invert;
        public bool validWhileDespawned;
        public int requiredTraitsDegree;
        // TODO public HediffDef hediff;
        public bool nullifiedIfNotColonist;
        public bool showBubble;
        public int stackLimitForSameOtherPawn = -1;
        public float lerpOpinionToZeroAfterDurationPct = 0.7f;
        public float maxCumulatedOpinionOffset = 3.40282347E+38f;

        public DefStat<GameConditionDef> gameCondition;
        public DefStat<ThoughtDef> nextThought;
        public DefStat<StatDef> effectMultiplyingStat;
        public DefStat<ThoughtDef> thoughtToMake;
        public DefStat<TaleDef> taleDef;

        public List<DefStat<TraitDef>> nullifyingTraits = new List<DefStat<TraitDef>>();
        public List<DefStat<TaleDef>> nullifyingOwnTales = new List<DefStat<TaleDef>>();
        public List<DefStat<TraitDef>> requiredTraits = new List<DefStat<TraitDef>>();

        public List<ThoughtStageStat> stages = new List<ThoughtStageStat>();

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

            Util.TryAssignStatDef(d.gameCondition, out this.gameCondition);
            Util.TryAssignStatDef(d.nextThought, out this.nextThought);
            Util.TryAssignStatDef(d.effectMultiplyingStat, out this.effectMultiplyingStat);
            Util.TryAssignStatDef(d.thoughtToMake, out this.thoughtToMake);
            Util.TryAssignStatDef(d.taleDef, out this.taleDef);

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
            d.stages.ForEach((ThoughtStage ts) => this.stages.Add(new ThoughtStageStat(ts)));
        }

        public void ApplyStats(Def def)
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

                Util.TryAssignDef(this.gameCondition, out to.gameCondition);
                Util.TryAssignDef(this.nextThought, out to.nextThought);
                Util.TryAssignDef(this.effectMultiplyingStat, out to.effectMultiplyingStat);
                Util.TryAssignDef(this.thoughtToMake, out to.thoughtToMake);
                Util.TryAssignDef(this.taleDef, out to.taleDef);

                to.nullifyingTraits.Clear();
                this.nullifyingTraits.ForEach((DefStat<TraitDef> s) => to.nullifyingTraits.Add(s.Def));

                to.nullifyingOwnTales.Clear();
                this.nullifyingOwnTales.ForEach((DefStat<TaleDef> s) => to.nullifyingOwnTales.Add(s.Def));

                to.requiredTraits.Clear();
                this.requiredTraits.ForEach((DefStat<TraitDef> s) => to.requiredTraits.Add(s.Def));
                
                this.stages.ForEach((ThoughtStageStat s) => s.ApplyStats(to.stages));
            }
        }

        public override bool Initialize()
        {
            if (base.Initialize())
            {
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
            return false;
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
                    this.maxCumulatedOpinionOffset == s.maxCumulatedOpinionOffset &&
                    Util.AreEqual(this.gameCondition, s.gameCondition) &&
                    Util.AreEqual(this.nextThought, s.nextThought) &&
                    Util.AreEqual(this.effectMultiplyingStat, s.effectMultiplyingStat) &&
                    Util.AreEqual(this.thoughtToMake, s.thoughtToMake) &&
                    Util.AreEqual(this.taleDef, s.taleDef) &&
                    Util.AreEqual(this.nullifyingTraits, s.nullifyingTraits) &&
                    Util.AreEqual(this.nullifyingOwnTales, s.nullifyingOwnTales) &&
                    Util.AreEqual(this.requiredTraits, s.requiredTraits) &&
                    Util.AreEqual(this.stages, s.stages, (ThoughtStageStat l, ThoughtStageStat r) => l.Equals(r));
            }
            return false;
        }
    }

    class ThoughtStageStat
    {
        [XmlElement(IsNullable = false)]
        public string label;
        public float baseMoodEffect;
        public float baseOpinionOffset;
        public bool visible = true;

        public bool isNull;

        public ThoughtStageStat(ThoughtStage ts)
        {
            if (ts != null)
            {
                this.label = ts.label;
                this.baseMoodEffect = ts.baseMoodEffect;
                this.baseOpinionOffset = ts.baseOpinionOffset;
                this.visible = ts.visible;
                this.isNull = false;
            }
            else
            {
                this.isNull = true;
            }
        }

        public void ApplyStats(IEnumerable<ThoughtStage> s)
        {
            if (!this.isNull)
            {
                foreach (var v in s)
                {
                    if (v.label.Equals(this.label))
                    {
                        v.label = this.label;
                        v.baseMoodEffect = this.baseMoodEffect;
                        v.baseOpinionOffset = this.baseOpinionOffset;
                        v.visible = this.visible;
                        break;
                    }
                }
            }
        }

        public override string ToString()
        {
            if (this.isNull)
                return typeof(ThoughtDefStats).Name + " - is null";
            return 
                typeof(ThoughtDefStats).Name + Environment.NewLine + 
                "label: " + label + Environment.NewLine +
                "baseMoodEffect: " + baseMoodEffect + Environment.NewLine +
                "baseOpinionOffset: " + baseOpinionOffset + Environment.NewLine +
                "visible: " + visible;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj != null &&
                obj is ThoughtStageStat s)
            {
                return
                    string.Equals(this.label, s.label) &&
                    this.baseMoodEffect == s.baseMoodEffect &&
                    this.baseOpinionOffset == s.baseOpinionOffset &&
                    this.visible == s.visible && 
                    this.isNull == s.isNull;
            }
            return false;
        }
    }
}
