using InGameDefEditor.Gui.EditorWidgets.Misc;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class ToolWidget : IDefEditorWidget
    {
        public readonly Tool Tool;

        private readonly List<IInputWidget> inputWidgets;

        public ToolWidget(Tool tool)
        {
            this.Tool = tool;

            if (this.Tool.capacities == null)
                this.Tool.capacities = new List<ToolCapacityDef>();

            inputWidgets = new List<IInputWidget>()
            {
                new FloatInputWidget<Tool>(this.Tool, "Power", (Tool t) => this.Tool.power, (Tool t, float f) => this.Tool.power = f),
                new FloatInputWidget<Tool>(this.Tool, "Armor Penetration", (Tool t) => this.Tool.armorPenetration, (Tool t, float f) => this.Tool.armorPenetration = f),
                new FloatInputWidget<Tool>(this.Tool, "Cooldown Time", (Tool t) => this.Tool.cooldownTime, (Tool t, float f) => this.Tool.cooldownTime = f)
            };

            this.ResetBuffers();
        }

        public string DisplayLabel => this.Tool.label;

        public void Draw(float x, ref float y, float width)
        {
            WindowUtil.DrawLabel(x, y, 300, this.DisplayLabel);
            y += 40;

            foreach (IInputWidget w in this.inputWidgets)
                w.Draw(x, ref y, width);

            x += 20;
            WindowUtil.PlusMinusLabel(
                x, ref y, 100, "Capabilities",
                new WindowUtil.DrawFloatOptionsArgs<ToolCapacityDef>()
                {
                    // Add
                    getDisplayName = delegate (ToolCapacityDef d) { return d.defName; },
                    updateItems = delegate()
                    {
                        HashSet<ToolCapacityDef> lookup = new HashSet<ToolCapacityDef>();
                        if (this.Tool.capacities != null)
                            this.Tool.capacities.ForEach((ToolCapacityDef d) => lookup.Add(d));

                        IEnumerable<ToolCapacityDef> defs = DefDatabase<ToolCapacityDef>.AllDefsListForReading;
                        List<ToolCapacityDef> list = new List<ToolCapacityDef>(defs.Count());
                        foreach (var tool in defs)
                            if (!lookup.Contains(tool))
                                list.Add(tool);
                        return list;
                    },
                    onSelect = delegate (ToolCapacityDef d) { this.Tool.capacities.Add(d); }
                },
                new WindowUtil.DrawFloatOptionsArgs<ToolCapacityDef>()
                {
                    // Remove
                    items = this.Tool.capacities,
                    getDisplayName = delegate (ToolCapacityDef d) { return d.defName; },
                    onSelect = delegate (ToolCapacityDef d) { this.Tool.capacities.Remove(d); }
                });

            x += 10;
            foreach (ToolCapacityDef d in this.Tool.capacities)
            {
                Widgets.Label(new Rect(x, y, 150, 32), "- " + d.defName);
                y += 40;
            }
        }

        public void ResetBuffers()
        {
            foreach (IInputWidget w in this.inputWidgets)
                w.ResetBuffers();
        }
    }
}