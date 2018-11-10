using InGameDefEditor.Stats.Misc;
using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    static class Util
    {
        public static bool ListsRoughlyEqual<T>(List<T> l, List<T> r)
        {
            if ((l == null || l.Count == 0) &&
                (r == null || r.Count == 0))
            {
                return true;
            }
            if (l != null && r != null &&
                l.Count == r.Count)
            {
                return true;
            }
            return false;
        }

        public static bool Equals<D>(List<DefStat<D>> l, List<DefStat<D>> r) where D : Def
        {
            if (!ListsRoughlyEqual(l, r))
                return false;

            if (l != null)
            {
                Dictionary<D, DefStat<D>> lookup = new Dictionary<D, DefStat<D>>();
                foreach (var v in l)
                    lookup[v.Def] = v;

                try
                {
                    foreach (var v in r)
                    {
                        if (lookup.TryGetValue(v.Def, out var found))
                        {
                            if (!v.Equals(found))
                            {
                                //Log.Error("Not Equal");
                                return false;
                            }
                        }
                        else
                        {
                            //Log.Error("Not Found");
                            return false;
                        }
                    }
                }
                finally
                {
                    lookup.Clear();
                    lookup = null;
                }
            }
            return true;
        }
    }
}
