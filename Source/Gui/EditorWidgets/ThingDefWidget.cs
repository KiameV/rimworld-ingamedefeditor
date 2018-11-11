using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class ThingDefWidget : IParentStatWidget
    {
        public readonly ThingDef ThingDef;
        public readonly WidgetType type;

        private List<DefStatValueWidget> StatModifiers = new List<DefStatValueWidget>();
        private List<VerbWidget> VerbWidgets = new List<VerbWidget>();
        private List<ToolWidget> ToolWidgets = new List<ToolWidget>();
        private List<DefStatValueWidget> EquipmentModifiers = new List<DefStatValueWidget>();

        public string DisplayLabel => ThingDef.label;
        public WidgetType Type => type;

        private readonly ProjectileDefWidget projectileWidget = null;

        public ThingDefWidget(ThingDef d, WidgetType type)
        {
            this.ThingDef = d;
            this.type = type;

            if (this.type == WidgetType.Projectile)
                this.projectileWidget = new ProjectileDefWidget(d);

            if (this.ThingDef.statBases == null)
                this.ThingDef.statBases = new List<StatModifier>();

            if (this.ThingDef.equippedStatOffsets == null)
                this.ThingDef.equippedStatOffsets = new List<StatModifier>();

            this.Rebuild();
        }

        public void DrawLeft(float x, ref float y, float width)
        {
            if (this.type == WidgetType.Apparel || 
                this.type == WidgetType.Weapon)
            {
                this.DrawStatModifiers(x, ref y, width);
            }
            else if (this.type == WidgetType.Projectile)
            {
                this.projectileWidget.Draw(x, ref y, width);
            }
        }

        public void DrawMiddle(float x, ref float y, float width)
        {
            if (this.type == WidgetType.Apparel)
            {
                this.DrawEquipmentStatOffsets(x, ref y, width);
            }
            else if (this.type == WidgetType.Weapon)
            {
                this.DrawVerbs(x, ref y, width);
                y += 20;
                this.DrawTools(x, ref y, width);
            }
        }

        public void DrawRight(float x, ref float y, float width)
        {
            if (this.type == WidgetType.Weapon)
            {
                this.DrawEquipmentStatOffsets(x, ref y, width);
            }
        }

        private void DrawVerbs(float x, ref float y, float width)
        {
            foreach (VerbWidget w in this.VerbWidgets)
            {
                w.Draw(x, ref y, width);
            }
        }

        private void DrawStatModifiers(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(
                x, ref y, 150, "Base Modifiers",
                new WindowUtil.DrawFloatOptionsArgs<StatDef>()
                {
                    items = this.GetPossibleStatModifiers(this.ThingDef.statBases),
                    getDisplayName = delegate (StatDef d) { return d.label; },
                    onSelect = delegate (StatDef d)
                    {
                        // Add
                        StatModifier m = new StatModifier()
                        {
                            stat = d,
                            value = 0
                        };
                        this.ThingDef.statBases.Add(m);
                        this.StatModifiers.Add(new DefStatValueWidget(m));
                    }
                },
                new WindowUtil.DrawFloatOptionsArgs<StatModifier>()
                {
                    items = this.ThingDef.statBases,
                    getDisplayName = delegate (StatModifier s) { return s.stat.defName; },
                    onSelect = delegate (StatModifier s)
                    {
                        // Remove
                        for (int i = 0; i < this.StatModifiers.Count; ++i)
                            if (this.StatModifiers[i].StatModifier.stat == s.stat)
                            {
                                this.StatModifiers.RemoveAt(i);
                                break;
                            }

                        for (int i = 0; i < this.ThingDef.statBases.Count; ++i)
                            if (this.ThingDef.statBases[i].stat == s.stat)
                            {
                                this.ThingDef.statBases.RemoveAt(i);
                                break;
                            }
                    }
                });
            x += 10;
            foreach (DefStatValueWidget w in this.StatModifiers)
            {
                w.Draw(x, ref y, width);
            }
        }

        private void DrawTools(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(x, ref y, 100, "Tools",
                delegate
                {
                    Find.WindowStack.Add(new Dialog_Name(
                        "Name the new tool",
                        delegate (string name)
                        {
                            Tool t = new Tool() { label = name };
                            this.ThingDef.tools.Add(t);
                            this.ToolWidgets.Add(new ToolWidget(t));
                        },
                        delegate (string name)
                        {
                            foreach (Tool t in this.ThingDef.tools)
                            {
                                if (t.label.Equals(name))
                                {
                                    return "Tool with name \"" + name + "\" already exists.";
                                }
                            }
                            return true;
                        }));
                },
                delegate
                {
                    WindowUtil.DrawFloatingOptions(
                        new WindowUtil.DrawFloatOptionsArgs<Tool>()
                        {
                            items = this.ThingDef.tools,
                            getDisplayName = delegate (Tool t) { return t.label; },
                            onSelect = delegate (Tool t)
                            {
                                for (int i = 0; i < this.ThingDef.tools.Count; ++i)
                                {
                                    if (this.ThingDef.tools[i].label.Equals(t.label))
                                    {
                                        this.ThingDef.tools.RemoveAt(i);
                                        break;
                                    }
                                }
                                for (int i = 0; i < this.ToolWidgets.Count; ++i)
                                {
                                    if (this.ToolWidgets[i].Tool.label.Equals(t.label))
                                    {
                                        this.ToolWidgets.RemoveAt(i);
                                        break;
                                    }
                                }
                            }
                        });
                });
            y += 10;

            x += 10;

            foreach (ToolWidget w in this.ToolWidgets)
            {
                w.Draw(x, ref y, width - x);
            }
        }

        private void DrawEquipmentStatOffsets(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(x, ref y, 200, "Equipped Stat Offsets",
                new WindowUtil.DrawFloatOptionsArgs<StatDef>()
                {
                    items = this.GetPossibleStatModifiers(this.ThingDef.equippedStatOffsets),
                    getDisplayName = delegate (StatDef d) { return d.label; },
                    onSelect = delegate (StatDef d)
                    {
                        StatModifier m = new StatModifier()
                        {
                            stat = d,
                            value = 0
                        };
                        this.ThingDef.equippedStatOffsets.Add(m);
                        this.EquipmentModifiers.Add(new DefStatValueWidget(m));
                    }
                },
                new WindowUtil.DrawFloatOptionsArgs<StatModifier>()
                {
                    items = this.ThingDef.equippedStatOffsets,
                    getDisplayName = delegate (StatModifier s) { return s.stat.defName; },
                    onSelect = delegate (StatModifier s)
                    {
                        for (int i = 0; i < this.EquipmentModifiers.Count; ++i)
                        {
                            if (this.EquipmentModifiers[i].StatModifier.stat == s.stat)
                            {
                                this.EquipmentModifiers.RemoveAt(i);
                                break;
                            }
                        }
                        for (int i = 0; i < this.ThingDef.equippedStatOffsets.Count; ++i)
                        {
                            if (this.ThingDef.equippedStatOffsets[i].stat == s.stat)
                            {
                                this.ThingDef.equippedStatOffsets.RemoveAt(i);
                                break;
                            }
                        }
                    }
                });

            foreach (DefStatValueWidget w in this.EquipmentModifiers)
            {
                w.Draw(x, ref y, width);
            }
        }

        private IEnumerable<StatDef> GetPossibleStatModifiers(IEnumerable<StatModifier> statModifiers)
        {
            HashSet<string> lookup = new HashSet<string>();
            if (statModifiers != null)
            {
                foreach (StatModifier s in statModifiers)
                    lookup.Add(s.stat.defName);
            }

            SortedDictionary<string, StatDef> sorted = new SortedDictionary<string, StatDef>();
            foreach (StatDef d in DefDatabase<StatDef>.AllDefsListForReading)
                if (!lookup.Contains(d.defName))
                    sorted[d.label] = d;

            return sorted.Values;
        }

        public void Rebuild()
        {
            if (this.ThingDef.statBases != null)
            {
                this.StatModifiers.Clear();
                foreach (StatModifier s in this.ThingDef.statBases)
                {
                    this.StatModifiers.Add(new DefStatValueWidget(s));
                }
            }
            if (this.ThingDef.Verbs != null)
            {
                this.VerbWidgets.Clear();
                foreach (VerbProperties v in this.ThingDef.Verbs)
                {
                    this.VerbWidgets.Add(new VerbWidget(v));
                }
            }
            if (this.ThingDef.tools != null)
            {
                this.ToolWidgets.Clear();
                foreach (Tool t in this.ThingDef.tools)
                {
                    this.ToolWidgets.Add(new ToolWidget(t));
                }
            }
            if (this.ThingDef.equippedStatOffsets != null)
            {
                this.EquipmentModifiers.Clear();
                foreach (StatModifier s in this.ThingDef.equippedStatOffsets)
                {
                    this.EquipmentModifiers.Add(new DefStatValueWidget(s));
                }
            }
            this.ResetBuffers();
        }

        public void ResetBuffers()
        {
            this.StatModifiers.ForEach((DefStatValueWidget w) => w.ResetBuffers());
            this.VerbWidgets.ForEach((VerbWidget w) => w.ResetBuffers());
            this.ToolWidgets.ForEach((ToolWidget w) => w.ResetBuffers());
            this.EquipmentModifiers.ForEach((DefStatValueWidget w) => w.ResetBuffers());

            if (this.projectileWidget != null)
                this.projectileWidget.ResetBuffers();
        }
    }
}
