using InGameDefEditor.Stats.Misc;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats
{
    class BiomeStat
    {
        [XmlIgnore]
        private BiomeDef def;

        [XmlElement(IsNullable = false)]
        public string defName;

        public Def Def => this.def;
        public string DefName => this.def.defName;
        public string Label => this.def.label;
        
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
        public ThingDef foragedFood;
        public bool wildPlantsCareAboutLocalFertility;
        public float wildPlantRegrowDays;
        public float movementDifficulty;
        public bool hasBedrock;

        public List<FloatValueStat<WeatherDef>> weatherCommonalities = null;
        public List<MinMaxStat<TerrainDef>> terrainsByFertility = null;
        public List<DefStat<SoundDef>> soundsAmbient = null;
        public List<TerrainPatchMaker> terrainPatchMakers = null;
        private List<FloatValueStat<ThingDef>> wildPlants = null;
        private List<FloatValueStat<PawnKindDef>> wildAnimals = null;
        private List<FloatValueDoubleDefStat<BiomeDef, IncidentDef>> diseases = null;
        private List<DefStat<ThingDef>> allowedPackAnimals = null;

        public BiomeStat() { }
        public BiomeStat(BiomeDef d)
        {
            this.def = d;
            this.defName = d.defName;

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
            this.foragedFood = d.foragedFood;
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

            List<BiomeAnimalRecord> animals = this.GetWildAnimal();
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
                this.diseases = new List<FloatValueDoubleDefStat<BiomeDef, IncidentDef>>(diseases.Count);
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

        public bool Initialize()
        {
            if (this.def == null)
            {
                this.def = DefDatabase<BiomeDef>.AllDefsListForReading.Find(
                    delegate (BiomeDef d) { return d.defName.Equals(this.defName); });

                if (this.def == null)
                {
                    Log.Error("Could not load def " + this.defName);
                    return false;
                }
            }

            List<ThingDef> thingDefs = null;
            if (this.wildPlants != null || this.allowedPackAnimals != null)
                thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;

            if (this.weatherCommonalities != null && this.weatherCommonalities.Count > 0)
            {
                List<WeatherDef> weatherDefs = DefDatabase<WeatherDef>.AllDefsListForReading;
                foreach (var v in this.weatherCommonalities)
                    if (!v.Initialize(weatherDefs))
                    {
                        Log.Warning("Unable to load WeatherDef " + v.defName);
                    }
                weatherDefs.Clear();
                weatherDefs = null;
            }

            if (this.terrainsByFertility != null && this.terrainsByFertility.Count > 0)
            {
                List<TerrainDef> terrainDefs = DefDatabase<TerrainDef>.AllDefsListForReading;
                foreach (var v in this.terrainsByFertility)
                    if (!v.Initialize(terrainDefs))
                    {
                        Log.Warning("Unable to load TerrainDef " + v.defName);
                    }
                terrainDefs.Clear();
                terrainDefs = null;
            }

            if (this.soundsAmbient != null && this.soundsAmbient.Count > 0)
            {
                List<SoundDef> soundDefs = DefDatabase<SoundDef>.AllDefsListForReading;
                foreach (var v in this.soundsAmbient)
                    if (!v.Initialize(soundDefs))
                    {
                        Log.Warning("Unable to load TerrainDef " + v.defName);
                    }
                soundDefs.Clear();
                soundDefs = null;
            }

            if (this.wildPlants != null && this.wildPlants.Count > 0)
                foreach (var v in this.wildPlants)
                    if (!v.Initialize(thingDefs))
                    {
                        Log.Warning("Unable to load TerrainDef " + v.defName);
                    }

            if (this.wildAnimals != null && this.wildAnimals.Count > 0)
            {
                List<PawnKindDef> pawnKindDefs = DefDatabase<PawnKindDef>.AllDefsListForReading;
                foreach (var v in this.wildAnimals)
                    if (!v.Initialize(pawnKindDefs))
                    {
                        Log.Warning("Unable to load TerrainDef " + v.defName);
                    }
                pawnKindDefs.Clear();
                pawnKindDefs = null;
            }

            if (this.wildAnimals != null && this.wildAnimals.Count > 0)
            {
                List<BiomeDef> biomeDefs = DefDatabase<BiomeDef>.AllDefsListForReading;
                List<IncidentDef> incidentDefs = DefDatabase<IncidentDef>.AllDefsListForReading;
                foreach (var v in this.diseases)
                    if (!v.Initialize(biomeDefs, incidentDefs))
                    {
                        Log.Warning("Unable to load TerrainDef " + v.defName);
                    }
                biomeDefs.Clear();
                biomeDefs = null;
                incidentDefs.Clear();
                incidentDefs = null;
            }

            if (this.allowedPackAnimals != null && this.allowedPackAnimals.Count > 0)
                foreach (var v in this.allowedPackAnimals)
                    if (!v.Initialize(thingDefs))
                    {
                        Log.Warning("Unable to load ThingDef " + v.defName);
                    }

            if (thingDefs != null)
            {
                thingDefs.Clear();
                thingDefs = null;
            }

            return true;
        }

        public void ApplyStats(BiomeDef to)
        {
            // TODO
        }

        public override int GetHashCode()
        {
            return this.def.GetHashCode();
        }

        public override string ToString()
        {
            return this.defName;
        }

        public override bool Equals(object obj)
        {
            if (obj != null &&
                obj is BiomeStat s)
            {
                // TODO
            }
            return false;
        }

        private List<BiomePlantRecord> GetWildPlants()
        {
            return typeof(BiomeDef).GetField("wildPlants", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this.def) as List<BiomePlantRecord>;
        }

        private void SetWildPlants(List<BiomePlantRecord> l)
        {
            typeof(BiomeDef).GetField("wildPlants", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this.def, l);
        }

        private List<BiomeAnimalRecord> GetWildAnimal()
        {
            return typeof(BiomeDef).GetField("wildAnimals", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this.def) as List<BiomeAnimalRecord>;
        }

        private void SetWildAnimal(List<BiomeAnimalRecord> l)
        {
            typeof(BiomeDef).GetField("wildAnimals", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this.def, l);
        }

        private List<BiomeDiseaseRecord> GetDiseases()
        {
            return typeof(BiomeDef).GetField("diseases", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this.def) as List<BiomeDiseaseRecord>;
        }

        private void SetDiseases(List<BiomeDiseaseRecord> l)
        {
            typeof(BiomeDef).GetField("diseases", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this.def, l);
        }

        private List<ThingDef> GetAllowedPackAnimals()
        {
            return typeof(BiomeDef).GetField("allowedPackAnimals", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this.def) as List<ThingDef>;
        }

        private void SetAllowedPackAnimals(List<ThingDef> l)
        {
            typeof(BiomeDef).GetField("allowedPackAnimals", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this.def, l);
        }
    }
}
