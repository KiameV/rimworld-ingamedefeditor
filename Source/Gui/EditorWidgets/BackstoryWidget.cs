using InGameDefEditor.Gui.EditorWidgets.Misc;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System.Collections.Generic;
using Verse;
using InGameDefEditor.Gui.EditorWidgets.Dialog;
using static InGameDefEditor.WindowUtil;
using System;
using System.Linq;
using InGameDefEditor.Stats;
using System.Text;
using UnityEngine;

namespace InGameDefEditor.Gui.EditorWidgets
{
	class BackstoryWidget : AParentDefStatWidget<Backstory>
	{
		private readonly Backstory Backstory;
		private readonly DefType type;
		private readonly List<IInputWidget> inputWidgets;

		private List<WorkTags> workTags;
		private PlusMinusArgs<WorkTags> workDisables;
		private PlusMinusArgs<WorkTags> requiredWorkTags;

		private List<IntInputWidget<Dictionary<SkillDef, int>>> skillGains;
		private PlusMinusArgs<SkillDef> skillGainsPlusMinus;

		private List<IntInputWidget<TraitEntry>> forcedTraits;
		private PlusMinusArgs<TraitDef> forcedTraitsPlusMinus;

		private List<IntInputWidget<TraitEntry>> disallowedTraits;
		private PlusMinusArgs<TraitDef> disallowedTraitsPlusMinus;

		//public List<string> spawnCategories = new List<string>();

		public override string DisplayLabel => this.Backstory.title;

		public BackstoryWidget(Backstory backstory, DefType type) : base(backstory, type)
		{
			if (backstory.skillGainsResolved == null)
				backstory.skillGainsResolved = new Dictionary<SkillDef, int>();
			if (backstory.forcedTraits == null)
				backstory.forcedTraits = new List<TraitEntry>();
			if (backstory.disallowedTraits == null)
				backstory.disallowedTraits = new List<TraitEntry>();

			this.Backstory = backstory;
			this.type = type;
			this.inputWidgets = new List<IInputWidget>()
			{
				new BoolInputWidget<Backstory>(this.Backstory, "Shuffleable", b => b.shuffleable, (b, v) => b.shuffleable = v),
				new EnumInputWidget<Backstory, BackstorySlot>(this.Backstory, "Slot", 200, b => b.slot, (b, v) => b.slot = v),
				new DefInputWidget<Backstory, BodyTypeDef>(this.Backstory, "Body Type Global", 200, b => BackstoryStats.GetBodyTypeGlobal(b), (b, v) => BackstoryStats.SetBodyTypeGlobal(b, v), true),
				new DefInputWidget<Backstory, BodyTypeDef>(this.Backstory, "Body Type Male", 200, b => BackstoryStats.GetBodyTypeMale(b), (b, v) => BackstoryStats.SetBodyTypeMale(b, v), true, d => d == BodyTypeDefOf.Female),
				new DefInputWidget<Backstory, BodyTypeDef>(this.Backstory, "Body Type Female", 200, b => BackstoryStats.GetBodyTypeFemale(b), (b, v) => BackstoryStats.SetBodyTypeFemale(b, v), true, d => d == BodyTypeDefOf.Male),
			};

			var dic = new SortedDictionary<string, WorkTags>();
			foreach (var v in Enum.GetValues(typeof(WorkTags)).Cast<WorkTags>())
				dic.Add(v.ToString(), v);
			this.workTags = new List<WorkTags>(dic.Values);
			dic.Clear();
			dic = null;

			this.workDisables = new PlusMinusArgs<WorkTags>()
			{
				allItems = this.workTags,
				isBeingUsed = v => (this.Backstory.workDisables & v) == v,
				onAdd = v => this.Backstory.workDisables |= v,
				onRemove = v => this.Backstory.workDisables &= ~v,
				getDisplayName = v => v.ToString()
			};

			this.requiredWorkTags = new PlusMinusArgs<WorkTags>()
			{
				allItems = this.workTags,
				isBeingUsed = v => (this.Backstory.requiredWorkTags & v) == v,
				onAdd = v => this.Backstory.requiredWorkTags |= v,
				onRemove = v => this.Backstory.requiredWorkTags &= ~v,
				getDisplayName = v => v.ToString()
			};

			this.skillGainsPlusMinus = new PlusMinusArgs<SkillDef>()
			{
				allItems = Util.SortedDefList<SkillDef>(),
				beingUsed = () => this.Backstory.skillGainsResolved.Keys,
				onAdd = v =>
				{
					this.Backstory.skillGainsResolved[v] = 0;
					this.skillGains.Add(this.CreateSkillGainsInput(v));
				},
				onRemove = v =>
				{
					this.Backstory.skillGainsResolved.Remove(v);
					for (int i = 0; i < this.skillGains.Count; ++i)
						if (this.skillGains[i].DisplayLabel == Util.GetLabel(v))
						{
							this.skillGains.RemoveAt(i);
							break;
						}
				},
				getDisplayName = v => Util.GetLabel(v),
			};
			
			this.forcedTraitsPlusMinus = new PlusMinusArgs<TraitDef>()
			{
				allItems = Util.SortedDefList<TraitDef>(),
				beingUsed = () => Util.ConvertItems(this.Backstory.forcedTraits, v => v.def),
				onAdd = v =>
				{
					TraitEntry te = new TraitEntry(v, 0);
					this.Backstory.forcedTraits.Add(te);
					this.forcedTraits.Add(this.CreateTraitEntryInput(te));
					this.RemoveDisallowedTraits(v);
				},
				onRemove = v =>
				{
					this.RemoveForcedTraits(v);
				},
				getDisplayName = v => Util.GetLabel(v),
			};

			this.disallowedTraitsPlusMinus = new PlusMinusArgs<TraitDef>()
			{
				allItems = Util.SortedDefList<TraitDef>(),
				beingUsed = () => Util.ConvertItems(this.Backstory.disallowedTraits, v => v.def),
				onAdd = v =>
				{
					TraitEntry te = new TraitEntry(v, 0);
					this.Backstory.disallowedTraits.Add(te);
					this.disallowedTraits.Add(this.CreateTraitEntryInput(te));
					this.RemoveForcedTraits(v);
				},
				onRemove = v =>
				{
					this.RemoveDisallowedTraits(v);
				},
				getDisplayName = v => Util.GetLabel(v),
			};

			this.Rebuild();
		}

