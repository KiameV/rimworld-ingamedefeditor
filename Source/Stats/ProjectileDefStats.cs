using InGameDefEditor.Stats.DefStat;
using RimWorld;
using System;
using System.Reflection;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats
{
    public class ProjectileDefStats : DefStat<ThingDef>, IParentStat
    {
        public int damage;
        public float stoppingPower;
        public float armorPenetration;
        public float speed;

        public ProjectileDefStats() { }
        public ProjectileDefStats(ThingDef d) : base(d)
        {
            this.damage = GetDamage(d.projectile);
            this.stoppingPower = d.projectile.stoppingPower;
            this.armorPenetration = GetArmorPenetration(d.projectile);
            this.speed = d.projectile.speed;
        }

        public void ApplyStats(Def to)
        {
            if (to is ThingDef t)
            {
                SetDamage(t.projectile, this.damage);
                t.projectile.stoppingPower = this.stoppingPower;
                SetArmorPenetration(t.projectile, this.armorPenetration);
                t.projectile.speed = this.speed;
            }
            else
                Log.Error("ProjectileStats passed none ThingDef!");
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) &&
                obj is ProjectileDefStats p)
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

        public override string ToString()
        {
            return
                base.ToString() + Environment.NewLine +
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
