using System;
using System.Collections.Generic;
using System.Linq;
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
		public delegate bool DrawInput(P d);

		public readonly P Parent;
        protected readonly string label;
        protected readonly GetValue getValue;
        protected readonly SetValue setValue;
		protected readonly DrawInput drawInput;

		protected string buffer = "";

        public AInputWidget(P parent, string label, GetValue getValue, SetValue setValue, DrawInput drawInput = null)
        {
            this.Parent = parent;
            this.label = label;
            this.getValue = getValue;
            this.setValue = setValue;
			this.drawInput = drawInput;
		}

        public abstract void Draw(float x, ref float y, float width);
        public abstract void ResetBuffers();
    }

    class BoolInputWidget<P> : AInputWidget<P, bool>
    {
        private bool value;

        public BoolInputWidget(P parent, string label, GetValue getValue, SetValue setValue, DrawInput drawInput = null) : base(parent, label, getValue, setValue, drawInput)
        {
            this.value = base.getValue(base.Parent);
            this.ResetBuffers();
        }

        public override void Draw(float x, ref float y, float width)
        {
			if (drawInput == null || drawInput(base.Parent))
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

        public IntInputWidget(P parent, string label, GetValue getValue, SetValue setValue, DrawInput drawInput = null) : base(parent, label, getValue, setValue, drawInput)
		{
            this.value = base.getValue(base.Parent);
            this.ResetBuffers();
        }

        public override void Draw(float x, ref float y, float width)
		{
			if (drawInput == null || drawInput(base.Parent))
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

        public FloatInputWidget(P parent, string label, GetValue getValue, SetValue setValue, DrawInput drawInput = null) : base(parent, label, getValue, setValue, drawInput)
		{
            this.value = base.getValue(base.Parent);
            this.ResetBuffers();
        }

        public override void Draw(float x, ref float y, float width)
		{
			if (drawInput == null || drawInput(base.Parent))
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

	class EnumInputWidget<P, E> : AInputWidget<P, E>
	{
		private readonly float labelWidth;
		private WindowUtil.FloatOptionsArgs<E> args;

		public EnumInputWidget(P parent, string label, float labelWidth, GetValue getValue, SetValue setValue, DrawInput drawInput = null) : base(parent, label, getValue, setValue, drawInput)
		{
			this.labelWidth = labelWidth;
			args = new FloatOptionsArgs<E>()
			{
				getDisplayName = v => v.ToString(),
				items = Enum.GetValues(typeof(E)).Cast<E>(),
				onSelect = v => base.setValue(base.Parent, v)
			};
		}

		public override void Draw(float x, ref float y, float width)
		{
			if (drawInput == null || drawInput(base.Parent))
				WindowUtil.DrawInput(x, ref y, width, base.label, this.labelWidth, base.getValue(base.Parent).ToString(), this.args);
		}

		public override void ResetBuffers() { }
	}

	class DefInputWidget<P, D> : AInputWidget<P, D> where D : Def, new()
	{
		private readonly float labelWidth;
		private WindowUtil.FloatOptionsArgs<D> args;

		public DefInputWidget(P parent, string label, float labelWidth, GetValue getValue, SetValue setValue, bool includeNullOption, DrawInput drawInput = null) : base(parent, label, getValue, setValue, drawInput)
		{
			this.labelWidth = labelWidth;
			args = new FloatOptionsArgs<D>()
			{
				getDisplayName = def => Util.GetDefLabel(def),
				items = Util.SortedDefList<D>(),
				onSelect = def => base.setValue(base.Parent, def),
				includeNullOption = includeNullOption
			};
		}

		public override void Draw(float x, ref float y, float width)
		{
			if (drawInput == null || drawInput(base.Parent))
				WindowUtil.DrawInput(x, ref y, width, base.label, this.labelWidth, Util.GetDefLabel(base.getValue(base.Parent)), this.args);
		}

		public override void ResetBuffers() { }
	}

	class DefPlusMinusInputWidget<D> : IInputWidget where D : Def, new()
	{
		private readonly string label;
		private readonly float labelWidth;
		private readonly List<D> items;
		private WindowUtil.PlusMinusArgs<D> args;

		public DefPlusMinusInputWidget(string label, float labelWidth, List<D> items)
		{
			this.label = label;
			this.labelWidth = labelWidth;
			this.items = items;

			args = new PlusMinusArgs<D>()
			{
				getDisplayName = def => Util.GetDefLabel(def),
				allItems = DefDatabase<D>.AllDefsListForReading,
				onAdd = def => Util.AddTo(this.items, def),
				onRemove = def => Util.RemoveFrom(this.items, def),
				beingUsed = () => this.items
			};
		}

		public void Draw(float x, ref float y, float width)
		{
			WindowUtil.PlusMinusLabel(x, ref y, labelWidth, label, this.args);
			IEnumerable<D> beingUsed = this.args.beingUsed.Invoke();
			x += 10;
			if (beingUsed != null)
			{
				foreach (var def in beingUsed)
				{
					WindowUtil.DrawLabel(x, y, width, "- " + Util.GetDefLabel(def));
					y += 30;
				}
			}
		}

		public void ResetBuffers() { }
	}
}
