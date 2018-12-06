using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	class EffecterDefWidget : IInputWidget
	{
		public EffecterDef EffecterDef;

		public FloatInputWidget<EffecterDef> positionRadius = null;
		public MinMaxInputWidget<EffecterDef> offsetTowardsTarget = null;
		// TODO public List<SubEffecterDef> children;

		public string DisplayLabel => Util.GetDefLabel(this.EffecterDef);

		public EffecterDefWidget(EffecterDef def)
		{
			this.EffecterDef = def;

			this.positionRadius = new FloatInputWidget<EffecterDef>(
				this.EffecterDef, "Position Radius", (d) => d.positionRadius, (d, f) => d.positionRadius = f);
			this.offsetTowardsTarget = new MinMaxInputWidget<EffecterDef>("Offset Towards Target",
				new FloatInputWidget<EffecterDef>(this.EffecterDef, "Min", (d) => d.offsetTowardsTarget.min, (d, f) => d.offsetTowardsTarget.min = f),
				new FloatInputWidget<EffecterDef>(this.EffecterDef, "Max", (d) => d.offsetTowardsTarget.max, (d, f) => d.offsetTowardsTarget.max = f));
		}

		public void Draw(float x, ref float y, float width)
		{
			WindowUtil.DrawLabel(x, y, 300, this.DisplayLabel, true);
			y += 40;

			x += 10;
			this.positionRadius.Draw(x, ref y, width);
			this.offsetTowardsTarget?.Draw(x, ref y, width);
		}

		public void ResetBuffers()
		{
			this.positionRadius.ResetBuffers();
			this.offsetTowardsTarget?.ResetBuffers();
		}
	}
}
