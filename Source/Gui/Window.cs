using UnityEngine;
using Verse;
using InGameDefEditor.Gui.EditorWidgets;
using RimWorld;
using System.Collections.Generic;
using InGameDefEditor.Gui.EditorWidgets.Misc;
using System;
using System.Linq;
using System.Reflection;

namespace InGameDefEditor
{
    class InGameDefEditorWindow : Window
	{
		private static IEditableDefType selectedDefType = null;
		private static IParentStatWidget selectedDef = null;

        private static Vector2 
			leftScroll = Vector2.zero,
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

			if (Widgets.ButtonText(new Rect(10, outerY, 200, 30), ((selectedDefType == null) ? "Def Type" : selectedDefType.Label)))
			{
				WindowUtil.DrawFloatingOptions(
                        new WindowUtil.FloatOptionsArgs<IEditableDefType>()
                        {
                            items = this.GetDefTypes(true),
                            getDisplayName = dt => dt.Label,
							onSelect = dt =>
							{
								selectedDefType = dt;
								selectedDef = null;
							}
						});
			}

			if (selectedDefType != null)
			{
				if (Widgets.ButtonText(new Rect(220, outerY, 200, 30), ((selectedDef == null) ? selectedDefType.Label + " Def" : selectedDef.DisplayLabel)))
				{
					WindowUtil.DrawFloatingOptions(new WindowUtil.FloatOptionsArgs<object>()
					{
						items = selectedDefType.GetDefs(),
						getDisplayName = i =>
						{
							if (i is Def def)
								return Util.GetDefLabel(def);
							else if (i is Backstory b)
								return b.title;
							return i.ToString();
						},
						onSelect = i =>
						{
							if (i is Def def)
								this.CreateSelected(def, selectedDefType.Type);
							else if (i is Backstory b)
								this.CreateSelected(b, selectedDefType.Type);
						}
					});
				}
			}

			if (Defs.ApplyStatsAutoThingDefs.Keys.Count > 0 && 
				Widgets.ButtonText(new Rect(rect.xMax - 300, outerY, 200, 30), "InGameDefEditor.AutoLoaded".Translate()))
			{
				var l = new List<FloatMenuOption>(Defs.ApplyStatsAutoThingDefs.Keys.Count);
				foreach (string s in Defs.ApplyStatsAutoThingDefs.Keys)
					l.Add(new FloatMenuOption(s, () => { }));
				Find.WindowStack.Add(new FloatMenu(l));
			}

			outerY += 60;

            if (selectedDef != null)
            {
				float x = 0;
				float y = 0;

                // Left column
                Widgets.BeginScrollView(
                    new Rect(0, outerY, 370, rect.height - outerY - 120),
                    ref leftScroll,
                    new Rect(0, 0, 354, this.previousYMaxLeft));

                selectedDef.DrawLeft(x, ref y, 354);
                this.previousYMaxLeft = y;

                Widgets.EndScrollView();

                // Middle Column
                Widgets.BeginScrollView(
                    new Rect(380, outerY, 370, rect.height - outerY - 120),
                    ref middleScroll,
                    new Rect(0, 0, 354, this.previousYMaxMiddle));
                y = 0;
                selectedDef.DrawMiddle(x, ref y, 354);
                this.previousYMaxMiddle = y;
                Widgets.EndScrollView();

                // Right Column
                Widgets.BeginScrollView(
                    new Rect(760, outerY, 370, rect.height - outerY - 120),
                    ref rightScroll,
                    new Rect(0, 0, 354, this.previousYMaxRight));
                y = 0;
                selectedDef.DrawRight(x, ref y, 354);
                this.previousYMaxRight = y;

                Widgets.EndScrollView();
			}

