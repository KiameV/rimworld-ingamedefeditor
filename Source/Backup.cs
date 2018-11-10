using InGameDefEditor.Stats.Misc;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    class Backup
    {
        private readonly static Dictionary<Def, ThingDefStats> thingDefBackup = new Dictionary<Def, ThingDefStats>();
        private readonly static Dictionary<Def, ProjectileStats> projectileBackup = new Dictionary<Def, ProjectileStats>();

        public static bool HasChanged(ThingDefStats s)
        {
            if (s == null)
                return false;

            if (thingDefBackup.TryGetValue(s.Def, out ThingDefStats found))
                return !s.Equals(found);
            return true;
        }

        public static bool HasChanged(ProjectileStats s)
        {
            if (s == null)
                return false;

            if (projectileBackup.TryGetValue(s.Def, out ProjectileStats found))
                return !s.Equals(found);
            return true;
        }

        public static void Initialize()
        {
            if (thingDefBackup == null || thingDefBackup.Count == 0)
            {
                foreach (ThingDef d in Defs.ApparelDefs.Values)
                {
                    thingDefBackup[d] = new ThingDefStats(d);
                }

                foreach (ThingDef d in Defs.WeaponDefs.Values)
                {
                    thingDefBackup[d] = new ThingDefStats(d);
                }
            }

            if (projectileBackup == null || projectileBackup.Count == 0)
            {
                foreach (ThingDef d in Defs.ProjectileDefs.Values)
                {
                    projectileBackup[d] = new ProjectileStats(d);
                }
            }
        }

        public static void ApplyStats(ThingDef d)
        {
            if (thingDefBackup.TryGetValue(d, out ThingDefStats s))
                s.ApplyStats(d);
            else if (projectileBackup.TryGetValue(d, out ProjectileStats ps))
                ps.ApplyStats(d);
            else
                Log.Warning("Unable to find backup for " + d.defName);
        }
    }
}
