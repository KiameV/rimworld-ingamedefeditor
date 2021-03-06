﻿using System;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;
using System.Reflection;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class IngestiblePropertiesStats : IInitializable
	{
		public int maxNumToIngestAtOnce;
		public int baseIngestTicks;
		public float chairSearchRadius;
		public bool useEatingSpeedStat;
		//public string ingestCommandString;
		//public string ingestReportString;
		//public string ingestReportStringEat;
		public bool ingestHoldUsesTable;
		public float joy;
		public FoodPreferability preferability;
		public bool nurseable;
		public float optimalityOffsetHumanlikes;
		public float optimalityOffsetFeedingAnimals;
		public DrugCategory drugCategory;

		//public DefStat<ThingDef> parent;
		public DefStat<JoyKindDef> joyKind;
		public DefStat<ThingDef> sourceDef;
		public DefStat<ThoughtDef> tasteThought;
		public DefStat<ThoughtDef> specialThoughtDirect;
		public DefStat<ThoughtDef> specialThoughtAsIngredient;
		public DefStat<EffecterDef> ingestEffect;
		public DefStat<EffecterDef> ingestEffectEat;
		public DefStat<SoundDef> ingestSound;

		public FoodTypeFlags foodType;

		public HoldOffsetSetStats ingestHoldOffsetStanding = null;

		public string Label => "<ingestible prop>";

		//public List<IngestionOutcomeDoerStats> outcomeDoers;

		// Reset to -1 after editing parent
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
			this.foodType = p.foodType;
			this.joy = p.joy;
			this.preferability = p.preferability;
			this.nurseable = p.nurseable;
			this.optimalityOffsetHumanlikes = p.optimalityOffsetHumanlikes;
			this.optimalityOffsetFeedingAnimals = p.optimalityOffsetFeedingAnimals;
			this.drugCategory = p.drugCategory;

			if (p.ingestHoldOffsetStanding != null)
				this.ingestHoldOffsetStanding = new HoldOffsetSetStats(p.ingestHoldOffsetStanding);

			//Util.AssignDefStat(p.parent, out this.parent);
			Util.AssignDefStat(p.joyKind, out this.joyKind);
			Util.AssignDefStat(p.sourceDef, out this.sourceDef);
			Util.AssignDefStat(p.tasteThought, out this.tasteThought);
			Util.AssignDefStat(p.specialThoughtDirect, out this.specialThoughtDirect);
			Util.AssignDefStat(p.specialThoughtAsIngredient, out this.specialThoughtAsIngredient);
			Util.AssignDefStat(p.ingestEffect, out this.ingestEffect);
			Util.AssignDefStat(p.ingestEffectEat, out this.ingestEffectEat);
			Util.AssignDefStat(p.ingestSound, out this.ingestSound);
		}

		public static void ResetCache(IngestibleProperties p)
		{
			typeof(IngestibleProperties).GetField("cachedNutrition", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(p, -1);
		}

		public void ApplyStats(IngestibleProperties to)
		{
			to.maxNumToIngestAtOnce = this.maxNumToIngestAtOnce;
			to.baseIngestTicks = this.baseIngestTicks;
			to.chairSearchRadius = this.chairSearchRadius;
			to.useEatingSpeedStat = this.useEatingSpeedStat;
			to.ingestHoldUsesTable = this.ingestHoldUsesTable;
			to.foodType = this.foodType;
			to.joy = this.joy;
			to.preferability = this.preferability;
			to.nurseable = this.nurseable;
			to.optimalityOffsetHumanlikes = this.optimalityOffsetHumanlikes;
			to.optimalityOffsetFeedingAnimals = this.optimalityOffsetFeedingAnimals;
			to.drugCategory = this.drugCategory;
			if (this.ingestHoldOffsetStanding != null)
			{
				if (to.ingestHoldOffsetStanding == null)
					to.ingestHoldOffsetStanding = new HoldOffsetSet();
				this.ingestHoldOffsetStanding.ApplyStats(to.ingestHoldOffsetStanding);
			}
			//Util.AssignDef(this.parent, out v.parent);
			Util.AssignDef(this.joyKind, out to.joyKind);
			Util.AssignDef(this.sourceDef, out to.sourceDef);
			Util.AssignDef(this.tasteThought, out to.tasteThought);
			Util.AssignDef(this.specialThoughtDirect, out to.specialThoughtDirect);
			Util.AssignDef(this.specialThoughtAsIngredient, out to.specialThoughtAsIngredient);
			Util.AssignDef(this.ingestEffect, out to.ingestEffect);
			Util.AssignDef(this.ingestEffectEat, out to.ingestEffectEat);
			Util.AssignDef(this.ingestSound, out to.ingestSound);
			ResetCache(to);
		}

		public bool Initialize()
		{
			//Util.InitializeDefStat(this.parent);
			Util.InitializeDefStat(this.joyKind);
			Util.InitializeDefStat(this.sourceDef);
			Util.InitializeDefStat(this.tasteThought);
			Util.InitializeDefStat(this.specialThoughtDirect);
			Util.InitializeDefStat(this.specialThoughtAsIngredient);
			Util.InitializeDefStat(this.ingestEffect);
			Util.InitializeDefStat(this.ingestEffectEat);
			Util.InitializeDefStat(this.ingestSound);
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is IngestiblePropertiesStats p)
			{
				return
					this.maxNumToIngestAtOnce == p.maxNumToIngestAtOnce &&
					this.baseIngestTicks == p.baseIngestTicks &&
					this.chairSearchRadius == p.chairSearchRadius &&
					this.useEatingSpeedStat == p.useEatingSpeedStat &&
					//this.ingestCommandString == p.ingestCommandString &&
					//this.ingestReportString == p.ingestReportString &&
					//this.ingestReportStringEat == p.ingestReportStringEat &&
					this.ingestHoldUsesTable == p.ingestHoldUsesTable &&
					this.foodType == p.foodType &&
					this.joy == p.joy &&
					this.preferability == p.preferability &&
					this.nurseable == p.nurseable &&
					this.optimalityOffsetHumanlikes == p.optimalityOffsetHumanlikes &&
					this.optimalityOffsetFeedingAnimals == p.optimalityOffsetFeedingAnimals &&
					this.drugCategory == p.drugCategory &&
					object.Equals(this.ingestHoldOffsetStanding, p.ingestHoldOffsetStanding) &&
					//object.Equals(this.parent, p.parent) &&
					object.Equals(this.joyKind, p.joyKind) &&
					object.Equals(this.sourceDef, p.sourceDef) &&
					object.Equals(this.tasteThought, p.tasteThought) &&
					object.Equals(this.specialThoughtDirect, p.specialThoughtDirect) &&
					object.Equals(this.specialThoughtAsIngredient, p.specialThoughtAsIngredient) &&
					object.Equals(this.ingestEffect, p.ingestEffect) &&
					object.Equals(this.ingestEffectEat, p.ingestEffectEat) &&
					object.Equals(this.ingestSound, p.ingestSound);
			}
			return false;
		}

		public override string ToString()
		{
			return
				"IngestibleProperties" +
				"\nmaxNumToIngestAtOnce: " + this.maxNumToIngestAtOnce +
				"\nbaseIngestTicks: " + this.baseIngestTicks +
				"\nchairSearchRadius: " + this.chairSearchRadius +
				"\nuseEatingSpeedStat: " + this.useEatingSpeedStat +
				"\ningestHoldUsesTable: " + this.ingestHoldUsesTable +
				"\nfoodType: " + this.foodType +
				"\njoy: " + this.joy +
				"\npreferability: " + this.preferability +
				"\nnurseable: " + this.nurseable +
				"\noptimalityOffsetHumanlikes: " + this.optimalityOffsetHumanlikes +
				"\noptimalityOffsetFeedingAnimals: " + this.optimalityOffsetFeedingAnimals +
				"\ndrugCategory: " + this.drugCategory +
				"\ningestHoldOffsetStanding: " + this.ingestHoldOffsetStanding?.ToString() +
				//"\nparent: " + this.parent?.defName +
				"\njoyKind: " + this.joyKind?.defName +
				"\nsourceDef: " + this.sourceDef?.defName +
				"\ntasteThought: " + this.tasteThought?.defName +
				"\nspecialThoughtDirect: " + this.specialThoughtDirect?.defName +
				"\nspecialThoughtAsIngredient: " + this.specialThoughtAsIngredient?.defName +
				"\ningestEffect: " + this.ingestEffect?.defName +
				"\ningestEffectEat: " + this.ingestEffectEat?.defName +
				"\ningestSound: " + this.ingestSound?.defName;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}
