using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
    class VerbWidget : IDefEditorWidget
    {
        public readonly VerbProperties VerbProperties;

        private string[] buffer = new string[6];
        private ProjectileDefWidget projectileWidget = null;

        public VerbWidget(VerbProperties p)
        {
            this.VerbProperties = p;
            if (p.defaultProjectile != null)
                this.projectileWidget = new ProjectileDefWidget(p.defaultProjectile);

            this.ResetBuffers();
        }

        public string DisplayLabel => VerbProperties.label;

        public void Draw(float x, ref float y, float width)
        {
            WindowUtil.DrawLabel(x, y, 300, this.DisplayLabel, true);
            y += 40;

            x += 10;
            this.buffer[0] = WindowUtil.DrawInput(x, ref y, "Warnup Time", ref VerbProperties.warmupTime, this.buffer[0]);
            this.buffer[1] = WindowUtil.DrawInput(x, ref y, "Range", ref VerbProperties.range, this.buffer[1]);
            this.buffer[2] = WindowUtil.DrawInput(x, ref y, "Time Between Shots", ref VerbProperties.ticksBetweenBurstShots, this.buffer[2]);
            this.buffer[3] = WindowUtil.DrawInput(x, ref y, "Burst Shot Count", ref VerbProperties.burstShotCount, this.buffer[3]);
            this.buffer[4] = WindowUtil.DrawInput(x, ref y, "Muzzle Flash Scale", ref VerbProperties.muzzleFlashScale, this.buffer[4]);
            this.buffer[5] = WindowUtil.DrawInput(x, ref y, "(AI) Avoid Friendly Fire Radius", ref VerbProperties.ai_AvoidFriendlyFireRadius, this.buffer[5]);
            WindowUtil.DrawInput(x, ref y, width, "Sound Cast", 100, (VerbProperties.soundCast != null) ? VerbProperties.soundCast.defName : "<none>",
                new WindowUtil.DrawFloatOptionsArgs<SoundDef>()
                {
                    items = DefLookupUtil.GetSortedDefs(DefDatabase<SoundDef>.AllDefsListForReading),
                    getDisplayName = delegate (SoundDef d) { return d.defName; },
                    onSelect = delegate (SoundDef d) { VerbProperties.soundCast = d; },
                    includeNullOption = true
                });
            WindowUtil.DrawInput(x, ref y, width, "Sound Cast Tail", 100, (VerbProperties.soundCastTail != null) ? VerbProperties.soundCastTail.defName : "<none>",
                new WindowUtil.DrawFloatOptionsArgs<SoundDef>()
                {
                    items = DefLookupUtil.GetSortedDefs(DefDatabase<SoundDef>.AllDefsListForReading),
                    getDisplayName = delegate (SoundDef d) { return d.defName; },
                    onSelect = delegate (SoundDef d) { VerbProperties.soundCastTail = d; },
                    includeNullOption = true
                });

            y += 30;

            if (VerbProperties.defaultProjectile != null)
            {
                WindowUtil.DrawInput(x, ref y, width, "Projectile", 100, this.VerbProperties.defaultProjectile.label, 
                    new WindowUtil.DrawFloatOptionsArgs<ThingDef>()
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
            this.buffer[0] = VerbProperties.warmupTime.ToString();
            this.buffer[1] = VerbProperties.range.ToString();
            this.buffer[2] = VerbProperties.ticksBetweenBurstShots.ToString();
            this.buffer[3] = VerbProperties.burstShotCount.ToString();
            this.buffer[4] = VerbProperties.muzzleFlashScale.ToString();
            this.buffer[5] = VerbProperties.ai_AvoidFriendlyFireRadius.ToString();

            this.projectileWidget.ResetBuffers();
        }
    }
}
