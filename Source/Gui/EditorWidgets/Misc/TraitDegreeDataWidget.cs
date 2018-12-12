using RimWorld;
using System.Collections.Generic;
using Verse;
using System;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	class TraitDegreeDataWidget : IInputWidget
	{
		private readonly TraitDegreeData data;
		private readonly List<IInputWidget> inputWidgets;

		private List<IntInputWidget<SkillDef>> skillGains;
		private WindowUtil.PlusMinusArgs<SkillDef> skillGainsArgs;

		private List<FloatInputWidget<StatModifier>> statOffsets;
		private WindowUtil.PlusMinusArgs<StatDef> statOffsetsArgs;

		private List<FloatInputWidget<StatModifier>> statFactors;
		private WindowUtil.PlusMinusArgs<StatDef> statFactorsArgs;

		// TODO Add 
		//public SimpleCurveStats randomMentalStateMtbDaysMoodCurve;

		public string DisplayLabel => this.data.label + "  " + this.data.degree;

		public TraitDegreeDataWidget(TraitDegreeData data)
		{
			if (data.disallowedMentalStates == null)
				data.disallowedMentalStates = new List<MentalStateDef>();
			if (data.disallowedInspirations == null)
				data.disallowedInspirations = new List<InspirationDef>();
			if (data.theOnlyAllowedMentalBreaks == null)
				data.theOnlyAllowedMentalBreaks = new List<MentalBreakDef>();

			this.data = data;
			this.inputWidgets = new List<IInputWidget>()
			{
				//new IntInputWidget<TraitDegreeData>(this.data, "Degree", d => d.degree, (d, v) => d.degree = v),
				new FloatInputWidget<TraitDegreeData>(this.data, "Commonality", d => d.commonality, (d, v) => d.commonality = v),
				new FloatInputWidget<TraitDegreeData>(this.data, "Social Fight Chance Factor", d => d.socialFightChanceFactor, (d, v) => d.socialFightChanceFactor = v),
				new FloatInputWidget<TraitDegreeData>(this.data, "Market Value Factor Offset", d => d.marketValueFactorOffset, (d, v) => d.marketValueFactorOffset = v),
				new FloatInputWidget<TraitDegreeData>(this.data, "Random Disease Mtb Days", d => d.randomDiseaseMtbDays, (d, v) => d.randomDiseaseMtbDays = v),
				new DefInputWidget<TraitDegreeData, ThinkTreeDef>(this.data, "Think Tree", 200, d => d.thinkTree, (d, v) => d.thinkTree = v, true),
				new DefInputWidget<TraitDegreeData, MentalStateDef>(this.data, "Random Mental State", 200, d => d.randomMentalState, (d, v) => d.randomMentalState = v, true),
				new DefPlusMinusInputWidget<InspirationDef>("Disallowed Inspirations", 200, this.data.disallowedInspirations),
				new DefPlusMinusInputWidget<MentalStateDef>("Disallowed Mental States", 200, this.data.disallowedMentalStates),
				new DefPlusMinusInputWidget<MentalBreakDef>("The Only Allowed Mental Breaks", 200, this.data.theOnlyAllowedMentalBreaks),
			};
			
			this.skillGainsArgs = new WindowUtil.PlusMinusArgs<SkillDef>()
			{
				getDisplayName = v => v.label,
				allItems = DefDatabase<SkillDef>.AllDefs,
				beingUsed = () =>
				{
					List<SkillDef> l = new List<SkillDef>();
					if (this.data.skillGains != null)
						foreach (var kv in this.data.skillGains)
							l.Add(kv.Key);
					return l;
				},
				onAdd = def =>
				{
					this.data.skillGains.Add(def, 0);
					this.skillGains.Add(this.CreateSkillGainInput(def));
				},
				onRemove = def =>
				{
					this.skillGains.RemoveAll(v => v.Parent == def);
					this.data.skillGains.Remove(def);
				}
			};

			this.statOffsetsArgs = new WindowUtil.PlusMinusArgs<StatDef>()
			{
				getDisplayName = v => v.label,
				allItems = DefDatabase<StatDef>.AllDefs,
				beingUsed = () =>
				{
					List<StatDef> l = new List<StatDef>();
					if (this.data.statOffsets != null)
						foreach (var v in this.data.statOffsets)
							l.Add(v.stat);
					return l;
				},
				onAdd = def =>
				{
					StatModifier sm = new StatModifier() { stat = def, value = 0 };
					this.data.statOffsets = Util.AddTo(this.data.statOffsets, sm);
					this.statOffsets = Util.AddTo(this.statOffsets, this.CreateStatModifierInput(sm));
				},
				onRemove = def =>
				{
					this.statOffsets?.RemoveAll(v => v.Parent.stat == def);
					this.data.statOffsets?.RemoveAll(v => v.stat == def);
				}
			};

			this.statFactorsArgs = new WindowUtil.PlusMinusArgs<StatDef>()
			{
				getDisplayName = v => v.label,
				allItems = DefDatabase<StatDef>.AllDefs,
				beingUsed = () =>
				{
					List<StatDef> l = new List<StatDef>();
					if (this.data.statFactors != null)
						foreach (var v in this.data.statFactors)
							l.Add(v.stat);
					return l;
				},
				onAdd = def =>
				{
					StatModifier sm = new StatModifier() { stat = def, value = 0 };
					this.data.statFactors = Util.AddTo(this.data.statFactors, sm);
					this.statFactors = Util.AddTo(this.statFactors, this.CreateStatModifierInput(sm));
				},
				onRemove = def =>
				{
					this.statFactors?.RemoveAll(v => v.Parent.stat == def);
					this.data.statFactors?.RemoveAll(v => v.stat == def);
				}
			};

			this.ResetBuffers();
		}

		public void Draw(float x, ref float y, float width)
		{
			WindowUtil.DrawLabel(x, y, width, this.DisplayLabel, true);
			y += 30;

			x += 20;
			foreach (var v in this.inputWidgets)
				v.Draw(x, ref y, width);
			
			WindowUtil.PlusMinusLabel(x, ref y, width, "Skill Gains", this.skillGainsArgs);
			foreach (var v in this.skillGains)
				v.Draw(x + 20, ref y, width);

			WindowUtil.PlusMinusLabel(x, ref y, width, "Stat Offsets", this.statOffsetsArgs);
			foreach (var v in this.statOffsets)
				v.Draw(x + 20, ref y, width);

			WindowUtil.PlusMinusLabel(x, ref y, width, "Stat Factors", this.statFactorsArgs);
			foreach (var v in this.statFactors)
				v.Draw(x + 20, ref y, width);
		}

		public void ResetBuffers()
		{
			this.skillGains?.Clear();
			Util.Populate(out this.skillGains, this.data.skillGains, v => this.CreateSkillGainInput(v.Key));

			this.statOffsets?.Clear();
			Util.Populate(out this.statOffsets, this.data.statOffsets, v => this.CreateStatModifierInput(v));

			this.statFactors?.Clear();
			Util.Populate(out this.statFactors, this.data.statFactors, v => this.CreateStatModifierInput(v));
		}

		private IntInputWidget<SkillDef> CreateSkillGainInput(SkillDef d)
		{
			return new IntInputWidget<SkillDef>(d, d.label, def => this.data.skillGains[def], (def, v) => this.data.skillGains[def] = v);
		}

		private FloatInputWidget<StatModifier> CreateStatModifierInput(StatModifier sm)
		{
			return new FloatInputWidget<StatModifier>(sm, Util.GetDefLabel(sm.stat), v => v.value, (s, v) => s.value = v);
		}
	}
}
