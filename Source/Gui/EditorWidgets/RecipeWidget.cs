using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using System;
using static InGameDefEditor.WindowUtil;
using static InGameDefEditor.Gui.EditorWidgets.Misc.ThingFilterWidget;
using InGameDefEditor.Stats.Misc;

namespace InGameDefEditor.Gui.EditorWidgets
{
	class RecipeWidget : AParentStatWidget<RecipeDef>
	{
		private readonly List<IInputWidget> inputWidgets;

		private EffecterDefWidget effectWorking = null;

		private ThingFilterWidget fixedIngredientFilter;
		private ThingFilterWidget defaultIngredientFilter;

		private readonly FloatOptionsArgs<ResearchProjectDef> researchPrerequisite;
		private readonly FloatOptionsArgs<WorkTypeDef> requiredGiverWorkType;
		private readonly FloatOptionsArgs<ThingDef> unfinishedThingDef;
		private readonly FloatOptionsArgs<SoundDef> soundWorking;
		private readonly FloatOptionsArgs<StatDef> workSpeedStat;
		private readonly FloatOptionsArgs<StatDef> efficiencyStat;
		private readonly FloatOptionsArgs<StatDef> workTableEfficiencyStat;
		private readonly FloatOptionsArgs<StatDef> workTableSpeedStat;
		//private readonly FloatOptionsArgs<HediffDef> addsHediff;
		//private readonly FloatOptionsArgs<HediffDef> removesHediff;
		private readonly FloatOptionsArgs<SkillDef> workSkill;

        // TODO Needs to be null when empty
        // private readonly PlusMinusArgs<SpecialProductType> specialProducts;

		private readonly PlusMinusArgs<SpecialThingFilterDef> forceHiddenSpecialFilters;
		//private readonly PlusMinusArgs<ThingDef> recipeUsers;
		private readonly PlusMinusArgs<BodyPartDef> appliedOnFixedBodyParts;

		private readonly List<IntInputWidget<ThingDefCountClass>> products = new List<IntInputWidget<ThingDefCountClass>>();
		private readonly PlusMinusArgs<ThingDef> productsPlusMinusArgs;

		private readonly List<IntInputWidget<SkillRequirement>> skillRequirements = new List<IntInputWidget<SkillRequirement>>();
		private readonly PlusMinusArgs<SkillDef> skillRequirementsPlusMinusArgs;

		private readonly List<IngredientCountWidget> ingredients = new List<IngredientCountWidget>();

		public RecipeWidget(RecipeDef d, WidgetType type) : base(d, type)
		{
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
			};

			this.researchPrerequisite = new FloatOptionsArgs<ResearchProjectDef>()
			{
				getDisplayName = (ResearchProjectDef def) => def.label,
				includeNullOption = true,
				items = DefDatabase<ResearchProjectDef>.AllDefsListForReading,
				onSelect = (ResearchProjectDef def) => base.Def.researchPrerequisite = def,
			};
			this.requiredGiverWorkType = new FloatOptionsArgs<WorkTypeDef>()
			{
				getDisplayName = (def) => def.label,
				includeNullOption = true,
				items = DefDatabase<WorkTypeDef>.AllDefsListForReading,
				onSelect = (def) => base.Def.requiredGiverWorkType = def,
			};
			this.unfinishedThingDef = new FloatOptionsArgs<ThingDef>()
			{
				getDisplayName = (def) => def.label,
				includeNullOption = true,
				items = DefDatabase<ThingDef>.AllDefsListForReading,
				onSelect = (def) => base.Def.unfinishedThingDef = def,
			};
			this.soundWorking = new FloatOptionsArgs<SoundDef>()
			{
				getDisplayName = (def) => def.label,
				includeNullOption = true,
				items = DefDatabase<SoundDef>.AllDefsListForReading,
				onSelect = (def) => base.Def.soundWorking = def,
			};
			this.workSpeedStat = new FloatOptionsArgs<StatDef>()
			{
				getDisplayName = (def) => def.label,
				includeNullOption = true,
				items = DefDatabase<StatDef>.AllDefsListForReading,
				onSelect = (def) => base.Def.workSpeedStat = def,
			};
			this.efficiencyStat = new FloatOptionsArgs<StatDef>()
			{
				getDisplayName = (def) => def.label,
				includeNullOption = true,
				items = DefDatabase<StatDef>.AllDefsListForReading,
				onSelect = (def) => base.Def.efficiencyStat = def,
			};
			this.workTableEfficiencyStat = new FloatOptionsArgs<StatDef>()
			{
				getDisplayName = (def) => def.label,
				includeNullOption = true,
				items = DefDatabase<StatDef>.AllDefsListForReading,
				onSelect = (def) => base.Def.workTableEfficiencyStat = def,
			};
			this.workTableSpeedStat = new FloatOptionsArgs<StatDef>()
			{
				getDisplayName = (def) => def.label,
				includeNullOption = true,
				items = DefDatabase<StatDef>.AllDefsListForReading,
				onSelect = (def) => base.Def.workTableSpeedStat = def,
			};
			/*this.addsHediff = new FloatOptionsArgs<HediffDef>()
			{
				getDisplayName = (def) => def.label,
				includeNullOption = true,
				items = DefDatabase<HediffDef>.AllDefsListForReading,
				onSelect = (def) => base.Def.addsHediff = def,
			};
			this.removesHediff = new FloatOptionsArgs<HediffDef>()
			{
				getDisplayName = (def) => def.label,
				includeNullOption = true,
				items = DefDatabase<HediffDef>.AllDefsListForReading,
				onSelect = (def) => base.Def.removesHediff = def,
			};*/
			this.workSkill = new FloatOptionsArgs<SkillDef>()
			{
				getDisplayName = (def) => def.label,
				includeNullOption = true,
				items = DefDatabase<SkillDef>.AllDefsListForReading,
				onSelect = (def) => base.Def.workSkill = def,
			};

