using RimWorld;
using System.Collections.Generic;
using Verse;
using System;

namespace InGameDefEditor
{
    static class Defs
    {
        public static readonly SortedDictionary<string, ThingDef> ApparelDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, ThingDef> WeaponDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, ThingDef> ProjectileDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, BiomeDef> BiomeDefs = new SortedDictionary<string, BiomeDef>();
        public static readonly SortedDictionary<string, ThoughtDef> ThoughtDefs = new SortedDictionary<string, ThoughtDef>();
		public static readonly SortedDictionary<string, RecipeDef> RecipeDefs = new SortedDictionary<string, RecipeDef>();
		public static readonly SortedDictionary<string, TraitDef> TraitDefs = new SortedDictionary<string, TraitDef>();

		private static bool isInit = false;
        public static void Initialize()
        {
            if (!isInit)
            {
                int i = 0;
                foreach (ThingDef d in DefDatabase<ThingDef>.AllDefsListForReading)
                {
					if (d == null)
					{
						Log.Warning("Null definition found. Skipping.");
						continue;
					}

                    ++i;
                    if (d.IsApparel)
                    {
						if (d.label == null)
							Log.Warning("ApparelDef [" + d.defName + "] has a null label.");
						else
							ApparelDefs[d.label] = d;
                    }
                    if (d.IsWeapon)
					{
						if (d.label == null)
							Log.Warning("WeaponDef [" + d.defName + "] has a null label.");
						else
						{
							WeaponDefs[d.label] = d;
							if (d.IsWeaponUsingProjectiles && d.Verbs != null)
							{
								foreach (VerbProperties v in d.Verbs)
								{
									if (v.defaultProjectile != null)
									{
										if (d.label == null)
											Log.Warning("ProjectileDef [" + v.defaultProjectile.defName + "] has a null label.");
										else
											ProjectileDefs[v.defaultProjectile.label] = v.defaultProjectile;
									}
								}
							}
						}
                    }
                    if (d.defName.StartsWith("Arrow_") || 
                        d.defName.StartsWith("Bullet_") || 
                        d.defName.StartsWith("Proj_"))
					{
						if (d.label == null)
							Log.Warning("Projectile [" + d.defName + "] has a null label.");
						else
							ProjectileDefs[d.label] = d;
                    }
                }

                foreach (BiomeDef d in DefDatabase<BiomeDef>.AllDefsListForReading)
				{
					if (d.label == null)
						Log.Warning("BiomeDef [" + d.defName + "] has a null label.");
					else
						BiomeDefs[d.label] = d;
                }

                foreach (ThoughtDef d in DefDatabase<ThoughtDef>.AllDefsListForReading)
				{
					ThoughtDefs[d.defName] = d;
                }

				foreach (RecipeDef d in DefDatabase<RecipeDef>.AllDefsListForReading)
				{
					if (d.label == null)
						Log.Warning("RecipeDef [" + d.defName + "] has a null label.");
					else
						RecipeDefs[d.label] = d;
				}

				foreach (TraitDef d in DefDatabase<TraitDef>.AllDefsListForReading)
				{
					TraitDefs[d.defName] = d;
				}

				if (i > 0)
                {
                    Backup.Initialize();
                    isInit = true;
                }
            }
        }

		public static void ResetAll()
		{
			foreach (ThingDef d in Defs.ApparelDefs.Values)
				Backup.ApplyStats(d);

			foreach (ThingDef d in Defs.WeaponDefs.Values)
				Backup.ApplyStats(d);

			foreach (ThingDef d in Defs.ProjectileDefs.Values)
				Backup.ApplyStats(d);

			foreach (BiomeDef d in Defs.BiomeDefs.Values)
				Backup.ApplyStats(d);

			foreach (RecipeDef d in Defs.RecipeDefs.Values)
				Backup.ApplyStats(d);

			foreach (TraitDef d in Defs.TraitDefs.Values)
				Backup.ApplyStats(d);

			foreach (ThoughtDef d in Defs.ThoughtDefs.Values)
				Backup.ApplyStats(d);
		}
	}
}