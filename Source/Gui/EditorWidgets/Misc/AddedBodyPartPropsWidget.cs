using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
    internal class AddedBodyPartPropsWidget : IInputWidget
    {
        private readonly List<IInputWidget> inputWidgets;

        public string DisplayLabel => "Added Body Part Props";

        public AddedBodyPartPropsWidget(AddedBodyPartProps addedBodyPartProps)
        {
            this.inputWidgets = new List<IInputWidget>()
            {
                new FloatInputWidget<AddedBodyPartProps>(addedBodyPartProps, "Part Efficiency", p => p.partEfficiency, (p, v) => p.partEfficiency = v),
                new BoolInputWidget<AddedBodyPartProps>(addedBodyPartProps, "Solid", p => p.solid, (p, v) => p.solid = v),
                new BoolInputWidget<AddedBodyPartProps>(addedBodyPartProps, "Is Good Weapon", p => p.isGoodWeapon, (p, v) => p.isGoodWeapon = v),
                new BoolInputWidget<AddedBodyPartProps>(addedBodyPartProps, "Better Than Natural", p => p.betterThanNatural, (p, v) => p.betterThanNatural = v),
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