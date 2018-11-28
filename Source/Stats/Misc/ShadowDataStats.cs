using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class ShadowDataStats
	{
		public Vector3Stats volume;
		public Vector3Stats offset;

		public ShadowDataStats() { }
		public ShadowDataStats(ShadowData s)
		{
			if (s.volume != null)
				this.volume = new Vector3Stats(s.volume);
			if (s.offset != null)
				this.offset = new Vector3Stats(s.offset);
		}

		public void ApplyStats(ShadowData s)
		{
			s.volume = this.volume.ToVector3();
			s.offset = this.offset.ToVector3();
		}
	}
}
