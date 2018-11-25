using InGameDefEditor.Stats.DefStat;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class ApparelPropertiesStats
    {
        public string wornGraphicPath;
        public float wearPerDay;
        public bool careIfWornByCorpse;
        public bool hatRenderedFrontOfFace;
        public bool useDeflectMetalEffect;

        public List<string> tags = new List<string>();
        public List<string> defaultOutfitTags;

        public List<DefStat<BodyPartGroupDef>> bodyPartGroups = new List<DefStat<BodyPartGroupDef>>();
        public List<DefStat<ApparelLayerDef>> layers = new List<DefStat<ApparelLayerDef>>();

        public ApparelPropertiesStats() { }
        public ApparelPropertiesStats(ApparelProperties a)
        {
            this.wornGraphicPath = a.wornGraphicPath;
            this.wearPerDay = a.wearPerDay;
            this.careIfWornByCorpse = a.careIfWornByCorpse;
            this.hatRenderedFrontOfFace = a.hatRenderedFrontOfFace;
            this.useDeflectMetalEffect = a.useDeflectMetalEffect;

            this.tags = Util.CreateList(a.tags);
            this.defaultOutfitTags = Util.CreateList(a.defaultOutfitTags);
            this.bodyPartGroups = Util.CreateDefStatList(a.bodyPartGroups);
            this.layers = Util.CreateDefStatList(a.layers);
        }

        public void ApplyStats(ApparelProperties to)
        {
            to.wornGraphicPath = this.wornGraphicPath;
            to.wearPerDay = this.wearPerDay;
            to.careIfWornByCorpse = this.careIfWornByCorpse;
            to.hatRenderedFrontOfFace = this.hatRenderedFrontOfFace;
            to.useDeflectMetalEffect = this.useDeflectMetalEffect;

            Util.Populate(to.tags, this.tags);
            Util.Populate(to.defaultOutfitTags, this.defaultOutfitTags);
            to.bodyPartGroups = Util.ConvertDefStats(this.bodyPartGroups);
            to.layers = Util.ConvertDefStats(this.layers);

            ApparelProperties.ResetStaticData();
            typeof(ApparelProperties).GetField("cachedHumanBodyCoverage", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(to, -1);
            typeof(ApparelProperties).GetField("interferingBodyPartGroups", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(to, null);
        }

        public bool Initialize()
        {
            Util.InitializeDefStat(this.bodyPartGroups);
            Util.InitializeDefStat(this.layers);
            return true;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) &&
                obj is ApparelPropertiesStats s)
            {
                if (this.wornGraphicPath.Equals(s.wornGraphicPath) && 
                    this.wearPerDay == s.wearPerDay &&
                    this.careIfWornByCorpse == s.careIfWornByCorpse &&
                    this.hatRenderedFrontOfFace == s.hatRenderedFrontOfFace &&
                    this.useDeflectMetalEffect == s.useDeflectMetalEffect &&
                    Util.AreEqual(this.tags, s.tags) &&
                    Util.AreEqual(this.defaultOutfitTags, s.defaultOutfitTags) &&
                    Util.AreEqual(this.bodyPartGroups, s.bodyPartGroups) &&
                    Util.AreEqual(this.layers, s.layers))
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
