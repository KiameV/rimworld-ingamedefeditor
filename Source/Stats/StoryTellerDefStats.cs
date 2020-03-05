using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Stats
{
	public enum StoryTellerCompPropertyTypes
	{
		CategoryIndividualMTBByBiome,
		CategoryMTB,
		ClassicIntro,
		DeepDrillInfestation,
		Disease,
		FactionInteraction,
		JourneyOffer,
		OnOffCycle,
		RandomMain,
		ShipChunkDrop,
		SingleMTB,
		Triggered,
	}

	public class StoryTellerDefStats : DefStat<StorytellerDef>, IParentStat
	{
		public int listOrder;
		public bool listVisible;
		public bool tutorialMode;
		public bool disableAdaptiveTraining;
		public bool disableAlerts;
		public bool disablePermadeath;
		public float adaptDaysMin;
		public float adaptDaysMax;
		public float adaptDaysGameStartGraceDays;

		public SimpleCurveStats populationIntentFactorFromPopCurve;
		public SimpleCurveStats populationIntentFactorFromPopAdaptDaysCurve;
		public SimpleCurveStats pointsFactorFromDaysPassed;
		public SimpleCurveStats pointsFactorFromAdaptDays;
		public SimpleCurveStats adaptDaysLossFromColonistLostByPostPopulation;
		public SimpleCurveStats adaptDaysLossFromColonistViolentlyDownedByPopulation;
		public SimpleCurveStats adaptDaysGrowthRateCurve;

		public DefStat<DifficultyDef> forcedDifficulty;

		public List<StorytellerCompPropertiesStats> comps = new List<StorytellerCompPropertiesStats>();

		//private string portraitLarge;
		//private string portraitTiny;
		//public Texture2D portraitLargeTex;
		//public Texture2D portraitTinyTex;

		public StoryTellerDefStats() : base() { }
		public StoryTellerDefStats(StorytellerDef d) : base(d)
		{
			this.listOrder = d.listOrder;
			this.listVisible = d.listVisible;
			this.tutorialMode = d.tutorialMode;
			this.disableAdaptiveTraining = d.disableAdaptiveTraining;
			this.disableAlerts = d.disableAlerts;
			this.disablePermadeath = d.disablePermadeath;
			this.adaptDaysMin = d.adaptDaysMin;
			this.adaptDaysMax = d.adaptDaysMax;
			this.adaptDaysGameStartGraceDays = d.adaptDaysGameStartGraceDays;

			this.populationIntentFactorFromPopCurve = Util.Assign(d.populationIntentFactorFromPopCurve, v => new SimpleCurveStats(v));
			this.populationIntentFactorFromPopAdaptDaysCurve = Util.Assign(d.populationIntentFactorFromPopAdaptDaysCurve, v => new SimpleCurveStats(v));
			this.pointsFactorFromDaysPassed = Util.Assign(d.pointsFactorFromDaysPassed, v => new SimpleCurveStats(v));
			this.pointsFactorFromAdaptDays = Util.Assign(d.pointsFactorFromAdaptDays, v => new SimpleCurveStats(v));
			this.adaptDaysLossFromColonistLostByPostPopulation = Util.Assign(d.adaptDaysLossFromColonistLostByPostPopulation, v => new SimpleCurveStats(v));
			this.adaptDaysLossFromColonistViolentlyDownedByPopulation = Util.Assign(d.adaptDaysLossFromColonistViolentlyDownedByPopulation, v => new SimpleCurveStats(v));
			this.adaptDaysGrowthRateCurve = Util.Assign(d.adaptDaysGrowthRateCurve, v => new SimpleCurveStats(v));

			this.forcedDifficulty = Util.AssignDefStat(d.forcedDifficulty);

			Util.Populate(out this.comps, d.comps, v => new StorytellerCompPropertiesStats(v));
		}

		public void ApplyStats(object to)
		{
			if (to is StorytellerDef t)
			{
				t.listOrder = this.listOrder;
				t.listVisible = this.listVisible;
				t.tutorialMode = this.tutorialMode;
				t.disableAdaptiveTraining = this.disableAdaptiveTraining;
				t.disableAlerts = this.disableAlerts;
				t.disablePermadeath = this.disablePermadeath;
				t.adaptDaysMin = this.adaptDaysMin;
				t.adaptDaysMax = this.adaptDaysMax;
				t.adaptDaysGameStartGraceDays = this.adaptDaysGameStartGraceDays;
				
				this.populationIntentFactorFromPopCurve?.ApplyStats(t.populationIntentFactorFromPopCurve);
				this.populationIntentFactorFromPopAdaptDaysCurve?.ApplyStats(t.populationIntentFactorFromPopAdaptDaysCurve);
				this.pointsFactorFromDaysPassed?.ApplyStats(t.pointsFactorFromDaysPassed);
				this.pointsFactorFromAdaptDays?.ApplyStats(t.pointsFactorFromAdaptDays);
				this.adaptDaysLossFromColonistLostByPostPopulation?.ApplyStats(t.adaptDaysLossFromColonistLostByPostPopulation);
				this.adaptDaysLossFromColonistViolentlyDownedByPopulation?.ApplyStats(t.adaptDaysLossFromColonistViolentlyDownedByPopulation);
				this.adaptDaysGrowthRateCurve?.ApplyStats(t.adaptDaysGrowthRateCurve);

				t.forcedDifficulty = Util.AssignDef(this.forcedDifficulty);
				
				if (this.comps != null && this.comps.Count > 0)
				{
					if (t.comps == null)
						t.comps = new List<StorytellerCompProperties>();
					Dictionary<int, StorytellerCompProperties> lookup = new Dictionary<int, StorytellerCompProperties>();
					t.comps.ForEach(v => lookup[StorytellerCompPropertiesStats.GetHashCode(v)] = v);
					t.comps.Clear();

					foreach (var v in this.comps)
					{
						//Log.Warning("Find: " + StorytellerCompPropertiesStats.GetLabel(v));
						if (lookup.TryGetValue(StorytellerCompPropertiesStats.GetHashCode(v), out StorytellerCompProperties p))
						{
							v.ApplyStats(p);
							t.comps.Add(p);
						}
						else
						{
							//Log.Warning("Create new " + StorytellerCompPropertiesStats.GetLabel(v));
							var newProp = v.ToStorytellerCompProperties();
							if (newProp != null)
								t.comps.Add(newProp);
						}
					}
				}
			}
		}

		public override bool Initialize()
		{
			if (!base.Initialize())
				return false;
			
			this.forcedDifficulty?.Initialize();
			this.comps?.ForEach(v => v.Initialize());

			return true;
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj) &&
				obj is StoryTellerDefStats d)
			{
				return
					this.listOrder == d.listOrder &&
					this.listVisible == d.listVisible &&
					this.tutorialMode == d.tutorialMode &&
					this.disableAdaptiveTraining == d.disableAdaptiveTraining &&
					this.disableAlerts == d.disableAlerts &&
					this.disablePermadeath == d.disablePermadeath &&
					this.adaptDaysMin == d.adaptDaysMin &&
					this.adaptDaysMax == d.adaptDaysMax &&
					this.adaptDaysGameStartGraceDays == d.adaptDaysGameStartGraceDays &&
					object.Equals(this.populationIntentFactorFromPopCurve, d.populationIntentFactorFromPopCurve) &&
					object.Equals(this.populationIntentFactorFromPopAdaptDaysCurve, d.populationIntentFactorFromPopAdaptDaysCurve) &&
					object.Equals(this.pointsFactorFromDaysPassed, d.pointsFactorFromDaysPassed) &&
					object.Equals(this.pointsFactorFromAdaptDays, d.pointsFactorFromAdaptDays) &&
					object.Equals(this.adaptDaysLossFromColonistLostByPostPopulation, d.adaptDaysLossFromColonistLostByPostPopulation) &&
					object.Equals(this.adaptDaysLossFromColonistViolentlyDownedByPopulation, d.adaptDaysLossFromColonistViolentlyDownedByPopulation) &&
					object.Equals(this.adaptDaysGrowthRateCurve, d.adaptDaysGrowthRateCurve) &&
					object.Equals(this.forcedDifficulty, d.forcedDifficulty) &&
					Util.AreEqual(this.comps, d.comps, v => v.GetHashCode());
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
	}
}
