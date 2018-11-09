namespace InGameDefEditor.Gui.EditorWidgets
{
    public enum WidgetType
    {
        Apparel,
        Weapon,
        Projectile
    };

    interface IParentStatWidget
    {
        string Label { get; }
        WidgetType Type { get; }

        void DrawLeft(float x, ref float y, float width);
        void DrawMiddle(float x, ref float y, float width);
        void DrawRight(float x, ref float y, float width);
        void ResetBuffers();
        void Rebuild();
    }

    interface IStatWidget
    {
        void Draw(float x, ref float y, float width);
        void ResetBuffers();
    }
}
