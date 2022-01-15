using System;
using Verse;
using InGameDefEditor.Stats.DefStat;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class SkyfallerPropertiesStats
	{
		//public string shadow = "Things/Skyfaller/SkyfallerShadowCircle";

		public bool hitRoof;
		public bool reversed;
		public float explosionRadius;
		public float explosionDamageFactor;
		public float shrapnelDistanceFactor;
		public SkyfallerMovementType movementType;
		public float speed;
		public float cameraShake;
		public bool rotateGraphicTowardsDirection;
		public int anticipationSoundTicks;
		public int motesPerCell;
		
		public Vector2Stats shadowSize;
		public MinMaxIntStats ticksToImpactRange;
		public MinMaxIntStats metalShrapnelCountRange;
		public MinMaxIntStats rubbleShrapnelCountRange;

		public DefStat<DamageDef> explosionDamage;
		public DefStat<SoundDef> impactSound;
		public DefStat<SoundDef> anticipationSound;
		
		public SkyfallerPropertiesStats() { }
		public SkyfallerPropertiesStats(SkyfallerProperties p)
		{
			this.hitRoof = p.hitRoof;
			this.reversed = p.reversed;
			this.explosionRadius = p.explosionRadius;
			this.explosionDamageFactor = p.explosionDamageFactor;
			this.shrapnelDistanceFactor = p.shrapnelDistanceFactor;
			this.movementType = p.movementType;
			this.speed = p.speed;
			this.cameraShake = p.cameraShake;
			this.rotateGraphicTowardsDirection = p.rotateGraphicTowardsDirection;
			this.anticipationSoundTicks = p.anticipationSoundTicks;
			this.motesPerCell = p.motesPerCell;

			if (p.shadowSize == null)
				this.shadowSize = new Vector2Stats(p.shadowSize);
			if (p.ticksToImpactRange == null)
				this.ticksToImpactRange = new MinMaxIntStats(p.ticksToImpactRange);
			if (p.metalShrapnelCountRange == null)
				this.metalShrapnelCountRange = new MinMaxIntStats(p.metalShrapnelCountRange);
			if (p.rubbleShrapnelCountRange == null)
				this.rubbleShrapnelCountRange = new MinMaxIntStats(p.rubbleShrapnelCountRange);

			Util.AssignDefStat(p.explosionDamage, out this.explosionDamage);
			Util.AssignDefStat(p.impactSound, out this.impactSound);
			Util.AssignDefStat(p.anticipationSound, out this.anticipationSound);
		}

		public bool Initialize()
		{
			//Util.InitializeDefStat(this.parent);
			Util.InitializeDefStat(this.explosionDamage);
			Util.InitializeDefStat(this.impactSound);
			Util.InitializeDefStat(this.anticipationSound);
			return true;
		}
	}
}
