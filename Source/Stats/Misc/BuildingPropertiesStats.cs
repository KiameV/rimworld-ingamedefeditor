using System;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;
using System.Collections.Generic;
using System.Reflection;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class BuildingPropertiesStats
	{
		//public Type blueprintClass = typeof(Blueprint_Build);
		//public string sowTag;
		//public Material turretTopMat;
		//public StorageSettings fixedStorageSettings;
		//public StorageSettings defaultStorageSettings;
		//public Graphic trapUnarmedGraphic;

		public bool isEdifice;
		public List<string> buildingTags;
		public bool isInert;
		public bool deconstructible;
		public bool alwaysDeconstructible;
		public bool claimable;
		public bool isSittable;
		public bool expandHomeArea;
		public float uninstallWork;
		public bool wantsHopperAdjacent;
		public bool allowWireConnection;
		public bool shipPart;
		public bool canPlaceOverImpassablePlant;
		public float heatPerTickWhileWorking;
		public bool canBuildNonEdificesUnder;
		public bool canPlaceOverWall;
		public bool allowAutoroof;
		public bool preventDeteriorationOnTop;
		public bool preventDeteriorationInside;
		public bool isMealSource;
		public bool isNaturalRock;
		public bool isResourceRock;
		public bool repairable;
		public float roofCollapseDamageMultiplier;
		public bool hasFuelingPort;
		public bool isPlayerEjectable;
		public float bed_healPerDay;
		public bool bed_defaultMedical;
		public bool bed_showSleeperBody;
		public bool bed_humanlike;
		public float bed_maxBodySize;
		public bool bed_caravansCanUse;
		public float nutritionCostPerDispense;
		public float turretBurstWarmupTime;
		public float turretBurstCooldownTime;
		//public string turretTopGraphicPath;
		public float turretTopDrawSize;
		public Vector2Stats turretTopOffset;
		public bool ai_combatDangerous;
		public bool ai_chillDestination;
		public int mineableYield;
		public float mineableNonMinedEfficiency;
		public float mineableDropChance;
		public bool mineableYieldWasteable;
		public float mineableScatterCommonality;
		public bool ignoreStoredThingsBeauty;
		public bool isTrap;
		public bool trapDestroyOnSpring;
		public float trapPeacefulWildAnimalsSpringChanceFactor;
		public float unpoweredWorkTableWorkSpeedFactor;
		public bool workSpeedPenaltyOutdoors;
		public bool workSpeedPenaltyTemperature;
		public int watchBuildingStandRectWidth;
		public int haulToContainerDuration;

		public MinMaxIntStats mineableScatterLumpSizeRange;
		public MinMaxIntStats watchBuildingStandDistanceRange;
		public GraphicDataStats blueprintGraphicData;
		public GraphicDataStats fullGraveGraphicData;
		public GraphicDataStats trapUnarmedGraphicData;

		public DefStat<TerrainDef> naturalTerrain;
		public DefStat<TerrainDef> leaveTerrain;
		public DefStat<ThingDef> smoothedThing;
		public DefStat<ThingDef> unsmoothedThing;
		public DefStat<ThingDef> turretGunDef;
		public DefStat<ThingDef> defaultPlantToGrow;
		public DefStat<ThingDef> mineableThing;
		public DefStat<SoundDef> soundDispense;
		public DefStat<SoundDef> soundDoorOpenPowered;
		public DefStat<SoundDef> soundDoorClosePowered;
		public DefStat<SoundDef> soundDoorOpenManual;
		public DefStat<SoundDef> soundDoorCloseManual;
		public DefStat<SoundDef> soundAmbient;
		public DefStat<DamageArmorCategoryDef> trapDamageCategory;
		public DefStat<JoyKindDef> joyKind;
		public DefStat<ConceptDef> spawnedConceptLearnOpportunity;
		public DefStat<ConceptDef> boughtConceptLearnOpportunity;

		public BuildingPropertiesStats() { }
		public BuildingPropertiesStats(BuildingProperties p)
		{
			this.isEdifice = p.isEdifice;
			this.isInert = p.isInert;
			this.deconstructible = GetDeconstructible(p);
			this.alwaysDeconstructible = p.alwaysDeconstructible;
			this.claimable = p.claimable;
			this.isSittable = p.isSittable;
			this.expandHomeArea = p.expandHomeArea;
			this.uninstallWork = p.uninstallWork;
			this.wantsHopperAdjacent = p.wantsHopperAdjacent;
			this.allowWireConnection = p.allowWireConnection;
			this.shipPart = p.shipPart;
			this.canPlaceOverImpassablePlant = p.canPlaceOverImpassablePlant;
			this.heatPerTickWhileWorking = p.heatPerTickWhileWorking;
			this.canBuildNonEdificesUnder = p.canBuildNonEdificesUnder;
			this.canPlaceOverWall = p.canPlaceOverImpassablePlant;
			this.allowAutoroof = p.allowAutoroof;
			this.preventDeteriorationOnTop = p.preventDeteriorationOnTop;
			this.preventDeteriorationInside = p.preventDeteriorationInside;
			this.isMealSource = p.isMealSource;
			this.isNaturalRock = p.isNaturalRock;
			this.isResourceRock = p.isResourceRock;
			this.repairable = p.repairable;
			this.roofCollapseDamageMultiplier = p.roofCollapseDamageMultiplier;
			this.hasFuelingPort = p.hasFuelingPort;
			this.isPlayerEjectable = p.isPlayerEjectable;
			this.bed_healPerDay = p.bed_healPerDay;
			this.bed_defaultMedical = p.bed_defaultMedical;
			this.bed_showSleeperBody = p.bed_showSleeperBody;
			this.bed_humanlike = p.bed_humanlike;
			this.bed_maxBodySize = p.bed_maxBodySize;
			this.bed_caravansCanUse = p.bed_caravansCanUse;
			this.nutritionCostPerDispense = p.nutritionCostPerDispense;
			this.turretBurstWarmupTime = p.turretBurstWarmupTime;
			this.turretBurstCooldownTime = p.turretBurstCooldownTime;
			//this.turretTopGraphicPath = p.turretTopGraphicPath;
			this.turretTopDrawSize = p.turretTopDrawSize;
			this.turretTopOffset = new Vector2Stats(p.turretTopOffset);
			this.ai_combatDangerous = p.ai_combatDangerous;
			this.ai_chillDestination = p.ai_chillDestination;
			this.mineableYield = p.mineableYield;
			this.mineableNonMinedEfficiency = p.mineableNonMinedEfficiency;
			this.mineableDropChance = p.mineableDropChance;
			this.mineableYieldWasteable = p.mineableYieldWasteable;
			this.mineableScatterCommonality = p.mineableScatterCommonality;
			this.ignoreStoredThingsBeauty = p.ignoreStoredThingsBeauty;
			this.isTrap = p.isTrap;
			this.trapDestroyOnSpring = p.trapDestroyOnSpring;
			this.trapPeacefulWildAnimalsSpringChanceFactor = p.trapPeacefulWildAnimalsSpringChanceFactor;
			this.unpoweredWorkTableWorkSpeedFactor = p.unpoweredWorkTableWorkSpeedFactor;
			this.workSpeedPenaltyOutdoors = p.workSpeedPenaltyOutdoors;
			this.workSpeedPenaltyTemperature = p.workSpeedPenaltyTemperature;
			this.watchBuildingStandRectWidth = p.watchBuildingStandRectWidth;
			this.haulToContainerDuration = p.haulToContainerDuration;

			if (p.mineableScatterLumpSizeRange != null)
				this.mineableScatterLumpSizeRange = new MinMaxIntStats(p.mineableScatterLumpSizeRange);
			if (p.watchBuildingStandDistanceRange != null)
				this.watchBuildingStandDistanceRange = new MinMaxIntStats(p.watchBuildingStandDistanceRange);
			if (p.blueprintGraphicData != null)
				this.blueprintGraphicData = new GraphicDataStats(p.blueprintGraphicData);
			if (p.trapUnarmedGraphicData != null)
				this.trapUnarmedGraphicData = new GraphicDataStats(p.trapUnarmedGraphicData);
			if (p.fullGraveGraphicData != null)
				this.fullGraveGraphicData = new GraphicDataStats(p.fullGraveGraphicData);

			Util.AssignDefStat(p.naturalTerrain, out this.naturalTerrain);
			Util.AssignDefStat(p.leaveTerrain, out this.leaveTerrain);
			Util.AssignDefStat(p.smoothedThing, out this.smoothedThing);
			Util.AssignDefStat(p.unsmoothedThing, out this.unsmoothedThing);
			Util.AssignDefStat(p.turretGunDef, out this.turretGunDef);
			Util.AssignDefStat(p.defaultPlantToGrow, out this.defaultPlantToGrow);
			Util.AssignDefStat(p.mineableThing, out this.mineableThing);
			Util.AssignDefStat(p.soundDispense, out this.soundDispense);
			Util.AssignDefStat(p.soundDoorOpenPowered, out this.soundDoorOpenPowered);
			Util.AssignDefStat(p.soundDoorClosePowered, out this.soundDoorClosePowered);
			Util.AssignDefStat(p.soundDoorOpenManual, out this.soundDoorOpenManual);
			Util.AssignDefStat(p.soundDoorCloseManual, out this.soundDoorCloseManual);
			Util.AssignDefStat(p.soundAmbient, out this.soundAmbient);
			Util.AssignDefStat(p.trapDamageCategory, out this.trapDamageCategory);
			Util.AssignDefStat(p.joyKind, out this.joyKind);
			Util.AssignDefStat(p.spawnedConceptLearnOpportunity, out this.spawnedConceptLearnOpportunity);
			Util.AssignDefStat(p.boughtConceptLearnOpportunity, out this.boughtConceptLearnOpportunity);

			Util.Populate(out this.buildingTags, p.buildingTags);
		}

		public static bool GetDeconstructible(BuildingProperties p)
		{
			return (bool)typeof(BuildingProperties).GetField("deconstructible", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(p);
		}

		public static void SetDeconstructible(BuildingProperties p, bool b)
		{
			typeof(BuildingProperties).GetField("deconstructible", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(p, b);
		}
	}
}
