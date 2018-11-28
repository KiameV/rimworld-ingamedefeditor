using System;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class DamageGraphicDataStats
	{
		public bool enabled;
		public RectStats rectN;
		public RectStats rectE;
		public RectStats rectS;
		public RectStats rectW;
		public RectStats rect;
		public List<string> scratches;
		public string cornerTL;
		public string cornerTR;
		public string cornerBL;
		public string cornerBR;
		public string edgeLeft;
		public string edgeRight;
		public string edgeTop;
		public string edgeBot;

		public DamageGraphicDataStats() { }
		public DamageGraphicDataStats(DamageGraphicData d)
		{
			enabled = d.enabled;
			rectN = new RectStats(d.rectN);
			rectE = new RectStats(d.rectE);
			rectS = new RectStats(d.rectS);
			rectW = new RectStats(d.rectW);
			rect = new RectStats(d.rect);
			cornerTL = d.cornerTL;
			cornerTR = d.cornerTR;
			cornerBL = d.cornerBL;
			cornerBR = d.cornerBR;
			edgeLeft = d.edgeLeft;
			edgeRight = d.edgeRight;
			edgeTop = d.edgeTop;
			edgeBot = d.edgeBot;
			if (d.scratches != null)
			{
				scratches = new List<string>();
				scratches.AddRange(d.scratches);
			}
		}
	}
}
