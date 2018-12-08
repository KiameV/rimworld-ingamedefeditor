using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Gui.EditorWidgets.Misc
{
    class VerbWidget : IInputWidget
	{
        public readonly VerbProperties VerbProperties;

        private readonly List<IInputWidget> inputWidgets;

        private ProjectileDefWidget projectileWidget = null;

        public VerbWidget(VerbProperties p)
        {
            this.VerbProperties = p;

            if (VerbProperties.defaultProjectile != null)
                this.projectileWidget = new ProjectileDefWidget(VerbProperties.defaultProjectile, DefType.Projectile);
			
            this.inputWidgets = new List<IInputWidget>()
            {
                new FloatInputWidget<VerbProperties>(this.VerbProperties, "Warmup Time", (VerbProperties parent) => parent.warmupTime, (VerbProperties parent, float f) => parent.warmupTime = f),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Default Cooldown", (VerbProperties parent) => parent.defaultCooldownTime, (VerbProperties parent, float f) => parent.defaultCooldownTime = f),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Min Range", (VerbProperties parent) => parent.minRange, (VerbProperties parent, float f) => parent.minRange = f),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Range", (VerbProperties parent) => parent.range, (VerbProperties parent, float f) => parent.range = f),
                new IntInputWidget<VerbProperties>(this.VerbProperties, "Time Between Shots", (VerbProperties parent) => parent.ticksBetweenBurstShots, (VerbProperties parent, int i) => parent.ticksBetweenBurstShots = i),
                new IntInputWidget<VerbProperties>(this.VerbProperties, "Burst Shot Count", (VerbProperties parent) => parent.burstShotCount, (VerbProperties parent, int i) => parent.burstShotCount = i),
				new BoolInputWidget<VerbProperties>(this.VerbProperties, "Stop Burst Lose LOS", (VerbProperties parent) => parent.stopBurstWithoutLos, (VerbProperties parent, bool b) => parent.stopBurstWithoutLos = b),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Muzzle Flash Scale", (VerbProperties parent) => parent.muzzleFlashScale, (VerbProperties parent, float f) => parent.muzzleFlashScale = f),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Noise Radius", (VerbProperties parent) => parent.noiseRadius, (VerbProperties parent, float f) => parent.noiseRadius = f),
				new BoolInputWidget<VerbProperties>(this.VerbProperties, "Standard Command", (VerbProperties parent) => parent.hasStandardCommand, (VerbProperties parent, bool b) => parent.hasStandardCommand = b),
				new BoolInputWidget<VerbProperties>(this.VerbProperties, "Require LOS", (VerbProperties parent) => parent.requireLineOfSight, (VerbProperties parent, bool b) => parent.requireLineOfSight = b),
				new BoolInputWidget<VerbProperties>(this.VerbProperties, "Force Normal Speed", (VerbProperties parent) => parent.forceNormalTimeSpeed, (VerbProperties parent, bool b) => parent.forceNormalTimeSpeed = b),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Consume Fuel", (VerbProperties parent) => parent.consumeFuelPerShot, (VerbProperties parent, float f) => parent.consumeFuelPerShot = f),
				new IntInputWidget<VerbProperties>(this.VerbProperties, "Base Melee Dmg", (VerbProperties parent) => parent.meleeDamageBaseAmount, (VerbProperties parent, int i) => parent.meleeDamageBaseAmount = i),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Base Armor Penetration", (VerbProperties parent) => parent.meleeArmorPenetrationBase, (VerbProperties parent, float f) => parent.meleeArmorPenetrationBase = f),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Forced Miss Radius", (VerbProperties parent) => parent.forcedMissRadius, (VerbProperties parent, float f) => parent.forcedMissRadius = f),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Accuracy Touch", (VerbProperties parent) => parent.accuracyTouch, (VerbProperties parent, float f) => parent.accuracyTouch = f),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Accuracy Short", (VerbProperties parent) => parent.accuracyShort, (VerbProperties parent, float f) => parent.accuracyShort = f),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Accuracy Medium", (VerbProperties parent) => parent.accuracyMedium, (VerbProperties parent, float f) => parent.accuracyMedium = f),
				new FloatInputWidget<VerbProperties>(this.VerbProperties, "Accuracy Long", (VerbProperties parent) => parent.accuracyLong, (VerbProperties parent, float f) => parent.accuracyLong = f),
				new BoolInputWidget<VerbProperties>(this.VerbProperties, "(AI) Is Weapon", (VerbProperties parent) => parent.ai_IsWeapon, (VerbProperties parent, bool b) => parent.ai_IsWeapon = b),
				new BoolInputWidget<VerbProperties>(this.VerbProperties, "(AI) Is Building Destroyer", (VerbProperties parent) => parent.ai_IsBuildingDestroyer, (VerbProperties parent, bool b) => parent.ai_IsBuildingDestroyer = b),
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

            WindowUtil.DrawInput(x, ref y, width, "InGameDefEditor.SoundCast".Translate(), 100, (VerbProperties.soundCast != null) ? VerbProperties.soundCast.defName : "None",
                new WindowUtil.FloatOptionsArgs<SoundDef>()
                {
                    items = DefLookupUtil.GetSortedDefs(DefDatabase<SoundDef>.AllDefsListForReading),
                    getDisplayName = delegate (SoundDef d) { return d.defName; },
                    onSelect = delegate (SoundDef d) { VerbProperties.soundCast = d; },
                    includeNullOption = true
                });
            WindowUtil.DrawInput(x, ref y, width, "InGameDefEditor.SoundCastTail".Translate(), 100, (VerbProperties.soundCastTail != null) ? VerbProperties.soundCastTail.defName : "None",
                new WindowUtil.FloatOptionsArgs<SoundDef>()
                {
                    items = DefLookupUtil.GetSortedDefs(DefDatabase<SoundDef>.AllDefsListForReading),
                    getDisplayName = delegate (SoundDef d) { return d.defName; },
                    onSelect = delegate (SoundDef d) { VerbProperties.soundCastTail = d; },
                    includeNullOption = true
                });

            y += 10;

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
                            this.projectileWidget = new ProjectileDefWidget(d, DefType.Projectile);
                        }
                    }, true);

                x += 20;
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
