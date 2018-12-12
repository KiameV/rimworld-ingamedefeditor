using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using Verse;

namespace InGameDefEditor.Stats
{
	[Serializable]
	public class DifficultyDefStat : DefStat<DifficultyDef>, IParentStat
	{
		public ColorStats drawColor;
		public bool isExtreme;
		public int difficulty;
		public float threatScale;
		public bool allowBigThreats;
		public bool allowIntroThreats;
		public bool allowCaveHives;
		public bool peacefulTemples;
		public bool predatorsHuntHumanlikes;
		public float colonistMoodOffset;
		public float tradePriceFactorLoss;
		public float cropYieldFactor;
		public float mineYieldFactor;
		public float researchSpeedFactor;
		public float diseaseIntervalFactor;
		public float enemyReproductionRateFactor;
		public float playerPawnInfectionChanceFactor;
		public float manhunterChanceOnDamageFactor;
		public float deepDrillInfestationChanceFactor;
		public float foodPoisonChanceFactor;
		public float raidBeaconThreatCountFactor;
		public float maintenanceCostFactor;
		public float enemyDeathOnDownedChanceFactor;
		public float adaptationGrowthRateFactorOverZero;
		public float adaptationEffectFactor;

		public string UniqueKey => base.defName;

		public DifficultyDefStat() { }
		public DifficultyDefStat(DifficultyDef d) : base(d)
		{
			this.drawColor = new ColorStats(d.drawColor);
			this.isExtreme = d.isExtreme;
			this.difficulty = d.difficulty;
			this.threatScale = d.threatScale;
			this.allowBigThreats = d.allowBigThreats;
			this.allowCaveHives = d.allowCaveHives;
			this.allowIntroThreats = d.allowIntroThreats;
			this.peacefulTemples = d.peacefulTemples;
			this.predatorsHuntHumanlikes = d.predatorsHuntHumanlikes;
			this.colonistMoodOffset = d.colonistMoodOffset;
			this.tradePriceFactorLoss = d.tradePriceFactorLoss;
			this.cropYieldFactor = d.cropYieldFactor;
			this.mineYieldFactor = d.mineYieldFactor;
			this.researchSpeedFactor = d.researchSpeedFactor;
			this.diseaseIntervalFactor = d.diseaseIntervalFactor;
			this.enemyReproductionRateFactor = d.enemyReproductionRateFactor;
			this.playerPawnInfectionChanceFactor = d.playerPawnInfectionChanceFactor;
			this.manhunterChanceOnDamageFactor = d.manhunterChanceOnDamageFactor;
			this.deepDrillInfestationChanceFactor = d.deepDrillInfestationChanceFactor;
			this.foodPoisonChanceFactor = d.foodPoisonChanceFactor;
			this.raidBeaconThreatCountFactor = d.raidBeaconThreatCountFactor;
			this.maintenanceCostFactor = d.maintenanceCostFactor;
			this.enemyDeathOnDownedChanceFactor = d.enemyDeathOnDownedChanceFactor;
			this.adaptationGrowthRateFactorOverZero = d.adaptationGrowthRateFactorOverZero;
			this.adaptationEffectFactor = d.adaptationEffectFactor;
		}

		public void ApplyStats(object def)
		{
			if (def is DifficultyDef to)
			{
				to.drawColor = this.drawColor.ToColor();
				to.isExtreme = this.isExtreme;
				to.difficulty = this.difficulty;
				to.threatScale = this.threatScale;
				to.allowBigThreats = this.allowBigThreats;
				to.allowCaveHives = this.allowCaveHives;
				to.allowIntroThreats = this.allowIntroThreats;
				to.peacefulTemples = this.peacefulTemples;
				to.predatorsHuntHumanlikes = this.predatorsHuntHumanlikes;
				to.colonistMoodOffset = this.colonistMoodOffset;
				to.tradePriceFactorLoss = this.tradePriceFactorLoss;
				to.cropYieldFactor = this.cropYieldFactor;
				to.mineYieldFactor = this.mineYieldFactor;
				to.researchSpeedFactor = this.researchSpeedFactor;
				to.diseaseIntervalFactor = this.diseaseIntervalFactor;
				to.enemyReproductionRateFactor = this.enemyReproductionRateFactor;
				to.playerPawnInfectionChanceFactor = this.playerPawnInfectionChanceFactor;
				to.manhunterChanceOnDamageFactor = this.manhunterChanceOnDamageFactor;
				to.deepDrillInfestationChanceFactor = this.deepDrillInfestationChanceFactor;
				to.foodPoisonChanceFactor = this.foodPoisonChanceFactor;
				to.raidBeaconThreatCountFactor = this.raidBeaconThreatCountFactor;
				to.maintenanceCostFactor = this.maintenanceCostFactor;
				to.enemyDeathOnDownedChanceFactor = this.enemyDeathOnDownedChanceFactor;
				to.adaptationGrowthRateFactorOverZero = this.adaptationGrowthRateFactorOverZero;
				to.adaptationEffectFactor = this.adaptationEffectFactor;
			}
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj) &&
				obj is DifficultyDefStat d)
			{
				return
					object.Equals(this.drawColor, d.drawColor) &&
					this.isExtreme == d.isExtreme &&
					this.difficulty == d.difficulty &&
					this.threatScale == d.threatScale &&
					this.allowBigThreats == d.allowBigThreats &&
					this.allowCaveHives == d.allowCaveHives &&
					this.allowIntroThreats == d.allowIntroThreats &&
					this.peacefulTemples == d.peacefulTemples &&
					this.predatorsHuntHumanlikes == d.predatorsHuntHumanlikes &&
					this.colonistMoodOffset == d.colonistMoodOffset &&
					this.tradePriceFactorLoss == d.tradePriceFactorLoss &&
					this.cropYieldFactor == d.cropYieldFactor &&
					this.mineYieldFactor == d.mineYieldFactor &&
					this.researchSpeedFactor == d.researchSpeedFactor &&
					this.diseaseIntervalFactor == d.diseaseIntervalFactor &&
					this.enemyReproductionRateFactor == d.enemyReproductionRateFactor &&
					this.playerPawnInfectionChanceFactor == d.playerPawnInfectionChanceFactor &&
					this.manhunterChanceOnDamageFactor == d.manhunterChanceOnDamageFactor &&
					this.deepDrillInfestationChanceFactor == d.deepDrillInfestationChanceFactor &&
					this.foodPoisonChanceFactor == d.foodPoisonChanceFactor &&
					this.raidBeaconThreatCountFactor == d.raidBeaconThreatCountFactor &&
					this.maintenanceCostFactor == d.maintenanceCostFactor &&
					this.enemyDeathOnDownedChanceFactor == d.enemyDeathOnDownedChanceFactor &&
					this.adaptationGrowthRateFactorOverZero == d.adaptationGrowthRateFactorOverZero &&
					this.adaptationEffectFactor == d.adaptationEffectFactor;
			}
			return false;
		}
	}
}
