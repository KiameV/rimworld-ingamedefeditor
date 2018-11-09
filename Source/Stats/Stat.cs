using RimWorld;
using System;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor
{
    public class Stat
    {
        [XmlIgnore]
        private StatDef def;

        [XmlElement(IsNullable = false)]
        public string defName;

        public float value;

        public StatDef Def => this.def;
        public string DefName => this.def.defName;
        public string Label => this.def.label;

        public Stat() { }
        public Stat(StatDef d)
        {
            this.def = d;
            this.defName = this.def.defName;
        }

        public bool Initialize()
        {
            if (this.def == null)
            {
                def = DefDatabase<StatDef>.AllDefsListForReading.Find(
                    delegate (StatDef d) { return d.defName.Equals(this.defName); });

                if (this.def == null)
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
            if (base.Equals(obj) && 
                obj is Stat stat)
            {
                return
                    string.Equals(this.def, stat.def) && 
                    this.value == stat.value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return
                typeof(Stat).Name + Environment.NewLine +
                "    defName: " + this.defName + Environment.NewLine +
                "    def set: " + ((this.def == null) ? "no" : "yes") + Environment.NewLine +
                "    value: " + this.value;
        }
    }
}
