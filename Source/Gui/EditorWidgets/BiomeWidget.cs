using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class BiomeWidget : AParentStatWidget<BiomeDef>
    {
        private readonly List<IInputWidget> inputWidgets;

        public BiomeWidget(BiomeDef d, WidgetType type) : base(d, type)
        {
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

        }

        public override void DrawRight(float x, ref float y, float width)
        {

        }

        public override void Rebuild()
        {
            this.ResetBuffers();
        }

        public override void ResetBuffers()
        {
            foreach (var w in this.inputWidgets)
                w.ResetBuffers();
        }
    }
}
