using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
	interface IInputWidget
	{
		string DisplayLabel { get; }
		void Draw(float x, ref float y, float width);
		void ResetBuffers();
	}

	interface IParentStatWidget
	{
		DefType Type { get; }
		Def BaseDef { get; }
		string DisplayLabel { get; }

		void DrawLeft(float x, ref float y, float width);
		void DrawMiddle(float x, ref float y, float width);
		void DrawRight(float x, ref float y, float width);
		void Rebuild();
		void ResetBuffers();
	}

	public abstract class AParentStatWidget<D> : IParentStatWidget where D : Def, new()
    {
        public readonly D Def;
        private readonly DefType type;

        protected AParentStatWidget(D def, DefType type)
        {
            this.Def = def;
            this.type = type;
        }

        public virtual string DisplayLabel => Util.GetDefLabel(this.Def);
        public DefType Type => this.type;

        public Def BaseDef => this.Def;

        public abstract void DrawLeft(float x, ref float y, float width);
        public abstract void DrawMiddle(float x, ref float y, float width);
        public abstract void DrawRight(float x, ref float y, float width);
        public abstract void Rebuild();
        public abstract void ResetBuffers();
    }
}
