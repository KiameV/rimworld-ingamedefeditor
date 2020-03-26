using InGameDefEditor.Stats.Misc;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	class SimpleCurveWidget : ACollapsibleWidget
	{
		protected readonly string name;
		protected SimpleCurve curve;

		protected readonly List<MinMaxFloatStats> points = new List<MinMaxFloatStats>();
		protected readonly List<MinMaxInputWidget<MinMaxFloatStats, float>> pointsInputs = new List<MinMaxInputWidget<MinMaxFloatStats, float>>();

		protected readonly FloatOptionsArgs<MinMaxFloatStats> pointsArgs;

		protected Vector2 scroll;
		protected float innerY = float.MaxValue;

		public override string DisplayLabel => this.name;

		public SimpleCurveWidget(string name, SimpleCurve curve) : base(true, true)
		{
			this.name = name;
			this.curve = curve;

			this.pointsArgs = new FloatOptionsArgs<MinMaxFloatStats>()
			{
				items = this.points,
				getDisplayName = v => v.Min + ", " + v.Max,
				onSelect = v =>
				{
					this.points.RemoveAll(p => object.Equals(p, v));
					this.pointsInputs.RemoveAll(p => object.Equals(p.Parent, v));
					this.RecreateCurve();
				},
			};

			this.ResetBuffers();
		}

		protected override void DrawInputs(float x, ref float y, float width)
		{
			if (this.curve != null)
			{
				WindowUtil.PlusMinusLabel(x, ref y, width, this.name,
				() =>
				{
					MinMaxFloatStats p = new MinMaxFloatStats(0, 0);
					this.points.Add(p);
					this.pointsInputs.Add(this.CreateFloatInput(p));
				},
				() => WindowUtil.DrawFloatingOptions(pointsArgs));
				SimpleCurveDrawer.DrawCurve(new Rect(x + 20, y, width, 100), curve);
				y += 110;

				if (this.innerY > 300)
				{
					Widgets.BeginScrollView(
						new Rect(x + 20, y, width - 16, 300),
						ref this.scroll,
						new Rect(0, 0, width - 32, this.innerY));
					this.innerY = 0;

					foreach (var v in this.pointsInputs)
						v.Draw(10, ref this.innerY, width - 60);

					Widgets.EndScrollView();
					y += 332;
				}
				else
				{
					this.innerY = 0;
					foreach (var v in this.pointsInputs)
					{
						float orig = y;
						v.Draw(10, ref y, width - 60);
						this.innerY += y - orig;
					}
				}
			}
		}

		public override void ResetBuffers()
		{

			if (this.curve != null)
			{
				this.points?.Clear();
				this.pointsInputs?.Clear();
				this.curve.Points?.ForEach(v =>
				{
					var m = new MinMaxFloatStats(v.x, v.y);
					this.points.Add(m);
					this.pointsInputs.Add(this.CreateFloatInput(m));
				});
			}
			this.scroll = Vector2.zero;
		}

		private MinMaxInputWidget<MinMaxFloatStats, float> CreateFloatInput(MinMaxFloatStats m)
		{
			return new MinMaxInputWidget<MinMaxFloatStats, float>(
				"Point",
				new FloatInputWidget<MinMaxFloatStats>(m, "X", p => p.Min, (p, v) =>
				{
					if (p.Min != v)
					{
						p.Min = v;
						this.RecreateCurve();
					}
				}),
				new FloatInputWidget<MinMaxFloatStats>(m, "Y", p => p.Max, (p, v) =>
				{
					if (p.Max != v)
					{
						p.Max = v;
						this.RecreateCurve();
					}
				}));
		}

		private void RecreateCurve()
		{
			if (this.curve != null)
			{
				List<CurvePoint> l = new List<CurvePoint>(this.points.Count);
				this.points.ForEach(v => l.Add(new CurvePoint(v.Min, v.Max)));
				this.curve.SetPoints(l);
			}
		}
	}

	class SimpleCurveToggleableWidget<P> : SimpleCurveWidget
	{
		public delegate SimpleCurve GetValue(P d);
		public delegate void SetValue(P d, SimpleCurve v);

		private readonly BoolInputWidget<P> Toggle;

		public SimpleCurveToggleableWidget(P parent, string name, GetValue getter, SetValue setter) : base(name, null)
		{
			this.Toggle = new BoolInputWidget<P>(parent, "InGameDefEditor.Enabled".Translate(), p => getter(p) != null, (p, v) =>
			{
				setter(p, new SimpleCurve());
				base.ResetBuffers();
			});
			this.Toggle.ResetBuffers();
		}

		protected override void DrawInputs(float x, ref float y, float width)
		{
			this.Toggle.Draw(x, ref y, width);
			base.DrawInputs(x, ref y, width);
		}

		public override void ResetBuffers()
		{
			base.ResetBuffers();
			this.Toggle?.ResetBuffers();
		}
	}
}
