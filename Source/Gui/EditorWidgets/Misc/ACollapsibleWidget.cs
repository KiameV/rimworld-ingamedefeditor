using UnityEngine;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
	abstract class ACollapsibleWidget : IInputWidget
	{
		public bool CanCollapse;
		private bool isCollapsed;

		public abstract string DisplayLabel { get; }

		public ACollapsibleWidget(bool canCollapse = true, bool isCollapsed = false)
		{
			this.CanCollapse = canCollapse;
			this.isCollapsed = isCollapsed;
		}

		public void Draw(float x, ref float y, float width)
		{
			if (this.CanCollapse)
			{
				if (Widgets.ButtonText(new Rect(x, y, 30, 30), ((this.isCollapsed) ? "+" : "-")))
					this.isCollapsed = !this.isCollapsed;
				WindowUtil.DrawLabel(x + 34, ref y, width - 34, this.DisplayLabel, 30, true);
			}
			if (!this.isCollapsed)
				this.DrawInputs(x + 10f, ref y, width - 10f);
		}

		protected abstract void DrawInputs(float x, ref float y, float width);

		public abstract void ResetBuffers();
	}
}