			if (selectedDefType != null && 
				Widgets.ButtonText(new Rect(rect.xMax - 340, rect.yMax - 32, 100, 32), "Reset".Translate()))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("Reset " + selectedDef.DisplayLabel + "?", delegate { this.ResetSelected(); }));
			}

			if (Widgets.ButtonText(new Rect(100, rect.yMax - 32, 100, 32), "Close".Translate()))
            {
                Find.WindowStack.TryRemove(typeof(InGameDefEditorWindow), true);
            }

			if (selectedDef != null &&
				Widgets.ButtonText(new Rect(rect.xMax - 230, rect.yMax - 32, 120, 32), "Reset".Translate() + " " + selectedDef.Type))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("Reset".Translate() + " " + selectedDef.Type + "?", () =>
				{
					selectedDefType.ResetTypeDefs();
					ResetSelected();
				}));
			}

            if (Widgets.ButtonText(new Rect(rect.xMax - 100, rect.yMax - 32, 100, 32), "InGameDefEditor.ResetAll".Translate()))
            {
                Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                    "Reset everything to the original game's settings?",
                    delegate
					{
						Defs.DisabledThingDefs.Clear();
						Defs.ApplyStatsAutoThingDefs.Clear();
						foreach (var v in this.GetDefTypes(false))
							v.ResetTypeDefs();
						if (selectedDef != null)
							ResetSelected();
					}));
            }
        }

		private void ResetSelected()
		{
			if (selectedDef != null)
			{
				selectedDef.DisableAutoDeploy();
				selectedDef.ResetParent();
				selectedDef.Rebuild();
			}
		}

		public void ResetScrolls()
        {
            leftScroll = middleScroll = rightScroll = Vector2.zero;
        }

        public override void PostClose()
        {
            base.PostClose();
            IOUtil.SaveData();
		}

		private void CreateSelected(Backstory b, DefType type)
		{
			selectedDef = new BackstoryWidget(b, type);
			this.ResetScrolls();
			IngredientCountWidget.ResetUniqueId();
		}

		private void CreateSelected(Def d, DefType type)
        {
            switch (type)
            {
                case DefType.Apparel:
				case DefType.Building:
				case DefType.Disabled:
				case DefType.Ingestible:
				case DefType.Mineable:
				case DefType.Weapon:
					selectedDef = new ThingDefWidget(d as ThingDef, type);
                    break;
				case DefType.Projectile:
					selectedDef = new ProjectileDefWidget(d as ThingDef, type);
					break;
				case DefType.Biome:
                    selectedDef = new BiomeWidget(d as BiomeDef, type);
                    break;
				case DefType.Recipe:
					selectedDef = new RecipeWidget(d as RecipeDef, type);
					break;
				case DefType.Trait:
					selectedDef = new TraitWidget(d as TraitDef, type);
					break;
				case DefType.Thought:
					selectedDef = new ThoughtDefWidget(d as ThoughtDef, type);
					break;
				case DefType.StoryTeller:
					selectedDef = new StoryTellerDefWidget(d as StorytellerDef, type);
					break;
				case DefType.Difficulty:
					selectedDef = new DifficultyDefWidget(d as DifficultyDef, type);
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

			IEnumerable<object> GetDefs();
			void ResetTypeDefs();
		}

		private struct EditableBackstoryType : IEditableDefType
		{
			private readonly string label;
			public readonly DefType type;
			public readonly IEnumerable<Backstory> items;

			public string Label => this.label;
			public DefType Type => this.type;

			public EditableBackstoryType(string label, DefType type, IEnumerable<Backstory> items)
			{
				this.label = label;
				this.type = type;
				this.items = items;
			}

			public IEnumerable<object> GetDefs()
			{
				List<object> l = new List<object>(this.items.Count());
				foreach (var b in this.items)
					l.Add(b);
				return l;
			}

			public void ResetTypeDefs()
			{
				foreach (var v in this.items)
				{
					Backup.ApplyStats(v);
					Defs.ApplyStatsAutoThingDefs.Remove(v.identifier);
					Defs.DisabledThingDefs.Remove(v.identifier);
				}
			}
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

			public IEnumerable<object> GetDefs()
			{
				List<object> l = new List<object>(this.Defs.Count());
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
				new EditableBackstoryType("Backstories", DefType.Backstory, Defs.Backstories.Values),
				new EditableDefType<BiomeDef>("Biomes", DefType.Biome, Defs.BiomeDefs.Values),
				new EditableDefType<ThingDef>("Buildings", DefType.Building, Defs.BuildingDefs.Values),
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