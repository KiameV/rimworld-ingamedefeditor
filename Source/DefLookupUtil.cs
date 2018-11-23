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
        public static bool TryGetDef<D>(string defName, out D def) where D : Def
        {
            if (defDic == null || defDic.Count == 0)
            {
                defDic = new Dictionary<string, Dictionary<string, Def>>();
                AddToDefDic(DefDatabase<ThingDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<StatDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<SoundDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<WeatherDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<TerrainDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<PawnKindDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<BiomeDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<ToolCapacityDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<IncidentDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<RecipeDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<ThoughtDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<TraitDef>.AllDefsListForReading);
                AddToDefDic(DefDatabase<SkillDef>.AllDefsListForReading);
            }

            string name = typeof(D).Name;
            if (defDic.TryGetValue(name, out Dictionary<string, Def> dic))
            {
                if (dic.TryGetValue(defName, out var d))
                {
                    def = d as D;
                    return true;
                }
                else
                    Log.Warning("Unable to find def [" + defName + "]");
            }
            else
                Log.Warning("Unable to find def type [" + name + "]");

            def = null;
            return false;
        }
        private static void AddToDefDic<D>(IEnumerable<D> defs) where D : Def
        {
            string name = typeof(D).Name;
            if (!defDic.TryGetValue(name, out Dictionary<string, Def> dic))
            {
                dic = new Dictionary<string, Def>();
                defDic[name] = dic;
            }

            foreach (Def d in defs)
            {
                dic[d.defName] = d;
            }
        }

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
