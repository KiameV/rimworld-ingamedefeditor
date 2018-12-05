using InGameDefEditor.Stats;
using RimWorld;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Verse;
using InGameDefEditor.Stats.DefStat;
using System.Collections.Generic;

namespace InGameDefEditor
{
    static class IOUtil
    {
        private static bool hasLoaded = false;

        public static void LoadData()
        {
            Defs.Initialize();
            if (!hasLoaded)
            {
                try
                {
					try
					{
						if (File.Exists(GetStatsPath()))
							File.Delete(GetStatsPath());
					}
					catch
					{
						Log.Message("Unable to delete stats.xml");
					}

					if (Load(DefType.Apparel, out RootApparel ra))
						ra?.stats.ForEach((d) => Initialize(d));

					if (Load(DefType.Weapon, out RootWeapons rw))
						rw?.stats.ForEach((d) => Initialize(d));

					if (Load(DefType.Projectile, out RootProjectiles rp))
						rp?.stats.ForEach((d) => Initialize(d));

					if (Load(DefType.Biome, out RootBiomes rb))
						rb?.stats.ForEach((d) => Initialize(d));

					if (Load(DefType.Recipe, out RootRecipe rr))
						rr?.stats.ForEach((d) => Initialize(d));

					if (Load(DefType.Trait, out RootTraits rtr))
						rtr?.stats.ForEach((d) => Initialize(d));

					if (Load(DefType.Thought, out RootThoughts rth))
						rth?.stats.ForEach((d) => Initialize(d));

					if (Load(DefType.StoryTeller, out RootStoryTeller rst))
						rst?.stats.ForEach((d) => Initialize(d));

					if (Load(DefType.Difficulty, out RootDifficulty dif))
						dif?.stats.ForEach((d) => Initialize(d));
				}
                catch(Exception e)
                {
					Log.Warning(e.Message);
                }
                finally
                {
                    hasLoaded = true;
                    DefLookupUtil.ClearDefDic();
                    Log.Message("InGameDefEditor".Translate() + ": Settings Applied");
                }
            }
        }
		
		public static void SaveData()
		{
			Util.Populate(out List<ThingDefStats> ap, Defs.ApparelDefs.Values, (v) => HasChanged(new ThingDefStats(v)), false);
			Save(DefType.Apparel, new RootApparel() { stats = ap });

			Util.Populate(out List<ThingDefStats> we, Defs.WeaponDefs.Values, (v) => HasChanged(new ThingDefStats(v)), false);
			Save(DefType.Weapon, new RootWeapons() { stats = we });

			Util.Populate(out List<ProjectileDefStats> pr, Defs.ProjectileDefs.Values, (v) => HasChanged(new ProjectileDefStats(v)), false);
			Save(DefType.Projectile, new RootProjectiles() { stats = pr });

			Util.Populate(out List<BiomeDefStats> bi, Defs.BiomeDefs.Values, (v) => HasChanged(new BiomeDefStats(v)), false);
			Save(DefType.Biome, new RootBiomes() { stats = bi });

			// TODO
			//Util.Populate(out List<RecipeDefStats> re, Defs.RecipeDefs.Values, (v) => HasChanged(new RecipeDefStats(v)), false);
			//Save(DefType.Recipe, new RootRecipe() { stats = re });

			Util.Populate(out List<TraitDefStat> traits, Defs.TraitDefs.Values, (v) => HasChanged(new TraitDefStat(v)), false);
			Save(DefType.Trait, new RootTraits() { stats = traits });

			Util.Populate(out List<ThoughtDefStats> thoughts, Defs.ThoughtDefs.Values, (v) => HasChanged(new ThoughtDefStats(v)), false);
			Save(DefType.Thought, new RootThoughts() { stats = thoughts });

			Util.Populate(out List<StoryTellerDefStats> storyTellers, Defs.StoryTellerDefs.Values, (v) => HasChanged(new StoryTellerDefStats(v)), false);
			Save(DefType.StoryTeller, new RootStoryTeller() { stats = storyTellers });

			Util.Populate(out List<DifficultyDefStat> difficulties, Defs.DifficultyDefs.Values, (v) => HasChanged(new DifficultyDefStat(v)), false);
			Save(DefType.Difficulty, new RootDifficulty() { stats = difficulties });
		}

		private static string basePath = null;
        private static string GetStatsPath()
        {
			if (basePath == null)
				basePath =(string)typeof(GenFilePaths).GetMethod("FolderUnderSaveData", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]
				{
					"InGameDefEditor"
				});
            return basePath + "/stats.xml";
		}

		private static string GetStatsPath(DefType type)
		{
			if (basePath == null)
				basePath = (string)typeof(GenFilePaths).GetMethod("FolderUnderSaveData", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]
				{
					"InGameDefEditor"
				});
			return basePath + "/" + type.ToString() + ".xml";
		}

		private static bool Load<T>(DefType type, out T t)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			FileStream fs = null;
			string path = GetStatsPath(type);

			if (!File.Exists(path))
			{
				t = default(T);
				return false;
			}

			try
			{
				fs = new FileStream(path, FileMode.Open);
				t = (T)serializer.Deserialize(fs);
			}
			catch (Exception e)
			{
				Log.Error(
					"Failed to load settings for Def type " + type.ToString() + Environment.NewLine + e.Message);
				t = default(T);
				return false;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
			return true;
		}

		private static bool Save<T>(DefType type, T t)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			FileStream fs = null;
			try
			{
				fs = new FileStream(GetStatsPath(type), FileMode.Create);
				serializer.Serialize(fs, t);
			}
			catch (Exception e)
			{
				Log.Error(
					"Failed to save settings for Def type " + type.ToString() + Environment.NewLine + e.GetType().Name + " -- " + e.Message);
				return false;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
			return true;
		}

		private static void Initialize<D>(DefStat<D> s) where D : Def, new()
		{
			if (s != null)
			{
				try
				{
					if (s.Initialize() &&
						s is IParentStat p)
					{
						try
						{
							p.ApplyStats(s.Def);
						}
						catch (Exception e)
						{
							Log.Error("Failed to apply settings to " + s.defName + " due to " + e.Message);
						}
					}
					else
					{
						Log.Warning("Unable to apply settings to " + s.defName);
					}
				}
				catch (Exception e)
				{
					Log.Error("Failed to load " + s.defName + " due to " + e.Message);
				}
			}
		}

		private static T HasChanged<T>(T d) where T : IParentStat
		{
			if (Backup.HasChanged(d))
				return d;
			return default(T);
		}
	}
}