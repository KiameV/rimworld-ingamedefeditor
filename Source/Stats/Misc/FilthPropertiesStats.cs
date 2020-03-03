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
            // TODO this.terrainSourced = p.terrainSourced;
            this.canFilthAttach = p.canFilthAttach;
			this.rainWashes = p.rainWashes;
			this.allowsFire = p.allowsFire;
			this.maxThickness = p.maxThickness;
		}

		public FilthProperties ToFilthProperties()
		{
			return new FilthProperties()
			{
				cleaningWorkToReduceThickness = this.cleaningWorkToReduceThickness,
				// TODO terrainSourced = this.terrainSourced,
				canFilthAttach = this.canFilthAttach,
				rainWashes = this.rainWashes,
				allowsFire = this.allowsFire,
				maxThickness = this.maxThickness,
			};
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is FilthPropertiesStats s)
			{
				return
					this.cleaningWorkToReduceThickness == s.cleaningWorkToReduceThickness &&
					this.terrainSourced == s.terrainSourced &&
					this.canFilthAttach == s.canFilthAttach &&
					this.rainWashes == s.rainWashes &&
					this.allowsFire == s.allowsFire &&
					this.maxThickness == s.maxThickness;
			}
			return false;
		}

		public override string ToString()
		{
			return
				"FilthPropertiesStats" +
				"\ncleaningWorkToReduceThickness: " + cleaningWorkToReduceThickness +
				"\nterrainSourced: " + terrainSourced +
				"\ncanFilthAttach: " + canFilthAttach +
				"\nrainWashes: " + rainWashes +
				"\nallowsFire: " + allowsFire +
				"\nmaxThickness: " + maxThickness;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}
