using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats
{
	[Serializable]
	public class BackstoryStats : IParentStat
	{
		[XmlIgnore]
		private Backstory backstory;
		public Backstory Backstory { get => this.backstory; }

		[XmlElement(IsNullable = false)]
		public string identifier;
		public bool shuffleable;

		public BackstorySlot slot;
		public WorkTags workDisables;
		public WorkTags requiredWorkTags;

		public DefStat<BodyTypeDef> bodyTypeGlobal;
		public DefStat<BodyTypeDef> bodyTypeMale;
		public DefStat<BodyTypeDef> bodyTypeFemale;

		public List<IntValueDefStat<SkillDef>> skillGains;
		public List<IntValueDefStat<TraitDef>> forcedTraits;
		public List<IntValueDefStat<TraitDef>> disallowedTraits;

		public List<string> spawnCategories = new List<string>();

		public string UniqueKey => identifier;

		public BackstoryStats() { }
		public BackstoryStats(Backstory b)
		{
			this.backstory = b;
			this.identifier = b.identifier;
			this.shuffleable = b.shuffleable;
			this.slot = b.slot;
			this.workDisables = b.workDisables;
			this.requiredWorkTags = b.requiredWorkTags;
			Util.AssignDefStat(GetBodyTypeGlobal(b), out this.bodyTypeGlobal);
			Util.AssignDefStat(GetBodyTypeMale(b), out this.bodyTypeMale);
			Util.AssignDefStat(GetBodyTypeFemale(b), out this.bodyTypeFemale);
			Util.Populate(out this.skillGains, b.skillGainsResolved, v => new IntValueDefStat<SkillDef>(v.Key, v.Value));
			Util.Populate(out this.forcedTraits, b.forcedTraits, v => new IntValueDefStat<TraitDef>(v.def, v.degree));
			Util.Populate(out this.disallowedTraits, b.disallowedTraits, v => new IntValueDefStat<TraitDef>(v.def, v.degree));
			Util.Populate(out this.spawnCategories, b.spawnCategories);
		}

		public bool Initialize()
		{
			if (!BackstoryDatabase.TryGetWithIdentifier(this.identifier, out this.backstory, false))
			{
				Log.Warning("Unable to load backstory " + this.identifier);
				return false;
			}

			Util.InitializeDefStat(this.bodyTypeGlobal);
			Util.InitializeDefStat(this.bodyTypeFemale);
			Util.InitializeDefStat(this.bodyTypeMale);

			this.skillGains?.ForEach(v => v.Initialize());
			this.forcedTraits?.ForEach(v => v.Initialize());
			this.disallowedTraits?.ForEach(v => v.Initialize());
			return true;
		}

		public void ApplyStats(object t)
		{
			if (t is Backstory to)
			{
				to.identifier = this.identifier;
				to.shuffleable = this.shuffleable;
				to.slot = this.slot;
				to.workDisables = this.workDisables;
				to.requiredWorkTags = this.requiredWorkTags;
				SetBodyTypeGlobal(to, Util.AssignDef(this.bodyTypeGlobal));
				SetBodyTypeMale(to, Util.AssignDef(this.bodyTypeMale));
				SetBodyTypeFemale(to, Util.AssignDef(this.bodyTypeFemale));
				if (this.skillGains != null && to.skillGainsResolved != null)
				{
					to.skillGainsResolved.Clear();
					foreach (var v in this.skillGains)
						to.skillGainsResolved.Add(v.Def, v.value);
				}
				Util.Populate(out to.forcedTraits, this.forcedTraits, v => new TraitEntry(v.Def, v.value));
				Util.Populate(out to.disallowedTraits, this.disallowedTraits, v => new TraitEntry(v.Def, v.value));
				Util.Populate(out to.spawnCategories, this.spawnCategories);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is BackstoryStats b)
			{
				return
					object.Equals(this.identifier, b.identifier) &&
					this.shuffleable == b.shuffleable &&
					this.slot == b.slot &&
					this.workDisables == b.workDisables &&
					this.requiredWorkTags == b.requiredWorkTags &&
					object.Equals(this.bodyTypeGlobal, b.bodyTypeGlobal) &&
					object.Equals(this.bodyTypeMale, b.bodyTypeMale) &&
					object.Equals(this.bodyTypeFemale, b.bodyTypeFemale) &&
					Util.AreEqual(this.skillGains, b.skillGains, v => v.defName.GetHashCode()) &&
					Util.AreEqual(this.forcedTraits, b.forcedTraits, v => v.defName.GetHashCode()) &&
					Util.AreEqual(this.disallowedTraits, b.disallowedTraits, v => v.defName.GetHashCode()) &&
					Util.AreEqual(this.spawnCategories, b.spawnCategories, v => v.GetHashCode());
			}
			return false;
		}

		public override string ToString()
		{
			return
				this.GetType().Name + Environment.NewLine +
				"    identifier: " + identifier;
		}

		public override int GetHashCode()
		{
			return this.identifier.GetHashCode();
		}

		public static BodyTypeDef GetBodyTypeGlobal(Backstory b)
		{
			return (BodyTypeDef)typeof(Backstory).GetField("bodyTypeGlobalResolved", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(b);
		}

		public static void SetBodyTypeGlobal(Backstory b, BodyTypeDef v)
		{
			typeof(Backstory).GetField("bodyTypeGlobalResolved", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(b, v);
		}

		public static BodyTypeDef GetBodyTypeMale(Backstory b)
		{
			return (BodyTypeDef)typeof(Backstory).GetField("bodyTypeMaleResolved", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(b);
		}

		public static void SetBodyTypeMale(Backstory b, BodyTypeDef v)
		{
			typeof(Backstory).GetField("bodyTypeMaleResolved", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(b, v);
		}

		public static BodyTypeDef GetBodyTypeFemale(Backstory b)
		{
			return (BodyTypeDef)typeof(Backstory).GetField("bodyTypeFemaleResolved", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(b);
		}

		public static void SetBodyTypeFemale(Backstory b, BodyTypeDef v)
		{
			typeof(Backstory).GetField("bodyTypeFemaleResolved", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(b, v);
		}
	}
}