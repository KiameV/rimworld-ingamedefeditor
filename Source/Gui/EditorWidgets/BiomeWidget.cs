using InGameDefEditor.Gui.EditorWidgets.Misc;
using InGameDefEditor.Stats;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using System;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class BiomeWidget : AParentStatWidget<BiomeDef>
    {
        private readonly List<IInputWidget> inputWidgets;

        private List<FloatInputWidget<WeatherCommonalityRecord>> weatherCommonalityRecords = new List<FloatInputWidget<WeatherCommonalityRecord>>();
        //private List<FloatMinMaxInputWidget<TerrainDef>> terrainsByFertility = new List<FloatMinMaxInputWidget<TerrainDef>>();
        private List<SoundDef> soundsAmbient = new List<SoundDef>();
        //private List<TerrainPatchMakerWidget> terrainPatchMakers = new List<TerrainPatchMakerWidget>();
        private List<FloatInputWidget<ThingDef>> wildPlants = new List<FloatInputWidget<ThingDef>>();
        private List<FloatInputWidget<PawnKindDef>> wildAnimals = new List<FloatInputWidget<PawnKindDef>>();
        private List<FloatInputWidget<IncidentDef>> diseases = new List<FloatInputWidget<IncidentDef>>();
        private List<PawnKindDef> allowedPackAnimals = new List<PawnKindDef>();

        public BiomeWidget(BiomeDef d, WidgetType type) : base(d, type)
        {
            if (base.Def.baseWeatherCommonalities == null)
                base.Def.baseWeatherCommonalities = new List<WeatherCommonalityRecord>();
            if (base.Def.terrainsByFertility == null)
                base.Def.terrainsByFertility = new List<TerrainThreshold>();
            if (base.Def.soundsAmbient == null)
                base.Def.soundsAmbient = new List<SoundDef>();
            if (base.Def.terrainPatchMakers == null)
                base.Def.terrainPatchMakers = new List<TerrainPatchMaker>();
            if (BiomeStats.GetWildAnimals(base.Def) == null)
                BiomeStats.SetWildAnimals(base.Def, new List<BiomeAnimalRecord>());
            if (BiomeStats.GetWildPlants(base.Def) == null)
                BiomeStats.SetWildPlants(base.Def, new List<BiomePlantRecord>());
            if (BiomeStats.GetDiseases(base.Def) == null)
                BiomeStats.SetDiseases(base.Def, new List<BiomeDiseaseRecord>());
            if (BiomeStats.GetAllowedPackAnimals(base.Def) == null)
                BiomeStats.SetAllowedPackAnimals(base.Def, new List<ThingDef>());

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

            this.Rebuild();
        }

        public override void DrawLeft(float x, ref float y, float width)
        {
            foreach (var w in this.inputWidgets)
                w.Draw(x, ref y, width);

            WindowUtil.DrawInput(x, ref y, width, "Foraged Food", 100, (this.Def.foragedFood != null) ? this.Def.foragedFood.defName : "<none>",
                new WindowUtil.DrawFloatOptionsArgs<ThingDef>()
                {
                    items = DefLookupUtil.GetSortedDefs(DefDatabase<ThingDef>.AllDefsListForReading),
                    getDisplayName = delegate (ThingDef d) { return d.defName; },
                    onSelect = delegate (ThingDef d) { this.Def.foragedFood = d; },
                    includeNullOption = true
                });
        }

        public override void DrawMiddle(float x, ref float y, float width)
        {
            this.DrawWeatherCommonality(x, ref y, width);
            this.DrawTerrainsByFertility(x, ref y, width);
            this.DrawAmbientSounds(x, ref y, width);
        }

        public override void DrawRight(float x, ref float y, float width)
        {

        }

        public override void Rebuild()
        {
            this.weatherCommonalityRecords.Clear();
            if (base.Def.baseWeatherCommonalities != null)
                base.Def.baseWeatherCommonalities.ForEach((WeatherCommonalityRecord r) => this.weatherCommonalityRecords.Add(
                    new FloatInputWidget<WeatherCommonalityRecord>(r, r.weather.label, (WeatherCommonalityRecord w) => w.commonality, (WeatherCommonalityRecord w, float f) => w.commonality = f)));

            // TODO TerrainsByFertility

            this.soundsAmbient.Clear();
            if (base.Def.soundsAmbient != null)
                base.Def.soundsAmbient.ForEach((SoundDef d) => this.soundsAmbient.Add(d));

            this.ResetBuffers();
        }

        public override void ResetBuffers()
        {
            foreach (var w in this.inputWidgets)
                w.ResetBuffers();

            foreach (var w in this.weatherCommonalityRecords)
                w.ResetBuffers();
        }

        private void DrawTerrainsByFertility(float x, ref float y, float width)
        {
            // TODO
        }

        private void DrawAmbientSounds(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(
                      x, ref y, 150, "Ambient Sounds",
                      new WindowUtil.DrawFloatOptionsArgs<SoundDef>()
                      {
                       // Add
                       getDisplayName = (SoundDef d) => d.defName,
                          updateItems = delegate ()
                          {
                              HashSet<SoundDef> lookup = new HashSet<SoundDef>();
                              base.Def.soundsAmbient.ForEach((SoundDef d) => lookup.Add(d));

                              IEnumerable<SoundDef> defs = DefDatabase<SoundDef>.AllDefsListForReading;
                              List<SoundDef> list = new List<SoundDef>(defs.Count());
                              foreach (var d in defs)
                                  if (!lookup.Contains(d))
                                      list.Add(d);
                              return list;
                          },
                          onSelect = delegate (SoundDef d)
                          {
                              base.Def.soundsAmbient.Add(d);
                              this.soundsAmbient.Add(d);
                          }
                      },
                      new WindowUtil.DrawFloatOptionsArgs<SoundDef>()
                      {
                       // Remove
                       getDisplayName = (SoundDef d) => d.defName,
                          updateItems = delegate ()
                          {
                              List<SoundDef> l = new List<SoundDef>();
                              foreach (var v in base.Def.soundsAmbient)
                                  l.Add(v);
                              return l;
                          },
                          onSelect = delegate (SoundDef d)
                          {
                              base.Def.soundsAmbient.Remove(d);
                              this.soundsAmbient.Remove(d);
                          }
                      });

            x += 10;
            foreach (var w in this.soundsAmbient)
            {
                WindowUtil.DrawLabel(x, y, width, "- " + w.defName);
                y += 40;
            }

            y += 40;
        }

        private void DrawWeatherCommonality(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(
                   x, ref y, 150, "Weather Commonality",
                   new WindowUtil.DrawFloatOptionsArgs<WeatherDef>()
                   {
                       // Add
                       getDisplayName = (WeatherDef d) => d.label,
                       updateItems = delegate ()
                       {
                           HashSet<WeatherDef> lookup = new HashSet<WeatherDef>();
                           base.Def.baseWeatherCommonalities.ForEach((WeatherCommonalityRecord r) => lookup.Add(r.weather));

                           IEnumerable<WeatherDef> defs = DefDatabase<WeatherDef>.AllDefsListForReading;
                           List<WeatherDef> list = new List<WeatherDef>(defs.Count());
                           foreach (var d in defs)
                               if (!lookup.Contains(d))
                                   list.Add(d);
                           return list;
                       },
                       onSelect = delegate (WeatherDef d)
                       {
                           WeatherCommonalityRecord r = new WeatherCommonalityRecord()
                           {
                               weather = d,
                               commonality = 0
                           };
                           base.Def.baseWeatherCommonalities.Add(r);
                           this.weatherCommonalityRecords.Add(new FloatInputWidget<WeatherCommonalityRecord>(
                               r, r.weather.label, (WeatherCommonalityRecord rec) => rec.commonality, (WeatherCommonalityRecord rec, float f) => rec.commonality = f));
                       }
                   },
                   new WindowUtil.DrawFloatOptionsArgs<WeatherDef>()
                   {
                       // Remove
                       getDisplayName = (WeatherDef d) => d.label,
                       updateItems = delegate ()
                       {
                           List<WeatherDef> l = new List<WeatherDef>();
                           foreach (var v in base.Def.baseWeatherCommonalities)
                               l.Add(v.weather);
                           return l;
                       },
                       onSelect = delegate (WeatherDef d)
                       {
                           for (int i = 0; i < base.Def.baseWeatherCommonalities.Count; ++i)
                               if (base.Def.baseWeatherCommonalities[i].weather == d)
                               {
                                   base.Def.baseWeatherCommonalities.RemoveAt(i);
                                   break;
                               }

                           for (int i = 0; i < this.weatherCommonalityRecords.Count; ++i)
                               if (this.weatherCommonalityRecords[i].Parent.weather == d)
                               {
                                   this.weatherCommonalityRecords.RemoveAt(i);
                                   break;
                               }
                       }
                   });

            x += 10;
            foreach (var w in this.weatherCommonalityRecords)
                w.Draw(x, ref y, width);

            y += 40;
        }
    }
}
