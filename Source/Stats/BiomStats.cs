using InGameDefEditor.Stats.Misc;
using RimWorld;
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
        public DefStat<ThingDef> foragedFood;
        public bool wildPlantsCareAboutLocalFertility;
        public float wildPlantRegrowDays;
        public float movementDifficulty;
        public bool hasBedrock;

        public List<DefStat<WeatherDef>> weatherCommonalities = null;
        public List<DefStat<TerrainDef>> terrainsByFertility = null;
        public List<DefStat<SoundDef>> soundsAmbient = null;
        public List<TerrainPatchMakerStats> terrainPatchMakers = null;
        public List<DefStat<ThingDef>> wildPlants = null;
        public List<DefStat<PawnKindDef>> wildAnimals = null;
        public List<DefStat<BiomeDef>> diseases = null;
        public List<DefStat<ThingDef>> allowedPackAnimals = null;

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
                this.weatherCommonalities = new List<DefStat<WeatherDef>>(d.baseWeatherCommonalities.Count);
                foreach (var v in d.baseWeatherCommonalities)
                    this.weatherCommonalities.Add(new FloatValueStat<WeatherDef>(v.weather)
                    {
                        value = v.commonality
                    });
            }

            if (d.terrainsByFertility != null)
            {
                this.terrainsByFertility = new List<DefStat<TerrainDef>>(d.terrainsByFertility.Count);
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
                this.wildPlants = new List<DefStat<ThingDef>>(plants.Count);
                foreach (var v in plants)
                    this.wildPlants.Add(new FloatValueStat<ThingDef>(v.plant)
                    {
                        value = v.commonality
                    });
            }

            List<BiomeAnimalRecord> animals = this.GetWildAnimal();
            if (animals != null)
            {
                this.wildAnimals = new List<DefStat<PawnKindDef>>(animals.Count);
                foreach (var v in animals)
                    this.wildAnimals.Add(new FloatValueStat<PawnKindDef>(v.animal)
                    {
                        value = v.commonality
                    });
            }

            List<BiomeDiseaseRecord> diseases = this.GetDiseases();
            if (diseases != null)
            {
                this.diseases = new List<DefStat<BiomeDef>>(diseases.Count);
                foreach (var v in diseases)
                    this.diseases.Add(new FloatValueDoubleDefStat<BiomeDef, IncidentDef>(v.biome, v.diseaseInc)
                    {
                        value = v.commonality
                    });
            }

            List<ThingDef> allowedPackAnimals = GetAllowedPackAnimals();
            if (d != null)
            {
                this.allowedPackAnimals = new List<DefStat<ThingDef>>(allowedPackAnimals.Count);
                foreach (var v in allowedPackAnimals)
                    this.allowedPackAnimals.Add(new DefStat<ThingDef>(v));
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
                        Log.Warning("Unable to load TerrainDef " + v.defName);
                    }

            if (this.wildPlants != null)
                foreach (var v in this.wildPlants)
                    if (!v.Initialize())
                    {
                        Log.Warning("Unable to load TerrainDef " + v.defName);
                    }

            if (this.wildAnimals != null)
                foreach (var v in this.wildAnimals)
                    if (!v.Initialize())
                    {
                        Log.Warning("Unable to load TerrainDef " + v.defName);
                    }

            if (this.wildAnimals != null)
                foreach (var v in this.diseases)
                    if (!v.Initialize())
                    {
                        Log.Warning("Unable to load TerrainDef " + v.defName);
                    }

            if (this.allowedPackAnimals != null)
                foreach (var v in this.allowedPackAnimals)
                    if (!v.Initialize())
                    {
                        Log.Warning("Unable to load ThingDef " + v.defName);
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
                
                Util.Populate(to.baseWeatherCommonalities, this.weatherCommonalities, delegate (DefStat<WeatherDef> s)
                {
                    return new WeatherCommonalityRecord()
                    {
                        weather = s.Def,
                        commonality = ((FloatValueStat<WeatherDef>)s).value
                    };
                });

                Util.Populate(to.terrainsByFertility, this.terrainsByFertility, delegate (DefStat<TerrainDef> s)
                {
                    return new TerrainThreshold
                    {
                        terrain = s.Def,
                        min = ((MinMaxStat<TerrainDef>)s).Min,
                        max = ((MinMaxStat<TerrainDef>)s).Max
                    };
                });

                Util.Populate(to.soundsAmbient, this.soundsAmbient, delegate (DefStat<SoundDef> s)
                {
                    return s.Def;
                });

                Util.Populate(to.terrainPatchMakers, this.terrainPatchMakers, delegate (TerrainPatchMakerStats s)
                {
                    var v = new TerrainPatchMaker();
                    s.ApplyStats(v);
                    return v;
                });

                Util.Populate(this.GetWildPlants(), this.wildPlants, delegate (DefStat<ThingDef> s)
                {
                    return new BiomePlantRecord()
                    {
                        plant = s.Def,
                        commonality = ((FloatValueStat<ThingDef>)s).value
                    };
                });

                Util.Populate(this.GetWildAnimal(), this.wildAnimals, delegate (DefStat<PawnKindDef> s)
                {
                    return new BiomeAnimalRecord()
                    {
                        animal = s.Def,
                        commonality = ((FloatValueStat<PawnKindDef>)s).value
                    };
                });

                Util.Populate(this.GetDiseases(), this.diseases, delegate (DefStat<BiomeDef> s)
                {
                    return new BiomeDiseaseRecord()
                    {
                        biome = s.Def,
                        diseaseInc = ((FloatValueDoubleDefStat<BiomeDef, IncidentDef>)s).Def2,
                        commonality = ((FloatValueDoubleDefStat<BiomeDef, IncidentDef>)s).value
                    };
                });

                Util.Populate(this.GetAllowedPackAnimals(), this.allowedPackAnimals, delegate (DefStat<ThingDef> s)
                {
                    return s.Def;
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
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj != null &&
                obj is BiomeStats s)
            {
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
        
        private List<BiomeAnimalRecord> GetWildAnimal()
        {
            return typeof(BiomeDef).GetField("wildAnimals", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(base.Def) as List<BiomeAnimalRecord>;
        }
        
        private List<BiomeDiseaseRecord> GetDiseases()
        {
            return typeof(BiomeDef).GetField("diseases", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(base.Def) as List<BiomeDiseaseRecord>;
        }
        
        private List<ThingDef> GetAllowedPackAnimals()
        {
            return typeof(BiomeDef).GetField("allowedPackAnimals", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(base.Def) as List<ThingDef>;
        }
    }
}
