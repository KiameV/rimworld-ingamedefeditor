using InGameDefEditor.Stats.DefStat;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	public enum StoryTellerCompPropertyTypes
	{
		None,
		CategoryIndividualMTBByBiome,
		CategoryMTB,
		ClassicIntro,
		DeepDrillInfestation,
		Disease,
		FactionInteraction,
		JourneyOffer,
		OnOffCycle,
		RandomMain,
		ShipChunkDrop,
		SingleMTB,
		Triggered,
	}

	[Serializable]
	public class StorytellerCompPropertiesStats
	{
		public string compClass;

		public float minDaysPassed;
		public float minIncChancePopulationIntentFactor;

		public List<DefStat<IncidentTargetTagDef>> allowedTargetTags;
		public List<DefStat<IncidentTargetTagDef>> disallowedTargetTags;

		// Shared
		public DefStat<IncidentCategoryDef> category;
		public DefStat<IncidentDef> incident;
		public float minSpacingDays;
		public float mtbDays;

		// StorytellerCompProperties_CategoryIndividualMTBByBiome
		public bool applyCaravanVisibility;

		// StorytellerCompProperties_CategoryMTB
		public SimpleCurveStats mtbDaysFactorByDaysPassedCurve;

		// StorytellerCompProperties_DeepDrillInfestation
		public float baseMtbDaysPerDrill;

		// StorytellerCompProperties_FactionInteraction
		public float baseIncidentsPerYear;
		public StoryDanger minDanger;
		public bool fullAlliesOnly;

		// StorytellerCompProperties_OnOffCycle : StorytellerCompProperties
		public float onDays;
		public float offDays;
		public MinMaxFloatStats numIncidentsRange;
		public SimpleCurveStats acceptFractionByDaysPassedCurve;
		public SimpleCurveStats acceptPercentFactorPerThreatPointsCurve;
		public bool applyRaidBeaconThreatMtbFactor;
		public float forceRaidEnemyBeforeDaysPassed;

		// StorytellerCompProperties_RandomMain : StorytellerCompProperties
		public List<FloatValueDefStat<IncidentCategoryDef>> categoryWeights;
		public float maxThreatBigIntervalDays;
		public MinMaxFloatStats randomPointsFactorRange;
		public bool skipThreatBigIfRaidBeacon;

		// StorytellerCompProperties_Triggered : StorytellerCompProperties
		public int delayTicks;

		public StorytellerCompPropertiesStats() { }
		public StorytellerCompPropertiesStats(StorytellerCompProperties p)
		{
			this.compClass = p.compClass.FullName;

			this.minDaysPassed = p.minDaysPassed;
			this.minIncChancePopulationIntentFactor = p.minIncChancePopulationIntentFactor;

			Util.Populate(out allowedTargetTags, p.allowedTargetTags, d => new DefStat<IncidentTargetTagDef>(d));
			Util.Populate(out disallowedTargetTags, p.disallowedTargetTags, d => new DefStat<IncidentTargetTagDef>(d));

			switch (this.compClass)
			{
				case "RimWorld.StorytellerComp_CategoryIndividualMTBByBiome":
					if (p is StorytellerCompProperties_CategoryIndividualMTBByBiome cb)
					{
						this.category = Util.AssignDefStat(cb.category);
						this.applyCaravanVisibility = cb.applyCaravanVisibility;
					}
					break;
				case "RimWorld.StorytellerComp_CategoryMTB":
					if (p is StorytellerCompProperties_CategoryMTB cm)
					{
						this.category = Util.AssignDefStat(cm.category);
						this.mtbDays = cm.mtbDays;
						this.mtbDaysFactorByDaysPassedCurve = Util.Assign(cm.mtbDaysFactorByDaysPassedCurve, v => new SimpleCurveStats(v));
					}
					break;
				case "RimWorld.StorytellerComp_DeepDrillInfestation":
					if (p is StorytellerCompProperties_DeepDrillInfestation cd)
					{
						this.baseMtbDaysPerDrill = cd.baseMtbDaysPerDrill;
					}
					break;
				case "RimWorld.StorytellerComp_Disease":
					if (p is StorytellerCompProperties_Disease d)
					{
						this.category = Util.AssignDefStat(d.category);
					}
					break;
				case "RimWorld.StorytellerComp_FactionInteraction":
					if (p is StorytellerCompProperties_FactionInteraction fi)
					{
						this.incident = Util.AssignDefStat(fi.incident);
						this.baseIncidentsPerYear = fi.baseIncidentsPerYear;
						this.minSpacingDays = fi.minSpacingDays;
						this.minDanger = fi.minDanger;
						this.fullAlliesOnly = fi.fullAlliesOnly;
					}
					break;
				case "RimWorld.StorytellerComp_OnOffCycle":
					if (p is StorytellerCompProperties_OnOffCycle ooc)
					{
						this.category = Util.AssignDefStat(GetCategory(ooc));
						this.onDays = ooc.onDays;
						this.offDays = ooc.offDays;
						this.minSpacingDays = ooc.minDaysPassed;
						this.numIncidentsRange = Util.Assign(ooc.numIncidentsRange, v => new MinMaxFloatStats(v));
						this.acceptFractionByDaysPassedCurve = Util.Assign(ooc.acceptFractionByDaysPassedCurve, v => new SimpleCurveStats(v));
						this.acceptPercentFactorPerThreatPointsCurve = Util.Assign(ooc.acceptPercentFactorPerThreatPointsCurve, v => new SimpleCurveStats(v));
						this.incident = Util.AssignDefStat(ooc.incident);
						this.applyRaidBeaconThreatMtbFactor = ooc.applyRaidBeaconThreatMtbFactor;
						this.forceRaidEnemyBeforeDaysPassed = ooc.forceRaidEnemyBeforeDaysPassed;
					}
					break;
				case "RimWorld.StorytellerComp_RandomMain":
					if (p is StorytellerCompProperties_RandomMain rm)
					{
						this.mtbDays = rm.mtbDays;
						Util.Populate(out this.categoryWeights, rm.categoryWeights, v => new FloatValueDefStat<IncidentCategoryDef>(v.category, v.weight));
						this.maxThreatBigIntervalDays = rm.maxThreatBigIntervalDays;
						this.randomPointsFactorRange = Util.Assign(rm.randomPointsFactorRange, v => new MinMaxFloatStats(v));
						this.skipThreatBigIfRaidBeacon = rm.skipThreatBigIfRaidBeacon;
					}
					break;
				case "RimWorld.StorytellerComp_SingleMTB":
					if (p is StorytellerCompProperties_SingleMTB smtb)
					{
						this.incident = Util.AssignDefStat(smtb.incident);
						this.mtbDays = smtb.mtbDays;
					}
					break;
				case "RimWorld.StorytellerComp_Triggered":
					if (p is StorytellerCompProperties_Triggered t)
					{
						this.incident = Util.AssignDefStat(t.incident);
						this.delayTicks = t.delayTicks;
					}
					break;
				case "RimWorld.StorytellerComp_ClassicIntro":
				case "RimWorld.StorytellerComp_ShipChunkDrop":
				case "RimWorld.StorytellerComp_JourneyOffer":
					// Do nothing
					break;
				default:
					Log.Warning("Unknown StorytellerProperty type of " + this.compClass);
					break;
			}
		}

		public bool Initialize()
		{
			this.allowedTargetTags?.ForEach(v => v.Initialize());
			this.disallowedTargetTags?.ForEach(v => v.Initialize());
			this.categoryWeights?.ForEach(v => v.Initialize());
			this.category?.Initialize();
			this.incident?.Initialize();
			return true;
		}

		public void ApplyStats(StorytellerCompProperties to)
		{
			to.minDaysPassed = this.minDaysPassed;
			to.minIncChancePopulationIntentFactor = this.minIncChancePopulationIntentFactor;
			
			Util.Populate(out to.allowedTargetTags, this.allowedTargetTags, (v) => v.Def);
			Util.Populate(out to.disallowedTargetTags, this.disallowedTargetTags, (v) => v.Def);

			switch (this.compClass)
			{
				case "RimWorld.StorytellerComp_CategoryIndividualMTBByBiome":
					if (to is StorytellerCompProperties_CategoryIndividualMTBByBiome cb)
					{
						cb.category = Util.AssignDef(this.category);
						cb.applyCaravanVisibility = this.applyCaravanVisibility;
					}
					break;
				case "RimWorld.StorytellerComp_CategoryMTB":
					if (to is StorytellerCompProperties_CategoryMTB cm)
					{
						cm.category = Util.AssignDef(this.category);
						cm.mtbDays = this.mtbDays;
						this.mtbDaysFactorByDaysPassedCurve?.ApplyStats(cm.mtbDaysFactorByDaysPassedCurve);
					}
					break;
				case "RimWorld.StorytellerComp_DeepDrillInfestation":
					if (to is StorytellerCompProperties_DeepDrillInfestation cd)
					{
						cd.baseMtbDaysPerDrill = this.baseMtbDaysPerDrill;
					}
					break;
				case "RimWorld.StorytellerComp_Disease":
					if (to is StorytellerCompProperties_Disease d)
					{
						d.category = Util.AssignDef(this.category);
					}
					break;
				case "RimWorld.StorytellerComp_FactionInteraction":
					if (to is StorytellerCompProperties_FactionInteraction fi)
					{
						fi.incident = Util.AssignDef(this.incident);
						fi.baseIncidentsPerYear = this.baseIncidentsPerYear;
						fi.minSpacingDays = this.minSpacingDays;
						fi.minDanger = this.minDanger;
						fi.fullAlliesOnly = this.fullAlliesOnly;
					}
					break;
				case "RimWorld.StorytellerComp_OnOffCycle":
					if (to is StorytellerCompProperties_OnOffCycle ooc)
					{
						SetCategory(ooc, Util.AssignDef(this.category));
						ooc.onDays = this.onDays;
						ooc.offDays = this.offDays;
						ooc.minSpacingDays = this.minDaysPassed;
						ooc.numIncidentsRange = Util.Assign(this.numIncidentsRange, v => v.ToFloatRange());
						this.acceptFractionByDaysPassedCurve?.ApplyStats(ooc.acceptFractionByDaysPassedCurve);
						this.acceptPercentFactorPerThreatPointsCurve?.ApplyStats(ooc.acceptPercentFactorPerThreatPointsCurve);
						ooc.incident = Util.AssignDef(this.incident);
						ooc.applyRaidBeaconThreatMtbFactor = this.applyRaidBeaconThreatMtbFactor;
						ooc.forceRaidEnemyBeforeDaysPassed = this.forceRaidEnemyBeforeDaysPassed;
					}
					break;
				case "RimWorld.StorytellerComp_RandomMain":
					if (to is StorytellerCompProperties_RandomMain rm)
					{
						rm.mtbDays = this.mtbDays;
						Util.ListIndexAssign(this.categoryWeights, rm.categoryWeights, (f, t) =>
						{
							t.category = f.Def;
							t.weight = f.value;
						});
						rm.maxThreatBigIntervalDays = this.maxThreatBigIntervalDays;
						rm.randomPointsFactorRange = Util.Assign(this.randomPointsFactorRange, v => v.ToFloatRange());
						rm.skipThreatBigIfRaidBeacon = this.skipThreatBigIfRaidBeacon;
					}
					break;
				case "RimWorld.StorytellerComp_SingleMTB":
					if (to is StorytellerCompProperties_SingleMTB smtb)
					{
						smtb.incident = Util.AssignDef(this.incident);
						smtb.mtbDays = this.mtbDays;
					}
					break;
				case "RimWorld.StorytellerComp_Triggered":
					if (to is StorytellerCompProperties_Triggered tr)
					{
						tr.incident = Util.AssignDef(this.incident);
						tr.delayTicks = this.delayTicks;
					}
					break;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is StorytellerCompPropertiesStats d)
			{
				if (this.minDaysPassed == d.minDaysPassed &&
					this.minIncChancePopulationIntentFactor == d.minIncChancePopulationIntentFactor &&
					Util.AreEqual(this.allowedTargetTags, d.allowedTargetTags) &&
					Util.AreEqual(this.disallowedTargetTags, d.disallowedTargetTags))
				{
					switch (this.compClass)
					{
						case "RimWorld.StorytellerComp_CategoryIndividualMTBByBiome":
							return
								object.Equals(this.category, d.category) &&
								this.applyCaravanVisibility == d.applyCaravanVisibility;
						case "RimWorld.StorytellerComp_CategoryMTB":
							return
								object.Equals(this.category, d.category) &&
								this.mtbDays == d.mtbDays &&
								object.Equals(this.mtbDaysFactorByDaysPassedCurve, d.mtbDaysFactorByDaysPassedCurve);
						case "RimWorld.StorytellerComp_DeepDrillInfestation":
							return this.baseMtbDaysPerDrill == d.baseMtbDaysPerDrill;
						case "RimWorld.StorytellerComp_Disease":
							return object.Equals(this.category, d.category);
						case "RimWorld.StorytellerComp_FactionInteraction":
							return
								object.Equals(this.incident, d.incident) &&
								this.baseIncidentsPerYear == d.baseIncidentsPerYear &&
								this.minSpacingDays == d.minSpacingDays &&
								this.minDanger == d.minDanger &&
								this.fullAlliesOnly == d.fullAlliesOnly;
						case "RimWorld.StorytellerComp_OnOffCycle":
							return
								object.Equals(this.category, d.category) &&
								this.onDays == d.onDays &&
								this.offDays == d.offDays &&
								this.minSpacingDays == d.minSpacingDays &&
								//object.Equals(this.numIncidentsRange, d.numIncidentsRange) &&
								//object.Equals(this.acceptFractionByDaysPassedCurve, d.acceptFractionByDaysPassedCurve) &&
								//object.Equals(this.acceptPercentFactorPerThreatPointsCurve, d.acceptPercentFactorPerThreatPointsCurve) &&
								//object.Equals(this.incident, d.incident) &&
								this.applyRaidBeaconThreatMtbFactor == d.applyRaidBeaconThreatMtbFactor &&
								this.forceRaidEnemyBeforeDaysPassed == d.forceRaidEnemyBeforeDaysPassed;
						case "RimWorld.StorytellerComp_RandomMain":
							return
								this.mtbDays == d.mtbDays &&
								Util.AreEqual(this.categoryWeights, d.categoryWeights) &&
								this.maxThreatBigIntervalDays == d.maxThreatBigIntervalDays &&
								object.Equals(this.randomPointsFactorRange, d.randomPointsFactorRange) &&
								this.skipThreatBigIfRaidBeacon == d.skipThreatBigIfRaidBeacon;
						case "RimWorld.StorytellerComp_SingleMTB":
							return
								object.Equals(this.incident, d.incident) &&
								this.mtbDays == d.mtbDays;
						case "RimWorld.StorytellerComp_Triggered":
							return
								object.Equals(this.incident, d.incident) &&
								this.delayTicks == d.delayTicks;
					}
					return true;
				}
			}
			return false;
		}

		public StorytellerCompProperties ToStorytellerCompProperties()
		{
			StorytellerCompProperties p = CreateStorytellerCompProperties(GetType(this.compClass));
			if (p == null)
				Log.Warning("Unable to create story teller comp property " + this.compClass);
			else
				this.ApplyStats(p);
			return p;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(this.compClass + "\n");
			sb.AppendLine("    minDaysPassed: " + this.minDaysPassed);
			sb.AppendLine("    minIncChancePopulationIntentFactor: " + this.minIncChancePopulationIntentFactor);
			sb.Append("    allowedTargetTags: ");
			this.allowedTargetTags?.ForEach(v => sb.Append(v.defName + ", "));
			sb.AppendLine();
			sb.Append("    disallowedTargetTags: ");
			this.disallowedTargetTags?.ForEach(v => sb.Append(v.defName + ", "));
			sb.AppendLine();
			sb.AppendLine("    category: " + this.category?.defName);
			sb.AppendLine("    incident: " + this.incident?.defName);
			sb.AppendLine("    minSpacingDays: " + this.minSpacingDays);
			sb.AppendLine("    mtbDays: " + this.mtbDays);
			sb.AppendLine("    applyCaravanVisibility: " + this.applyCaravanVisibility);
			sb.AppendLine("    mtbDaysFactorByDaysPassedCurve: " + this.mtbDaysFactorByDaysPassedCurve);
			sb.AppendLine("    baseMtbDaysPerDrill: " + this.baseMtbDaysPerDrill);
			sb.AppendLine("    baseIncidentsPerYear: " + this.baseIncidentsPerYear);
			sb.AppendLine("    minDanger: " + this.minDanger);
			sb.AppendLine("    fullAlliesOnly: " + this.fullAlliesOnly);
			sb.AppendLine("    onDays: " + this.onDays);
			sb.AppendLine("    offDays: " + this.offDays);
			sb.AppendLine("    numIncidentsRange: " + this.numIncidentsRange);
			sb.AppendLine("    acceptFractionByDaysPassedCurve: " + this.acceptFractionByDaysPassedCurve);
			sb.AppendLine("    acceptPercentFactorPerThreatPointsCurve: " + this.acceptPercentFactorPerThreatPointsCurve);
			sb.AppendLine("    applyRaidBeaconThreatMtbFactor: " + this.applyRaidBeaconThreatMtbFactor);
			sb.AppendLine("    forceRaidEnemyBeforeDaysPassed: " + this.forceRaidEnemyBeforeDaysPassed);
			sb.Append("    categoryWeights: ");
			this.categoryWeights?.ForEach(v => sb.Append(v.defName + "=" + v.value + ", "));
			sb.AppendLine();
			sb.AppendLine("    maxThreatBigIntervalDays: " + this.maxThreatBigIntervalDays);
			sb.AppendLine("    randomPointsFactorRange: " + this.randomPointsFactorRange);
			sb.AppendLine("    skipThreatBigIfRaidBeacon: " + this.skipThreatBigIfRaidBeacon);
			sb.AppendLine("    delayTicks: " + this.delayTicks);
			return sb.ToString();
		}

		public override int GetHashCode()
		{
			return GetHashCode(this);
		}

		public static StorytellerCompProperties CreateStorytellerCompProperties(StoryTellerCompPropertyTypes type)
		{
			switch (type)
			{
				case StoryTellerCompPropertyTypes.CategoryIndividualMTBByBiome:
					return new StorytellerCompProperties_CategoryIndividualMTBByBiome();
				case StoryTellerCompPropertyTypes.CategoryMTB:
					return new StorytellerCompProperties_CategoryMTB();
				case StoryTellerCompPropertyTypes.ClassicIntro:
					return new StorytellerCompProperties_ClassicIntro();
				case StoryTellerCompPropertyTypes.DeepDrillInfestation:
					return new StorytellerCompProperties_DeepDrillInfestation();
				case StoryTellerCompPropertyTypes.Disease:
					return new StorytellerCompProperties_Disease();
				case StoryTellerCompPropertyTypes.FactionInteraction:
					return new StorytellerCompProperties_FactionInteraction();
				case StoryTellerCompPropertyTypes.JourneyOffer:
					return new StorytellerCompProperties_JourneyOffer();
				case StoryTellerCompPropertyTypes.OnOffCycle:
					return new StorytellerCompProperties_OnOffCycle();
				case StoryTellerCompPropertyTypes.RandomMain:
					return new StorytellerCompProperties_RandomMain();
				case StoryTellerCompPropertyTypes.ShipChunkDrop:
					return new StorytellerCompProperties_ShipChunkDrop();
				case StoryTellerCompPropertyTypes.SingleMTB:
					return new StorytellerCompProperties_SingleMTB();
				case StoryTellerCompPropertyTypes.Triggered:
					return new StorytellerCompProperties_Triggered();
			}
			return null;
		}

		public static StoryTellerCompPropertyTypes GetType(string compClass)
		{
			if (compClass == null)
				return StoryTellerCompPropertyTypes.None;
			foreach (var v in Enum.GetValues(typeof(StoryTellerCompPropertyTypes)))
				if (compClass.IndexOf(v.ToString()) != -1)
					return (StoryTellerCompPropertyTypes)v;
			Log.Error("Could not find type " + compClass);
			return StoryTellerCompPropertyTypes.None;
		}

		public static string GetLabel(StorytellerCompPropertiesStats s)
		{
			StringBuilder sb = new StringBuilder(GetType(s.compClass).ToString());
			IncidentCategoryDef cd = s.category?.Def;
			IncidentDef id = s.incident?.Def;
			if (cd != null)
			{
				sb.Append(" - ");
				sb.Append(Util.GetDefLabel(cd));
			}
			if (id != null)
			{
				sb.Append(" - ");
				sb.Append(Util.GetDefLabel(id));
			}
			return sb.ToString();
		}

		public static string GetLabel(StorytellerCompProperties p)
		{
			if (p == null)
			{
				Log.Warning("Cannot get label for null property");
				return null;
			}
			
			StringBuilder sb = new StringBuilder(GetType(p.compClass.Name).ToString());
			IncidentCategoryDef cd = GetCategory(p);
			IncidentDef id = GetIncident(p);
			if (cd != null)
			{
				sb.Append(" - ");
				sb.Append(Util.GetDefLabel(cd));
			}
			if (id != null)
			{
				sb.Append(" - ");
				sb.Append(Util.GetDefLabel(id));
			}
			return sb.ToString();
		}

		public static int GetHashCode(StorytellerCompPropertiesStats s)
		{
			StringBuilder sb = new StringBuilder(GetLabel(s));
			s.allowedTargetTags?.ForEach(v => sb.Append(v.defName));
			return sb.ToString().GetHashCode();
		}

		public static int GetHashCode(StorytellerCompProperties p)
		{
			StringBuilder sb = new StringBuilder(GetLabel(p));
			p.allowedTargetTags?.ForEach(v => sb.Append(v.defName));
			return sb.ToString().GetHashCode();
		}

		public static IncidentCategoryDef GetCategory(StorytellerCompProperties_OnOffCycle s)
		{
			return (IncidentCategoryDef)typeof(StorytellerCompProperties_OnOffCycle).GetField("category", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(s);
		}

		public static void SetCategory(StorytellerCompProperties_OnOffCycle s, IncidentCategoryDef v)
		{
			typeof(StorytellerCompProperties_OnOffCycle).GetField("category", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(s, v);
		}

		public static bool HasCategory(StorytellerCompProperties p)
		{
			return
				p is StorytellerCompProperties_CategoryIndividualMTBByBiome ||
				p is StorytellerCompProperties_CategoryMTB ||
				p is StorytellerCompProperties_Disease ||
				p is StorytellerCompProperties_OnOffCycle;
		}

		public static bool HasIncident(StorytellerCompProperties p)
		{
			return
				p is StorytellerCompProperties_FactionInteraction ||
				p is StorytellerCompProperties_SingleMTB ||
				p is StorytellerCompProperties_Triggered ||
				p is StorytellerCompProperties_OnOffCycle;
		}

		public static IncidentCategoryDef GetCategory(StorytellerCompProperties p)
		{
			if (p is StorytellerCompProperties_CategoryIndividualMTBByBiome f)
				return f.category;
			if (p is StorytellerCompProperties_CategoryMTB s)
				return s.category;
			if (p is StorytellerCompProperties_Disease t)
				return t.category;
			if (p is StorytellerCompProperties_OnOffCycle o)
				return GetCategory(o);
			return null;
		}

		public static void SetCategory(StorytellerCompProperties p, IncidentCategoryDef d)
		{
			if (p is StorytellerCompProperties_CategoryIndividualMTBByBiome f)
				f.category = d;
			else if (p is StorytellerCompProperties_CategoryMTB s)
				s.category = d;
			else if (p is StorytellerCompProperties_Disease t)
				t.category = d;
			else if (p is StorytellerCompProperties_OnOffCycle o)
				SetCategory(o, d);
		}

		public static IncidentDef GetIncident(StorytellerCompProperties p)
		{
			if (p is StorytellerCompProperties_FactionInteraction f)
				return f.incident;
			if (p is StorytellerCompProperties_SingleMTB s)
				return s.incident;
			if (p is StorytellerCompProperties_Triggered t)
				return t.incident;
			if (p is StorytellerCompProperties_OnOffCycle o)
				return o.incident;
			return null;
		}

		public static void SetIncident(StorytellerCompProperties p, IncidentDef d)
		{
			if (p is StorytellerCompProperties_FactionInteraction f)
				f.incident = d;
			else if (p is StorytellerCompProperties_SingleMTB s)
				s.incident = d;
			else if (p is StorytellerCompProperties_Triggered t)
				t.incident = d;
			else if (p is StorytellerCompProperties_OnOffCycle o)
				o.incident = d;
		}
	}
}
