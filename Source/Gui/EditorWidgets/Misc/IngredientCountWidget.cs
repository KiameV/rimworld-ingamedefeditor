using InGameDefEditor.Stats.Misc;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	class IngredientCountWidget : IInputWidget
	{
		private static int uniqueCount = 1;
		public static void  ResetUniqueId() { uniqueCount = 1; }
		private long uniqueId;

		public readonly IngredientCount IngredientCount;

		private ThingFilterWidget filter;
		private FloatInputWidget<IngredientCount> count;

		public IngredientCountWidget(IngredientCount ingredientCount)
		{
			this.uniqueId = uniqueCount;
			++uniqueCount;

			this.IngredientCount = ingredientCount;

			this.filter = new ThingFilterWidget(this.IngredientCount.filter);

			this.count = new FloatInputWidget<IngredientCount>(
				this.IngredientCount, "Ingredient Count", (ic) => IngredientCountStats.GetIngredientCount(ic), (ic, f) => IngredientCountStats.SetIngredientCount(ic, f));
		}

		public string DisplayLabel => "Ingredient " + this.uniqueId;

		public void Draw(float x, ref float y, float width)
		{
			this.count.Draw(x, ref y, width);
			this.filter.Draw(x, ref y, width);
		}

		public void ResetBuffers()
		{
			this.count.ResetBuffers();
			this.filter.ResetBuffers();
		}
	}
}
