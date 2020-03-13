using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using System.Reflection;

namespace InGameDefEditor.Stats
{
	[Serializable]
	public class RecipeDefStats : DefStat<RecipeDef>, IParentStat
	{
		//private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

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
		public DefStat<EffecterDef> effectWorking;

		public List<DefStat<SpecialThingFilterDef>> forceHiddenSpecialFilters;
		public List<DefStat<ThingDef>> recipeUsers;
		public List<DefStat<BodyPartDef>> appliedOnFixedBodyParts;

		public List<IntValueDefStat<ThingDef>> products;
		public List<IntValueDefStat<SkillDef>> skillRequirements;
		public List<IngredientCountStats> ingredients;
		
		public List<DefStat<ThingDef>> premultipliedSmallIngredients;
		public List<string> factionPrerequisiteTags;

		// TODO Needs to be null when empty
		public List<SpecialProductType> specialProducts;

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
			Util.AssignDefStat(def.effectWorking, out this.effectWorking);

			Util.Populate(out this.specialProducts, def.specialProducts, true);
			Util.Populate(out this.forceHiddenSpecialFilters, def.forceHiddenSpecialFilters, v => new DefStat<SpecialThingFilterDef>(v));
			Util.Populate(out this.recipeUsers, def.recipeUsers, v => new DefStat<ThingDef>(v));
			Util.Populate(out this.appliedOnFixedBodyParts, def.appliedOnFixedBodyParts, v => new DefStat<BodyPartDef>(v));
			Util.Populate(out this.products, def.products, (v) => new IntValueDefStat<ThingDef>(v.thingDef, v.count), false);
			Util.Populate(out this.skillRequirements, def.skillRequirements, (v) => new IntValueDefStat<SkillDef>(v.skill, v.minLevel), false);
			Util.Populate(out this.ingredients, def.ingredients, (v) => new IngredientCountStats(v), false);
			Util.Populate(out this.premultipliedSmallIngredients, GetPremultipliedSmallIngredients(def), v => new DefStat<ThingDef>(v));
			Util.Populate(out this.factionPrerequisiteTags, def.factionPrerequisiteTags);
	}

		internal void PreSave(RecipeDef d)
		{
			//d.fixedIngredientFilter.ResolveReferences();
			//d.defaultIngredientFilter.ResolveReferences();
			//foreach (var v in d.ingredients)
			//	v.ResolveReferences();
		}

		public void ApplyStats(object def)
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
				
				//d.fixedIngredientFilter = new ThingFilter();
				//this.fixedIngredientFilter.ApplyStats(d.fixedIngredientFilter);

				//d.fixedIngredientFilter = new ThingFilter();
				//this.defaultIngredientFilter.ApplyStats(d.defaultIngredientFilter);

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
				Util.AssignDef(this.effectWorking, out d.effectWorking);

				Util.Populate(out d.specialProducts, this.specialProducts, true);
				Util.Populate(out d.forceHiddenSpecialFilters, this.forceHiddenSpecialFilters, v => v.Def);
				Util.Populate(out d.recipeUsers, this.recipeUsers, v => v.Def);
				Util.Populate(out d.appliedOnFixedBodyParts, this.appliedOnFixedBodyParts, v => v.Def);
				Util.Populate(out d.products, this.products, (v) => new ThingDefCountClass(v.Def, v.value), false);
				Util.Populate(out d.skillRequirements, this.skillRequirements, delegate (IntValueDefStat<SkillDef> v) {
					return new SkillRequirement()
					{
						skill = v.Def,
						minLevel = v.value
					};
				}, false);

				Util.ListIndexAssign(this.ingredients, d.ingredients, (f, t) => IngredientCountStats.SetIngredientCount(t, f.Count));

				Util.Populate(out List<ThingDef> l, this.premultipliedSmallIngredients, v => v.Def);
				SetPremultipliedSmallIngredients(d, l);

