﻿using System;
using Verse;

namespace InGameDefEditor.Stats.DefStat
{
	[Serializable]
	public class MinMaxFloatDefStat<D> : DefStat<D> where D : Def, new()
	{
        public float Min;
        public float Max;

        public MinMaxFloatDefStat() { }
        public MinMaxFloatDefStat(D d) : base(d) { }
        public MinMaxFloatDefStat(MinMaxFloatDefStat<D> s) : base(s.Def)
        {
            this.Min = s.Min;
            this.Max = s.Max;
        }

        /*public override void ApplyStats(DefStat<D> to)
        {
            base.ApplyStats(to);
            if (to is MinMaxStat<D> s)
            {
                s.Min = this.Min;
                s.Max = this.Max;
            }
        }*/

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) && 
                obj is MinMaxFloatDefStat<D> stat)
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
