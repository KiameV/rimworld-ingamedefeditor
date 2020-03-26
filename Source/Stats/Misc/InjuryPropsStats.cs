using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class InjuryPropsStats
    {
        public float painPerSeverity;
        public float averagePainPerSeverityPermanent;
        public float bleedRate;
        public bool canMerge;
        public string destroyedLabel;
        public string destroyedOutLabel;
        public bool useRemovedLabel;

		public InjuryPropsStats() { }
		public InjuryPropsStats(InjuryProps p)
		{
			this.painPerSeverity = p.painPerSeverity;
			this.averagePainPerSeverityPermanent = p.averagePainPerSeverityPermanent;
			this.bleedRate = p.bleedRate;
			this.canMerge = p.canMerge;
			this.destroyedLabel = p.destroyedLabel;
			this.destroyedOutLabel = p.destroyedOutLabel;
			this.useRemovedLabel = p.useRemovedLabel;
		}

		public InjuryProps ToInjuryProps()
		{
			return new InjuryProps()
			{
				painPerSeverity = this.painPerSeverity,
				averagePainPerSeverityPermanent = this.averagePainPerSeverityPermanent,
				bleedRate = this.bleedRate,
				canMerge = this.canMerge,
				destroyedLabel = this.destroyedLabel,
				destroyedOutLabel = this.destroyedOutLabel,
				useRemovedLabel = this.useRemovedLabel,
			};
		}

		public override bool Equals(object obj)
		{
			if (obj is InjuryProps p)
			{
				return
					this.painPerSeverity == p.painPerSeverity &&
					this.averagePainPerSeverityPermanent == p.averagePainPerSeverityPermanent &&
					this.bleedRate == p.bleedRate &&
					this.canMerge == p.canMerge &&
					this.destroyedLabel == p.destroyedLabel &&
					this.destroyedOutLabel == p.destroyedOutLabel &&
					this.useRemovedLabel == p.useRemovedLabel;
			}
			return false;
		}
	}
}
