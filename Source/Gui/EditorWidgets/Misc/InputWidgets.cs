using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
    abstract class AInputWidget<P, V> : IInputWidget
    {
        public delegate V GetValue(P d);
        public delegate void SetValue(P d, V v);
		public struct ShouldDrawInputResult
		{
			public bool drawInput;
			public string message;
			public ShouldDrawInputResult(bool drawInput, string message = null)
			{
				this.drawInput = drawInput;
				this.message = message;
			}
		}
		public delegate ShouldDrawInputResult ShouldDrawInput(P d);

		public readonly P Parent;
        protected readonly string label;
        protected readonly GetValue getValue;
        protected readonly SetValue setValue;
		protected readonly ShouldDrawInput shouldDrawInput;

		protected string buffer = "";

		public string DisplayLabel => "";

		public AInputWidget(P parent, string label, GetValue getValue, SetValue setValue, ShouldDrawInput shouldDrawInput = null)
        {
            this.Parent = parent;
            this.label = label;
            this.getValue = getValue;
            this.setValue = setValue;
			this.shouldDrawInput = shouldDrawInput;
		}

		public void Draw(float x, ref float y, float width)
		{
			if (this.shouldDrawInput == null)
				this.DrawInput(x, ref y, width);
			else
			{
				ShouldDrawInputResult result = this.shouldDrawInput(this.Parent);
				if (result.drawInput)
					this.DrawInput(x, ref y, width);
				else if (result.message != null && result.message.Length > 0)
					DrawLabel(x, ref y, width, result.message);
			}
		}
		protected abstract void DrawInput(float x, ref float y, float width);
        public abstract void ResetBuffers();
    }

    class BoolInputWidget<P> : AInputWidget<P, bool>
    {
        private bool value;

        public BoolInputWidget(P parent, string label, GetValue getValue, SetValue setValue, ShouldDrawInput shouldDrawInput = null) : base(parent, label, getValue, setValue, shouldDrawInput)
        {
            this.value = base.getValue(base.Parent);
            this.ResetBuffers();
        }

		protected override void DrawInput(float x, ref float y, float width)
		{
			this.value = WindowUtil.DrawInput(x, ref y, width, base.label, this.value, delegate (bool b) { base.setValue(this.Parent, b); });
        }

        public override void ResetBuffers()
        {
            this.value = base.getValue(this.Parent);
        }
    }

    class IntInputWidget<P> : AInputWidget<P, int>
    {
        private int value;

        public IntInputWidget(P parent, string label, GetValue getValue, SetValue setValue, ShouldDrawInput shouldDrawInput = null) : base(parent, label, getValue, setValue, shouldDrawInput)
		{
            this.value = base.getValue(base.Parent);
            this.ResetBuffers();
        }

        protected override void DrawInput(float x, ref float y, float width)
		{
			this.buffer = WindowUtil.DrawInput(x, ref y, width, this.label, this.value, delegate (int i) { base.setValue(this.Parent, i); }, this.buffer);
        }

        public override void ResetBuffers()
        {
            this.buffer = base.getValue(base.Parent).ToString();
        }
    }

    class FloatInputWidget<P> : AInputWidget<P, float>
    {
        private float value;

        public FloatInputWidget(P parent, string label, GetValue getValue, SetValue setValue, ShouldDrawInput shouldDrawInput = null) : base(parent, label, getValue, setValue, shouldDrawInput)
		{
            this.value = base.getValue(base.Parent);
            this.ResetBuffers();
        }

        protected override void DrawInput(float x, ref float y, float width)
		{
			this.buffer = WindowUtil.DrawInput(x, ref y, width, this.label, this.value, delegate (float f) { base.setValue(this.Parent, f); }, this.buffer);
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

		public string DisplayLabel => label;

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

		public EnumInputWidget(P parent, string label, float labelWidth, GetValue getValue, SetValue setValue, ShouldDrawInput shouldDrawInput = null) : base(parent, label, getValue, setValue, shouldDrawInput)
		{
			this.labelWidth = labelWidth;
			args = new FloatOptionsArgs<E>()
			{
				getDisplayName = v => v.ToString(),
				items = Enum.GetValues(typeof(E)).Cast<E>(),
				onSelect = v => base.setValue(base.Parent, v)
			};
		}

		protected override void DrawInput(float x, ref float y, float width)
		{
			WindowUtil.DrawInput(x, ref y, width, base.label, this.labelWidth, base.getValue(base.Parent).ToString(), this.args);
		}

		public override void ResetBuffers() { }
	}

	class DefInputWidget<P, D> : AInputWidget<P, D> where D : Def, new()
	{
		private readonly float labelWidth;
		private WindowUtil.FloatOptionsArgs<D> args;

		public DefInputWidget(P parent, string label, float labelWidth, GetValue getValue, SetValue setValue, bool includeNullOption, ShouldDrawInput shouldDrawInput = null) : base(parent, label, getValue, setValue, shouldDrawInput)
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

		protected override void DrawInput(float x, ref float y, float width)
		{
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

		public string DisplayLabel => label;

		public void Draw(float x, ref float y, float width)
		{
			WindowUtil.PlusMinusLabel(x, ref y, width, label, this.args);
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
