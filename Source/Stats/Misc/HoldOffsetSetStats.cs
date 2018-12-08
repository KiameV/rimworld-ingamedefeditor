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

		public void ApplyStats(HoldOffsetSet s)
		{
			if (this.northDefault != null)
			{
				if (s.northDefault == null)
					s.northDefault = new HoldOffset();
				this.northDefault.ApplyStats(s.northDefault);
			}
			if (this.east != null)
			{
				if (s.east == null)
					s.east = new HoldOffset();
				this.east.ApplyStats(s.east);
			}
			if (this.south != null)
			{
				if (s.south == null)
					s.south = new HoldOffset();
				this.south.ApplyStats(s.south);
			}
			if (this.west != null)
			{
				if (s.west == null)
					s.west = new HoldOffset();
				this.west.ApplyStats(s.west);
			}
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
