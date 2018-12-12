using InGameDefEditor.Gui.EditorWidgets.Misc;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System.Collections.Generic;
using Verse;
using InGameDefEditor.Gui.EditorWidgets.Dialog;

namespace InGameDefEditor.Gui.EditorWidgets
{
	class StoryTellerDefWidget : AParentDefStatWidget<StorytellerDef>
	{
		private readonly List<IInputWidget> inputWidgets;
		
		private readonly List<SimpleCurveWidget> simpleCurveWidgets = new List<SimpleCurveWidget>();

		private List<StorytellerCompPropertiesWidget> comps;

		public StoryTellerDefWidget(StorytellerDef def, DefType type) : base(def, type)
		{
			if (base.Def.comps == null)
				base.Def.comps = new List<StorytellerCompProperties>();

			this.inputWidgets = new List<IInputWidget>()
			{
				new IntInputWidget<StorytellerDef>(base.Def, "List Order", d => d.listOrder, (d, v) => d.listOrder = v),
				new BoolInputWidget<StorytellerDef>(base.Def, "List Visible", d => d.listVisible, (d, v) => d.listVisible = v),
				new BoolInputWidget<StorytellerDef>(base.Def, "Tutorial Mode", d => d.tutorialMode, (d, v) => d.tutorialMode = v),
				new BoolInputWidget<StorytellerDef>(base.Def, "Disable Adaptive Training", d => d.disableAdaptiveTraining, (d, v) => d.disableAdaptiveTraining = v),
				new BoolInputWidget<StorytellerDef>(base.Def, "Disable Alerts", d => d.disableAlerts, (d, v) => d.disableAlerts = v),
				new BoolInputWidget<StorytellerDef>(base.Def, "Disable Permadeath", d => d.disablePermadeath, (d, v) => d.disablePermadeath = v),
				new FloatInputWidget<StorytellerDef>(base.Def, "Adapt Days Min", d => d.adaptDaysMin, (d, v) => d.adaptDaysMin = v),
				new FloatInputWidget<StorytellerDef>(base.Def, "Adapt Days Max", d => d.adaptDaysMax, (d, v) => d.adaptDaysMax = v),
				new FloatInputWidget<StorytellerDef>(base.Def, "Game Start Grace Days", d => d.adaptDaysGameStartGraceDays, (d, v) => d.adaptDaysGameStartGraceDays = v),
				new DefInputWidget<StorytellerDef, DifficultyDef>(base.Def, "Forced Difficulty", 150, d => d.forcedDifficulty, (d, v) => d.forcedDifficulty = v, false)
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
			WindowUtil.PlusMinusLabel(x, ref y, width, "Components",
				delegate
				{
					Find.WindowStack.Add(new Dialog_StoryTellerComponentDialog(v =>
					{
						if (v == null)
							return "Component must be set";
						AcceptanceReport r = this.IsUnique(v);
						if (!r.Accepted)
							return r;

						this.comps.Add(new StorytellerCompPropertiesWidget(v));
						base.Def.comps.Add(v);

						return true;
					}));
				},
				delegate
				{
					WindowUtil.DrawFloatingOptions(
						new WindowUtil.FloatOptionsArgs<StorytellerCompPropertiesWidget>()
						{
							items = this.comps,
							getDisplayName = c => c.DisplayLabel,
							onSelect = c =>
							{
								for (int i = 0; i < this.comps.Count; ++i)
									if (this.comps[i] == c)
									{
										this.comps.RemoveAt(i);
										base.Def.comps.RemoveAt(i);
									}
							}
						});
				});

			foreach (var v in this.comps)
				v.Draw(x + 10, ref y, width);
		}

		public override void DrawRight(float x, ref float y, float width)
		{
			foreach (var v in this.simpleCurveWidgets)
				v.Draw(x, ref y, width);
		}

		public override void Rebuild()
		{
			this.comps?.Clear();
			Util.Populate(out this.comps, base.Def.comps, v => new StorytellerCompPropertiesWidget(v));
			this.comps?.ForEach(v => v.Rebuild());

			this.simpleCurveWidgets?.Clear();
			this.simpleCurveWidgets.Add(new SimpleCurveWidget("Population Intent Factor From Pop Curve", base.Def.populationIntentFactorFromPopCurve));
			this.simpleCurveWidgets.Add(new SimpleCurveWidget("Population Intent Factor From Pop Adapt Days Curve", base.Def.populationIntentFactorFromPopAdaptDaysCurve));
			this.simpleCurveWidgets.Add(new SimpleCurveWidget("Points Factor From Days Passed", base.Def.pointsFactorFromDaysPassed));
			this.simpleCurveWidgets.Add(new SimpleCurveWidget("Points Factor From Adapt Days", base.Def.pointsFactorFromAdaptDays));
			this.simpleCurveWidgets.Add(new SimpleCurveWidget("Adapt Days Loss From Colonist Lost By Post Population", base.Def.adaptDaysLossFromColonistLostByPostPopulation));
			this.simpleCurveWidgets.Add(new SimpleCurveWidget("Adapt Days Loss From Colonist Violently Downed By Population", base.Def.adaptDaysLossFromColonistViolentlyDownedByPopulation));
			this.simpleCurveWidgets.Add(new SimpleCurveWidget("Adapt Days Growth Rate Curve", base.Def.adaptDaysGrowthRateCurve));

			this.ResetBuffers();
		}

		public override void ResetBuffers()
		{
			this.inputWidgets?.ForEach(v => v.ResetBuffers());
			this.comps?.ForEach(v => v.ResetBuffers());
		}
		
		private AcceptanceReport IsUnique(StorytellerCompProperties toCheck)
		{
			var checkHashCode = StorytellerCompPropertiesStats.GetHashCode(toCheck);
			foreach (var c in base.Def.comps)
			{
				if (checkHashCode == StorytellerCompPropertiesStats.GetHashCode(c))
					return "Component already exists [" + StorytellerCompPropertiesStats.GetLabel(toCheck) + "]";
			}
			return true;
		}
	}
}