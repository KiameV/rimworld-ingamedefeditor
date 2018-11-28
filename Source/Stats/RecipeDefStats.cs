using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;

namespace InGameDefEditor.Stats
{
	[Serializable]
	public class RecipeDefStats : DefStat<RecipeDef>, IParentStat
	{
		public float workAmount;
		public bool allowMixingIngredients;
		public bool autoStripCorpses;
		public bool productHasIngredientStuff;
		public int targetCountAdjustment;
		public float workSkillLearnFactor;
		public bool hideBodyPartNames;
		public bool isViolation;
		public float surgerySuccessChanceFactor;
		public float deathOnFailedSurgeryChance;
		public bool targetsBodyPart;
		public bool anesthetize;
		public bool dontShowIfAnyIngredientMissing;

		public EffecterDefStat effectWorking;

		public ThingFilterStats fixedIngredientFilter;
		public ThingFilterStats defaultIngredientFilter;

		public DefStat<ResearchProjectDef> researchPrerequisite;
		public DefStat<WorkTypeDef> requiredGiverWorkType;
		public DefStat<ThingDef> unfinishedThingDef;
		public DefStat<SoundDef> soundWorking;
		public DefStat<StatDef> workSpeedStat;
		public DefStat<StatDef> efficiencyStat;
		public DefStat<StatDef> workTableEfficiencyStat;
		public DefStat<StatDef> workTableSpeedStat;
		public DefStat<HediffDef> addsHediff;
		public DefStat<HediffDef> removesHediff;
		public DefStat<SkillDef> workSkill;

        // TODO Needs to be null when empty
        //public List<SpecialProductType> specialProducts;

		public List<DefStat<SpecialThingFilterDef>> forceHiddenSpecialFilters;
		//public List<DefStat<ThingDef>> recipeUsers;
		public List<DefStat<BodyPartDef>> appliedOnFixedBodyParts;

		public List<IntValueDefStat<ThingDef>> products;
		public List<IntValueDefStat<SkillDef>> skillRequirements;
		public List<IngredientCountStats> ingredients;

		//[Unsaved]
		//private List<ThingDef> premultipliedSmallIngredients;
		//public List<string> factionPrerequisiteTags;
		//[MustTranslate]
		//public string successfullyRemovedHediffMessage;
		//private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		public RecipeDefStats() : base() { }
		public RecipeDefStats(RecipeDef def) : base(def)
		{
			this.workAmount = def.workAmount;
			this.allowMixingIngredients = def.allowMixingIngredients;
			this.autoStripCorpses = def.autoStripCorpses;
			this.productHasIngredientStuff = def.productHasIngredientStuff;
			this.targetCountAdjustment = def.targetCountAdjustment;
			this.workSkillLearnFactor = def.workSkillLearnFactor;
			this.hideBodyPartNames = def.hideBodyPartNames;
			this.isViolation = def.isViolation;
			this.surgerySuccessChanceFactor = def.surgerySuccessChanceFactor;
			this.deathOnFailedSurgeryChance = def.deathOnFailedSurgeryChance;
			this.targetsBodyPart = def.targetsBodyPart;
			this.anesthetize = def.anesthetize;
			this.dontShowIfAnyIngredientMissing = def.dontShowIfAnyIngredientMissing;

			if (def.effectWorking != null)
				this.effectWorking = new EffecterDefStat(def.effectWorking);

			this.fixedIngredientFilter = new ThingFilterStats(def.fixedIngredientFilter);
			this.defaultIngredientFilter = new ThingFilterStats(def.defaultIngredientFilter);

			Util.AssignDefStat(def.researchPrerequisite, out this.researchPrerequisite);
			Util.AssignDefStat(def.requiredGiverWorkType, out this.requiredGiverWorkType);
			Util.AssignDefStat(def.unfinishedThingDef, out this.unfinishedThingDef);
			Util.AssignDefStat(def.soundWorking, out this.soundWorking);
			Util.AssignDefStat(def.workSpeedStat, out this.workSpeedStat);
			Util.AssignDefStat(def.efficiencyStat, out this.efficiencyStat);
			Util.AssignDefStat(def.workTableEfficiencyStat, out this.workTableEfficiencyStat);
			Util.AssignDefStat(def.workTableSpeedStat, out this.workTableSpeedStat);
			Util.AssignDefStat(def.addsHediff, out this.addsHediff);
			Util.AssignDefStat(def.removesHediff, out this.removesHediff);
			Util.AssignDefStat(def.workSkill, out this.workSkill);

			/*if (def.specialProducts == null)
				def.specialProducts = new List<SpecialProductType>(0);
			this.specialProducts = Util.CreateList(def.specialProducts);*/

			if (def.forceHiddenSpecialFilters == null)
				def.forceHiddenSpecialFilters = new List<SpecialThingFilterDef>(0);
			this.forceHiddenSpecialFilters = Util.CreateDefStatList(def.forceHiddenSpecialFilters);

			/*if (def.recipeUsers == null)
				def.recipeUsers = new List<ThingDef>(0);
			this.recipeUsers = Util.CreateDefStatList(def.recipeUsers);*/

			if (def.appliedOnFixedBodyParts == null)
				def.appliedOnFixedBodyParts = new List<BodyPartDef>();
			this.appliedOnFixedBodyParts = Util.CreateDefStatList(def.appliedOnFixedBodyParts);

			Util.Populate(out this.products, def.products, (v) => new IntValueDefStat<ThingDef>(v.thingDef, v.count), false);
			Util.Populate(out this.skillRequirements, def.skillRequirements, (v) => new IntValueDefStat<SkillDef>(v.skill, v.minLevel), false);
			Util.Populate(out this.ingredients, def.ingredients, (v) => new IngredientCountStats(v), false);
		}

