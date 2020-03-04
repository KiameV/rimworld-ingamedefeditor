using InGameDefEditor.Gui.EditorWidgets.Misc;
using Verse;
using UnityEngine;

namespace InGameDefEditor.Gui.EditorWidgets
{
	interface IInputWidget
	{
		string DisplayLabel { get; }
		void Draw(float x, ref float y, float width);
		void ResetBuffers();
	}

	interface IParentStatWidget
	{
		DefType Type { get; }
		string DisplayLabel { get; }

		void DrawLeft(float x, ref float y, float width);
		void DrawMiddle(float x, ref float y, float width);
		void DrawRight(float x, ref float y, float width);
		void Rebuild();
		void ResetBuffers();
		void ResetParent();
		void DisableAutoDeploy();
	}

	public abstract class AParentDefStatWidget<D> : IParentStatWidget where D : Def, new()
    {
        public readonly D Def;
        private readonly DefType type;

		private BoolInputWidget<D> autoApplySettingsInput;

		protected AParentDefStatWidget(D def, DefType type)
        {
            this.Def = def;
            this.type = type;

			this.autoApplySettingsInput = new BoolInputWidget<D>(
				def, "InGameDefEditor.AutoApplySettings".Translate(),
				d =>
				{
					if (Defs.ApplyStatsAutoThingDefs.TryGetValue(d.defName, out bool b))
						return b;
					return false;
				},
				(d, applyAuto) =>
				{
					if (applyAuto)
						Defs.ApplyStatsAutoThingDefs[d.defName] = applyAuto;
					else
						Defs.ApplyStatsAutoThingDefs.Remove(d.defName);
				});
		}

        public virtual string DisplayLabel => Util.GetDefLabel(this.Def);
        public DefType Type => this.type;

        public Def BaseDef => this.Def;

		public void DisableAutoDeploy()
		{
			Defs.ApplyStatsAutoThingDefs.Remove(this.Def.defName);
		}

		public virtual void DrawLeft(float x, ref float y, float width)
		{
			this.autoApplySettingsInput.ColorOverride = (Defs.ApplyStatsAutoThingDefs.ContainsKey(this.Def.defName)) ? Color.green : Color.red;
			this.autoApplySettingsInput.Draw(x, ref y, width);
		}

        public abstract void DrawMiddle(float x, ref float y, float width);
        public abstract void DrawRight(float x, ref float y, float width);
        public abstract void Rebuild();
        public virtual void ResetBuffers()
		{
			this.autoApplySettingsInput.ResetBuffers();
		}

		public void ResetParent()
		{
			Backup.ApplyStats(this.Def);
		}
    }
}
