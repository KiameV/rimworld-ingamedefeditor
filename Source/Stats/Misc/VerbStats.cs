using System;
using System.Xml.Serialization;
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

        [XmlIgnore]
        public SoundDef SoundCastDef;
        public string soundCastDefName;

        [XmlIgnore]
        public SoundDef SoundCastTailDef;
        public string soundCastTailDefName;

        [XmlIgnore]
        public ThingDef ProjectileDef;
        public string projectileDefName;

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
            this.SoundCastDef = v.soundCast;
            this.SoundCastTailDef = v.soundCastTail;
            if (v.defaultProjectile != null)
            {
                this.ProjectileDef = v.defaultProjectile;
                this.projectileDefName = v.defaultProjectile.defName;
            }
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
            to.defaultProjectile = this.ProjectileDef;
            to.soundCast = this.SoundCastDef;
            to.soundCastTail = this.SoundCastTailDef;
        }

        public bool Initialize()
        {
            if (this.projectileDefName == null || this.projectileDefName.Length == 0)
                return false;

            if (this.ProjectileDef == null)
            {
                this.ProjectileDef = DefDatabase<ThingDef>.AllDefsListForReading.Find(
                    delegate (ThingDef d) { return d.defName.Equals(this.projectileDefName); });
            }

            if (this.SoundCastDef == null || this.SoundCastTailDef == null)
            {
                foreach (SoundDef d in DefDatabase<SoundDef>.AllDefsListForReading)
                {
                    if (d.defName.Equals(this.soundCastDefName))
                        this.SoundCastDef = d;
                    if (d.defName.Equals(this.soundCastTailDefName))
                        this.SoundCastTailDef = d;

                    if (this.SoundCastDef != null && this.SoundCastTailDef != null)
                        break;
                }
            }

            return this.ProjectileDef != null;
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
                return
                    String.Equals(this.name, v.name) &&
                    this.warmupTime == v.warmupTime &&
                    this.range == v.range &&
                    this.timeBetweenShots == v.timeBetweenShots &&
                    this.burstShotCount == v.burstShotCount &&
                    this.muzzleFlashScale == v.muzzleFlashScale &&
                    this.aiAvoidFriendlyRadius == v.aiAvoidFriendlyRadius &&
                    String.Equals(this.projectileDefName, v.projectileDefName) && 
                    String.Equals(this.soundCastDefName, v.soundCastDefName) && 
                    String.Equals(this.soundCastTailDefName, v.soundCastTailDefName);
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
                "    projectileDefName: " + this.projectileDefName + Environment.NewLine +
                "    soundCastDefName: " + this.soundCastDefName + Environment.NewLine +
                "    soundCastTailDefName: " + this.soundCastTailDefName;
        }
    }
}
