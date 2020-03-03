using System;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class ExtraDamageStats : IInitializable
	{
		public DefStat<DamageDef> damageDefStat;
		public float amount;
		public float armorPenetration;
        public float chance;

		public string Label => damageDefStat.defName;

		public ExtraDamageStats() { }
		public ExtraDamageStats(ExtraDamage d)
		{
			this.damageDefStat = Util.AssignDefStat(d.def);
			this.amount = d.amount;
			this.armorPenetration = d.armorPenetration;
            this.chance = d.chance;
		}

		public bool Initialize()
		{
			return this.damageDefStat.Initialize();
		}

		public void ApplyStats(ExtraDamage to)
		{
			to.def = this.damageDefStat.Def;
			to.amount = this.amount;
			to.armorPenetration = this.armorPenetration;
            to.chance = this.chance;
		}
	}
}
