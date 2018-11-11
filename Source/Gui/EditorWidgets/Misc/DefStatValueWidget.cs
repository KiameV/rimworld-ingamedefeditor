using RimWorld;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
    class DefStatValueWidget : IDefEditorWidget
    {
        public readonly StatModifier StatModifier;

        private string buffer = "";

        public DefStatValueWidget(StatModifier s)
        {
            this.StatModifier = s;
            this.ResetBuffers();
        }

        public string DisplayLabel => StatModifier.stat.label;

        public void Draw(float x, ref float y, float width)
        {
            this.buffer = WindowUtil.DrawInput(x, ref y, this.StatModifier.stat.label, ref this.StatModifier.value, this.buffer);
        }

        public void ResetBuffers()
        {
            this.buffer = this.StatModifier.value.ToString();
        }
    }
}
