using InGameDefEditor.Gui.EditorWidgets.Misc;
using InGameDefEditor.Stats;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets
{
	class TraitWidget : AParentDefStatWidget<TraitDef>
	{
		private readonly List<IInputWidget> inputWidgets;
		private readonly List<WorkTags> workTags;

		private List<TraitDegreeDataWidget> traitDegreeDataWidgets;

		private PlusMinusArgs<WorkTags> requiredWorkTags;
		private PlusMinusArgs<WorkTags> disabledWorkTags;

		private PlusMinusArgs<WorkTypeDef> disabledWorkTypes;
		private PlusMinusArgs<WorkTypeDef> requiredWorkTypes;
		private PlusMinusArgs<TraitDef> conflictingTraits;

		public TraitWidget(TraitDef def, DefType type) : base(def, type)
		{
			this.inputWidgets = new List<IInputWidget>()
			{
				new FloatInputWidget<TraitDef>(base.Def, "Commonality Male", d => TraitDefStat.GetCommonality(d), (d, v) => TraitDefStat.SetCommonality(d, v)),
				new FloatInputWidget<TraitDef>(base.Def, "Commonality Female", d => TraitDefStat.GetCommonalityFemale(d), (d, v) => TraitDefStat.SetCommonalityFemale(d, v)),
				new BoolInputWidget<TraitDef>(base.Def, "Allow On Hostile", d => d.allowOnHostileSpawn, (d, v) => d.allowOnHostileSpawn = v),
			};

			var dic = new SortedDictionary<string, WorkTags>();
			foreach (var v in Enum.GetValues(typeof(WorkTags)).Cast<WorkTags>())
				if (v != WorkTags.None)
					dic.Add(v.ToString(), v);
			this.workTags = new List<WorkTags>(dic.Values);
			dic.Clear();
			dic = null;

			this.requiredWorkTags = new PlusMinusArgs<WorkTags>()
			{
				allItems = this.workTags,
				isBeingUsed = v => (base.Def.requiredWorkTags & v) == v,
				onAdd = v =>
				{
					base.Def.requiredWorkTags |= v;
					base.Def.disabledWorkTags &= ~v;
				},
				onRemove = v => base.Def.requiredWorkTags &= ~v,
				getDisplayName = v => v.ToString()
			};

			this.disabledWorkTags = new PlusMinusArgs<WorkTags>()
			{
				allItems = this.workTags,
				isBeingUsed = v => (base.Def.disabledWorkTags & v) == v,
				onAdd = v =>
				{
					base.Def.disabledWorkTags |= v;
					base.Def.requiredWorkTags &= ~v;
				},
				onRemove = v => base.Def.disabledWorkTags &= ~v,
				getDisplayName = v => v.ToString()
			};

			this.disabledWorkTypes = new PlusMinusArgs<WorkTypeDef>()
			{
				allItems = DefDatabase<WorkTypeDef>.AllDefs,
				beingUsed = () => base.Def.disabledWorkTypes,
				onAdd = v =>
				{
					base.Def.disabledWorkTypes = Util.AddTo(base.Def.disabledWorkTypes, v);
					base.Def.requiredWorkTypes = Util.RemoveFrom(base.Def.requiredWorkTypes, v);
				},
				onRemove = v => base.Def.disabledWorkTypes = Util.RemoveFrom(base.Def.disabledWorkTypes, v),
				getDisplayName = v => Util.GetDefLabel(v)
			};

			this.requiredWorkTypes = new PlusMinusArgs<WorkTypeDef>()
			{
				allItems = DefDatabase<WorkTypeDef>.AllDefs,
				beingUsed = () => base.Def.requiredWorkTypes,
				onAdd = v =>
				{
					base.Def.requiredWorkTypes = Util.AddTo(base.Def.requiredWorkTypes, v);
					base.Def.disabledWorkTypes = Util.RemoveFrom(base.Def.disabledWorkTypes, v);
				},
				onRemove = v => base.Def.requiredWorkTypes = Util.RemoveFrom(base.Def.requiredWorkTypes, v),
				getDisplayName = v => Util.GetDefLabel(v)
			};

			this.conflictingTraits = new PlusMinusArgs<TraitDef>()
			{
				allItems = DefDatabase<TraitDef>.AllDefs,
				beingUsed = () => base.Def.conflictingTraits,
				onAdd = v => base.Def.conflictingTraits = Util.AddTo(base.Def.conflictingTraits, v),
				onRemove = v => base.Def.conflictingTraits = Util.RemoveFrom(base.Def.conflictingTraits, v),
				getDisplayName = v => Util.GetDefLabel(v)
			};

			this.Rebuild();
		}

		public override void DrawLeft(float x, ref float y, float width)
		{
			foreach (var v in this.inputWidgets)
				v.Draw(x, ref y, width);

			WindowUtil.PlusMinusLabel(x, ref y, width, "Required Work Tags", this.requiredWorkTags);
			foreach (var v in this.workTags)
				if ((base.Def.requiredWorkTags & v) == v)
					WindowUtil.DrawLabel(x + 20, ref y, width, "- " + v.ToString(), 30);

			WindowUtil.PlusMinusLabel(x, ref y, width, "Disabled Work Tags", this.disabledWorkTags);
			foreach (var v in this.workTags)
				if ((base.Def.disabledWorkTags & v) == v)
					WindowUtil.DrawLabel(x + 20, ref y, width, "- " + v.ToString(), 30);
		}

		public override void DrawMiddle(float x, ref float y, float width)
		{
			foreach (var v in this.traitDegreeDataWidgets)
				v.Draw(x, ref y, width);
		}

		public override void DrawRight(float x, ref float y, float width)
		{
			WindowUtil.PlusMinusLabel(x, ref y, width, "Required Work Types", this.requiredWorkTypes);
			foreach (var v in this.Def.requiredWorkTypes)
			{
				WindowUtil.DrawLabel(x + 20, y, width, "- " + Util.GetDefLabel(v));
				y += 30;
			}
			WindowUtil.PlusMinusLabel(x, ref y, width, "Disabled Work Types", this.disabledWorkTypes);
			foreach (var v in this.Def.disabledWorkTypes)
			{
				WindowUtil.DrawLabel(x + 20, y, width, "- " + Util.GetDefLabel(v));
				y += 30;
			}
			WindowUtil.PlusMinusLabel(x, ref y, width, "Confliting Traits", this.conflictingTraits);
			foreach (var v in this.Def.conflictingTraits)
			{
				WindowUtil.DrawLabel(x + 20, y, width, "- " + Util.GetDefLabel(v));
				y += 30;
			}
		}

		public override void Rebuild()
		{
			if (this.traitDegreeDataWidgets == null)
				this.traitDegreeDataWidgets = new List<TraitDegreeDataWidget>();
			else
				this.traitDegreeDataWidgets.Clear();
			this.Def.degreeDatas?.ForEach(v => this.traitDegreeDataWidgets.Add(new TraitDegreeDataWidget(v)));
			this.ResetBuffers();
		}

		public override void ResetBuffers()
		{
			this.inputWidgets.ForEach(v => v.ResetBuffers());
			this.traitDegreeDataWidgets.ForEach(v => v.ResetBuffers());
		}
	}
}
