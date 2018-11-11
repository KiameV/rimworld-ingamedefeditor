using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class ToolWidget : IDefEditorWidget
    {
        public readonly Tool Tool;

        private string[] comBuffer = new string[3];

        public ToolWidget(Tool t)
        {
            this.Tool = t;

            if (this.Tool.capacities == null)
                this.Tool.capacities = new List<ToolCapacityDef>();

            this.ResetBuffers();
        }

        public string DisplayLabel => this.Tool.label;

        public void Draw(float x, ref float y, float width)
        {
            WindowUtil.DrawLabel(x, y, 300, this.DisplayLabel);
            y += 40;

            this.comBuffer[0] = WindowUtil.DrawInput(x + 20, ref y, "Power", ref this.Tool.power, this.comBuffer[0]);
            this.comBuffer[1] = WindowUtil.DrawInput(x + 20, ref y, "Armor Penetration", ref this.Tool.armorPenetration, this.comBuffer[1]);
            this.comBuffer[2] = WindowUtil.DrawInput(x + 20, ref y, "Cooldown Time", ref this.Tool.cooldownTime, this.comBuffer[2]);

            x += 20;
            WindowUtil.PlusMinusLabel(
                x, ref y, 100, "Capabilities",
                new WindowUtil.DrawFloatOptionsArgs<ToolCapacityDef>()
                {
                    // Add
                    getDisplayName = delegate (ToolCapacityDef d) { return d.defName; },
                    updateItems = delegate()
                    {
                        IEnumerable<ToolCapacityDef> defs = DefDatabase<ToolCapacityDef>.AllDefsListForReading;
                        List<ToolCapacityDef> list = new List<ToolCapacityDef>(defs);
                        foreach (var tool in defs)
                            if (!this.Tool.capacities.Contains(tool))
                                list.Add(tool);
                        return list;
                    },
                    onSelect = delegate (ToolCapacityDef d) { this.Tool.capacities.Add(d); }
                },
                new WindowUtil.DrawFloatOptionsArgs<ToolCapacityDef>()
                {
                    // Subtract
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
            comBuffer[0] = this.Tool.power.ToString();
            comBuffer[1] = this.Tool.armorPenetration.ToString();
            comBuffer[2] = this.Tool.cooldownTime.ToString();
        }
    }
}