			// Plus Minus
			/*this.specialProducts = new PlusMinusArgs<SpecialProductType>()
			{
				allItems = Enum.GetValues(typeof(SpecialProductType)).OfType<SpecialProductType>().ToList(),
				beingUsed = () => base.Def.specialProducts,
				onAdd = (spt) => base.Def.specialProducts.Add(spt),
				onRemove = (spt) => base.Def.specialProducts.Remove(spt),
				getDisplayName = (spt) => spt.ToString()
			};*/

			this.forceHiddenSpecialFilters = new PlusMinusArgs<SpecialThingFilterDef>()
			{
				allItems = DefDatabase<SpecialThingFilterDef>.AllDefsListForReading,
				beingUsed = () => base.Def.forceHiddenSpecialFilters,
				onAdd = (def) => base.Def.forceHiddenSpecialFilters.Add(def),
				onRemove = (def) => base.Def.forceHiddenSpecialFilters.Remove(def),
				getDisplayName = (def) => def.label
			};

			/*this.recipeUsers = new PlusMinusArgs<ThingDef>()
			{
				allItems = DefDatabase<ThingDef>.AllDefsListForReading,
				beingUsed = () => base.Def.recipeUsers,
				onAdd = (def) => base.Def.recipeUsers.Add(def),
				onRemove = (def) => base.Def.recipeUsers.Remove(def),
				getDisplayName = (def) => def.label
			};*/

			this.appliedOnFixedBodyParts = new PlusMinusArgs<BodyPartDef>()
			{
				allItems = DefDatabase<BodyPartDef>.AllDefsListForReading,
				beingUsed = () => base.Def.appliedOnFixedBodyParts,
				onAdd = (def) => base.Def.appliedOnFixedBodyParts.Add(def),
				onRemove = (def) => base.Def.appliedOnFixedBodyParts.Remove(def),
				getDisplayName = (def) => def.label
			};

			this.productsPlusMinusArgs = new PlusMinusArgs<ThingDef>()
			{
				allItems = DefDatabase<ThingDef>.AllDefsListForReading,
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
					base.Def.products.Add(tdc);
					this.products.Add(this.CreateThingDefCountClass(tdc));
				},
				onRemove = delegate (ThingDef def)
				{
					base.Def.products.RemoveAll((tdc) => tdc.thingDef == def);
					this.products.RemoveAll((input) => input.Parent.thingDef == def);
				},
				getDisplayName = (en) => en.ToString()
			};

