using RimWorld;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    static class Defs
	{
		public static readonly SortedDictionary<string, ThingDef> DisabledThingDefs = new SortedDictionary<string, ThingDef>();

		public static readonly SortedDictionary<string, ThingDef> ApparelDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, ThingDef> WeaponDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, ThingDef> ProjectileDefs = new SortedDictionary<string, ThingDef>();
        public static readonly SortedDictionary<string, BiomeDef> BiomeDefs = new SortedDictionary<string, BiomeDef>();
        public static readonly SortedDictionary<string, ThoughtDef> ThoughtDefs = new SortedDictionary<string, ThoughtDef>();
		public static readonly SortedDictionary<string, RecipeDef> RecipeDefs = new SortedDictionary<string, RecipeDef>();
		public static readonly SortedDictionary<string, TraitDef> TraitDefs = new SortedDictionary<string, TraitDef>();
		public static readonly SortedDictionary<string, StorytellerDef> StoryTellerDefs = new SortedDictionary<string, StorytellerDef>();
		public static readonly SortedDictionary<string, DifficultyDef> DifficultyDefs = new SortedDictionary<string, DifficultyDef>();
		public static readonly SortedDictionary<string, ThingDef> IngestibleDefs = new SortedDictionary<string, ThingDef>();
		public static readonly SortedDictionary<string, ThingDef> MineableDefs = new SortedDictionary<string, ThingDef>();
		private static bool isInit = false;

		public static void Initialize()
        {
            if (!isInit)
            {
                int i = 0;
                foreach (ThingDef d in DefDatabase<ThingDef>.AllDefsListForReading)
                {
					string label = Util.GetDefLabel(d);
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

                    if (d.IsWeapon)
					{
						WeaponDefs[label] = d;
						if (d.IsWeaponUsingProjectiles && d.Verbs != null)
						{
							d.Verbs.ForEach(v =>
							{
								if (v.defaultProjectile != null)
									ProjectileDefs[Util.GetDefLabel(v.defaultProjectile)] = v.defaultProjectile;
								else
									Log.Warning("No projectiles defined for " + d.defName);
							});
						}
                    }

                    if (d.defName.StartsWith("Arrow_") || 
                        d.defName.StartsWith("Bullet_") || 
                        d.defName.StartsWith("Proj_"))
					{
						ProjectileDefs[label] = d;
                    }

					if (d.IsIngestible)
					{
						IngestibleDefs[label] = d;
					}

					if (d.mineable)
					{
						MineableDefs[label] = d;
					}
                }

				DefDatabase<BiomeDef>.AllDefsListForReading.ForEach(d => BiomeDefs[Util.GetDefLabel(d)] = d);
				DefDatabase<ThoughtDef>.AllDefsListForReading.ForEach(d => ThoughtDefs[Util.GetDefLabel(d)] = d);
				DefDatabase<RecipeDef>.AllDefsListForReading.ForEach(d =>
				{
					if (!d.defName.StartsWith("OCD_MineDeep"))
						RecipeDefs[Util.GetDefLabel(d)] = d;
				});
				DefDatabase<TraitDef>.AllDefsListForReading.ForEach(d => TraitDefs[Util.GetDefLabel(d)] = d);
				DefDatabase<StorytellerDef>.AllDefsListForReading.ForEach(d => StoryTellerDefs[Util.GetDefLabel(d)] = d);
				DefDatabase<DifficultyDef>.AllDefsListForReading.ForEach(d => DifficultyDefs[Util.GetDefLabel(d)] = d);

				if (i > 0)
                {
                    Backup.Initialize();
                    isInit = true;
                }
            }
        }

		public static void ResetAll()
		{
			Defs.DisabledThingDefs.Clear();

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

			foreach (ThingDef d in Defs.IngestibleDefs.Values)
				Backup.ApplyStats(d);

			foreach (ThingDef d in Defs.MineableDefs.Values)
				Backup.ApplyStats(d);
		}
	}
}