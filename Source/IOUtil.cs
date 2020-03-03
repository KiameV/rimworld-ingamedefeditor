using InGameDefEditor.Stats;
using RimWorld;
using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Verse;
using InGameDefEditor.Stats.DefStat;
using System.Collections.Generic;
using System.Text;

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

					StringBuilder sb = new StringBuilder("InGameDefEditor".Translate() + ": Loading Def Settings\n");

					if (Load("AutoApplyDefs", out RootAutoApplyDefs raad))
						raad?.autoApplyDefs.ForEach(s => Defs.ApplyStatsAutoThingDefs.Add(s, true));

					if (Load(DefType.Apparel, out RootApparel ra))
						ra?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Weapon, out RootWeapons rw))
						rw?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Projectile, out RootProjectiles rp))
						rp?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Biome, out RootBiomes rb))
						rb?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Recipe, out RootRecipe rr))
						rr?.recipes.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Trait, out RootTraits rtr))
						rtr?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Thought, out RootThoughts rth))
						rth?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.StoryTeller, out RootStoryTeller rst))
						rst?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Difficulty, out RootDifficulty dif))
						dif?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Ingestible, out RootIngestible ing))
						ing?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Mineable, out RootMineable min))
						min?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Backstory, out RootBackstory back))
						back?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Building, out RootBuilding building))
						building?.stats.ForEach((d) => Initialize(d, sb));

					// Do Last
					sb.AppendLine("Disabling the following defs:");
					if (Load("DisabledDefs", out RootDisabledDefs rdd))
					{
						rdd?.disabledThingDefs.ForEach(defName =>
						{
							if (!DefLookupUtil.TryGetDef(defName, out ThingDef def))
							{
								Log.Error("Could not disable ThingDef " + defName);
							}
							else
							{
								Defs.DisabledThingDefs.Add(defName, def);
								typeof(DefDatabase<ThingDef>).GetMethod("Remove", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { def });
								sb.AppendLine("- " + def.defName);
							}
						});
					}
					Log.Message(sb.ToString());
				}
				catch (Exception e)
				{
					Log.Warning("InGameDefEditor".Translate() + ": encountered an error - " + e.Message);
				}
				finally
				{
					hasLoaded = true;
					DefLookupUtil.ClearDefDic();
				}
			}
		}

        public static void SaveData()
        {
            Save("DisabledDefs", new RootDisabledDefs() { disabledThingDefs = new List<string>(Defs.DisabledThingDefs.Keys) });

			var aad = new List<string>();
			foreach (var kv in Defs.ApplyStatsAutoThingDefs)
				if (kv.Value)
					aad.Add(kv.Key);
			Save("AutoApplyDefs", new RootAutoApplyDefs() { autoApplyDefs = aad }); ;

            try
            {
                Util.Populate(out List<ThingDefStats> ap, Defs.ApparelDefs.Values, (v) => HasChanged(new ThingDefStats(v)), false);
                Save(DefType.Apparel, new RootApparel() { stats = ap });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Apparel + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Util.Populate(out List<ThingDefStats> we, Defs.WeaponDefs.Values, (v) => HasChanged(new ThingDefStats(v)), false);
                Save(DefType.Weapon, new RootWeapons() { stats = we });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Weapon + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Util.Populate(out List<ProjectileDefStats> pr, Defs.ProjectileDefs.Values, (v) => HasChanged(new ProjectileDefStats(v)), false);
                Save(DefType.Projectile, new RootProjectiles() { stats = pr });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Projectile + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Util.Populate(out List<BiomeDefStats> bi, Defs.BiomeDefs.Values, (v) => HasChanged(new BiomeDefStats(v)), false);
                Save(DefType.Biome, new RootBiomes() { stats = bi });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Biome + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                //if (Controller.EnableRecipes)
                //{ 
                Util.Populate(out List<RecipeDefStats> re, Defs.RecipeDefs.Values, (v) => HasChanged(new RecipeDefStats(v)), false);
                Save(DefType.Recipe, new RootRecipe() { recipes = re });
                //}
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Recipe + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Util.Populate(out List<TraitDefStat> traits, Defs.TraitDefs.Values, (v) => HasChanged(new TraitDefStat(v)), false);
                Save(DefType.Trait, new RootTraits() { stats = traits });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Trait + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Util.Populate(out List<ThoughtDefStats> thoughts, Defs.ThoughtDefs.Values, (v) => HasChanged(new ThoughtDefStats(v)), false);
                Save(DefType.Thought, new RootThoughts() { stats = thoughts });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Thought + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Util.Populate(out List<StoryTellerDefStats> storyTellers, Defs.StoryTellerDefs.Values, (v) => HasChanged(new StoryTellerDefStats(v)), false);
                Save(DefType.StoryTeller, new RootStoryTeller() { stats = storyTellers });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.StoryTeller + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Util.Populate(out List<DifficultyDefStat> difficulties, Defs.DifficultyDefs.Values, (v) => HasChanged(new DifficultyDefStat(v)), false);
                Save(DefType.Difficulty, new RootDifficulty() { stats = difficulties });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Difficulty + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Util.Populate(out List<ThingDefStats> ingestibles, Defs.IngestibleDefs.Values, v => HasChanged(new ThingDefStats(v)), false);
                Save(DefType.Ingestible, new RootIngestible() { stats = ingestibles });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Apparel + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Util.Populate(out List<ThingDefStats> mineable, Defs.MineableDefs.Values, v => HasChanged(new ThingDefStats(v)), false);
                Save(DefType.Mineable, new RootMineable() { stats = mineable });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Ingestible + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Util.Populate(out List<BackstoryStats> backstories, Defs.Backstories.Values, v => HasChanged(new BackstoryStats(v)), false);
                Save(DefType.Backstory, new RootBackstory() { stats = backstories });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Backstory + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Util.Populate(out List<ThingDefStats> buildings, Defs.BuildingDefs.Values, v => HasChanged(new ThingDefStats(v)), false);
                Save(DefType.Building, new RootBuilding() { stats = buildings });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Building + ".\n" + e.GetType().Name + "\n" + e.Message);
            }
        }

		private static string basePath = null;
		private static string GetStatsPath()
		{
			if (basePath == null)
				basePath = (string)typeof(GenFilePaths).GetMethod("FolderUnderSaveData", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]
				{
					"InGameDefEditor"
				});
			return basePath + "/stats.xml";
		}

		private static string GetStatsPath(string typeName)
		{
			if (basePath == null)
				basePath = (string)typeof(GenFilePaths).GetMethod("FolderUnderSaveData", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]
				{
					"InGameDefEditor"
				});
			return basePath + "/" + typeName.ToString() + ".xml";
		}

		private static bool Load<T>(DefType type, out T t)
		{
			return Load<T>(type.ToString(), out t);
		}

		private static bool Load<T>(string typeName, out T t)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			FileStream fs = null;
			string path = GetStatsPath(typeName);

			if (!File.Exists(path))
			{
				t = default(T);
				return false;
			}

			try
			{
				fs = new FileStream(path, FileMode.Open);
				try
				{
					t = (T)serializer.Deserialize(fs);
				}
				catch (Exception e)
				{
					Log.Error("Failed to load settings for Def type " + typeName + Environment.NewLine + e.GetType().Name + Environment.NewLine + e.Message);
					t = default(T);
					return false;
				}
			}
			catch (Exception e)
			{
				Log.Error("Failed to open file " + path + Environment.NewLine + e.GetType().Name + Environment.NewLine + e.Message);
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
			return Save<T>(type.ToString(), t);
		}

		private static bool Save<T>(string typeName, T t)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			FileStream fs = null;
			try
			{
				fs = new FileStream(GetStatsPath(typeName), FileMode.Create);
				serializer.Serialize(fs, t);
			}
			catch (Exception e)
			{
				Log.Error(
					"Failed to save settings for Def type " + typeName + Environment.NewLine + e.GetType().Name + " -- " + e.Message);
				return false;
			}
			finally
			{
				if (fs != null)
					fs.Close();
			}
			return true;
		}

		private static void Initialize(BackstoryStats b, StringBuilder sb)
		{
			if (b != null)
			{
				try
				{
					if (b.Initialize() &&
						Defs.ApplyStatsAutoThingDefs.TryGetValue(b.Backstory.identifier, out bool apply) && apply)
					{
						try
						{
							b.ApplyStats(b.Backstory);
							sb.AppendLine($"- Backstory: {b.Backstory.identifier}");
						}
						catch (Exception e)
						{
							Log.Error("Failed to apply settings to Backstory [" + b.identifier + "] due to " + e.Message);
						}
					}
					else
					{
						Log.Warning("Unable to initialize Backstory " + b.identifier);
					}
				}
				catch (Exception e)
				{
					Log.Error("Failed to load Backstory [" + b.identifier + "] due to " + e.Message);
				}
			}
		}

		private static void Initialize<D>(DefStat<D> s, StringBuilder sb) where D : Def, new()
		{
			if (s != null)
			{
				try
				{
					if (s.Initialize() &&
						s is IParentStat p)
					{
						if (Defs.ApplyStatsAutoThingDefs.TryGetValue(s.defName, out bool apply) && apply)
						{
							try
							{
								p.ApplyStats(s.Def);
								sb.AppendLine($"- Def: {s.defName}");
							}
							catch (Exception e)
							{
								Log.Error($"Failed to apply settings to Def [{s.defName}] due to {e.Message}");
							}
						}
					}
					else
					{
						Log.Warning($"  Unable to initialize Def {s.defName}");
					}
				}
				catch (Exception e)
				{
					Log.Error($"Failed to load Def [{s.defName}] due to {e.Message}");
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