using RimWorld;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
    class StuffPropertiesWidget
    {
		private StuffProperties stuffProperties;

		private readonly List<IInputWidget> inputWidgets;

		private List<FloatInputWidget<StatModifier>> statOffsets;
		private WindowUtil.PlusMinusArgs<StatDef> statOffsetsArgs;

		private List<FloatInputWidget<StatModifier>> statFactors;
		private WindowUtil.PlusMinusArgs<StatDef> statFactorsArgs;

		public StuffPropertiesWidget(StuffProperties sp)
		{
			this.stuffProperties = sp;

			this.inputWidgets = new List<IInputWidget>()
			{
				new FloatInputWidget<StuffProperties>(this.stuffProperties, "Commonality", d => d.commonality, (d, v) => d.commonality = v),
				new ColorWidget<StuffProperties>(this.stuffProperties, "Color", d => d.color, (d, v) => d.color = v),
				new BoolInputWidget<StuffProperties>(this.stuffProperties, "Allow Color Generators", d => d.allowColorGenerators, (d, v) => d.allowColorGenerators = v),
				new DefInputWidget<StuffProperties, EffecterDef>(this.stuffProperties, "Construct Effect", 200, d => d.constructEffect, (d, v) => d.constructEffect = v, true),
				new DefInputWidget<StuffProperties, StuffAppearanceDef>(this.stuffProperties, "Appearance", 200, d => d.appearance, (d, v) => d.appearance = v, true),
				new DefInputWidget<StuffProperties, SoundDef>(this.stuffProperties, "Sound Impact", 200, d => d.soundImpactStuff, (d, v) => d.soundImpactStuff = v, true),
				new DefInputWidget<StuffProperties, SoundDef>(this.stuffProperties, "Sound Melee Hit Sharp", 200, d => d.soundMeleeHitSharp, (d, v) => d.soundMeleeHitSharp = v, true),
				new DefInputWidget<StuffProperties, SoundDef>(this.stuffProperties, "Sound Melee Hit Blunt", 200, d => d.soundMeleeHitBlunt, (d, v) => d.soundMeleeHitBlunt = v, true),
			};

			this.statOffsetsArgs = new WindowUtil.PlusMinusArgs<StatDef>()
			{
				getDisplayName = v => v.label,
				allItems = DefDatabase<StatDef>.AllDefs,
				beingUsed = () =>
				{
					List<StatDef> l = new List<StatDef>();
					if (this.stuffProperties.statOffsets != null)
						foreach (var v in this.stuffProperties.statOffsets)
							l.Add(v.stat);
					return l;
				},
				onAdd = def =>
				{
					StatModifier sm = new StatModifier() { stat = def, value = 0 };
					this.stuffProperties.statOffsets = Util.AddTo(this.stuffProperties.statOffsets, sm);
					this.statOffsets = Util.AddTo(this.statOffsets, this.CreateStatModifierInput(sm));
				},
				onRemove = def =>
				{
					this.statOffsets?.RemoveAll(v => v.Parent.stat == def);
					this.stuffProperties.statOffsets?.RemoveAll(v => v.stat == def);
				}
			};

			this.statFactorsArgs = new WindowUtil.PlusMinusArgs<StatDef>()
			{
				getDisplayName = v => v.label,
				allItems = DefDatabase<StatDef>.AllDefs,
				beingUsed = () =>
				{
					List<StatDef> l = new List<StatDef>();
					if (this.stuffProperties.statFactors != null)
						foreach (var v in this.stuffProperties.statFactors)
							l.Add(v.stat);
					return l;
				},
				onAdd = def =>
				{
					StatModifier sm = new StatModifier() { stat = def, value = 0 };
					this.stuffProperties.statFactors = Util.AddTo(this.stuffProperties.statFactors, sm);
					this.statFactors = Util.AddTo(this.statFactors, this.CreateStatModifierInput(sm));
				},
				onRemove = def =>
				{
					this.statFactors?.RemoveAll(v => v.Parent.stat == def);
					this.stuffProperties.statFactors?.RemoveAll(v => v.stat == def);
				}
			};

			this.ResetBuffers();
		}

		public string DisplayLabel => "Stuff Properties";

		public void Draw(float x, ref float y, float width)
		{
			WindowUtil.DrawLabel(x, y, width, this.DisplayLabel, true);
			y += 30;

			x += 20;
			foreach (var v in this.inputWidgets)
				v.Draw(x, ref y, width);

			WindowUtil.PlusMinusLabel(x, ref y, width, "Stat Offsets", this.statOffsetsArgs);
			foreach (var v in this.statOffsets)
				v.Draw(x + 20, ref y, width);

			WindowUtil.PlusMinusLabel(x, ref y, width, "Stat Factors", this.statFactorsArgs);
			foreach (var v in this.statFactors)
				v.Draw(x + 20, ref y, width);
		}

		public void ResetBuffers()
		{
			this.statOffsets?.Clear();
			Util.Populate(out this.statOffsets, this.stuffProperties.statOffsets, v => this.CreateStatModifierInput(v));

			this.statFactors?.Clear();
			Util.Populate(out this.statFactors, this.stuffProperties.statFactors, v => this.CreateStatModifierInput(v));
		}

		private FloatInputWidget<StatModifier> CreateStatModifierInput(StatModifier sm)
		{
			return new FloatInputWidget<StatModifier>(sm, Util.GetLabel(sm.stat), v => v.value, (s, v) => s.value = v);
		}
	}
}
