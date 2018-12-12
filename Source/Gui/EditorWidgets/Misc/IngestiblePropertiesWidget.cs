using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using static InGameDefEditor.WindowUtil;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	class IngestiblePropertiesWidget : IInputWidget
	{
		public readonly IngestibleProperties Parent;
		private readonly List<IInputWidget> inputWidgets;
		private readonly List<FoodTypeFlags> foodTypeSortedFlags;

		private readonly PlusMinusArgs<FoodTypeFlags> foodTypePlusMinus;

		// TODO ingestHoldOffsetStanding

		public string DisplayLabel => "Ingestible Properties";

		public IngestiblePropertiesWidget(IngestibleProperties props)
		{
			this.Parent = props;
			this.inputWidgets = new List<IInputWidget>()
			{
				new IntInputWidget<IngestibleProperties>(this.Parent, "Max Number Ingest At Once", p => p.maxNumToIngestAtOnce, (p, v) => p.maxNumToIngestAtOnce = v),
				new IntInputWidget<IngestibleProperties>(this.Parent, "Base Ingest Ticks", p => p.baseIngestTicks, (p, v) => p.baseIngestTicks = v),
				new FloatInputWidget<IngestibleProperties>(this.Parent, "Chair Search Radius", p => p.chairSearchRadius, (p, v) => p.chairSearchRadius = v),
				new BoolInputWidget<IngestibleProperties>(this.Parent, "Use Eating Speed Stat", p => p.useEatingSpeedStat, (p, v) => p.useEatingSpeedStat = v),
				new BoolInputWidget<IngestibleProperties>(this.Parent, "Ingest Hold Uses Table", p => p.ingestHoldUsesTable, (p, v) => p.ingestHoldUsesTable = v),
				new FloatInputWidget<IngestibleProperties>(this.Parent, "Joy", p => p.joy, (p, v) => p.joy = v),
				new EnumInputWidget<IngestibleProperties, FoodPreferability>(this.Parent, "Preferability", 200, p => p.preferability, (p, v) => p.preferability = v),
				new BoolInputWidget<IngestibleProperties>(this.Parent, "Nurseable", p => p.nurseable, (p, v) => p.nurseable = v),
				new FloatInputWidget<IngestibleProperties>(this.Parent, "Optimality Offset Humanlikes", p => p.optimalityOffsetHumanlikes, (p, v) => p.optimalityOffsetHumanlikes = v),
				new FloatInputWidget<IngestibleProperties>(this.Parent, "Optimality Offset Feeding Animals", p => p.optimalityOffsetFeedingAnimals, (p, v) => p.optimalityOffsetFeedingAnimals = v),
				new EnumInputWidget<IngestibleProperties, DrugCategory>(this.Parent, "DrugCategory", 200, p => p.drugCategory, (p, v) => p.drugCategory = v),
				//new DefInputWidget<IngestibleProperties, ThingDef>(this.Parent, "Parent", 200, p => p.parent, (p, v) => { p.parent = v; IngestiblePropertiesStats.ResetCache(p); }, true),
				new DefInputWidget<IngestibleProperties, JoyKindDef>(this.Parent, "Joy Kind", 200, p => p.joyKind, (p, v) => p.joyKind = v, true),
				new DefInputWidget<IngestibleProperties, ThingDef>(this.Parent, "Source Def", 200, p => p.sourceDef, (p, v) => p.sourceDef = v, true),
				new DefInputWidget<IngestibleProperties, ThoughtDef>(this.Parent, "Taste Thought", 200, p => p.tasteThought, (p, v) => p.tasteThought = v, true),
				new DefInputWidget<IngestibleProperties, ThoughtDef>(this.Parent, "Special Thought Direct", 200, p => p.specialThoughtDirect, (p, v) => p.specialThoughtDirect = v, true),
				new DefInputWidget<IngestibleProperties, ThoughtDef>(this.Parent, "Special Thought As Ingredient", 200, p => p.specialThoughtAsIngredient, (p, v) => p.specialThoughtAsIngredient = v, true),
				new DefInputWidget<IngestibleProperties, EffecterDef>(this.Parent, "Ingest Effect", 200, p => p.ingestEffect, (p, v) => p.ingestEffect = v, true),
				new DefInputWidget<IngestibleProperties, EffecterDef>(this.Parent, "Ingest Effect Eat", 200, p => p.ingestEffectEat, (p, v) => p.ingestEffectEat = v, true),
				new DefInputWidget<IngestibleProperties, SoundDef>(this.Parent, "Sound Def", 200, p => p.ingestSound, (p, v) => p.ingestSound = v, true),
			};
			
			var dic = new SortedDictionary<string, FoodTypeFlags>();
			foreach (var v in Enum.GetValues(typeof(FoodTypeFlags)).Cast<FoodTypeFlags>())
					dic.Add(v.ToString(), v);
			this.foodTypeSortedFlags = new List<FoodTypeFlags>(dic.Values);
			dic.Clear();
			dic = null;

			this.foodTypePlusMinus = new PlusMinusArgs<FoodTypeFlags>()
			{
				allItems = this.foodTypeSortedFlags,
				isBeingUsed = v => (this.Parent.foodType & v) == v,
				onAdd = v => this.Parent.foodType |= v,
				onRemove = v => this.Parent.foodType &= ~v,
				getDisplayName = v => v.ToString()
			};
		}

		public void Draw(float x, ref float y, float width)
		{
			foreach (var v in this.inputWidgets)
				v.Draw(x, ref y, width);

			WindowUtil.PlusMinusLabel(x, ref y, width, "Food Type Flags", this.foodTypePlusMinus);
			WindowUtil.DrawFlagList(x, ref y, width, this.foodTypeSortedFlags, (int)this.Parent.foodType, v => v == FoodTypeFlags.None);
		}

		public void ResetBuffers()
		{
			this.inputWidgets?.ForEach(v => v.ResetBuffers());
		}
	}
}
