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

		public MinMaxFloatStats visualSizeRange;

		public DefStat<ThingDef> harvestedThingDef;
		public DefStat<SoundDef> soundHarvesting;
		public DefStat<SoundDef> soundHarvestFinish;

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
			this.leaflessGraphicPath = GetLeaflessGraphicPath(p);
			this.immatureGraphicPath = GetImmatureGraphicPath(p);
			this.dropLeaves = p.dropLeaves;

			if (p.visualSizeRange != null)
				this.visualSizeRange = new MinMaxFloatStats(p.visualSizeRange);

			Util.AssignDefStat(p.harvestedThingDef, out this.harvestedThingDef);
			Util.AssignDefStat(p.soundHarvesting, out this.soundHarvesting);
			Util.AssignDefStat(p.soundHarvestFinish, out this.soundHarvestFinish);

			Util.Populate(out this.sowTags, p.sowTags);
			Util.Populate(out this.wildBiomes, p.wildBiomes, (v) => new FloatValueDefStat<BiomeDef>(v.biome, v.commonality));
			this.sowResearchPrerequisites = Util.CreateDefStatList(p.sowResearchPrerequisites);
		}

		public static string GetLeaflessGraphicPath(PlantProperties p)
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
		}
	}
}
