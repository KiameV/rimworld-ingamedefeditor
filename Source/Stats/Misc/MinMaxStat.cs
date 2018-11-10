using System;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class MinMaxStat<D> where D : Def
    {
        [XmlIgnore]
        private D def;

        [XmlElement(IsNullable = false)]
        public string defName;

        public float Min;
        public float Max;

        public D Def => this.def;
        public string DefName => this.def.defName;
        public string Label => this.def.label;

        public MinMaxStat() { }
        public MinMaxStat(D d)
        {
            this.def = d;
            this.defName = this.def.defName;
        }

        public bool Initialize(System.Collections.Generic.IEnumerable<D> defs)
        {
            if (this.def == null)
            {
                foreach (D d in defs)
                {
                    if (d.defName.Equals(this.defName))
                    {
                        this.def = d;
                        return true;
                    }
                }
                Log.Error("Could not load def " + this.defName);
            }

            return this.def != null;
        }

        public override bool Equals(object obj)
        {
#if DEBUG
            Log.Warning("Equals:");
            Log.Warning(this.ToString());
            Log.Warning(obj.ToString());
#endif
            if (obj != null && 
                obj is MinMaxStat<D> stat)
            {
                return
                    string.Equals(this.defName, stat.defName) && 
                    this.Min == stat.Min &&
                    this.Max == stat.Max;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (this.defName + this.Min + this.Max).GetHashCode();
        }

        public override string ToString()
        {
            return
                typeof(FloatValueStat<D>).Name + Environment.NewLine +
                "    defName: " + this.defName + Environment.NewLine +
                "    def set: " + ((this.def == null) ? "no" : "yes") + Environment.NewLine +
                "    Min: " + this.Min + Environment.NewLine +
                "    Max: " + this.Max;
        }
    }
}
