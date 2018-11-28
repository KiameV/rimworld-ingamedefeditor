using System;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;
using System.Collections.Generic;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class IngestiblePropertiesStats
	{
		public int maxNumToIngestAtOnce;
		public int baseIngestTicks;
		public float chairSearchRadius;
		public bool useEatingSpeedStat;
		//public string ingestCommandString;
		//public string ingestReportString;
		//public string ingestReportStringEat;
		public bool ingestHoldUsesTable;
		public FoodTypeFlags foodType;
		public float joy;
		public FoodPreferability preferability;
		public bool nurseable;
		public float optimalityOffsetHumanlikes;
		public float optimalityOffsetFeedingAnimals;
		public DrugCategory drugCategory;

		public HoldOffsetSetStats ingestHoldOffsetStanding;

		//public DefStat<ThingDef> parent;
		public DefStat<JoyKindDef> joyKind;
		public DefStat<ThingDef> sourceDef;
		public DefStat<ThoughtDef> tasteThought;
		public DefStat<ThoughtDef> specialThoughtDirect;
		public DefStat<ThoughtDef> specialThoughtAsIngredient;
		public DefStat<EffecterDef> ingestEffect;
		public DefStat<EffecterDef> ingestEffectEat;
		public DefStat<SoundDef> ingestSound;

		public List<IngestionOutcomeDoerStats> outcomeDoers;

		//private float cachedNutrition = -1f;

		public IngestiblePropertiesStats() { }
		public IngestiblePropertiesStats(IngestibleProperties p)
		{
			this.maxNumToIngestAtOnce = p.maxNumToIngestAtOnce;
			this.baseIngestTicks = p.baseIngestTicks;
			this.chairSearchRadius = p.chairSearchRadius;
			this.useEatingSpeedStat = p.useEatingSpeedStat;
			//this.ingestCommandString = p.ingestCommandString;
			//this.ingestReportString = p.ingestReportString;
			//this.ingestReportStringEat = p.ingestReportStringEat;
			this.ingestHoldUsesTable = p.ingestHoldUsesTable;
			this.ingestHoldUsesTable = p.ingestHoldUsesTable;
			this.foodType = p.foodType;
			this.joy = p.joy;
			this.preferability = p.preferability;
			this.nurseable = p.nurseable;
			this.optimalityOffsetHumanlikes = p.optimalityOffsetHumanlikes;
			this.optimalityOffsetFeedingAnimals = p.optimalityOffsetFeedingAnimals;
			this.drugCategory = p.drugCategory;

			if (p.ingestHoldOffsetStanding != null)
				this.ingestHoldOffsetStanding = new HoldOffsetSetStats(p.ingestHoldOffsetStanding);
			
			Util.AssignDefStat(p.joyKind, out this.joyKind);
			Util.AssignDefStat(p.sourceDef, out this.sourceDef);
			Util.AssignDefStat(p.tasteThought, out this.tasteThought);
			Util.AssignDefStat(p.specialThoughtDirect, out this.specialThoughtDirect);
			Util.AssignDefStat(p.specialThoughtAsIngredient, out this.specialThoughtAsIngredient);
			Util.AssignDefStat(p.ingestEffect, out this.ingestEffect);
			Util.AssignDefStat(p.ingestEffectEat, out this.ingestEffectEat);
			Util.AssignDefStat(p.ingestSound, out this.ingestSound);

			Util.Populate(out this.outcomeDoers, p.outcomeDoers, (v) => new IngestionOutcomeDoerStats(v));
		}
	}
}
