using UnityEngine;
using Verse;
using InGameDefEditor.Gui.EditorWidgets;
using RimWorld;
using System.Collections.Generic;

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
                new ButtonWidget<ThingDef>("Apparel", WidgetType.Apparel, Defs.ApparelDefs.Values, this.CreateSelected),
                new ButtonWidget<ThingDef>("Weapons", WidgetType.Weapon, Defs.WeaponDefs.Values, this.CreateSelected),
                new ButtonWidget<ThingDef>("Projectiles", WidgetType.Projectile, Defs.ProjectileDefs.Values, this.CreateSelected),
                new ButtonWidget<BiomeDef>("Biomes", WidgetType.Biome, Defs.BiomeDefs.Values, this.CreateSelected)
            };
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

                if (Widgets.ButtonText(new Rect(30, rect.yMax - 100, 100, 32), "Reset".Translate()))
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("Reset " + this.selected.DisplayLabel + "?", delegate { this.ResetSelected(); }));
                }
            }

            if (Widgets.ButtonText(new Rect(100, rect.yMax - 32, 100, 32), "Close".Translate()))
            {
                Find.WindowStack.TryRemove(typeof(InGameDefEditorWindow), true);
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - 100, rect.yMax - 32, 100, 32), "InGameDefEditor.ResetAll".Translate()))
            {
                Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                    "Reset everything to the original game's settings?",
                    delegate
                    {
                        foreach (ThingDef d in Defs.ApparelDefs.Values)
                        {
                            Backup.ApplyStats(d);
                        }

                        foreach (ThingDef d in Defs.WeaponDefs.Values)
                        {
                            Backup.ApplyStats(d);
                        }

                        foreach (ThingDef d in Defs.ProjectileDefs.Values)
                        {
                            Backup.ApplyStats(d);
                        }

                        foreach (BiomeDef d in Defs.BiomeDefs.Values)
                        {
                            Backup.ApplyStats(d);
                        }

                        if (this.selected != null)
                            this.selected.Rebuild();
                    }));
            }
        }

        private void ResetSelected()
        {
            Backup.ApplyStats(this.selected.BaseDef);
            this.selected.Rebuild();
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

        private void CreateSelected(Def d, WidgetType type)
        {
            switch (type)
            {
                case WidgetType.Apparel:
                case WidgetType.Projectile:
                case WidgetType.Weapon:
                    this.selected = new ThingDefWidget(d as ThingDef, type);
                    break;
                case WidgetType.Biome:
                    this.selected = new BiomeWidget(d as BiomeDef, type);
                    break;
            }
            this.ResetScrolls();
        }

        #region ButonWidget
        private interface IButtonWidget
        {
            void Draw(float x, float y, float width, IParentStatWidget selected);
        }

        private class ButtonWidget<D> : IButtonWidget where D : Def
        {
            public delegate void OnSelect(Def d, WidgetType type);

            private readonly string label;
            private readonly WidgetType type;
            private readonly IEnumerable<D> possibleDefs;
            private readonly OnSelect onSelect;

            public ButtonWidget(string label, WidgetType type, IEnumerable<D> possibleDefs, OnSelect onSelect)
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
                        new WindowUtil.DrawFloatOptionsArgs<D>()
                        {
                            items = possibleDefs,
                            getDisplayName = delegate (D d) { return d.label; },
                            onSelect = delegate (D d)
                            {
                                this.onSelect(d, this.type);
                            }
                        });
                }
            }
        }
        #endregion
    }
}