using System;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;
using System.Collections.Generic;
using System.Reflection;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class RacePropertiesStats
	{
		//public Type deathActionWorkerClass;
		//private DeathActionWorkerStats deathActionWorkerInt;
		//private PawnKindDef cachedAnyPawnKind;

		public Intelligence intelligence;
		public bool hasGenders;
		public bool needsRest;
		public PawnNameCategory nameCategory;
		public FoodTypeFlags foodType;
		public bool makesFootprints;
		public int executionRange;
		public float lifeExpectancy;
		public bool herdAnimal;
		public bool packAnimal;
		public bool predator;
		public float maxPreyBodySize;
		public float wildness;
		public float petness;
		public float nuzzleMtbHours;
		public float manhunterOnDamageChance;
		public float manhunterOnTameFailChance;
		public bool canBePredatorPrey;
		public bool herdMigrationAllowed;
		public float gestationPeriodDays;
		public float mateMtbHours;
		public float nameOnTameChance;
		public float nameOnNuzzleChance;
		public float baseBodySize;
		public float baseHealthScale;
		public float baseHungerRate;
		public string meatLabel;
		public float meatMarketValue;

		public ColorStats meatColor;
		public ShadowDataStats specialShadowData;
		public MinMaxIntStats soundCallIntervalRange;
		public SimpleCurveStats ageGenerationCurve;
		public SimpleCurveStats litterSizeCurve;

		public List<FloatValueDefStat<BiomeDef>> wildBiomes;
		public List<LifeStageAgeStats> lifeStageAges;
		public List<string> untrainableTags;
		public List<string> trainableTags;

		private DefStat<FleshTypeDef> fleshType;
		private DefStat<ThingDef> bloodDef;
		public DefStat<ThinkTreeDef> thinkTreeMain;
		public DefStat<ThinkTreeDef> thinkTreeConstant;
		public DefStat<BodyDef> body;
		public DefStat<TrainabilityDef> trainability;
		private DefStat<RulePackDef> nameGenerator;
		private DefStat<RulePackDef> nameGeneratorFemale;
		public DefStat<ThingDef> useMeatFrom;
		public DefStat<ThingDef> useLeatherFrom;
		public DefStat<ThingDef> leatherDef;
		public DefStat<SoundDef> soundMeleeHitPawn;
		public DefStat<SoundDef> soundMeleeHitBuilding;
		public DefStat<SoundDef> soundMeleeMiss;
		public DefStat<ThingDef> meatDef;
		public DefStat<ThingDef> corpseDef;
		public List<DefStat<HediffGiverSetDef>> hediffGiverSets;

		public RacePropertiesStats() { }
		public RacePropertiesStats(RaceProperties p)
		{
			this.intelligence = p.intelligence;
			this.hasGenders = p.hasGenders;
			this.needsRest = p.needsRest;
			this.nameCategory = p.nameCategory;
			this.foodType = p.foodType;
			this.makesFootprints = p.makesFootprints;
			this.executionRange = p.executionRange;
			this.lifeExpectancy = p.lifeExpectancy;
			this.herdAnimal = p.herdAnimal;
			this.packAnimal = p.packAnimal;
			this.predator = p.predator;
			this.maxPreyBodySize = p.maxPreyBodySize;
			this.wildness = p.wildness;
			this.petness = p.petness;
			this.nuzzleMtbHours = p.nuzzleMtbHours;
			this.manhunterOnDamageChance = p.manhunterOnDamageChance;
			this.manhunterOnTameFailChance = p.manhunterOnTameFailChance;
			this.canBePredatorPrey = p.canBePredatorPrey;
			this.herdMigrationAllowed = p.herdMigrationAllowed;
			this.gestationPeriodDays = p.gestationPeriodDays;
			this.mateMtbHours = p.mateMtbHours;
			this.nameOnTameChance = p.nameOnTameChance;
			this.nameOnNuzzleChance = p.nameOnNuzzleChance;
			this.baseBodySize = p.baseBodySize;
			this.baseHealthScale = p.baseHealthScale;
			this.baseHungerRate = p.baseHungerRate;
			this.meatLabel = p.meatLabel;
			this.meatMarketValue = p.meatMarketValue;

			this.meatColor = new ColorStats(p.meatColor);
			this.specialShadowData = new ShadowDataStats(p.specialShadowData);
			this.soundCallIntervalRange = new MinMaxIntStats(p.soundCallIntervalRange);
			this.ageGenerationCurve = new SimpleCurveStats(p.ageGenerationCurve);
			this.litterSizeCurve = new SimpleCurveStats(p.litterSizeCurve);

			Util.Populate(out this.wildBiomes, p.wildBiomes, (v) => new FloatValueDefStat<BiomeDef> (v.biome, v.commonality));
			Util.Populate(out this.lifeStageAges, p.lifeStageAges, (v) => new LifeStageAgeStats(v));
			Util.Populate(out this.untrainableTags, p.untrainableTags);
			Util.Populate(out this.trainableTags, p.trainableTags);

			Util.AssignDefStat(GetFleshType(p), out this.fleshType);
			Util.AssignDefStat(GetBloodDef(p), out this.bloodDef);
			Util.AssignDefStat(p.thinkTreeMain, out this.thinkTreeMain);
			Util.AssignDefStat(p.thinkTreeConstant, out this.thinkTreeConstant);
			Util.AssignDefStat(p.body, out this.body);
			Util.AssignDefStat(p.trainability, out this.trainability);
			Util.AssignDefStat(GetNameGenerator(p), out this.nameGenerator);
			Util.AssignDefStat(GetNameGeneratorFemale(p), out this.nameGeneratorFemale);
			Util.AssignDefStat(p.useMeatFrom, out this.useMeatFrom);
			Util.AssignDefStat(p.useLeatherFrom, out this.useLeatherFrom);
			Util.AssignDefStat(p.leatherDef, out this.leatherDef);
			Util.AssignDefStat(p.soundMeleeHitPawn, out this.soundMeleeHitPawn);
			Util.AssignDefStat(p.soundMeleeHitBuilding, out this.soundMeleeHitBuilding);
			Util.AssignDefStat(p.soundMeleeMiss, out this.soundMeleeMiss);
			Util.AssignDefStat(p.meatDef, out this.meatDef);
			Util.AssignDefStat(p.corpseDef, out this.corpseDef);

			this.hediffGiverSets = Util.CreateDefStatList(p.hediffGiverSets);
		}

		public static FleshTypeDef GetFleshType(RaceProperties p)
		{
			return (FleshTypeDef)typeof(RaceProperties).GetField("fleshType", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(p);
		}

		public static void SetFleshType(RaceProperties p, FleshTypeDef d)
		{
			typeof(RaceProperties).GetField("fleshType", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(p, d);
		}

		public static ThingDef GetBloodDef(RaceProperties p)
		{
			return (ThingDef)typeof(RaceProperties).GetField("bloodDef", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(p);
		}

		public static void SetBloodDef(RaceProperties p, ThingDef d)
		{
			typeof(RaceProperties).GetField("bloodDef", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(p, d);
		}

		public static RulePackDef GetNameGenerator(RaceProperties p)
		{
			return (RulePackDef)typeof(RaceProperties).GetField("nameGenerator", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(p);
		}

		public static void SetNameGenerator(RaceProperties p, RulePackDef d)
		{
			typeof(RaceProperties).GetField("nameGenerator", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(p, d);
		}

		public static RulePackDef GetNameGeneratorFemale(RaceProperties p)
		{
			return (RulePackDef)typeof(RaceProperties).GetField("nameGeneratorFemale", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(p);
		}

		public static void SetNameGeneratorFemale(RaceProperties p, RulePackDef d)
		{
			typeof(RaceProperties).GetField("nameGeneratorFemale", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(p, d);
		}
	}
}
