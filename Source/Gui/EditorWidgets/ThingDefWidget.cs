using InGameDefEditor.Gui.Dialog;
using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class ThingDefWidget : ABuildableDefWidget<ThingDef>
	{
        private List<VerbWidget> VerbWidgets = new List<VerbWidget>();
        private List<ToolWidget> ToolWidgets = new List<ToolWidget>();
        private readonly List<FloatInputWidget<StatModifier>> EquipmentModifiers = new List<FloatInputWidget<StatModifier>>();
		private IngestiblePropertiesWidget ingestiblePropertiesWidget = null;

		public ThingDefWidget(ThingDef def, DefType type) : base(def, type)
        {
            if (base.Def.equippedStatOffsets == null)
                base.Def.equippedStatOffsets = new List<StatModifier>();

			this.Rebuild();
        }

        public override void DrawLeftInput(float x, ref float y, float width)
        {

        }

        public override void DrawMiddleInput(float x, ref float y, float width)
		{
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
			this.ingestiblePropertiesWidget?.Draw(x, ref y, width);
		}

        public override void DrawRightInput(float x, ref float y, float width)
		{
			if (base.Type == DefType.Weapon)
			{
				this.DrawEquipmentStatOffsets(x, ref y, width);
			}
		}

        public override void Rebuild()
        {
			base.Rebuild();

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
			this.ingestiblePropertiesWidget = null;
			if (base.Def.ingestible != null)
				this.ingestiblePropertiesWidget = new IngestiblePropertiesWidget(base.Def.ingestible);
			this.ResetBuffers();
		}

        public override void ResetBuffers()
        {
			base.ResetBuffers();

            this.VerbWidgets.ForEach(v => v.ResetBuffers());
            this.ToolWidgets.ForEach(v => v.ResetBuffers());
            this.EquipmentModifiers.ForEach(v => v.ResetBuffers());
			this.ingestiblePropertiesWidget?.ResetBuffers();
		}

		private void DrawVerbs(float x, ref float y, float width)
		{
			foreach (VerbWidget w in this.VerbWidgets)
			{
				w.Draw(x, ref y, width);
			}
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
					items = base.GetPossibleStatModifiers(base.Def.equippedStatOffsets),
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
	}
}
