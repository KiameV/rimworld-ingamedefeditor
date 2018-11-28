using System;
using Verse;

namespace InGameDefEditor.Stats.DefStat
{
	[Serializable]
	public class IntValueDefStat<D> : DefStat<D> where D : Def, new()
	{
		public int value;

		public IntValueDefStat() { }
		public IntValueDefStat(D d) : base(d) { }
		public IntValueDefStat(D d, int value) : base(d)
		{
			this.value = value;
		}
		public IntValueDefStat(IntValueDefStat<D> s) : base(s.Def)
		{
			this.value = s.value;
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is IntValueDefStat<D> s)
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
