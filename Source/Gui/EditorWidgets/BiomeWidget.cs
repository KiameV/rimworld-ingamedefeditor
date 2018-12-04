using InGameDefEditor.Gui.Dialog;
using InGameDefEditor.Gui.EditorWidgets.Misc;
using InGameDefEditor.Stats;
using RimWorld;
using System.Collections.Generic;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class BiomeWidget : AParentStatWidget<BiomeDef>
    {
        private readonly List<IInputWidget> inputWidgets;

        private List<FloatInputWidget<WeatherCommonalityRecord>> weatherCommonalityRecords = new List<FloatInputWidget<WeatherCommonalityRecord>>();
        private List<MinMaxInputWidget<TerrainThreshold>> terrainsByFertility = new List<MinMaxInputWidget<TerrainThreshold>>();
        private List<SoundDef> soundsAmbient = new List<SoundDef>();
        private List<TerrainPatchMakerWidget> terrainPatchMakers = new List<TerrainPatchMakerWidget>();
        private List<FloatInputWidget<BiomePlantRecord>> wildPlants = new List<FloatInputWidget<BiomePlantRecord>>();
        private List<FloatInputWidget<BiomeAnimalRecord>> wildAnimals = new List<FloatInputWidget<BiomeAnimalRecord>>();
        private List<FloatInputWidget<BiomeDiseaseRecord>> diseases = new List<FloatInputWidget<BiomeDiseaseRecord>>();
        private List<ThingDef> allowedPackAnimals = new List<ThingDef>();

        private PlusMinusArgs<WeatherDef> weatherPlusMinusArgs;
        private PlusMinusArgs<SoundDef> ambientSoundPlusMinusArgs;
        private PlusMinusArgs<TerrainDef> terrainFertilityPlusMinusArgs;
        private PlusMinusArgs<IncidentDef> diseasesPlusMinusArgs;
        private PlusMinusArgs<ThingDef> wildPlantPlusMinusArgs;
        private PlusMinusArgs<PawnKindDef> wildAnimalPlusMinusArgs;
        private PlusMinusArgs<ThingDef> allowedPackAnimalsPlusMinusArgs;

        public BiomeWidget(BiomeDef d, DefType type) : base(d, type)
        {
            if (base.Def.baseWeatherCommonalities == null)
                base.Def.baseWeatherCommonalities = new List<WeatherCommonalityRecord>();
            if (base.Def.terrainsByFertility == null)
                base.Def.terrainsByFertility = new List<TerrainThreshold>();
            if (base.Def.soundsAmbient == null)
                base.Def.soundsAmbient = new List<SoundDef>();
            if (base.Def.terrainPatchMakers == null)
                base.Def.terrainPatchMakers = new List<TerrainPatchMaker>();
            if (BiomeDefStats.GetWildAnimals(base.Def) == null)
                BiomeDefStats.SetWildAnimals(base.Def, new List<BiomeAnimalRecord>());
            if (BiomeDefStats.GetWildPlants(base.Def) == null)
                BiomeDefStats.SetWildPlants(base.Def, new List<BiomePlantRecord>());
            if (BiomeDefStats.GetDiseases(base.Def) == null)
                BiomeDefStats.SetDiseases(base.Def, new List<BiomeDiseaseRecord>());
            if (BiomeDefStats.GetAllowedPackAnimals(base.Def) == null)
                BiomeDefStats.SetAllowedPackAnimals(base.Def, new List<ThingDef>());

            this.inputWidgets = new List<IInputWidget>()
            {
                new BoolInputWidget<BiomeDef>(base.Def, "Can Build Base", (BiomeDef def) => def.canBuildBase, (BiomeDef def, bool b) => def.canBuildBase = b),
                new BoolInputWidget<BiomeDef>(base.Def, "Can Auto Choose", (BiomeDef def) => def.canAutoChoose, (BiomeDef def, bool b) => def.canAutoChoose = b),
                new BoolInputWidget<BiomeDef>(base.Def, "Allow Roads", (BiomeDef def) => def.allowRoads, (BiomeDef def, bool b) => def.allowRoads = b),
                new BoolInputWidget<BiomeDef>(base.Def, "Allow Rivers", (BiomeDef def) => def.allowRivers, (BiomeDef def, bool b) => def.allowRivers = b),
                new FloatInputWidget<BiomeDef>(base.Def, "Animal Density", (BiomeDef def) => def.animalDensity, (BiomeDef def, float f) => def.animalDensity = f),
                new FloatInputWidget<BiomeDef>(base.Def, "Plant Density", (BiomeDef def) => def.plantDensity, (BiomeDef def, float f) => def.plantDensity = f),
                new FloatInputWidget<BiomeDef>(base.Def, "Disease Mtb Days", (BiomeDef def) => def.diseaseMtbDays, (BiomeDef def, float f) => def.diseaseMtbDays = f),
                new FloatInputWidget<BiomeDef>(base.Def, "Settlement Selection Weight", (BiomeDef def) => def.settlementSelectionWeight, (BiomeDef def, float f) => def.settlementSelectionWeight = f),
                new BoolInputWidget<BiomeDef>(base.Def, "Impassable", (BiomeDef def) => def.impassable, (BiomeDef def, bool b) => def.impassable = b),
                new BoolInputWidget<BiomeDef>(base.Def, "Has Virtual Plants", (BiomeDef def) => def.hasVirtualPlants, (BiomeDef def, bool b) => def.hasVirtualPlants = b),
                new FloatInputWidget<BiomeDef>(base.Def, "Forageability", (BiomeDef def) => def.forageability, (BiomeDef def, float f) => def.forageability = f),
                new BoolInputWidget<BiomeDef>(base.Def, "Local Plants Care Fertability", (BiomeDef def) => def.wildPlantsCareAboutLocalFertility, (BiomeDef def, bool b) => def.wildPlantsCareAboutLocalFertility = b),
                new FloatInputWidget<BiomeDef>(base.Def, "Wild Plant Regrow Days", (BiomeDef def) => def.wildPlantRegrowDays, (BiomeDef def, float f) => def.wildPlantRegrowDays = f),
                new FloatInputWidget<BiomeDef>(base.Def, "Movement Difficulty", (BiomeDef def) => def.movementDifficulty, (BiomeDef def, float f) => def.movementDifficulty = f),
                new BoolInputWidget<BiomeDef>(base.Def, "Has Bedrock", (BiomeDef def) => def.hasBedrock, (BiomeDef def, bool b) => def.hasBedrock = b)
            };

            this.weatherPlusMinusArgs = new PlusMinusArgs<WeatherDef>()
            {
				allItems = DefDatabase<WeatherDef>.AllDefsListForReading,
                beingUsed = () => Util.ConvertItems(this.weatherCommonalityRecords, (FloatInputWidget<WeatherCommonalityRecord> w) => w.Parent.weather),
                onAdd = delegate (WeatherDef wd)
                {
                    WeatherCommonalityRecord rec = new WeatherCommonalityRecord() { weather = wd };
                    this.weatherCommonalityRecords.Add(this.CreateWeatherWidget(rec));
                    base.Def.baseWeatherCommonalities.Add(rec);
                },
                onRemove = delegate (WeatherDef wd)
                {
                    this.weatherCommonalityRecords.RemoveAll((FloatInputWidget<WeatherCommonalityRecord> w) => w.Parent.weather == wd);
                    base.Def.baseWeatherCommonalities.RemoveAll((WeatherCommonalityRecord rec) => rec.weather == wd);
				},
				getDisplayName = (WeatherDef def) => def.label,
			};

            this.terrainFertilityPlusMinusArgs = new PlusMinusArgs<TerrainDef>()
			{
				allItems = DefDatabase<TerrainDef>.AllDefsListForReading,
                beingUsed = () => Util.ConvertItems(this.terrainsByFertility, (MinMaxInputWidget<TerrainThreshold> w) => w.Parent.terrain),
                onAdd = delegate(TerrainDef td)
                {
                    TerrainThreshold tt = new TerrainThreshold() { terrain = td };
                    this.terrainsByFertility.Add(this.CreateThresholdWidget(tt));
                    this.Def.terrainsByFertility.Add(tt);
                },
                onRemove = delegate(TerrainDef td)
                {
                    this.terrainsByFertility.RemoveAll((MinMaxInputWidget<TerrainThreshold> w) => w.Parent.terrain == td);
                    base.Def.terrainsByFertility.RemoveAll((TerrainThreshold tt) => tt.terrain == td);
				},
				getDisplayName = (TerrainDef def) => def.label,
			};

            this.ambientSoundPlusMinusArgs = new PlusMinusArgs<SoundDef>()
            {
                allItems = DefDatabase<SoundDef>.AllDefsListForReading,
                beingUsed = () => this.soundsAmbient,
                onAdd = delegate (SoundDef sd)
                {
                    this.soundsAmbient.Add(sd);
                    this.Def.soundsAmbient.Add(sd);
                },
                onRemove = delegate (SoundDef sd)
                {
                    this.soundsAmbient.Remove(sd);
                    this.Def.soundsAmbient.Remove(sd);
                },
                getDisplayName = (SoundDef sd) => sd.defName
            };

            this.diseasesPlusMinusArgs = new PlusMinusArgs<IncidentDef>()
            {
                allItems = DefDatabase<IncidentDef>.AllDefsListForReading.FindAll((IncidentDef id) => id.defName.StartsWith("Disease_")),
                beingUsed = () => Util.ConvertItems(this.diseases, (FloatInputWidget<BiomeDiseaseRecord> w) => w.Parent.diseaseInc),
                onAdd = delegate (IncidentDef id)
                {
                    BiomeDiseaseRecord r = new BiomeDiseaseRecord() { diseaseInc = id, commonality = 0 };
                    this.diseases.Add(this.CreateDiseaseWidget(r));
                    BiomeDefStats.GetDiseases(this.Def).Add(r);
                },
                onRemove = delegate (IncidentDef id)
                {
                    this.diseases.RemoveAll((FloatInputWidget<BiomeDiseaseRecord> w) => w.Parent.diseaseInc == id);
                    BiomeDefStats.GetDiseases(this.Def).RemoveAll((BiomeDiseaseRecord rec) => rec.diseaseInc == id);
                },
				getDisplayName = (IncidentDef def) => def.label,
			};

            this.wildPlantPlusMinusArgs = new PlusMinusArgs<ThingDef>()
            {
                allItems = DefDatabase<ThingDef>.AllDefsListForReading.FindAll((ThingDef td) => td.plant != null),
                beingUsed = () => Util.ConvertItems(this.wildPlants, (FloatInputWidget<BiomePlantRecord> w) => w.Parent.plant),
                onAdd = delegate (ThingDef td)
                {
                    BiomePlantRecord r = new BiomePlantRecord() { plant = td, commonality = 0 };
                    this.wildPlants.Add(this.CreateWildPlantWidget(r));
                    BiomeDefStats.GetWildPlants(this.Def).Add(r);
                },
                onRemove = delegate (ThingDef td)
                {
                    this.wildPlants.RemoveAll((FloatInputWidget<BiomePlantRecord> w) => w.Parent.plant == td);
                    BiomeDefStats.GetWildPlants(this.Def).RemoveAll((BiomePlantRecord rec) => rec.plant == td);
                },
				getDisplayName = (ThingDef def) => def.label,
            };

            this.wildAnimalPlusMinusArgs = new PlusMinusArgs<PawnKindDef>()
            {
                allItems = DefDatabase<PawnKindDef>.AllDefsListForReading.FindAll((PawnKindDef pdk) => pdk.race.race.thinkTreeMain.defName.Equals("Animal")),
                beingUsed = () => Util.ConvertItems(this.wildAnimals, (FloatInputWidget<BiomeAnimalRecord> w) => w.Parent.animal),
                onAdd = delegate (PawnKindDef td)
                {
                    BiomeAnimalRecord r = new BiomeAnimalRecord() { animal = td, commonality = 0 };
                    this.wildAnimals.Add(this.CreateWildAnimalWidget(r));
                    BiomeDefStats.GetWildAnimals(this.Def).Add(r);
                },
                onRemove = delegate (PawnKindDef td)
                {
                    this.wildAnimals.RemoveAll((FloatInputWidget<BiomeAnimalRecord> w) => w.Parent.animal == td);
                    BiomeDefStats.GetWildAnimals(this.Def).RemoveAll((BiomeAnimalRecord rec) => rec.animal == td);
                },
				getDisplayName = (PawnKindDef def) => def.label,
            };

            this.allowedPackAnimalsPlusMinusArgs = new PlusMinusArgs<ThingDef>()
            {
                allItems = DefDatabase<ThingDef>.AllDefsListForReading.FindAll((ThingDef td) => td.race != null && td.race.herdAnimal),
                beingUsed = () => this.allowedPackAnimals,
                onAdd = delegate (ThingDef td)
                {
                    this.allowedPackAnimals.Add(td);
                    BiomeDefStats.GetAllowedPackAnimals(base.Def).Add(td);
                },
                onRemove = delegate (ThingDef td)
                {
                    this.allowedPackAnimals.Remove(td);
                    BiomeDefStats.GetAllowedPackAnimals(base.Def).Remove(td);
                },
				getDisplayName = (ThingDef def) => def.label,
            };

            this.Rebuild();
        }

        public override void DrawLeft(float x, ref float y, float width)
        {
            foreach (var w in this.inputWidgets)
                w.Draw(x, ref y, width);

            WindowUtil.DrawInput(x, ref y, width, "Foraged Food", 100, (this.Def.foragedFood != null) ? this.Def.foragedFood.defName : "None",
                new WindowUtil.FloatOptionsArgs<ThingDef>()
                {
                    items = DefLookupUtil.GetSortedDefs(DefDatabase<ThingDef>.AllDefsListForReading),
                    getDisplayName = delegate (ThingDef d) { return d.defName; },
                    onSelect = delegate (ThingDef d) { this.Def.foragedFood = d; },
                    includeNullOption = true
                });

            this.DrawTerrainPatchMakers(x, ref y, width);
        }

        public override void DrawMiddle(float x, ref float y, float width)
        {
            this.DrawWeatherCommonality(x, ref y, width);
            this.DrawTerrainsByFertility(x, ref y, width);
            this.DrawAmbientSounds(x, ref y, width);
            this.DrawDiseases(x, ref y, width);
        }

        public override void DrawRight(float x, ref float y, float width)
        {
            this.DrawWildPlants(x, ref y, width);
            this.DrawWildAnimals(x, ref y, width);
            this.DrawAllowedPackAnimals(x, ref y, width);
        }

        public override void Rebuild()
        {
            this.weatherCommonalityRecords.Clear();
            base.Def.baseWeatherCommonalities.ForEach((WeatherCommonalityRecord r) => this.weatherCommonalityRecords.Add(this.CreateWeatherWidget(r)));

            this.terrainsByFertility.Clear();
            base.Def.terrainsByFertility.ForEach((TerrainThreshold tt) => this.terrainsByFertility.Add(this.CreateThresholdWidget(tt)));

            this.terrainPatchMakers.Clear();
            base.Def.terrainPatchMakers.ForEach((TerrainPatchMaker m) => this.terrainPatchMakers.Add(new TerrainPatchMakerWidget(m)));

            this.soundsAmbient.Clear();
            base.Def.soundsAmbient.ForEach((SoundDef d) => this.soundsAmbient.Add(d));

            this.diseases.Clear();
            BiomeDefStats.GetDiseases(base.Def).ForEach((BiomeDiseaseRecord r) => this.diseases.Add(this.CreateDiseaseWidget(r)));

            this.wildPlants.Clear();
            BiomeDefStats.GetWildPlants(base.Def).ForEach((BiomePlantRecord r) => this.wildPlants.Add(this.CreateWildPlantWidget(r)));

            this.wildAnimals.Clear();
            BiomeDefStats.GetWildAnimals(base.Def).ForEach((BiomeAnimalRecord r) => this.wildAnimals.Add(this.CreateWildAnimalWidget(r)));

            this.allowedPackAnimals.Clear();
            BiomeDefStats.GetAllowedPackAnimals(base.Def).ForEach((ThingDef d) => this.allowedPackAnimals.Add(d));

            this.ResetBuffers();
        }

        public override void ResetBuffers()
        {
            foreach (var w in this.inputWidgets)
                w.ResetBuffers();
            foreach (var w in this.weatherCommonalityRecords)
                w.ResetBuffers();
            foreach (var w in this.terrainsByFertility)
                w.ResetBuffers();
            foreach (var w in this.terrainPatchMakers)
                w.ResetBuffers();
            foreach (var w in this.wildPlants)
                w.ResetBuffers();
            foreach (var w in this.wildAnimals)
                w.ResetBuffers();
            foreach (var w in this.diseases)
                w.ResetBuffers();
        }

        private void DrawDiseases(float x, ref float y, float width)
        {
            PlusMinusLabel(x, ref y, width, "Disease Commonality", this.diseasesPlusMinusArgs);

            x += 10;
            foreach (var v in this.diseases)
                v.Draw(x, ref y, width);
        }

        private void DrawTerrainPatchMakers(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(x, ref y, width, "Patch Maker",
                delegate
                {
                    Find.WindowStack.Add(new Dialog_Name(
                        "Perlin Frequency",
                        delegate (string name)
                        {
                            TerrainPatchMaker m = new TerrainPatchMaker() { perlinFrequency = float.Parse(name) };
                            base.Def.terrainPatchMakers.Add(m);
                            this.terrainPatchMakers.Add(new TerrainPatchMakerWidget(m));
                        },
                        delegate (string name)
                        {
                            if (!float.TryParse(name, out float freq))
                                return "Must be a number";
                            foreach (var v in base.Def.terrainPatchMakers)
                                if (v.perlinFrequency == freq)
                                    return "Perlin Frequency must be unique";
                            return true;
                        }));
                },
                delegate
                {
                    WindowUtil.DrawFloatingOptions(
                        new WindowUtil.FloatOptionsArgs<TerrainPatchMaker>()
                        {
                            items = base.Def.terrainPatchMakers,
                            getDisplayName = (TerrainPatchMaker m) => m.perlinFrequency.ToString(),
                            onSelect = delegate (TerrainPatchMaker m)
                            {
                                base.Def.terrainPatchMakers.RemoveAll((TerrainPatchMaker tpm) => tpm.perlinFrequency == m.perlinFrequency);
                                this.terrainPatchMakers.RemoveAll((TerrainPatchMakerWidget w) => w.Parent.perlinFrequency == m.perlinFrequency);
                            }
                        });
                });

            x += 10;
            foreach (var v in this.terrainPatchMakers)
                v.Draw(x, ref y, width);
        }

        private void DrawTerrainsByFertility(float x, ref float y, float width)
        {
            PlusMinusLabel(x, ref y, width, "Terrain Fertility", this.terrainFertilityPlusMinusArgs);
            x += 10;
            foreach (var v in this.terrainsByFertility)
                v.Draw(x, ref y, width);
        }

        private void DrawAmbientSounds(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(x, ref y, width, "Ambient Sounds", this.ambientSoundPlusMinusArgs);

            x += 10;
            foreach (var w in this.soundsAmbient)
            {
                WindowUtil.DrawLabel(x, y, width, "- " + w.defName);
                y += 40;
            }
        }

        private void DrawWeatherCommonality(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(x, ref y, width, "Weather Commonality", this.weatherPlusMinusArgs);

            x += 10;
            foreach (var w in this.weatherCommonalityRecords)
                w.Draw(x, ref y, width);
        }

        private void DrawWildPlants(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(x, ref y, width, "Wild Plant Commonality", this.wildPlantPlusMinusArgs);

            x += 10;
            foreach (var w in this.wildPlants)
                w.Draw(x, ref y, width);
        }

        private void DrawWildAnimals(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(x, ref y, width, "Wild Animal Commonality", this.wildAnimalPlusMinusArgs);

            x += 10;
            foreach (var w in this.wildAnimals)
                w.Draw(x, ref y, width);
        }

        private void DrawAllowedPackAnimals(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(x, ref y, width, "Allowed Pack Animals", this.allowedPackAnimalsPlusMinusArgs);

            x += 10;
            foreach (var w in this.allowedPackAnimals)
            {
                WindowUtil.DrawLabel(x, y, width, "- " + w.defName);
                y += 40;
            }
        }

        private FloatInputWidget<BiomeDiseaseRecord> CreateDiseaseWidget(BiomeDiseaseRecord r)
        {
            return new FloatInputWidget<BiomeDiseaseRecord>(
                r, r.diseaseInc.label, (BiomeDiseaseRecord w) => w.commonality, (BiomeDiseaseRecord w, float f) => w.commonality = f);
        }

        private MinMaxInputWidget<TerrainThreshold> CreateThresholdWidget(TerrainThreshold tt)
        {
            return new MinMaxInputWidget<TerrainThreshold>(
                tt.terrain.label,
                new FloatInputWidget<TerrainThreshold>(tt, "Min", (TerrainThreshold t) => t.min, (TerrainThreshold t, float f) => t.min = f),
                new FloatInputWidget<TerrainThreshold>(tt, "Max", (TerrainThreshold t) => t.max, (TerrainThreshold t, float f) => t.max = f));
        }

        private FloatInputWidget<WeatherCommonalityRecord> CreateWeatherWidget(WeatherCommonalityRecord r)
        {
            return new FloatInputWidget<WeatherCommonalityRecord>(
                r, r.weather.label, (WeatherCommonalityRecord w) => w.commonality, (WeatherCommonalityRecord w, float f) => w.commonality = f);
        }

        private FloatInputWidget<BiomePlantRecord> CreateWildPlantWidget(BiomePlantRecord r)
        {
            return new FloatInputWidget<BiomePlantRecord>(
                r, r.plant.label, (BiomePlantRecord w) => w.commonality, (BiomePlantRecord w, float f) => w.commonality = f);
        }

        private FloatInputWidget<BiomeAnimalRecord> CreateWildAnimalWidget(BiomeAnimalRecord r)
        {
            return new FloatInputWidget<BiomeAnimalRecord>(
                r, r.animal.label, (BiomeAnimalRecord w) => w.commonality, (BiomeAnimalRecord w, float f) => w.commonality = f);
        }
    }
}
