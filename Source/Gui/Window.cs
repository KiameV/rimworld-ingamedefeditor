using UnityEngine;
using Verse;
using InGameDefEditor.Gui.EditorWidgets;
using RimWorld;
using System.Collections.Generic;
using InGameDefEditor.Gui.EditorWidgets.Misc;
using System;

namespace InGameDefEditor
{
    class InGameDefEditorWindow : Window
	{
		private IEditableDefType selectedDefType = null;
		private IParentStatWidget selectedDef = null;

        private Vector2 leftScroll = Vector2.zero,
                        middleScroll = Vector2.zero,
                        rightScroll = Vector2.zero;

        private float previousYMaxLeft = 0,
                      previousYMaxMiddle = 0,
                      previousYMaxRight = 0;
		
		public override Vector2 InitialSize => new Vector2((float)UI.screenWidth - 20, (float)UI.screenHeight - 20);

        public InGameDefEditorWindow()
        {
            IOUtil.LoadData();
        }

		public override void DoWindowContents(Rect rect)
        {
            Text.Font = GameFont.Small;
            float outerY = 10;

			if (Widgets.ButtonText(new Rect(10, outerY, 200, 30), ((this.selectedDefType == null) ? "Def Type" : this.selectedDefType.Label)))
			{
				WindowUtil.DrawFloatingOptions(
                        new WindowUtil.FloatOptionsArgs<IEditableDefType>()
                        {
                            items = this.GetDefTypes(true),
                            getDisplayName = dt => dt.Label,
							onSelect = dt =>
							{
								this.selectedDefType = dt;
								this.selectedDef = null;
							}
						});
			}

			if (this.selectedDefType != null)
			{
				if (Widgets.ButtonText(new Rect(220, outerY, 200, 30), ((this.selectedDef == null) ? this.selectedDefType.Label + " Def" : Util.GetDefLabel(this.selectedDef.BaseDef))))
				{
					WindowUtil.DrawFloatingOptions(new WindowUtil.FloatOptionsArgs<Def>()
					{
						items = this.selectedDefType.GetDefs(),
						getDisplayName = def => Util.GetDefLabel(def),
						onSelect = def => CreateSelected(def, this.selectedDefType.Type)
					});
				}
			}
            
            outerY += 60;

            if (this.selectedDef != null)
            {
				float x = 0;
				float y = 0;

                // Left column
                Widgets.BeginScrollView(
                    new Rect(0, outerY, 370, rect.height - outerY - 120),
                    ref this.leftScroll,
                    new Rect(0, 0, 354, this.previousYMaxLeft));

                this.selectedDef.DrawLeft(x, ref y, 354);
                this.previousYMaxLeft = y;

                Widgets.EndScrollView();

                // Middle Column
                Widgets.BeginScrollView(
                    new Rect(380, outerY, 370, rect.height - outerY - 120),
                    ref this.middleScroll,
                    new Rect(0, 0, 354, this.previousYMaxMiddle));
                y = 0;
                this.selectedDef.DrawMiddle(x, ref y, 354);
                this.previousYMaxMiddle = y;
                Widgets.EndScrollView();

                // Right Column
                Widgets.BeginScrollView(
                    new Rect(760, outerY, 370, rect.height - outerY - 120),
                    ref this.rightScroll,
                    new Rect(0, 0, 354, this.previousYMaxRight));
                y = 0;
                this.selectedDef.DrawRight(x, ref y, 354);
                this.previousYMaxRight = y;

                Widgets.EndScrollView();

                if (Widgets.ButtonText(new Rect(rect.xMax - 340, rect.yMax - 32, 100, 32), "Reset".Translate()))
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("Reset " + this.selectedDef.DisplayLabel + "?", delegate { this.ResetSelected(); }));
                }
            }

            if (Widgets.ButtonText(new Rect(100, rect.yMax - 32, 100, 32), "Close".Translate()))
            {
                Find.WindowStack.TryRemove(typeof(InGameDefEditorWindow), true);
            }

