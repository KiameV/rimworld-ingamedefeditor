using InGameDefEditor.Stats.Misc;
using RimWorld;
using System.Collections.Generic;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	class StorytellerCompPropertiesWidget : ACollapsibleWidget
	{
		private readonly StorytellerCompProperties Props;
		private readonly string label;
		
		private readonly List<IInputWidget> inputWidgets;
		private readonly List<SimpleCurveWidget> simpleCurveWidgets = new List<SimpleCurveWidget>();

		private PlusMinusArgs<IncidentTargetTagDef> allowedTargetTags;
		private PlusMinusArgs<IncidentTargetTagDef> disallowedTargetTags;

		private List<FloatInputWidget<IncidentCategoryEntry>> categoryWeights;
		private PlusMinusArgs<IncidentCategoryDef> categoryWeightArgs;

		public override string DisplayLabel => label;

		public StorytellerCompPropertiesWidget(StorytellerCompProperties p) : base(true, true)
		{
			if (p.allowedTargetTags == null)
				p.allowedTargetTags = new List<IncidentTargetTagDef>();
			if (p.disallowedTargetTags == null)
				p.disallowedTargetTags = new List<IncidentTargetTagDef>();

			this.Props = p;
			this.label = StorytellerCompPropertiesStats.GetLabel(p);

			this.inputWidgets = new List<IInputWidget>()
			{
				new FloatInputWidget<StorytellerCompProperties>(p, "Min Days Passed", c => c.minDaysPassed, (c, v) => c.minDaysPassed = v),
				new FloatInputWidget<StorytellerCompProperties>(p, "Min Inc Chance Pop Intent Factor", c => c.minIncChancePopulationIntentFactor, (c, v) => c.minIncChancePopulationIntentFactor = v),
			};

			this.allowedTargetTags = new PlusMinusArgs<IncidentTargetTagDef>()
			{
				allItems = DefDatabase<IncidentTargetTagDef>.AllDefs,
				getDisplayName = v => Util.GetDefLabel(v),
				beingUsed = () => this.Props.allowedTargetTags,
				onAdd = v =>
				{
					Util.AddTo(this.Props.allowedTargetTags, v);
					Util.RemoveFrom(this.Props.disallowedTargetTags, v);
				},
				onRemove = v => Util.RemoveFrom(this.Props.allowedTargetTags, v)
			};

			this.disallowedTargetTags = new PlusMinusArgs<IncidentTargetTagDef>()
			{
				allItems = DefDatabase<IncidentTargetTagDef>.AllDefs,
				getDisplayName = v => Util.GetDefLabel(v),
				beingUsed = () => this.Props.disallowedTargetTags,
				onAdd = v =>
				{
					Util.AddTo(this.Props.disallowedTargetTags, v);
					Util.RemoveFrom(this.Props.allowedTargetTags, v);
				},
				onRemove = v => Util.RemoveFrom(this.Props.disallowedTargetTags, v)
			};
			
			switch (p.compClass.FullName)
			{
				case "RimWorld.StorytellerComp_CategoryIndividualMTBByBiome":
					if (p is StorytellerCompProperties_CategoryIndividualMTBByBiome cb)
					{
						this.inputWidgets.Add(new BoolInputWidget<StorytellerCompProperties_CategoryIndividualMTBByBiome>(
							cb, "Apply Caravan Visibility", c => c.applyCaravanVisibility, (c, v) => c.applyCaravanVisibility = v));
					}
					break;
				case "RimWorld.StorytellerComp_CategoryMTB":
					if (p is StorytellerCompProperties_CategoryMTB cm)
					{
						this.inputWidgets.Add(new FloatInputWidget<StorytellerCompProperties_CategoryMTB>(
							cm, "Mean Time Between (Days)", c => c.mtbDays, (c, v) => c.mtbDays = v));
					}
					break;
				case "RimWorld.StorytellerComp_DeepDrillInfestation":
					if (p is StorytellerCompProperties_DeepDrillInfestation cd)
					{
						this.inputWidgets.Add(new FloatInputWidget<StorytellerCompProperties_DeepDrillInfestation>(
							cd, "Drill Base Mean Time Between (Days)", c => c.baseMtbDaysPerDrill, (c, v) => c.baseMtbDaysPerDrill = v));
					}
					break;
				case "RimWorld.StorytellerComp_FactionInteraction":
					if (p is StorytellerCompProperties_FactionInteraction fi)
					{
						this.inputWidgets.Add(new FloatInputWidget<StorytellerCompProperties_FactionInteraction>(
							fi, "Base Incidents Per Year", c => c.baseIncidentsPerYear, (c, v) => c.baseIncidentsPerYear = v));
						this.inputWidgets.Add(new FloatInputWidget<StorytellerCompProperties_FactionInteraction>(
							fi, "Min Spacing Days", c => c.minSpacingDays, (c, v) => c.minSpacingDays = v));
						this.inputWidgets.Add(new EnumInputWidget<StorytellerCompProperties_FactionInteraction, StoryDanger>(
							fi, "Min Danger", 200, c => c.minDanger, (c, v) => c.minDanger = v));
						this.inputWidgets.Add(new BoolInputWidget<StorytellerCompProperties_FactionInteraction>(
							fi, "Full Allies Only", c => c.fullAlliesOnly, (c, v) => c.fullAlliesOnly = v));
					}
					break;
				case "RimWorld.StorytellerComp_OnOffCycle":
					if (p is StorytellerCompProperties_OnOffCycle ooc)
					{
						this.inputWidgets.Add(new FloatInputWidget<StorytellerCompProperties_OnOffCycle>(
							ooc, "On Days", c => c.onDays, (c, v) => c.onDays = v));
						this.inputWidgets.Add(new FloatInputWidget<StorytellerCompProperties_OnOffCycle>(
							ooc, "Off Days", c => c.offDays, (c, v) => c.offDays = v));
						this.inputWidgets.Add(new FloatInputWidget<StorytellerCompProperties_OnOffCycle>(
							ooc, "Min Spacing Days", c => c.minSpacingDays, (c, v) => c.minSpacingDays = v));
						this.inputWidgets.Add(new BoolInputWidget<StorytellerCompProperties_OnOffCycle>(
							ooc, "Apply Raid Beacon Thread Mean Time Between Factor", c => c.applyRaidBeaconThreatMtbFactor, (c, v) => c.applyRaidBeaconThreatMtbFactor = v));
						this.inputWidgets.Add(new FloatInputWidget<StorytellerCompProperties_OnOffCycle>(
							ooc, "Force Raid Enemy Before Days Passed", c => c.forceRaidEnemyBeforeDaysPassed, (c, v) => c.forceRaidEnemyBeforeDaysPassed = v));
						this.inputWidgets.Add(new MinMaxInputWidget<StorytellerCompProperties_OnOffCycle, float>(
							"Num Incidents range",
							new FloatInputWidget<StorytellerCompProperties_OnOffCycle>(
								ooc, "Min", c => c.numIncidentsRange.min, (c, v) => c.numIncidentsRange.min = v),
							new FloatInputWidget<StorytellerCompProperties_OnOffCycle>(
								ooc, "Max", c => c.numIncidentsRange.max, (c, v) => c.numIncidentsRange.max = v)));
					}
					break;
				case "RimWorld.StorytellerComp_RandomMain":
					if (p is StorytellerCompProperties_RandomMain rm)
					{
						this.inputWidgets.Add(new FloatInputWidget<StorytellerCompProperties_RandomMain>(
							rm, "Mean Time Between (Days)", c => c.mtbDays, (c, v) => c.mtbDays = v));
						this.inputWidgets.Add(new FloatInputWidget<StorytellerCompProperties_RandomMain>(
							rm, "Max Threat Big Interval Days", c => c.maxThreatBigIntervalDays, (c, v) => c.maxThreatBigIntervalDays = v));
						this.inputWidgets.Add(new BoolInputWidget<StorytellerCompProperties_RandomMain>(
							rm, "Skip Threat Big If Raid Beacon", c => c.skipThreatBigIfRaidBeacon, (c, v) => c.skipThreatBigIfRaidBeacon = v));
						this.inputWidgets.Add(new MinMaxInputWidget<StorytellerCompProperties_RandomMain, float>(
							"Random Points Factor Range",
							new FloatInputWidget<StorytellerCompProperties_RandomMain>(
								rm, "Min", c => c.randomPointsFactorRange.min, (c, v) => c.randomPointsFactorRange.min = v),
							new FloatInputWidget<StorytellerCompProperties_RandomMain>(
								rm, "Max", c => c.randomPointsFactorRange.max, (c, v) => c.randomPointsFactorRange.max = v)));

						this.categoryWeightArgs = new PlusMinusArgs<IncidentCategoryDef>()
						{
							allItems = DefDatabase<IncidentCategoryDef>.AllDefs,
							isBeingUsed = d =>
							{
								foreach (var input in this.categoryWeights)
									if (input.Parent.category == d)
										return true;
								return false;
							},
							getDisplayName = d => Util.GetDefLabel(d),
							onAdd = d =>
							{
								IncidentCategoryEntry ice = new IncidentCategoryEntry()
								{
									category = d,
									weight = 0
								};
								this.categoryWeights.Add(this.CreateCategoryWeightInput(ice));
								((StorytellerCompProperties_RandomMain)this.Props).categoryWeights.Add(ice);
							},
							onRemove = d =>
							{
								this.categoryWeights.RemoveAll(v => v.Parent.category == d);
								((StorytellerCompProperties_RandomMain)this.Props).categoryWeights.RemoveAll(v => v.category == d);
							}
						};
					}
					break;
				case "RimWorld.StorytellerComp_SingleMTB":
					if (p is StorytellerCompProperties_SingleMTB smtb)
					{
						this.inputWidgets.Add(new FloatInputWidget<StorytellerCompProperties_SingleMTB>(
							smtb, "Mean Time Between (Days)", c => c.mtbDays, (c, v) => c.mtbDays = v));
					}
					break;
				case "RimWorld.StorytellerComp_Triggered":
					if (p is StorytellerCompProperties_Triggered t)
					{
						this.inputWidgets.Add(new IntInputWidget<StorytellerCompProperties_Triggered>(
							t, "Delay Ticks", c => c.delayTicks, (c, v) => c.delayTicks = v));
					}
					break;
			}
			this.Rebuild();
		}

		private FloatInputWidget<IncidentCategoryEntry> CreateCategoryWeightInput(IncidentCategoryEntry ice)
		{
			return new FloatInputWidget<IncidentCategoryEntry>(ice, Util.GetDefLabel(ice.category), c => c.weight, (c, v) => c.weight = v);
		}

		protected override void DrawInputs(float x, ref float y, float width)
		{
			foreach (var v in this.inputWidgets)
				v.Draw(x, ref y, width);

			if (this.Props is StorytellerCompProperties_RandomMain rm &&
				this.categoryWeightArgs != null)
			{
				WindowUtil.PlusMinusLabel(x, ref y, width, "Category Weights", this.categoryWeightArgs);
				if (rm.categoryWeights != null)
					foreach (var v in rm.categoryWeights)
						WindowUtil.DrawLabel(x + 20, ref y, width, "- " + Util.GetDefLabel(v.category), 30);
			}

			if (this.allowedTargetTags != null)
			{
				WindowUtil.PlusMinusLabel(x, ref y, width, "Allowed Target Tags", this.allowedTargetTags);
				if (this.Props.allowedTargetTags != null)
					foreach (var v in this.Props.allowedTargetTags)
						WindowUtil.DrawLabel(x + 20, ref y, width, "- " + Util.GetDefLabel(v), 30);
			}

			if (this.disallowedTargetTags != null)
			{
				WindowUtil.PlusMinusLabel(x, ref y, width, "Disallowed Target Tags", this.disallowedTargetTags);
				if (this.Props.disallowedTargetTags != null)
					foreach (var v in this.Props.disallowedTargetTags)
						WindowUtil.DrawLabel(x + 20, ref y, width, "- " + Util.GetDefLabel(v), 30);
			}

			foreach (var v in this.simpleCurveWidgets)
				v.Draw(x, ref y, width);
		}

		public void Rebuild()
		{
			if (this.Props is StorytellerCompProperties_RandomMain rm)
			{
				this.categoryWeights?.Clear();
				Util.Populate(out this.categoryWeights, rm.categoryWeights, v => this.CreateCategoryWeightInput(v));
			}
			this.simpleCurveWidgets.Clear();
			switch (this.Props.compClass.FullName)
			{
				case "RimWorld.StorytellerComp_CategoryMTB":
					if (this.Props is StorytellerCompProperties_CategoryMTB cm)
					{
						if (cm.mtbDaysFactorByDaysPassedCurve != null)
							this.simpleCurveWidgets.Add(new SimpleCurveWidget("Mean Time Between Factor By Days", cm.mtbDaysFactorByDaysPassedCurve));
					}
					break;
				case "RimWorld.StorytellerComp_OnOffCycle":
					if (this.Props is StorytellerCompProperties_OnOffCycle ooc)
					{
						if (ooc.acceptFractionByDaysPassedCurve != null)
							this.simpleCurveWidgets.Add(new SimpleCurveWidget("Accept Fraction By Days Passed", ooc.acceptFractionByDaysPassedCurve));
						if (ooc.acceptPercentFactorPerThreatPointsCurve != null)
							this.simpleCurveWidgets.Add(new SimpleCurveWidget("Accept Percent Factor Per Threat Points", ooc.acceptPercentFactorPerThreatPointsCurve));
					}
					break;
			}
			this.ResetBuffers();
		}

		public override void ResetBuffers()
		{
			this.inputWidgets.ForEach(v => v.ResetBuffers());
		}
	}
}
