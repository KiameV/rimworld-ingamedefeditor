using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class FloatValueStat<D> : DefStat<D> where D : Def
    {
        public float value;

        public FloatValueStat() { }
        public FloatValueStat(D d) : base(d) { }
        public FloatValueStat(FloatValueStat<D> s) : base(s.Def)
        {
            this.value = s.value;
        }

        public override void AssignStats(DefStat<D> to)
        {
            base.AssignStats(to);
            if (to is FloatValueStat<D> s)
                s.value = this.value;
        }

        public override bool Equals(object obj)
        {
            if (obj != null &&
                obj is FloatValueStat<D> s)
            {
                return 
                    base.Equals(obj) &&
                    this.value == s.value;
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
                "    value: " + this.value;
        }
    }
}
