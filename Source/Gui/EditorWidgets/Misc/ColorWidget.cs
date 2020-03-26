using UnityEngine;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
    class ColorWidget<P> : AInputWidget<P, Color>
    {
        private readonly FloatInputWidget<P> r, g, b;

        public ColorWidget(P parent, string label, GetValue getValue, SetValue setValue) : base(parent, label, getValue, setValue)
        {
            this.r = new FloatInputWidget<P>(parent,
                "Red",
                p => getValue(p).r,
                (p, v) =>
                {
                    if (v > 0 && v < 1)
                    {
                        Color c = getValue(p);
                        c.r = v;
                        setValue(p, c);
                    }
                });
            this.g = new FloatInputWidget<P>(parent,
                "Green",
                p => getValue(p).g,
                (p, v) =>
                {
                    if (v > 0 && v < 1)
                    {
                        Color c = getValue(p);
                        c.g = v;
                        setValue(p, c);
                    }
                });
            this.b = new FloatInputWidget<P>(parent,
                "Blue",
                p => getValue(p).b,
                (p, v) =>
                {
                    if (v > 0 && v < 1)
                    {
                        Color c = getValue(p);
                        c.b = v;
                        setValue(p, c);
                    }
                });
        }

        protected override void DrawInput(float x, ref float y, float width)
        {
            WindowUtil.DrawLabel(x, ref y, width, base.label);
            x += 20;
            this.r.Draw(x, ref y, width);
            this.g.Draw(x, ref y, width);
            this.b.Draw(x, ref y, width);
        }

        public override void ResetBuffers()
        {
            this.r.ResetBuffers();
            this.g.ResetBuffers();
            this.b.ResetBuffers();
        }
    }
}