			if (this.selectedDef != null &&
				Widgets.ButtonText(new Rect(rect.xMax - 230, rect.yMax - 32, 120, 32), "Reset".Translate() + " " + this.selectedDef.Type))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("Reset".Translate() + " " + this.selectedDef.Type + "?", () => { this.selectedDefType.ResetTypeDefs(); this.selectedDef?.ResetBuffers(); }));
			}

            if (Widgets.ButtonText(new Rect(rect.xMax - 100, rect.yMax - 32, 100, 32), "InGameDefEditor.ResetAll".Translate()))
            {
                Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                    "Reset everything to the original game's settings?",
                    delegate
                    {
						foreach (var v in this.GetDefTypes(false))
							v.ResetTypeDefs();
						if (this.selectedDef != null)
							this.selectedDef.Rebuild();
					}));
            }
        }

		private void ResetSelected()
		{
			if (this.selectedDef != null)
			{
				Backup.ApplyStats(this.selectedDef.BaseDef);
				this.selectedDef.Rebuild();
			}
		}

		public void ResetScrolls()
        {
            this.leftScroll = this.middleScroll = this.rightScroll = Vector2.zero;
        }

        public override void PostClose()
        {
            base.PostClose();
            IOUtil.SaveData();
        }

        private void CreateSelected(Def d, DefType type)
        {
            switch (type)
            {
                case DefType.Apparel:
                case DefType.Weapon:
				case DefType.Mineable:
				case DefType.Ingestible:
				case DefType.Disabled:
					this.selectedDef = new ThingDefWidget(d as ThingDef, type);
                    break;
				case DefType.Projectile:
					this.selectedDef = new ProjectileDefWidget(d as ThingDef, type);
					break;
				case DefType.Biome:
                    this.selectedDef = new BiomeWidget(d as BiomeDef, type);
                    break;
				case DefType.Recipe:
					this.selectedDef = new RecipeWidget(d as RecipeDef, type);
					break;
				case DefType.Trait:
					this.selectedDef = new TraitWidget(d as TraitDef, type);
					break;
				case DefType.Thought:
					this.selectedDef = new ThoughtDefWidget(d as ThoughtDef, type);
					break;
				case DefType.StoryTeller:
					this.selectedDef = new StoryTellerDefWidget(d as StorytellerDef, type);
					break;
				case DefType.Difficulty:
					this.selectedDef = new DifficultyDefWidget(d as DifficultyDef, type);
					break;
			}
            this.ResetScrolls();
			IngredientCountWidget.ResetUniqueId();
		}

		private IEnumerable<DifficultyDef> SortDifficultyOptions(SortedDictionary<string, DifficultyDef>.ValueCollection values)
		{
			SortedDictionary<int, List<DifficultyDef>> d = new SortedDictionary<int, List<DifficultyDef>>();
			foreach (var v in values)
			{
				if (!d.TryGetValue(v.difficulty, out List<DifficultyDef> defs))
				{
					defs = new List<DifficultyDef>();
					d[v.difficulty] = defs;
				}
				defs.Add(v);
			}
			List<DifficultyDef> result = new List<DifficultyDef>(values.Count);
			foreach (List<DifficultyDef> defs in d.Values)
				foreach (DifficultyDef def in defs)
					result.Add(def);
			return result;
		}

		#region ButonWidget
		private interface IEditableDefType
		{
			DefType Type { get; }
			string Label { get; }

			IEnumerable<Def> GetDefs();
			void ResetTypeDefs();
		}
		private struct EditableDefType<D> : IEditableDefType where D : Def, new()
		{
			private readonly string label;
			public readonly DefType type;
			public readonly IEnumerable<D> Defs;
			
			public string Label => this.label;
			public DefType Type => this.type;

			public EditableDefType(string label, DefType type, IEnumerable<D> defs)
			{
				this.label = label;
				this.type = type;
				this.Defs = defs;
			}

			public IEnumerable<Def> GetDefs()
			{
				List<Def> l = new List<Def>();
				foreach (Def d in this.Defs)
					l.Add(d);
				return l;
			}

			public void ResetTypeDefs()
			{
				foreach (var v in this.Defs)
					Backup.ApplyStats(v);
			}
		}

		private IEnumerable<IEditableDefType> GetDefTypes(bool includeDisabled)
		{
			List<IEditableDefType> defTypes = new List<IEditableDefType>()
			{
				new EditableDefType<ThingDef>("Apparel", DefType.Apparel, Defs.ApparelDefs.Values),
				new EditableDefType<BiomeDef>("Biomes", DefType.Biome, Defs.BiomeDefs.Values),
				new EditableDefType<DifficultyDef>("Difficulty", DefType.Difficulty, this.SortDifficultyOptions(Defs.DifficultyDefs.Values)),
				new EditableDefType<ThingDef>("Ingestible", DefType.Ingestible, Defs.IngestibleDefs.Values),
				new EditableDefType<ThingDef>("Mineable", DefType.Mineable, Defs.MineableDefs.Values),
				new EditableDefType<ThingDef>("Projectiles", DefType.Projectile, Defs.ProjectileDefs.Values),
				new EditableDefType<RecipeDef>("Recipes", DefType.Recipe, Defs.RecipeDefs.Values),
				new EditableDefType<StorytellerDef>("Story Tellers", DefType.StoryTeller, Defs.StoryTellerDefs.Values),
				new EditableDefType<ThoughtDef>("Thoughts", DefType.Thought, Defs.ThoughtDefs.Values),
				new EditableDefType<TraitDef>("Traits", DefType.Trait, Defs.TraitDefs.Values),
				new EditableDefType<ThingDef>("Weapons", DefType.Weapon, Defs.WeaponDefs.Values),
			};

			if (includeDisabled && Defs.DisabledThingDefs.Count > 0)
				defTypes.Add(new EditableDefType<ThingDef>("Disabled", DefType.Disabled, Defs.DisabledThingDefs.Values));
			return defTypes;
		}

		/*private interface IButtonWidget
        {
			DefType Type { get; }
            void Draw(float x, float y, float width, IParentStatWidget selected);
			void ResetTypeDefs();
        }

        private class ButtonWidget<D> : IButtonWidget where D : Def, new()
        {
            public delegate void OnSelect(Def d, DefType type);
			
			private readonly string label;
            private readonly DefType type;
            private readonly IEnumerable<D> possibleDefs;
            private readonly OnSelect onSelect;

			public DefType Type { get => this.type; }

			public ButtonWidget(string label, DefType type, IEnumerable<D> possibleDefs, OnSelect onSelect)
            {
                this.label = label;
                this.type = type;
                this.possibleDefs = possibleDefs;
                this.onSelect = onSelect;
			}

            public void Draw(float x, float y, float width, IParentStatWidget selected)
            {
                string label = this.label;
                if (selected != null && selected.Type == this.type)
                    label = selected.DisplayLabel;

                if (Widgets.ButtonText(new Rect(x, y, width, 30), label))
                {
                    WindowUtil.DrawFloatingOptions(
                        new WindowUtil.FloatOptionsArgs<D>()
                        {
                            items = possibleDefs,
                            getDisplayName = def => Util.GetDefLabel(def),
                            onSelect = def => this.onSelect(def, this.type)
                        });
                }
            }

			public void ResetTypeDefs()
			{
				foreach (var v in this.possibleDefs)
					Backup.ApplyStats(v);
			}
		}*/
		#endregion
	}
}