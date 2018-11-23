using System;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats.DefStat
{
    public class DefStat<D> : IDefStat where D : Def
    {
        [XmlIgnore]
        private D def;

        [XmlElement(IsNullable = false)]
        public string defName;
        
        public D Def => this.def;
        public string DefName => this.def.defName;
        public string Label => this.def.label;
        public Def BaseDef => this.def;

        public DefStat() { }
        public DefStat(string defName)
        {
            this.def = null;
            this.defName = defName;
        }
        public DefStat(D d)
        {
            this.def = d;
            this.defName = this.def.defName;
        }

        public virtual bool Initialize()
        {
            if (this.def == null)
            {
                if (!DefLookupUtil.TryGetDef(this.defName, out this.def))
                    Log.Error("Could not load def " + this.defName);
            }
            return this.def != null;
        }

        public override bool Equals(object obj)
        {
            if (obj != null &&
                obj is DefStat<D> stat)
            {
                return string.Equals(this.defName, stat.defName);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Def.GetHashCode();
        }

        /*public virtual void ApplyStats(DefStat<D> to)
        {
            // Empty
        }*/

        public override string ToString()
        {
            return
                this.GetType().Name + Environment.NewLine +
                "    defName: " + this.defName + Environment.NewLine +
                "    def set: " + ((this.def == null) ? "no" : "yes") + Environment.NewLine;
        }
    }
}
