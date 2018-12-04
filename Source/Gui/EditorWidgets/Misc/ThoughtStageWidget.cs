using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	class ThoughtStageWidget : IInputWidget
	{
		private readonly ThoughtStage stage;

		private readonly List<IInputWidget> inputWidgets;

		public string DisplayLabel => stage.label;
		
		public ThoughtStageWidget(ThoughtStage stage)
		{
			this.stage = stage;

			this.inputWidgets = new List<IInputWidget>()
			{
				new FloatInputWidget<ThoughtStage>(this.stage, "Base Mood Effect", s => s.baseMoodEffect, (s, v) => s.baseMoodEffect = v),
				new FloatInputWidget<ThoughtStage>(this.stage, "Base Opinion Offset", s => s.baseOpinionOffset, (s, v) => s.baseOpinionOffset = v),
				new BoolInputWidget<ThoughtStage>(this.stage, "Visible", s => s.visible, (s, v) => s.visible = v)
			};
		}

		public void Draw(float x, ref float y, float width)
		{
			Widgets.Label(new Rect(x, y, width, 42), this.stage.description);
			y += 45;
			foreach (var v in this.inputWidgets)
				v.Draw(x, ref y, width);
		}

		public void ResetBuffers()
		{
			this.inputWidgets.ForEach(v => v.ResetBuffers());
		}
	}
}
