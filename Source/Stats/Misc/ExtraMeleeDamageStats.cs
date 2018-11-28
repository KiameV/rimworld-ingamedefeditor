using System;
using RimWorld;
using Verse;
using InGameDefEditor.Stats.DefStat;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class ExtraMeleeDamageStats : IInitializable
	{
		public DefStat<DamageDef> damageDefStat;
		public int amount;
		public float armorPenetration;

		public string Label => damageDefStat.defName;

		public ExtraMeleeDamageStats() { }
		public ExtraMeleeDamageStats(ExtraMeleeDamage d)
		{
			this.damageDefStat = new DefStat<DamageDef>(d.def);
			this.amount = d.amount;
			this.armorPenetration = d.armorPenetration;
		}

		public bool Initialize()
		{
			return this.damageDefStat.Initialize();
		}

		public void ApplyStats(ExtraMeleeDamage to)
		{
			to.def = this.damageDefStat.Def;
			to.amount = this.amount;
			to.armorPenetration = this.armorPenetration;
		}
	}
}