		public void DisableAutoDeploy()
		{
			Defs.ApplyStatsAutoDefs.Remove(this.Backstory.identifier);
		}

		public override void DrawLeft(float x, ref float y, float width)
		{
			foreach (var v in this.inputWidgets)
				v.Draw(x, ref y, width);
		}

		public override void DrawMiddle(float x, ref float y, float width)
		{
			PlusMinusLabel(x, ref y, width, "Skill Gains", this.skillGainsPlusMinus);
			foreach (var v in this.skillGains)
				v.Draw(x, ref y, width);

			PlusMinusLabel(x, ref y, width, "Required Work Tags", this.requiredWorkTags);
			WindowUtil.DrawFlagList(x, ref y, width, workTags, (int)this.Backstory.requiredWorkTags, v => v == WorkTags.None);

			y += 10;
			PlusMinusLabel(x, ref y, width, "Disabled Work Tags", this.workDisables);
			WindowUtil.DrawFlagList(x, ref y, width, workTags, (int)this.Backstory.workDisables, v => v == WorkTags.None);

			y += 10;
			PlusMinusLabel(x, ref y, width, "Forced Traits", this.forcedTraitsPlusMinus);
			foreach (var v in this.forcedTraits)
				v.Draw(x + 10, ref y, width - 10);

			y += 10;
			PlusMinusLabel(x, ref y, width, "Disallowed Traits", this.disallowedTraitsPlusMinus);
			foreach (var v in this.disallowedTraits)
				v.Draw(x + 10, ref y, width - 10);
		}

		public override void DrawRight(float x, ref float y, float width)
		{
			DrawLabel(x, ref y, width, "Spawn Categories", 30f, true);
			foreach (var v in this.Backstory.spawnCategories)
				DrawLabel(10, ref y, width - 10, "- " + v);
		}

		public override void Rebuild()
		{
			this.ResetBuffers();
		}

		public override void ResetBuffers()
		{
			this.inputWidgets?.ForEach(v => v.ResetBuffers());
			this.skillGains?.ForEach(v => v.ResetBuffers());
			this.forcedTraits?.ForEach(v => v.ResetBuffers());
			this.disallowedTraits?.ForEach(v => v.ResetBuffers());

			if (this.skillGains == null)
				this.skillGains = new List<IntInputWidget<Dictionary<SkillDef, int>>>();
			this.skillGains.Clear();
			foreach (KeyValuePair<SkillDef, int> kv in this.Backstory.skillGainsResolved)
				this.skillGains.Add(this.CreateSkillGainsInput(kv.Key));

			if (this.forcedTraits != null)
				this.forcedTraits.Clear();
			Util.Populate(out this.forcedTraits, this.Backstory.forcedTraits, te => this.CreateTraitEntryInput(te));

			if (this.disallowedTraits != null)
				this.disallowedTraits.Clear();
			Util.Populate(out this.disallowedTraits, this.Backstory.disallowedTraits, te => this.CreateTraitEntryInput(te));
		}

		public new void ResetParent()
		{
			base.ResetParent();
			Backup.ApplyStats(this.Backstory);
		}

		private void RemoveForcedTraits(TraitDef td)
		{
			this.Backstory.forcedTraits.RemoveAll(d => d.def == td);
			this.forcedTraits.RemoveAll(d => d.Parent.def == td);
		}

		private void RemoveDisallowedTraits(TraitDef td)
		{
			this.Backstory.disallowedTraits.RemoveAll(d => d.def == td);
			this.disallowedTraits.RemoveAll(d => d.Parent.def == td);
		}

		private IntInputWidget<Dictionary<SkillDef, int>> CreateSkillGainsInput(SkillDef sd)
		{
			return new IntInputWidget<Dictionary<SkillDef, int>>(this.Backstory.skillGainsResolved, Util.GetLabel(sd), d => d[sd], (d, i) => d[sd] = i);
		}

		private IntInputWidget<TraitEntry> CreateTraitEntryInput(TraitEntry te)
		{
			var input = new IntInputWidget<TraitEntry>(te, Util.GetLabel(te.def) + " (Degree)", d => d.degree, (d, i) => d.degree = i);
			StringBuilder sb = new StringBuilder(Util.GetLabel(te.def));
			sb.AppendLine();
			sb.AppendLine("Degrees:");
			foreach (var degree in te.def.degreeDatas)
			{
				sb.AppendLine(degree.degree + " = " + degree.label);
			}
			input.ToolTip = sb.ToString();
			input.IsValid = v =>
			{
				foreach (var degree in te.def.degreeDatas)
					if (degree.degree == v)
						return true;
				return false;
			};
			return input;
		}

		protected override void AddDefsToAutoApply(bool isAutoApply)
		{

		}
	}
}
