using System;
using Verse;
using System.Collections.Generic;
using System.Text;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class SimpleCurveStats
	{
		public List<Vector2Stats> points;

		public SimpleCurveStats() { }
		public SimpleCurveStats(SimpleCurve w)
		{
			Util.Populate(out points, w.Points, (v) => new Vector2Stats(v.x, v.y));
		}

		public void ApplyStats(SimpleCurve to)
		{
			if (to != null)
			{
				Util.Populate(out List<CurvePoint> l, this.points, v => new CurvePoint(v.ToVector2()));
				to.SetPoints(l);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is SimpleCurveStats s)
			{
				return Util.AreEqual(this.points, s.points, v => v.GetHashCode());
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
	}
}
