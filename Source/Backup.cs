using InGameDefEditor.Stats;
using InGameDefEditor.Stats.DefStat;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    class Backup
    {
        private static bool initialized = false;
		private readonly static Dictionary<string, BackstoryStats> backupBackstories = new Dictionary<string, BackstoryStats>();
        private readonly static Dictionary<string, IParentStat> backupDefs = new Dictionary<string, IParentStat>();

        public static bool HasChanged<T>(object o)
        {
            switch(o)
            {
                case IDefStat ds:
                    return Defs.ApplyStatsAutoDefs.Contains(ds.BaseDef);
                case BackstoryStats bs:
                    return Defs.ApplyStatsAutoDefs.ContainsBackstory(bs.Backstory);
            }
            Log.Warning($"Unable to determine if {Util.GetLabel(o)} should be auto-applied");
            return false;
            /*string key = t.UniqueKey;
            if (t is ThoughtDefStats)
				key += THOUGHT_KEY;
			else if (t is TraitDefStat)
				key += TRAIT_KEY;

            if (backup.TryGetValue(key, out IParentStat found))
            {
                return !t.Equals(found);
            }

            Log.Message(t.UniqueKey + " not found in backup");

            return true;*/
        }

        public static void Initialize()
        {
            if (!initialized)
            {
                initialized = true;
                foreach (ThingDef d in Defs.ApparelDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new ThingDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

                foreach (ThingDef d in Defs.WeaponDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new ThingDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

                foreach (ThingDef d in Defs.ProjectileDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new ProjectileDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

                foreach (BiomeDef d in Defs.BiomeDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new BiomeDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

                foreach (ThoughtDef d in Defs.ThoughtDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new ThoughtDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

                foreach (RecipeDef d in Defs.RecipeDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new RecipeDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Message("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

				foreach (TraitDef d in Defs.TraitDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new TraitDefStat(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

				foreach (StorytellerDef d in Defs.StoryTellerDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new StoryTellerDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

				foreach (DifficultyDef d in Defs.DifficultyDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new DifficultyDefStat(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

				foreach (ThingDef d in Defs.IngestibleDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new ThingDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

				foreach (ThingDef d in Defs.MineableDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new ThingDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

				foreach (Backstory b in Defs.Backstories.Values)
                {
                    try
                    {
                        backupBackstories[b.identifier] = new BackstoryStats(b);
                    }
                    catch (Exception e)
                    {
                        if (b != null)
                            Log.Warning("Failed to initialize backup for " + b.identifier + ". " + e.Message);
                    }
                }

				foreach (ThingDef d in Defs.BuildingDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new ThingDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

                foreach (ThingDef d in Defs.ResourceDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new ThingDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

                foreach (ThingDef d in Defs.PlantDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new ThingDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }

                /*foreach (ThingDef d in Defs.AnimalDefs.Values)
                {
                    try
                    {
                        backupDefs[d.defName] = new ThingDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }*/
            }
        }

        public static bool ApplyStats(Backstory b)
        {
            if (backupBackstories.TryGetValue(b.identifier, out BackstoryStats s))
            {
                s.ApplyStats(b);
                return true;
            }
            Log.Warning("Unable to find backup for Backstory " + b.identifier);
            return false;
		}

		public static bool ApplyStats(Def def)
		{
			string key = def.defName;
            if (backupDefs.TryGetValue(key, out IParentStat s))
            {
                s.ApplyStats(def);
                return true;
            }
            Log.Warning("Unable to find backup for Def " + def.defName);
            return false;
		}

        public static bool ApplyStats(object o)
        {
            switch(o)
            {
                case Backstory b:
                    return ApplyStats(b);
                case Def d:
                    return ApplyStats(d);
            }
            Log.Warning($"Unable to ApplyStats to {Util.GetLabel(o)}");
            return false;
        }
	}
}
