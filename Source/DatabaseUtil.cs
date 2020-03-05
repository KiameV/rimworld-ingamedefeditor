using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace InGameDefEditor
{
    class DatabaseUtil
    {
        private static readonly MethodInfo removeDefMI = typeof(DefDatabase<Def>).GetMethod("Remove", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly FieldInfo shuffleableBackstoryListFI = typeof(BackstoryDatabase).GetField("shuffleableBackstoryList", BindingFlags.Static | BindingFlags.NonPublic);

        public static void Add(Backstory b)
        {
            BackstoryDatabase.AddBackstory(b);
        }

        public static void Add<D>(D d) where D : Def
        {
            DefDatabase<D>.Add(d);
        }

        public static bool Add(object o)
        {
            switch (o)
            {
                case Backstory b:
                    Add(b);
                    return true;
                case Def d:
                    Add(d);
                    return true;
            }
            return false;
        }

        public static void Remove(Backstory b)
        {
            BackstoryDatabase.allBackstories.Remove(b.identifier);
            if (shuffleableBackstoryListFI.GetValue(null) is Dictionary<Pair<BackstorySlot, BackstoryCategoryFilter>, List<Backstory>> dic)
            {
                Log.Message("Cleared BackstoryDatabase.shuffleableBackstoryList");
                dic.Clear();
            }
            else
            {
                Log.Warning("Failed to clear BackstoryDatabase.shuffleableBackstoryList");
            }
        }

        public static void Remove<D>(D d) where D : Def
        {
            removeDefMI.Invoke(null, new object[] { d });
        }

        public static bool Remove(object o)
        {
            switch (o)
            {
                case Backstory b:
                    Remove(b);
                    return true;
                case Def d:
                    Remove(d);
                    return true;
            }
            return false;
        }

        public static bool RemoveDef(string defName)
        {
            Def d = DefDatabase<Def>.GetNamed(defName, false);
            if (d != null)
            {
                Remove(d);
                return true;
            }
            return false;
        }

        public static bool RemoveBackstory(string identifier)
        {
            if (BackstoryDatabase.TryGetWithIdentifier(identifier, out Backstory b, false))
            {
                Remove(b);
                return true;
            }
            return false;
        }
    }
}