				Util.Populate(out d.factionPrerequisiteTags, this.factionPrerequisiteTags);
			}
		}

		public override bool Initialize()
		{
			if (!base.Initialize())
				return false;

			if (!Util.InitializeDefStat(researchPrerequisite))
				Log.Warning($"Failed to initialize researcg prerequisite {researchPrerequisite.defName}");
			if (!Util.InitializeDefStat(requiredGiverWorkType))
				Log.Warning($"Failed to initialize required giver work type {requiredGiverWorkType.defName}");
			if (!Util.InitializeDefStat(unfinishedThingDef))
				Log.Warning($"Failed to initialize unfinished thing def {unfinishedThingDef.defName}");
			if (!Util.InitializeDefStat(soundWorking))
				Log.Warning($"Failed to initialize sound working {soundWorking.defName}");
			if (!Util.InitializeDefStat(workSpeedStat))
				Log.Warning($"Failed to initialize work speed stat {workSpeedStat.defName}");
			if (!Util.InitializeDefStat(efficiencyStat))
				Log.Warning($"Failed to initialize efficiency stat {efficiencyStat.defName}");
			if (!Util.InitializeDefStat(workTableEfficiencyStat))
				Log.Warning($"Failed to initialize work table efficiency stat {workTableEfficiencyStat.defName}"); ;
			if (!Util.InitializeDefStat(workTableSpeedStat))
				Log.Warning($"Failed to initialize work table speed stat {workTableSpeedStat.defName}");
			if (!Util.InitializeDefStat(addsHediff))
				Log.Warning($"Failed to initialize adds hediff {addsHediff.defName}");
			if (!Util.InitializeDefStat(removesHediff))
				Log.Warning($"Failed to initialize removes hediff {removesHediff.defName}");
			if (!Util.InitializeDefStat(workSkill))
				Log.Warning($"Failed to initialize work skill {workSkill.defName}");
			if (!Util.InitializeDefStat(effectWorking))
				Log.Warning($"Failed to initialize effect working {effectWorking.defName}");

			foreach (var v in this.ingredients)
			{
				if (!v.Initialize())
					Log.Warning($"Failed to initialize ingredients portion");
			}

			if (!Util.InitializeDefStat(forceHiddenSpecialFilters))
				Log.Warning($"Failed to initialize force hidden special filters portion");
			if (!Util.InitializeDefStat(recipeUsers))
				Log.Warning($"Failed to initialize recipe users portion");
			if (!Util.InitializeDefStat(appliedOnFixedBodyParts))
				Log.Warning($"Failed to initialize applied on fixed body parts portion");

			if (!Util.InitializeDefStat(products))
				Log.Warning($"Failed to initialize products portion");
			if (!Util.InitializeDefStat(skillRequirements))
				Log.Warning($"Failed to initialize skill requirements portion");
			if (!Util.InitializeDefStat(premultipliedSmallIngredients))
				Log.Warning($"Failed to initialize premultiplied small ingredients portion");

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
					//object.Equals(this.fixedIngredientFilter, s.fixedIngredientFilter) &&
					//object.Equals(this.defaultIngredientFilter, s.defaultIngredientFilter) &&
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
					Util.AreEqual(this.effectWorking, s.effectWorking) &&
					Util.AreEqual(this.specialProducts, s.specialProducts, v => v.ToString().GetHashCode()) &&
					Util.AreEqual(this.forceHiddenSpecialFilters, s.forceHiddenSpecialFilters) &&
					Util.AreEqual(this.appliedOnFixedBodyParts, s.appliedOnFixedBodyParts) &&
					Util.AreEqual(this.products, s.products, v => v.GetHashCode()) &&
					Util.AreEqual(this.skillRequirements, s.skillRequirements, v => v.GetHashCode()) &&
					Util.AreEqual(this.ingredients, s.ingredients, v => v.GetHashCode()) && 
					Util.AreEqual(this.premultipliedSmallIngredients, s.premultipliedSmallIngredients, v => v.Def.GetHashCode()) && 
					Util.AreEqual(this.factionPrerequisiteTags, s.factionPrerequisiteTags, v => v.GetHashCode());
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

		public static List<ThingDef> GetPremultipliedSmallIngredients(RecipeDef d)
		{
			return (List<ThingDef>)typeof(RecipeDef).GetField("premultipliedSmallIngredients", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(d);
		}

		public static void SetPremultipliedSmallIngredients(RecipeDef d, List<ThingDef> v)
		{
			typeof(RecipeDef).GetField("premultipliedSmallIngredients", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(d, v);
		}
	}
}