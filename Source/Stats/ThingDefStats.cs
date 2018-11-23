using InGameDefEditor.Stats.DefStat;
using InGameDefEditor.Stats.Misc;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace InGameDefEditor.Stats
{
    public class ThingDefStats : DefStat<ThingDef>, IParentStat
    {
        public List<FloatValueStat<StatDef>> StatModifiers = null;
        public List<VerbStats> VerbStats = null;
        public List<ToolStats> Tools = null;
        public List<FloatValueStat<StatDef>> EquippedStatOffsets = null;
        
        public bool IsApparel => base.Def.IsApparel;
        public bool IsWeapon => base.Def.IsWeapon;

        public ThingDefStats() { }
        public ThingDefStats(ThingDef d) : base(d)
        {
            this.SetStatModifiers(d.statBases);
            this.SetVerbs(d.Verbs);
            this.SetTools(d.tools);
            this.SetEquippedStatOffsets(d.equippedStatOffsets);
        }

        public override bool Initialize()
        {
            if (!base.Initialize())
                return false;

            if (this.StatModifiers != null)
            {
                foreach (var s in this.StatModifiers)
                {
                    //Log.Error(s.defName);
                    if (!s.Initialize())
                    {
                        Log.Error("Failed to initialize Stat Modifier " + s.DefName + " for " + this.defName);
                    }
                    //Log.Error("    Found");
                }
            }

            if (this.VerbStats != null)
            {
                foreach (VerbStats vs in this.VerbStats)
                {
                    //Log.Error(vs.name);
                    if (!vs.Initialize())
                    {
                        Log.Error("Failed to initialize Verb " + vs.name + " for " + this.defName);
                    }
                }
            }

            if (this.Tools != null)
            {
                foreach (ToolStats ts in this.Tools)
                {
                    //Log.Error(ts.label);
                    if (!ts.Initialize())
                    {
                        Log.Error("Failed to initialize Tool " + ts.label + " for " + this.defName);
                    }
                }
            }

            if (this.EquippedStatOffsets != null)
            {
                foreach (var s in this.EquippedStatOffsets)
                {
                    //Log.Error(s.defName);
                    if (!s.Initialize())
                    {
                        Log.Error("Failed to initialize Equipped Stat Offsets " + s.DefName + " for " + this.defName);
                    }
                }
            }


            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj) &&
                obj is ThingDefStats stats)
            {
                if (!Util.AreEqual(this.StatModifiers, stats.StatModifiers) ||
                    !Util.AreEqual(this.EquippedStatOffsets, stats.EquippedStatOffsets) || 
                    !Util.AreEqual(this.VerbStats, stats.VerbStats, null) || 
                    !Util.AreEqual(this.Tools, stats.Tools, null))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public void SetTools(List<Tool> tools)
        {
            if (tools != null)
            {
                this.Tools = new List<ToolStats>(tools.Count);
                foreach (Tool t in tools)
                {
                    this.Tools.Add(new ToolStats(t));
                }
            }
        }

        public void SetVerbs(List<VerbProperties> verbs)
        {
            if (verbs != null)
            {
                this.VerbStats = new List<VerbStats>(verbs.Count);
                foreach (VerbProperties v in verbs)
                {
					try
					{
						if (v != null)
						{
							VerbStats verb = new VerbStats(v);
							this.VerbStats.Add(verb);
						}
					}
					catch(Exception e)
					{
						Log.Warning("Poorly formatted VerbProperties in " + this.Def.defName + Environment.NewLine + e.Message);
					}
                }
            }
        }

        public void SetEquippedStatOffsets(List<StatModifier> offsets)
        {
            if (offsets != null)
            {
                this.EquippedStatOffsets = new List<FloatValueStat<StatDef>>(offsets.Count);
                foreach (StatModifier m in offsets)
                {
                    this.EquippedStatOffsets.Add(new FloatValueStat<StatDef>(m.stat)
                    {
                        value = m.value
                    });
                }
            }
        }

        public void SetStatModifiers(List<StatModifier> modifiers)
        {
            if (modifiers != null)
            {
                this.StatModifiers = new List<FloatValueStat<StatDef>>(modifiers.Count);
                foreach (StatModifier m in modifiers)
                {
                    this.StatModifiers.Add(new FloatValueStat<StatDef>(m.stat)
                    {
                        value = m.value
                    });
                }
            }
        }

        public void ApplyStats(Def to)
        {
#if DEBUG_THINGDEF
            Log.Warning("ApplyStats for " + this.defName);
#endif
            if (to is ThingDef t)
            {
                try
                {
                    this.ApplyStatModifiers(t);
                    this.ApplyVerbStats(t);
                    this.ApplyTools(t);
                    this.ApplyEquipmentStatOffsets(t);
                }
                catch (Exception e)
                {
                    Log.Warning("Failed to apply stats [" + t.defName + "]\n" + e.Message);
                }
#if DEBUG_THINGDEF
            Log.Warning("ApplyStats Done");
#endif
            }
            else
                Log.Error("ThingDefStat passed none ThingDef!");
        }

        private void ApplyEquipmentStatOffsets(ThingDef d)
        {
            if (d.equippedStatOffsets != null)
                d.equippedStatOffsets.Clear();

            if (this.EquippedStatOffsets == null || this.EquippedStatOffsets.Count == 0)
                return;

            if (d.equippedStatOffsets == null)
                d.equippedStatOffsets = new List<StatModifier>(this.EquippedStatOffsets.Count);

            Dictionary<string, StatModifier> lookup = new Dictionary<string, StatModifier>();
            foreach (StatModifier to in d.equippedStatOffsets)
                lookup.Add(to.stat.defName, to);
            
            foreach (var from in this.EquippedStatOffsets)
            {
                if (lookup.TryGetValue(from.DefName, out StatModifier to))
                {
                    to.value = ((FloatValueStat<StatDef>)from).value;
                }
                else
                {
                    d.equippedStatOffsets.Add(new StatModifier
                    {
                        stat = from.Def,
                        value = ((FloatValueStat<StatDef>)from).value
                    });
                }
            }
        }

        private void ApplyStatModifiers(ThingDef d)
        {
            if (d.statBases != null)
                d.statBases.Clear();

            if (this.StatModifiers == null || this.StatModifiers.Count == 0)
                return;

            if (d.statBases == null)
                d.statBases = new List<StatModifier>(this.StatModifiers.Count);

            Dictionary<string, StatModifier> lookup = new Dictionary<string, StatModifier>();
            foreach (StatModifier m in d.statBases)
            {
                lookup.Add(m.stat.ToString(), m);
            }

            foreach (var from in this.StatModifiers)
            {
                if (lookup.TryGetValue(from.DefName, out StatModifier to))
                {
                    to.value = ((FloatValueStat<StatDef>)from).value;
                }
                else
                {
                    d.statBases.Add(new StatModifier
                    {
                        stat = from.Def,
                        value = ((FloatValueStat<StatDef>)from).value
                    });
                }
            }
            lookup.Clear();
            lookup = null;
        }

        private void ApplyVerbStats(ThingDef d)
        {
            if (d.Verbs == null || d.Verbs.Count == 0)
                return;

            if (this.VerbStats == null || this.VerbStats.Count == 0)
            {
                Log.Error("Null or Empty verbs " + this.Def.defName);
                return;
            }

            Dictionary<string, VerbProperties> lookup = new Dictionary<string, VerbProperties>();
            foreach (VerbProperties v in d.Verbs)
            {
                lookup.Add(v.verbClass.Name, v);
            }
            
            foreach (VerbStats from in this.VerbStats)
            {
                if (lookup.TryGetValue(from.name, out VerbProperties to))
                {
                    from.ApplyStats(to);
                }
            }
            lookup.Clear();
            lookup = null;
        }

        private void ApplyTools(ThingDef d)
        {
            if (d.tools != null)
                d.tools.Clear();

            if (this.Tools == null || this.Tools.Count == 0)
                return;

            if (d.tools == null)
                d.tools = new List<Tool>(this.Tools.Count);

            Dictionary<string, Tool> lookup = new Dictionary<string, Tool>();
            foreach (Tool to in d.tools)
            {
                lookup.Add(to.label, to);
            }

            foreach (ToolStats from in this.Tools)
            {
                if (lookup.TryGetValue(from.label, out Tool to))
                {
                    from.ApplyStats(to);
                }
                else
                {
                    Tool t = new Tool();
                    from.ApplyStats(t);
                    d.tools.Add(t);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Stats:");
            sb.Append(Environment.NewLine);
            sb.AppendLine("    DefName: " + this.defName);
            sb.AppendLine("    StatModifiers: " + ((this.StatModifiers == null) ? "null" : ""));
            if (this.StatModifiers != null)
            {
                foreach (var s in this.StatModifiers)
                    sb.AppendLine(s.ToString());
            }
            sb.AppendLine("    VerbStats: " + ((this.VerbStats == null) ? "null" : ""));
            if (this.VerbStats != null)
            {
                foreach (VerbStats s in this.VerbStats)
                    sb.AppendLine(s.ToString());
            }
            sb.AppendLine("    Tools: " + ((this.Tools == null) ? "null" : ""));
            if (this.Tools != null)
            {
                foreach (ToolStats s in this.Tools)
                    sb.AppendLine(s.ToString());
            }
            sb.AppendLine("    EquippedStatOffsets: " + ((this.EquippedStatOffsets == null) ? "null" : ""));
            if (this.EquippedStatOffsets != null)
            {
                foreach (var s in this.EquippedStatOffsets)
                    sb.AppendLine(s.ToString());
            }
            return sb.ToString();
        }
    }
}
