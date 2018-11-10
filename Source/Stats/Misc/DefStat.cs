using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class DefStat<D> where D : Def
    {
        [XmlIgnore]
        private D def;

        [XmlElement(IsNullable = false)]
        public string defName;

        public D Def => this.def;
        public string DefName => this.def.defName;
        public string Label => this.def.label;

        public DefStat() { }
        public DefStat(D d)
        {
            this.def = d;
            this.defName = this.def.defName;
        }

        public bool Initialize(IEnumerable<D> defs)
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
            Log.Warning("Not Equals:");
            Log.Warning(this.ToString());
            Log.Warning(obj.ToString());
#endif
            if (obj != null &&
                obj is FloatValueStat<D> stat)
            {
                return string.Equals(this.defName, stat.defName);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.defName.GetHashCode();
        }

        public override string ToString()
        {
            return
                this.GetType().Name + Environment.NewLine +
                "    defName: " + this.defName + Environment.NewLine +
                "    def set: " + ((this.def == null) ? "no" : "yes") + Environment.NewLine;
        }
    }
}
