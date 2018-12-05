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
		public List<RecipeDefStats> stats;
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
}
