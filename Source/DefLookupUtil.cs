using System.Collections.Generic;
using Verse;

namespace InGameDefEditor
{
    static class DefLookupUtil
    {
        public static IEnumerable<T> GetSortedDefs<T>(IEnumerable<T> items) where T : Def
        {
            SortedDictionary<string, T> dic = new SortedDictionary<string, T>();
            foreach (T t in items)
            {
                dic.Add(t.defName, t);
            }
            return dic.Values;
        }
    }
}
