using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System.Collections.Generic;

namespace InGameDefEditor.Gui.EditorWidgets
{
	class DifficultyDefWidget : AParentDefStatWidget<DifficultyDef>
	{
		private readonly List<IInputWidget> leftInputWidgets;
		private readonly List<IInputWidget> midddleInputWidgets;

		public DifficultyDefWidget(DifficultyDef def, DefType type) : base(def, type)
		{
			//public ColorStats drawColor;
			this.leftInputWidgets = new List<IInputWidget>()
			{
				new IntInputWidget<DifficultyDef>(base.Def, "Difficulty", d => d.difficulty, (d, v) => d.difficulty = v),
				new BoolInputWidget<DifficultyDef>(base.Def, "Is Extreme", d => d.isExtreme, (d, v) => d.isExtreme = v),
				new BoolInputWidget<DifficultyDef>(base.Def, "Allow Big Threats", d => d.allowBigThreats, (d, v) => d.allowBigThreats = v),
				new BoolInputWidget<DifficultyDef>(base.Def, "Allow Intro Threats", d => d.allowIntroThreats, (d, v) => d.allowIntroThreats = v),
				new BoolInputWidget<DifficultyDef>(base.Def, "Allow Cave Hives", d => d.allowCaveHives, (d, v) => d.allowCaveHives = v),
				new BoolInputWidget<DifficultyDef>(base.Def, "PeacefulTemples", d => d.peacefulTemples, (d, v) => d.peacefulTemples = v),
				new BoolInputWidget<DifficultyDef>(base.Def, "Predators Hunt Humanlikes", d => d.predatorsHuntHumanlikes, (d, v) => d.predatorsHuntHumanlikes = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "ThreatScale", d => d.threatScale, (d, v) => d.threatScale = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Colonist Mood Offset", d => d.colonistMoodOffset, (d, v) => d.colonistMoodOffset = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Trade Price Factor Loss", d => d.tradePriceFactorLoss, (d, v) => d.tradePriceFactorLoss = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Crop Yield Factor", d => d.cropYieldFactor, (d, v) => d.cropYieldFactor = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Mine Yield Factor", d => d.mineYieldFactor, (d, v) => d.mineYieldFactor = v),
			};

			this.midddleInputWidgets = new List<IInputWidget>()
			{
				new FloatInputWidget<DifficultyDef>(base.Def, "Enemy Death On Downed Chance Factor", d => d.enemyDeathOnDownedChanceFactor, (d, v) => d.enemyDeathOnDownedChanceFactor = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Research Speed Factor", d => d.researchSpeedFactor, (d, v) => d.researchSpeedFactor = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Disease Interval Factor", d => d.diseaseIntervalFactor, (d, v) => d.diseaseIntervalFactor = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Enemy Reproduction Rate Factor", d => d.enemyReproductionRateFactor, (d, v) => d.enemyReproductionRateFactor = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Player Pawn Infection Chance Factor", d => d.playerPawnInfectionChanceFactor, (d, v) => d.playerPawnInfectionChanceFactor = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Manhunter Chance On Damage Factor", d => d.manhunterChanceOnDamageFactor, (d, v) => d.manhunterChanceOnDamageFactor = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Deep Drill Infestation Chance Factor", d => d.deepDrillInfestationChanceFactor, (d, v) => d.deepDrillInfestationChanceFactor = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Food Poison Chance Factor", d => d.foodPoisonChanceFactor, (d, v) => d.foodPoisonChanceFactor = v),
				// TODO new FloatInputWidget<DifficultyDef>(base.Def, "Raid Beacon Threat Count Factor", d => d.raidBeaconThreatCountFactor, (d, v) => d.raidBeaconThreatCountFactor = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Maintenance Cost Factor", d => d.maintenanceCostFactor, (d, v) => d.maintenanceCostFactor = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Adaptation Growth Rate Factor Over Zero", d => d.adaptationGrowthRateFactorOverZero, (d, v) => d.adaptationGrowthRateFactorOverZero = v),
				new FloatInputWidget<DifficultyDef>(base.Def, "Adaptation Effect Factor", d => d.adaptationEffectFactor, (d, v) => d.adaptationEffectFactor = v),
			};
			this.Rebuild();
		}

		public override void DrawLeft(float x, ref float y, float width)
		{
			foreach (var v in this.leftInputWidgets)
				v.Draw(x, ref y, width);
		}

		public override void DrawMiddle(float x, ref float y, float width)
		{
			foreach (var v in this.midddleInputWidgets)
				v.Draw(x, ref y, width);
		}

		public override void DrawRight(float x, ref float y, float width)
		{
			// Nothing
		}

		public override void Rebuild()
		{
			this.ResetBuffers();
		}

		public override void ResetBuffers()
		{
			this.leftInputWidgets.ForEach(v => v.ResetBuffers());
			this.midddleInputWidgets.ForEach(v => v.ResetBuffers());
		}
	}
}
