using InGameDefEditor.Gui.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	public delegate V GetValue<P, V>(P d);
	public delegate void SetValue<P, V>(P d, V v);
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
		
		public string ToolTip = null;

		protected string buffer = "";

		public string DisplayLabel => label;

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
			{
				this.DrawInput(x, ref y, width);
				if (this.ToolTip != null)
				{
					if (Mouse.IsOver(new Rect(x, y - 32, width, 32)))
					{
						Vector2 vector = Text.CalcSize(ToolTip);
						if (vector.x > 260f)
						{
							vector.x = 260f;
							vector.y = Text.CalcHeight(ToolTip, vector.x);
						}
						Rect rect = new Rect(0, 0, vector.x, vector.y);
						rect = rect.ContractedBy(-4f);
						Rect rect2 = new Rect(UI.MousePositionOnUI.x, UI.MousePositionOnUIInverted.y + 20, rect.width, rect.height);

						Text.Font = GameFont.Small;
						Find.WindowStack.ImmediateWindow(ToolTip.GetHashCode(), rect2, WindowLayer.Super, delegate
						{
							Widgets.DrawAtlas(rect, ActiveTip.TooltipBGAtlas);
							Text.Font = GameFont.Small;
							Widgets.Label(rect.ContractedBy(4f), ToolTip);
						}, false, false, 1f);
					}
				}
			}
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
		public Color? ColorOverride = null;

        public BoolInputWidget(P parent, string label, GetValue getValue, SetValue setValue, ShouldDrawInput shouldDrawInput = null) : base(parent, label, getValue, setValue, shouldDrawInput)
        {
            this.value = base.getValue(base.Parent);
            this.ResetBuffers();
        }

		protected override void DrawInput(float x, ref float y, float width)
		{
			if (ColorOverride != null)
				GUI.color = ColorOverride.Value;
			this.value = WindowUtil.DrawInput(x, ref y, width, base.label, this.value, delegate (bool b) { base.setValue(this.Parent, b); });
			GUI.color = Color.white;
		}

        public override void ResetBuffers()
        {
            this.value = base.getValue(this.Parent);
        }
    }

    class IntInputWidget<P> : AInputWidget<P, int>
    {
        private int value;
		public Predicate<int> IsValid = null;

        public IntInputWidget(P parent, string label, GetValue getValue, SetValue setValue, ShouldDrawInput shouldDrawInput = null) : base(parent, label, getValue, setValue, shouldDrawInput)
		{
            this.value = base.getValue(base.Parent);
            this.ResetBuffers();
        }

        protected override void DrawInput(float x, ref float y, float width)
		{
			this.buffer = WindowUtil.DrawInput(x, ref y, width, this.label, this.value, delegate (int i)
			{
				if (IsValid == null || IsValid(i))
					base.setValue(this.Parent, i);
			}, this.buffer);
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

    class MinMaxInputWidget<P, V> : IInputWidget
    {
        private readonly string label;
        private readonly AInputWidget<P, V> min;
        private readonly AInputWidget<P, V> max;

        public P Parent { get { return min.Parent; } }

		public string DisplayLabel => label;

		public MinMaxInputWidget(string label, AInputWidget<P, V> min, AInputWidget<P, V> max)
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

	static class WidgetFactory<P>
    {
		public static MinMaxInputWidget<P, int> CreateMinMaxIntWidget(P parent, string label, GetValue<P, int> getMinValue, SetValue<P, int> setMinValue, GetValue<P, int> getMaxValue, SetValue<P, int> setMaxValue)
		{
			return new MinMaxInputWidget<P, int>(
				label,
				new IntInputWidget<P>(parent, "Min", p => getMinValue(p), (p, v) => setMinValue(p, v)),
				new IntInputWidget<P>(parent, "Max", p => getMaxValue(p), (p, v) => setMaxValue(p, v)));
		}

		public static MinMaxInputWidget<P, float> CreateMinMaxFloatWidget(P parent, string label, GetValue<P, float> getMinValue, SetValue<P, float> setMinValue, GetValue<P, float> getMaxValue, SetValue<P, float> setMaxValue)
		{
			return new MinMaxInputWidget<P, float>(
				label,
				new FloatInputWidget<P>(parent, "Min", p => getMinValue(p), (p, v) => setMinValue(p, v)),
				new FloatInputWidget<P>(parent, "Max", p => getMaxValue(p), (p, v) => setMaxValue(p, v)));
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

		public DefInputWidget(P parent, string label, float labelWidth, GetValue getValue, SetValue setValue, bool includeNullOption, Predicate<D> exclude = null, ShouldDrawInput shouldDrawInput = null) : base(parent, label, getValue, setValue, shouldDrawInput)
		{
			this.labelWidth = labelWidth;
			args = new FloatOptionsArgs<D>()
			{
				getDisplayName = def => Util.GetLabel(def),
				items = Util.SortedDefList<D>(exclude),
				onSelect = def => base.setValue(base.Parent, def),
				includeNullOption = includeNullOption
			};
		}

		protected override void DrawInput(float x, ref float y, float width)
		{
			WindowUtil.DrawInput(x, ref y, width, base.label, this.labelWidth, Util.GetLabel(base.getValue(base.Parent)), this.args);
		}

		public override void ResetBuffers() { }
	}

	class DefPlusMinusInputWidget<D> : IInputWidget where D : Def, new()
	{
		public delegate List<D> GetItems();
		private readonly string label;
		private readonly float labelWidth;
		private readonly GetItems getItems;
		private WindowUtil.PlusMinusArgs<D> args;

		public DefPlusMinusInputWidget(string label, float labelWidth, GetItems getItems)
		{
			this.label = label;
			this.labelWidth = labelWidth;
			this.getItems = getItems;

			args = new PlusMinusArgs<D>()
			{
				getDisplayName = def => Util.GetLabel(def),
				allItems = DefDatabase<D>.AllDefs,
				onAdd = def => Util.AddTo(this.getItems(), def),
				onRemove = def => Util.RemoveFrom(this.getItems(), def),
				beingUsed = () => this.getItems()
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
					WindowUtil.DrawLabel(x, y, width, "- " + Util.GetLabel(def));
					y += 30;
				}
			}
		}

		public void ResetBuffers() { }
	}

	class TextPlusMinusInputWidget : IInputWidget
	{
		public delegate List<string> GetItems();
		private readonly string label;
		private readonly float labelWidth;
		private readonly GetItems getItems;
		private WindowUtil.PlusMinusArgs<string> args;

		public TextPlusMinusInputWidget(string label, float labelWidth, GetItems getItems)
		{
			this.label = label;
			this.labelWidth = labelWidth;
			this.getItems = getItems;

			args = new PlusMinusArgs<string>()
			{
				getDisplayName = s => s,
				onRemove = s => this.getItems().Remove(s),
				beingUsed = () => this.getItems(),
				addArgs = new FloatOptionsArgs<string>()
				{
					skipListCustomOnly = true,
					onCustomOption = () => Find.WindowStack.Add(new Dialog_Name(
						"Tag",
						delegate (string s)
						{
							this.getItems().Add(s);
						},
						delegate (string s)
						{
							return s.Trim().Length > 0 && !this.getItems().Contains(s);
						})),
				},
				removeArgs = new FloatOptionsArgs<string>()
				{
					getDisplayName = (s) => s,
					items = this.getItems(),
					onSelect = (s) => this.getItems().Remove(s),
				},
			};
		}

		public string DisplayLabel => label;

		public void Draw(float x, ref float y, float width)
		{
			WindowUtil.PlusMinusLabel(x, ref y, width, label, this.args);
			IEnumerable<string> beingUsed = this.args.beingUsed.Invoke();
			x += 10;
			foreach (var s in beingUsed)
			{
				WindowUtil.DrawLabel(x, y, width, $"- {s}");
				y += 30;
			}
		}

		public void ResetBuffers()
		{

		}
	}
}
