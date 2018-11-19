using InGameDefEditor.Stats.DefStat;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	public class ThingFilterStats : IStat<ThingFilter>
	{
		/*private FloatRange allowedHitPointsPercents = FloatRange.ZeroToOne;
		public bool allowedHitPointsConfigurable;
		public bool allowedQualitiesConfigurable;
		private FoodPreferability disallowWorsePreferability;
		private bool disallowInedibleByHuman;
		private float disallowCheaperThan;*/

		//private HashSet<DefStat<ThingDef>> allowedDefs;
		private List<DefStat<ThingDef>> disallowedThingDefs;
		private HashSet<DefStat<ThingDef>> thingDefs;
		private List<string> categories;
		private List<string> disallowedCategories;
		private List<string> specialFiltersToAllow;
		private List<string> specialFiltersToDisallow;

		/*
		private QualityRangeStats allowedQualities;
		private List<DefStat<ThingDef>> allowAllWhoCanMake;
		private List<DefStat<StuffCategoryDef>> stuffCategoriesToAllow;
		private List<DefStat<SpecialThingFilterDef>> disallowedSpecialFilters;
		private List<string> tradeTagsToAllow;
		private List<string> tradeTagsToDisallow;
		private List<string> thingSetMakerTagsToAllow;
		private List<string> thingSetMakerTagsToDisallow;*/

		//private Type allowWithComp;
		//private Type disallowWithComp;

		public ThingFilterStats() { }
		public ThingFilterStats(ThingFilter f)
		{
			/*this.allowedHitPointsPercents = f.AllowedHitPointsPercents;
			this.allowedHitPointsConfigurable = f.allowedHitPointsConfigurable;
			this.allowedQualitiesConfigurable = f.allowedQualitiesConfigurable;
			this.disallowWorsePreferability = GetDisallowWorsePreferability(f);
			this.disallowInedibleByHuman = GetDisallowInedibleByHuman(f);
			this.disallowCheaperThan = GetDisallowCheaperThan(f);
			this.allowedDefs = Util.CreateDefStatHashSet(GetAllowedDefs(f));*/

			/*if (GetAllowedQualities(f) != null)
				this.allowedQualities = new QualityRangeStats(GetAllowedQualities(f));*/

			this.disallowedThingDefs = Util.CreateDefStatList(GetDisallowedThingDefs(f));
			/*this.allowAllWhoCanMake = Util.CreateDefStatList(GetAllowAllWhoCanMake(f));
			this.stuffCategoriesToAllow = Util.CreateDefStatList(GetStuffCategoriesToAllow(f));
			this.disallowedSpecialFilters = Util.CreateDefStatList(GetDisallowedSpecialFilters(f));*/
			this.thingDefs = Util.CreateDefStatHashSet(GetThingDefs(f));
			if (GetAllowedDefs(f) != null)
				foreach (var v in GetAllowedDefs(f))
					this.thingDefs.Add(new DefStat<ThingDef>(v));
			this.categories = Util.CreateList(GetCategories(f));
			/*this.tradeTagsToAllow = Util.CreateList(GetTradeTagsToAllow(f));
			this.tradeTagsToDisallow = Util.CreateList(GetTradeTagsToDisallow(f));
			this.thingSetMakerTagsToAllow = Util.CreateList(GetThingSetMakerTagsToAllow(f));
			this.thingSetMakerTagsToDisallow = Util.CreateList(GetThingSetMakerTagsToDisallow(f));*/
			this.disallowedCategories = Util.CreateList(GetDisallowedCategories(f));
			this.specialFiltersToAllow = Util.CreateList(GetSpecialFiltersToAllow(f));
			this.specialFiltersToDisallow = Util.CreateList(GetSpecialFiltersToDisallow(f));
		}

		public bool Initialize()
		{
			//Util.InitializeDefStat(this.allowedDefs);
			Util.InitializeDefStat(this.disallowedThingDefs);
			/*Util.InitializeDefStat(this.allowAllWhoCanMake);
			Util.InitializeDefStat(this.stuffCategoriesToAllow);
			Util.InitializeDefStat(this.disallowedSpecialFilters);*/
			Util.InitializeDefStat(this.thingDefs);
			return true;
		}

		public void ApplyStats(ThingFilter to)
		{
			/*to.AllowedHitPointsPercents = this.allowedHitPointsPercents;
			to.allowedHitPointsConfigurable = this.allowedHitPointsConfigurable;
			to.allowedQualitiesConfigurable = this.allowedQualitiesConfigurable;
			SetDisallowWorsePreferability(to, this.disallowWorsePreferability);
			SetDisallowInedibleByHuman(to, this.disallowInedibleByHuman);
			SetDisallowCheaperThan(to, this.disallowCheaperThan);
			SetAllowedDefs(to, Util.ConvertDefStats(this.allowedDefs));*/

			/*if (this.allowedQualities != null)
				to.AllowedQualityLevels = new QualityRange(this.allowedQualities.Min, this.allowedQualities.Max);
			*/
			SetDisallowedThingDefs(to, Util.ConvertDefStats(this.disallowedThingDefs));
			/*SetAllowAllWhoCanMake(to, Util.ConvertDefStats(this.allowAllWhoCanMake));
			SetStuffCategoriesToAllow(to, Util.ConvertDefStats(this.stuffCategoriesToAllow));
			SetDisallowedSpecialFilters(to, Util.ConvertDefStats(this.disallowedSpecialFilters));*/
			SetThingDefs(to, new List<ThingDef>(Util.ConvertDefStats(this.thingDefs)));

			SetCategories(to, Util.CreateList(this.categories));
			/*SetTradeTagsToAllow(to, Util.CreateList(this.tradeTagsToAllow));
			SetTradeTagsToDisallow(to, Util.CreateList(this.tradeTagsToDisallow));
			SetThingSetMakerTagsToAllow(to, Util.CreateList(this.thingSetMakerTagsToAllow));
			SetThingSetMakerTagsToDisallow(to, Util.CreateList(this.thingSetMakerTagsToDisallow));*/
			SetDisallowedCategories(to, Util.CreateList(this.disallowedCategories));
			SetSpecialFiltersToAllow(to, Util.CreateList(this.specialFiltersToAllow));
			SetSpecialFiltersToDisallow(to, Util.CreateList(this.specialFiltersToDisallow));

			to.ResolveReferences();
		}
		
		public void PreSave(ThingFilter tf)
		{
			tf.ResolveReferences();
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is ThingFilterStats f)
			{
				return 
					/*this.allowedHitPointsPercents == f.allowedHitPointsPercents && 
					this.allowedHitPointsConfigurable == f.allowedHitPointsConfigurable &&
					this.allowedQualitiesConfigurable == f.allowedQualitiesConfigurable &&
					this.disallowWorsePreferability == f.disallowWorsePreferability  &&
					this.disallowInedibleByHuman == f.disallowInedibleByHuman &&
					this.disallowCheaperThan == f.disallowCheaperThan &&*/
					//object.Equals(this.allowedQualities, f.allowedQualities) &&
					//Util.AreEqual(this.allowedDefs, f.allowedDefs) &&
					Util.AreEqual(this.disallowedThingDefs, f.disallowedThingDefs) &&
					//Util.AreEqual(this.allowAllWhoCanMake, f.allowAllWhoCanMake) &&
					//Util.AreEqual(this.stuffCategoriesToAllow, f.stuffCategoriesToAllow) &&
					//Util.AreEqual(this.disallowedSpecialFilters, f.disallowedSpecialFilters) &&
					Util.AreEqual(this.thingDefs, f.thingDefs) &&
					Util.AreEqual(this.categories, f.categories) &&
					/*Util.AreEqual(this.tradeTagsToAllow, f.tradeTagsToAllow) &&
					Util.AreEqual(this.tradeTagsToDisallow, f.tradeTagsToDisallow) &&
					Util.AreEqual(this.thingSetMakerTagsToAllow, f.thingSetMakerTagsToAllow) &&*/
					Util.AreEqual(this.disallowedCategories, f.disallowedCategories) &&
					Util.AreEqual(this.specialFiltersToAllow, f.specialFiltersToAllow) &&
					Util.AreEqual(this.specialFiltersToDisallow, f.specialFiltersToDisallow);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static QualityRange GetAllowedQualities(ThingFilter f)
		{
			return (QualityRange)typeof(ThingFilter).GetField("allowedQualities", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetAllowedQualities(ThingFilter f, QualityRange qr)
		{
			typeof(ThingFilter).GetField("allowedQualities", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, qr);
		}

		public static FoodPreferability GetDisallowWorsePreferability(ThingFilter f)
		{
			return (FoodPreferability)typeof(ThingFilter).GetField("disallowWorsePreferability", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetDisallowWorsePreferability(ThingFilter f, FoodPreferability fp)
		{
			typeof(ThingFilter).GetField("disallowWorsePreferability", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, fp);
		}

		public static bool GetDisallowInedibleByHuman(ThingFilter f)
		{
			return (bool)typeof(ThingFilter).GetField("disallowInedibleByHuman", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetDisallowInedibleByHuman(ThingFilter f, bool b)
		{
			typeof(ThingFilter).GetField("disallowInedibleByHuman", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, b);
		}

		public static float GetDisallowCheaperThan(ThingFilter f)
		{
			return (float)typeof(ThingFilter).GetField("disallowCheaperThan", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetDisallowCheaperThan(ThingFilter f, float fl)
		{
			typeof(ThingFilter).GetField("disallowCheaperThan", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, fl);
		}

		public static HashSet<ThingDef> GetAllowedDefs(ThingFilter f)
		{
			return (HashSet<ThingDef>)typeof(ThingFilter).GetField("allowedDefs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetAllowedDefs(ThingFilter f, HashSet<ThingDef> v)
		{
			typeof(ThingFilter).GetField("allowedDefs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<ThingDef> GetDisallowedThingDefs(ThingFilter f)
		{
			return (List<ThingDef>)typeof(ThingFilter).GetField("disallowedThingDefs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetDisallowedThingDefs(ThingFilter f, List<ThingDef> v)
		{
			typeof(ThingFilter).GetField("disallowedThingDefs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<ThingDef> GetAllowAllWhoCanMake(ThingFilter f)
		{
			return (List<ThingDef>)typeof(ThingFilter).GetField("allowAllWhoCanMake", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetAllowAllWhoCanMake(ThingFilter f, List<ThingDef> v)
		{
			typeof(ThingFilter).GetField("allowAllWhoCanMake", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<StuffCategoryDef> GetStuffCategoriesToAllow(ThingFilter f)
		{
			return (List<StuffCategoryDef>)typeof(ThingFilter).GetField("stuffCategoriesToAllow", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetStuffCategoriesToAllow(ThingFilter f, List<StuffCategoryDef> v)
		{
			typeof(ThingFilter).GetField("stuffCategoriesToAllow", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<SpecialThingFilterDef> GetDisallowedSpecialFilters(ThingFilter f)
		{
			return (List<SpecialThingFilterDef>)typeof(ThingFilter).GetField("disallowedSpecialFilters", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetDisallowedSpecialFilters(ThingFilter f, List<SpecialThingFilterDef> v)
		{
			typeof(ThingFilter).GetField("disallowedSpecialFilters", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<ThingDef> GetThingDefs(ThingFilter f)
		{
			return (List<ThingDef>)typeof(ThingFilter).GetField("thingDefs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetThingDefs(ThingFilter f, List<ThingDef> v)
		{
			typeof(ThingFilter).GetField("thingDefs", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<string> GetCategories(ThingFilter f)
		{
			return (List<string>)typeof(ThingFilter).GetField("categories", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetCategories(ThingFilter f, List<string> v)
		{
			typeof(ThingFilter).GetField("categories", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<string> GetTradeTagsToAllow(ThingFilter f)
		{
			return (List<string>)typeof(ThingFilter).GetField("tradeTagsToAllow", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetTradeTagsToAllow(ThingFilter f, List<string> v)
		{
			typeof(ThingFilter).GetField("tradeTagsToAllow", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<string> GetTradeTagsToDisallow(ThingFilter f)
		{
			return (List<string>)typeof(ThingFilter).GetField("tradeTagsToDisallow", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetTradeTagsToDisallow(ThingFilter f, List<string> v)
		{
			typeof(ThingFilter).GetField("tradeTagsToDisallow", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<string> GetThingSetMakerTagsToAllow(ThingFilter f)
		{
			return (List<string>)typeof(ThingFilter).GetField("thingSetMakerTagsToAllow", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetThingSetMakerTagsToAllow(ThingFilter f, List<string> v)
		{
			typeof(ThingFilter).GetField("thingSetMakerTagsToAllow", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<string> GetThingSetMakerTagsToDisallow(ThingFilter f)
		{
			return (List<string>)typeof(ThingFilter).GetField("thingSetMakerTagsToDisallow", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetThingSetMakerTagsToDisallow(ThingFilter f, List<string> v)
		{
			typeof(ThingFilter).GetField("thingSetMakerTagsToDisallow", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<string> GetDisallowedCategories(ThingFilter f)
		{
			return (List<string>)typeof(ThingFilter).GetField("disallowedCategories", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetDisallowedCategories(ThingFilter f, List<string> v)
		{
			typeof(ThingFilter).GetField("disallowedCategories", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<string> GetSpecialFiltersToAllow(ThingFilter f)
		{
			return (List<string>)typeof(ThingFilter).GetField("specialFiltersToAllow", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetSpecialFiltersToAllow(ThingFilter f, List<string> v)
		{
			typeof(ThingFilter).GetField("specialFiltersToAllow", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}

		public static List<string> GetSpecialFiltersToDisallow(ThingFilter f)
		{
			return (List<string>)typeof(ThingFilter).GetField("specialFiltersToDisallow", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(f);
		}

		public static void SetSpecialFiltersToDisallow(ThingFilter f, List<string> v)
		{
			typeof(ThingFilter).GetField("specialFiltersToDisallow", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(f, v);
		}
	}
}
