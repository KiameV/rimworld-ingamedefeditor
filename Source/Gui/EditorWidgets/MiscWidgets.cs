using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets
{
    class StatModifierWidget : IStatWidget
    {
        public readonly StatModifier StatModifier;

        private string buffer = "";

        public StatModifierWidget(StatModifier s)
        {
            this.StatModifier = s;
            this.ResetBuffers();
        }

        public void Draw(float x, ref float y, float width)
        {
            this.buffer = WindowUtil.DrawInput(x, ref y, this.StatModifier.stat.label, ref this.StatModifier.value, this.buffer);
        }

        public void ResetBuffers()
        {
            this.buffer = this.StatModifier.value.ToString();
        }
    }

    class ProjectileWidget : IStatWidget
    {
        public readonly ThingDef ProjectileDef;

        private string[] buffer = new string[4];

        public ProjectileWidget(ThingDef d)
        {
            this.ProjectileDef = d;
            this.ResetBuffers();
        }

        public void Draw(float x, ref float y, float width)
        {
            int i = ProjectileStats.GetDamage(this.ProjectileDef.projectile);
            this.buffer[0] = WindowUtil.DrawInput(x, ref y, "Base Damage", ref i, this.buffer[0]);
            ProjectileStats.SetDamage(this.ProjectileDef.projectile, i);

            this.buffer[1] = WindowUtil.DrawInput(x, ref y, "Stopping Power", ref this.ProjectileDef.projectile.stoppingPower, this.buffer[1]);

            float f = ProjectileStats.GetArmorPenetration(this.ProjectileDef.projectile);
            this.buffer[2] = WindowUtil.DrawInput(x, ref y, "Armor Penetration", ref f, this.buffer[2]);
            ProjectileStats.SetArmorPenetration(this.ProjectileDef.projectile, f);

            this.buffer[3] = WindowUtil.DrawInput(x, ref y, "Speed", ref this.ProjectileDef.projectile.speed, this.buffer[3]);
        }

        public void ResetBuffers()
        {
            this.buffer[0] = ProjectileStats.GetDamage(this.ProjectileDef.projectile).ToString();
            this.buffer[1] = this.ProjectileDef.projectile.stoppingPower.ToString();
            this.buffer[2] = ProjectileStats.GetArmorPenetration(this.ProjectileDef.projectile).ToString();
            this.buffer[3] = this.ProjectileDef.projectile.speed.ToString();
        }
    }

    class VerbWidget : IStatWidget
    {
        public readonly VerbProperties VerbProperties;

        private string[] buffer = new string[6];
        private ProjectileWidget projectileWidget = null;

        public VerbWidget(VerbProperties p)
        {
            this.VerbProperties = p;
            if (p.defaultProjectile != null)
                this.projectileWidget = new ProjectileWidget(p.defaultProjectile);

            this.ResetBuffers();
        }

        public void Draw(float x, ref float y, float width)
        {
            WindowUtil.DrawLabel(x, y, 300, VerbProperties.label, true);
            y += 40;

            x += 10;
            this.buffer[0] = WindowUtil.DrawInput(x, ref y, "Warnup Time", ref VerbProperties.warmupTime, this.buffer[0]);
            this.buffer[1] = WindowUtil.DrawInput(x, ref y, "Range", ref VerbProperties.range, this.buffer[1]);
            this.buffer[2] = WindowUtil.DrawInput(x, ref y, "Time Between Shots", ref VerbProperties.ticksBetweenBurstShots, this.buffer[2]);
            this.buffer[3] = WindowUtil.DrawInput(x, ref y, "Burst Shot Count", ref VerbProperties.burstShotCount, this.buffer[3]);
            this.buffer[4] = WindowUtil.DrawInput(x, ref y, "Muzzle Flash Scale", ref VerbProperties.muzzleFlashScale, this.buffer[4]);
            this.buffer[5] = WindowUtil.DrawInput(x, ref y, "(AI) Avoid Friendly Fire Radius", ref VerbProperties.ai_AvoidFriendlyFireRadius, this.buffer[5]);
            WindowUtil.DrawInput(x, ref y, width, "Sound Cast", 100, (VerbProperties.soundCast != null) ? VerbProperties.soundCast.defName : "<none>",
                delegate
                {
                    WindowUtil.DrawFloatingOptions(
                        DefLookupUtil.GetSortedDefs(DefDatabase<SoundDef>.AllDefsListForReading),
                        delegate (SoundDef d) { return d.defName; },
                        delegate (SoundDef d)
                        {
                            VerbProperties.soundCast = d;
                        }, true);
                });
            WindowUtil.DrawInput(x, ref y, width, "Sound Cast Tail", 100, (VerbProperties.soundCastTail != null) ? VerbProperties.soundCastTail.defName : "<none>",
                delegate
                {
                    WindowUtil.DrawFloatingOptions(
                        DefLookupUtil.GetSortedDefs(DefDatabase<SoundDef>.AllDefsListForReading),
                        delegate (SoundDef d) { return d.defName; },
                        delegate (SoundDef d)
                        {
                            VerbProperties.soundCastTail = d;
                        }, true);
                });

            y += 30;

            if (VerbProperties.defaultProjectile != null)
            {
                WindowUtil.DrawInput(x, ref y, width, "Projectile", 100, this.VerbProperties.defaultProjectile.label, 
                    delegate
                    {
                        WindowUtil.DrawFloatingOptions(
                            Equipment.ProjectileDefs.Values,
                            delegate (ThingDef d) { return d.label; },
                            delegate (ThingDef d)
                            {
                                this.VerbProperties.defaultProjectile = d;
                                this.projectileWidget = new ProjectileWidget(d);
                            });
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

    class ToolWidget : IStatWidget
    {
        public readonly Tool Tool;

        private string[] comBuffer = new string[3];

        public ToolWidget(Tool t)
        {
            this.Tool = t;

            this.ResetBuffers();
        }

        public void Draw(float x, ref float y, float width)
        {
            WindowUtil.DrawLabel(x, y, 300, this.Tool.label);
            y += 40;

            this.comBuffer[0] = WindowUtil.DrawInput(x + 20, ref y, "Power", ref this.Tool.power, this.comBuffer[0]);
            this.comBuffer[1] = WindowUtil.DrawInput(x + 20, ref y, "Armor Penetration", ref this.Tool.armorPenetration, this.comBuffer[1]);
            this.comBuffer[2] = WindowUtil.DrawInput(x + 20, ref y, "Cooldown Time", ref this.Tool.cooldownTime, this.comBuffer[2]);

            x += 20;
            WindowUtil.PlusMinusLabel(
                x, ref y, 100, "Capabilities",
                delegate
                {
                    List<ToolCapacityDef> l = new List<ToolCapacityDef>(DefDatabase<ToolCapacityDef>.AllDefsListForReading);
                    l.RemoveAll((ToolCapacityDef d) => { return this.Tool.capacities.Contains(d); });

                    WindowUtil.DrawFloatingOptions(l,
                        delegate (ToolCapacityDef d) { return d.defName; },
                        delegate (ToolCapacityDef d) { this.Tool.capacities.Add(d); });
                },
                delegate
                {
                    WindowUtil.DrawFloatingOptions(this.Tool.capacities,
                        delegate (ToolCapacityDef d) { return d.defName; },
                        delegate (ToolCapacityDef d) { this.Tool.capacities.Remove(d); });
                });

            x += 10;
            foreach (ToolCapacityDef d in this.Tool.capacities)
            {
                Widgets.Label(new Rect(x, y, 150, 32), d.defName);
                y += 40;
            }
        }

        public void ResetBuffers()
        {
            comBuffer[0] = this.Tool.power.ToString();
            comBuffer[1] = this.Tool.armorPenetration.ToString();
            comBuffer[2] = this.Tool.cooldownTime.ToString();
        }
    }
}