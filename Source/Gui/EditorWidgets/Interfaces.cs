namespace InGameDefEditor.Gui.EditorWidgets
{
    public enum WidgetType
    {
        Apparel,
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

        void DrawLeft(float x, ref float y, float width);
        void DrawMiddle(float x, ref float y, float width);
        void DrawRight(float x, ref float y, float width);
        void Rebuild();
    }

    interface IDefEditorWidget : IStatWidget
    {
        void Draw(float x, ref float y, float width);
    }
}
