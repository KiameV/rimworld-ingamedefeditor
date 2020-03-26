using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
    internal class InjuryPropsWidget : IInputWidget
    {
        private readonly List<IInputWidget> inputWidgets;

        public string DisplayLabel => "Injury Props";

        public InjuryPropsWidget(InjuryProps injuryProps)
        {
            this.inputWidgets = new List<IInputWidget>()
            {
                new FloatInputWidget<InjuryProps>(injuryProps, "Pain Per Severity", p => p.painPerSeverity, (p, v) => p.painPerSeverity = v),
                new FloatInputWidget<InjuryProps>(injuryProps, "Average Pain Per Severity Permanent", p => p.averagePainPerSeverityPermanent, (p, v) => p.averagePainPerSeverityPermanent = v),
                new FloatInputWidget<InjuryProps>(injuryProps, "Bleed Rate", p => p.bleedRate, (p, v) => p.bleedRate = v),
                new BoolInputWidget<InjuryProps>(injuryProps, "Can Merge", p => p.canMerge, (p, v) => p.canMerge = v),
                new BoolInputWidget<InjuryProps>(injuryProps, "Use Removed Label", p => p.useRemovedLabel, (p, v) => p.useRemovedLabel = v),
            };
        }

        public void Draw(float x, ref float y, float width)
        {
            WindowUtil.DrawLabel(x, y, 300, this.DisplayLabel, true);
            y += 40;

            x += 10;
            foreach (var i in this.inputWidgets)
                i.Draw(x, ref y, width - 10);
        }

        public void ResetBuffers()
        {
            this.inputWidgets.ForEach(i => i.ResetBuffers());
        }
    }
}