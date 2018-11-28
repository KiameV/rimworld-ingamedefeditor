using System;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats.DefStat
{
	[Serializable]
	public class FloatValueDoubleDefStat<D1, D2> : FloatValueDefStat<D1> where D1 : Def, new() where D2 : Def, new()
	{
        [XmlIgnore]
        private D2 def2;
        
        public string defName2 = null;

        public D2 Def2 => this.def2;
        public string DefName2 => this.defName2;

        public FloatValueDoubleDefStat() { }
        public FloatValueDoubleDefStat(D1 d1, D2 d2, float value = 0) : base(d1, value)
        {
            this.def2 = d2;
            if (this.def2 != null)
            {
                this.defName2 = this.def2.defName;
            }
        }

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;
            
            if (this.def2 == null && !string.IsNullOrEmpty(this.defName2))
            {
                if (!DefLookupUtil.TryGetDef(this.defName2, out this.def2))
                    Log.Error("Could not load def " + this.defName2);
                return this.def2 != null;
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
            return base.Def.GetHashCode() + ((this.Def2 != null) ? this.Def2.GetHashCode() : "none".GetHashCode());
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
