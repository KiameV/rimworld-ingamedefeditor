﻿using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class FloatValueStat<D> : DefStat<D> where D : Def
    {
        public float value;

        public FloatValueStat() { }
        public FloatValueStat(D d) : base(d) { }

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
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return
                base.ToString() + Environment.NewLine + 
                "    value: " + this.value;
        }
    }
}
