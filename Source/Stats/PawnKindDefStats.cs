using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Stats
{
	[Serializable]
	public class PawnKindDefStats : DefStat<PawnKindDef>, IParentStat
	{
		public DefStat<ThingDef> race;

		public DefStat<FactionDef> defaultFactionType;

		public List<TraitRequirementStat> forcedTraits;

		public List<DefStat<TraitDef>> disallowedTraits;

		public int minGenerationAge;

		public int maxGenerationAge;

		public Gender? fixedGender;

		public bool allowOldAgeInjuries;

		public bool destroyGearOnDrop;

		public float defendPointRadius;

		public bool factionHostileOnKill;

		public bool factionHostileOnDeath;

		public FloatRange? initialResistanceRange;

		public FloatRange? initialWillRange;

		//public float royalTitleChance;

		//public DefStat<RoyalTitleDef> titleRequired;

		//public DefStat<RoyalTitleDef> minTitleRequired;

		//public List<DefStat<RoyalTitleDef>> titleSelectOne;

		//public bool allowRoyalRoomRequirements = true;

		//public bool allowRoyalApparelRequirements = true;

		public bool isFighter = true;

		public float combatPower = -1f;

		public bool canArriveManhunter = true;

		public bool canBeSapper;

		public bool isGoodBreacher;

		public float baseRecruitDifficulty = 0.5f;

		public bool aiAvoidCover;

		public FloatRange fleeHealthThresholdRange;

		public float acceptArrestChanceFactor;

		public FloatRange gearHealthRange;

		public FloatRange weaponMoney;

		public List<string> weaponTags;

		public DefStat<ThingDef> weaponStuffOverride;

		public DefStat<ThingStyleDef> weaponStyleDef;

		public FloatRange apparelMoney = FloatRange.Zero;

		public List<DefStat<ThingDef>> apparelRequired;

		public List<string> apparelTags;

		public List<string> apparelDisallowTags;

		public float apparelAllowHeadgearChance = 1f;

		public bool apparelIgnoreSeasons;

		public bool ignoreFactionApparelStuffRequirements;

		// TODO This might be needed
		//public List<SpecificApparelRequirement> specificApparelRequirements;

		public List<DefStat<ThingDef>> techHediffsRequired;

		public FloatRange techHediffsMoney = FloatRange.Zero;

		public List<string> techHediffsTags;

		public List<string> techHediffsDisallowTags;

		public float techHediffsChance;

		public int techHediffsMaxAmount = 1;

		public float biocodeWeaponChance;

		public float chemicalAddictionChance;

		public float combatEnhancingDrugsChance;

		public IntRange combatEnhancingDrugsCount;

		public List<DefStat<ChemicalDef>> forcedAddictions;

		public bool trader;

		public int extraSkillLevels;

		public int minTotalSkillLevels;

		public int minBestSkillLevel;

		public IntRange wildGroupSize;

		public float ecoSystemWeight;

		public PawnKindDefStats() { }
		public PawnKindDefStats(PawnKindDef d) : base(d)
		{
			if (d.race != null)
				this.race = new DefStat<ThingDef>(d.race);
			if (d.defaultFactionType != null)
				this.defaultFactionType = new DefStat<FactionDef>(d.defaultFactionType);
			if (d.forcedTraits?.Count > 0)
			{
				this.forcedTraits = new List<TraitRequirementStat>(d.forcedTraits.Count);
				foreach (var t in d.forcedTraits)
					this.forcedTraits.Add(new TraitRequirementStat(t));
			}
			if (d.disallowedTraits?.Count > 0)
            {
				this.disallowedTraits = new List<DefStat<TraitDef>>(d.disallowedTraits.Count);
				foreach (var t in d.disallowedTraits)
					this.disallowedTraits.Add(new DefStat<TraitDef>(t));

			}
			this.minGenerationAge = d.minGenerationAge;
			this.maxGenerationAge = d.maxGenerationAge;
			if (d.fixedGender != null)
				this.fixedGender = d.fixedGender.Value;

			this.allowOldAgeInjuries = d.allowOldAgeInjuries;
			this.destroyGearOnDrop = d.destroyGearOnDrop;
			this.defendPointRadius = d.defendPointRadius;
			this.factionHostileOnKill = d.factionHostileOnKill;
			this.factionHostileOnDeath = d.factionHostileOnDeath;
			if (d.initialResistanceRange != null)
				this.initialResistanceRange = d.initialResistanceRange.Value;
			if (d.initialWillRange != null)
				this.initialWillRange = d.initialWillRange.Value;
			//this.allowRoyalRoomRequirements = true;
			//this.allowRoyalApparelRequirements = true;
			this.isFighter = d.isFighter;
			this.combatPower = d.combatPower;
			this.canArriveManhunter = d.canArriveManhunter;
			this.canBeSapper = d.canBeSapper;
			this.isGoodBreacher = d.isGoodBreacher;
			this.baseRecruitDifficulty = d.baseRecruitDifficulty;
			this.aiAvoidCover = d.aiAvoidCover;
			this.fleeHealthThresholdRange = new FloatRange(d.fleeHealthThresholdRange.min, d.fleeHealthThresholdRange.max);
			this.acceptArrestChanceFactor =  d.acceptArrestChanceFactor;
			this.gearHealthRange = new FloatRange(d.gearHealthRange.min, d.gearHealthRange.max);
			this.weaponMoney = new FloatRange(d.weaponMoney.min, d.weaponMoney.max);
			if (d.weaponTags != null)
            {
				this.weaponTags = new List<string>(d.weaponTags.Count);
				foreach (var s in d.weaponTags)
					this.weaponTags.Add(s);
            }
			if (d.weaponStuffOverride != null)
				this.weaponStuffOverride = new DefStat<ThingDef>(d.weaponStuffOverride);
			if (d.weaponStyleDef != null)
				this.weaponStyleDef = new DefStat<ThingStyleDef>(d.weaponStyleDef);
			this.apparelMoney = new FloatRange(d.apparelMoney.min, d.apparelMoney.max);
			if (d.apparelRequired != null)
			{
				this.apparelRequired = new List<DefStat<ThingDef>>(d.apparelRequired.Count);
				foreach (var a in d.apparelRequired)
					this.apparelRequired.Add(new DefStat<ThingDef>(a));
			}
			if (d.apparelTags != null)
			{
				this.apparelTags = new List<string>(d.apparelTags.Count);
				foreach (var s in d.apparelTags)
					this.apparelTags.Add(s);
			}
			if (d.apparelDisallowTags != null)
			{
				this.apparelDisallowTags = new List<string>(d.apparelDisallowTags.Count);
				foreach (var s in d.apparelDisallowTags)
					this.apparelDisallowTags.Add(s);
			}
			this.apparelAllowHeadgearChance = d.apparelAllowHeadgearChance;
			this.apparelIgnoreSeasons = d.apparelIgnoreSeasons;
			this.ignoreFactionApparelStuffRequirements = d.ignoreFactionApparelStuffRequirements;
			if (d.techHediffsRequired != null)
			{
				this.techHediffsRequired = new List<DefStat<ThingDef>>(d.techHediffsRequired.Count);
				foreach (var a in d.techHediffsRequired)
					this.techHediffsRequired.Add(new DefStat<ThingDef>(a));
			}
			this.techHediffsMoney = new FloatRange(d.techHediffsMoney.min, d.techHediffsMoney.max);
			if (d.techHediffsTags != null)
			{
				this.techHediffsTags = new List<string>(d.techHediffsTags.Count);
				foreach (var s in d.techHediffsTags)
					this.techHediffsTags.Add(s);
			}
			if (d.techHediffsDisallowTags != null)
			{
				this.techHediffsDisallowTags = new List<string>(d.techHediffsDisallowTags.Count);
				foreach (var s in d.techHediffsDisallowTags)
					this.techHediffsDisallowTags.Add(s);
			}
			this.techHediffsChance = d.techHediffsChance;
			this.techHediffsMaxAmount = d.techHediffsMaxAmount;
			this.biocodeWeaponChance = d.biocodeWeaponChance;
			this.chemicalAddictionChance = d.chemicalAddictionChance;
			this.combatEnhancingDrugsChance = d.combatEnhancingDrugsChance;
			this.combatEnhancingDrugsCount = new IntRange(d.combatEnhancingDrugsCount.min, d.combatEnhancingDrugsCount.max);
			if (d.forcedAddictions != null)
			{
				this.forcedAddictions = new List<DefStat<ChemicalDef>>(d.forcedAddictions.Count);
				foreach (var a in d.forcedAddictions)
					this.forcedAddictions.Add(new DefStat<ChemicalDef>(a));
			}
			this.trader = d.trader;
			this.extraSkillLevels = d.extraSkillLevels;
			this.minTotalSkillLevels = d.minTotalSkillLevels;
			this.minBestSkillLevel = d.minBestSkillLevel;
			this.wildGroupSize = new IntRange(d.wildGroupSize.min, d.wildGroupSize.max);
			this.ecoSystemWeight = d.ecoSystemWeight;
		}

		public override bool Initialize()
		{
			if (!base.Initialize())
				return false;
			if (this.forcedTraits?.Count > 0)
				foreach (var v in this.forcedTraits)
					v?.Initialize();
			this.race?.Initialize();
			this.defaultFactionType?.Initialize();
			this.weaponStuffOverride?.Initialize();
			this.weaponStyleDef?.Initialize();
			if (this.apparelRequired?.Count > 0)
				foreach (var v in this.apparelRequired)
					v?.Initialize();
			if (this.techHediffsRequired?.Count > 0)
				foreach (var v in this.techHediffsRequired)
					v?.Initialize();
			if (this.forcedAddictions?.Count > 0)
				foreach (var v in this.forcedAddictions)
					v?.Initialize();
			return true;
		}

		public void ApplyStats(object to)
		{
#if DEBUG_HEDIFFDEF
            Log.Warning("ApplyStats for " + d.defName);
#endif
			if (to is PawnKindDef d)
			{
				d.race = this.race?.Def;
				d.defaultFactionType = this.defaultFactionType?.Def;
				d.forcedTraits?.Clear();
				if (this.forcedTraits?.Count > 0)
				{
					d.forcedTraits = new List<TraitRequirement>(this.forcedTraits.Count);
					foreach (var t in this.forcedTraits)
						d.forcedTraits.Add(new TraitRequirement()
						{
							def = t.def.Def,
							degree = t.degree,
						});
				}
				d.disallowedTraits?.Clear();
				if (this.disallowedTraits?.Count > 0)
				{
					d.disallowedTraits = new List<TraitDef>(this.disallowedTraits.Count);
					foreach (var t in this.disallowedTraits)
						d.disallowedTraits.Add(t.Def);
				}
				d.minGenerationAge = this.minGenerationAge;
				d.maxGenerationAge = this.maxGenerationAge;
				d.fixedGender = null;
				if (this.fixedGender != null)
					d.fixedGender = this.fixedGender.Value;
				d.allowOldAgeInjuries = this.allowOldAgeInjuries;
				d.destroyGearOnDrop = this.destroyGearOnDrop;
				d.defendPointRadius = this.defendPointRadius;
				d.factionHostileOnKill = this.factionHostileOnKill;
				d.factionHostileOnDeath = this.factionHostileOnDeath;
				d.initialResistanceRange = null;
				if (this.initialResistanceRange != null)
					d.initialResistanceRange = this.initialResistanceRange.Value;
				d.initialWillRange = null;
				if (this.initialWillRange != null)
					d.initialWillRange = this.initialWillRange.Value;
				//d.allowRoyalRoomRequirements = this.allowRoyalRoomRequirements;
				//d.allowRoyalApparelRequirements = this.allowRoyalApparelRequirements;
				d.isFighter = this.isFighter;
				d.combatPower = this.combatPower;
				d.canArriveManhunter = this.canArriveManhunter;
				d.canBeSapper = this.canBeSapper;
				d.isGoodBreacher = this.isGoodBreacher;
				d.baseRecruitDifficulty = this.baseRecruitDifficulty;
				d.aiAvoidCover = this.aiAvoidCover;
				d.fleeHealthThresholdRange = new FloatRange(this.fleeHealthThresholdRange.min, this.fleeHealthThresholdRange.max);
				d.acceptArrestChanceFactor = this.acceptArrestChanceFactor;
				d.gearHealthRange = new FloatRange(this.gearHealthRange.min, this.gearHealthRange.max);
				d.weaponMoney = new FloatRange(this.weaponMoney.min, this.weaponMoney.max);
				d.weaponTags?.Clear();
				if (this.weaponTags != null)
				{
					d.weaponTags = new List<string>(this.weaponTags.Count);
					foreach (var s in this.weaponTags)
						d.weaponTags.Add(s);
				}
				d.weaponStuffOverride = this.weaponStuffOverride?.Def;
				d.weaponStyleDef = this.weaponStyleDef?.Def;
				d.apparelMoney = new FloatRange(this.apparelMoney.min, this.apparelMoney.max);
				d.apparelRequired?.Clear();
				if (this.apparelRequired != null)
				{
					d.apparelRequired = new List<ThingDef>(this.apparelRequired.Count);
					foreach (var a in this.apparelRequired)
						d.apparelRequired.Add(a.Def);
				}
				d.apparelTags?.Clear();
				if (this.apparelTags != null)
				{
					d.apparelTags = new List<string>(this.apparelTags.Count);
					foreach (var s in this.apparelTags)
						d.apparelTags.Add(s);
				}
				d.apparelDisallowTags?.Clear();
				if (this.apparelDisallowTags != null)
				{
					d.apparelDisallowTags = new List<string>(this.apparelDisallowTags.Count);
					foreach (var s in this.apparelDisallowTags)
						d.apparelDisallowTags.Add(s);
				}
				d.apparelAllowHeadgearChance = this.apparelAllowHeadgearChance;
				d.apparelIgnoreSeasons = this.apparelIgnoreSeasons;
				d.ignoreFactionApparelStuffRequirements = this.ignoreFactionApparelStuffRequirements;
				d.techHediffsRequired?.Clear();
				if (this.techHediffsRequired != null)
				{
					d.techHediffsRequired = new List<ThingDef>(this.techHediffsRequired.Count);
					foreach (var a in this.techHediffsRequired)
						d.techHediffsRequired.Add(a.Def);
				}
				d.techHediffsMoney = new FloatRange(this.techHediffsMoney.min, this.techHediffsMoney.max);
				d.techHediffsTags?.Clear();
				if (this.techHediffsTags != null)
				{
					d.techHediffsTags = new List<string>(this.techHediffsTags.Count);
					foreach (var s in this.techHediffsTags)
						d.techHediffsTags.Add(s);
				}
				d.techHediffsDisallowTags?.Clear();
				if (this.techHediffsDisallowTags != null)
				{
					d.techHediffsDisallowTags = new List<string>(this.techHediffsDisallowTags.Count);
					foreach (var s in this.techHediffsDisallowTags)
						d.techHediffsDisallowTags.Add(s);
				}
				d.techHediffsChance = this.techHediffsChance;
				d.techHediffsMaxAmount = this.techHediffsMaxAmount;
				d.biocodeWeaponChance = this.biocodeWeaponChance;
				d.chemicalAddictionChance = this.chemicalAddictionChance;
				d.combatEnhancingDrugsChance = this.combatEnhancingDrugsChance;
				d.combatEnhancingDrugsCount = new IntRange(this.combatEnhancingDrugsCount.min, this.combatEnhancingDrugsCount.max);
				d.forcedAddictions?.Clear();
				if (this.forcedAddictions != null)
				{
					d.forcedAddictions = new List<ChemicalDef>(this.forcedAddictions.Count);
					foreach (var a in this.forcedAddictions)
						d.forcedAddictions.Add(a.Def);
				}
				d.trader = this.trader;
				d.extraSkillLevels = this.extraSkillLevels;
				d.minTotalSkillLevels = this.minTotalSkillLevels;
				d.minBestSkillLevel = this.minBestSkillLevel;
				d.wildGroupSize = new IntRange(this.wildGroupSize.min, this.wildGroupSize.max);
				d.ecoSystemWeight = this.ecoSystemWeight;
#if DEBUG_PKD
            Log.Warning("ApplyStats Done");
#endif
			}
			else
				Log.Error("PawnKindDefStat passed none PawnKindDef!");
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
