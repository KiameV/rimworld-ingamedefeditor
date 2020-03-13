using InGameDefEditor.Stats.DefStat;
using RimWorld;
using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class VerbStats : IInitializable
	{
		public VerbCategory category;
		public string label;
		public bool isPrimary;
		public float minRange;
		public float range;
		public int burstShotCount;
		public int ticksBetweenBurstShots;
		public float noiseRadius;
		public bool hasStandardCommand;
		public bool targetable;
		public bool requireLineOfSight;
		public bool mustCastOnOpenGround;
		public bool forceNormalTimeSpeed;
		public bool onlyManualCast;
		public bool stopBurstWithoutLos;
		public float commonality;
		public Intelligence minIntelligence;
		public float consumeFuelPerShot;
		public float warmupTime;
		public float defaultCooldownTime;
		public float muzzleFlashScale;
		public bool ensureLinkedBodyPartsGroupAlwaysUsable;
		public int meleeDamageBaseAmount;
		public float meleeArmorPenetrationBase;
		public bool ai_IsWeapon;
		public bool ai_IsBuildingDestroyer;
		public float ai_AvoidFriendlyFireRadius;
		public float forcedMissRadius;
		public float accuracyTouch;
		public float accuracyShort;
		public float accuracyMedium;
		public float accuracyLong;

		public TargetingParameterStats targetParams;
		public SurpriseAttackPropStats surpriseAttack;

		public DefStat<SoundDef> soundCast;
		public DefStat<SoundDef> soundCastTail;
		public DefStat<SoundDef> soundAiming;
		public DefStat<DamageDef> meleeDamageDef;
		public DefStat<ThingDef> impactMote;
		public DefStat<BodyPartGroupDef> linkedBodyPartsGroup;
		public DefStat<ThingDef> defaultProjectile;
		public DefStat<ThingDef> spawnDef;
		public DefStat<TaleDef> colonyWideTaleDef;
		public DefStat<BodyPartTagDef> bodypartTagTarget;
		public DefStat<RulePackDef> rangedFireRulepack;
		
		public string Label => label;

		public VerbStats() { }
        public VerbStats(VerbProperties v)
        {
			this.category = v.category;
			this.label = v.label;
			this.isPrimary = v.isPrimary;
			this.minRange = v.minRange;
			this.range = v.range;
			this.burstShotCount = v.burstShotCount;
			this.ticksBetweenBurstShots = v.ticksBetweenBurstShots;
			this.noiseRadius = v.noiseRadius;
			this.hasStandardCommand = v.hasStandardCommand;
			this.targetable = v.targetable;
			this.requireLineOfSight = v.requireLineOfSight;
			this.mustCastOnOpenGround = v.mustCastOnOpenGround;
			this.forceNormalTimeSpeed = v.forceNormalTimeSpeed;
			this.onlyManualCast = v.onlyManualCast;
			this.stopBurstWithoutLos = v.stopBurstWithoutLos;
			this.commonality = v.commonality;
			this.minIntelligence = v.minIntelligence;
			this.consumeFuelPerShot = v.consumeFuelPerShot;
			this.warmupTime = v.warmupTime;
			this.defaultCooldownTime = v.defaultCooldownTime;
			this.muzzleFlashScale = v.muzzleFlashScale;
			this.ensureLinkedBodyPartsGroupAlwaysUsable = v.ensureLinkedBodyPartsGroupAlwaysUsable;
			this.meleeDamageBaseAmount = v.meleeDamageBaseAmount;
			this.meleeArmorPenetrationBase = v.meleeArmorPenetrationBase;
			this.ai_IsWeapon = v.ai_IsWeapon;
			this.ai_IsBuildingDestroyer = v.ai_IsBuildingDestroyer;
			this.ai_AvoidFriendlyFireRadius = v.ai_AvoidFriendlyFireRadius;
			this.forcedMissRadius = v.forcedMissRadius;
			this.accuracyTouch = v.accuracyTouch;
			this.accuracyShort = v.accuracyShort;
			this.accuracyMedium = v.accuracyMedium;
			this.accuracyLong = v.accuracyLong;
			if (v.targetParams != null)
				this.targetParams = new TargetingParameterStats(v.targetParams);
			if (v.surpriseAttack != null)
				this.surpriseAttack = new SurpriseAttackPropStats(v.surpriseAttack);
			Util.AssignDefStat(v.soundCast, out this.soundCast);
			Util.AssignDefStat(v.soundCastTail, out this.soundCastTail);
			Util.AssignDefStat(v.soundAiming, out this.soundAiming);
			Util.AssignDefStat(v.meleeDamageDef, out this.meleeDamageDef);
			Util.AssignDefStat(v.impactMote, out this.impactMote);
			Util.AssignDefStat(v.linkedBodyPartsGroup, out this.linkedBodyPartsGroup);
			Util.AssignDefStat(v.defaultProjectile, out this.defaultProjectile);
			Util.AssignDefStat(v.spawnDef, out this.spawnDef);
			Util.AssignDefStat(v.colonyWideTaleDef, out this.colonyWideTaleDef);
			Util.AssignDefStat(v.bodypartTagTarget, out this.bodypartTagTarget);
			Util.AssignDefStat(v.rangedFireRulepack, out this.rangedFireRulepack);
		}

		public void ApplyStats(VerbProperties to)
        {
			to.category = this.category;
			to.label = this.label;
			to.isPrimary = this.isPrimary;
			to.minRange = this.minRange;
			to.range = this.range;
			to.burstShotCount = this.burstShotCount;
			to.ticksBetweenBurstShots = this.ticksBetweenBurstShots;
			to.noiseRadius = this.noiseRadius;
			to.hasStandardCommand = this.hasStandardCommand;
			to.targetable = this.targetable;
			to.requireLineOfSight = this.requireLineOfSight;
			to.mustCastOnOpenGround = this.mustCastOnOpenGround;
			to.forceNormalTimeSpeed = this.forceNormalTimeSpeed;
			to.onlyManualCast = this.onlyManualCast;
			to.stopBurstWithoutLos = this.stopBurstWithoutLos;
			to.commonality = this.commonality;
			to.minIntelligence = this.minIntelligence;
			to.consumeFuelPerShot = this.consumeFuelPerShot;
			to.warmupTime = this.warmupTime;
			to.defaultCooldownTime = this.defaultCooldownTime;
			to.muzzleFlashScale = this.muzzleFlashScale;
			to.ensureLinkedBodyPartsGroupAlwaysUsable = this.ensureLinkedBodyPartsGroupAlwaysUsable;
			to.meleeDamageBaseAmount = this.meleeDamageBaseAmount;
			to.meleeArmorPenetrationBase = this.meleeArmorPenetrationBase;
			to.ai_IsWeapon = this.ai_IsWeapon;
			to.ai_IsBuildingDestroyer = this.ai_IsBuildingDestroyer;
			to.ai_AvoidFriendlyFireRadius = this.ai_AvoidFriendlyFireRadius;
			to.forcedMissRadius = this.forcedMissRadius;
			to.accuracyTouch = this.accuracyTouch;
			to.accuracyShort = this.accuracyShort;
			to.accuracyMedium = this.accuracyMedium;
			to.accuracyLong = this.accuracyLong;

			/* TODO
			if (v.targetParams != null && this.targetParams != null)
				this.targetParams = new TargetingParameterStats(v.targetParams);
			if (v.surpriseAttack != null)
				this.surpriseAttack = new SurpriseAttackPropStats(v.surpriseAttack);*/

			Util.AssignDef(this.soundCast, out to.soundCast);
			Util.AssignDef(this.soundCastTail, out to.soundCastTail);
			Util.AssignDef(this.soundAiming, out to.soundAiming);
			Util.AssignDef(this.meleeDamageDef, out to.meleeDamageDef);
			Util.AssignDef(this.impactMote, out to.impactMote);
			Util.AssignDef(this.linkedBodyPartsGroup, out to.linkedBodyPartsGroup);
			Util.AssignDef(this.defaultProjectile, out to.defaultProjectile);
			Util.AssignDef(this.spawnDef, out to.spawnDef);
			Util.AssignDef(this.colonyWideTaleDef, out to.colonyWideTaleDef);
			Util.AssignDef(this.bodypartTagTarget, out to.bodypartTagTarget);
			Util.AssignDef(this.rangedFireRulepack, out to.rangedFireRulepack);
		}

        public bool Initialize()
		{
			if (this.surpriseAttack != null)
			{
				if (!this.surpriseAttack.Initialize())
					return false;
			}

			bool result = false;
			if (!Util.InitializeDefStat(this.soundCast)) result = false;
			if (!Util.InitializeDefStat(this.soundCastTail)) result = false;
			if (!Util.InitializeDefStat(this.soundAiming)) result = false;
			if (!Util.InitializeDefStat(this.meleeDamageDef)) result = false;
			if (!Util.InitializeDefStat(this.impactMote)) result = false;
			if (!Util.InitializeDefStat(this.linkedBodyPartsGroup)) result = false;
			if (!Util.InitializeDefStat(this.defaultProjectile)) result = false;
			if (!Util.InitializeDefStat(this.spawnDef)) result = false;
			if (!Util.InitializeDefStat(this.colonyWideTaleDef)) result = false;
			if (!Util.InitializeDefStat(this.bodypartTagTarget)) result = false;
			if (!Util.InitializeDefStat(this.rangedFireRulepack)) result = false;
			return result;
        }

        public override bool Equals(object obj)
        {
#if DEBUG_VERB
            Log.Warning("Equals:");
            Log.Warning(this.ToString());
            Log.Warning(obj.ToString());
#endif
            if (obj != null &&
                obj is VerbStats v)
            {
				return
					this.category == v.category &&
					this.label == v.label &&
					this.isPrimary == v.isPrimary &&
					this.minRange == v.minRange &&
					this.range == v.range &&
					this.burstShotCount == v.burstShotCount &&
					this.ticksBetweenBurstShots == v.ticksBetweenBurstShots &&
					this.noiseRadius == v.noiseRadius &&
					this.hasStandardCommand == v.hasStandardCommand &&
					this.targetable == v.targetable &&
					this.requireLineOfSight == v.requireLineOfSight &&
					this.mustCastOnOpenGround == v.mustCastOnOpenGround &&
					this.forceNormalTimeSpeed == v.forceNormalTimeSpeed &&
					this.onlyManualCast == v.onlyManualCast &&
					this.stopBurstWithoutLos == v.stopBurstWithoutLos &&
					this.commonality == v.commonality &&
					this.minIntelligence == v.minIntelligence &&
					this.consumeFuelPerShot == v.consumeFuelPerShot &&
					this.warmupTime == v.warmupTime &&
					this.defaultCooldownTime == v.defaultCooldownTime &&
					this.muzzleFlashScale == v.muzzleFlashScale &&
					this.ensureLinkedBodyPartsGroupAlwaysUsable == v.ensureLinkedBodyPartsGroupAlwaysUsable &&
					this.meleeDamageBaseAmount == v.meleeDamageBaseAmount &&
					this.meleeArmorPenetrationBase == v.meleeArmorPenetrationBase &&
					this.ai_IsWeapon == v.ai_IsWeapon &&
					this.ai_IsBuildingDestroyer == v.ai_IsBuildingDestroyer &&
					this.ai_AvoidFriendlyFireRadius == v.ai_AvoidFriendlyFireRadius &&
					this.forcedMissRadius == v.forcedMissRadius &&
					this.accuracyTouch == v.accuracyTouch &&
					this.accuracyShort == v.accuracyShort &&
					this.accuracyMedium == v.accuracyMedium &&
					this.accuracyLong == v.accuracyLong &&
					//object.Equals(this.targetParams, v.targetParams) &&
					//object.Equals(this.surpriseAttack, v.surpriseAttack) &&
					Util.AreEqual(this.soundCast, v.soundCast) &&
					Util.AreEqual(this.soundCastTail, v.soundCastTail) &&
					Util.AreEqual(this.soundAiming, v.soundAiming) &&
					Util.AreEqual(this.meleeDamageDef, v.meleeDamageDef) &&
					Util.AreEqual(this.impactMote, v.impactMote) &&
					Util.AreEqual(this.linkedBodyPartsGroup, v.linkedBodyPartsGroup) &&
					Util.AreEqual(this.defaultProjectile, v.defaultProjectile) &&
					Util.AreEqual(this.spawnDef, v.spawnDef) &&
					Util.AreEqual(this.colonyWideTaleDef, v.colonyWideTaleDef) &&
					Util.AreEqual(this.bodypartTagTarget, v.bodypartTagTarget) &&
					Util.AreEqual(this.rangedFireRulepack, v.rangedFireRulepack);
			}
            return false;
        }

        public override int GetHashCode()
        {
            return this.label.GetHashCode();
        }

        public override string ToString()
        {
            return
                typeof(VerbState).Name + Environment.NewLine +
                "    warmupTime: " + this.warmupTime + Environment.NewLine +
                "    range: " + this.range + Environment.NewLine +
				"    ticksBetweenBurstShots: " + this.ticksBetweenBurstShots + Environment.NewLine +
                "    burstShotCount: " + this.burstShotCount + Environment.NewLine +
                "    muzzleFlashScale: " + this.muzzleFlashScale + Environment.NewLine +
                "    aiAvoidFriendlyRadius: " + this.ai_AvoidFriendlyFireRadius + Environment.NewLine +
                "    projectileDefName: " + ((this.defaultProjectile == null) ? "null" : this.defaultProjectile.defName.ToString()) + Environment.NewLine +
                "    soundCastDefName: " + this.soundCast.ToString() + Environment.NewLine +
                "    soundCastTailDefName: " + this.soundCastTail.ToString();
        }
    }
}
