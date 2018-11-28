using System;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Stats.DefStat
{
	[Serializable]
	public class EffecterDefStat : DefStat<EffecterDef>
    {
        public float positionRadius;
        public FloatRange offsetTowardsTarget;

		// TODO
		// public List<SubEffecterStat> children;

		public EffecterDefStat() { }
		public EffecterDefStat(EffecterDef d) : base(d)
        {
            this.positionRadius = d.positionRadius;
            this.offsetTowardsTarget = d.offsetTowardsTarget;

			/*if (d.children == null)
				d.children = new List<SubEffecterDef>(0);
			this.children = new List<SubEffecterStat>(d.children);*/
		}

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;
            //foreach (var v in this.children)
            //    v.Initialize();
            return true;
		}

		public void ApplyStats(EffecterDef to)
		{
			to.positionRadius = this.positionRadius;
			to.offsetTowardsTarget = this.offsetTowardsTarget;

			/*if (to.children == null && this.children != null)
				to.children = new List<SubEffecterStat>(this.children.Count);
			else
				to.children.Clear();
			if (this.children != null)
				this.children.ForEach((SubEffecterStat s) => to.children.Add(s));*/
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is EffecterDefStat s)
			{
				return
					this.positionRadius == s.positionRadius &&
					this.offsetTowardsTarget == s.offsetTowardsTarget/* &&
					Util.AreEqual(this.children, s.children)*/;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}