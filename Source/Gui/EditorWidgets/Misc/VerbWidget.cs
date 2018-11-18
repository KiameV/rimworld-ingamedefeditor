using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
    class VerbWidget : IDefEditorWidget
    {
        public readonly VerbProperties VerbProperties;

        private readonly List<IInputWidget> inputWidgets;

        private ProjectileDefWidget projectileWidget = null;

        public VerbWidget(VerbProperties p)
        {
            this.VerbProperties = p;

            if (VerbProperties.defaultProjectile != null)
                this.projectileWidget = new ProjectileDefWidget(VerbProperties.defaultProjectile);

            this.inputWidgets = new List<IInputWidget>()
            {
                new FloatInputWidget<VerbProperties>(this.VerbProperties, "Warmup Time", (VerbProperties parent) => parent.warmupTime, (VerbProperties parent, float f) => parent.warmupTime = f),
                new FloatInputWidget<VerbProperties>(this.VerbProperties, "Range", (VerbProperties parent) => parent.range, (VerbProperties parent, float f) => parent.range = f),
                new IntInputWidget<VerbProperties>(this.VerbProperties, "Time Between Shots", (VerbProperties parent) => parent.ticksBetweenBurstShots, (VerbProperties parent, int i) => parent.ticksBetweenBurstShots = i),
                new IntInputWidget<VerbProperties>(this.VerbProperties, "Burst Shot Count", (VerbProperties parent) => parent.burstShotCount, (VerbProperties parent, int i) => parent.burstShotCount = i),
                new FloatInputWidget<VerbProperties>(this.VerbProperties, "Muzzle Flash Scale", (VerbProperties parent) => parent.muzzleFlashScale, (VerbProperties parent, float f) => parent.muzzleFlashScale = f),
                new FloatInputWidget<VerbProperties>(this.VerbProperties, "(AI) Avoid Friendly Fire Radius", (VerbProperties parent) => parent.ai_AvoidFriendlyFireRadius, (VerbProperties parent, float f) => parent.ai_AvoidFriendlyFireRadius = f)
            };

            this.ResetBuffers();
        }

        public string DisplayLabel => VerbProperties.label;

        public void Draw(float x, ref float y, float width)
        {
            WindowUtil.DrawLabel(x, y, 300, this.DisplayLabel, true);
            y += 40;

            x += 10;
            foreach (var w in this.inputWidgets)
                w.Draw(x, ref y, width);

            WindowUtil.DrawInput(x, ref y, width, "InGameDefEditor.SoundCast".Translate(), 100, (VerbProperties.soundCast != null) ? VerbProperties.soundCast.defName : "<none>",
                new WindowUtil.FloatOptionsArgs<SoundDef>()
                {
                    items = DefLookupUtil.GetSortedDefs(DefDatabase<SoundDef>.AllDefsListForReading),
                    getDisplayName = delegate (SoundDef d) { return d.defName; },
                    onSelect = delegate (SoundDef d) { VerbProperties.soundCast = d; },
                    includeNullOption = true
                });
            WindowUtil.DrawInput(x, ref y, width, "InGameDefEditor.SoundCastTail".Translate(), 100, (VerbProperties.soundCastTail != null) ? VerbProperties.soundCastTail.defName : "<none>",
                new WindowUtil.FloatOptionsArgs<SoundDef>()
                {
                    items = DefLookupUtil.GetSortedDefs(DefDatabase<SoundDef>.AllDefsListForReading),
                    getDisplayName = delegate (SoundDef d) { return d.defName; },
                    onSelect = delegate (SoundDef d) { VerbProperties.soundCastTail = d; },
                    includeNullOption = true
                });

            y += 30;

            if (VerbProperties.defaultProjectile != null)
            {
                WindowUtil.DrawInput(x, ref y, width, "InGameDefEditor.Projectiles".Translate(), 100, this.VerbProperties.defaultProjectile.label, 
                    new WindowUtil.FloatOptionsArgs<ThingDef>()
                    {
                        items = Defs.ProjectileDefs.Values,
                        getDisplayName = delegate (ThingDef d) { return d.label; },
                        onSelect = delegate (ThingDef d)
                        {
                            this.VerbProperties.defaultProjectile = d;
                            this.projectileWidget = new ProjectileDefWidget(d);
                        }
                    }, true);
                y += 10;

                x += 10;
                this.projectileWidget.Draw(x, ref y, width - x);
            }
        }

        public void ResetBuffers()
        {
            foreach (var w in this.inputWidgets)
                w.ResetBuffers();

            if (this.projectileWidget != null)
                this.projectileWidget.ResetBuffers();
        }
    }
}
