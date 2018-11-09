using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Verse;

namespace InGameDefEditor
{
    public class Stats
    {
        [XmlIgnore]
        protected ThingDef def;

        [XmlElement(IsNullable = false)]
        public string defName;
        public List<Stat> StatModifiers = null;
        public List<VerbStats> VerbStats = null;
        public List<ToolStats> Tools = null;
        public List<Stat> EquippedStatOffsets = null;

        public ThingDef Def => this.def;
        public string DefName => this.def.defName;
        public string Label => this.def.label;
        public bool IsApparel => this.def.IsApparel;
        public bool IsWeapon => this.def.IsWeapon;

        public Stats() { }
        public Stats(ThingDef d)
        {
            this.def = d;
            this.defName = this.def.defName;
            this.SetStatModifiers(d.statBases);
            this.SetVerbs(d.Verbs);
            this.SetTools(d.tools);
            this.SetEquippedStatOffsets(d.equippedStatOffsets);
        }

        public bool Initialize()
        {
            if (this.def == null)
            {
                def = DefDatabase<ThingDef>.AllDefsListForReading.Find(
                    delegate (ThingDef d) { return d.defName.Equals(this.defName); });

                if (this.def == null)
                {
                    Log.Error("Could not load def " + this.defName);
                    return false;
                }
            }

            if (this.StatModifiers != null)
            {
                foreach (Stat s in this.StatModifiers)
                {
                    if (!s.Initialize())
                    {
                        Log.Error("Failed to initialize Stat Modifier " + s.DefName + " for " + this.defName);
                    }
                }
            }

            if (this.VerbStats != null)
            {
                foreach (VerbStats vs in this.VerbStats)
                {
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
                    if (!ts.Initialize())
                    {
                        Log.Error("Failed to initialize Tool " + ts.label + " for " + this.defName);
                    }
                }
            }

            if (this.EquippedStatOffsets != null)
            {
                foreach (Stat s in this.EquippedStatOffsets)
                {
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
            if (obj is Stats stats)
            {
                if (!string.Equals(this.DefName, stats.DefName))
                {
#if DEBUG
                    Log.Error(this.DefName + " DefNames not equal");
#endif
                    return false;
                }

                if (!ListsRoughlyEqual(this.StatModifiers, stats.StatModifiers) ||
                    !ListsRoughlyEqual(this.VerbStats, stats.VerbStats) ||
                    !ListsRoughlyEqual(this.Tools, stats.Tools) ||
                    !ListsRoughlyEqual(this.EquippedStatOffsets, stats.EquippedStatOffsets))
                {
#if DEBUG
                    Log.Error(this.DefName + " Lists not roughly equal");
#endif
                    return false;
                }

                if (this.StatModifiers != null)
                {
                    Dictionary<string, Stat> lookup = new Dictionary<string, Stat>();
                    foreach (Stat s in this.StatModifiers)
                    {
                        lookup.Add(s.DefName, s);
                    }

                    foreach (Stat s in stats.StatModifiers)
                    {
                        if (!lookup.TryGetValue(s.DefName, out Stat sFound) ||
                            sFound.value != s.value)
                        {
#if DEBUG
                            Log.Error(this.DefName + " " + s.DefName + " StatModifiers Not found");
#endif
                            return false;
                        }
                    }
                    lookup.Clear();
                    lookup = null;
                }

                if (this.VerbStats != null)
                {
                    Dictionary<string, VerbStats> lookup = new Dictionary<string, VerbStats>();
                    foreach (VerbStats vs in this.VerbStats)
                    {
                        lookup.Add(vs.name, vs);
                    }

                    foreach (VerbStats s in stats.VerbStats)
                    {
                        if (!lookup.TryGetValue(s.name, out VerbStats vsFound) ||
                            !vsFound.Equals(s))
                        {
#if DEBUG
                            Log.Error(this.DefName + " " + s.name + " VerbStats Not found");
#endif
                            return false;
                        }
                    }
                    lookup.Clear();
                    lookup = null;
                }

                if (this.Tools != null)
                {
                    Dictionary<string, ToolStats> lookup = new Dictionary<string, ToolStats>();
                    foreach (ToolStats ts in this.Tools)
                    {
                        lookup.Add(ts.label, ts);
                    }

                    foreach (ToolStats s in stats.Tools)
                    {
                        if (!lookup.TryGetValue(s.label, out ToolStats tsFound) ||
                            !tsFound.Equals(s))
                        {
#if DEBUG
                            Log.Error(this.DefName + " " + s.label + " Tools Not found");
#endif
                            return false;
                        }
                    }
                    lookup.Clear();
                    lookup = null;
                }

                if (this.EquippedStatOffsets != null)
                {
                    Dictionary<string, Stat> lookup = new Dictionary<string, Stat>();
                    foreach (Stat s in this.EquippedStatOffsets)
                    {
                        lookup.Add(s.DefName, s);
                    }

                    foreach (Stat s in stats.EquippedStatOffsets)
                    {
                        if (!lookup.TryGetValue(s.DefName, out Stat sFound) ||
                            sFound.value != s.value)
                        {
#if DEBUG
                            Log.Error(this.DefName + " " + s.DefName + " EquippedStatOffsets Not found");
#endif
                            return false;
                        }
                    }
                    lookup.Clear();
                    lookup = null;
                }
                return true;
            }
            return false;
        }

        private bool ListsRoughlyEqual<T>(List<T> l1, List<T> l2)
        {
            if ((l1 == null || l1.Count == 0) &&
                (l2 == null || l2.Count == 0))
            {
                return true;
            }
            if (l1 != null && l2 != null &&
                l1.Count == l2.Count)
            {
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
                    this.VerbStats.Add(new VerbStats(v));
                }
            }
        }

        public void SetEquippedStatOffsets(List<StatModifier> offsets)
        {
            if (offsets != null)
            {
                this.EquippedStatOffsets = new List<Stat>(offsets.Count);
                foreach (StatModifier m in offsets)
                {
                    this.EquippedStatOffsets.Add(new Stat(m.stat)
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
                this.StatModifiers = new List<Stat>(modifiers.Count);
                foreach (StatModifier m in modifiers)
                {
                    this.StatModifiers.Add(new Stat(m.stat)
                    {
                        value = m.value
                    });
                }
            }
        }

        public void ApplyStats(ThingDef d)
        {
#if DEBUG
            Log.Warning("ApplyStats for " + d.label);
#endif
            try
            {
                this.ApplyStatModifiers(d);
                this.ApplyVerbStats(d);
                this.ApplyTools(d);
                this.ApplyEquipmentStatOffsets(d);
            }
            catch (Exception e)
            {
                Log.Warning("Failed to apply stats [" + d.defName + "]\n" + e.Message);
            }
#if DEBUG
            Log.Warning("ApplyStats Done");
#endif
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
            
            foreach (Stat from in this.EquippedStatOffsets)
            {
                if (lookup.TryGetValue(from.DefName, out StatModifier to))
                {
                    to.value = from.value;
                }
                else
                {
                    d.equippedStatOffsets.Add(new StatModifier
                    {
                        stat = from.Def,
                        value = from.value
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

            foreach (Stat from in this.StatModifiers)
            {
                if (lookup.TryGetValue(from.DefName, out StatModifier to))
                {
                    to.value = from.value;
                }
                else
                {
                    d.statBases.Add(new StatModifier
                    {
                        stat = from.Def,
                        value = from.value
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
                Tool to;
                if (lookup.TryGetValue(from.label, out to))
                {
                    from.ApplyStats(to);
                }
                else
                {
                    to = new Tool();
                    from.ApplyStats(to);
                    d.tools.Add(to);
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
                foreach (Stat s in this.StatModifiers)
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
                foreach (Stat s in this.EquippedStatOffsets)
                    sb.AppendLine(s.ToString());
            }
            return sb.ToString();
        }
    }
}
