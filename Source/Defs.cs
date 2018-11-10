using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    static class Defs
    {
        public static readonly SortedDictionary<string, ThingDef> ApparelDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, ThingDef> WeaponDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, ThingDef> ProjectileDefs = new SortedDictionary<string, ThingDef>();

        private static bool isInit = false;
        public static void Initialize()
        {
            if (!isInit)
            {
                foreach (ThingDef d in DefDatabase<ThingDef>.AllDefs)
                {
                    if (d.IsApparel)
                    {
                        ApparelDefs[d.label] = d;
                    }
                    if (d.IsWeapon)
                    {
                        WeaponDefs[d.label] = d;

                        if (d.IsWeaponUsingProjectiles && d.Verbs != null)
                        {
                            foreach (VerbProperties v in d.Verbs)
                            {
                                if (v.defaultProjectile != null)
                                {
                                    ProjectileDefs[v.defaultProjectile.label] = v.defaultProjectile;
                                }
                            }
                        }
                    }
                    if (d.defName.StartsWith("Arrow_") || 
                        d.defName.StartsWith("Bullet_") || 
                        d.defName.StartsWith("Proj_"))
                    {
                        ProjectileDefs[d.label] = d;
                    }
                }

                if (ApparelDefs.Count > 0)
                {
                    Backup.Initialize();
                    
                    isInit = true;
                }
            }
        }
    }
}