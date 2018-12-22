using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System.Collections.Generic;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets
{
	class ThoughtDefWidget : AParentDefStatWidget<ThoughtDef>
	{
		private readonly List<IInputWidget> inputWidgets;

		private PlusMinusArgs<TraitDef> requiredTraits;
		private PlusMinusArgs<TraitDef> nullifyingTraits;
		private PlusMinusArgs<TaleDef> nullifyingOwnTales;

		private List<ThoughtStageWidget> stages;

		const int TRAIT_DEGREE_MIN = -2147483648;

		public ThoughtDefWidget(ThoughtDef def, DefType type) : base(def, type)
		{
			if (def.requiredTraits == null)
				def.requiredTraits = new List<TraitDef>();
			if (def.nullifyingTraits == null)
				def.nullifyingTraits = new List<TraitDef>();
			if (def.nullifyingOwnTales == null)
				def.nullifyingOwnTales = new List<TaleDef>();
			if (def.stages == null)
				def.stages = new List<ThoughtStage>();
			this.stages = new List<ThoughtStageWidget>(def.stages.Count);

			this.inputWidgets = new List<IInputWidget>()
			{
				new IntInputWidget<ThoughtDef>(base.Def, "Stack Limit", d => d.stackLimit, (d, v) => d.stackLimit = v),
				new FloatInputWidget<ThoughtDef>(base.Def, "Stacked Effect Multiplier", d => d.stackedEffectMultiplier, (d, v) => d.stackedEffectMultiplier = v),
				new FloatInputWidget<ThoughtDef>(base.Def, "Duration (Days)", d => d.durationDays, (d, v) => d.durationDays = v),
				new BoolInputWidget<ThoughtDef>(base.Def, "Invert", d => d.invert, (d, v) => d.invert = v),
				new BoolInputWidget<ThoughtDef>(base.Def, "Valid While Despawned", d => d.validWhileDespawned, (d, v) => d.validWhileDespawned = v),
				new BoolInputWidget<ThoughtDef>(base.Def, "Require Trait Degree", d => d.requiredTraitsDegree != TRAIT_DEGREE_MIN, (d, v) =>
				{
					if (d.requiredTraitsDegree != TRAIT_DEGREE_MIN)
						d.requiredTraitsDegree = TRAIT_DEGREE_MIN;
					else
						d.requiredTraitsDegree = 0;
					this.inputWidgets[6].ResetBuffers();
				}),
				new IntInputWidget<ThoughtDef>(base.Def, "Degree", d => d.requiredTraitsDegree, (d, v) => d.requiredTraitsDegree = v, d => new AInputWidget<ThoughtDef, int>.ShouldDrawInputResult(d.requiredTraitsDegree != TRAIT_DEGREE_MIN)),
				new BoolInputWidget<ThoughtDef>(base.Def, "Colonists Only", d => d.nullifiedIfNotColonist, (d, v) => d.nullifiedIfNotColonist = v),
				new BoolInputWidget<ThoughtDef>(base.Def, "Show Bubble", d => d.showBubble, (d, v) => d.showBubble = v),
				new IntInputWidget<ThoughtDef>(base.Def, "Stack Limit For Other Pawns", d => d.stackLimitForSameOtherPawn, (d, v) => d.stackLimitForSameOtherPawn = v),
				new FloatInputWidget<ThoughtDef>(base.Def, "Lerp Opinion To Zero Pct", d => d.lerpOpinionToZeroAfterDurationPct, (d, v) => d.lerpOpinionToZeroAfterDurationPct = v),
				new FloatInputWidget<ThoughtDef>(base.Def, "Max Cumulated Opinion Offset", d => d.maxCumulatedOpinionOffset, (d, v) => d.maxCumulatedOpinionOffset = v),
				new DefInputWidget<ThoughtDef, ThoughtDef>(base.Def, "Next Thought", 150, d => d.nextThought, (d, v) => d.nextThought = v, true),
				new DefInputWidget<ThoughtDef, GameConditionDef>(base.Def, "Game Condition", 150, d => d.gameCondition, (d, v) => d.gameCondition = v, true),
				new DefInputWidget<ThoughtDef, TaleDef>(base.Def, "Tale Def", 150, d => d.taleDef, (d, v) => d.taleDef = v, true),
				new DefInputWidget<ThoughtDef, HediffDef>(base.Def, "Hediff", 150, d => d.hediff, (d, v) => d.hediff = v, true),
				new DefInputWidget<ThoughtDef, StatDef>(base.Def, "Effect Multiply Ing Stat", 200, d => d.effectMultiplyingStat, (d, v) => d.effectMultiplyingStat = v, true),
				new DefInputWidget<ThoughtDef, ThoughtDef>(base.Def, "Thought To Make", 200, d => d.thoughtToMake, (d, v) => d.thoughtToMake = v, true),
			};

			this.nullifyingOwnTales = new PlusMinusArgs<TaleDef>()
			{
				allItems = DefDatabase<TaleDef>.AllDefs,
				beingUsed = () => base.Def.nullifyingOwnTales,
				onAdd = v => base.Def.nullifyingOwnTales = Util.AddTo(base.Def.nullifyingOwnTales, v),
				onRemove = v => base.Def.nullifyingOwnTales = Util.RemoveFrom(base.Def.nullifyingOwnTales, v),
				getDisplayName = v => Util.GetDefLabel(v)
			};

			this.requiredTraits = new PlusMinusArgs<TraitDef>()
			{
				allItems = DefDatabase<TraitDef>.AllDefs,
				beingUsed = () => base.Def?.requiredTraits,
				onAdd = v =>
				{
					base.Def.requiredTraits = Util.AddTo(base.Def.requiredTraits, v);
					base.Def.nullifyingTraits = Util.RemoveFrom(base.Def.nullifyingTraits, v);
				},
				onRemove = v => base.Def.requiredTraits = Util.RemoveFrom(base.Def.requiredTraits, v),
				getDisplayName = v => Util.GetDefLabel(v)
			};

			this.nullifyingTraits = new PlusMinusArgs<TraitDef>()
			{
				allItems = DefDatabase<TraitDef>.AllDefs,
				beingUsed = () => base.Def.nullifyingTraits,
				onAdd = v =>
				{
					base.Def.nullifyingTraits = Util.AddTo(base.Def.nullifyingTraits, v);
					base.Def.requiredTraits = Util.RemoveFrom(base.Def.requiredTraits, v);
				},
				onRemove = v => base.Def.nullifyingTraits = Util.RemoveFrom(base.Def.nullifyingTraits, v),
				getDisplayName = v => Util.GetDefLabel(v)
			};

			this.Rebuild();
		}

		public override void DrawLeft(float x, ref float y, float width)
		{
			foreach (var v in this.inputWidgets)
				v.Draw(x, ref y, width);
		}

		public override void DrawMiddle(float x, ref float y, float width)
		{
			WindowUtil.PlusMinusLabel(x, ref y, width, "Nullifying Own Tales", this.nullifyingOwnTales);
			foreach (var v in this.Def.nullifyingOwnTales)
			{
				WindowUtil.DrawLabel(x + 20, y, width, "- " + Util.GetDefLabel(v));
				y += 30;
			}
			WindowUtil.PlusMinusLabel(x, ref y, width, "Required Traits", this.requiredTraits);
			foreach (var v in this.Def.requiredTraits)
			{
				WindowUtil.DrawLabel(x + 20, y, width, "- " + Util.GetDefLabel(v));
				y += 30;
			}
			WindowUtil.PlusMinusLabel(x, ref y, width, "Nullifying Traits", this.nullifyingTraits);
			foreach (var v in this.Def.nullifyingTraits)
			{
				WindowUtil.DrawLabel(x + 20, y, width, "- " + Util.GetDefLabel(v));
				y += 30;
			}
		}

		public override void DrawRight(float x, ref float y, float width)
		{
			/* TODO Add the ability to edit stages
			WindowUtil.PlusMinusLabel(x, ref y, 100, "Stages",
				delegate
				{
					Find.WindowStack.Add(new Dialog_Name(
						"Thought Stage Label",
						delegate (string name)
						{
							ThoughtStage s = new ThoughtStage() { label = name };
							base.Def.stages.Add(s);
							this.stages.Add(new ThoughtStageWidget(s));
						},
						delegate (string name)
						{
							foreach (var v in base.Def.stages)
								if (object.Equals(v.label, name))
									return "label must be unique";
							return true;
						}));
				},
				delegate
				{
					WindowUtil.DrawFloatingOptions(
						new WindowUtil.FloatOptionsArgs<ThoughtStage>()
						{
							items = base.Def.stages,
							getDisplayName = s => s.label,
							onSelect = s =>
							{
								base.Def.stages.RemoveAll(v => object.Equals(s.label, v.label));
								this.stages.RemoveAll(v => object.Equals(s.label, v.DisplayLabel));
							}
						});
				});

			x += 10;*/
			foreach (var v in this.stages)
			{
				if (v != null)
				{
					v.Draw(x, ref y, width);
				}
			}
		}

		public override void Rebuild()
		{
			this.stages.Clear();
			base.Def.stages.ForEach(v =>
			{
				if (v != null)
					this.stages.Add(new ThoughtStageWidget(v));
			});
			this.ResetBuffers();
		}

		public override void ResetBuffers()
		{
			this.inputWidgets.ForEach(v => v.ResetBuffers());
			this.stages.ForEach(v => v.ResetBuffers());
		}
	}
}
