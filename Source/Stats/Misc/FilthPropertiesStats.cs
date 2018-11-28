using System;
using RimWorld;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class FilthPropertiesStats
	{
		public float cleaningWorkToReduceThickness;
		public bool terrainSourced;
		public bool canFilthAttach;
		public bool rainWashes;
		public bool allowsFire;
		public int maxThickness;

		public FilthPropertiesStats() { }
		public FilthPropertiesStats(FilthProperties p)
		{
			this.cleaningWorkToReduceThickness = p.cleaningWorkToReduceThickness;
			this.terrainSourced = p.terrainSourced;
			this.canFilthAttach = p.canFilthAttach;
			this.rainWashes = p.rainWashes;
			this.allowsFire = p.allowsFire;
			this.maxThickness = p.maxThickness;
		}
	}
}
