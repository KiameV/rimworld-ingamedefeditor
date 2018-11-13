using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class MinMaxStat<D> : DefStat<D> where D : Def
    {
        public float Min;
        public float Max;

        public MinMaxStat() { }
        public MinMaxStat(D d) : base(d) { }
        public MinMaxStat(MinMaxStat<D> s) : base(s.Def)
        {
            this.Min = s.Min;
            this.Max = s.Max;
        }

        public override void AssignStats(DefStat<D> to)
        {
            base.AssignStats(to);
            if (to is MinMaxStat<D> s)
            {
                s.Min = this.Min;
                s.Max = this.Max;
            }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) && 
                obj is MinMaxStat<D> stat)
            {
                return
                    this.Min == stat.Min &&
                    this.Max == stat.Max;
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
                base.ToString() + Environment.NewLine + 
                "    Min: " + this.Min + Environment.NewLine +
                "    Max: " + this.Max;
        }
    }
}
