using System;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    class Backup
    {
        private readonly static Dictionary<Def, Stats> defBackup = new Dictionary<Def, Stats>();
        private readonly static Dictionary<Def, ProjectileStats> projectileBackup = new Dictionary<Def, ProjectileStats>();

        public static bool HasChanged(Stats s)
        {
            if (s == null)
                return false;

            if (defBackup.TryGetValue(s.Def, out Stats found))
            {
                return !s.Equals(found);
            }
            return true;
        }

        public static bool HasChanged(ProjectileStats s)
        {
            if (s == null)
                return false;

            if (projectileBackup.TryGetValue(s.Def, out ProjectileStats found))
            {
#if DEBUG
                Log.Error(s.ToString());
                Log.Error(found.ToString());
#endif
                return !s.Equals(found);
            }
            return true;
        }

        public static void Init(IEnumerable<ThingDef> apparelDefs, IEnumerable<ThingDef> weaponDefs)
        {
            if (defBackup == null || defBackup.Count == 0)
            {
                foreach (ThingDef d in apparelDefs)
                {
                    defBackup[d] = new Stats(d);
                }

                foreach (ThingDef d in weaponDefs)
                {
                    defBackup[d] = new Stats(d);
                }
            }

            if (projectileBackup == null || projectileBackup.Count == 0)
            {
                foreach (ThingDef d in weaponDefs)
                {
                    if (d.Verbs != null)
                    {
                        foreach (VerbProperties v in d.Verbs)
                        {
                            if (v.defaultProjectile != null)
                            {
                                if (!projectileBackup.ContainsKey(v.defaultProjectile))
                                {
                                    projectileBackup.Add(v.defaultProjectile, new ProjectileStats(v.defaultProjectile));
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ApplyStats(ThingDef d)
        {
            if (defBackup.TryGetValue(d, out Stats s))
                s.ApplyStats(d);
            else if (projectileBackup.TryGetValue(d, out ProjectileStats ps))
                ps.ApplyStats(d);
        }
    }
}
