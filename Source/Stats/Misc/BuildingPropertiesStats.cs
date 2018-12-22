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
		//public string turretTopGraphicPath;

		public bool isEdifice;
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

		public List<string> buildingTags;

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

		public void ApplyStats(BuildingProperties p)
		{
			p.isEdifice = this.isEdifice;
			p.isInert = this.isInert;
			SetDeconstructible(p, this.deconstructible);
			p.alwaysDeconstructible = this.alwaysDeconstructible;
			p.claimable = this.claimable;
			p.isSittable = this.isSittable;
			p.expandHomeArea = this.expandHomeArea;
			p.uninstallWork = this.uninstallWork;
			p.wantsHopperAdjacent = this.wantsHopperAdjacent;
			p.allowWireConnection = this.allowWireConnection;
			p.shipPart = this.shipPart;
			p.canPlaceOverImpassablePlant = this.canPlaceOverImpassablePlant;
			p.heatPerTickWhileWorking = this.heatPerTickWhileWorking;
			p.canBuildNonEdificesUnder = this.canBuildNonEdificesUnder;
			p.canPlaceOverWall = this.canPlaceOverImpassablePlant;
			p.allowAutoroof = this.allowAutoroof;
			p.preventDeteriorationOnTop = this.preventDeteriorationOnTop;
			p.preventDeteriorationInside = this.preventDeteriorationInside;
			p.isMealSource = this.isMealSource;
			p.isNaturalRock = this.isNaturalRock;
			p.isResourceRock = this.isResourceRock;
			p.repairable = this.repairable;
			p.roofCollapseDamageMultiplier = this.roofCollapseDamageMultiplier;
			p.hasFuelingPort = this.hasFuelingPort;
			p.isPlayerEjectable = this.isPlayerEjectable;
			p.bed_healPerDay = this.bed_healPerDay;
			p.bed_defaultMedical = this.bed_defaultMedical;
			p.bed_showSleeperBody = this.bed_showSleeperBody;
			p.bed_humanlike = this.bed_humanlike;
			p.bed_maxBodySize = this.bed_maxBodySize;
			p.bed_caravansCanUse = this.bed_caravansCanUse;
			p.nutritionCostPerDispense = this.nutritionCostPerDispense;
			p.turretBurstWarmupTime = this.turretBurstWarmupTime;
			p.turretBurstCooldownTime = this.turretBurstCooldownTime;
			p.turretTopDrawSize = this.turretTopDrawSize;
			p.turretTopOffset = this.turretTopOffset.ToVector2();
			p.ai_combatDangerous = this.ai_combatDangerous;
			p.ai_chillDestination = this.ai_chillDestination;
			p.mineableYield = this.mineableYield;
			p.mineableNonMinedEfficiency = this.mineableNonMinedEfficiency;
			p.mineableDropChance = this.mineableDropChance;
			p.mineableYieldWasteable = this.mineableYieldWasteable;
			p.mineableScatterCommonality = this.mineableScatterCommonality;
			p.ignoreStoredThingsBeauty = this.ignoreStoredThingsBeauty;
			p.isTrap = this.isTrap;
			p.trapDestroyOnSpring = this.trapDestroyOnSpring;
			p.trapPeacefulWildAnimalsSpringChanceFactor = this.trapPeacefulWildAnimalsSpringChanceFactor;
			p.unpoweredWorkTableWorkSpeedFactor = this.unpoweredWorkTableWorkSpeedFactor;
			p.workSpeedPenaltyOutdoors = this.workSpeedPenaltyOutdoors;
			p.workSpeedPenaltyTemperature = this.workSpeedPenaltyTemperature;
			p.watchBuildingStandRectWidth = this.watchBuildingStandRectWidth;
			p.haulToContainerDuration = this.haulToContainerDuration;

			if (this.mineableScatterLumpSizeRange != null && p.mineableScatterLumpSizeRange != null)
				p.mineableScatterLumpSizeRange = this.mineableScatterLumpSizeRange.ToIntRange();
			if (this.watchBuildingStandDistanceRange != null && p.watchBuildingStandDistanceRange != null)
				p.watchBuildingStandDistanceRange = this.watchBuildingStandDistanceRange.ToIntRange();
			// TODO
			//if (this.blueprintGraphicData != null && p.blueprintGraphicData != null)
			//this.blueprintGraphicData = this.blueprintGraphicData.app
			//if (this.trapUnarmedGraphicData != null && p.trapUnarmedGraphicData != null)
			//	this.trapUnarmedGraphicData = new GraphicDataStats(p.trapUnarmedGraphicData);
			//if (this.fullGraveGraphicData != null && p.fullGraveGraphicData != null)
			//	this.fullGraveGraphicData = new GraphicDataStats(p.fullGraveGraphicData);

			Util.AssignDef(this.naturalTerrain, out p.naturalTerrain);
			Util.AssignDef(this.leaveTerrain, out p.leaveTerrain);
			Util.AssignDef(this.smoothedThing, out p.smoothedThing);
			Util.AssignDef(this.unsmoothedThing, out p.unsmoothedThing);
			Util.AssignDef(this.turretGunDef, out p.turretGunDef);
			Util.AssignDef(this.defaultPlantToGrow, out p.defaultPlantToGrow);
			Util.AssignDef(this.mineableThing, out p.mineableThing);
			Util.AssignDef(this.soundDispense, out p.soundDispense);
			Util.AssignDef(this.soundDoorOpenPowered, out p.soundDoorOpenPowered);
			Util.AssignDef(this.soundDoorClosePowered, out p.soundDoorClosePowered);
			Util.AssignDef(this.soundDoorOpenManual, out p.soundDoorOpenManual);
			Util.AssignDef(this.soundDoorCloseManual, out p.soundDoorCloseManual);
			Util.AssignDef(this.soundAmbient, out p.soundAmbient);
			Util.AssignDef(this.trapDamageCategory, out p.trapDamageCategory);
			Util.AssignDef(this.joyKind, out p.joyKind);
			Util.AssignDef(this.spawnedConceptLearnOpportunity, out p.spawnedConceptLearnOpportunity);
			Util.AssignDef(this.boughtConceptLearnOpportunity, out p.boughtConceptLearnOpportunity);

			Util.Populate(out p.buildingTags, this.buildingTags);
		}

		public bool Initialize()
		{
			Util.InitializeDefStat(this.naturalTerrain);
			Util.InitializeDefStat(this.leaveTerrain);
			Util.InitializeDefStat(this.smoothedThing);
			Util.InitializeDefStat(this.unsmoothedThing);
			Util.InitializeDefStat(this.turretGunDef);
			Util.InitializeDefStat(this.defaultPlantToGrow);
			Util.InitializeDefStat(this.mineableThing);
			Util.InitializeDefStat(this.soundDispense);
			Util.InitializeDefStat(this.soundDoorOpenPowered);
			Util.InitializeDefStat(this.soundDoorClosePowered);
			Util.InitializeDefStat(this.soundDoorOpenManual);
			Util.InitializeDefStat(this.soundDoorCloseManual);
			Util.InitializeDefStat(this.soundAmbient);
			Util.InitializeDefStat(this.trapDamageCategory);
			Util.InitializeDefStat(this.joyKind);
			Util.InitializeDefStat(this.spawnedConceptLearnOpportunity);
			Util.InitializeDefStat(this.boughtConceptLearnOpportunity);
			return true;
		}

		public override string ToString()
		{
			return
				"\nisEdifice: " + this.isEdifice +
				"\nisInert: " + this.isInert +
				"\ndeconstructible: " + this.deconstructible +
				"\nalwaysDeconstructible: " + this.alwaysDeconstructible +
				"\nclaimable: " + this.claimable +
				"\nisSittable: " + this.isSittable +
				"\nexpandHomeArea: " + this.expandHomeArea +
				"\nuninstallWork: " + this.uninstallWork +
				"\nwantsHopperAdjacent: " + this.wantsHopperAdjacent +
				"\nallowWireConnection: " + this.allowWireConnection +
				"\nshipPart: " + this.shipPart +
				"\ncanPlaceOverImpassablePlant: " + this.canPlaceOverImpassablePlant +
				"\nheatPerTickWhileWorking: " + this.heatPerTickWhileWorking +
				"\ncanBuildNonEdificesUnder: " + this.canBuildNonEdificesUnder +
				"\ncanPlaceOverWall: " + this.canPlaceOverImpassablePlant +
				"\nallowAutoroof: " + this.allowAutoroof +
				"\npreventDeteriorationOnTop: " + this.preventDeteriorationOnTop +
				"\npreventDeteriorationInside: " + this.preventDeteriorationInside +
				"\nisMealSource: " + this.isMealSource +
				"\nisNaturalRock: " + this.isNaturalRock +
				"\nisResourceRock: " + this.isResourceRock +
				"\nrepairable: " + this.repairable +
				"\nroofCollapseDamageMultiplier: " + this.roofCollapseDamageMultiplier +
				"\nhasFuelingPort: " + this.hasFuelingPort +
				"\nisPlayerEjectable: " + this.isPlayerEjectable +
				"\nbed_healPerDay: " + this.bed_healPerDay +
				"\nbed_defaultMedical: " + this.bed_defaultMedical +
				"\nbed_showSleeperBody: " + this.bed_showSleeperBody +
				"\nbed_humanlike: " + this.bed_humanlike +
				"\nbed_maxBodySize: " + this.bed_maxBodySize +
				"\nbed_caravansCanUse: " + this.bed_caravansCanUse +
				"\nnutritionCostPerDispense: " + this.nutritionCostPerDispense +
				"\nturretBurstWarmupTime: " + this.turretBurstWarmupTime +
				"\nturretBurstCooldownTime: " + this.turretBurstCooldownTime +
				"\nturretTopDrawSize: " + this.turretTopDrawSize +
				"\nturretTopOffset: " + this.turretTopOffset.ToString() +
				"\nai_combatDangerous: " + this.ai_combatDangerous +
				"\nai_chillDestination: " + this.ai_chillDestination +
				"\nmineableYield: " + this.mineableYield +
				"\nmineableNonMinedEfficiency: " + this.mineableNonMinedEfficiency +
				"\nmineableDropChance: " + this.mineableDropChance +
				"\nmineableYieldWasteable: " + this.mineableYieldWasteable +
				"\nmineableScatterCommonality: " + this.mineableScatterCommonality +
				"\nignoreStoredThingsBeauty: " + this.ignoreStoredThingsBeauty +
				"\nisTrap: " + this.isTrap +
				"\ntrapDestroyOnSpring: " + this.trapDestroyOnSpring +
				"\ntrapPeacefulWildAnimalsSpringChanceFactor: " + this.trapPeacefulWildAnimalsSpringChanceFactor +
				"\nunpoweredWorkTableWorkSpeedFactor: " + this.unpoweredWorkTableWorkSpeedFactor +
				"\nworkSpeedPenaltyOutdoors: " + this.workSpeedPenaltyOutdoors +
				"\nworkSpeedPenaltyTemperature: " + this.workSpeedPenaltyTemperature +
				"\nwatchBuildingStandRectWidth: " + this.watchBuildingStandRectWidth +
				"\nhaulToContainerDuration: " + this.haulToContainerDuration +
				"\nmineableScatterLumpSizeRange: " + Util.ToString(this.mineableScatterLumpSizeRange) +
				"\nwatchBuildingStandDistanceRange: " + Util.ToString(this.watchBuildingStandDistanceRange) +
				"\blueprintGraphicData: " + Util.ToString(this.blueprintGraphicData) +
				"\ntrapUnarmedGraphicData: " + Util.ToString(this.trapUnarmedGraphicData) +
				"\nfullGraveGraphicData: " + Util.ToString(this.fullGraveGraphicData) +
				"\nnaturalTerrain: " + Util.ToString(this.naturalTerrain) +
				"\nleaveTerrain: " + Util.ToString(this.leaveTerrain) +
				"\nsmoothedThing: " + Util.ToString(this.smoothedThing) +
				"\nunsmoothedThing: " + Util.ToString(this.unsmoothedThing) +
				"\nturretGunDef: " + Util.ToString(this.turretGunDef) +
				"\ndefaultPlantToGrow: " + Util.ToString(this.defaultPlantToGrow) +
				"\nmineableThing: " + Util.ToString(this.mineableThing) +
				"\nsoundDispense: " + Util.ToString(this.soundDispense) +
				"\nsoundDoorOpenPowered: " + Util.ToString(this.soundDoorOpenPowered) +
				"\nsoundDoorClosePowered: " + Util.ToString(this.soundDoorClosePowered) +
				"\nsoundDoorOpenManual: " + Util.ToString(this.soundDoorOpenManual) +
				"\nsoundDoorCloseManual: " + Util.ToString(this.soundDoorCloseManual) +
				"\nsoundAmbient: " + Util.ToString(this.soundAmbient) +
				"\ntrapDamageCategory: " + Util.ToString(this.trapDamageCategory) +
				"\njoyKind: " + Util.ToString(this.joyKind) +
				"\nspawnedConceptLearnOpportunity: " + Util.ToString(this.spawnedConceptLearnOpportunity) +
				"\nboughtConceptLearnOpportunity: " + Util.ToString(this.boughtConceptLearnOpportunity) +
				"\nbuildingTags: " + ((this.buildingTags != null) ? string.Join(", ", this.buildingTags.ToArray()) : "null");
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is BuildingPropertiesStats s)
			{
				return
					this.isEdifice == s.isEdifice &&
					this.isInert == s.isInert &&
					this.deconstructible == s.deconstructible &&
					this.alwaysDeconstructible == s.alwaysDeconstructible &&
					this.claimable == s.claimable &&
					this.isSittable == s.isSittable &&
					this.expandHomeArea == s.expandHomeArea &&
					this.uninstallWork == s.uninstallWork &&
					this.wantsHopperAdjacent == s.wantsHopperAdjacent &&
					this.allowWireConnection == s.allowWireConnection &&
					this.shipPart == s.shipPart &&
					this.canPlaceOverImpassablePlant == s.canPlaceOverImpassablePlant &&
					this.heatPerTickWhileWorking == s.heatPerTickWhileWorking &&
					this.canBuildNonEdificesUnder == s.canBuildNonEdificesUnder &&
					this.canPlaceOverWall == s.canPlaceOverImpassablePlant &&
					this.allowAutoroof == s.allowAutoroof &&
					this.preventDeteriorationOnTop == s.preventDeteriorationOnTop &&
					this.preventDeteriorationInside == s.preventDeteriorationInside &&
					this.isMealSource == s.isMealSource &&
					this.isNaturalRock == s.isNaturalRock &&
					this.isResourceRock == s.isResourceRock &&
					this.repairable == s.repairable &&
					this.roofCollapseDamageMultiplier == s.roofCollapseDamageMultiplier &&
					this.hasFuelingPort == s.hasFuelingPort &&
					this.isPlayerEjectable == s.isPlayerEjectable &&
					this.bed_healPerDay == s.bed_healPerDay &&
					this.bed_defaultMedical == s.bed_defaultMedical &&
					this.bed_showSleeperBody == s.bed_showSleeperBody &&
					this.bed_humanlike == s.bed_humanlike &&
					this.bed_maxBodySize == s.bed_maxBodySize &&
					this.bed_caravansCanUse == s.bed_caravansCanUse &&
					this.nutritionCostPerDispense == s.nutritionCostPerDispense &&
					this.turretBurstWarmupTime == s.turretBurstWarmupTime &&
					this.turretBurstCooldownTime == s.turretBurstCooldownTime &&
					this.turretTopDrawSize == s.turretTopDrawSize &&
					object.Equals(this.turretTopOffset, s.turretTopOffset) &&
					this.ai_combatDangerous == s.ai_combatDangerous &&
					this.ai_chillDestination == s.ai_chillDestination &&
					this.mineableYield == s.mineableYield &&
					this.mineableNonMinedEfficiency == s.mineableNonMinedEfficiency &&
					this.mineableDropChance == s.mineableDropChance &&
					this.mineableYieldWasteable == s.mineableYieldWasteable &&
					this.mineableScatterCommonality == s.mineableScatterCommonality &&
					this.ignoreStoredThingsBeauty == s.ignoreStoredThingsBeauty &&
					this.isTrap == s.isTrap &&
					this.trapDestroyOnSpring == s.trapDestroyOnSpring &&
					this.trapPeacefulWildAnimalsSpringChanceFactor == s.trapPeacefulWildAnimalsSpringChanceFactor &&
					this.unpoweredWorkTableWorkSpeedFactor == s.unpoweredWorkTableWorkSpeedFactor &&
					this.workSpeedPenaltyOutdoors == s.workSpeedPenaltyOutdoors &&
					this.workSpeedPenaltyTemperature == s.workSpeedPenaltyTemperature &&
					this.watchBuildingStandRectWidth == s.watchBuildingStandRectWidth &&
					this.haulToContainerDuration == s.haulToContainerDuration &&
					object.Equals(this.mineableScatterLumpSizeRange, s.mineableScatterLumpSizeRange) &&
					object.Equals(this.watchBuildingStandDistanceRange, s.watchBuildingStandDistanceRange) &&
					// TODO
					//object.Equals(this.blueprintGraphicData, s.blueprintGraphicData) &&
					//object.Equals(this.trapUnarmedGraphicData, s.trapUnarmedGraphicData) &&
					//object.Equals(this.fullGraveGraphicData, s.fullGraveGraphicData) &&
					object.Equals(this.naturalTerrain, s.naturalTerrain) &&
					object.Equals(this.leaveTerrain, s.leaveTerrain) &&
					object.Equals(this.smoothedThing, s.smoothedThing) &&
					object.Equals(this.unsmoothedThing, s.unsmoothedThing) &&
					object.Equals(this.turretGunDef, s.turretGunDef) &&
					object.Equals(this.defaultPlantToGrow, s.defaultPlantToGrow) &&
					object.Equals(this.mineableThing, s.mineableThing) &&
					object.Equals(this.soundDispense, s.soundDispense) &&
					object.Equals(this.soundDoorOpenPowered, s.soundDoorOpenPowered) &&
					object.Equals(this.soundDoorClosePowered, s.soundDoorClosePowered) &&
					object.Equals(this.soundDoorOpenManual, s.soundDoorOpenManual) &&
					object.Equals(this.soundDoorCloseManual, s.soundDoorCloseManual) &&
					object.Equals(this.soundAmbient, s.soundAmbient) &&
					object.Equals(this.trapDamageCategory, s.trapDamageCategory) &&
					object.Equals(this.joyKind, s.joyKind) &&
					object.Equals(this.spawnedConceptLearnOpportunity, s.spawnedConceptLearnOpportunity) &&
					object.Equals(this.boughtConceptLearnOpportunity, s.boughtConceptLearnOpportunity) &&
					Util.AreEqual(this.buildingTags, s.buildingTags, v => v.GetHashCode());
			}
			return false;
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