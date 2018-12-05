using InGameDefEditor.Gui.Dialog;
using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class ThingDefWidget : AParentStatWidget<ThingDef>
    {
        private List<FloatInputWidget<StatModifier>> StatModifiers = new List<FloatInputWidget<StatModifier>>();
        private List<VerbWidget> VerbWidgets = new List<VerbWidget>();
        private List<ToolWidget> ToolWidgets = new List<ToolWidget>();
        private List<FloatInputWidget<StatModifier>> EquipmentModifiers = new List<FloatInputWidget<StatModifier>>();
		
		private BoolInputWidget<ThingDef> disabledInput;
		private bool isDisabled;

        public ThingDefWidget(ThingDef d, DefType type) : base(d, type)
        {
            if (base.Def.statBases == null)
                base.Def.statBases = new List<StatModifier>();

            if (base.Def.equippedStatOffsets == null)
                base.Def.equippedStatOffsets = new List<StatModifier>();

			this.isDisabled = Defs.DisabledThingDefs.ContainsKey(d.defName);

			this.disabledInput = new BoolInputWidget<ThingDef>(
				d, "Disable Def",
				def =>
				{
					this.isDisabled = Defs.DisabledThingDefs.ContainsKey(def.defName);
					return this.isDisabled;
				},
				(def, isDisabled) =>
				{
					this.isDisabled = isDisabled;
					if (isDisabled)
					{
						Defs.DisabledThingDefs[def.defName] = def;
						typeof(DefDatabase<ThingDef>).GetMethod("Remove", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { def });
					}
					else
					{
						Defs.DisabledThingDefs.Remove(def.defName);
						DefDatabase<ThingDef>.Add(def);
					}
				});

            this.Rebuild();
        }

        public override void DrawLeft(float x, ref float y, float width)
        {
			this.disabledInput.Draw(x, ref y, width);
			if (this.isDisabled)
				return;

			y += 10;
			if (base.Type == DefType.Apparel ||
				base.Type == DefType.Weapon)
			{
				this.DrawStatModifiers(x, ref y, width);
			}
        }

        public override void DrawMiddle(float x, ref float y, float width)
		{
			if (this.isDisabled)
				return;

			if (base.Type == DefType.Apparel)
            {
                this.DrawEquipmentStatOffsets(x, ref y, width);
            }
            else if (base.Type == DefType.Weapon)
            {
                this.DrawVerbs(x, ref y, width);
                y += 20;
                this.DrawTools(x, ref y, width);
            }
        }

        public override void DrawRight(float x, ref float y, float width)
		{
			if (this.isDisabled)
				return;

			if (base.Type == DefType.Weapon)
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
                x, ref y, width, "Base Modifiers",
                new WindowUtil.FloatOptionsArgs<StatDef>()
                {
                    items = this.GetPossibleStatModifiers(base.Def.statBases),
                    getDisplayName = delegate (StatDef d) { return d.label; },
                    onSelect = delegate (StatDef d)
                    {
                        // Add
                        StatModifier m = new StatModifier()
                        {
                            stat = d,
                            value = 0
                        };
                        base.Def.statBases.Add(m);
                        this.StatModifiers.Add(CreateFloatInput(m));
                    }
                },
                new WindowUtil.FloatOptionsArgs<StatModifier>()
                {
                    items = base.Def.statBases,
                    getDisplayName = delegate (StatModifier s) { return s.stat.defName; },
                    onSelect = delegate (StatModifier s)
                    {
                        // Remove
                        for (int i = 0; i < this.StatModifiers.Count; ++i)
                            if (this.StatModifiers[i].Parent.stat == s.stat)
                            {
                                this.StatModifiers.RemoveAt(i);
                                break;
                            }

                        for (int i = 0; i < base.Def.statBases.Count; ++i)
                            if (base.Def.statBases[i].stat == s.stat)
                            {
                                base.Def.statBases.RemoveAt(i);
                                break;
                            }
                    }
                });

            x += 10;
            foreach (var w in this.StatModifiers)
                w.Draw(x, ref y, width);
        }

        private void DrawTools(float x, ref float y, float width)
        {
            WindowUtil.PlusMinusLabel(x, ref y, width, "Tools",
                delegate
                {
                    Find.WindowStack.Add(new Dialog_Name(
                        "Name the new tool",
                        delegate (string name)
                        {
                            Tool t = new Tool() { label = name };
                            base.Def.tools.Add(t);
                            this.ToolWidgets.Add(new ToolWidget(t));
                        },
                        delegate (string name)
                        {
                            foreach (Tool t in base.Def.tools)
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
                        new WindowUtil.FloatOptionsArgs<Tool>()
                        {
                            items = base.Def.tools,
                            getDisplayName = delegate (Tool t) { return t.label; },
                            onSelect = delegate (Tool t)
                            {
                                for (int i = 0; i < base.Def.tools.Count; ++i)
                                {
                                    if (base.Def.tools[i].label.Equals(t.label))
                                    {
                                        base.Def.tools.RemoveAt(i);
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
            WindowUtil.PlusMinusLabel(x, ref y, width, "Equipped Stat Offsets",
                new WindowUtil.FloatOptionsArgs<StatDef>()
                {
                    items = this.GetPossibleStatModifiers(base.Def.equippedStatOffsets),
                    getDisplayName = delegate (StatDef d) { return d.label; },
                    onSelect = delegate (StatDef d)
                    {
                        StatModifier m = new StatModifier()
                        {
                            stat = d,
                            value = 0
                        };
                        base.Def.equippedStatOffsets.Add(m);
                        this.EquipmentModifiers.Add(this.CreateFloatInput(m));
                    }
                },
                new WindowUtil.FloatOptionsArgs<StatModifier>()
                {
                    items = base.Def.equippedStatOffsets,
                    getDisplayName = delegate (StatModifier s) { return s.stat.defName; },
                    onSelect = delegate (StatModifier s)
                    {
                        for (int i = 0; i < this.EquipmentModifiers.Count; ++i)
                        {
                            if (this.EquipmentModifiers[i].Parent.stat == s.stat)
                            {
                                this.EquipmentModifiers.RemoveAt(i);
                                break;
                            }
                        }
                        for (int i = 0; i < base.Def.equippedStatOffsets.Count; ++i)
                        {
                            if (base.Def.equippedStatOffsets[i].stat == s.stat)
                            {
                                base.Def.equippedStatOffsets.RemoveAt(i);
                                break;
                            }
                        }
                    }
                });

            foreach (var w in this.EquipmentModifiers)
                w.Draw(x, ref y, width);
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

        public override void Rebuild()
        {
            if (base.Def.statBases != null)
            {
                this.StatModifiers.Clear();
                foreach (StatModifier s in base.Def.statBases)
                {
                    this.StatModifiers.Add(this.CreateFloatInput(s));
                }
            }
            if (base.Def.Verbs != null)
            {
                this.VerbWidgets.Clear();
                foreach (VerbProperties v in base.Def.Verbs)
                {
                    this.VerbWidgets.Add(new VerbWidget(v));
                }
            }
            if (base.Def.tools != null)
            {
                this.ToolWidgets.Clear();
                foreach (Tool t in base.Def.tools)
                {
                    this.ToolWidgets.Add(new ToolWidget(t));
                }
            }
            if (base.Def.equippedStatOffsets != null)
            {
                this.EquipmentModifiers.Clear();
                foreach (StatModifier s in base.Def.equippedStatOffsets)
                {
                    this.EquipmentModifiers.Add(this.CreateFloatInput(s));
                }
            }
            this.ResetBuffers();
        }

        public override void ResetBuffers()
        {
			this.disabledInput.ResetBuffers();
			this.StatModifiers.ForEach((FloatInputWidget<StatModifier> w) => w.ResetBuffers());
            this.VerbWidgets.ForEach((VerbWidget w) => w.ResetBuffers());
            this.ToolWidgets.ForEach((ToolWidget w) => w.ResetBuffers());
            this.EquipmentModifiers.ForEach((FloatInputWidget<StatModifier> w) => w.ResetBuffers());
        }

        private FloatInputWidget<StatModifier> CreateFloatInput(StatModifier sm)
        {
            return new FloatInputWidget<StatModifier>(sm, sm.stat.label, (StatModifier m) => m.value, (StatModifier m, float f) => m.value = f);
        }
    }
}
