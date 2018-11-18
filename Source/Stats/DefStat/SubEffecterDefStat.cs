using Verse;

namespace InGameDefEditor.Stats.DefStat
{
	public class SubEffecterStat
	{
		public int ticksBeforeSustainerStart;
		public IntRange intermittentSoundInterval;
		public FloatRange airTime;
		public FloatRange scale;
		public FloatRange rotationRate;
		public FloatRange rotation;
		public FloatRange speed;
		public bool absoluteAngle;
		public FloatRange angle;
		public float positionRadius;
		public float positionLerpFactor;
		public MoteSpawnLocType spawnLocType;
		public float chancePerTick;
		public int ticksBetweenMotes;
		public IntRange burstCount;

		public DefStat<SoundDef> soundDef;
		public DefStat<ThingDef> moteDef;

		public SubEffecterStat(SubEffecterDef d)
		{
			this.ticksBeforeSustainerStart = d.ticksBeforeSustainerStart;
			this.intermittentSoundInterval = d.intermittentSoundInterval;
			this.airTime = d.airTime;
			this.scale = d.scale;
			this.rotationRate = d.rotationRate;
			this.rotation = d.rotation;
			this.speed = d.speed;
			this.absoluteAngle = d.absoluteAngle;
			this.angle = d.angle;
			this.positionRadius = d.positionRadius;
			this.positionLerpFactor = d.positionLerpFactor;
			this.spawnLocType = d.spawnLocType;
			this.chancePerTick = d.chancePerTick;
			this.ticksBetweenMotes = d.ticksBetweenMotes;
			this.burstCount = d.burstCount;

			Util.TryAssignStatDef(d.soundDef, out this.soundDef);
			Util.TryAssignStatDef(d.moteDef, out this.moteDef);
		}

		public void ApplyStats(SubEffecterDef to)
		{
			to.ticksBeforeSustainerStart = this.ticksBeforeSustainerStart;
			to.intermittentSoundInterval = this.intermittentSoundInterval;
			to.airTime = this.airTime;
			to.scale = this.scale;
			to.rotationRate = this.rotationRate;
			to.rotation = this.rotation;
			to.speed = this.speed;
			to.absoluteAngle = this.absoluteAngle;
			to.angle = this.angle;
			to.positionRadius = this.positionRadius;
			to.positionLerpFactor = this.positionLerpFactor;
			to.spawnLocType = this.spawnLocType;
			to.chancePerTick = this.chancePerTick;
			to.ticksBetweenMotes = this.ticksBetweenMotes;
			to.burstCount = this.burstCount;

			Util.TryAssignDef(this.soundDef, out to.soundDef);
			Util.TryAssignDef(this.moteDef, out to.moteDef);
		}

		/*public void ApplyStats(SubEffecterStat to)
		{
			to.ticksBeforeSustainerStart = this.ticksBeforeSustainerStart;
			to.intermittentSoundInterval = this.intermittentSoundInterval;
			to.airTime = this.airTime;
			to.scale = this.scale;
			to.rotationRate = this.rotationRate;
			to.rotation = this.rotation;
			to.speed = this.speed;
			to.absoluteAngle = this.absoluteAngle;
			to.angle = this.angle;
			to.positionRadius = this.positionRadius;
			to.positionLerpFactor = this.positionLerpFactor;
			to.spawnLocType = this.spawnLocType;
			to.chancePerTick = this.chancePerTick;
			to.ticksBetweenMotes = this.ticksBetweenMotes;
			to.burstCount = this.burstCount;

			Util.AssignStat(this.soundDef, to.soundDef);
			Util.AssignStat(this.moteDef, to.moteDef);
		}*/

		public bool Initialize()
		{
			Util.InitializeDefStat(this.soundDef);
			Util.InitializeDefStat(this.moteDef);
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is SubEffecterStat s)
			{
				return
					this.ticksBeforeSustainerStart == s.ticksBeforeSustainerStart &&
					this.intermittentSoundInterval == s.intermittentSoundInterval &&
					this.airTime == s.airTime &&
					this.scale == s.scale &&
					this.rotationRate == s.rotationRate &&
					this.rotation == s.rotation &&
					this.speed == s.speed &&
					this.absoluteAngle == s.absoluteAngle &&
					this.angle == s.angle &&
					this.positionRadius == s.positionRadius &&
					this.positionLerpFactor == s.positionLerpFactor &&
					this.spawnLocType == s.spawnLocType &&
					this.chancePerTick == s.chancePerTick &&
					this.ticksBetweenMotes == s.ticksBetweenMotes &&
					this.burstCount == s.burstCount &&
					Util.AreEqual(this.soundDef, s.soundDef) &&
					Util.AreEqual(this.moteDef, s.moteDef);
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