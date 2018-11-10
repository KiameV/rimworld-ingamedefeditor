using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class ToolStats
    {
        public string label;
        public float power;
        public float armorPenetration;
        public float cooldownTime;
        public List<ToolCapacityDefStat> capabilities;

        public ToolStats() { }
        public ToolStats(Tool t)
        {
            this.label = t.label;
            this.power = t.power;
            this.armorPenetration = t.armorPenetration;
            this.cooldownTime = t.cooldownTime;
            if (t.capacities != null)
            {
                this.capabilities = new List<ToolCapacityDefStat>();
                foreach (ToolCapacityDef d in t.capacities)
                {
                    ToolCapacityDefStat td = new ToolCapacityDefStat(d);
                    this.capabilities.Add(td);
                }
            }
        }

        public void ApplyStats(Tool to)
        {
            to.label = this.label;
            to.power = this.power;
            to.armorPenetration = this.armorPenetration;
            to.cooldownTime = this.cooldownTime;
            if (to.capacities != null)
            {
                to.capacities.Clear();
            }

            if (this.capabilities != null)
            {
                to.capacities = new List<ToolCapacityDef>(this.capabilities.Count);
                foreach (ToolCapacityDefStat s in this.capabilities)
                {
                    to.capacities.Add(s.Def);
                }
            }
        }

        public bool Initialize()
        {
            if (this.capabilities != null)
            {
                foreach (ToolCapacityDefStat s in this.capabilities)
                {
                    s.Initialize();
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
#if DEBUG
            Log.Warning("Equals:");
            Log.Warning(this.ToString());
            Log.Warning(obj.ToString());
#endif
            if (obj != null &&
                obj is ToolStats)
            {
                ToolStats t = obj as ToolStats;
                if (String.Equals(this.label, t.label) &&
                    this.power == t.power &&
                    this.armorPenetration == t.armorPenetration &&
                    this.cooldownTime == t.cooldownTime)
                {
                    // TODO Add Capabilities check
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.label.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(
                typeof(ToolStats).Name + Environment.NewLine +
                "    label: " + this.label + Environment.NewLine +
                "    power: " + this.power + Environment.NewLine +
                "    armorPenetration: " + this.armorPenetration + Environment.NewLine +
                "    cooldownTime: " + this.cooldownTime + Environment.NewLine + 
                "    Capabilities:" + Environment.NewLine);
            foreach (ToolCapacityDefStat s in this.capabilities)
            {
                sb.AppendLine("    " + s.defName);
            }
            return sb.ToString();
        }
    }

    public class ToolCapacityDefStat
    {
        [XmlIgnore]
        protected ToolCapacityDef def;

        [XmlElement(IsNullable = false)]
        public string defName;

        public ToolCapacityDef Def => this.def;
        public string DefName => this.def.defName;
        public string Label => this.def.label;

        public ToolCapacityDefStat() : base() { }
        public ToolCapacityDefStat(ToolCapacityDef def)
        {
            this.def = def;
            this.defName = this.def.defName;
        }

        public bool Initialize()
        {
            if (this.def == null)
            {
                def = DefDatabase<ToolCapacityDef>.AllDefsListForReading.Find(
                    delegate (ToolCapacityDef d) { return d.defName.Equals(this.defName); });

                if (this.def == null)
                {
                    Log.Error("Could not load def " + this.defName);
                }
            }

            return this.def != null;
        }
    }
}
