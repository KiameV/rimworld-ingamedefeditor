using RimWorld;
using System;
using System.Reflection;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class ProjectileStats
    {
        [XmlIgnore]
        protected ThingDef def;

        [XmlElement(IsNullable = false)]
        public string defName;
        public int damage;
        public float stoppingPower;
        public float armorPenetration;
        public float speed;

        public ThingDef Def => this.def;
        public string DefName => this.def.defName;
        public string Label => this.def.label;

        public ProjectileStats() { }
        public ProjectileStats(ThingDef d)
        {
            this.def = d;
            this.defName = this.def.defName;
            this.damage = GetDamage(d.projectile);
            this.stoppingPower = d.projectile.stoppingPower;
            this.armorPenetration = GetArmorPenetration(d.projectile);
            this.speed = d.projectile.speed;
        }

        public void ApplyStats(ThingDef def)
        {
#if DEBUG
            Log.Error("Applying stats " + def.defName);
            Log.Error(this.ToString());
#endif
            SetDamage(def.projectile, this.damage);
            def.projectile.stoppingPower = this.stoppingPower;
            SetArmorPenetration(def.projectile, this.armorPenetration);
            def.projectile.speed = this.speed;
        }

        public bool Initialize()
        {
            if (this.def == null)
            {
                def = DefDatabase<ThingDef>.AllDefsListForReading.Find(
                    delegate (ThingDef d) { return d.defName.Equals(this.defName); });

                if (this.def == null)
                    Log.Error("Could not load def " + this.defName);
            }

            return this.def != null;
        }

        public override bool Equals(object obj)
        {
#if DEBUG
            Log.Warning("Equals:");
            Log.Warning(this.ToString());
            Log.Warning(obj.ToString());
#endif
            if (obj != null &&
                obj is ProjectileStats p)
            {
                return
                    string.Equals(this.DefName, p.DefName) &&
                    this.damage == p.damage &&
                    this.stoppingPower == p.stoppingPower &&
                    this.armorPenetration == p.armorPenetration &&
                    this.speed == p.speed;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return
                typeof(FloatValueStat<StatDef>).Name + Environment.NewLine +
                "    defName: " + this.defName + Environment.NewLine +
                "    def set: " + ((this.def == null) ? "no" : "yes") + Environment.NewLine +
                "    damage: " + this.damage + Environment.NewLine +
                "    stoppingPower: " + this.stoppingPower + Environment.NewLine +
                "    armorPenetration: " + this.armorPenetration + Environment.NewLine +
                "    speed: " + this.speed;
        }

        public static int GetDamage(ProjectileProperties p)
        {
            FieldInfo fi = typeof(ProjectileProperties).GetField("damageAmountBase", BindingFlags.NonPublic | BindingFlags.Instance);
            return (int)fi.GetValue(p);
        }

        public static void SetDamage(ProjectileProperties p, int value)
        {
            FieldInfo fi = typeof(ProjectileProperties).GetField("damageAmountBase", BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(p, value);
        }

        public static float GetArmorPenetration(ProjectileProperties p)
        {
            FieldInfo fi = typeof(ProjectileProperties).GetField("armorPenetrationBase", BindingFlags.NonPublic | BindingFlags.Instance);
            return (float)fi.GetValue(p);
        }

        public static void SetArmorPenetration(ProjectileProperties p, float value)
        {
            FieldInfo fi = typeof(ProjectileProperties).GetField("armorPenetrationBase", BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(p, value);
        }
    }
}
