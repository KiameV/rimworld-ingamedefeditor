using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class VerbStats
    {
        public string name;
        public float warmupTime = 0;
        public float range = 0;
        public float timeBetweenShots = 0;
        public float burstShotCount = 0;
        public float muzzleFlashScale = 0;
        public float aiAvoidFriendlyRadius = 0;

        public DefStat<SoundDef> SoundCastDefStat = null;
        public DefStat<SoundDef> SoundCastTailDefStat = null;
        public DefStat<ThingDef> ProjectileDefStat = null;

        public VerbStats() { }
        public VerbStats(VerbProperties v)
        {
            this.name = v.verbClass.Name;
            this.warmupTime = v.warmupTime;
            this.range = v.range;
            this.timeBetweenShots = v.ticksBetweenBurstShots;
            this.burstShotCount = v.burstShotCount;
            this.muzzleFlashScale = v.muzzleFlashScale;
            this.aiAvoidFriendlyRadius = v.ai_AvoidFriendlyFireRadius;
            if (v.soundCast != null)
                this.SoundCastDefStat = new DefStat<SoundDef>(v.soundCast);
            if (v.soundCastTail != null)
                this.SoundCastTailDefStat = new DefStat<SoundDef>(v.soundCastTail);
            if (v.defaultProjectile != null)
                this.ProjectileDefStat = new ProjectileStats(v.defaultProjectile);
        }

        public void ApplyStats(VerbProperties to)
        {
#if DEBUG
            Log.Message("warmupTime = " + this.warmupTime);
            Log.Message("range = " + this.range);
            Log.Message("timeBetweenShots = " + this.timeBetweenShots);
            Log.Message("burstShotCount = " + this.burstShotCount);
            Log.Message("muzzleFlashScale = " + this.muzzleFlashScale);
            Log.Message("aiAvoidFriendlyRadius = " + this.aiAvoidFriendlyRadius);
#endif
            to.warmupTime = this.warmupTime;
            to.range = this.range;
            to.ticksBetweenBurstShots = (int)this.timeBetweenShots;
            to.burstShotCount = (int)this.burstShotCount;
            to.muzzleFlashScale = this.muzzleFlashScale;
            to.ai_AvoidFriendlyFireRadius = this.aiAvoidFriendlyRadius;
            to.soundCast = null;
            if (this.SoundCastDefStat != null)
                to.soundCast = this.SoundCastDefStat.Def;
            to.soundCastTail = null;
            if (this.SoundCastTailDefStat != null)
                to.soundCastTail = this.SoundCastTailDefStat.Def;
            if (this.ProjectileDefStat != null)
                ((ProjectileStats)this.ProjectileDefStat).ApplyStats(to.defaultProjectile);
        }

        public bool Initialize()
        {
            if (this.SoundCastDefStat != null)
                this.SoundCastDefStat.Initialize();
            if (this.SoundCastTailDefStat != null)
                this.SoundCastTailDefStat.Initialize();
            if (this.ProjectileDefStat != null)
                this.ProjectileDefStat.Initialize();
            return true;
        }

        public override bool Equals(object obj)
        {
#if DEBUG
            Log.Warning("Equals:");
            Log.Warning(this.ToString());
            Log.Warning(obj.ToString());
#endif
            if (obj != null &&
                obj is VerbStats)
            {
                VerbStats v = obj as VerbStats;
                if (String.Equals(this.name, v.name) &&
                    this.warmupTime == v.warmupTime &&
                    this.range == v.range &&
                    this.timeBetweenShots == v.timeBetweenShots &&
                    this.burstShotCount == v.burstShotCount &&
                    this.muzzleFlashScale == v.muzzleFlashScale &&
                    this.aiAvoidFriendlyRadius == v.aiAvoidFriendlyRadius &&
                    Util.AreEqual(this.ProjectileDefStat, v.ProjectileDefStat) &&
                    Util.AreEqual(this.SoundCastDefStat, v.SoundCastDefStat) &&
                    Util.AreEqual(this.SoundCastTailDefStat, v.SoundCastTailDefStat))
                {
                    if (this.ProjectileDefStat == null && v.ProjectileDefStat == null)
                        return true;
                    if (this.ProjectileDefStat != null && v.ProjectileDefStat != null &&
                        this.ProjectileDefStat.defName.Equals(v.ProjectileDefStat.defName))
                        return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }

        public override string ToString()
        {
            return
                typeof(VerbState).Name + Environment.NewLine +
                "    warmupTime: " + this.warmupTime + Environment.NewLine +
                "    range: " + this.range + Environment.NewLine +
                "    timeBetweenShots: " + this.timeBetweenShots + Environment.NewLine +
                "    burstShotCount: " + this.burstShotCount + Environment.NewLine +
                "    muzzleFlashScale: " + this.muzzleFlashScale + Environment.NewLine +
                "    aiAvoidFriendlyRadius: " + this.aiAvoidFriendlyRadius + Environment.NewLine +
                "    projectileDefName: " + ((this.ProjectileDefStat == null) ? "null" : this.ProjectileDefStat.defName.ToString()) + Environment.NewLine +
                "    soundCastDefName: " + this.SoundCastDefStat.ToString() + Environment.NewLine +
                "    soundCastTailDefName: " + this.SoundCastTailDefStat.ToString();
        }
    }
}
