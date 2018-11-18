using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
    class TerrainPatchMakerWidget : IStatWidget
    {
        public readonly TerrainPatchMaker Parent;

        public string DisplayLabel => Parent.perlinFrequency.ToString();

        private List<IInputWidget> inputWidgets;
        private List<MinMaxInputWidget<TerrainThreshold>> thresholds;
        private PlusMinusArgs<TerrainDef> thresholdsArgs;

        public TerrainPatchMakerWidget(TerrainPatchMaker parent)
        {
            this.Parent = parent;
            this.inputWidgets = new List<IInputWidget>(6)
            {
                //new FloatInputWidget<TerrainPatchMaker>(this.Parent, "Perlin Frequency", (TerrainPatchMaker p) => p.perlinFrequency, (TerrainPatchMaker p, float f) => p.perlinFrequency = f),
                new FloatInputWidget<TerrainPatchMaker>(this.Parent, "Perlin Lacunarity", (TerrainPatchMaker p) => p.perlinLacunarity, (TerrainPatchMaker p, float f) => p.perlinLacunarity = f),
                new FloatInputWidget<TerrainPatchMaker>(this.Parent, "Perlin Persistence", (TerrainPatchMaker p) => p.perlinPersistence, (TerrainPatchMaker p, float f) => p.perlinPersistence = f),
                new IntInputWidget<TerrainPatchMaker>(this.Parent, "Perlin Octaves", (TerrainPatchMaker p) => p.perlinOctaves, (TerrainPatchMaker p, int i) => p.perlinOctaves = i),
                new FloatInputWidget<TerrainPatchMaker>(this.Parent, "Min Fertility", (TerrainPatchMaker p) => p.minFertility, (TerrainPatchMaker p, float f) => p.minFertility = f),
                new FloatInputWidget<TerrainPatchMaker>(this.Parent, "Max Fertility", (TerrainPatchMaker p) => p.maxFertility, (TerrainPatchMaker p, float f) => p.maxFertility = f),
                new IntInputWidget<TerrainPatchMaker>(this.Parent, "Min Size", (TerrainPatchMaker p) => p.minSize, (TerrainPatchMaker p, int i) => p.minSize = i),
            };

            if (this.Parent.thresholds != null)
            {
                this.thresholds = new List<MinMaxInputWidget<TerrainThreshold>>(this.Parent.thresholds.Count);
                foreach (var v in this.Parent.thresholds)
                {
                    this.thresholds.Add(this.CreateMinMaxInputWidget(v));
                }
            }
            else
                this.thresholds = new List<MinMaxInputWidget<TerrainThreshold>>(0);

            this.thresholdsArgs = new PlusMinusArgs<TerrainDef>()
            {
                allItems = DefDatabase<TerrainDef>.AllDefsListForReading,
                beingUsed = delegate ()
                {
                    List<TerrainDef> l = new List<TerrainDef>();
                    foreach (var v in this.thresholds)
                        l.Add(v.Parent.terrain);
                    return l;
                },
                onAdd = delegate (TerrainDef d)
                {
                    TerrainThreshold tt = new TerrainThreshold() { terrain = d };
                    this.Parent.thresholds.Add(tt);
                    this.thresholds.Add(this.CreateMinMaxInputWidget(tt));
                },
                onRemove = delegate (TerrainDef d)
                {
                    this.Parent.thresholds.RemoveAll((TerrainThreshold tt) => tt.terrain == d);
                    this.thresholds.RemoveAll((MinMaxInputWidget<TerrainThreshold> mmw) => mmw.Parent.terrain == d);
                },
				getDisplayName = (TerrainDef def) => def.label
			};
        }

        public void Draw(float x, ref float y, float width)
        {
            WindowUtil.DrawLabel(x, y, width, "Perlin Frequency " + this.Parent.perlinFrequency, true);
            y += 40;

            x += 10;
            foreach (var v in this.inputWidgets)
                v.Draw(x, ref y, width);

            WindowUtil.PlusMinusLabel(x, ref y, 100, "Thresholds", this.thresholdsArgs);

            x += 10;
            foreach (var v in this.thresholds)
                v.Draw(x, ref y, width);
        }

        public void ResetBuffers()
        {
            foreach (var v in this.thresholds)
                v.ResetBuffers();
        }

        private MinMaxInputWidget<TerrainThreshold> CreateMinMaxInputWidget(TerrainThreshold tt)
        {
            return new MinMaxInputWidget<TerrainThreshold>(
                tt.terrain.label,
                new FloatInputWidget<TerrainThreshold>(tt, "Min", (TerrainThreshold t) => t.min, (TerrainThreshold t, float f) => t.min = f),
                new FloatInputWidget<TerrainThreshold>(tt, "Max", (TerrainThreshold t) => t.max, (TerrainThreshold t, float f) => t.max = f));
        }
    }
}
