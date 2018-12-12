using InGameDefEditor.Stats;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    class Backup
    {
        private readonly static Dictionary<string, IParentStat> backup = new Dictionary<string, IParentStat>();

		public static bool HasChanged<T>(T t) where T : IParentStat
        {
            if (t == null)
                return false;

            if (backup.TryGetValue(t.UniqueKey, out IParentStat found))
                return !t.Equals(found);

            Log.Message(t.UniqueKey + " not found in backup");

            return true;
        }

        public static void Initialize()
        {
            if (backup == null || backup.Count == 0)
            {
                foreach (ThingDef d in Defs.ApparelDefs.Values)
                    backup[d.defName] = new ThingDefStats(d);

                foreach (ThingDef d in Defs.WeaponDefs.Values)
                    backup[d.defName] = new ThingDefStats(d);

                foreach (ThingDef d in Defs.ProjectileDefs.Values)
                    backup[d.defName] = new ProjectileDefStats(d);

                foreach (BiomeDef d in Defs.BiomeDefs.Values)
                    backup[d.defName] = new BiomeDefStats(d);

                foreach (ThoughtDef d in Defs.ThoughtDefs.Values)
                    backup[d.defName] = new ThoughtDefStats(d);

				foreach (RecipeDef d in Defs.RecipeDefs.Values)
					backup[d.defName] = new RecipeDefStats(d);

				foreach (TraitDef d in Defs.TraitDefs.Values)
					backup[d.defName] = new TraitDefStat(d);

				foreach (StorytellerDef d in Defs.StoryTellerDefs.Values)
					backup[d.defName] = new StoryTellerDefStats(d);

				foreach (DifficultyDef d in Defs.DifficultyDefs.Values)
					backup[d.defName] = new DifficultyDefStat(d);

				foreach (ThingDef d in Defs.IngestibleDefs.Values)
					backup[d.defName] = new ThingDefStats(d);

				foreach (ThingDef d in Defs.MineableDefs.Values)
					backup[d.defName] = new ThingDefStats(d);

				foreach (Backstory b in Defs.Backstories.Values)
					backup[b.identifier] = new BackstoryStats(b);
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
			if (backup.TryGetValue(def.defName, out IParentStat s))
				s.ApplyStats(def);
			else
				Log.Warning("Unable to find backup for Def " + def.defName);
		}
	}
}
