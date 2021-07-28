using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace InGameDefEditor.Stats
{
	[Serializable]
	public class HediffDefStats : DefStat<HediffDef>, IParentStat
	{
		public bool isBad;
		public DefStat<ThingDef> spawnThingOnRemoved = null;
		public float chanceToCauseNoPain;
		public bool makesSickThought;
		public bool makesAlert;
		public DefStat<NeedDef> causesNeed = null;
		public List<DefStat<NeedDef>> disablesNeeds = null;
		public float minSeverity;
		public float maxSeverity;
		public bool scenarioCanAdd;
		//public List<HediffGiver> hediffGivers;
		public bool cureAllAtOnceIfCuredByItem;
		public DefStat<TaleDef> taleOnVisible = null;
		public bool everCurableByItem;
		public string battleStateLabel;
		//public List<string> tags;
		public bool priceImpact;
		public bool chronic;
		//public SimpleCurveStats removeOnRedressChanceByDaysCurve;
		public bool displayWound;
		public ColorStats defaultLabelColor = null;
		public InjuryPropsStats injuryProps = null;
		public AddedBodyPartPropsStats addedPartProps = null;

		public HediffDefStats() { }
		public HediffDefStats(HediffDef d) : base(d)
		{
			this.isBad = d.isBad;
			this.chanceToCauseNoPain = d.chanceToCauseNoPain;
			this.makesSickThought = d.makesSickThought;
			this.makesAlert = d.makesAlert;
			this.minSeverity = d.minSeverity;
			this.maxSeverity = d.maxSeverity;
			this.cureAllAtOnceIfCuredByItem = d.cureAllAtOnceIfCuredByItem;
			this.everCurableByItem = d.everCurableByItem;
			this.battleStateLabel = d.battleStateLabel;
			this.priceImpact = d.priceImpact;
			this.chronic = d.chronic;
			//this.removeOnRedressChanceByDaysCurve = new SimpleCurveStats(d.removeOnRedressChanceByDaysCurve);
			this.displayWound = d.displayWound;
			if (d.defaultLabelColor != null)
				this.defaultLabelColor = new ColorStats(d.defaultLabelColor);
			if (d.injuryProps != null)
				this.injuryProps = new InjuryPropsStats(d.injuryProps);
			if (d.addedPartProps != null)
				this.addedPartProps = new AddedBodyPartPropsStats(d.addedPartProps);

			Util.AssignDefStat(d.spawnThingOnRemoved, out this.spawnThingOnRemoved);
			Util.AssignDefStat(d.causesNeed, out this.causesNeed);
			if (d.disablesNeeds != null)
			{
				this.disablesNeeds = new List<DefStat<NeedDef>>(d.disablesNeeds.Count);
				Util.ListIndexAssign(d.disablesNeeds, this.disablesNeeds, (f, t) => Util.AssignDefStat(f, out t));
			}
			Util.AssignDefStat(d.taleOnVisible, out this.taleOnVisible);
		}

		public override bool Initialize()
		{
			if (!base.Initialize())
				return false;

			this.spawnThingOnRemoved?.Initialize();
			this.causesNeed?.Initialize();
			if (this.disablesNeeds?.Count > 0)
				foreach(var dn in this.disablesNeeds)
					dn?.Initialize();
			this.taleOnVisible?.Initialize();
			return true;
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj) &&
				obj is HediffDefStats d)
			{
				return
					this.isBad == d.isBad &&
					this.chanceToCauseNoPain == d.chanceToCauseNoPain &&
					this.makesSickThought == d.makesSickThought &&
					this.makesAlert == d.makesAlert &&
					this.minSeverity == d.minSeverity &&
					this.maxSeverity == d.maxSeverity &&
					this.cureAllAtOnceIfCuredByItem == d.cureAllAtOnceIfCuredByItem &&
					this.everCurableByItem == d.everCurableByItem &&
					this.battleStateLabel == d.battleStateLabel &&
					this.priceImpact == d.priceImpact &&
					this.chronic == d.chronic &&
					//this.removeOnRedressChanceByDaysCurve.Equals(d.removeOnRedressChanceByDaysCurve) &&
					this.displayWound == d.displayWound &&
					this.defaultLabelColor.Equals(d.defaultLabelColor) &&
					this.injuryProps.Equals(d.injuryProps) &&
					this.addedPartProps.Equals(d.addedPartProps) &&
					this.spawnThingOnRemoved.Equals(d.spawnThingOnRemoved) &&
					this.causesNeed.Equals(d.causesNeed) &&
					Util.AreEqual(this.disablesNeeds, d.disablesNeeds) &&
					this.taleOnVisible.Equals(d.taleOnVisible);
			}
			return false;
		}

		public void ApplyStats(object to)
		{
#if DEBUG_HEDIFFDEF
            Log.Warning("ApplyStats for " + this.defName);
#endif
			if (to is HediffDef d)
			{
				d.isBad = this.isBad;
				d.chanceToCauseNoPain = this.chanceToCauseNoPain;
				d.makesSickThought = this.makesSickThought;
				d.makesAlert = this.makesAlert;
				d.minSeverity = this.minSeverity;
				d.maxSeverity = this.maxSeverity;
				d.cureAllAtOnceIfCuredByItem = this.cureAllAtOnceIfCuredByItem;
				d.everCurableByItem = this.everCurableByItem;
				d.battleStateLabel = this.battleStateLabel;
				d.priceImpact = this.priceImpact;
				d.chronic = this.chronic;
				//this.removeOnRedressChanceByDaysCurve?.ApplyStats(d.removeOnRedressChanceByDaysCurve);
				d.displayWound = this.displayWound;
				d.defaultLabelColor = this.defaultLabelColor?.ToColor() ?? Color.white;
				d.injuryProps = this.injuryProps?.ToInjuryProps() ?? null;
				d.addedPartProps = this.addedPartProps.ToAddedBodyPartProps() ?? null;

				d.spawnThingOnRemoved = this.spawnThingOnRemoved?.Def ?? null;
				d.causesNeed = this.causesNeed?.Def ?? null;
				Util.Populate(out d.disablesNeeds, this.disablesNeeds, (f) => f.Def, true);
				d.taleOnVisible = this.taleOnVisible.Def ?? null;
#if DEBUG_HEDIFFDEF
            Log.Warning("ApplyStats Done");
#endif
			}
			else
				Log.Error("ThingDefStat passed none ThingDef!");
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}