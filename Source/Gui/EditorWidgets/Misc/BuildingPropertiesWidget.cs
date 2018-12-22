using InGameDefEditor.Stats.Misc;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using System;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	class BuildingPropertiesWidget : IInputWidget
	{
		private readonly BuildingProperties props;

		private readonly List<IInputWidget> inputWidgets;

		public string DisplayLabel => "Building Properties";

		public BuildingPropertiesWidget(BuildingProperties props)
		{
			this.props = props;
			this.inputWidgets = new List<IInputWidget>();
			if (props.mineableThing != null)
			{
				this.inputWidgets.Add(new IntInputWidget<BuildingProperties>(this.props, "Mineable - Yield", p => p.mineableYield, (p, v) => p.mineableYield = v));
				this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Mineable - Non Mined Efficiency", p => p.mineableNonMinedEfficiency, (p, v) => p.mineableNonMinedEfficiency = v));
				this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Mineable - Drop Chance", p => p.mineableDropChance, (p, v) => p.mineableDropChance = v));
				this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Mineable - Yield Wasteable", p => p.mineableYieldWasteable, (p, v) => p.mineableYieldWasteable = v));
				this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Mineable - Scatter Commonality", p => p.mineableScatterCommonality, (p, v) => p.mineableScatterCommonality = v));
				this.inputWidgets.Add(new DefInputWidget<BuildingProperties, ThingDef>(this.props, "Mineable - Thing", 200, p => p.mineableThing, (p, v) => p.mineableThing = v, true));
				this.inputWidgets.Add(new MinMaxInputWidget<BuildingProperties, int>("Mineable - Scatter Lump Size Range",
					new IntInputWidget<BuildingProperties>(this.props, "Min", p => p.mineableScatterLumpSizeRange.min, (p, v) => p.mineableScatterLumpSizeRange.min = v),
					new IntInputWidget<BuildingProperties>(this.props, "Max", p => p.mineableScatterLumpSizeRange.max, (p, v) => p.mineableScatterLumpSizeRange.max = v)));
			}
			else if (props.isTrap)
			{
				this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Is Trap", p => p.isTrap, (p, v) => p.isTrap = v));
				this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Trap - Destroy On Spring", p => p.trapDestroyOnSpring, (p, v) => p.trapDestroyOnSpring = v));
				this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Trap - Peaceful Wild Animals Spring Chance Factor", p => p.trapPeacefulWildAnimalsSpringChanceFactor, (p, v) => p.trapPeacefulWildAnimalsSpringChanceFactor = v));
				if (props.trapDamageCategory != null)
					this.inputWidgets.Add(new DefInputWidget<BuildingProperties, DamageArmorCategoryDef>(this.props, "Trap - Damage Category", 200, p => p.trapDamageCategory, (p, v) => p.trapDamageCategory = v, true));
			}

			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Is Edifice", p => p.isEdifice, (p, v) => p.isEdifice = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Is Inert", p => p.isInert, (p, v) => p.isInert = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Deconstructible", p => BuildingPropertiesStats.GetDeconstructible(p), (p, v) => BuildingPropertiesStats.SetDeconstructible(p, v)));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Always Deconstructible", p => p.alwaysDeconstructible, (p, v) => p.alwaysDeconstructible = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Claimable", p => p.claimable, (p, v) => p.claimable = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Is Sittable", p => p.isSittable, (p, v) => p.isSittable = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "ExpandHomeArea", p => p.expandHomeArea, (p, v) => p.expandHomeArea = v));
			this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Uninstall Work", p => p.uninstallWork, (p, v) => p.uninstallWork = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Wants Hopper Adjacent", p => p.wantsHopperAdjacent, (p, v) => p.wantsHopperAdjacent = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Allow Wire Connection", p => p.allowWireConnection, (p, v) => p.allowWireConnection = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Ship Part", p => p.shipPart, (p, v) => p.shipPart = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Can Place Over Impassable Plant", p => p.canPlaceOverImpassablePlant, (p, v) => p.canPlaceOverImpassablePlant = v));
			this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Heat Per Tick While Working", p => p.heatPerTickWhileWorking, (p, v) => p.heatPerTickWhileWorking = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Can Build Non Edifices Under", p => p.canBuildNonEdificesUnder, (p, v) => p.canBuildNonEdificesUnder = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Can Place Over Wall", p => p.canPlaceOverWall, (p, v) => p.canPlaceOverWall = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Allow Auto-Roof", p => p.allowAutoroof, (p, v) => p.allowAutoroof = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Prevent Deterioration On Top", p => p.preventDeteriorationOnTop, (p, v) => p.preventDeteriorationOnTop = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Prevent Deterioration Inside", p => p.preventDeteriorationInside, (p, v) => p.preventDeteriorationInside = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Is Meal Source", p => p.isMealSource, (p, v) => p.isMealSource = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Is Natural Rock", p => p.isNaturalRock, (p, v) => p.isNaturalRock = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Is Resource Rock", p => p.isResourceRock, (p, v) => p.isResourceRock = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Repairable", p => p.repairable, (p, v) => p.repairable = v));
			this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Roof Collapse Damage Multiplier", p => p.roofCollapseDamageMultiplier, (p, v) => p.roofCollapseDamageMultiplier = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Has Fueling Port", p => p.hasFuelingPort, (p, v) => p.hasFuelingPort = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Is Player Ejectable", p => p.isPlayerEjectable, (p, v) => p.isPlayerEjectable = v));
			this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Bed - Heal Per Day", p => p.bed_healPerDay, (p, v) => p.bed_healPerDay = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Bed - Default Medical", p => p.bed_defaultMedical, (p, v) => p.bed_defaultMedical = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Bed - Show Sleeper Body", p => p.bed_showSleeperBody, (p, v) => p.bed_showSleeperBody = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Bed - Humanlike", p => p.bed_humanlike, (p, v) => p.bed_humanlike = v));
			this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Bed - Max Body Size", p => p.bed_maxBodySize, (p, v) => p.bed_maxBodySize = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Bed - Caravans Can Use", p => p.bed_caravansCanUse, (p, v) => p.bed_caravansCanUse = v));
			this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Nutrition Cost Per Dispense", p => p.nutritionCostPerDispense, (p, v) => p.nutritionCostPerDispense = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "AI - Combat Dangerous", p => p.ai_combatDangerous, (p, v) => p.ai_combatDangerous = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "AI - Chill Destination", p => p.ai_chillDestination, (p, v) => p.ai_chillDestination = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Ignore Stored Things Beauty", p => p.ignoreStoredThingsBeauty, (p, v) => p.ignoreStoredThingsBeauty = v));
			this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Unpowered Work Table Work Speed Factor", p => p.unpoweredWorkTableWorkSpeedFactor, (p, v) => p.unpoweredWorkTableWorkSpeedFactor = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Work Speed Penalty Outdoors", p => p.workSpeedPenaltyOutdoors, (p, v) => p.workSpeedPenaltyOutdoors = v));
			this.inputWidgets.Add(new BoolInputWidget<BuildingProperties>(this.props, "Work Speed Penalty Temperature", p => p.workSpeedPenaltyTemperature, (p, v) => p.workSpeedPenaltyTemperature = v));
			this.inputWidgets.Add(new IntInputWidget<BuildingProperties>(this.props, "Watch Building Stand Rect Width", p => p.watchBuildingStandRectWidth, (p, v) => p.watchBuildingStandRectWidth = v));
			this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Turret - Burst Warmup Time", p => p.turretBurstWarmupTime, (p, v) => p.turretBurstWarmupTime = v));
			this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Turret - Burst Cooldown Time", p => p.turretBurstCooldownTime, (p, v) => p.turretBurstCooldownTime = v));
			this.inputWidgets.Add(new FloatInputWidget<BuildingProperties>(this.props, "Turret - Top Draw Size", p => p.turretTopDrawSize, (p, v) => p.turretTopDrawSize = v));
			this.inputWidgets.Add(new MinMaxInputWidget<BuildingProperties, float>("Turret - Top Offset",
				new FloatInputWidget<BuildingProperties>(this.props, "X", p => p.turretTopOffset.x, (p, v) => p.turretTopOffset.x = v),
				new FloatInputWidget<BuildingProperties>(this.props, "Y", p => p.turretTopOffset.y, (p, v) => p.turretTopOffset.y = v)));
			if (props.turretGunDef != null)
				this.inputWidgets.Add(new DefInputWidget<BuildingProperties, ThingDef>(this.props, "Turret - Gun Def", 200, p => p.turretGunDef, (p, v) => p.turretGunDef = v, true));
			this.inputWidgets.Add(new IntInputWidget<BuildingProperties>(this.props, "Haul To Container Duration", p => p.haulToContainerDuration, (p, v) => p.haulToContainerDuration = v));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, TerrainDef>(this.props, "Natural Terrain", 200, p => p.naturalTerrain, (p, v) => p.naturalTerrain = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, TerrainDef>(this.props, "Leave Terrain", 200, p => p.leaveTerrain, (p, v) => p.leaveTerrain = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, ThingDef>(this.props, "Smoothed Thing", 200, p => p.smoothedThing, (p, v) => p.smoothedThing = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, ThingDef>(this.props, "Unsmoothed Thing", 200, p => p.unsmoothedThing, (p, v) => p.unsmoothedThing = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, ThingDef>(this.props, "Default Plant To Grow", 200, p => p.defaultPlantToGrow, (p, v) => p.defaultPlantToGrow = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, SoundDef>(this.props, "Sound Dispense", 200, p => p.soundDispense, (p, v) => p.soundDispense = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, SoundDef>(this.props, "Sound Door Open Powered", 200, p => p.soundDoorOpenPowered, (p, v) => p.soundDoorOpenPowered = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, SoundDef>(this.props, "Sound Door Close Powered", 200, p => p.soundDoorClosePowered, (p, v) => p.soundDoorClosePowered = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, SoundDef>(this.props, "Sound Door Open Manual", 200, p => p.soundDoorOpenManual, (p, v) => p.soundDoorOpenManual = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, SoundDef>(this.props, "Sound Door Close Manual", 200, p => p.soundDoorCloseManual, (p, v) => p.soundDoorCloseManual = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, SoundDef>(this.props, "Sound Ambient", 200, p => p.soundAmbient, (p, v) => p.soundAmbient = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, ConceptDef>(this.props, "Spawned Concept Learn Opportunity", 200, p => p.spawnedConceptLearnOpportunity, (p, v) => p.spawnedConceptLearnOpportunity = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, ConceptDef>(this.props, "Bought Concept Learn Opportunity", 200, p => p.boughtConceptLearnOpportunity, (p, v) => p.boughtConceptLearnOpportunity = v, true));
			this.inputWidgets.Add(new DefInputWidget<BuildingProperties, JoyKindDef>(this.props, "Joy Kind", 200, p => p.joyKind, (p, v) => p.joyKind = v, true));

			// TODO List<string> buildingTags", p => p., (p, v) => p. = v),
		}

		public void Draw(float x, ref float y, float width)
		{
			foreach (var v in this.inputWidgets)
				v.Draw(x, ref y, width);
		}

		public void ResetBuffers()
		{
			this.inputWidgets.ForEach(v => v.ResetBuffers());
		}
	}
}