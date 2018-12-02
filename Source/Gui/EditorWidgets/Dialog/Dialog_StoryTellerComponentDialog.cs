using System;
using InGameDefEditor.Gui.EditorWidgets.Misc;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using UnityEngine;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Dialog
{
	public class Dialog_StoryTellerComponentDialog : Window
	{
		public delegate AcceptanceReport IsValid(StorytellerCompProperties p);

		private readonly IsValid isValid;

		public StorytellerCompProperties comp;

		private EnumInputWidget<Dialog_StoryTellerComponentDialog, StoryTellerCompPropertyTypes> compInput;
		private DefInputWidget<StorytellerCompProperties, IncidentCategoryDef> categoryInput;
		private DefInputWidget<StorytellerCompProperties, IncidentDef> incidentInput;

		public override Vector2 InitialSize => new Vector2(300f, 350f);

		public Dialog_StoryTellerComponentDialog(IsValid isValid)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = true;

			this.isValid = isValid;
			this.compInput = new EnumInputWidget<Dialog_StoryTellerComponentDialog, StoryTellerCompPropertyTypes>(
				this, "Component", 100, d =>
				{
					if (d.comp != null)
						return StorytellerCompPropertiesStats.GetType(d.comp.compClass.Name);
					return StoryTellerCompPropertyTypes.None;
				}, (d, v) =>
				{
					this.comp = StorytellerCompPropertiesStats.CreateStorytellerCompProperties(v);
					if (this.comp != null)
					{
						this.categoryInput = null;
						this.incidentInput = null;
						if (StorytellerCompPropertiesStats.HasCategory(d.comp))
							CreateCategoryInput();
						if (StorytellerCompPropertiesStats.HasIncident(d.comp))
							CreateIncidentInput();
					}
				});
		}

		private void CreateCategoryInput()
		{
			this.categoryInput = new DefInputWidget<StorytellerCompProperties, IncidentCategoryDef>(this.comp, "Category", 100, c => StorytellerCompPropertiesStats.GetCategory(c), (c, v) => StorytellerCompPropertiesStats.SetCategory(c, v), true);
		}

		private void CreateIncidentInput()
		{
			this.incidentInput = new DefInputWidget<StorytellerCompProperties, IncidentDef>(this.comp, "Incident", 100, c => StorytellerCompPropertiesStats.GetIncident(c), (c, v) => StorytellerCompPropertiesStats.SetIncident(c, v), true);
		}

		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;

			float y = 15;
			this.compInput.Draw(0, ref y, inRect.width);
			this.categoryInput?.Draw(20, ref y, inRect.width - 20);
			this.incidentInput?.Draw(20, ref y, inRect.width - 20);

			if (Widgets.ButtonText(new Rect(15f, inRect.height - 50f, 75, 35f), "OK".Translate(), true, false, true))
			{
				AcceptanceReport acceptanceReport = this.isValid(this.comp);
				if (!acceptanceReport.Accepted)
				{
					if (acceptanceReport.Reason.NullOrEmpty())
					{
						Messages.Message("Invalid Input", MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
					}
				}
				else
				{
					Find.WindowStack.TryRemove(this, true);
				}
			}
			if (Widgets.ButtonText(new Rect(100, inRect.height - 50, 75, 35), "Close".Translate()))
				Find.WindowStack.TryRemove(this, true);
		}
	}
}
