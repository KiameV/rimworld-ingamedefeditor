using InGameDefEditor.Stats.DefStat;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using System;

namespace InGameDefEditor
{
	static class Util
	{
		public static bool IsNullEmpty<T>(List<T> l)
		{
			return l == null || l.Count == 0;
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

		public static bool AreEqual<T>(IEnumerable<T> left, IEnumerable<T> right)
		{
			if (!ListsRoughlyEqual(left, right))
				return false;

			if (left == null || right == null)
				return true;

			int count = 0;
			foreach (var l in left)
				foreach (var r in right)
					if (object.Equals(l, r))
					{
						++count;
						break;
					}

			return count == left.Count();
		}

		public static bool AreEqual(IEnumerable<IDefStat> l, IEnumerable<IDefStat> r)
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			Dictionary<Def, IDefStat> lookup = new Dictionary<Def, IDefStat>();
			foreach (var v in l)
				lookup[v.BaseDef] = v;

			try
			{
				foreach (var v in r)
				{
					if (lookup.TryGetValue(v.BaseDef, out var found))
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
			return true;
		}

		public static bool AreEqual<D>(IEnumerable<DefStat<D>> l, IEnumerable<DefStat<D>> r) where D : Def
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			Dictionary<Def, IDefStat> lookup = new Dictionary<Def, IDefStat>();
			foreach (var v in l)
				lookup[v.BaseDef] = v;

			try
			{
				foreach (var v in r)
				{
					if (lookup.TryGetValue(v.BaseDef, out var found))
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
			return true;
		}

		public static bool AreEqual<D>(IEnumerable<FloatValueStat<D>> l, IEnumerable<FloatValueStat<D>> r) where D : Def
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			Dictionary<Def, IDefStat> lookup = new Dictionary<Def, IDefStat>();
			foreach (var v in l)
				lookup[v.BaseDef] = v;

			try
			{
				foreach (var v in r)
				{
					if (lookup.TryGetValue(v.BaseDef, out var found))
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
			return true;
		}

		public static bool AreEqual<D1, D2>(List<FloatValueDoubleDefStat<D1, D2>> l, IEnumerable<FloatValueDoubleDefStat<D1, D2>> r) where D1 : Def where D2 : Def
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			Dictionary<Def, IDefStat> lookup = new Dictionary<Def, IDefStat>();
			foreach (var v in l)
				lookup[v.BaseDef] = v;

			try
			{
				foreach (var v in r)
				{
					if (lookup.TryGetValue(v.BaseDef, out var found))
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
			return true;
		}

		public static bool AreEqual<D>(List<MinMaxDefStat<D>> l, IEnumerable<MinMaxDefStat<D>> r) where D : Def
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			Dictionary<Def, IDefStat> lookup = new Dictionary<Def, IDefStat>();
			foreach (var v in l)
				lookup[v.BaseDef] = v;

			try
			{
				foreach (var v in r)
				{
					if (lookup.TryGetValue(v.BaseDef, out var found))
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
			return true;
		}

		public delegate bool LREqual<T>(T l, T r);
		public static bool AreEqual<T>(IEnumerable<T> l, IEnumerable<T> r, LREqual<T> areEqual)
		{
			if (!ListsRoughlyEqual(l, r))
				return false;

			if (l == null || r == null)
				return true;

			LinkedList<T> ll = new LinkedList<T>(l);
			LinkedListNode<T> n = ll.First;
			while (n != null)
			{
				var next = n.Next;
				foreach (var v in r)
				{
					if ((areEqual != null && areEqual(n.Value, v)) ||
						(areEqual == null && object.Equals(n.Value, v)))
					{
						ll.Remove(n);
						break;
					}
				}
				n = next;
			}

			return ll.Count == 0;
		}

		public static bool AreEqual<T>(DefStat<T> l, DefStat<T> r) where T : Def
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

        public static void Populate<T>(List<T> to, List<T> from)
        {
            to.Clear();

            if (!IsNullEmpty(from))
            {
                foreach (T t in from)
                    to.Add(t);
            }
        }

        public delegate U Convert<T, U>(T t);
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

		public static void AssignDefStat<D>(D def, out DefStat<D> stat) where D : Def
		{
			if (def == null)
				stat = null;
			else
				stat = new DefStat<D>(def);
		}

		public static void AssignDef<D>(DefStat<D> from, out D def) where D : Def
		{
			if (from == null)
				def = null;
			else
				def = from.Def;
		}

		public static bool InitializeDefStat<D>(DefStat<D> s) where D : Def
		{
			if (s != null)
				if (!s.Initialize())
				{
					Log.Warning("Failed to initialize DefStat " + s.defName);
					return false;
				}
			return true;
		}

		public static bool InitializeDefStat<D>(IEnumerable<DefStat<D>> stats) where D : Def
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

		public static bool InitializeDefStat<D>(IEnumerable<IntValueDefStat<D>> stats) where D : Def
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
				return new List<T>(0);
			return new List<T>(from);
		}

		public static List<DefStat<D>> CreateDefStatList<D>(IEnumerable<D> from) where D : Def
		{
			if (from == null)
				return new List<DefStat<D>>(0);
			List<DefStat<D>> l = new List<DefStat<D>>(from.Count());
			foreach (var v in from)
				l.Add(new DefStat<D>(v));
			return l;
		}

		public static HashSet<T> CreateHashSet<T>(IEnumerable<T> from)
		{
			if (from == null)
				return new HashSet<T>();
			return new HashSet<T>(from);
		}

		public static HashSet<DefStat<D>> CreateDefStatHashSet<D>(IEnumerable<D> from) where D : Def
		{
			if (from == null)
				return new HashSet<DefStat<D>>();
			HashSet<DefStat<D>> hs = new HashSet<DefStat<D>>();
			foreach (var v in from)
				hs.Add(new DefStat<D>(v));
			return hs;
		}

		public static HashSet<D> ConvertDefStats<D>(HashSet<DefStat<D>> from) where D : Def
		{
			if (from == null)
				return null;
			HashSet<D> to = new HashSet<D>();
			foreach (var v in from)
				to.Add(v.Def);
			return to;
		}

		public static List<D> ConvertDefStats<D>(List<DefStat<D>> from) where D : Def
		{
			if (from == null)
				return null;
			List<D> to = new List<D>(from.Count);
			foreach (var v in from)
				to.Add(v.Def);
			return to;
		}

		internal static string GetDisplayLabel(string s)
		{
			if (string.IsNullOrEmpty(s))
				return "<none>";
			return s;
		}
	}
}
