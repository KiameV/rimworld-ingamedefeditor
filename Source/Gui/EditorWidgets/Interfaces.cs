using InGameDefEditor.Gui.EditorWidgets.Misc;
using Verse;
using UnityEngine;
using System.Reflection;
using RimWorld;

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
		object BaseObject { get; }
		DefType Type { get; }
		string DisplayLabel { get; }
		bool IsDisabled { get; }
		void DrawLeft(float x, ref float y, float width);
		void DrawMiddle(float x, ref float y, float width);
		void DrawRight(float x, ref float y, float width);
		void Rebuild();
		void ResetBuffers();
		void ResetParent();
		//void DisableAutoDeploy();
		void DrawStaticButtons(float x, ref float y, float width);
	}

	public abstract class AParentDefStatWidget<D> : IParentStatWidget
	{
		public object BaseObject => this.Def;
		public readonly D Def;
        private readonly DefType type;

		private BoolInputWidget<D> autoApplySettingsInput;
		private BoolInputWidget<D> disableDefInput;
		public bool IsDisabled => Defs.DisabledDefs.Contains(Def);
		protected bool IsAutoApply => Defs.ApplyStatsAutoDefs.Contains(Def);

		protected abstract void AddDefsToAutoApply(bool isAutoApply);

		protected AParentDefStatWidget(D def, DefType type)
        {
            this.Def = def;
            this.type = type;

			this.autoApplySettingsInput = new BoolInputWidget<D>(
				def, "InGameDefEditor.AutoApplySettings".Translate(),
				d => IsAutoApply,
				(d, applyAuto) =>
				{
					if (applyAuto)
					{
						if (!Defs.ApplyStatsAutoDefs.Add(d))
							Log.Warning($"Failed to apply auto load to {Util.GetLabel(d)}");
						else
							AddDefsToAutoApply(true);
					}
					else
					{
						if (!Defs.ApplyStatsAutoDefs.Remove(d))
							Log.Warning($"Failed to remove auto load from {Util.GetLabel(d)}");
						else
							AddDefsToAutoApply(false);
					}
				});
			
			this.disableDefInput = new BoolInputWidget<D>(
				 def, "InGameDefEditor.DisableDef".Translate(),
				 d => IsDisabled,
				 (d, isDisabled) =>
				 {
					 if (isDisabled)
					 {
						 if (!Defs.DisabledDefs.Add(d))
							 Log.Warning($"Failed to disable {this.DisplayLabel}");
						 else
						 {
							 Defs.ApplyStatsAutoDefs.Remove(d);
							 DatabaseUtil.Remove(d);
						 }
					 }
					 else
					 {
						 if (!Defs.DisabledDefs.Remove(d))
							 Log.Warning($"Failed to enable {this.DisplayLabel}");
						 else
							 DatabaseUtil.Add(d);
					 }
				 }, d =>
				 {
					 bool enabled = Current.Game == null || IsDisabled;
					 return new AInputWidget<D, bool>.ShouldDrawInputResult(enabled, (enabled) ? "" : "Cannot disabled defs while a game is not running.");
				 });
		}

        public virtual string DisplayLabel => Util.GetLabel(this.Def);
        public DefType Type => this.type;

		public void DrawStaticButtons(float x, ref float y, float width)
		{
			try
			{
				bool isDisabled = IsDisabled;
				if (IsDisabled)
					GUI.color = Color.red;
				this.disableDefInput.Draw(x, ref y, width);

				if (!IsDisabled)
				{
					if (IsAutoApply)
						GUI.color = Color.green;
					else
						GUI.color = Color.red;
					this.autoApplySettingsInput.Draw(x, ref y, width);
				}
			}
			finally
			{
				GUI.color = Color.white;
			}
		}

		public abstract void DrawLeft(float x, ref float y, float width);
        public abstract void DrawMiddle(float x, ref float y, float width);
        public abstract void DrawRight(float x, ref float y, float width);

		public abstract void Rebuild();

        public virtual void ResetBuffers()
		{
			this.autoApplySettingsInput.ResetBuffers();
			this.disableDefInput.ResetBuffers();
		}

		public void ResetParent()
		{
			Backup.ApplyStats(this.Def);
		}
    }
}
