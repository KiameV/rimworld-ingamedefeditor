using RimWorld;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    static class Defs
	{
		public static readonly DefsDictionary DisabledDefs = new DefsDictionary();
		public static readonly DefsDictionary ApplyStatsAutoDefs = new DefsDictionary();

		public static readonly SortedDictionary<string, ThingDef> ApparelDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, ThingDef> WeaponDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, ThingDef> ProjectileDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, BiomeDef> BiomeDefs = new SortedDictionary<string, BiomeDef>();
        public static readonly SortedDictionary<string, ThoughtDef> ThoughtDefs = new SortedDictionary<string, ThoughtDef>();
		public static readonly SortedDictionary<string, RecipeDef> RecipeDefs = new SortedDictionary<string, RecipeDef>();
		public static readonly SortedDictionary<string, TraitDef> TraitDefs = new SortedDictionary<string, TraitDef>();
		public static readonly SortedDictionary<string, StorytellerDef> StoryTellerDefs = new SortedDictionary<string, StorytellerDef>();
		public static readonly SortedDictionary<string, DifficultyDef> DifficultyDefs = new SortedDictionary<string, DifficultyDef>();
		public static readonly SortedDictionary<string, HediffDef> HediffDefs = new SortedDictionary<string, HediffDef>();
		public static readonly SortedDictionary<string, ThingDef> IngestibleDefs = new SortedDictionary<string, ThingDef>();
		public static readonly SortedDictionary<string, ThingDef> MineableDefs = new SortedDictionary<string, ThingDef>();
		public static readonly SortedDictionary<string, Backstory> Backstories = new SortedDictionary<string, Backstory>();
		public static readonly SortedDictionary<string, ThingDef> BuildingDefs = new SortedDictionary<string, ThingDef>();
		private static bool isInit = false;
		
		public static void Initialize()
        {
            if (!isInit)
            {
                int i = 0;
                foreach (ThingDef d in DefDatabase<ThingDef>.AllDefs)
                {
					string label = Util.GetLabel(d);
					if (d == null)
					{
						Log.Warning("Null definition found. Skipping.");
						continue;
					}

                    ++i;
                    if (d.IsApparel)
					{
						ApparelDefs[label] = d;
                    }
                    else if (d.IsWeapon)
					{
						WeaponDefs[label] = d;
						if (d.IsWeaponUsingProjectiles && d.Verbs != null)
						{
							d.Verbs.ForEach(v =>
							{
								if (v.defaultProjectile != null)
									ProjectileDefs[Util.GetLabel(v.defaultProjectile)] = v.defaultProjectile;
								/*else
								{
									if (d.building == null ||
										(!d.building.IsTurret && !d.building.IsMortar))
									{
										Log.Warning("No projectiles defined for " + d.defName);
									}
								}*/
							});
						}
                    }
					else if(d.defName.StartsWith("Arrow_") || 
                        d.defName.StartsWith("Bullet_") || 
                        d.defName.StartsWith("Proj_"))
					{
						ProjectileDefs[label] = d;
                    }
					else if(d.IsIngestible)
					{
						IngestibleDefs[label] = d;
					}
					else if(d.mineable)
					{
						MineableDefs[label] = d;
					}
					else if(d.building != null)
					{
						BuildingDefs[label] = d;
					}
                }

				foreach (var d in DefDatabase<BiomeDef>.AllDefs)
					BiomeDefs[Util.GetLabel(d)] = d;

				foreach (var d in DefDatabase<ThoughtDef>.AllDefs)
					ThoughtDefs[Util.GetLabel(d)] = d;

				foreach (var d in DefDatabase<RecipeDef>.AllDefs)
				{
					if (!d.defName.StartsWith("OCD_MineDeep"))
						RecipeDefs[Util.GetLabel(d)] = d;
				}

				foreach (var d in DefDatabase<TraitDef>.AllDefs)
					TraitDefs[Util.GetLabel(d)] = d;

				foreach (var d in DefDatabase<StorytellerDef>.AllDefs)
					StoryTellerDefs[Util.GetLabel(d)] = d;

				foreach (var d in DefDatabase<DifficultyDef>.AllDefs)
					DifficultyDefs[Util.GetLabel(d)] = d;

				foreach (var d in DefDatabase<HediffDef>.AllDefs)
					HediffDefs[Util.GetLabel(d)] = d;

				foreach (var b in BackstoryDatabase.allBackstories.Values)
					Backstories[b.title] = b;

				if (i > 0)
                {
                    Backup.Initialize();
                    isInit = true;
                }
            }
        }

		/*public static void ResetAll()
		{
			Defs.DisabledDefs.Clear();
			Defs.ApplyStatsAutoDefs.Clear();

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

			foreach (StorytellerDef d in Defs.StoryTellerDefs.Values)
				Backup.ApplyStats(d);

			foreach (DifficultyDef d in Defs.DifficultyDefs.Values)
				Backup.ApplyStats(d);

			foreach (HediffDef d in Defs.HediffDefs.Values)
				Backup.ApplyStats(d);

			foreach (ThingDef d in Defs.IngestibleDefs.Values)
				Backup.ApplyStats(d);

			foreach (ThingDef d in Defs.MineableDefs.Values)
				Backup.ApplyStats(d);

			foreach (Backstory b in Defs.Backstories.Values)
				Backup.ApplyStats(b);

			foreach (ThingDef d in Defs.BuildingDefs.Values)
				Backup.ApplyStats(d);
		}*/
	}
}