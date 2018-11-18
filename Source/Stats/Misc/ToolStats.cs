using InGameDefEditor.Stats.DefStat;
using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace InGameDefEditor.Stats.Misc
{
    public class ToolStats
    {
        public string label;
        public float power;
        public float armorPenetration;
        public float cooldownTime;
        public List<DefStat<ToolCapacityDef>> capabilities;

        public ToolStats() { }
        public ToolStats(Tool t)
        {
            this.label = t.label;
            this.power = t.power;
            this.armorPenetration = t.armorPenetration;
            this.cooldownTime = t.cooldownTime;
            if (t.capacities != null)
            {
                this.capabilities = new List<DefStat<ToolCapacityDef>>();
                foreach (ToolCapacityDef d in t.capacities)
                {
                    this.capabilities.Add(new DefStat<ToolCapacityDef>(d));
                }
            }
        }

        public void ApplyStats(Tool to)
        {
            to.label = this.label;
            to.power = this.power;
            to.armorPenetration = this.armorPenetration;
            to.cooldownTime = this.cooldownTime;

            if (this.capabilities != null && to.capacities == null)
                to.capacities = new List<ToolCapacityDef>(this.capabilities.Count);
            Util.Populate(to.capacities, this.capabilities, delegate (DefStat<ToolCapacityDef> s) { return s.Def; });
        }

        public bool Initialize()
        {
            if (this.capabilities != null)
                foreach (var v in this.capabilities)
                    v.Initialize();
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
                    this.cooldownTime == t.cooldownTime)// && 
                    //Util.AreEqual(this.capabilities, t.capabilities))
                {
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
            foreach (var v in this.capabilities)
            {
                sb.AppendLine("    " + v.defName);
            }
            return sb.ToString();
        }
    }
}