			this.skillRequirementsPlusMinusArgs = new PlusMinusArgs<SkillDef>()
			{
				allItems = DefDatabase<SkillDef>.AllDefsListForReading,
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
					base.Def.skillRequirements.Add(sr);
					this.skillRequirements.Add(this.CreateSkillRequirements(sr));
				},
				onRemove = delegate (SkillDef sd)
				{
					base.Def.skillRequirements.RemoveAll((sr) => sr.skill == sd);
					this.skillRequirements.RemoveAll((sr) => sr.Parent.skill == sd);
				},
				getDisplayName = (en) => en.ToString()
			};

			if (base.Def.ingredients == null)
				base.Def.ingredients = new List<IngredientCount>();
			foreach (var v in base.Def.ingredients)
				this.ingredients.Add(new IngredientCountWidget(v));
			
            this.Rebuild();
        }

		public override void DrawLeft(float x, ref float y, float width)
		{
			foreach (var v in inputWidgets)
				v.Draw(x, ref y, width);

			WindowUtil.DrawInput(x, ref y, width, "Research Prerequisite", 150, Util.GetDisplayLabel(this.Def.researchPrerequisite?.label), this.researchPrerequisite);
			WindowUtil.DrawInput(x, ref y, width, "Required Giver WorkType", 150, Util.GetDisplayLabel(this.Def.requiredGiverWorkType?.defName), this.requiredGiverWorkType);
			WindowUtil.DrawInput(x, ref y, width, "Unfinished Thing Def", 150, Util.GetDisplayLabel(this.Def.unfinishedThingDef?.defName), this.unfinishedThingDef);
			WindowUtil.DrawInput(x, ref y, width, "Sound Working", 150, Util.GetDisplayLabel(this.Def.soundWorking?.defName), this.soundWorking);
			WindowUtil.DrawInput(x, ref y, width, "Efficiency Stat", 150, Util.GetDisplayLabel(this.Def.efficiencyStat?.label), this.efficiencyStat);
			//WindowUtil.DrawInput(x, ref y, width, "Adds Hediff", 150, Util.GetDisplayLabel(this.Def.addsHediff?.defName), this.addsHediff);
			//WindowUtil.DrawInput(x, ref y, width, "Removes Hediff", 150, Util.GetDisplayLabel(this.Def.removesHediff?.defName), this.removesHediff);
			WindowUtil.DrawInput(x, ref y, width, "Work Speed Stat", 150, Util.GetDisplayLabel(this.Def.workSpeedStat?.label), this.workSpeedStat);
			WindowUtil.DrawInput(x, ref y, width, "Work Table Efficiency Stat", 150, Util.GetDisplayLabel(this.Def.workTableEfficiencyStat?.label), this.workTableEfficiencyStat);
			WindowUtil.DrawInput(x, ref y, width, "Work Table SpeedStat", 150, Util.GetDisplayLabel(this.Def.workTableSpeedStat?.label), this.workTableSpeedStat);
			WindowUtil.DrawInput(x, ref y, width, "Work Skill", 150, Util.GetDisplayLabel(this.Def.workSkill?.label), this.workSkill);

			if (this.effectWorking != null)
			{ 
				effectWorking.Draw(x, ref y, width);
			}
		}

        public override void DrawMiddle(float x, ref float y, float width)
		{
			WindowUtil.PlusMinusLabel(x, ref y, 150, "Products", this.productsPlusMinusArgs);
			foreach (var v in this.products)
			{
				v.Draw(x + 10, ref y, width);
			}

			WindowUtil.PlusMinusLabel(x, ref y, 150, "Skill Requirements", this.skillRequirementsPlusMinusArgs);
			foreach (var v in this.skillRequirements)
			{
				v.Draw(x + 10, ref y, width);
			}

			WindowUtil.PlusMinusLabel(x, ref y, 150, "Ingredients",
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
			y += 8;

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

			WindowUtil.PlusMinusLabel(x, ref y, 150, "Force Hidden Special Filters", this.forceHiddenSpecialFilters);
			foreach (var v in base.Def.forceHiddenSpecialFilters)
			{
				WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v);
				y += 32;
			}
			y += 8;

			WindowUtil.PlusMinusLabel(x, ref y, 150, "Applied On Fixed Body Parts", this.appliedOnFixedBodyParts);
			foreach (var v in base.Def.appliedOnFixedBodyParts)
			{
				WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v);
				y += 32;
			}
			y += 8;

			/*WindowUtil.PlusMinusLabel(x, ref y, 150, "Recipe Users", this.recipeUsers);
			foreach (var v in base.Def.recipeUsers)
			{
				WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v);
				y += 40;
			}*/
		}

        public override void DrawRight(float x, ref float y, float width)
        {
			WindowUtil.DrawLabel(x, y, width, "Fixed Ingredient Filter", true);
			y += 40;
			fixedIngredientFilter.Draw(x + 10, ref y, width - 10);


			WindowUtil.DrawLabel(x, y, width, "Default Ingredient Filter", true);
			y += 40;
			defaultIngredientFilter.Draw(x + 10, ref y, width - 10);
		}

		public override void Rebuild()
		{
			foreach (var v in this.inputWidgets)
				v.ResetBuffers();

			this.effectWorking = null;
			if (base.Def.effectWorking != null)
				this.effectWorking = new EffecterDefWidget(base.Def.effectWorking);

			this.fixedIngredientFilter = new ThingFilterWidget(base.Def.fixedIngredientFilter);
			this.defaultIngredientFilter = new ThingFilterWidget(base.Def.defaultIngredientFilter);

			//if (base.Def.specialProducts == null)
			//	base.Def.specialProducts = new List<SpecialProductType>(0);

			if (base.Def.forceHiddenSpecialFilters == null)
				base.Def.forceHiddenSpecialFilters = new List<SpecialThingFilterDef>(0);

			if (base.Def.recipeUsers == null)
				base.Def.recipeUsers = new List<ThingDef>(0);

			if (base.Def.appliedOnFixedBodyParts == null)
				base.Def.appliedOnFixedBodyParts = new List<BodyPartDef>(0);

			if (base.Def.products == null)
				base.Def.products = new List<ThingDefCountClass>();

			if (base.Def.skillRequirements == null)
				base.Def.skillRequirements = new List<SkillRequirement>(0);

			if (base.Def.ingredients == null)
				base.Def.ingredients = new List<IngredientCount>();

			this.products.Clear();
			foreach (var v in base.Def.products)
				this.products.Add(this.CreateThingDefCountClass(v));

			this.ingredients.Clear();
			foreach (var v in base.Def.ingredients)
				this.ingredients.Add(new IngredientCountWidget(v));

			this.skillRequirements.Clear();
			foreach (var v in base.Def.skillRequirements)
				this.skillRequirements.Add(this.CreateSkillRequirements(v));
		}

		public override void ResetBuffers()
		{
			foreach (var v in this.inputWidgets)
				v.ResetBuffers();
		}

		/*private void DrawTerrainPatchMakers(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(x, ref y, 100, "Patch Maker",
                delegate
                {
                    Find.WindowStack.Add(new Dialog_Name(
                        "Perlin Frequency",
                        delegate (string name)
                        {
                            TerrainPatchMaker m = new TerrainPatchMaker() { perlinFrequency = float.Parse(name) };
                            base.Def.terrainPatchMakers.Add(m);
                            this.terrainPatchMakers.Add(new TerrainPatchMakerWidget(m));
                        },
                        delegate (string name)
                        {
                            if (!float.TryParse(name, out float freq))
                                return "Must be a number";
                            foreach (var v in base.Def.terrainPatchMakers)
                                if (v.perlinFrequency == freq)
                                    return "Perlin Frequency must be unique";
                            return true;
                        }));
                },
                delegate
                {
                    WindowUtil.DrawFloatingOptions(
                        new WindowUtil.DrawFloatOptionsArgs<TerrainPatchMaker>()
                        {
                            items = base.Def.terrainPatchMakers,
                            getDisplayName = (TerrainPatchMaker m) => m.perlinFrequency.ToString(),
                            onSelect = delegate (TerrainPatchMaker m)
                            {
                                base.Def.terrainPatchMakers.RemoveAll((TerrainPatchMaker tpm) => tpm.perlinFrequency == m.perlinFrequency);
                                this.terrainPatchMakers.RemoveAll((TerrainPatchMakerWidget w) => w.Parent.perlinFrequency == m.perlinFrequency);
                            }
                        });
                });

            x += 10;
            foreach (var v in this.terrainPatchMakers)
                v.Draw(x, ref y, width);
        }*/

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
