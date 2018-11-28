using System;
using Verse;
using InGameDefEditor.Stats.DefStat;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class MotePropertiesStats
	{
		public bool realTime;
		public float fadeInTime;
		public float fadeOutTime;
		public float solidTime;
		public float speedPerTime;
		public float growthRate;
		public bool collide;
		public bool needsMaintenance;
		public bool rotateTowardsTarget;
		public bool rotateTowardsMoveDirection;

		public DefStat<SoundDef> landSound;

		public Vector3Stats acceleration;
		public Vector3Stats attachedDrawOffset;

		public MotePropertiesStats() { }
		public MotePropertiesStats(MoteProperties p)
		{
			this.realTime = p.realTime;
			this.fadeInTime = p.fadeInTime;
			this.fadeOutTime = p.fadeOutTime;
			this.solidTime = p.solidTime;
			this.speedPerTime = p.speedPerTime;
			this.growthRate = p.growthRate;
			this.collide = p.collide;
			this.needsMaintenance = p.needsMaintenance;
			this.rotateTowardsTarget = p.rotateTowardsTarget;
			this.rotateTowardsMoveDirection = p.rotateTowardsMoveDirection;

			Util.AssignDefStat(p.landSound, out this.landSound);

			this.acceleration = new Vector3Stats(p.acceleration);
			this.attachedDrawOffset = new Vector3Stats(p.attachedDrawOffset);
		}
	}
}
