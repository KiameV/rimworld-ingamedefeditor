using System;
using Verse;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

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

		public void ApplyStats(SimpleCurve to)
		{
			Util.Populate(out List<CurvePoint> l, this.points, v => new CurvePoint(v.ToVector2()));
			SetPoints(to, l);
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is SimpleCurveStats s)
			{
				return Util.AreEqual(this.points, s.points, (l, r) => l.x == r.x && l.y == r.y);
			}
			return false;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder("SimpleCurveStats\n");
			if (points != null)
				this.points.ForEach(v => sb.AppendLine(v.ToString()));
			return sb.ToString();
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
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