		internal void PreSave(RecipeDef d)
		{
			d.fixedIngredientFilter.ResolveReferences();
			d.defaultIngredientFilter.ResolveReferences();
			foreach (var v in d.ingredients)
				v.ResolveReferences();
		}

		public void ApplyStats(Def def)
		{
			if (def is RecipeDef d)
			{
				d.workAmount = this.workAmount;
				d.allowMixingIngredients = this.allowMixingIngredients;
				d.autoStripCorpses = this.autoStripCorpses;
				d.productHasIngredientStuff = this.productHasIngredientStuff;
				d.targetCountAdjustment = this.targetCountAdjustment;
				d.workSkillLearnFactor = this.workSkillLearnFactor;
				d.hideBodyPartNames = this.hideBodyPartNames;
				d.isViolation = this.isViolation;
				d.surgerySuccessChanceFactor = this.surgerySuccessChanceFactor;
				d.deathOnFailedSurgeryChance = this.deathOnFailedSurgeryChance;
				d.targetsBodyPart = this.targetsBodyPart;
				d.anesthetize = this.anesthetize;
				d.dontShowIfAnyIngredientMissing = this.dontShowIfAnyIngredientMissing;

				if (this.effectWorking != null)
				{
					d.effectWorking = new EffecterDef();
					this.effectWorking.ApplyStats(d.effectWorking);
				}

				d.fixedIngredientFilter = new ThingFilter();
				this.fixedIngredientFilter.ApplyStats(d.fixedIngredientFilter);
				
				d.fixedIngredientFilter = new ThingFilter();
				this.defaultIngredientFilter.ApplyStats(d.defaultIngredientFilter);

				Util.AssignDef(this.researchPrerequisite, out d.researchPrerequisite);
				Util.AssignDef(this.requiredGiverWorkType, out d.requiredGiverWorkType);
				Util.AssignDef(this.unfinishedThingDef, out d.unfinishedThingDef);
				Util.AssignDef(this.soundWorking, out d.soundWorking);
				Util.AssignDef(this.workSpeedStat, out d.workSpeedStat);
				Util.AssignDef(this.efficiencyStat, out d.efficiencyStat);
				Util.AssignDef(this.workTableEfficiencyStat, out d.workTableEfficiencyStat);
				Util.AssignDef(this.workTableSpeedStat, out d.workTableSpeedStat);
				Util.AssignDef(this.addsHediff, out d.addsHediff);
				Util.AssignDef(this.removesHediff, out d.removesHediff);
				Util.AssignDef(this.workSkill, out d.workSkill);

				//d.specialProducts = Util.CreateList(this.specialProducts);

				/* TODO
				 * if (d.forceHiddenSpecialFilters == null)
					d.forceHiddenSpecialFilters = new List<SpecialThingFilterDef>();
				else
					d.forceHiddenSpecialFilters.Clear();
				foreach (var v in this.forceHiddenSpecialFilters)
				{
					SpecialThingFilterDef
				}*/

				//d.forceHiddenSpecialFilters = Util.CreateDefStatList(this.forceHiddenSpecialFilters);

				/*
				 * TODO Doubt this is needed
				if (this.recipeUsers == null)
				{
					if (d.recipeUsers != null)
						d.recipeUsers.Clear();
				}*/

				d.appliedOnFixedBodyParts = Util.ConvertDefStats(this.appliedOnFixedBodyParts);

				Util.Populate(out d.products, this.products, (v) => new ThingDefCountClass(v.Def, v.value), false);
				Util.Populate(out d.skillRequirements, this.skillRequirements, delegate (IntValueDefStat<SkillDef> v) {
					return new SkillRequirement() {
							skill = v.Def,
							minLevel = v.value }; }, false);

				
				/* TODO Re-add
					if (d.ingredients == null)
					d.ingredients = new List<IngredientCount>();
				d.ingredients.Clear();
				foreach (var v in this.ingredients)
				{
					ThingFilter tf = new ThingFilter();
					v.ThingFilterStats.ApplyStats(tf);
					var i = new IngredientCount()
					{
						filter = tf
					};
					IngredientCountStats.SetIngredientCount(i, v.Count);
					d.ingredients.Add(i);
				}*/
			}
		}

