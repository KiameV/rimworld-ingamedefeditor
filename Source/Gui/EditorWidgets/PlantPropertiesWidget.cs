using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class PlantPropertiesWidget : IInputWidget
	{
		private readonly PlantProperties Parent;

        private List<IInputWidget> inputWidgets;

        //private PlusMinusArgs<BiomeDef> wildBiomes;
        //private List<FloatInputWidget<PlantBiomeRecord>> wildBiomesWidgets;

        public PlantPropertiesWidget(PlantProperties parent)
        {
            this.Parent = parent;

            /*if (this.Parent.wildBiomes == null)
                this.Parent.wildBiomes = new List<PlantBiomeRecord>();
            this.wildBiomesWidgets = new List<FloatInputWidget<PlantBiomeRecord>>();
            this.wildBiomes = new PlusMinusArgs<BiomeDef>()
            {
                allItems = DefDatabase<BiomeDef>.AllDefs,
                beingUsed = () =>
                {
                    List<BiomeDef> l = new List<BiomeDef>(this.Parent.wildBiomes.Count);
                    foreach (var d in this.Parent.wildBiomes)
                        l.Add(d.biome);
                    return l;
                },
                onAdd = (def) => {
                    var r = new PlantBiomeRecord() { biome = def, commonality = 0 };
                    this.Parent.wildBiomes.Add(r);
                    wildBiomesWidgets.Add(new FloatInputWidget<PlantBiomeRecord>(r, r.biome.label, p => p.commonality, (p, f) => p.commonality = f));
                },
                onRemove = (def) => {
                    this.Parent.wildBiomes.RemoveAll(r => r.biome == def);
                    this.wildBiomesWidgets.RemoveAll(r => r.DisplayLabel == def.label);
                },
                getDisplayName = (def) => def.label
            };*/

            this.inputWidgets = new List<IInputWidget>()
            {
                new IntInputWidget<PlantProperties>(this.Parent, "Wild Cluster Radius", p => p.wildClusterRadius, (p, i) => p.wildClusterRadius = i),
                new FloatInputWidget<PlantProperties>(this.Parent, "Wild Cluster Weight", p => p.wildClusterWeight, (p, f) => p.wildClusterWeight = f),
                new FloatInputWidget<PlantProperties>(this.Parent, "Wild Order", p => p.wildOrder, (p, f) => p.wildOrder = f),
                new BoolInputWidget<PlantProperties>(this.Parent, "Wild Equal Local Distribution", p => p.wildEqualLocalDistribution, (p, b) => p.wildEqualLocalDistribution = b),
                new BoolInputWidget<PlantProperties>(this.Parent, "Cave Plant", p => p.cavePlant, (p, b) => p.cavePlant = b),
                new FloatInputWidget<PlantProperties>(this.Parent, "Cave Plant Weight", p => p.cavePlantWeight, (p, f) => p.cavePlantWeight = f),
                new FloatInputWidget<PlantProperties>(this.Parent, "Sow Work", p => p.sowWork, (p, f) => p.sowWork = f),
                new IntInputWidget<PlantProperties>(this.Parent, "Sow Min Skill", p => p.sowMinSkill, (p, i) => p.sowMinSkill = i),
                new BoolInputWidget<PlantProperties>(this.Parent, "Block Adjacent Sow", p => p.blockAdjacentSow, (p, b) => p.blockAdjacentSow = b),
                new BoolInputWidget<PlantProperties>(this.Parent, "Must Be Wild To Sow", p => p.mustBeWildToSow, (p, b) => p.mustBeWildToSow = b),
                new FloatInputWidget<PlantProperties>(this.Parent, "Harvest Work", p => p.harvestWork, (p, f) => p.harvestWork = f),
                new FloatInputWidget<PlantProperties>(this.Parent, "Harvest Yield", p => p.harvestYield, (p, f) => p.harvestYield = f),
                new DefInputWidget<PlantProperties, ThingDef>(this.Parent, "Harvested ThingDef", 200, def => def.harvestedThingDef, (def, v) => def.harvestedThingDef = v, false),
                new FloatInputWidget<PlantProperties>(this.Parent, "Harvest Min Growth", p => p.harvestMinGrowth, (p, f) => p.harvestMinGrowth = f),
                new FloatInputWidget<PlantProperties>(this.Parent, "Harvest After Growth", p => p.harvestAfterGrowth, (p, f) => p.harvestAfterGrowth = f),
                new BoolInputWidget<PlantProperties>(this.Parent, "Harvest Failable", p => p.harvestFailable, (p, b) => p.harvestFailable = b),
                new DefInputWidget<PlantProperties, SoundDef>(this.Parent, "Sound Harvesting", 200, def => def.soundHarvesting, (def, v) => def.soundHarvesting = v, false),
                new DefInputWidget<PlantProperties, SoundDef>(this.Parent, "Sound Harvest Finish", 200, def => def.soundHarvestFinish, (def, v) => def.soundHarvestFinish = v, false),
                new BoolInputWidget<PlantProperties>(this.Parent, "Auto Harvestable", p => p.autoHarvestable, (p, b) => p.autoHarvestable = b),
                new FloatInputWidget<PlantProperties>(this.Parent, "Grow Days", p => p.growDays, (p, f) => p.growDays = f),
                new FloatInputWidget<PlantProperties>(this.Parent, "Lifespan Days Per Grow Days", p => p.lifespanDaysPerGrowDays, (p, f) => p.lifespanDaysPerGrowDays = f),
                new FloatInputWidget<PlantProperties>(this.Parent, "Grow Min Glow", p => p.growMinGlow, (p, f) => p.growMinGlow = f),
                new FloatInputWidget<PlantProperties>(this.Parent, "Grow Optimal Glow", p => p.growOptimalGlow, (p, f) => p.growOptimalGlow = f),
                new FloatInputWidget<PlantProperties>(this.Parent, "Fertility Min", p => p.fertilityMin, (p, f) => p.fertilityMin = f),
                new FloatInputWidget<PlantProperties>(this.Parent, "Fertility Sensitivity", p => p.fertilitySensitivity, (p, f) => p.fertilitySensitivity = f),
                new BoolInputWidget<PlantProperties>(this.Parent, "Die If Leafless", p => p.dieIfLeafless, (p, b) => p.dieIfLeafless = b),
                new BoolInputWidget<PlantProperties>(this.Parent, "Never Blightable", p => p.neverBlightable, (p, b) => p.neverBlightable = b),
                new BoolInputWidget<PlantProperties>(this.Parent, "Interferes With Roof", p => p.interferesWithRoof, (p, b) => p.interferesWithRoof = b),
                new BoolInputWidget<PlantProperties>(this.Parent, "Die If No Sunlight", p => p.dieIfNoSunlight, (p, b) => p.dieIfNoSunlight = b),
                new BoolInputWidget<PlantProperties>(this.Parent, "Die From Toxic Fallout", p => p.dieFromToxicFallout, (p, b) => p.dieFromToxicFallout = b),
                new EnumInputWidget<PlantProperties, PlantPurpose>(this.Parent, "Purpose", 200, p => p.purpose, (p, e) => p.purpose = e),
                new BoolInputWidget<PlantProperties>(this.Parent, "Human Food Plant", p => p.humanFoodPlant, (p, b) => p.humanFoodPlant = b),
                new BoolInputWidget<PlantProperties>(this.Parent, "Tree Lovers Care If Chopped", p => p.treeLoversCareIfChopped, (p, b) => p.treeLoversCareIfChopped = b),
                new BoolInputWidget<PlantProperties>(this.Parent, "Allow Auto Cut", p => p.allowAutoCut, (p, b) => p.allowAutoCut = b),
                new EnumInputWidget<PlantProperties, TreeCategory>(this.Parent, "Tree Category", 200, p => p.treeCategory, (p, e) => p.treeCategory = e),
                new DefInputWidget<PlantProperties, ThingDef>(this.Parent, "Burned ThingDef", 200, def => def.burnedThingDef, (def, v) => def.burnedThingDef = v, false),
                new BoolInputWidget<PlantProperties>(this.Parent, "Show Growth In Inspect Pane", p => p.showGrowthInInspectPane, (p, b) => p.showGrowthInInspectPane = b),
                new FloatInputWidget<PlantProperties>(this.Parent, "Min Spacing Between Same Plant", p => p.minSpacingBetweenSamePlant, (p, f) => p.minSpacingBetweenSamePlant = f),
                new FloatInputWidget<PlantProperties>(this.Parent, "Top Wind Exposure", p => p.topWindExposure, (p, f) => p.topWindExposure = f),
                new IntInputWidget<PlantProperties>(this.Parent, "Max Mesh Count", p => p.maxMeshCount, (p, i) => p.maxMeshCount = i),
                new BoolInputWidget<PlantProperties>(this.Parent, "Drop Leaves", p => p.dropLeaves, (p, b) => p.dropLeaves = b),
            };

            this.ResetBuffers();
        }

        public string DisplayLabel => throw new NotImplementedException();

        public void Draw(float x, ref float y, float width)
        {
            //WindowUtil.PlusMinusLabel(x, ref y, width, "Wild Biomes", this.wildBiomes);
            //foreach (var v in this.wildBiomesWidgets)
            //    v.Draw(x + 10, ref y, width);

            foreach (var v in this.inputWidgets)
                v.Draw(x, ref y, width);
        }

        public void ResetBuffers()
        {
            foreach (var v in this.inputWidgets)
                v.ResetBuffers();
            /*this.wildBiomesWidgets.Clear();
            foreach (var v in this.Parent.wildBiomes)
                wildBiomesWidgets.Add(new FloatInputWidget<PlantBiomeRecord>(v, v.biome.label, p => p.commonality, (p, f) => p.commonality = f));*/
        }
    }
}
