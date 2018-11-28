using System;
using Verse;
using System.Collections.Generic;
using System.Reflection;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class SimpleCurveStats
	{
		public List<Vector2Stats> points;

		public SimpleCurveStats() { }
		public SimpleCurveStats(SimpleCurve w)
		{
			Util.Populate(out points, GetPoints(w), (v) => new Vector2Stats(v.x, v.y));
		}

		public static List<CurvePoint> GetPoints(SimpleCurve c)
		{
			return (List<CurvePoint>)typeof(SimpleCurve).GetField("points", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(c);
		}

		public static void SetPoints(SimpleCurve c, List<CurvePoint> l)
		{
			typeof(SimpleCurve).GetField("points", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(c, l);
		}
	}
}
