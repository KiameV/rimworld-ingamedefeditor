using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace InGameDefEditor.Stats
{
	[Serializable]
	public class BiomeDefStats : DefStat<BiomeDef>, IParentStat
    {
        public bool canBuildBase;
        public bool canAutoChoose;
        public bool allowRoads;
        public bool allowRivers;
        public float animalDensity;
        public float plantDensity;
        public float diseaseMtbDays;
        public float settlementSelectionWeight;
        public bool impassable;
        public bool hasVirtualPlants;
        public float forageability;
        public DefStat<ThingDef> foragedFood = null;
        public bool wildPlantsCareAboutLocalFertility;
        public float wildPlantRegrowDays;
        public float movementDifficulty;
        public bool hasBedrock;

        public List<FloatValueDefStat<WeatherDef>> weatherCommonalities = null;
        public List<MinMaxFloatDefStat<TerrainDef>> terrainsByFertility = null;
        public List<DefStat<SoundDef>> soundsAmbient = null;
        public List<TerrainPatchMakerStats> terrainPatchMakers = null;
        public List<FloatValueDefStat<ThingDef>> wildPlants = null;
        public List<FloatValueDefStat<PawnKindDef>> wildAnimals = null;
        public List<FloatValueDoubleDefStat<IncidentDef, BiomeDef>> diseases = null;
        public List<DefStat<PawnKindDef>> allowedPackAnimals = null;

        public BiomeDefStats() { }
        public BiomeDefStats(BiomeDef d) : base(d)
        {
            this.canBuildBase = d.canBuildBase;
            this.canAutoChoose = d.canAutoChoose;
            this.allowRoads = d.allowRoads;
            this.allowRivers = d.allowRivers;
            this.animalDensity = d.animalDensity;
            this.plantDensity = d.plantDensity;
            this.diseaseMtbDays = d.diseaseMtbDays;
            this.settlementSelectionWeight = d.settlementSelectionWeight;
            this.impassable = d.impassable;
            this.hasVirtualPlants = d.hasVirtualPlants;
            this.forageability = d.forageability;
            if (d.foragedFood != null)
                this.foragedFood = new DefStat<ThingDef>(d.foragedFood);
            this.wildPlantsCareAboutLocalFertility = d.wildPlantsCareAboutLocalFertility;
            this.wildPlantRegrowDays = d.wildPlantRegrowDays;
            this.movementDifficulty = d.movementDifficulty;
            this.hasBedrock = d.hasBedrock;

            if (d.baseWeatherCommonalities != null)
            {
                this.weatherCommonalities = new List<FloatValueDefStat<WeatherDef>>(d.baseWeatherCommonalities.Count);
                foreach (var v in d.baseWeatherCommonalities)
                    this.weatherCommonalities.Add(new FloatValueDefStat<WeatherDef>(v.weather)
                    {
                        value = v.commonality
                    });
            }

            if (d.terrainsByFertility != null)
            {
                this.terrainsByFertility = new List<MinMaxFloatDefStat<TerrainDef>>(d.terrainsByFertility.Count);
                foreach (var v in d.terrainsByFertility)
                    this.terrainsByFertility.Add(new MinMaxFloatDefStat<TerrainDef>(v.terrain)
                    {
                        Min = v.min,
                        Max = v.max
                    });
            }

            if (d.soundsAmbient != null)
            {
                this.soundsAmbient = new List<DefStat<SoundDef>>(d.soundsAmbient.Count);
                foreach (var v in d.soundsAmbient)
                    this.soundsAmbient.Add(new DefStat<SoundDef>(v));
            }

            if (d.terrainPatchMakers != null)
            {
                this.terrainPatchMakers = new List<TerrainPatchMakerStats>(d.terrainPatchMakers.Count);
                foreach (var v in d.terrainPatchMakers)
                    this.terrainPatchMakers.Add(new TerrainPatchMakerStats(v));
            }

            List<BiomePlantRecord> plants = GetWildPlants(base.Def);
            if (plants != null)
            {
                this.wildPlants = new List<FloatValueDefStat<ThingDef>>(plants.Count);
                foreach (var v in plants)
                    this.wildPlants.Add(new FloatValueDefStat<ThingDef>(v.plant)
                    {
                        value = v.commonality
                    });
            }

            List<BiomeAnimalRecord> animals = GetWildAnimals(base.Def);
            if (animals != null)
            {
                this.wildAnimals = new List<FloatValueDefStat<PawnKindDef>>(animals.Count);
                foreach (var v in animals)
                    this.wildAnimals.Add(new FloatValueDefStat<PawnKindDef>(v.animal)
                    {
                        value = v.commonality
                    });
            }

            List<BiomeDiseaseRecord> diseases = GetDiseases(base.Def);
            if (diseases != null)
            {
                this.diseases = new List<FloatValueDoubleDefStat<IncidentDef, BiomeDef>>(diseases.Count);
                foreach (var v in diseases)
                    this.diseases.Add(new FloatValueDoubleDefStat<IncidentDef, BiomeDef>(v.diseaseInc, v.biome)
                    {
                        value = v.commonality
                    });
            }

            List<ThingDef> allowedPackAnimals = GetAllowedPackAnimals(base.Def);
            if (d != null)
            {
                this.allowedPackAnimals = new List<DefStat<PawnKindDef>>(allowedPackAnimals.Count);
                foreach (var v in allowedPackAnimals)
                    this.allowedPackAnimals.Add(new DefStat<PawnKindDef>(v.race.AnyPawnKind));
            }
        }

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            if (this.foragedFood != null)
                this.foragedFood.Initialize();

            if (this.weatherCommonalities != null)
                foreach (var v in this.weatherCommonalities)
                    if (!v.Initialize())
                        Log.Warning("Unable to load WeatherDef " + v.defName);

            if (this.terrainsByFertility != null)
                foreach (var v in this.terrainsByFertility)
                    if (!v.Initialize())
                        Log.Warning("Unable to load TerrainDef " + v.defName);

            if (this.terrainPatchMakers != null)
                foreach (var v in this.terrainPatchMakers)
                    if (!v.Initialize())
                        Log.Warning("Unable to initialize TerrainPatchMaker");

            if (this.soundsAmbient != null)
                foreach (var v in this.soundsAmbient)
                    if (!v.Initialize())
                        Log.Warning("Unable to load SoundDef " + v.defName);

            if (this.wildPlants != null)
                foreach (var v in this.wildPlants)
                    if (!v.Initialize())
                        Log.Warning("Unable to load ThingDef " + v.defName);

            if (this.wildAnimals != null)
                foreach (var v in this.wildAnimals)
                    if (!v.Initialize())
                        Log.Warning("Unable to load PawnKindDef " + v.defName);

            if (this.diseases != null)
                foreach (var v in this.diseases)
                    if (!v.Initialize())
                        if (v.Def == null)
                            Log.Warning("Unable to load IncidentDef " + v.defName);

            if (this.allowedPackAnimals != null)
                foreach (var v in this.allowedPackAnimals)
                    if (!v.Initialize())
                        Log.Warning("Unable to load PawnKindDef " + v.defName);

            return true;
        }

        public void ApplyStats(Def to)
        {
            if (to is BiomeDef t)
            {
                t.canBuildBase = this.canBuildBase;
                t.canAutoChoose = this.canAutoChoose;
                t.allowRoads = this.allowRoads;
                t.allowRivers = this.allowRivers;
                t.animalDensity = this.animalDensity;
                t.plantDensity = this.plantDensity;
                t.diseaseMtbDays = this.diseaseMtbDays;
                t.settlementSelectionWeight = this.settlementSelectionWeight;
                t.impassable = this.impassable;
                t.hasVirtualPlants = this.hasVirtualPlants;
                t.forageability = this.forageability;
                if (this.foragedFood == null)
                    t.foragedFood = null;
                else
                    t.foragedFood = this.foragedFood.Def;
                t.wildPlantsCareAboutLocalFertility = this.wildPlantsCareAboutLocalFertility;
                t.wildPlantRegrowDays = this.wildPlantRegrowDays;
                t.movementDifficulty = this.movementDifficulty;
                t.hasBedrock = this.hasBedrock;
                
                if (this.weatherCommonalities != null && t.baseWeatherCommonalities == null)
                    t.baseWeatherCommonalities = new List<WeatherCommonalityRecord>(this.weatherCommonalities.Count);
                Util.Populate(t.baseWeatherCommonalities, this.weatherCommonalities, delegate (FloatValueDefStat<WeatherDef> s)
                {
                    return new WeatherCommonalityRecord()
                    {
                        weather = s.Def,
                        commonality = s.value
                    };
                });

                if (this.terrainsByFertility != null && t.terrainsByFertility == null)
                    t.terrainsByFertility = new List<TerrainThreshold>(this.terrainsByFertility.Count);
                Util.Populate(t.terrainsByFertility, this.terrainsByFertility, delegate (MinMaxFloatDefStat<TerrainDef> s)
                {
                    return new TerrainThreshold
                    {
                        terrain = s.Def,
                        min = s.Min,
                        max = s.Max
                    };
                });

                if (this.soundsAmbient != null && t.soundsAmbient == null)
                    t.soundsAmbient = new List<SoundDef>(this.soundsAmbient.Count);
                Util.Populate(t.soundsAmbient, this.soundsAmbient, delegate (DefStat<SoundDef> s)
                {
                    return s.Def;
                });

                if (this.terrainPatchMakers != null && t.terrainPatchMakers == null)
                    t.terrainPatchMakers = new List<TerrainPatchMaker>(this.terrainPatchMakers.Count);
                Util.Populate(t.terrainPatchMakers, this.terrainPatchMakers, delegate (TerrainPatchMakerStats s)
                {
                    var v = new TerrainPatchMaker();
                    s.ApplyStats(v);
                    return v;
                });

                List<BiomePlantRecord> wildPlants = GetWildPlants(base.Def);
                if (this.wildPlants != null && wildPlants == null)
                {
                    wildPlants = new List<BiomePlantRecord>(this.wildPlants.Count);
                    SetWildPlants(base.Def, wildPlants);
                }
                Util.Populate(wildPlants, this.wildPlants, delegate (FloatValueDefStat<ThingDef> s)
                {
                    return new BiomePlantRecord()
                    {
                        plant = s.Def,
                        commonality = s.value
                    };
                });

                List<BiomeAnimalRecord> wildAnimals = GetWildAnimals(base.Def);
                if (this.wildAnimals != null && wildAnimals == null)
                {
                    wildAnimals = new List<BiomeAnimalRecord>(this.wildAnimals.Count);
                    SetWildAnimals(base.Def, wildAnimals);
                }
                Util.Populate(wildAnimals, this.wildAnimals, delegate (FloatValueDefStat<PawnKindDef> s)
                {
                    return new BiomeAnimalRecord()
                    {
                        animal = s.Def,
                        commonality = s.value
                    };
                });

                List<BiomeDiseaseRecord> diseases = GetDiseases(base.Def);
                if (this.diseases != null && diseases == null)
                {
                    diseases = new List<BiomeDiseaseRecord>(this.diseases.Count);
                    SetDiseases(base.Def, diseases);
                }
                Util.Populate(diseases, this.diseases, delegate (FloatValueDoubleDefStat<IncidentDef, BiomeDef> s)
                {
                    return new BiomeDiseaseRecord()
                    {
                        diseaseInc = s.Def,
                        biome = s.Def2,
                        commonality = s.value
                    };
                });
                
                List<ThingDef> allowedPackAnimals = GetAllowedPackAnimals(base.Def);
                if (this.allowedPackAnimals != null && allowedPackAnimals == null)
                {
                    allowedPackAnimals = new List<ThingDef>(this.allowedPackAnimals.Count);
                    SetAllowedPackAnimals(base.Def, allowedPackAnimals);
                }
                Util.Populate(allowedPackAnimals, this.allowedPackAnimals, delegate (DefStat<PawnKindDef> s)
                {
                    return s.Def.race;
                });
            }
            else
                Log.Error("ThingDefStat passed none ThingDef!");
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(
                base.ToString() + Environment.NewLine +
                "canBuildBase: " + this.canBuildBase + Environment.NewLine +
                "canAutoChoose: " + this.canAutoChoose + Environment.NewLine +
                "allowRoads: " + this.allowRoads + Environment.NewLine +
                "allowRivers: " + this.allowRivers + Environment.NewLine +
                "animalDensity: " + this.animalDensity + Environment.NewLine +
                "plantDensity: " + this.plantDensity + Environment.NewLine +
                "diseaseMtbDays: " + this.diseaseMtbDays + Environment.NewLine +
                "settlementSelectionWeight: " + this.settlementSelectionWeight + Environment.NewLine +
                "impassable: " + this.impassable + Environment.NewLine +
                "hasVirtualPlants: " + this.hasVirtualPlants + Environment.NewLine +
                "forageability: " + this.forageability + Environment.NewLine +
                "foragedFood: " + ((this.foragedFood == null) ? "null" : this.foragedFood.DefName) + Environment.NewLine +
                "wildPlantsCareAboutLocalFertility: " + this.wildPlantsCareAboutLocalFertility + Environment.NewLine +
                "wildPlantRegrowDays: " + this.wildPlantRegrowDays + Environment.NewLine +
                "movementDifficulty: " + this.movementDifficulty + Environment.NewLine +
                "hasBedrock: " + this.hasBedrock + Environment.NewLine);
            sb.AppendLine("Weather Commonalities:");
            if (base.Def.baseWeatherCommonalities != null)
            {
                foreach (var v in base.Def.baseWeatherCommonalities)
                {
                    sb.AppendLine("    " + v.weather.label + " " + v.commonality);
                }
            }
            else
                sb.AppendLine("  is null");
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj != null &&
                obj is BiomeDefStats s)
            {
#if DEBUG_BIOME
                Log.Error(this.ToString());
                Log.Error(s.ToString());
#endif
                if (this.canBuildBase == s.canBuildBase &&
                    this.canAutoChoose == s.canAutoChoose &&
                    this.allowRoads == s.allowRoads &&
                    this.allowRivers == s.allowRivers &&
                    this.animalDensity == s.animalDensity &&
                    this.plantDensity == s.plantDensity &&
                    this.diseaseMtbDays == s.diseaseMtbDays &&
                    this.settlementSelectionWeight == s.settlementSelectionWeight &&
                    this.impassable == s.impassable &&
                    this.hasVirtualPlants == s.hasVirtualPlants &&
                    this.forageability == s.forageability &&
                    this.wildPlantsCareAboutLocalFertility == s.wildPlantsCareAboutLocalFertility &&
                    this.wildPlantRegrowDays == s.wildPlantRegrowDays &&
                    this.movementDifficulty == s.movementDifficulty &&
                    this.hasBedrock == s.hasBedrock &&
                    object.Equals(this.foragedFood, s.foragedFood)&&
                    Util.AreEqual(this.weatherCommonalities, s.weatherCommonalities) &&
                    Util.AreEqual(this.terrainsByFertility, s.terrainsByFertility) &&
                    Util.AreEqual(this.soundsAmbient, s.soundsAmbient) &&
                    Util.AreEqual(this.terrainPatchMakers, s.terrainPatchMakers, null) &&
                    Util.AreEqual(this.wildPlants, s.wildPlants) &&
                    Util.AreEqual(this.wildAnimals, s.wildAnimals) &&
                    Util.AreEqual(this.diseases, s.diseases) &&
                    Util.AreEqual(this.allowedPackAnimals, s.allowedPackAnimals))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<BiomePlantRecord> GetWildPlants(BiomeDef def)
        {
            return typeof(BiomeDef).GetField("wildPlants", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(def) as List<BiomePlantRecord>;
        }

        public static void SetWildPlants(BiomeDef def, List<BiomePlantRecord> value)
        {
            typeof(BiomeDef).GetField("wildPlants", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(def, value);
        }

        public static List<BiomeAnimalRecord> GetWildAnimals(BiomeDef def)
        {
            return typeof(BiomeDef).GetField("wildAnimals", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(def) as List<BiomeAnimalRecord>;
        }

        public static void SetWildAnimals(BiomeDef def, List<BiomeAnimalRecord> value)
        {
            typeof(BiomeDef).GetField("wildAnimals", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(def, value);
        }

        public static List<BiomeDiseaseRecord> GetDiseases(BiomeDef def)
        {
            return typeof(BiomeDef).GetField("diseases", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(def) as List<BiomeDiseaseRecord>;
        }

        public static void SetDiseases(BiomeDef def, List<BiomeDiseaseRecord> value)
        {
            typeof(BiomeDef).GetField("diseases", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(def, value);
        }

        public static List<ThingDef> GetAllowedPackAnimals(BiomeDef def)
        {
            return typeof(BiomeDef).GetField("allowedPackAnimals", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(def) as List<ThingDef>;
        }

        public static void SetAllowedPackAnimals(BiomeDef def, List<ThingDef> value)
        {
            typeof(BiomeDef).GetField("allowedPackAnimals", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(def, value);
        }
    }
}
