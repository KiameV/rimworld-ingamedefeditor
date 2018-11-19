using InGameDefEditor.Stats;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    class Backup
    {
        private readonly static Dictionary<Def, IParentStat> backup = new Dictionary<Def, IParentStat>();

        public static bool HasChanged<T>(T t) where T : IParentStat
        {
            if (t == null)
                return false;

            if (backup.TryGetValue(t.BaseDef, out IParentStat found))
                return !t.Equals(found);

            Log.Message(t.BaseDef.defName + " not found in backup");

            return true;
        }

        public static void Initialize()
        {
            if (backup == null || backup.Count == 0)
            {
                foreach (ThingDef d in Defs.ApparelDefs.Values)
                    backup[d] = new ThingDefStats(d);

                foreach (ThingDef d in Defs.WeaponDefs.Values)
                    backup[d] = new ThingDefStats(d);

                foreach (ThingDef d in Defs.ProjectileDefs.Values)
                    backup[d] = new ProjectileDefStats(d);

                foreach (BiomeDef d in Defs.BiomeDefs.Values)
                    backup[d] = new BiomeDefStats(d);

                foreach (ThoughtDef d in Defs.ThoughtDefs.Values)
                    backup[d] = new ThoughtDefStats(d);

				foreach (RecipeDef d in Defs.RecipeDefs.Values)
					backup[d] = new RecipeDefStats(d);
			}
        }

        public static void ApplyStats(Def def)
        {
            if (backup.TryGetValue(def, out IParentStat s))
                s.ApplyStats(def);
            else
                Log.Warning("Unable to find backup for " + def.defName);
        }
    }
}
