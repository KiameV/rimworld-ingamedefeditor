using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    interface IStatWidget
    {
        string DisplayLabel { get; }
        void ResetBuffers();
    }

    interface IParentStatWidget : IStatWidget
    {
        DefType Type { get; }
        Def BaseDef { get; }

        void DrawLeft(float x, ref float y, float width);
        void DrawMiddle(float x, ref float y, float width);
        void DrawRight(float x, ref float y, float width);
        void Rebuild();
    }

    interface IDefEditorWidget : IStatWidget
    {
        void Draw(float x, ref float y, float width);
    }

    public abstract class AParentStatWidget<D> : IParentStatWidget where D : Def
    {
        public readonly D Def;
        private readonly DefType type;

        protected AParentStatWidget(D def, DefType type)
        {
            this.Def = def;
            this.type = type;
        }

        public virtual string DisplayLabel => this.Def.label;
        public DefType Type => this.type;

        public Def BaseDef => this.Def;

        public abstract void DrawLeft(float x, ref float y, float width);
        public abstract void DrawMiddle(float x, ref float y, float width);
        public abstract void DrawRight(float x, ref float y, float width);
        public abstract void Rebuild();
        public abstract void ResetBuffers();
    }
}
