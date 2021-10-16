using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System.Collections.Generic;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class PawnKindDefWidget : AParentDefStatWidget<PawnKindDef>
	{
		private readonly List<IInputWidget> leftInputWidgets;
		private readonly List<IInputWidget> middleInputWidgets;
		private readonly List<IInputWidget> rightInputWidgets;

		public PawnKindDefWidget(PawnKindDef def, DefType type) : base(def, type)
		{
			this.leftInputWidgets = new List<IInputWidget>()
			{
				new IntInputWidget<PawnKindDef>(base.Def, "Min Generation Age", d => d.minGenerationAge, (d, v) => d.minGenerationAge = v),
				new IntInputWidget<PawnKindDef>(base.Def, "Max Generation Age", d => d.maxGenerationAge, (d, v) => d.maxGenerationAge = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "Allow Old Age Injuries", d => d.allowOldAgeInjuries, (d, v) => d.allowOldAgeInjuries = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "Destroy Gear on Drop", d => d.destroyGearOnDrop, (d, v) => d.destroyGearOnDrop = v),
				new FloatInputWidget<PawnKindDef>(base.Def, "Defend Point Radius", d => d.defendPointRadius, (d, v) => d.defendPointRadius = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "Faction Hostil On Kill", d => d.factionHostileOnKill, (d, v) => d.factionHostileOnKill = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "Faction Hostil On Death", d => d.factionHostileOnDeath, (d, v) => d.factionHostileOnDeath = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "Is Fighter", d => d.isFighter, (d, v) => d.isFighter = v),
				new FloatInputWidget<PawnKindDef>(base.Def, "Combat Power", d => d.combatPower, (d, v) => d.combatPower = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "Can Arrive Manhunter", d => d.canArriveManhunter, (d, v) => d.canArriveManhunter = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "Can Be Sapper", d => d.canBeSapper, (d, v) => d.canBeSapper = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "Is Good Breacher", d => d.isGoodBreacher, (d, v) => d.isGoodBreacher = v),
				new FloatInputWidget<PawnKindDef>(base.Def, "Base Recruit Difficulty", d => d.baseRecruitDifficulty, (d, v) => d.baseRecruitDifficulty = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "AI Avoid Cover", d => d.aiAvoidCover, (d, v) => d.aiAvoidCover = v),
				new FloatInputWidget<PawnKindDef>(base.Def, "Accept Arrest Chance Factor", d => d.acceptArrestChanceFactor, (d, v) => d.acceptArrestChanceFactor = v),
				new FloatInputWidget<PawnKindDef>(base.Def, "apparelAllowHeadgearChance", d => d.apparelAllowHeadgearChance, (d, v) => d.apparelAllowHeadgearChance = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "Apparel Ignore Seasons", d => d.apparelIgnoreSeasons, (d, v) => d.apparelIgnoreSeasons = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "Ignore Faction Apparel Stuff Requirements", d => d.ignoreFactionApparelStuffRequirements, (d, v) => d.ignoreFactionApparelStuffRequirements = v),
				new FloatInputWidget<PawnKindDef>(base.Def, "Tech Hediffs Chance", d => d.techHediffsChance, (d, v) => d.techHediffsChance = v),
				new IntInputWidget<PawnKindDef>(base.Def, "Tech Hediffs Max Amount", d => d.techHediffsMaxAmount, (d, v) => d.techHediffsMaxAmount = v),
				new FloatInputWidget<PawnKindDef>(base.Def, "Biocode Weapon Chance", d => d.biocodeWeaponChance, (d, v) => d.biocodeWeaponChance = v),
				new FloatInputWidget<PawnKindDef>(base.Def, "Chemical Addiction Chance", d => d.chemicalAddictionChance, (d, v) => d.chemicalAddictionChance = v),
				new FloatInputWidget<PawnKindDef>(base.Def, "Combat Enhancing Drugs Chance", d => d.combatEnhancingDrugsChance, (d, v) => d.combatEnhancingDrugsChance = v),
				new BoolInputWidget<PawnKindDef>(base.Def, "Trader", d => d.trader, (d, v) => d.trader = v),
				new IntInputWidget<PawnKindDef>(base.Def, "Extra Skill Levels", d => d.extraSkillLevels, (d, v) => d.extraSkillLevels = v),
				new IntInputWidget<PawnKindDef>(base.Def, "Min Total Skill Levels", d => d.minTotalSkillLevels, (d, v) => d.minTotalSkillLevels = v),
				new IntInputWidget<PawnKindDef>(base.Def, "Min Best Skill Level", d => d.minBestSkillLevel, (d, v) => d.minBestSkillLevel = v),
				new FloatInputWidget<PawnKindDef>(base.Def, "Eco System Weight", d => d.ecoSystemWeight, (d, v) => d.ecoSystemWeight = v),
			};
			if (def.apparelRequired == null)
				def.apparelRequired = new List<ThingDef>();
			if (def.techHediffsRequired == null)
				def.techHediffsRequired = new List<ThingDef>();
			if (def.forcedAddictions == null)
				def.forcedAddictions = new List<ChemicalDef>();
			if (def.weaponTags == null)
				def.weaponTags = new List<string>();
			if (def.apparelTags == null)
				def.apparelTags = new List<string>();
			if (def.apparelDisallowTags == null)
				def.apparelDisallowTags = new List<string>();
			if (def.techHediffsTags == null)
				def.techHediffsTags = new List<string>();
			if (def.techHediffsDisallowTags == null)
				def.techHediffsDisallowTags = new List<string>();
			this.middleInputWidgets = new List<IInputWidget>()
			{
				new DefInputWidget<PawnKindDef, ThingDef>(base.Def, "Race", 150, d => d.race, (d, v) => d.race = v, true),
				new DefInputWidget<PawnKindDef, FactionDef>(base.Def, "Default Faction Type", 150, d => d.defaultFactionType, (d, v) => d.defaultFactionType = v, true),
				new DefPlusMinusInputWidget<TraitDef>("Disallowed Traits", 150, () => def.disallowedTraits),
				new DefInputWidget<PawnKindDef, ThingDef>(base.Def, "Weapon Stuff Override", 150, d => d.weaponStuffOverride, (d, v) => d.weaponStuffOverride = v, true),
				new DefInputWidget<PawnKindDef, ThingStyleDef>(base.Def, "Weapon Style Def", 150, d => d.weaponStyleDef, (d, v) => d.weaponStyleDef = v, true),
				new DefPlusMinusInputWidget<ThingDef>("Apparel Required", 150, () => def.apparelRequired),
				new DefPlusMinusInputWidget<ThingDef>("Tech Hediffs Required", 150, () => def.techHediffsRequired),
				new DefPlusMinusInputWidget<ChemicalDef>("Forced Addictions", 150, () => def.forcedAddictions),
				new TextPlusMinusInputWidget("Weapon Tags", 150, () => def.weaponTags),
				new TextPlusMinusInputWidget("Apparel Tags", 150, () => def.apparelTags),
				new TextPlusMinusInputWidget("Apparel Disallow Tags", 150, () => def.apparelDisallowTags),
				new TextPlusMinusInputWidget("Tech Hediffs Tags", 150, () => def.techHediffsTags),
				new TextPlusMinusInputWidget("Tech Hediffs Disallow Tags", 150, () => def.techHediffsDisallowTags),
			};

			this.rightInputWidgets = new List<IInputWidget>()
			{
				WidgetFactory<PawnKindDef>.CreateMinMaxFloatWidget(def, "Flee Health Threshold Range",
					p => p.fleeHealthThresholdRange.min, (p, v) => p.fleeHealthThresholdRange.min = v,
					p => p.fleeHealthThresholdRange.max, (p, v) => p.fleeHealthThresholdRange.max = v),
				WidgetFactory<PawnKindDef>.CreateMinMaxFloatWidget(def, "Gear Health Range",
					p => p.gearHealthRange.min, (p, v) => p.gearHealthRange.min = v,
					p => p.gearHealthRange.max, (p, v) => p.gearHealthRange.max = v),
				WidgetFactory<PawnKindDef>.CreateMinMaxFloatWidget(def, "Weapon Money",
					p => p.weaponMoney.min, (p, v) => p.weaponMoney.min = v,
					p => p.weaponMoney.max, (p, v) => p.weaponMoney.max = v),
				WidgetFactory<PawnKindDef>.CreateMinMaxFloatWidget(def, "Apparel Money",
					p => p.apparelMoney.min, (p, v) => p.apparelMoney.min = v,
					p => p.apparelMoney.max, (p, v) => p.apparelMoney.max = v),
				WidgetFactory<PawnKindDef>.CreateMinMaxFloatWidget(def, "Tech Hediffs Money",
					p => p.techHediffsMoney.min, (p, v) => p.techHediffsMoney.min = v,
					p => p.techHediffsMoney.max, (p, v) => p.techHediffsMoney.max = v),
				WidgetFactory<PawnKindDef>.CreateMinMaxIntWidget(def, "Wild Group Size",
					p => p.wildGroupSize.min, (p, v) => p.wildGroupSize.min = v,
					p => p.wildGroupSize.max, (p, v) => p.wildGroupSize.max = v),
				WidgetFactory<PawnKindDef>.CreateMinMaxIntWidget(def, "Combat Enhancing Drugs Count",
					p => p.combatEnhancingDrugsCount.min, (p, v) => p.combatEnhancingDrugsCount.min = v,
					p => p.combatEnhancingDrugsCount.max, (p, v) => p.combatEnhancingDrugsCount.max = v)
			};
			if (def.initialResistanceRange != null)
				this.rightInputWidgets.Add(WidgetFactory<PawnKindDef>.CreateMinMaxFloatWidget(def, "Initial Resistance Range", 
					p => p.initialResistanceRange.Value.min, (p, v) => { var f = p.initialResistanceRange.Value; f.min = v; p.initialResistanceRange = f; },
					p => p.initialResistanceRange.Value.max, (p, v) => { var f = p.initialResistanceRange.Value; f.max = v; p.initialResistanceRange = f; }));
			if (def.initialWillRange != null)
				this.rightInputWidgets.Add(WidgetFactory<PawnKindDef>.CreateMinMaxFloatWidget(def, "Initial Will Range",
					p => p.initialWillRange.Value.min, (p, v) => { var f = p.initialWillRange.Value; f.min = v; p.initialWillRange = f; },
					p => p.initialWillRange.Value.max, (p, v) => { var f = p.initialWillRange.Value; f.max = v; p.initialWillRange = f; }));

			this.Rebuild();
		}

		public override void DrawLeft(float x, ref float y, float width)
		{
			foreach (var v in this.leftInputWidgets)
				v.Draw(x, ref y, width);
		}

		public override void DrawMiddle(float x, ref float y, float width)
		{
			foreach (var v in this.middleInputWidgets)
				v.Draw(x, ref y, width);
		}

		public override void DrawRight(float x, ref float y, float width)
		{
			foreach (var v in this.rightInputWidgets)
				v.Draw(x, ref y, width);
		}

		public override void Rebuild()
		{
			this.ResetBuffers();
		}

		public override void ResetBuffers()
		{
			this.leftInputWidgets.ForEach(v => v.ResetBuffers());
			this.middleInputWidgets.ForEach(v => v.ResetBuffers());
			this.rightInputWidgets.ForEach(v => v.ResetBuffers());
		}

		protected override void AddDefsToAutoApply(bool isAutoApply)
		{

		}
	}
}
