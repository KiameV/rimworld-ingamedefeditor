using RimWorld;
using System;
using Verse;

namespace InGameDefEditor.Stats.DefStat
{
	[Serializable]
	public class FloatValueDefStat<D> : DefStat<D> where D : Def, new()
	{
        public float value;

        public FloatValueDefStat() { }
        public FloatValueDefStat(D d, float value = 0) : base(d)
		{
			this.value = value;
		}

		/*public override void ApplyStats(DefStat<D> to)
        {
            base.ApplyStats(to);
            if (to is FloatValueStat<D> s)
                s.value = this.value;
        }*/

		public override bool Equals(object obj)
        {
            if (obj != null &&
                obj is FloatValueDefStat<D> s)
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
