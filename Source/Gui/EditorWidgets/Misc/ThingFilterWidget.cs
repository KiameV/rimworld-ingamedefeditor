using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Linq;
using System.Collections.Generic;
using Verse;
using static InGameDefEditor.WindowUtil;
using InGameDefEditor.Gui.Dialog;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	class ThingFilterWidget : ACollapsibleWidget
	{
		public readonly ThingFilter ThingFilter;
		private readonly string label;

		public override string DisplayLabel => label;

		public ThingFilterWidget(string label, ThingFilter tf) : base(true, true)
		{
			this.label = label;
			this.ThingFilter = tf;
		}

		public override void ResetBuffers()
		{

		}

		protected override void DrawInputs(float x, ref float y, float width)
		{

		}
	}

	/*public readonly ThingFilter ThingFilter;

	private readonly DrawOptionsEnum DrawOptions;

	public string DisplayLabel => "Thing Filter";

	//private readonly List<IInputWidget> inputWidgets;

	private FloatOptionsArgs<FoodPreferability> disallowWorsePreferabilityArgs;

	private PlusMinusArgs<ThingDef> thingDefsPlusMinusArgs;
	private PlusMinusArgs<ThingDef> disallowedThingDefsPlusMinusArgs;

	private PlusMinusArgs<string> categories;
	private PlusMinusArgs<string> disallowedCategories;

	private PlusMinusArgs<string> specialFiltersToAllow;
	private PlusMinusArgs<string> specialFiltersToDisallow;


	private QualityRangeWidget allowedQualities;
	private PlusMinusArgs<ThingDef> allowedDefs;
	private PlusMinusArgs<ThingDef> allowAllWhoCanMake;
	private PlusMinusArgs<StuffCategoryDef> stuffCategoriesToAllow;
	private PlusMinusArgs<SpecialThingFilterDef> disallowedSpecialFilters;

	private List<string> tradeTagsToAllow;
	private List<string> tradeTagsToDisallow;
	private List<string> thingSetMakerTagsToAllow;
	private List<string> thingSetMakerTagsToDisallow;* /

	[Flags]
	public enum DrawOptionsEnum
	{
		All = 1,
		Category = 2,
		SpecialFilters = 4,
	}

	public ThingFilterWidget(ThingFilter thingFilter, DrawOptionsEnum drawOptions = DrawOptionsEnum.All)
	{
		this.ThingFilter = thingFilter;
		this.DrawOptions = drawOptions;

		/*inputWidgets = new List<IInputWidget>()
		{
			new BoolInputWidget<ThingFilter>(this.ThingFilter, "Allowed Hit Points Configurable", (tf) => tf.allowedHitPointsConfigurable, (tf, b) => tf.allowedHitPointsConfigurable = b),
			new MinMaxInputWidget<ThingFilter>("Allowed Hit Points Percents",  
				new FloatInputWidget<ThingFilter>(this.ThingFilter, "Min", (tf) => tf.AllowedHitPointsPercents.min, (tf, f) => tf.AllowedHitPointsPercents = new FloatRange(f, tf.AllowedHitPointsPercents.max)),
				new FloatInputWidget<ThingFilter>(this.ThingFilter, "Max", (tf) => tf.AllowedHitPointsPercents.max, (tf, f) => tf.AllowedHitPointsPercents = new FloatRange(tf.AllowedHitPointsPercents.min, f))),
			new BoolInputWidget<ThingFilter>(this.ThingFilter, "Allowed Qualities Configurable", (tf) => tf.allowedQualitiesConfigurable, (tf, b) => tf.allowedQualitiesConfigurable = b),
			new BoolInputWidget<ThingFilter>(this.ThingFilter, "Disallow Inedible By Human", (tf) => ThingFilterStats.GetDisallowInedibleByHuman(tf), (tf, b) => ThingFilterStats.SetDisallowInedibleByHuman(tf, b)),
			//new FloatInputWidget<ThingFilter>(this.ThingFilter, "Disallow Cheaper Than", (tf) => ThingFilterStats.GetDisallowCheaperThan(tf), (tf, f) => ThingFilterStats.SetDisallowCheaperThan(tf, f)),
		};* /

		this.disallowWorsePreferabilityArgs = new FloatOptionsArgs<FoodPreferability>()
		{
			items = Enum.GetValues(typeof(FoodPreferability)).Cast<FoodPreferability>().ToList(),
			getDisplayName = (fp) => fp.ToString(),
			includeNullOption = true,
			onSelect = (fp) => ThingFilterStats.SetDisallowWorsePreferability(this.ThingFilter, fp),
		};

		IEnumerable<ThingDef> tds = this.GetThingDefsSorted();

		this.thingDefsPlusMinusArgs = new PlusMinusArgs<ThingDef>()
		{
			allItems = tds,
			beingUsed = () => ThingFilterStats.GetAllowedDefs(this.ThingFilter),
			getDisplayName = (td) => td.label,
			onAdd = delegate (ThingDef td)
			{
				ThingFilterStats.GetThingDefs(this.ThingFilter).Add(td);
				this.ThingFilter.ResolveReferences();
			},
			onRemove = delegate (ThingDef td)
			{
				ThingFilterStats.GetAllowedDefs(this.ThingFilter).Remove(td);
				ThingFilterStats.GetThingDefs(this.ThingFilter).Remove(td);
				this.ThingFilter.ResolveReferences();
			},
		};

		this.disallowedThingDefsPlusMinusArgs = new PlusMinusArgs<ThingDef>()
		{
			allItems = tds,
			beingUsed = () => ThingFilterStats.GetDisallowedThingDefs(this.ThingFilter),
			getDisplayName = (td) => td.label,
			onAdd = delegate (ThingDef td)
			{
				ThingFilterStats.GetDisallowedThingDefs(this.ThingFilter).Add(td);
				this.ThingFilter.ResolveReferences();
			},
			onRemove = delegate (ThingDef td)
			{
				ThingFilterStats.GetDisallowedThingDefs(this.ThingFilter).Remove(td);
				this.ThingFilter.ResolveReferences();
			},
		};


		SortedDictionary<string, object> categoryByDefName = new SortedDictionary<string, object>();
		foreach (var v in DefDatabase<ThingCategoryDef>.AllDefsListForReading)
			categoryByDefName.Add(v.defName, null);

		this.categories = new PlusMinusArgs<string>()
		{
			allItems = categoryByDefName.Keys,
			isBeingUsed = (s) => ThingFilterStats.GetCategories(this.ThingFilter).Contains(s),
			getDisplayName = (s) => s,
			onCustomOption = () => Find.WindowStack.Add(new Dialog_Name(
				"Allowed Custom Category", delegate (string s)
				{
					ThingFilterStats.GetCategories(this.ThingFilter).Add(s);
					ThingFilterStats.GetDisallowedCategories(this.ThingFilter).Remove(s);
					this.ThingFilter.ResolveReferences();
				},
				delegate (string name)
				{
					if (ThingFilterStats.GetCategories(this.ThingFilter).Contains(name))
						return "Category with name \"" + name + "\" already exists.";
					return true;
				})),
			onAdd = delegate (string s)
			{
				ThingFilterStats.GetCategories(this.ThingFilter).Add(s);
				ThingFilterStats.GetDisallowedCategories(this.ThingFilter).Remove(s);
				this.ThingFilter.ResolveReferences();

			},
			onRemove = delegate (string s)
			{
				ThingFilterStats.GetCategories(this.ThingFilter).Remove(s);
				this.ThingFilter.ResolveReferences();
			}
		};

		this.disallowedCategories = new PlusMinusArgs<string>()
		{
			allItems = categoryByDefName.Keys,
			isBeingUsed = (s) => ThingFilterStats.GetDisallowedCategories(this.ThingFilter).Contains(s),
			getDisplayName = (s) => s,
			onCustomOption = () => Find.WindowStack.Add(new Dialog_Name(
				"Disallowed Custom Category", delegate (string s)
				{
					ThingFilterStats.GetDisallowedCategories(this.ThingFilter).Add(s);
					ThingFilterStats.GetCategories(this.ThingFilter).Remove(s);
					this.ThingFilter.ResolveReferences();
				},
				delegate (string name)
				{
					if (ThingFilterStats.GetDisallowedCategories(this.ThingFilter).Contains(name))
						return "Disallow category to disallow with name \"" + name + "\" already exists.";
					return true;
				})),
			onAdd = delegate (string s)
			{
				ThingFilterStats.GetDisallowedCategories(this.ThingFilter).Add(s);
				ThingFilterStats.GetCategories(this.ThingFilter).Remove(s);
				this.ThingFilter.ResolveReferences();

			},
			onRemove = delegate (string s)
			{
				ThingFilterStats.GetDisallowedCategories(this.ThingFilter).Remove(s);
				this.ThingFilter.ResolveReferences();
			}
		};

		List<String> specialFilters = new List<string>() { "AllowCorpsesColonist", "AllowCorpsesStranger" };
		this.specialFiltersToAllow = new PlusMinusArgs<string>()
		{
			allItems = specialFilters,
			beingUsed = () => ThingFilterStats.GetSpecialFiltersToAllow(this.ThingFilter),
			getDisplayName = (s) => s,
			onCustomOption = () => Find.WindowStack.Add(new Dialog_Name(
				"Allowed Custom Filter", delegate (string s)
				{
					ThingFilterStats.GetSpecialFiltersToAllow(this.ThingFilter).Add(s);
					ThingFilterStats.GetSpecialFiltersToDisallow(this.ThingFilter).Remove(s);
					this.ThingFilter.ResolveReferences();

				},
				delegate (string name)
				{
					if (ThingFilterStats.GetSpecialFiltersToAllow(this.ThingFilter).Contains(name))
						return "Special Filter with name \"" + name + "\" already exists.";
					return true;
				})),
			onAdd = delegate (string s)
			{
				ThingFilterStats.GetSpecialFiltersToAllow(this.ThingFilter).Add(s);
				ThingFilterStats.GetSpecialFiltersToDisallow(this.ThingFilter).Remove(s);
				this.ThingFilter.ResolveReferences();

			},
			onRemove = delegate (string s)
			{
				ThingFilterStats.GetSpecialFiltersToAllow(this.ThingFilter).Remove(s);
				this.ThingFilter.ResolveReferences();
			}
		};

		this.specialFiltersToDisallow = new PlusMinusArgs<string>()
		{
			allItems = specialFilters,
			beingUsed = () => ThingFilterStats.GetSpecialFiltersToDisallow(this.ThingFilter),
			getDisplayName = (s) => s,
			onCustomOption = () => Find.WindowStack.Add(new Dialog_Name(
				"Disallow Custom Filter", delegate (string s)
				{
					ThingFilterStats.GetSpecialFiltersToDisallow(this.ThingFilter).Add(s);
					ThingFilterStats.GetSpecialFiltersToAllow(this.ThingFilter).Remove(s);
					this.ThingFilter.ResolveReferences();

				},
				delegate (string name)
				{
					if (ThingFilterStats.GetSpecialFiltersToDisallow(this.ThingFilter).Contains(name))
						return "Special Filter with name \"" + name + "\" already exists.";
					return true;
				})),
			onAdd = delegate (string s)
			{
				ThingFilterStats.GetSpecialFiltersToDisallow(this.ThingFilter).Add(s);
				ThingFilterStats.GetSpecialFiltersToAllow(this.ThingFilter).Remove(s);
				this.ThingFilter.ResolveReferences();

			},
			onRemove = delegate (string s)
			{
				ThingFilterStats.GetSpecialFiltersToDisallow(this.ThingFilter).Remove(s);
				this.ThingFilter.ResolveReferences();
			}
		};

		this.ResetBuffers();
	}

	public void Draw(float x, ref float y, float width)
	{
		/*if (this.ShouldDrawPart(DrawOptionsEnum.All))
		{
			for (int i = 0; i < this.inputWidgets.Count; ++i)
				if (i != 1 || this.ThingFilter.allowedHitPointsConfigurable)
					this.inputWidgets[i].Draw(x, ref y, width);
		}* /

		WindowUtil.PlusMinusLabel(x, ref y, width, "Allowed Defs", this.thingDefsPlusMinusArgs);
		foreach (var v in ThingFilterStats.GetAllowedDefs(this.ThingFilter))
		{
			WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v.label);
			y += 32;
		}
		y += 8;

		/*WindowUtil.DrawLabel(x, y, 100, "Thing Defs", true);
		y += 32;
		foreach (var v in ThingFilterStats.GetThingDefs(this.ThingFilter))
		{
			WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v.label);
			y += 32;
		}
		y += 8;* /

		if (this.ShouldDrawPart(DrawOptionsEnum.All) ||
			this.ShouldDrawPart(DrawOptionsEnum.Category))
		{
			WindowUtil.PlusMinusLabel(x, ref y, width, "Allowed Categories", this.categories);
			foreach (var v in ThingFilterStats.GetCategories(this.ThingFilter))
			{
				WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v);
				y += 32;
			}
			y += 8;

			WindowUtil.PlusMinusLabel(x, ref y, width, "Disallow Categories", this.disallowedCategories);
			foreach (var v in ThingFilterStats.GetDisallowedCategories(this.ThingFilter))
			{
				WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v);
				y += 32;
			}
			y += 8;
		}

		if (this.ShouldDrawPart(DrawOptionsEnum.All) ||
			this.ShouldDrawPart(DrawOptionsEnum.SpecialFilters))
		{
			WindowUtil.PlusMinusLabel(x, ref y, width, "Disallowed Defs", this.disallowedThingDefsPlusMinusArgs);
			foreach (var v in ThingFilterStats.GetDisallowedThingDefs(this.ThingFilter))
			{
				WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v.label);
				y += 32;
			}
			y += 8;

			WindowUtil.PlusMinusLabel(x, ref y, width, "Allowed Custom Filters", this.specialFiltersToAllow);
			foreach (var v in ThingFilterStats.GetSpecialFiltersToAllow(this.ThingFilter))
			{
				WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v);
				y += 32;
			}
			y += 8;

			WindowUtil.PlusMinusLabel(x, ref y, width, "Disallowed Custom Filters", this.specialFiltersToDisallow);
			foreach (var v in ThingFilterStats.GetSpecialFiltersToDisallow(this.ThingFilter))
			{
				WindowUtil.DrawLabel(x + 10, y, width - 10, "- " + v);
				y += 32;
			}
			y += 8;
		}
	}

	public void ResetBuffers()
	{
		//foreach (var v in inputWidgets)
		//	v.ResetBuffers();

		if (ThingFilterStats.GetAllowedDefs(this.ThingFilter) == null)
			ThingFilterStats.SetAllowedDefs(this.ThingFilter, new HashSet<ThingDef>());
		if (ThingFilterStats.GetThingDefs(this.ThingFilter) == null)
			ThingFilterStats.SetThingDefs(this.ThingFilter, new List<ThingDef>());
		this.AddRange(ThingFilterStats.GetAllowedDefs(this.ThingFilter), ThingFilterStats.GetThingDefs(this.ThingFilter));
		if (ThingFilterStats.GetDisallowedThingDefs(this.ThingFilter) == null)
			ThingFilterStats.SetDisallowedThingDefs(this.ThingFilter, new List<ThingDef>());
		if (ThingFilterStats.GetCategories(this.ThingFilter) == null)
			ThingFilterStats.SetCategories(this.ThingFilter, new List<string>());
		if (ThingFilterStats.GetDisallowedCategories(this.ThingFilter) == null)
			ThingFilterStats.SetDisallowedCategories(this.ThingFilter, new List<string>());
		if (ThingFilterStats.GetSpecialFiltersToAllow(this.ThingFilter) == null)
			ThingFilterStats.SetSpecialFiltersToAllow(this.ThingFilter, new List<string>());
		if (ThingFilterStats.GetSpecialFiltersToDisallow(this.ThingFilter) == null)
			ThingFilterStats.SetSpecialFiltersToDisallow(this.ThingFilter, new List<string>());
	}

	private void AddRange(IEnumerable<ThingDef> from, List<ThingDef> to)
	{
		foreach (var v in from)
			if (!to.Contains(v))
				to.Add(v);
	}

	private bool ShouldDrawPart(DrawOptionsEnum d)
	{
		return (this.DrawOptions & d) == d;
	}

	private IEnumerable<ThingDef> GetThingDefsSorted()
	{
		SortedDictionary<string, ThingDef> d = new SortedDictionary<string, ThingDef>();
		foreach (var v in DefDatabase<ThingDef>.AllDefsListForReading)
			d[v.label] = v;
		return d.Values;
	}
}*/
}