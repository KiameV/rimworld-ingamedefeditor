using InGameDefEditor.Gui.EditorWidgets.Misc;
using RimWorld;
using System.Collections.Generic;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class HediffDefWidget : AParentDefStatWidget<HediffDef>
	{
		private readonly List<IInputWidget> leftInputWidgets;
		private readonly List<IInputWidget> middleInputWidgets;
		private readonly List<IInputWidget> rightInputWidgets;

		public HediffDefWidget(HediffDef def, DefType type) : base(def, type)
		{
			this.leftInputWidgets = new List<IInputWidget>()
			{
				new BoolInputWidget<HediffDef>(base.Def, "Is Bad", d => d.isBad, (d, v) => d.isBad = v),
				new BoolInputWidget<HediffDef>(base.Def, "Makes Sick Thought", d => d.makesSickThought, (d, v) => d.makesSickThought = v),
				new BoolInputWidget<HediffDef>(base.Def, "Makes Alert", d => d.makesAlert, (d, v) => d.makesAlert = v),
				new BoolInputWidget<HediffDef>(base.Def, "Scenario Can Add", d => d.scenarioCanAdd, (d, v) => d.scenarioCanAdd = v),
				new BoolInputWidget<HediffDef>(base.Def, "Cure All AtOnce If Cured By Item", d => d.cureAllAtOnceIfCuredByItem, (d, v) => d.cureAllAtOnceIfCuredByItem = v),
				new BoolInputWidget<HediffDef>(base.Def, "Ever Curable By Item", d => d.everCurableByItem, (d, v) => d.everCurableByItem = v),
				new BoolInputWidget<HediffDef>(base.Def, "Price Impact", d => d.priceImpact, (d, v) => d.priceImpact = v),
				new BoolInputWidget<HediffDef>(base.Def, "Chronic", d => d.chronic, (d, v) => d.chronic = v),
				new BoolInputWidget<HediffDef>(base.Def, "Display Wound", d => d.displayWound, (d, v) => d.displayWound = v),
				new FloatInputWidget<HediffDef>(base.Def, "Chance To Cause No Pain", d => d.chanceToCauseNoPain, (d, v) => d.chanceToCauseNoPain = v),
				new FloatInputWidget<HediffDef>(base.Def, "Min Severity", d => d.minSeverity, (d, v) => d.minSeverity = v),
				new FloatInputWidget<HediffDef>(base.Def, "Max Severity", d => d.maxSeverity, (d, v) => d.maxSeverity = v),
				new ColorWidget<HediffDef>(base.Def, "Default Label Color", d => d.defaultLabelColor, (d, v) => d.defaultLabelColor = v),
			};

			this.middleInputWidgets = new List<IInputWidget>()
			{
				new DefInputWidget<HediffDef, ThingDef>(base.Def, "Spawn Thing On Removed", 150, d => d.spawnThingOnRemoved, (d, v) => d.spawnThingOnRemoved = v, true),
				new DefInputWidget<HediffDef, NeedDef>(base.Def, "Causes Need", 150, d => d.causesNeed, (d, v) => d.causesNeed = v, true),
				new DefPlusMinusInputWidget<NeedDef>("Disables Needs", 150, () => def.disablesNeeds),
				new DefInputWidget<HediffDef, TaleDef>(base.Def, "Tale On Visible", 150, d => d.taleOnVisible, (d, v) => d.taleOnVisible = v, true),
				//new SimpleCurveToggleableWidget<HediffDef>(base.Def, "Remove On Redress Chance By Days Curve", d => d.removeOnRedressChanceByDaysCurve, (d, v) => d.removeOnRedressChanceByDaysCurve = v),
			};

			this.rightInputWidgets = new List<IInputWidget>(2);
			if (base.Def.injuryProps != null)
				this.rightInputWidgets.Add(new InjuryPropsWidget(base.Def.injuryProps));
			if (base.Def.addedPartProps != null)
				this.rightInputWidgets.Add(new AddedBodyPartPropsWidget(base.Def.addedPartProps));

			this.Rebuild();
		}

		public override void DrawLeft(float x, ref float y, float width)
		{
			foreach (var v in this.leftInputWidgets)
				v.Draw(x, ref y, width);
		}

		public override void DrawMiddle(float x, ref float y, float width)
		{
			foreach (var v in this.middleInputWidgets)
				v.Draw(x, ref y, width);
		}

		public override void DrawRight(float x, ref float y, float width)
		{
			foreach (var v in this.rightInputWidgets)
				v.Draw(x, ref y, width);
		}

		public override void Rebuild()
		{
			this.ResetBuffers();
		}

		public override void ResetBuffers()
		{
			this.leftInputWidgets.ForEach(v => v.ResetBuffers());
			this.middleInputWidgets.ForEach(v => v.ResetBuffers());
			this.rightInputWidgets.ForEach(v => v.ResetBuffers());
		}

		protected override void AddDefsToAutoApply(bool isAutoApply)
		{

		}
	}
}
