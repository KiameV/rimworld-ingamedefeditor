using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace InGameDefEditor.Stats
{
    public class BiomeStats : DefStat<BiomeDef>, IParentStat
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

        public List<FloatValueStat<WeatherDef>> weatherCommonalities = null;
        public List<MinMaxStat<TerrainDef>> terrainsByFertility = null;
        public List<DefStat<SoundDef>> soundsAmbient = null;
        public List<TerrainPatchMakerStats> terrainPatchMakers = null;
        public List<FloatValueStat<ThingDef>> wildPlants = null;
        public List<FloatValueStat<PawnKindDef>> wildAnimals = null;
        public List<FloatValueDoubleDefStat<IncidentDef, BiomeDef>> diseases = null;
        public List<DefStat<PawnKindDef>> allowedPackAnimals = null;

        public BiomeStats() { }
        public BiomeStats(BiomeDef d) : base(d)
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
                this.weatherCommonalities = new List<FloatValueStat<WeatherDef>>(d.baseWeatherCommonalities.Count);
                foreach (var v in d.baseWeatherCommonalities)
                    this.weatherCommonalities.Add(new FloatValueStat<WeatherDef>(v.weather)
                    {
                        value = v.commonality
                    });
            }

            if (d.terrainsByFertility != null)
            {
                this.terrainsByFertility = new List<MinMaxStat<TerrainDef>>(d.terrainsByFertility.Count);
                foreach (var v in d.terrainsByFertility)
                    this.terrainsByFertility.Add(new MinMaxStat<TerrainDef>(v.terrain)
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

            List<BiomePlantRecord> plants = this.GetWildPlants();
            if (plants != null)
            {
                this.wildPlants = new List<FloatValueStat<ThingDef>>(plants.Count);
                foreach (var v in plants)
                    this.wildPlants.Add(new FloatValueStat<ThingDef>(v.plant)
                    {
                        value = v.commonality
                    });
            }

            List<BiomeAnimalRecord> animals = this.GetWildAnimals();
            if (animals != null)
            {
                this.wildAnimals = new List<FloatValueStat<PawnKindDef>>(animals.Count);
                foreach (var v in animals)
                    this.wildAnimals.Add(new FloatValueStat<PawnKindDef>(v.animal)
                    {
                        value = v.commonality
                    });
            }

            List<BiomeDiseaseRecord> diseases = this.GetDiseases();
            if (diseases != null)
            {
                this.diseases = new List<FloatValueDoubleDefStat<IncidentDef, BiomeDef>>(diseases.Count);
                foreach (var v in diseases)
                    this.diseases.Add(new FloatValueDoubleDefStat<IncidentDef, BiomeDef>(v.diseaseInc, v.biome)
                    {
                        value = v.commonality
                    });
            }

            List<ThingDef> allowedPackAnimals = GetAllowedPackAnimals();
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
                    {
                        Log.Warning("Unable to load WeatherDef " + v.defName);
                    }

            if (this.terrainsByFertility != null)
                foreach (var v in this.terrainsByFertility)
                    if (!v.Initialize())
                    {
                        Log.Warning("Unable to load TerrainDef " + v.defName);
                    }

            if (this.soundsAmbient != null)
                foreach (var v in this.soundsAmbient)
                    if (!v.Initialize())
                    {
                        Log.Warning("Unable to load SoundDef " + v.defName);
                    }

            if (this.wildPlants != null)
                foreach (var v in this.wildPlants)
                    if (!v.Initialize())
                    {
                        Log.Warning("Unable to load ThingDef " + v.defName);
                    }

            if (this.wildAnimals != null)
                foreach (var v in this.wildAnimals)
                    if (!v.Initialize())
                    {
                        Log.Warning("Unable to load PawnKindDef " + v.defName);
                    }

            if (this.diseases != null)
                foreach (var v in this.diseases)
                    if (!v.Initialize())
                    {
                        if (v.Def == null)
                            Log.Warning("Unable to load IncidentDef " + v.defName);
                    }

            if (this.allowedPackAnimals != null)
                foreach (var v in this.allowedPackAnimals)
                    if (!v.Initialize())
                    {
                        Log.Warning("Unable to load PawnKindDef " + v.defName);
                    }

            return true;
        }

        public void ApplyStats(Def def)
        {
            if (def is BiomeDef to)
            {
                to.canBuildBase = this.canBuildBase;
                to.canAutoChoose = this.canAutoChoose;
                to.allowRoads = this.allowRoads;
                to.allowRivers = this.allowRivers;
                to.animalDensity = this.animalDensity;
                to.plantDensity = this.plantDensity;
                to.diseaseMtbDays = this.diseaseMtbDays;
                to.settlementSelectionWeight = this.settlementSelectionWeight;
                to.impassable = this.impassable;
                to.hasVirtualPlants = this.hasVirtualPlants;
                to.forageability = this.forageability;
                if (this.foragedFood == null)
                    to.foragedFood = null;
                else
                    to.foragedFood = this.foragedFood.Def;
                to.wildPlantsCareAboutLocalFertility = this.wildPlantsCareAboutLocalFertility;
                to.wildPlantRegrowDays = this.wildPlantRegrowDays;
                to.movementDifficulty = this.movementDifficulty;
                to.hasBedrock = this.hasBedrock;
                
                if (this.weatherCommonalities != null && to.baseWeatherCommonalities == null)
                    to.baseWeatherCommonalities = new List<WeatherCommonalityRecord>(this.weatherCommonalities.Count);
                Util.Populate(to.baseWeatherCommonalities, this.weatherCommonalities, delegate (FloatValueStat<WeatherDef> s)
                {
                    return new WeatherCommonalityRecord()
                    {
                        weather = s.Def,
                        commonality = s.value
                    };
                });

                if (this.terrainsByFertility != null && to.terrainsByFertility == null)
                    to.terrainsByFertility = new List<TerrainThreshold>(this.terrainsByFertility.Count);
                Util.Populate(to.terrainsByFertility, this.terrainsByFertility, delegate (MinMaxStat<TerrainDef> s)
                {
                    return new TerrainThreshold
                    {
                        terrain = s.Def,
                        min = s.Min,
                        max = s.Max
                    };
                });

                if (this.soundsAmbient != null && to.soundsAmbient == null)
                    to.soundsAmbient = new List<SoundDef>(this.soundsAmbient.Count);
                Util.Populate(to.soundsAmbient, this.soundsAmbient, delegate (DefStat<SoundDef> s)
                {
                    return s.Def;
                });

                if (this.terrainPatchMakers != null && to.terrainPatchMakers == null)
                    to.terrainPatchMakers = new List<TerrainPatchMaker>(this.terrainPatchMakers.Count);
                Util.Populate(to.terrainPatchMakers, this.terrainPatchMakers, delegate (TerrainPatchMakerStats s)
                {
                    var v = new TerrainPatchMaker();
                    s.ApplyStats(v);
                    return v;
                });

                List<BiomePlantRecord> wildPlants = this.GetWildPlants();
                if (this.wildPlants != null && wildPlants == null)
                {
                    wildPlants = new List<BiomePlantRecord>(this.wildPlants.Count);
                    this.SetWildPlants(wildPlants);
                }
                Util.Populate(wildPlants, this.wildPlants, delegate (FloatValueStat<ThingDef> s)
                {
                    return new BiomePlantRecord()
                    {
                        plant = s.Def,
                        commonality = s.value
                    };
                });

                List<BiomeAnimalRecord> wildAnimals = this.GetWildAnimals();
                if (this.wildAnimals != null && wildAnimals == null)
                {
                    wildAnimals = new List<BiomeAnimalRecord>(this.wildAnimals.Count);
                    this.SetWildAnimals(wildAnimals);
                }
                Util.Populate(wildAnimals, this.wildAnimals, delegate (FloatValueStat<PawnKindDef> s)
                {
                    return new BiomeAnimalRecord()
                    {
                        animal = s.Def,
                        commonality = s.value
                    };
                });

                List<BiomeDiseaseRecord> diseases = this.GetDiseases();
                if (this.diseases != null && diseases == null)
                {
                    diseases = new List<BiomeDiseaseRecord>(this.diseases.Count);
                    this.SetDiseases(diseases);
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
                
                List<ThingDef> allowedPackAnimals = this.GetAllowedPackAnimals();
                if (this.allowedPackAnimals != null && allowedPackAnimals == null)
                {
                    allowedPackAnimals = new List<ThingDef>(this.allowedPackAnimals.Count);
                    this.SetAllowedPackAnimals(allowedPackAnimals);
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
            return
                base.ToString() + Environment.NewLine +
                "canBuildBase" + this.canBuildBase + Environment.NewLine +
                "canAutoChoose" + this.canAutoChoose + Environment.NewLine +
                "allowRoads" + this.allowRoads + Environment.NewLine +
                "allowRivers" + this.allowRivers + Environment.NewLine +
                "animalDensity" + this.animalDensity + Environment.NewLine +
                "plantDensity" + this.plantDensity + Environment.NewLine +
                "diseaseMtbDays" + this.diseaseMtbDays + Environment.NewLine +
                "settlementSelectionWeight" + this.settlementSelectionWeight + Environment.NewLine +
                "impassable" + this.impassable + Environment.NewLine +
                "hasVirtualPlants" + this.hasVirtualPlants + Environment.NewLine +
                "forageability" + this.forageability + Environment.NewLine +
                "foragedFood" + ((this.foragedFood == null) ? "null" : this.foragedFood.DefName) + Environment.NewLine +
                "wildPlantsCareAboutLocalFertility" + this.wildPlantsCareAboutLocalFertility + Environment.NewLine +
                "wildPlantRegrowDays" + this.wildPlantRegrowDays + Environment.NewLine +
                "movementDifficulty" + this.movementDifficulty + Environment.NewLine +
                "hasBedrock" + this.hasBedrock;
        }

        public override bool Equals(object obj)
        {
            if (obj != null &&
                obj is BiomeStats s)
            {
#if DEBUG
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
                    this.foragedFood == s.foragedFood &&
                    this.wildPlantsCareAboutLocalFertility == s.wildPlantsCareAboutLocalFertility &&
                    this.wildPlantRegrowDays == s.wildPlantRegrowDays &&
                    this.movementDifficulty == s.movementDifficulty &&
                    this.hasBedrock == s.hasBedrock &&
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

        private List<BiomePlantRecord> GetWildPlants()
        {
            return typeof(BiomeDef).GetField("wildPlants", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(base.Def) as List<BiomePlantRecord>;
        }

        private void SetWildPlants(List<BiomePlantRecord> value)
        {
            typeof(BiomeDef).GetField("wildPlants", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.Def, value);
        }

        private List<BiomeAnimalRecord> GetWildAnimals()
        {
            return typeof(BiomeDef).GetField("wildAnimals", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(base.Def) as List<BiomeAnimalRecord>;
        }

        private void SetWildAnimals(List<BiomeAnimalRecord> value)
        {
            typeof(BiomeDef).GetField("wildAnimals", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.Def, value);
        }

        private List<BiomeDiseaseRecord> GetDiseases()
        {
            return typeof(BiomeDef).GetField("diseases", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(base.Def) as List<BiomeDiseaseRecord>;
        }

        private void SetDiseases(List<BiomeDiseaseRecord> value)
        {
            typeof(BiomeDef).GetField("diseases", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.Def, value);
        }

        private List<ThingDef> GetAllowedPackAnimals()
        {
            return typeof(BiomeDef).GetField("allowedPackAnimals", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(base.Def) as List<ThingDef>;
        }

        private void SetAllowedPackAnimals(List<ThingDef> value)
        {
            typeof(BiomeDef).GetField("allowedPackAnimals", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(base.Def, value);
        }
    }
}
