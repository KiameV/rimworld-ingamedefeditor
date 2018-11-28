using System.Collections.Generic;
using RimWorld;
using Verse;

namespace InGameDefEditor
{
    static class DefLookupUtil
    {
        // Key: type name
        // Value: Dictionary Key: defName
        //                   Value: Def
        private static Dictionary<string, Dictionary<string, Def>> defDic = null;

        public static void ClearDefDic()
        {
            if (defDic != null)
            {
                defDic.Clear();
                defDic = null;
            }
        }

        public static bool TryGetDef<D>(string defName, out D def) where D : Def, new()
        {
            if (defDic == null)
                defDic = new Dictionary<string, Dictionary<string, Def>>();

            string name = typeof(D).Name;
			if (!defDic.TryGetValue(name, out Dictionary<string, Def> dic))
			{
				dic = new Dictionary<string, Def>();
				foreach (var v in DefDatabase<D>.AllDefsListForReading)
					dic.Add(v.defName, v);
				defDic[name] = dic;
			}

			if (dic.TryGetValue(defName, out var d))
			{
				def = d as D;
				return true;
			}

			Log.Warning("Unable to find def [" + defName + "] of type [" + name + "]");
            def = null;
            return false;
        }

        public static IEnumerable<T> GetSortedDefs<T>(IEnumerable<T> items) where T : Def
        {
            SortedDictionary<string, T> dic = new SortedDictionary<string, T>();
            foreach (T t in items)
                dic.Add(t.defName, t);
            return dic.Values;
        }
    }
}
