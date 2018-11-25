using InGameDefEditor.Stats.DefStat;
using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class ThingDefCountClassStats
    {
        public DefStat<ThingDef> ThingDef;
        public float Count = 0;

        public ThingDefCountClassStats() { }
        public ThingDefCountClassStats(ThingDefCountClass t)
        {
            this.ThingDef = new DefStat<ThingDef>(t.thingDef);
            this.Count = t.count;
        }

        public override bool Equals(object obj)
        {
            if (obj != null &&
                obj is ThingDefCountClassStats s)
            {
                return
                    this.Count == s.Count &&
                    object.Equals(this.ThingDef, s.ThingDef);
            }
            return false;
        }

        public bool Initialize()
        {
            return this.ThingDef.Initialize();
        }


        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return
                this.GetType().Name + Environment.NewLine +
                "    ThingFilterStats: " + this.ThingDef.defName + Environment.NewLine +
                "    Count: " + this.Count;
        }
    }
}
