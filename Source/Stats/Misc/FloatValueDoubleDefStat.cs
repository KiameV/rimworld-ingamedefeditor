using System;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class FloatValueDoubleDefStat<D1, D2> : FloatValueStat<D1> where D1 : Def where D2 : Def
    {
        [XmlIgnore]
        private D2 def2;
        
        [XmlElement(IsNullable = false)]
        public string defName2;

        public FloatValueDoubleDefStat() { }
        public FloatValueDoubleDefStat(D1 d1, D2 d2) : base(d1)
        {
            this.def2 = d2;
            this.defName2 = this.def2.defName;
        }

        public bool Initialize(System.Collections.Generic.IEnumerable<D1> defs1, System.Collections.Generic.IEnumerable<D2> defs2)
        {
            if (!base.Initialize(defs1))
                return false;

            if (this.def2 == null)
            {
                foreach (D2 d in defs2)
                {
                    if (d.defName.Equals(this.defName2))
                    {
                        this.def2 = d;
                        return true;
                    }
                }
                Log.Error("Could not load def " + this.defName2);
                return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) && 
                obj is FloatValueDoubleDefStat<D1, D2> stat)
            {
                return
                    base.Equals(obj) &&
                    string.Equals(this.defName2, stat.defName2);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + (this.defName2 + this.value).GetHashCode();
        }

        public override string ToString()
        {
            return
                base.ToString() + 
                "    defName2: " + this.defName2 + Environment.NewLine +
                "    def2 set: " + ((this.def2 == null) ? "no" : "yes");
        }
    }
}
