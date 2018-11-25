using System;
using InGameDefEditor.Stats.DefStat;
using RimWorld;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class DamageMultiplierStats
    {
        public DefStat<ThingDef> DamageDef;
        public float Multiplier = -1f;

        public DamageMultiplierStats() { }
        public DamageMultiplierStats(DamageMultiplierStats d)
        {
            this.DamageDef = new DefStat<ThingDef>(d.DamageDef);
            this.Multiplier = d.Multiplier;
        }

        public override bool Equals(object obj)
        {
            if (obj != null &&
                obj is DamageMultiplierStats d)
            {
                return
                    this.Multiplier == d.Multiplier &&
                    object.Equals(this.DamageDef, d.DamageDef);
            }
            return false;
        }

        public bool Initialize()
        {
            return this.DamageDef.Initialize();
        }


        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return
                this.GetType().Name + Environment.NewLine +
                "    DamageDef: " + this.DamageDef.defName + Environment.NewLine +
                "    Multiplier: " + this.Multiplier;
        }
    }
}
