using System;
using RimWorld;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class GasPropertiesStats
	{
		public bool blockTurretTracking;
		public float accuracyPenalty;
		public MinMaxFloatStats expireSeconds;
		public float rotationSpeed;

		public GasPropertiesStats() { }
		public GasPropertiesStats(GasProperties p)
		{
			this.blockTurretTracking = p.blockTurretTracking;
			this.accuracyPenalty = p.accuracyPenalty;
			this.expireSeconds = new MinMaxFloatStats(p.expireSeconds);
			this.rotationSpeed = p.rotationSpeed;
		}
	}
}
