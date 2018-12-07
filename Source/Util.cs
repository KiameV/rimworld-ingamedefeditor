using InGameDefEditor.Stats.DefStat;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using System;
using InGameDefEditor.Stats;
using InGameDefEditor.Stats.Misc;

namespace InGameDefEditor
{
	static class Util
	{
		public static bool IsNullEmpty<T>(List<T> l)
		{
			return l == null || l.Count == 0;
		}

		public static bool FloatsRoughlyEqual(float l, float r)
		{
			if (l == r)
				return true;
			if (l > 1e30f && r > 1e30f)
				return true;
			if (l < -1e30f && r < -1e30f)
				return true;
			if (Math.Abs(l - r) < 0.001f)
				return true;
			return false;
		}

		public static bool ListsRoughlyEqual<T>(IEnumerable<T> l, IEnumerable<T> r)
		{
			//Log.Error("L: " + ((l == null) ? "null" : l.Count().ToString()) + " R: " + ((r == null) ? "null" : r.Count().ToString()));
			if ((l == null || (l != null && l.Count() == 0)) &&
				(r == null || (r != null && r.Count() == 0)))
			{
				return true;
			}
			if (l != null && r != null &&
				l.Count() == r.Count())
			{
				return true;
			}
			return false;
		}

		public delegate int GetKey<T>(T t);
		public static bool AreEqual<T>(IEnumerable<T> l, IEnumerable<T> r, GetKey<T> getKey)
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			if (getKey == null)
			{
				int count = 0;
				foreach (var i in l)
					foreach (var j in r)
						if (object.Equals(i, j))
						{
							++count;
							break;
						}

				return count == l.Count();
			}

			Dictionary<int, T> lookup = new Dictionary<int, T>();
			foreach (var v in l)
				lookup[getKey(v)] = v;
			
			foreach (var v in r)
			{
				if (!lookup.TryGetValue(getKey(v), out var found))
				{
					//Log.Error("Not Found");
					return false;
				}
				if (!v.Equals(found))
				{
					//Log.Error("Not Equal");
					return false;
				}
			}
			lookup.Clear();
			lookup = null;
			return true;
		}

