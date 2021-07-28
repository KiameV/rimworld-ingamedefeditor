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
using System.Linq;

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
					{
						Log.Message($"Auto-Apply Settings to:");
						raad?.autoApplyDefs.ForEach(s => {
							if (!Defs.ApplyStatsAutoDefs.Add(s))
								Log.Warning($"unable to auto-apply stats to {s} as the def/backstory was not found");
						});

						raad?.autoApplyDefsV2.ForEach(s => {
							if (!Defs.ApplyStatsAutoDefs.AddDef(s))
								Log.Warning($"unable to auto-apply stats to {s} as the def was not found");
						});
						raad?.autoApplyBackstories.ForEach(s => {
							if (!Defs.ApplyStatsAutoDefs.AddBackstory(s))
								Log.Warning($"unable to auto-apply stats to {s} as the backstory was not found");
						});
					}

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

					if (Load(DefType.Hediff, out RootHediffs hed))
						hed?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Ingestible, out RootIngestible ing))
						ing?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Mineable, out RootMineable min))
						min?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Backstory, out RootBackstory back))
						back?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Building, out RootBuilding building))
						building?.stats.ForEach((d) => Initialize(d, sb));

					if (Load(DefType.Resource, out RootResource resource))
						resource?.stats.ForEach((d) => Initialize(d, sb));

					/*if (Load(DefType.Animal, out RootAnimals animal))
						animal?.stats.ForEach((d) => Initialize(d, sb));*/

					if (Load(DefType.Plant, out RootPlants plant))
						plant?.stats.ForEach((d) => Initialize(d, sb));

					// Do Last
					sb.AppendLine("Disabling the following defs:");
					if (Load("DisabledDefs", out RootDisabledDefs rdd))
					{
						rdd?.disabledThingDefs.ForEach(s => {
							if (!Defs.DisabledDefs.Add(s))
								Log.Warning($"failed to disable def {s} as the def was not found");
							else
								sb.AppendLine($"- {s}");
						});
						rdd?.disabledDefsV2.ForEach(s => {
							if (!Defs.DisabledDefs.AddDef(s))
								Log.Warning($"failed to disable def {s} as the def was not found");
							else
								sb.AppendLine($"- {s}");
						});
						rdd?.disabledBackstories.ForEach(s => {
							if (!Defs.DisabledDefs.AddBackstory(s))
								Log.Warning($"failed to disable def {s} as the def was not found");
							else
								sb.AppendLine($"- {s}");
						});

						Defs.DisabledDefs.Backstories.ForEach(b => DatabaseUtil.Remove(b));
						Defs.DisabledDefs.Defs.ForEach(d => DatabaseUtil.Remove(d));
					}

					Log.Message(sb.ToString());
				}
				catch (Exception e)
				{
					Log.Warning("InGameDefEditor".Translate() + ": encountered an error on load - " + e.Message);
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
			try
			{
				var dBackstories = new List<string>();
				Defs.DisabledDefs.Backstories.ForEach(b => dBackstories.Add(b.identifier));
				var dDefs = new List<string>();
				Defs.DisabledDefs.Defs.ForEach(d => dDefs.Add(d.defName));
				Save("DisabledDefs", new RootDisabledDefs() { 
					disabledBackstories = dBackstories,
					disabledDefsV2 = dDefs
				});
			}
			catch (Exception e)
			{
				Log.Error("Problem saving disabled defs settings\n" + e.GetType().Name + "\n" + e.Message);
			}

			try
			{
				var aaBackstories = new List<string>();
				Defs.ApplyStatsAutoDefs.Backstories.ForEach(b => aaBackstories.Add(b.identifier));
				var aaDefs = new List<string>();
				Defs.ApplyStatsAutoDefs.Defs.ForEach(d => aaDefs.Add(d.defName));
				Save("AutoApplyDefs", new RootAutoApplyDefs() {
					autoApplyBackstories = aaBackstories,
					autoApplyDefsV2 = aaDefs
				});
			}
			catch (Exception e)
			{
				Log.Error("Problem saving auto-apply settings\n" + e.GetType().Name + "\n" + e.Message);
			}

            try
			{
                Save(DefType.Apparel, new RootApparel() { stats = GetChangedDefs(Defs.ApparelDefs.Values, (d) => new ThingDefStats(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Apparel + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Save(DefType.Weapon, new RootWeapons() { stats = GetChangedDefs(Defs.WeaponDefs.Values, (d) => new ThingDefStats(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Weapon + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Save(DefType.Projectile, new RootProjectiles() { stats = GetChangedDefs(Defs.ProjectileDefs.Values, (d) => new ProjectileDefStats(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Projectile + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Save(DefType.Biome, new RootBiomes() { stats = GetChangedDefs(Defs.BiomeDefs.Values, (d) => new BiomeDefStats(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Biome + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Save(DefType.Recipe, new RootRecipe() { recipes = GetChangedDefs(Defs.RecipeDefs.Values, (d) => new RecipeDefStats(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Recipe + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Save(DefType.Trait, new RootTraits() { stats = GetChangedDefs(Defs.TraitDefs.Values, (d) => new TraitDefStat(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Trait + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Save(DefType.Thought, new RootThoughts() { stats = GetChangedDefs(Defs.ThoughtDefs.Values, (d) => new ThoughtDefStats(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Thought + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Save(DefType.StoryTeller, new RootStoryTeller() { stats = GetChangedDefs(Defs.StoryTellerDefs.Values, (d) => new StoryTellerDefStats(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.StoryTeller + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Save(DefType.Difficulty, new RootDifficulty() { stats = GetChangedDefs(Defs.DifficultyDefs.Values, (d) => new DifficultyDefStat(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Difficulty + ".\n" + e.GetType().Name + "\n" + e.Message);
			}

			try
			{
				Save(DefType.Hediff, new RootHediffs() { stats = GetChangedDefs(Defs.HediffDefs.Values, (d) => new HediffDefStats(d)) });
			}
			catch (Exception e)
			{
				Log.Error("Problem saving " + DefType.Hediff + ".\n" + e.GetType().Name + "\n" + e.Message);
			}

			try
            {
                Save(DefType.Ingestible, new RootIngestible() { stats = GetChangedDefs(Defs.IngestibleDefs.Values, (d) => new ThingDefStats(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Apparel + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Save(DefType.Mineable, new RootMineable() { stats = GetChangedDefs(Defs.MineableDefs.Values, (d) => new ThingDefStats(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Ingestible + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Save(DefType.Backstory, new RootBackstory() { stats = GetChangedBackstory(Defs.Backstories.Values) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Backstory + ".\n" + e.GetType().Name + "\n" + e.Message);
            }

            try
            {
                Save(DefType.Building, new RootBuilding() { stats = GetChangedDefs(Defs.BuildingDefs.Values, (d) => new ThingDefStats(d)) });
            }
            catch (Exception e)
            {
                Log.Error("Problem saving " + DefType.Building + ".\n" + e.GetType().Name + "\n" + e.Message);
			}

			try
			{
				Save(DefType.Resource, new RootResource() { stats = GetChangedDefs(Defs.ResourceDefs.Values, (d) => new ThingDefStats(d)) });
			}
			catch (Exception e)
			{
				Log.Error("Problem saving " + DefType.Resource + ".\n" + e.GetType().Name + "\n" + e.Message);
			}

			/*try
			{
				Save(DefType.Animal, new RootAnimals() { stats = GetChangedDefs(Defs.AnimalDefs.Values, (d) => new ThingDefStats(d)) });
			}
			catch (Exception e)
			{
				Log.Error("Problem saving " + DefType.Animal + ".\n" + e.GetType().Name + "\n" + e.Message);
			}*/

			try
			{
				Save(DefType.Plant, new RootPlants() { stats = GetChangedDefs(Defs.PlantDefs.Values, (d) => new ThingDefStats(d)) });
			}
			catch (Exception e)
			{
				Log.Error("Problem saving " + DefType.Plant + ".\n" + e.GetType().Name + "\n" + e.Message);
			}
		}

		private static List<BackstoryStats> GetChangedBackstory(IEnumerable<Backstory> backstories)
		{
			List<BackstoryStats> result = new List<BackstoryStats>(backstories.Count());
			foreach (var b in backstories)
			{
				if (Defs.ApplyStatsAutoDefs.ContainsBackstory(b))
				{
					result.Add(new BackstoryStats(b));
				}
			}
			return result;
		}

		private static List<S> GetChangedDefs<D, S>(IEnumerable<D> defs, Func<D, S> newInstance) where D : Def, new() where S : IDefStat
		{
			List<S> result = new List<S>(defs.Count());
			foreach (var d in defs)
			{
				if (Defs.ApplyStatsAutoDefs.ContainsDef(d))
				{
					result.Add(newInstance(d));
				}
			}
			return result;
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
						Defs.ApplyStatsAutoDefs.Contains(b.Backstory))
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
					Log.Message($"Initialize {s.defName} and IParentStat? {s is IParentStat}");
					if (s.Initialize() &&
						s is IParentStat p)
					{
						Log.Message($"Auto Apply? {Defs.ApplyStatsAutoDefs.Contains(s.Def)}");
						if (Defs.ApplyStatsAutoDefs.Contains(s.Def))
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
						Log.Warning($"Unable to initialize Def {s.defName}\n{s.ToString()}");
					}
				}
				catch (Exception e)
				{
					Log.Error($"Failed to load Def [{s.defName}] due to {e.Message}\n{s.ToString()}");
				}
			}
		}
	}
}