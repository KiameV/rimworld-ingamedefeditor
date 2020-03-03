using InGameDefEditor.Stats;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    class Backup
    {
		private const string THOUGHT_KEY = "thought";
		private const string TRAIT_KEY = "trait";

		private readonly static Dictionary<string, IParentStat> backup = new Dictionary<string, IParentStat>();

		public static bool HasChanged<T>(T t) where T : IParentStat
        {
            if (t == null)
                return false;

            if (Defs.ApplyStatsAutoThingDefs.TryGetValue(t.UniqueKey, out bool b) && b)
                return b;
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
            if (backup == null || backup.Count == 0)
            {
                foreach (ThingDef d in Defs.ApparelDefs.Values)
                {
                    try
                    {
                        backup[d.defName] = new ThingDefStats(d);
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
                        backup[d.defName] = new ThingDefStats(d);
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
                        backup[d.defName] = new ProjectileDefStats(d);
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
                        backup[d.defName] = new BiomeDefStats(d);
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
                        backup[d.defName + THOUGHT_KEY] = new ThoughtDefStats(d);
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
                        backup[d.defName] = new RecipeDefStats(d);
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
                        backup[d.defName + TRAIT_KEY] = new TraitDefStat(d);
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
                        backup[d.defName] = new StoryTellerDefStats(d);
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
                        backup[d.defName] = new DifficultyDefStat(d);
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
                        backup[d.defName] = new ThingDefStats(d);
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
                        backup[d.defName] = new ThingDefStats(d);
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
                        backup[b.identifier] = new BackstoryStats(b);
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
                        backup[d.defName] = new ThingDefStats(d);
                    }
                    catch (Exception e)
                    {
                        if (d != null)
                            Log.Warning("Failed to initialize backup for " + d.defName + ". " + e.Message);
                    }
                }
			}
        }

        public static void ApplyStats(Backstory b)
        {
            if (backup.TryGetValue(b.identifier, out IParentStat s))
                s.ApplyStats(b);
            else
                Log.Warning("Unable to find backup for Backstory " + b.identifier);
		}

		public static void ApplyStats(Def def)
		{
			string key = def.defName;
			if (def is ThoughtDef)
				key += THOUGHT_KEY;
			else if (def is TraitDef)
				key += TRAIT_KEY;

			if (backup.TryGetValue(key, out IParentStat s))
				s.ApplyStats(def);
			else
				Log.Warning("Unable to find backup for Def " + def.defName);
		}
	}
}
