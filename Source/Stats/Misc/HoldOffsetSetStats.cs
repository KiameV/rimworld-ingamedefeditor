using System;
using RimWorld;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class HoldOffsetSetStats
	{
		public HoldOffsetStats northDefault;
		public HoldOffsetStats east;
		public HoldOffsetStats south;
		public HoldOffsetStats west;

		public HoldOffsetSetStats() { }
		public HoldOffsetSetStats(HoldOffsetSet s)
		{
			if (s.northDefault != null)
				this.northDefault = new HoldOffsetStats(s.northDefault);
			if (s.east != null)
				this.east = new HoldOffsetStats(s.east);
			if (s.south != null)
				this.south = new HoldOffsetStats(s.south);
			if (s.west != null)
				this.west = new HoldOffsetStats(s.west);
		}

		public HoldOffsetSet ToHoldOffsetSet()
		{
			return new HoldOffsetSet()
			{
				northDefault = this.northDefault?.ToHoldOffset(),
				east = this.east?.ToHoldOffset(),
				south = this.south?.ToHoldOffset(),
				west = this.west?.ToHoldOffset(),
			};
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is HoldOffsetSetStats s)
			{
				return
					object.Equals(this.northDefault, s.northDefault) &&
					object.Equals(this.east, s.east) &&
					object.Equals(this.south, s.south) &&
					object.Equals(this.west, s.west);
			}
			return false;
		}

		public override string ToString()
		{
			return
				"IngestibleProperties" +
				"\nnorthDefault: " + northDefault +
				"\neast: " + east +
				"\nsouth: " + south +
				"\nwest:" + west;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}
}
