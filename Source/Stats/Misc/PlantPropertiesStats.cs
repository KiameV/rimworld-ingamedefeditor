using System;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;
using System.Collections.Generic;
using System.Reflection;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class PlantPropertiesStats
	{
		//public Graphic leaflessGraphic;
		//public Graphic immatureGraphic;

		public int wildClusterRadius;
		public float wildClusterWeight;
		public float wildOrder;
		public bool wildEqualLocalDistribution;
		public bool cavePlant;
		public float cavePlantWeight;
		public float sowWork;
		public int sowMinSkill;
		public bool blockAdjacentSow;
		public bool mustBeWildToSow;
		public float harvestWork;
		public float harvestYield;
		public string harvestTag;
		public float harvestMinGrowth;
		public float harvestAfterGrowth;
		public bool harvestFailable;
		public float growDays;
		public float lifespanDaysPerGrowDays;
		public float growMinGlow;
		public float growOptimalGlow;
		public float fertilityMin;
		public float fertilitySensitivity;
		public bool dieIfLeafless;
		public bool neverBlightable;
		public bool interferesWithRoof;
		public PlantPurpose purpose;
		public float topWindExposure;
		public int maxMeshCount;
		public string leaflessGraphicPath;
		public string immatureGraphicPath;
		public bool dropLeaves;

		public bool dieFromToxicFallout;
		public bool autoHarvestable;
		public bool humanFoodPlant;
		public bool treeLoversCareIfChopped;
		public bool allowAutoCut;
		public DefStat<ThingDef> burnedThingDef;
		public bool dieIfNoSunlight;
		public TreeCategory treeCategory;
		public bool showGrowthInInspectPane;
		public float minSpacingBetweenSamePlant;

		public MinMaxFloatStats visualSizeRange;

		public DefStat<ThingDef> harvestedThingDef;
		//public DefStat<SoundDef> soundHarvesting;
		//public DefStat<SoundDef> soundHarvestFinish;

		public List<string> sowTags;
		public List<FloatValueDefStat<BiomeDef>> wildBiomes;
		public List<DefStat<ResearchProjectDef>> sowResearchPrerequisites;

		public PlantPropertiesStats() { }
		public PlantPropertiesStats(PlantProperties p)
		{
			this.wildClusterRadius = p.wildClusterRadius;
			this.wildClusterWeight = p.wildClusterWeight;
			this.wildOrder = p.wildOrder;
			this.wildEqualLocalDistribution = p.wildEqualLocalDistribution;
			this.cavePlant = p.cavePlant;
			this.cavePlantWeight = p.cavePlantWeight;
			this.sowWork = p.sowWork;
			this.sowMinSkill = p.sowMinSkill;
			this.blockAdjacentSow = p.blockAdjacentSow;
			this.mustBeWildToSow = p.mustBeWildToSow;
			this.harvestWork = p.harvestWork;
			this.harvestYield = p.harvestYield;
			this.harvestTag = p.harvestTag;
			this.harvestMinGrowth = p.harvestMinGrowth;
			this.harvestAfterGrowth = p.harvestAfterGrowth;
			this.harvestFailable = p.harvestFailable;
			this.growDays = p.growDays;
			this.lifespanDaysPerGrowDays = p.lifespanDaysPerGrowDays;
			this.growMinGlow = p.growMinGlow;
			this.growOptimalGlow = p.growOptimalGlow;
			this.fertilityMin = p.fertilityMin;
			this.fertilitySensitivity = p.fertilitySensitivity;
			this.dieIfLeafless = p.dieIfLeafless;
			this.neverBlightable = p.neverBlightable;
			this.interferesWithRoof = p.interferesWithRoof;
			this.purpose = p.purpose;
			this.topWindExposure = p.topWindExposure;
			this.maxMeshCount = p.maxMeshCount;
			//this.leaflessGraphicPath = GetLeaflessGraphicPath(p);
			//this.immatureGraphicPath = GetImmatureGraphicPath(p);
			this.dropLeaves = p.dropLeaves;
			this.dieFromToxicFallout = p.dieFromToxicFallout;
			this.autoHarvestable = p.autoHarvestable;
			this.humanFoodPlant = p.humanFoodPlant;
			this.treeLoversCareIfChopped = p.treeLoversCareIfChopped;
			this.allowAutoCut = p.allowAutoCut;
			this.dieIfNoSunlight = p.dieIfNoSunlight;
			this.treeCategory = p.treeCategory;
			this.showGrowthInInspectPane = p.showGrowthInInspectPane;
			this.minSpacingBetweenSamePlant = p.minSpacingBetweenSamePlant;

			if (p.visualSizeRange != null)
				this.visualSizeRange = new MinMaxFloatStats(p.visualSizeRange);

			Util.AssignDefStat(p.harvestedThingDef, out this.harvestedThingDef);
			//Util.AssignDefStat(p.soundHarvesting, out this.soundHarvesting);
			//Util.AssignDefStat(p.soundHarvestFinish, out this.soundHarvestFinish);
			Util.AssignDefStat(p.burnedThingDef, out this.burnedThingDef);

			Util.Populate(out this.sowTags, p.sowTags);
			Util.Populate(out this.wildBiomes, p.wildBiomes, (v) => new FloatValueDefStat<BiomeDef>(v.biome, v.commonality));
			this.sowResearchPrerequisites = Util.CreateDefStatList(p.sowResearchPrerequisites);
		}

		/*public static string GetLeaflessGraphicPath(PlantProperties p)
		{
			return (string)typeof(PlantProperties).GetField("leaflessGraphicPath", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(p);
		}

		public static void SetLeaflessGraphicPath(PlantProperties p, string s)
		{
			typeof(PlantProperties).GetField("leaflessGraphicPath", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(p, s);
		}

		public static string GetImmatureGraphicPath(PlantProperties p)
		{
			return (string)typeof(PlantProperties).GetField("immatureGraphicPath", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(p);
		}

		public static void SetImmatureGraphicPath(PlantProperties p, string s)
		{
			typeof(PlantProperties).GetField("immatureGraphicPath", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(p, s);
		}*/

        public void ApplyStats(PlantProperties to)
		{
			to.wildClusterRadius = this.wildClusterRadius;
			to.wildClusterWeight = this.wildClusterWeight;
			to.wildOrder = this.wildOrder;
			to.wildEqualLocalDistribution = this.wildEqualLocalDistribution;
			to.cavePlant = this.cavePlant;
			to.cavePlantWeight = this.cavePlantWeight;
			to.sowWork = this.sowWork;
			to.sowMinSkill = this.sowMinSkill;
			to.blockAdjacentSow = this.blockAdjacentSow;
			to.mustBeWildToSow = this.mustBeWildToSow;
			to.harvestWork = this.harvestWork;
			to.harvestYield = this.harvestYield;
			to.harvestTag = this.harvestTag;
			to.harvestMinGrowth = this.harvestMinGrowth;
			to.harvestAfterGrowth = this.harvestAfterGrowth;
			to.harvestFailable = this.harvestFailable;
			to.growDays = this.growDays;
			to.lifespanDaysPerGrowDays = this.lifespanDaysPerGrowDays;
			to.growMinGlow = this.growMinGlow;
			to.growOptimalGlow = this.growOptimalGlow;
			to.fertilityMin = this.fertilityMin;
			to.fertilitySensitivity = this.fertilitySensitivity;
			to.dieIfLeafless = this.dieIfLeafless;
			to.neverBlightable = this.neverBlightable;
			to.interferesWithRoof = this.interferesWithRoof;
			to.purpose = this.purpose;
			to.topWindExposure = this.topWindExposure;
			to.maxMeshCount = this.maxMeshCount;
			//to.leaflessGraphicPath = GetLeaflessGraphicPath(p);
			//to.immatureGraphicPath = GetImmatureGraphicPath(p);
			to.dropLeaves = this.dropLeaves;
			to.dieFromToxicFallout = this.dieFromToxicFallout;
			to.autoHarvestable = this.autoHarvestable;
			to.humanFoodPlant = this.humanFoodPlant;
			to.treeLoversCareIfChopped = this.treeLoversCareIfChopped;
			to.allowAutoCut = this.allowAutoCut;
			to.dieIfNoSunlight = this.dieIfNoSunlight;
			to.treeCategory = this.treeCategory;
			to.showGrowthInInspectPane = this.showGrowthInInspectPane;
			to.minSpacingBetweenSamePlant = this.minSpacingBetweenSamePlant;

			if (this.visualSizeRange != null)
				to.visualSizeRange = this.visualSizeRange.ToFloatRange();

			Util.AssignDef(this.harvestedThingDef, out to.harvestedThingDef);
			//Util.AssignDef(this.soundHarvesting, out to.soundHarvesting);
			//Util.AssignDef(this.soundHarvestFinish, out to.soundHarvestFinish);
			Util.AssignDef(this.burnedThingDef, out to.burnedThingDef);

			Util.Populate(out to.sowTags, this.sowTags);
			Util.Populate(out to.wildBiomes, this.wildBiomes, (v) => new PlantBiomeRecord() { biome = v.Def, commonality = v.value });
			Util.Populate(out to.sowResearchPrerequisites, this.sowResearchPrerequisites, (v) => v.Def, false);
		}

		public bool Initialize()
		{
			//Util.InitializeDefStat(this.parent);
			Util.InitializeDefStat(this.burnedThingDef);
			Util.InitializeDefStat(this.harvestedThingDef);
			//Util.InitializeDefStat(this.soundHarvesting);
			//Util.InitializeDefStat(this.soundHarvestFinish);
			Util.InitializeDefStat(this.sowResearchPrerequisites);
			return true;
		}
	}
}
