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

		public GasProperties ToGasProperties()
		{
			return new GasProperties()
			{
				blockTurretTracking = this.blockTurretTracking,
				accuracyPenalty = this.accuracyPenalty,
				expireSeconds = this.expireSeconds.ToFloatRange(),
				rotationSpeed = this.rotationSpeed,
			};
		}

		public override bool Equals(object obj)
		{
			if (obj != null && 
				obj is GasProperties p)
			{
				return
					this.blockTurretTracking == p.blockTurretTracking &&
					this.accuracyPenalty == p.accuracyPenalty &&
					object.Equals(this.expireSeconds, p.expireSeconds) &&
					this.rotationSpeed == p.rotationSpeed;
			}
			return false;
		}

		public override string ToString()
		{
			return 
				"GasPropertiesStats" +
				"\nblockTurretTracking: " + blockTurretTracking +
				"\naccuracyPenalty: " + accuracyPenalty +
				"\nexpireSeconds: " + expireSeconds +
				"\nrotationSpeed: " + rotationSpeed;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}
