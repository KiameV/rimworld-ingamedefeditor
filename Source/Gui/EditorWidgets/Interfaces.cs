using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    public enum WidgetType
    {
        Apparel,
        Biome,
        Weapon,
        Projectile
    };

    interface IStatWidget
    {
        string DisplayLabel { get; }
        void ResetBuffers();
    }

    interface IParentStatWidget : IStatWidget
    {
        WidgetType Type { get; }
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
        private readonly WidgetType type;

        protected AParentStatWidget(D def, WidgetType type)
        {
            this.Def = def;
            this.type = type;
        }

        public virtual string DisplayLabel => this.Def.label;
        public WidgetType Type => this.type;

        public Def BaseDef => this.Def;

        public abstract void DrawLeft(float x, ref float y, float width);
        public abstract void DrawMiddle(float x, ref float y, float width);
        public abstract void DrawRight(float x, ref float y, float width);
        public abstract void Rebuild();
        public abstract void ResetBuffers();
    }
}
