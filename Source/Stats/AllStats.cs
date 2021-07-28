using InGameDefEditor.Stats.DefStat;
using System.Collections.Generic;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats
{
	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootDisabledDefs
	{
		public List<string> disabledThingDefs;
		public List<string> disabledDefsV2;
		public List<string> disabledBackstories;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootAutoApplyDefs
	{
		public List<string> autoApplyDefs;
		public List<string> autoApplyDefsV2;
		public List<string> autoApplyBackstories;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
    public class RootApparel
	{
		public List<ThingDefStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootWeapons
	{
		public List<ThingDefStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootProjectiles
	{
		public List<ProjectileDefStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootBiomes
	{
		public List<BiomeDefStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootRecipe
	{
		public List<RecipeDefStats> recipes;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootTraits
	{
		public List<TraitDefStat> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootThoughts
	{
		public List<ThoughtDefStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootStoryTeller
	{
		public List<StoryTellerDefStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootDifficulty
	{
		public List<DifficultyDefStat> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootIngestible
	{
		public List<ThingDefStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootMineable
	{
		public List<ThingDefStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootBackstory
	{
		public List<BackstoryStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootBuilding
	{
		public List<ThingDefStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootResource
	{
		public List<ThingDefStats> stats;
	}

   [XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootHediffs
	{
		public List<HediffDefStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootAnimals
	{
		public List<ThingDefStats> stats;
	}

	[XmlRoot("IGDE", Namespace = "http://www.InGameDefEditor.kiamev.com", IsNullable = false)]
	public class RootPlants
	{
		public List<ThingDefStats> stats;
	}
}
