using InGameDefEditor.Stats.DefStat;
using RimWorld;
using Verse;

namespace InGameDefEditor.Stats
{
	[System.Serializable]
	public class SkillDefStats : DefStat<SkillDef>, IParentStat
	{
		public string skillLabel;
		public bool usuallyDefinedInBackstories;
		public bool pawnCreatorSummaryVisible;
		public WorkTags disablingWorkTags;
		public float listOrder;

		public SkillDefStats(SkillDef def) : base(def)
		{
			this.skillLabel = def.skillLabel;
			this.usuallyDefinedInBackstories = def.usuallyDefinedInBackstories;
			this.pawnCreatorSummaryVisible = def.pawnCreatorSummaryVisible;
			this.disablingWorkTags = def.disablingWorkTags;
			this.listOrder = def.listOrder;
		}

		public void ApplyStats(Def to)
		{
			if (to is SkillDef t)
			{
				t.skillLabel = this.skillLabel;
				t.usuallyDefinedInBackstories = this.usuallyDefinedInBackstories;
				t.pawnCreatorSummaryVisible = this.pawnCreatorSummaryVisible;
				t.disablingWorkTags = this.disablingWorkTags;
				t.listOrder = this.listOrder;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is SkillDef s)
			{
				return
					this.skillLabel.Equals(s.skillLabel) &&
					this.usuallyDefinedInBackstories == s.usuallyDefinedInBackstories &&
					this.pawnCreatorSummaryVisible == s.pawnCreatorSummaryVisible &&
					this.disablingWorkTags == s.disablingWorkTags &&
					this.listOrder == s.listOrder;
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