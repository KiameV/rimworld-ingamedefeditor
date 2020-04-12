using RimWorld;
using System.Linq;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    class DefsDictionary
    {
        private readonly SortedDictionary<string, Backstory> backstories = new SortedDictionary<string, Backstory>();
        private readonly SortedDictionary<string, Def> defs = new SortedDictionary<string, Def>();

        public IEnumerable<Pair<string, object>> All
        {
            get
            {
                List<Pair<string, object>> l = new List<Pair<string, object>>();
                foreach (var kv in backstories)
                    l.Add(new Pair<string, object>(kv.Key, kv.Value));
                foreach (var kv in defs)
                    l.Add(new Pair<string, object>(kv.Key, kv.Value));
                return l;
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                List<string> l = new List<string>();
                l.AddRange(backstories.Keys);
                l.AddRange(defs.Keys);
                return l;
            }
        }

        public List<Backstory> Backstories => backstories.Values.ToList();
        public List<Def> Defs => defs.Values.ToList();

        public int Count
        {
            get
            {
                return backstories.Count
                    + defs.Count;
            }
        }

        public bool Add(object o)
        {
            switch (o)
            {
                case Backstory b:
                    backstories[b.identifier] = b;
                    return true;
                case Def d:
                    defs[d.defName] = d;
                    return true;
                case string s:
                    if (DatabaseUtil.TryGetFromString(s, out object obj))
                        return Add(obj);
                    break;
            }
            return false;
        }

        public bool AddDef(string defName)
        {
            if (DatabaseUtil.TryGetFromString(defName, out object o) && o is Def d)
            {
                defs[d.defName] = d;
                return true;
            }
            return false;
        }

        public bool AddBackstory(string identifier)
        {
            if (BackstoryDatabase.TryGetWithIdentifier(identifier, out Backstory b, false))
            {
                backstories[b.identifier] = b;
                return true;
            }
            return false;
        }

        public void Add(Backstory b)
        {
            this.backstories[b.identifier] = b;
        }

        public void Add(Def def)
        {
            this.defs[def.defName] = def;
        }

        public bool ContainsBackstory(string identifier)
        {
            return backstories.ContainsKey(identifier);
        }

        public bool ContainsBackstory(Backstory b)
        {
            return backstories.ContainsKey(b.identifier);
        }

        public bool ContainsDef(string defName)
        {
            return defs.ContainsKey(defName);
        }

        public bool ContainsDef(Def d)
        {
            return (d == null) ? false : defs.ContainsKey(d.defName);
        }

        public bool Contains(object o)
        {
            switch (o)
            {
                case Backstory b:
                    return backstories.ContainsKey(b.identifier);
                case Def d:
                    return defs.ContainsKey(d.defName);
            }
            return false;
        }

        public bool Remove(object o)
        {
            switch (o)
            {
                case Backstory b:
                    return backstories.Remove(b.identifier);
                case Def d:
                    return defs.Remove(d.defName);
                case string s:
                    if (DatabaseUtil.TryGetFromString(s, out object obj))
                        return Remove(o);
                    break;
            }
            return false;
        }

        internal void Clear()
        {
            backstories.Clear();
            defs.Clear();
        }
        /*
        private readonly SortedDictionary<string, Backstory> backstories = new SortedDictionary<string, Backstory>();
        private readonly SortedDictionary<string, BiomeDef> biomeDefs = new SortedDictionary<string, BiomeDef>();
        private readonly SortedDictionary<string, DifficultyDef> difficultyDefs = new SortedDictionary<string, DifficultyDef>();
        private readonly SortedDictionary<string, RecipeDef> recipeDefs = new SortedDictionary<string, RecipeDef>();
        private readonly SortedDictionary<string, ThoughtDef> thoughtDefs = new SortedDictionary<string, ThoughtDef>();
        private readonly SortedDictionary<string, TraitDef> traitDefs = new SortedDictionary<string, TraitDef>();
        private readonly SortedDictionary<string, ThingDef> thingDefs = new SortedDictionary<string, ThingDef>();

        public IEnumerable<string> Keys
        {
            get
            {
                List<string> l = new List<string>();
                l.AddRange(backstories.Keys);
                l.AddRange(biomeDefs.Keys);
                l.AddRange(difficultyDefs.Keys);
                l.AddRange(recipeDefs.Keys);
                l.AddRange(thoughtDefs.Keys);
                l.AddRange(traitDefs.Keys);
                l.AddRange(thingDefs.Keys);
                return l;
            }
        }

        public int Count
        {
            get
            {
                return backstories.Count 
                    + biomeDefs.Count
                    + difficultyDefs.Count
                    + recipeDefs.Count
                    + thoughtDefs.Count 
                    + traitDefs.Count 
                    + thingDefs.Count;
            }
        }

        public bool Add(object o)
        {
            switch (o)
            {
                case Backstory b:
                    return backstories.ContainsKey(b.identifier);
                case BiomeDef d:
                    return biomeDefs.ContainsKey(d.defName);
                case DifficultyDef d:
                    return difficultyDefs.ContainsKey(d.defName);
                case RecipeDef d:
                    return recipeDefs.ContainsKey(d.defName);
                case ThoughtDef d:
                    return thoughtDefs.ContainsKey(d.defName);
                case TraitDef d:
                    return traitDefs.ContainsKey(d.defName);
                case ThingDef d:
                    return thingDefs.ContainsKey(d.defName);
            }
            return false;
        }

        public void Add(Backstory b)
        {
            this.backstories.Add(b.identifier, b);
        }

        public void Add(BiomeDef def)
        {
            this.biomeDefs.Add(def.defName, def);
        }

        public void Add(TraitDef def)
        {
            this.traitDefs.Add(def.defName, def);
        }

        public void Add(ThoughtDef def)
        {
            this.thoughtDefs.Add(def.defName, def);
        }

        public void Add(ThingDef def)
        {
            this.thingDefs.Add(def.defName, def);
        }

        public bool ContainsBackstory(string identifier)
        {
            return backstories.ContainsKey(identifier);
        }

        public bool ContainsBiomeDef(string defName)
        {
            return biomeDefs.ContainsKey(defName);
        }

        public bool ContainsThoughtDef(string defName)
        {
            return thoughtDefs.ContainsKey(defName);
        }

        public bool ContainsTraitDef(string defName)
        {
            return traitDefs.ContainsKey(defName);
        }

        public bool ContainsThingDef(string defName)
        {
            return thingDefs.ContainsKey(defName);
        }

        public bool Contains(object o)
        {
            switch (o)
            {
                case Backstory b:
                    return backstories.ContainsKey(b.identifier);
                case BiomeDef d:
                    return biomeDefs.ContainsKey(d.defName);
                case ThoughtDef d:
                    return thoughtDefs.ContainsKey(d.defName);
                case TraitDef d:
                    return traitDefs.ContainsKey(d.defName);
                case ThingDef d:
                    return thingDefs.ContainsKey(d.defName);
            }
            return false;
        }

        public bool Remove(object o)
        {
            switch (o)
            {
                case Backstory b:
                    return backstories.Remove(b.identifier);
                case BiomeDef d:
                    return biomeDefs.Remove(d.defName);
                case ThoughtDef d:
                    return thoughtDefs.Remove(d.defName);
                case TraitDef d:
                    return traitDefs.Remove(d.defName);
                case ThingDef d:
                    return thingDefs.Remove(d.defName);
            }
            return false;
        }

        internal void Clear()
        {
            backstories.Clear();
            biomeDefs.Clear();
            thoughtDefs.Clear();
            traitDefs.Clear();
            thingDefs.Clear();
        }*/
    }
}