		public override bool Initialize()
		{
			if (!base.Initialize())
				return false;

			Util.InitializeDefStat(researchPrerequisite);
			Util.InitializeDefStat(requiredGiverWorkType);
			Util.InitializeDefStat(unfinishedThingDef);
			Util.InitializeDefStat(soundWorking);
			Util.InitializeDefStat(workSpeedStat);
			Util.InitializeDefStat(efficiencyStat);
			Util.InitializeDefStat(workTableEfficiencyStat);
			Util.InitializeDefStat(workTableSpeedStat);
			Util.InitializeDefStat(addsHediff);
			Util.InitializeDefStat(removesHediff);
			Util.InitializeDefStat(workSkill);

            foreach (var v in this.ingredients)
                v.Initialize();

			Util.InitializeDefStat(forceHiddenSpecialFilters);
			//Util.InitializeDefStat(recipeUsers);
			Util.InitializeDefStat(appliedOnFixedBodyParts);

			Util.InitializeDefStat(products);
			Util.InitializeDefStat(skillRequirements);

			return true;
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj) &&
				obj is RecipeDefStats s)
			{
				//Log.Error("Are Equal:");
				//Log.Error(this.ToString());
				//Log.Error(obj.ToString());
				return
					this.workAmount == s.workAmount &&
					this.allowMixingIngredients == s.allowMixingIngredients &&
					this.autoStripCorpses == s.autoStripCorpses &&
					this.productHasIngredientStuff == s.productHasIngredientStuff &&
					this.targetCountAdjustment == s.targetCountAdjustment &&
					this.workSkillLearnFactor == s.workSkillLearnFactor &&
					this.hideBodyPartNames == s.hideBodyPartNames &&
					this.isViolation == s.isViolation &&
					this.surgerySuccessChanceFactor == s.surgerySuccessChanceFactor &&
					this.deathOnFailedSurgeryChance == s.deathOnFailedSurgeryChance &&
					this.targetsBodyPart == s.targetsBodyPart &&
					this.anesthetize == s.anesthetize &&
					this.dontShowIfAnyIngredientMissing == s.dontShowIfAnyIngredientMissing &&
					object.Equals(this.effectWorking, s.effectWorking) &&
					object.Equals(this.fixedIngredientFilter, s.fixedIngredientFilter) &&
					object.Equals(this.defaultIngredientFilter, s.defaultIngredientFilter) &&
					Util.AreEqual(this.researchPrerequisite, s.researchPrerequisite) &&
					Util.AreEqual(this.requiredGiverWorkType, s.requiredGiverWorkType) &&
					Util.AreEqual(this.unfinishedThingDef, s.unfinishedThingDef) &&
					Util.AreEqual(this.soundWorking, s.soundWorking) &&
					Util.AreEqual(this.workSpeedStat, s.workSpeedStat) &&
					Util.AreEqual(this.efficiencyStat, s.efficiencyStat) &&
					Util.AreEqual(this.workTableEfficiencyStat, s.workTableEfficiencyStat) &&
					Util.AreEqual(this.workTableSpeedStat, s.workTableSpeedStat) &&
					Util.AreEqual(this.addsHediff, s.addsHediff) &&
					Util.AreEqual(this.removesHediff, s.removesHediff) &&
					Util.AreEqual(this.workSkill, s.workSkill) &&
					//Util.AreEqual(this.specialProducts, s.specialProducts) &&
					Util.AreEqual(this.forceHiddenSpecialFilters, s.forceHiddenSpecialFilters) &&
					Util.AreEqual(this.appliedOnFixedBodyParts, s.appliedOnFixedBodyParts) &&
					Util.AreEqual(this.products, s.products) &&
					Util.AreEqual(this.skillRequirements, s.skillRequirements) &&
					Util.AreEqual(this.ingredients, s.ingredients);
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