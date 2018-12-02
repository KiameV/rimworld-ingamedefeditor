using System;
using RimWorld;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class ThoughtStageStats
	{
		[XmlElement(IsNullable = false)]
		public string label;
		public float baseMoodEffect;
		public float baseOpinionOffset;
		public bool visible = true;

		public bool isNull;

		public ThoughtStageStats() { }
		public ThoughtStageStats(ThoughtStage ts)
		{
			if (ts != null)
			{
				this.label = ts.label;
				this.baseMoodEffect = ts.baseMoodEffect;
				this.baseOpinionOffset = ts.baseOpinionOffset;
				this.visible = ts.visible;
				this.isNull = false;
			}
			else
			{
				this.isNull = true;
			}
		}

		public void ApplyStats(IEnumerable<ThoughtStage> s)
		{
			// TODO - This will not work when add/remove stages is added
			if (!this.isNull)
			{
				foreach (var v in s)
				{
					if (v != null && object.Equals(v.label, this.label))
					{
						v.label = this.label;
						v.baseMoodEffect = this.baseMoodEffect;
						v.baseOpinionOffset = this.baseOpinionOffset;
						v.visible = this.visible;
						break;
					}
				}
			}
		}

		public override string ToString()
		{
			if (this.isNull)
				return typeof(ThoughtDefStats).Name + " - is null";
			return
				typeof(ThoughtDefStats).Name + Environment.NewLine +
				"label: " + label + Environment.NewLine +
				"baseMoodEffect: " + baseMoodEffect + Environment.NewLine +
				"baseOpinionOffset: " + baseOpinionOffset + Environment.NewLine +
				"visible: " + visible;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj != null &&
				obj is ThoughtStageStats s)
			{
				if (this.isNull)
					return s.isNull;
				return
					string.Equals(this.label, s.label) &&
					this.baseMoodEffect == s.baseMoodEffect &&
					this.baseOpinionOffset == s.baseOpinionOffset &&
					this.visible == s.visible &&
					s.isNull == false;
			}
			return false;
		}
	}
}
