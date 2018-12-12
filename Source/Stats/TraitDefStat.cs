using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace InGameDefEditor.Stats
{
	public class TraitDefStat : DefStat<TraitDef>, IParentStat
	{
		public float commonality;
		public float commonalityFemale;
		public bool allowOnHostileSpawn;
		public WorkTags requiredWorkTags;
		public WorkTags disabledWorkTags;

		public List<DefStat<WorkTypeDef>> disabledWorkTypes;
		public List<DefStat<WorkTypeDef>> requiredWorkTypes;
		public List<DefStat<TraitDef>> conflictingTraits;

		public List<TraitDegreeDataStats> degreeDatas;

		public string UniqueKey => base.defName;

		public TraitDefStat() : base() { }
		public TraitDefStat(TraitDef d) : base(d)
		{
			this.commonality = GetCommonality(d);
			this.commonalityFemale = GetCommonalityFemale(d);
			this.allowOnHostileSpawn = d.allowOnHostileSpawn;
			this.requiredWorkTags = d.requiredWorkTags;
			this.disabledWorkTags = d.disabledWorkTags;

			this.disabledWorkTypes = Util.CreateDefStatList(d.disabledWorkTypes);
			this.conflictingTraits = Util.CreateDefStatList(d.conflictingTraits);
			this.requiredWorkTypes = Util.CreateDefStatList(d.requiredWorkTypes);

			Util.Populate(out this.degreeDatas, d.degreeDatas, (v) => new TraitDegreeDataStats(v));
		}

		public override bool Initialize()
		{
			if (!base.Initialize())
				return false;
		
			Util.InitializeDefStat(this.disabledWorkTypes);
			Util.InitializeDefStat(this.requiredWorkTypes);
			Util.InitializeDefStat(this.conflictingTraits);
			if (this.degreeDatas != null)
				this.degreeDatas.ForEach((v) => v.Initialize());

			return true;
		}

		public void ApplyStats(object to)
		{
			if (to is TraitDef d)
			{
				SetCommonality(d, this.commonality);
				SetCommonalityFemale(d, this.commonalityFemale);
				d.allowOnHostileSpawn = this.allowOnHostileSpawn;
				d.requiredWorkTags = this.requiredWorkTags;
				d.disabledWorkTags = this.disabledWorkTags;

				Util.Populate(out d.disabledWorkTypes, this.disabledWorkTypes, v => v.Def);
				Util.Populate(out d.conflictingTraits, this.conflictingTraits, v => v.Def);
				Util.Populate(out d.requiredWorkTypes, this.requiredWorkTypes, v => v.Def);

				if (this.degreeDatas != null && d.degreeDatas != null)
				{
					Dictionary<string, TraitDegreeData> lookup = new Dictionary<string, TraitDegreeData>();
					d.degreeDatas.ForEach(v => lookup.Add(v.label, v));
					this.degreeDatas.ForEach(v =>
					{
						if (lookup.TryGetValue(v.label, out TraitDegreeData data))
							v.ApplyStats(data);
						else
							Log.Warning("Unable to find degree data [" + v.label + "] for trait [" + this.defName + "]");
					});
					lookup.Clear();
					lookup = null;
				}
			}
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj) && 
				obj is TraitDefStat s)
			{
				return
					this.commonality == s.commonality &&
					this.commonalityFemale == s.commonalityFemale &&
					this.allowOnHostileSpawn == s.allowOnHostileSpawn &&
					this.requiredWorkTags == s.requiredWorkTags &&
					this.disabledWorkTags == s.disabledWorkTags &&
					Util.AreEqual(this.disabledWorkTypes, s.disabledWorkTypes) &&
					Util.AreEqual(this.conflictingTraits, s.conflictingTraits) &&
					Util.AreEqual(this.requiredWorkTypes, s.requiredWorkTypes) &&
					Util.AreEqual(this.degreeDatas, s.degreeDatas, null);
			}
			return false;
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static float GetCommonality(TraitDef d)
		{
			return (float)typeof(TraitDef).GetField("commonality", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(d);
		}

		public static void SetCommonality(TraitDef d, float f)
		{
			typeof(TraitDef).GetField("commonality", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(d, f);
		}

		public static float GetCommonalityFemale(TraitDef d)
		{
			return (float)typeof(TraitDef).GetField("commonalityFemale", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(d);
		}

		public static void SetCommonalityFemale(TraitDef d, float f)
		{
			typeof(TraitDef).GetField("commonalityFemale", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(d, f);
		}
	}
}
