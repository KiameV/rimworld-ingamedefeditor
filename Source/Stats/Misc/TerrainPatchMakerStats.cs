using InGameDefEditor.Stats.DefStat;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
	[Serializable]
	public class TerrainPatchMakerStats : IInitializable
	{
        public List<MinMaxFloatDefStat<TerrainDef>> thresholds = new List<MinMaxFloatDefStat<TerrainDef>>();
        public float perlinFrequency;
        public float perlinLacunarity;
        public float perlinPersistence;
        public int perlinOctaves;
        public float minFertility;
        public float maxFertility;
        public int minSize;

		public string Label => perlinFrequency.ToString();

		public TerrainPatchMakerStats() { }
        public TerrainPatchMakerStats(TerrainPatchMaker m)
        {
            this.perlinFrequency = m.perlinFrequency;
            this.perlinLacunarity = m.perlinLacunarity;
            this.perlinPersistence = m.perlinPersistence;
            this.perlinOctaves = m.perlinOctaves;
            this.minFertility = m.minFertility;
            this.maxFertility = m.maxFertility;
            this.minSize = m.minSize;

            if (m.thresholds != null)
            {
                this.thresholds = new List<MinMaxFloatDefStat<TerrainDef>>(m.thresholds.Count);
                foreach (TerrainThreshold t in m.thresholds)
                {
                    this.thresholds.Add(new MinMaxFloatDefStat<TerrainDef>(t.terrain)
                    {
                        Min = t.min,
                        Max = t.max
                    });
                }
            }
        }

        public void ApplyStats(TerrainPatchMaker to)
        {
            to.perlinFrequency = this.perlinFrequency;
            to.perlinLacunarity = this.perlinLacunarity;
            to.perlinPersistence = this.perlinPersistence;
            to.perlinOctaves = this.perlinOctaves;
            to.minFertility = this.minFertility;
            to.minFertility = this.minFertility;
            to.maxFertility = this.maxFertility;
            to.minSize = this.minSize;

            if (to.thresholds != null)
                to.thresholds.Clear();

            if (this.thresholds != null && this.thresholds.Count > 0)
            {
                if (to.thresholds == null)
                    to.thresholds = new List<TerrainThreshold>(this.thresholds.Count);
                foreach (var v in this.thresholds)
                {
                    to.thresholds.Add(new TerrainThreshold()
                    {
                        terrain = v.Def,
                        min = v.Min,
                        max = v.Max
                    });
                }
            }
        }

        public bool Initialize()
        {
            if (this.thresholds != null)
                foreach (var v in this.thresholds)
                    v.Initialize();
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.perlinFrequency.ToString());
            sb.Append(this.perlinLacunarity.ToString());
            sb.Append(this.perlinOctaves.ToString());
            sb.Append(this.perlinPersistence.ToString());
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj != null &&
                obj is TerrainPatchMakerStats s)
            {
                if (this.perlinFrequency == s.perlinFrequency &&
                    this.perlinLacunarity == s.perlinLacunarity &&
                    this.perlinPersistence == s.perlinPersistence &&
                    this.perlinOctaves == s.perlinOctaves &&
                    this.minFertility == s.minFertility &&
                    this.minFertility == s.minFertility &&
                    this.maxFertility == s.maxFertility &&
                    this.minSize == s.minSize &&
                    Util.AreEqual(this.thresholds, s.thresholds))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
