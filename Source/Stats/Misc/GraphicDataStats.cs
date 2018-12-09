using InGameDefEditor.Stats.DefStat;
using System;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class GraphicDataStats
	{
		public string texPath;
		//public Type graphicClass;
		public DefStat<ShaderTypeDef> shaderType;
		//public List<ShaderParameterStats> shaderParameters;
		public ColorStats color;
		public ColorStats colorTwo;
		public Vector2Stats drawSize;
		public float onGroundRandomRotateAngle;
		public bool drawRotated;
		public bool allowFlip;
		public float flipExtraRotation;
		public LinkDrawerType linkType;
		public LinkFlags linkFlags;

		public ShadowDataStats shadowData;
		public DamageGraphicDataStats damageData;

		public GraphicDataStats() { }
		public GraphicDataStats(GraphicData d)
		{
			this.texPath = d.texPath;
			//this.graphicClass = d.graphicClass;
			Util.AssignDefStat(d.shaderType, out this.shaderType);
			this.color = new ColorStats(d.color);
			this.colorTwo = new ColorStats(d.colorTwo);
			this.drawSize = new Vector2Stats(d.drawSize);
			this.onGroundRandomRotateAngle = d.onGroundRandomRotateAngle;
			this.drawRotated = d.drawRotated;
			this.allowFlip = d.allowFlip;
			this.flipExtraRotation = d.flipExtraRotation;
			if (d.shadowData != null)
				this.shadowData = new ShadowDataStats(d.shadowData);
			if (d.damageData != null)
				this.damageData = new DamageGraphicDataStats(d.damageData);
			this.linkType = d.linkType;
			this.linkFlags = d.linkFlags;
		}

		public bool Initialize()
		{
			this.shaderType?.Initialize();
			return true;
		}
	}
}
