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
		private IParentStatWidget selected = null;

        private Vector2 leftScroll = Vector2.zero,
                        middleScroll = Vector2.zero,
                        rightScroll = Vector2.zero;

        private float previousYMaxLeft = 0,
                      previousYMaxMiddle = 0,
                      previousYMaxRight = 0;

        private readonly List<IButtonWidget> buttons;

        public override Vector2 InitialSize => new Vector2((float)UI.screenWidth - 20, (float)UI.screenHeight - 20);

        public InGameDefEditorWindow()
        {
            IOUtil.LoadData();
            buttons = new List<IButtonWidget>()
            {
                new ButtonWidget<ThingDef>("Apparel", DefType.Apparel, Defs.ApparelDefs.Values, this.CreateSelected),
                new ButtonWidget<ThingDef>("Weapons", DefType.Weapon, Defs.WeaponDefs.Values, this.CreateSelected),
                new ButtonWidget<ThingDef>("Projectiles", DefType.Projectile, Defs.ProjectileDefs.Values, this.CreateSelected),
                new ButtonWidget<BiomeDef>("Biomes", DefType.Biome, Defs.BiomeDefs.Values, this.CreateSelected),
				new ButtonWidget<TraitDef>("Traits", DefType.Trait, Defs.TraitDefs.Values, this.CreateSelected),
				new ButtonWidget<ThoughtDef>("Thoughts", DefType.Thought, Defs.ThoughtDefs.Values, this.CreateSelected),
			};

			if (Controller.EnableRecipes)
				buttons.Add(new ButtonWidget<RecipeDef>("Recipes", DefType.Recipe, Defs.RecipeDefs.Values, this.CreateSelected));
        }

        public override void DoWindowContents(Rect rect)
        {
            Text.Font = GameFont.Small;
            float outerY = 0;

            float x = 10;
            float buttonX = x;
            foreach (var w in this.buttons)
            {
                w.Draw(buttonX, outerY, 150, this.selected);
                buttonX += 160;

                if (buttonX + 150 > rect.xMax)
                {
                    buttonX = 10;
                    outerY += 40;
                }
            }
            
            outerY += 60;

            if (this.selected != null)
            {
                float y = 0;
                x = 0;

                // Left column
                Widgets.BeginScrollView(
                    new Rect(0, outerY, 370, rect.height - outerY - 120),
                    ref this.leftScroll,
                    new Rect(0, 0, 354, this.previousYMaxLeft));

                this.selected.DrawLeft(x, ref y, 354);
                this.previousYMaxLeft = y;

                Widgets.EndScrollView();

                // Middle Column
                Widgets.BeginScrollView(
                    new Rect(380, outerY, 370, rect.height - outerY - 120),
                    ref this.middleScroll,
                    new Rect(0, 0, 354, this.previousYMaxMiddle));
                y = 0;
                this.selected.DrawMiddle(x, ref y, 354);
                this.previousYMaxMiddle = y;
                Widgets.EndScrollView();

                // Right Column
                Widgets.BeginScrollView(
                    new Rect(760, outerY, 370, rect.height - outerY - 120),
                    ref this.rightScroll,
                    new Rect(0, 0, 354, this.previousYMaxRight));
                y = 0;
                this.selected.DrawRight(x, ref y, 354);
                this.previousYMaxRight = y;

                Widgets.EndScrollView();

                if (Widgets.ButtonText(new Rect(rect.xMax - 340, rect.yMax - 32, 100, 32), "Reset".Translate()))
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("Reset " + this.selected.DisplayLabel + "?", delegate { this.ResetSelected(); }));
                }
            }

            if (Widgets.ButtonText(new Rect(100, rect.yMax - 32, 100, 32), "Close".Translate()))
            {
                Find.WindowStack.TryRemove(typeof(InGameDefEditorWindow), true);
            }

			if (this.selected != null &&
				Widgets.ButtonText(new Rect(rect.xMax - 230, rect.yMax - 32, 120, 32), "Reset".Translate() + " " + this.selected.Type))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("Reset".Translate() + " " + this.selected.Type + "?", delegate { this.ResetDefType(this.selected.Type); }));
			}

            if (Widgets.ButtonText(new Rect(rect.xMax - 100, rect.yMax - 32, 100, 32), "InGameDefEditor.ResetAll".Translate()))
            {
                Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                    "Reset everything to the original game's settings?",
                    delegate
                    {
						foreach (var v in this.buttons)
							v.ResetTypeDefs();
						if (this.selected != null)
							this.selected.Rebuild();
					}));
            }
        }

		private void ResetSelected()
		{
			if (this.selected != null)
			{
				Backup.ApplyStats(this.selected.BaseDef);
				this.selected.Rebuild();
			}
		}

		private void ResetDefType(DefType type)
		{
			foreach (var v in this.buttons)
			{
				if (v.Type == type)
				{
					v.ResetTypeDefs();
					if (this.selected != null)
						this.selected.Rebuild();
					break;
				}
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
                    this.selected = new ThingDefWidget(d as ThingDef, type);
                    break;
				case DefType.Projectile:
					this.selected = new ProjectileDefWidget(d as ThingDef, type);
					break;
				case DefType.Biome:
                    this.selected = new BiomeWidget(d as BiomeDef, type);
                    break;
				case DefType.Recipe:
					this.selected = new RecipeWidget(d as RecipeDef, type);
					break;
				case DefType.Trait:
					this.selected = new TraitWidget(d as TraitDef, type);
					break;
				case DefType.Thought:
					this.selected = new ThoughtDefWidget(d as ThoughtDef, type);
					break;
            }
            this.ResetScrolls();
			IngredientCountWidget.ResetUniqueId();
        }

        #region ButonWidget
        private interface IButtonWidget
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
		}
        #endregion
    }
}