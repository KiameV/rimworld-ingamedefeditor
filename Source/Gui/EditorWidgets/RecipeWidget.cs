using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using System;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets
{
	class RecipeWidget : AParentDefStatWidget<RecipeDef>
	{
		private readonly List<IInputWidget> inputWidgets;

		private ThingFilterWidget fixedIngredientFilter;
		private ThingFilterWidget defaultIngredientFilter;

		private readonly PlusMinusArgs<SpecialThingFilterDef> forceHiddenSpecialFilters;
		private readonly PlusMinusArgs<ThingDef> recipeUsers;
		private readonly PlusMinusArgs<BodyPartDef> appliedOnFixedBodyParts;
		// TODO Needs to be null when empty
		private readonly PlusMinusArgs<SpecialProductType> specialProducts;

		private readonly List<IntInputWidget<ThingDefCountClass>> products = new List<IntInputWidget<ThingDefCountClass>>();
		//private readonly PlusMinusArgs<ThingDef> productsPlusMinusArgs;

		private readonly List<IntInputWidget<SkillRequirement>> skillRequirements = new List<IntInputWidget<SkillRequirement>>();
		private readonly PlusMinusArgs<SkillDef> skillRequirementsPlusMinusArgs;

		// TODO Read private readonly List<IngredientCountWidget> ingredients = new List<IngredientCountWidget>();
		private readonly List<IngredientCountWidget> ingredients = new List<IngredientCountWidget>();

		public RecipeWidget(RecipeDef d, DefType type) : base(d, type)
		{
			if (base.Def.skillRequirements == null)
				base.Def.skillRequirements = new List<SkillRequirement>();
			if (base.Def.forceHiddenSpecialFilters == null)
				base.Def.forceHiddenSpecialFilters = new List<SpecialThingFilterDef>();
			if (base.Def.recipeUsers == null)
				base.Def.recipeUsers = new List<ThingDef>();
			if (base.Def.appliedOnFixedBodyParts == null)
				base.Def.appliedOnFixedBodyParts = new List<BodyPartDef>();

			this.inputWidgets = new List<IInputWidget>()
			{
				new FloatInputWidget<RecipeDef>(base.Def, "Work Amount", (RecipeDef def) => d.workAmount, (RecipeDef def, float f) => d.workAmount = f),
				new BoolInputWidget<RecipeDef>(base.Def, "Allow Mixing Ingredients", (RecipeDef def) => d.allowMixingIngredients, (RecipeDef def, bool b) => d.allowMixingIngredients = b),
				new BoolInputWidget<RecipeDef>(base.Def, "Auto Strip Corpses", (RecipeDef def) => d.autoStripCorpses, (RecipeDef def, bool b) => d.autoStripCorpses = b),
				new BoolInputWidget<RecipeDef>(base.Def, "Product Has Ingredient Stuff", (RecipeDef def) => d.productHasIngredientStuff, (RecipeDef def, bool b) => d.productHasIngredientStuff = b),
				new IntInputWidget<RecipeDef>(base.Def, "Target Count Adjustment", (RecipeDef def) => d.targetCountAdjustment, (RecipeDef def, int i) => d.targetCountAdjustment = i),
				new FloatInputWidget<RecipeDef>(base.Def, "Work Skill Learn Factor", (RecipeDef def) => d.workSkillLearnFactor, (RecipeDef def, float f) => d.workSkillLearnFactor = f),
				new BoolInputWidget<RecipeDef>(base.Def, "Hide Body Part Names", (RecipeDef def) => d.hideBodyPartNames, (RecipeDef def, bool b) => d.hideBodyPartNames = b),
				new BoolInputWidget<RecipeDef>(base.Def, "Is Violation", (RecipeDef def) => d.isViolation, (RecipeDef def, bool b) => d.isViolation = b),
				new FloatInputWidget<RecipeDef>(base.Def, "Surgery Success Chance Factor", (RecipeDef def) => d.surgerySuccessChanceFactor, (RecipeDef def, float f) => d.surgerySuccessChanceFactor = f),
				new FloatInputWidget<RecipeDef>(base.Def, "Death On Failed Surgery Chance", (RecipeDef def) => d.deathOnFailedSurgeryChance, (RecipeDef def, float f) => d.deathOnFailedSurgeryChance = f),
				new BoolInputWidget<RecipeDef>(base.Def, "Targets Body Part", (RecipeDef def) => d.targetsBodyPart, (RecipeDef def, bool b) => d.targetsBodyPart = b),
				new BoolInputWidget<RecipeDef>(base.Def, "Anesthetize", (RecipeDef def) => d.anesthetize, (RecipeDef def, bool b) => d.anesthetize = b),
				new BoolInputWidget<RecipeDef>(base.Def, "Dont Show If Any Ingredient Missing", (RecipeDef def) => d.dontShowIfAnyIngredientMissing, (RecipeDef def, bool b) => d.dontShowIfAnyIngredientMissing = b),
				new DefInputWidget<RecipeDef, ResearchProjectDef>(base.Def, "Research Prerequisite", 200, def => def.researchPrerequisite, (def, v) => def.researchPrerequisite = v, true),
				new DefInputWidget<RecipeDef, WorkTypeDef>(base.Def, "Required Giver Work Type", 200, def => def.requiredGiverWorkType, (def, v) => def.requiredGiverWorkType = v, true),
				new DefInputWidget<RecipeDef, ThingDef>(base.Def, "Unfinished Thing Def", 200, def => def.unfinishedThingDef, (def, v) => def.unfinishedThingDef = v, true),
				new DefInputWidget<RecipeDef, SoundDef>(base.Def, "Sound Working", 200, def => def.soundWorking, (def, v) => def.soundWorking = v, true),
				new DefInputWidget<RecipeDef, StatDef>(base.Def, "Work Speed Stat", 200, def => def.workSpeedStat, (def, v) => def.workSpeedStat = v, true),
				new DefInputWidget<RecipeDef, StatDef>(base.Def, "Efficiency Stat", 200, def => def.efficiencyStat, (def, v) => def.efficiencyStat = v, true),
				new DefInputWidget<RecipeDef, StatDef>(base.Def, "Work Table Efficiency Stat", 200, def => def.workTableEfficiencyStat, (def, v) => def.workTableEfficiencyStat = v, true),
				new DefInputWidget<RecipeDef, StatDef>(base.Def, "Work Table Speed Stat", 200, def => def.workTableSpeedStat, (def, v) => def.workTableSpeedStat = v, true),
				//new DefInputWidget<RecipeDef, HediffDef>(base.Def, "Adds Hediff", 200, def => def.addsHediff, (def, v) => def.addsHediff = v, true),
				//new DefInputWidget<RecipeDef, HediffDef>(base.Def, "Removes Hediff", 200, def => def.removesHediff, (def, v) => def.removesHediff = v, true),
				new DefInputWidget<RecipeDef, SkillDef>(base.Def, "Work Skill", 200, def => def.workSkill, (def, v) => def.workSkill = v, true),
				new DefInputWidget<RecipeDef, EffecterDef>(base.Def, "Effect Working", 200, def => def.effectWorking, (def, v) => def.effectWorking = v, true),
			};

			this.specialProducts = new PlusMinusArgs<SpecialProductType>()
			{
				allItems = Enum.GetValues(typeof(SpecialProductType)).OfType<SpecialProductType>().ToList(),
				beingUsed = () => base.Def.specialProducts,
				onAdd = (spt) => base.Def.specialProducts = Util.AddTo(base.Def.specialProducts, spt),
				onRemove = (spt) => base.Def.specialProducts = Util.RemoveFrom(base.Def.specialProducts, spt, true),
				getDisplayName = (spt) => spt.ToString()
			};

			this.forceHiddenSpecialFilters = new PlusMinusArgs<SpecialThingFilterDef>()
			{
				allItems = DefDatabase<SpecialThingFilterDef>.AllDefs,
				beingUsed = () => base.Def.forceHiddenSpecialFilters,
				onAdd = (def) => base.Def.forceHiddenSpecialFilters = Util.AddTo(base.Def.forceHiddenSpecialFilters, def),
				onRemove = (def) => base.Def.forceHiddenSpecialFilters = Util.RemoveFrom(base.Def.forceHiddenSpecialFilters, def, false),
				getDisplayName = (def) => def.label
			};

			this.recipeUsers = new PlusMinusArgs<ThingDef>()
			{
				allItems = DefDatabase<ThingDef>.AllDefs,
				beingUsed = () => base.Def.recipeUsers,
				onAdd = (def) => base.Def.recipeUsers.Add(def),
				onRemove = (def) => base.Def.recipeUsers.Remove(def),
				getDisplayName = (def) => def.label
			};

			this.appliedOnFixedBodyParts = new PlusMinusArgs<BodyPartDef>()
			{
				allItems = DefDatabase<BodyPartDef>.AllDefs,
				beingUsed = () => base.Def.appliedOnFixedBodyParts,
				onAdd = (def) => base.Def.appliedOnFixedBodyParts = Util.AddTo(base.Def.appliedOnFixedBodyParts, def),
				onRemove = (def) => base.Def.appliedOnFixedBodyParts = Util.RemoveFrom(base.Def.appliedOnFixedBodyParts, def, false),
				getDisplayName = (def) => def.label
			};

			this.skillRequirementsPlusMinusArgs = new PlusMinusArgs<SkillDef>()
			{
				allItems = DefDatabase<SkillDef>.AllDefs,
				isBeingUsed = delegate (SkillDef sd)
				{
					foreach (var v in base.Def.skillRequirements)
						if (v.skill == sd)
							return true;
					return false;
				},
				onAdd = delegate (SkillDef sd)
				{
					SkillRequirement sr = new SkillRequirement() { skill = sd, minLevel = 0 };
					base.Def.skillRequirements = Util.AddTo(base.Def.skillRequirements, sr);
					this.skillRequirements.Add(this.CreateSkillRequirements(sr));
				},
				onRemove = delegate (SkillDef sd)
				{
					base.Def.skillRequirements.RemoveAll((sr) => sr.skill == sd);
					this.skillRequirements.RemoveAll((sr) => sr.Parent.skill == sd);
				},
				getDisplayName = (en) => en.ToString()
			};


			/*this.productsPlusMinusArgs = new PlusMinusArgs<ThingDef>()
			{
				allItems = DefDatabase<ThingDef>.AllDefs,
				isBeingUsed = delegate (ThingDef td)
				{
					foreach (var v in base.Def.products)
						if (v.thingDef == td)
							return true;
					return false;
				},
				onAdd = delegate (ThingDef td)
				{
					ThingDefCountClass tdc = new ThingDefCountClass(td, 0);
					base.Def.products = Util.AddAndCreateIfNeeded(base.Def.products, tdc);
					this.products.Add(this.CreateThingDefCountClass(tdc));
				},
				onRemove = delegate (ThingDef def)
				{
					base.Def.products.RemoveAll((tdc) => tdc.thingDef == def);
					base.Def.products = Util.NullIfNeeded(base.Def.products);
					this.products.RemoveAll((input) => input.Parent.thingDef == def);
				},
				getDisplayName = (en) => en.ToString()
			};*/

			this.Rebuild();
        }

		public override void DrawLeft(float x, ref float y, float width)
		{
			base.DrawLeft(x, ref y, width);

			foreach (var v in inputWidgets)
				v.Draw(x, ref y, width);
		}

        public override void DrawMiddle(float x, ref float y, float width)
		{
			if (this.ingredients.Count > 0)
			{
				WindowUtil.DrawLabel(x, ref y, width, "Ingredients", 30, true);
				foreach (var v in this.ingredients)
					v.Draw(x + 10, ref y, width);
			}
			y += 8;

			if (this.products.Count > 0)
			{
				WindowUtil.DrawLabel(x, ref y, 150, "Products", 30, true);
				foreach (var v in this.products)
					v.Draw(x + 10, ref y, width);
			}
			y += 8;

			WindowUtil.PlusMinusLabel(x, ref y, width, "Skill Requirements", this.skillRequirementsPlusMinusArgs);
			foreach (var v in this.skillRequirements)
				v.Draw(x + 10, ref y, width);
			y += 8;

			WindowUtil.PlusMinusLabel(x, ref y, width, "Force Hidden Special Filters", this.forceHiddenSpecialFilters);
			if (base.Def.forceHiddenSpecialFilters != null)
				foreach (var v in base.Def.forceHiddenSpecialFilters)
				{
					WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v);
					y += 32;
				}
			y += 8;

			WindowUtil.PlusMinusLabel(x, ref y, width, "Applied On Fixed Body Parts", this.appliedOnFixedBodyParts);
			if (base.Def.appliedOnFixedBodyParts != null)
				foreach (var v in base.Def.appliedOnFixedBodyParts)
				{
					WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v);
					y += 32;
				}
			y += 8;

			//foreach(var v in this.)
			/*WindowUtil.PlusMinusLabel(x, ref y, 150, "Ingredients",
				delegate ()
				{
					IngredientCount c = new IngredientCount() { filter = new ThingFilter() };
					IngredientCountStats.SetIngredientCount(c, 0);
					base.Def.ingredients.Add(c);
					this.ingredients.Add(new IngredientCountWidget(c));
				},
				delegate ()
				{
					List<string> items = new List<string>(this.ingredients.Count);
					foreach (var v in this.ingredients)
						items.Add(v.DisplayLabel);
					WindowUtil.DrawFloatingOptions(new FloatOptionsArgs<string>()
					{
						items = items,
						getDisplayName = (s) => s,
						onSelect = (s) => this.ingredients.RemoveAll((i) => i.DisplayLabel.Equals(s)),
					});
				});
			foreach (var v in this.ingredients)
			{
				WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v.DisplayLabel, true);
				y += 32;
				v.Draw(x + 20, ref y, width - 20);
			}
			y += 8;*/

			/*WindowUtil.PlusMinusLabel(x, ref y, 150, "Special Products", this.specialProducts);
            if (base.Def.specialProducts != null)
            {
                foreach (var v in base.Def.specialProducts)
                {
                    WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v);
                    y += 32;
                }
                y += 8;
            }*/

			/*WindowUtil.PlusMinusLabel(x, ref y, 150, "Recipe Users", this.recipeUsers);
			foreach (var v in base.Def.recipeUsers)
			{
				WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v);
				y += 40;
			}*/
		}

		public override void DrawRight(float x, ref float y, float width)
		{
			//effectWorking?.Draw(x, ref y, width);

			fixedIngredientFilter.Draw(x + 10, ref y, width - 10);
			
			defaultIngredientFilter.Draw(x + 10, ref y, width - 10);
		}

		public override void Rebuild()
		{
			foreach (var v in this.inputWidgets)
				v.ResetBuffers();

			//this.effectWorking = null;
			//if (base.Def.effectWorking != null)
			//	this.effectWorking = new EffecterDefWidget(base.Def.effectWorking);

			this.fixedIngredientFilter = new ThingFilterWidget("Fixed Ingredient Filter", base.Def.fixedIngredientFilter);
			this.defaultIngredientFilter = new ThingFilterWidget("Default Ingredient Filter", base.Def.defaultIngredientFilter);

			this.ingredients.Clear();
			if (base.Def.ingredients != null)
				foreach (var v in base.Def.ingredients)
					this.ingredients.Add(new IngredientCountWidget(v));

			this.products?.Clear();
			if (base.Def.products != null)
				foreach (var v in base.Def.products)
					this.products.Add(this.CreateThingDefCountClass(v));

			this.skillRequirements?.Clear();
			if (base.Def.skillRequirements != null)
				foreach (var v in base.Def.skillRequirements)
					this.skillRequirements.Add(this.CreateSkillRequirements(v));
		}

		public override void ResetBuffers()
		{
			foreach (var v in this.inputWidgets)
				v.ResetBuffers();
		}

		private IntInputWidget<SkillRequirement> CreateSkillRequirements(SkillRequirement skillRequirement)
		{
			return new IntInputWidget<SkillRequirement>(skillRequirement, skillRequirement.skill.label, (sr) => sr.minLevel, (sr, i) => sr.minLevel = i);
		}

		private IntInputWidget<ThingDefCountClass> CreateThingDefCountClass(ThingDefCountClass tdc)
		{
			return new IntInputWidget<ThingDefCountClass>(tdc, tdc.thingDef.label, (tdcc) => tdcc.count, (tdcc, i) => tdcc.count = i);
		}
	}
}