		public static bool AreEqual(IEnumerable<IDefStat> l, IEnumerable<IDefStat> r)
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			return AreEqual(l, r, v => v.BaseDef.GetHashCode());
		}

		public static bool AreEqual<D>(IEnumerable<DefStat<D>> l, IEnumerable<DefStat<D>> r) where D : Def, new()
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			return AreEqual(l, r, v => v.GetHashCode());
		}

		public static bool AreEqual<D>(IEnumerable<FloatValueDefStat<D>> l, IEnumerable<FloatValueDefStat<D>> r) where D : Def, new()
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			Dictionary<Def, IDefStat> lookup = new Dictionary<Def, IDefStat>();
			foreach (var v in l)
				lookup[v.BaseDef] = v;

			return AreEqual(l, r, v => v.GetHashCode());
		}

		public static bool AreEqual<D>(IEnumerable<IntValueDefStat<D>> l, IEnumerable<IntValueDefStat<D>> r) where D : Def, new()
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			Dictionary<Def, IDefStat> lookup = new Dictionary<Def, IDefStat>();
			foreach (var v in l)
				lookup[v.BaseDef] = v;

			return AreEqual(l, r, v => v.GetHashCode());
		}

		public static bool AreEqual<D1, D2>(List<FloatValueDoubleDefStat<D1, D2>> l, IEnumerable<FloatValueDoubleDefStat<D1, D2>> r) where D1 : Def, new() where D2 : Def, new()
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			return AreEqual(l, r, v => v.GetHashCode());
		}

		public static bool AreEqual<D>(List<MinMaxFloatDefStat<D>> l, IEnumerable<MinMaxFloatDefStat<D>> r) where D : Def, new()
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			return AreEqual(l, r, v => v.GetHashCode());
		}

		public static bool AreEqual<T>(DefStat<T> l, DefStat<T> r) where T : Def, new()
		{
			return
				l == null && r == null ||
				(l != null && r != null && l.Equals(r));

		}

		public delegate T CreateItem<T, U>(U u);
		public static void Populate<T, U>(List<T> to, List<U> from, CreateItem<T, U> createItem)
        {
            to.Clear();

            if (!IsNullEmpty(from))
			{
				foreach (U u in from)
					to.Add(createItem(u));
			}
		}

		public delegate T CreateItemInt<T, U>(U u, int v);
		public static void Populate<T, U>(out List<T> to, Dictionary<U, int> from, CreateItemInt<T, U> createItem, bool nullifyOutput = false)
		{
			if (from == null ||
				from.Count == 0)
			{
				if (nullifyOutput)
					to = null;
				else
					to = new List<T>();
			}
			else
			{
				to = new List<T>(from.Count);
				foreach (KeyValuePair<U, int> kv in from)
					to.Add(createItem(kv.Key, kv.Value));
			}
		}

		public static void Populate<T>(out List<T> to, List<T> from, bool nullifyOutput = false)
		{
			if (from == null ||
				from.Count == 0)
			{
				if (nullifyOutput)
					to = null;
				else
					to = new List<T>();
			}
			else
				to = new List<T>(from);
		}

		public static void Populate<T, U>(out List<T> to, IEnumerable<U> from, CreateItem<T, U> createItem, bool nullifyOutput = false)
		{
			if (from == null ||
				from.Count() == 0)
			{
				if (nullifyOutput)
					to = null;
				else
					to = new List<T>();
			}
			else
			{
				to = new List<T>(from.Count());
				foreach (U u in from)
				{
					var v = createItem(u);
					if (v != null)
						to.Add(v);
				}
			}
		}

		public static void Populate<T>(List<T> to, List<T> from)
        {
            to.Clear();

            if (!IsNullEmpty(from))
            {
                foreach (T t in from)
                    to.Add(t);
            }
        }

		public delegate void ApplyStats<F, T>(F from, T to);
		public static void ListIndexAssign<F, T>(List<F> from, List<T> to, ApplyStats<F, T> applyStats)
		{
			if (from != null && to != null && from.Count == to.Count && applyStats != null)
			{
				for (int i = 0; i < from.Count; ++i)
					applyStats(from[i], to[i]);
			}
		}

		public delegate U Convert<T, U>(T t);

		internal static IEnumerable<D> SortedDefList<D>() where D : Def, new()
		{
			List<D> list = DefDatabase<D>.AllDefsListForReading;
			list.Sort((l, r) => Util.GetDefLabel(l).CompareTo(Util.GetDefLabel(r)));
			return list;
		}

		public static IEnumerable<U> ConvertItems<T, U>(IEnumerable<T> t, Convert<T, U> convert)
		{
			List<U> u = new List<U>();
			foreach (var v in t)
				u.Add(convert(v));
			return u;
		}

		public static string IsNull<T>(string label, T t)
		{
			return label + " is " + ((t == null) ? "null" : "not null");
		}

		public static T Assign<T, U>(U from, CreateItem<T, U> createItem)
		{
			if (from == null)
				return default(T);
			return createItem(from);
		}

		public static void AssignDefStat<D>(D def, out DefStat<D> stat) where D : Def, new()
		{
			if (def == null)
				stat = null;
			else
				stat = new DefStat<D>(def);
		}

		public static DefStat<D> AssignDefStat<D>(D def) where D : Def, new()
		{
			if (def == null)
				return null;
			return new DefStat<D>(def);
		}

		public static void AssignDef<D>(DefStat<D> from, out D def) where D : Def, new()
		{
			if (from == null)
				def = null;
			else
				def = from.Def;
		}

		public static D AssignDef<D>(DefStat<D> from) where D : Def, new()
		{
			if (from == null)
				return null;
			return from.Def;
		}

		public static bool InitializeDefStat(IInitializable i)
		{
			if (i != null)
				if (!i.Initialize())
				{
					Log.Warning("Failed to initialize DefStat " + i.Label);
					return false;
				}
			return true;
		}

		public static bool InitializeDefStat<D>(IEnumerable<DefStat<D>> stats) where D : Def, new()
		{
			bool isInitialized = true;
			if (stats != null)
				foreach (DefStat<D> s in stats)
					if (!s.Initialize())
					{
						Log.Warning("Failed to initialize DefStat " + s.defName);
						isInitialized = false;
					}
			return isInitialized;
		}

		public static bool InitializeDefStat<D>(IEnumerable<IntValueDefStat<D>> stats) where D : Def, new()
		{
			bool isInitialized = true;
			if (stats != null)
				foreach (DefStat<D> s in stats)
					if (!s.Initialize())
					{
						Log.Warning("Failed to initialize DefStat " + s.defName);
						isInitialized = false;
					}
			return isInitialized;
		}

		public static List<T> CreateList<T>(IEnumerable<T> from)
		{
			if (from == null)
				return null;
			return new List<T>(from);
		}

		public static List<DefStat<D>> CreateDefStatList<D>(IEnumerable<D> from) where D : Def, new()
		{
			if (from == null)
				return null;
			List<DefStat<D>> l = new List<DefStat<D>>(from.Count());
			foreach (var v in from)
				l.Add(new DefStat<D>(v));
			return l;
		}

		public static HashSet<T> CreateHashSet<T>(IEnumerable<T> from)
		{
			if (from == null)
				return null;
			return new HashSet<T>(from);
		}

		public static HashSet<DefStat<D>> CreateDefStatHashSet<D>(IEnumerable<D> from) where D : Def, new()
		{
			if (from == null)
				return null;
			HashSet<DefStat<D>> hs = new HashSet<DefStat<D>>();
			foreach (var v in from)
				hs.Add(new DefStat<D>(v));
			return hs;
		}

		public static HashSet<D> ConvertDefStats<D>(HashSet<DefStat<D>> from) where D : Def, new()
		{
			if (from == null)
				return null;
			HashSet<D> to = new HashSet<D>();
			foreach (var v in from)
				to.Add(v.Def);
			return to;
		}

		public static List<D> ConvertDefStats<D>(List<DefStat<D>> from) where D : Def, new()
		{
			if (from == null)
				return null;
			List<D> to = new List<D>(from.Count);
			foreach (var v in from)
				to.Add(v.Def);
			return to;
		}

		public static string GetDisplayLabel(string s)
		{
			if (string.IsNullOrEmpty(s))
				return "None";
			return s;
		}

		public static List<T> CreateIfNeeded<T>(List<T> l)
		{
			if (l == null)
				l = new List<T>();
			return l;
		}

		public static List<T> AddTo<T>(List<T> l, T t)
		{
			if (l == null)
				l = new List<T>();
			l.Add(t);
			return l;
		}

		public static List<T> NullIfNeeded<T>(List<T> l)
		{
			if (l != null && l.Count == 0)
				l = null;
			return l;
		}

		public static List<T> RemoveFrom<T>(List<T> l, T t, bool nullifyIfEmpty = false)
		{
			if (l != null)
			{
				l.Remove(t);
				if (nullifyIfEmpty && l.Count == 0)
					l = null;
			}
			return l;
		}

		public static string GetDefLabel<D>(D d) where D : Def, new()
		{
			if (d == null)
				return "None";
			return d.label ?? d.defName;
		}
	}
}
