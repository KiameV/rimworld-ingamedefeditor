using UnityEngine;
using Verse;
using InGameDefEditor.Gui.EditorWidgets;

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

        public override Vector2 InitialSize => new Vector2((float)UI.screenWidth - 20, (float)UI.screenHeight - 20);

        public InGameDefEditorWindow()
        {
            Equipment.LoadData();
        }

        public override void DoWindowContents(Rect rect)
        {
            Text.Font = GameFont.Small;
            float outerY = 0;

            float x = 10;
            this.DrawApparelButton(new Rect(x, outerY, 150, 30));
            x += 160;
            this.DrawWeaponButton(new Rect(x, outerY, 150, 30));
            x += 160;
            this.DrawProjectileButton(new Rect(x, outerY, 150, 30));

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
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("Reset " + this.selected.Label + "?", delegate { this.ResetSelected(); }));
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
                        foreach (ThingDef d in Equipment.ApparelDefs.Values)
                        {
                            Backup.ApplyStats(d);
                        }

                        foreach (ThingDef d in Equipment.WeaponDefs.Values)
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
            if (this.selected is ThingDefWidget)
            {
                Backup.ApplyStats(((ThingDefWidget)this.selected).ThingDef);
            }
            this.selected.Rebuild();
        }

        private void DrawProjectileButton(Rect rect)
        {
            string label = null;
            if (this.selected != null && this.selected is ThingDefWidget)
            {
                ThingDef d = ((ThingDefWidget)this.selected).ThingDef;
                if (Equipment.ProjectileDefs.ContainsKey(d.label))
                    label = d.label;
            }
            if (label == null)
                label = "InGameDefEditor.Projectiles".Translate();

            if (Widgets.ButtonText(rect, label))
            {
                WindowUtil.DrawFloatingOptions(Equipment.ProjectileDefs.Values,
                    delegate (ThingDef d) { return d.label; },
                    delegate (ThingDef d)
                    {
                        this.selected = new ThingDefWidget(d, WidgetType.Projectile);
                        this.ResetScrolls();
                    });
            }
        }

        private void DrawWeaponButton(Rect rect)
        {
            string label = null;
            if (this.selected != null && this.selected is ThingDefWidget)
            {
                ThingDef d = ((ThingDefWidget)this.selected).ThingDef;
                if (d.IsWeapon)
                    label = d.label;
            }
            if (label == null)
                label = "InGameDefEditor.Weapons".Translate();

            if (Widgets.ButtonText(rect, label))
            {
                WindowUtil.DrawFloatingOptions(Equipment.WeaponDefs.Values,
                    delegate (ThingDef d) { return d.label; },
                    delegate (ThingDef d)
                    {
                        this.selected = new ThingDefWidget(d, WidgetType.Weapon);
                        this.ResetScrolls();
                    });
            }
        }

        private void DrawApparelButton(Rect rect)
        {
            string label = null;
            if (this.selected != null && this.selected is ThingDefWidget)
            {
                ThingDef d = ((ThingDefWidget)this.selected).ThingDef;
                if (d.IsApparel)
                    label = d.label;
            }
            if (label == null)
                label = "Apparel".Translate();

            if (Widgets.ButtonText(rect, label))
            {
                WindowUtil.DrawFloatingOptions(Equipment.ApparelDefs.Values,
                    delegate (ThingDef d) { return d.label; },
                    delegate (ThingDef d)
                    {
                        this.selected = new ThingDefWidget(d, WidgetType.Apparel);
                        this.ResetScrolls();
                    });
            }
        }

        public void ResetScrolls()
        {
            this.leftScroll = this.middleScroll = this.rightScroll = Vector2.zero;
        }

        public override void PostClose()
        {
            base.PostClose();
            Equipment.SaveData();
        }
    }
}
