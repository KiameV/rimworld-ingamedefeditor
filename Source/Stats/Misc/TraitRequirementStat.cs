using InGameDefEditor.Stats.DefStat;
using RimWorld;
using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class TraitRequirementStat
	{
		public DefStat<TraitDef> def;
		public int? degree;

		public TraitRequirementStat() { }
		public TraitRequirementStat(TraitRequirement d)
		{
			Util.AssignDefStat(d.def, out this.def);
			this.degree = d.degree;
		}

		public bool Initialize()
		{
			Util.InitializeDefStat(this.def);
			return true;
		}

		public void ApplyStats(TraitRequirement to)
		{
			Util.AssignDef(this.def, out to.def);
			to.degree = this.degree;
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is TraitRequirement d)
			{
				return
					object.Equals(this.def, d.def) &&
					((this.degree == null && d.degree == null) ||
					 (this.degree != null && d.degree != null && this.degree.Value == d.degree.Value));
			}
			return false;
		}

		public override string ToString()
		{
			if (this.degree == null)
				return $"{this.def.defName} null";
			return $"{this.def.defName} {this.degree}";
		}

		public override int GetHashCode()
		{
			return base.ToString().GetHashCode();
		}
	}
}