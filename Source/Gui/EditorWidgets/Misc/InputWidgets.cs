using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
    interface IInputWidget
    {
        void Draw(float x, ref float y, float width);
        void ResetBuffers();
    }

    abstract class AInputWidget<P, V> : IInputWidget
    {
        public delegate V GetValue(P d);
        public delegate void SetValue(P d, V v);
        
        public readonly P Parent;
        protected readonly string label;
        protected readonly GetValue getValue;
        protected readonly SetValue setValue;

        protected string buffer = "";

        public AInputWidget(P parent, string label, GetValue getValue, SetValue setValue)
        {
            this.Parent = parent;
            this.label = label;
            this.getValue = getValue;
            this.setValue = setValue;
        }

        public abstract void Draw(float x, ref float y, float width);
        public abstract void ResetBuffers();
    }

    class BoolInputWidget<P> : AInputWidget<P, bool>
    {
        private bool value;

        public BoolInputWidget(P parent, string label, GetValue getValue, SetValue setValue) : base(parent, label, getValue, setValue)
        {
            this.value = base.getValue(base.Parent);
            this.ResetBuffers();
        }

        public override void Draw(float x, ref float y, float width)
        {
            this.value = DrawInput(x, ref y, base.label, this.value, delegate(bool b) { base.setValue(this.Parent, b); });
        }

        public override void ResetBuffers()
        {
            this.value = base.getValue(this.Parent);
        }
    }

    class IntInputWidget<P> : AInputWidget<P, int>
    {
        private int value;

        public IntInputWidget(P parent, string label, GetValue getValue, SetValue setValue) : base(parent, label, getValue, setValue)
        {
            this.value = base.getValue(base.Parent);
            this.ResetBuffers();
        }

        public override void Draw(float x, ref float y, float width)
        {
            this.buffer = WindowUtil.DrawInput(x, ref y, this.label, this.value, delegate (int i) { base.setValue(this.Parent, i); }, this.buffer);
        }

        public override void ResetBuffers()
        {
            this.buffer = base.getValue(base.Parent).ToString();
        }
    }

    class FloatInputWidget<P> : AInputWidget<P, float>
    {
        private float value;

        public FloatInputWidget(P parent, string label, GetValue getValue, SetValue setValue) : base(parent, label, getValue, setValue)
        {
            this.value = base.getValue(base.Parent);
            this.ResetBuffers();
        }

        public override void Draw(float x, ref float y, float width)
        {
            this.buffer = WindowUtil.DrawInput(x, ref y, this.label, this.value, delegate (float f) { base.setValue(this.Parent, f); }, this.buffer);
        }

        public override void ResetBuffers()
        {
            this.buffer = base.getValue(base.Parent).ToString();
        }
    }

    class MinMaxInputWidget<P> : IInputWidget
    {
        private readonly string label;
        private readonly FloatInputWidget<P> min;
        private readonly FloatInputWidget<P> max;

        public P Parent { get { return min.Parent; } }

        public MinMaxInputWidget(string label, FloatInputWidget<P> min, FloatInputWidget<P> max)
        {
            this.label = label;
            this.min = min;
            this.max = max;
            this.ResetBuffers();
        }

        public void Draw(float x, ref float y, float width)
        {
            DrawLabel(x, y, width, label, true);
            x += 10;
            y += 40;
            this.min.Draw(x, ref y, width);
            this.max.Draw(x, ref y, width);
        }

        public void ResetBuffers()
        {
            this.min.ResetBuffers();
            this.max.ResetBuffers();
        }
    }
}
