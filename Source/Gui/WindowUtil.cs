﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace InGameDefEditor
{
    public static class WindowUtil
    {
        public delegate void OnChange<T>(T t);
        public static string DrawInput(float x, ref float y, float width, string label, float value, OnChange<float> onChange, string valueBuffer)
        {
            string s = DrawLabeledInput(x, y, width, label, valueBuffer);
            if (float.TryParse(s, out float f))
				onChange(f);
            y += 40;
            return s;
        }

        public static string DrawInput(float x, ref float y, float width, string label, int value, OnChange<int> onChange, string valueBuffer)
        {
            string s = DrawLabeledInput(x, y, width, label, valueBuffer);
            if (Double.TryParse(s, out double d))
				onChange((int)d);
            y += 40;
            return s;
        }

        public static bool DrawInput(float x, ref float y, float width, string label, bool value, OnChange<bool> onChange, float buttonWidth = 60f)
        {
            DrawLabel(x, y, 240, label);
            
            if (Widgets.ButtonText(new Rect(width - buttonWidth, y, buttonWidth, 32), value.ToString()))
            {
                value = !value;
                onChange(value);
            }
            y += 40;
            return value;
        }

        public static void DrawInput<T>(float x, ref float y, float width, string label, float labelWidth, string buttonText, FloatOptionsArgs<T> floatingOptionArgs, bool isBolded = false)
        {
            DrawLabel(x, y, labelWidth, label, isBolded);
            x = x + labelWidth + 10;
            if (Widgets.ButtonText(new Rect(x, y, width - x - 10, 30), buttonText))
                DrawFloatingOptions(floatingOptionArgs);
            y += 40;
        }

        public static String DrawLabeledInput(float x, float y, float width, string label, string buffer)
        {
            DrawLabel(x, y, 240, label);
            return Widgets.TextField(new Rect(width - 60, y, 60, 32), buffer);
        }

		public static void DrawLabel(float x, ref float y, float width, string label, float yInc = 32, bool bolded = false)
		{
			DrawLabel(x, y, width, label, bolded);
			y += yInc;
		}

		public static void DrawLabel(float x, float y, float width, string label, bool bolded = false)
        {
			if (label == null)
				return;

            // 0.14 is about the size of each character
            if (label.Length > width * 0.137)
            {
                Text.Font = GameFont.Tiny;
            }

            Rect r = new Rect(x, y + 2, width, 30);
            if (bolded)
                GUI.Label(r, label, new GUIStyle(Text.CurFontStyle) { fontStyle = FontStyle.Bold });
            else
                Widgets.Label(r, label);

            Text.Font = GameFont.Small;
        }

		public static void DrawList<D>(float x, ref float y, float width, string label, IEnumerable<D> items, ref Vector2 scroll, ref float previousY, bool bolded = false) where D : Def, new()
		{
			DrawLabel(x, ref y, width, label, 30, bolded);
			DrawList(x, ref y, width, items, ref scroll, ref previousY, bolded);
		}

		public static void DrawList<D>(float x, ref float y, float width, IEnumerable<D> items, ref Vector2 scroll, ref float previousY, bool bolded = false) where D : Def, new()
		{
			if (previousY > 300)
			{
				Widgets.BeginScrollView(
					new Rect(x + 20, y, width - 16, 300),
					ref scroll,
					new Rect(0, 0, width - 32, previousY));
				previousY = 0;

				foreach (var v in items)
					DrawLabel(10, ref previousY, width - 10, "- " + Util.GetLabel(v));

				Widgets.EndScrollView();
				y += 332;
			}
			else
			{
				previousY = 0;
				foreach (var v in items)
				{
					float orig = y;
					DrawLabel(10, ref y, width - 10, "- " + Util.GetLabel(v));
					previousY += y - orig;
				}
			}
		}

		public static void DrawList<D>(float x, ref float y, float width, string label, IEnumerable<D> items, bool bolded = false) where D : Def, new()
		{
			DrawLabel(x, ref y, width, label, 30, bolded);
			DrawList(x, ref y, width, items, bolded);
		}

		public static void DrawList<D>(float x, ref float y, float width, IEnumerable<D> items, bool bolded = false) where D : Def, new()
		{
			foreach (var v in items)
				DrawLabel(10, ref y, width - 10, "- " + Util.GetLabel(v));
		}

		public delegate string GetDisplayName<T>(T t);
        public delegate void OnSelect<T>(T t);
        public delegate IEnumerable<T> UpdateItems<T>();
		public delegate void OnCustomOption();
		public class FloatOptionsArgs<T>
        {
            public IEnumerable<T> items = null;
            public UpdateItems<T> updateItems = null;
            public GetDisplayName<T> getDisplayName = null;
            public OnSelect<T> onSelect = null;
			public OnCustomOption onCustomOption = null;
			public bool includeNullOption = false;
            public bool skipListCustomOnly = false;
        }
        public static void DrawFloatingOptions<T>(FloatOptionsArgs<T> args)
        {
            if (args.skipListCustomOnly && args.onCustomOption != null)
            {
                Log.Warning("Invoke onCustomOptions");
                args.onCustomOption?.Invoke();
                return;
            }

            if (args == null || args.items == null || args.items.Count() == 0)
                return;

            List<FloatMenuOption> options = new List<FloatMenuOption>();
            if (args.includeNullOption)
            {
                options.Add(new FloatMenuOption(
                    "None", delegate { args.onSelect(default); },
                    MenuOptionPriority.High, null, null, 0f, null, null));
            }
			if (args.onCustomOption != null)
			{
				options.Add(new FloatMenuOption(
					"Custom...", () => args.onCustomOption(),
					MenuOptionPriority.High, null, null, 0f, null, null));
			}
            foreach (T t in args.items)
            {
                options.Add(new FloatMenuOption(
                    args.getDisplayName(t), delegate { args.onSelect(t); }, 
                    MenuOptionPriority.Default, null, null, 0f, null, null));
            }
			if (options == null || options.Count > 0)
				Find.WindowStack.Add(new FloatMenu(options));
        }

		internal static void DrawFlagList<E>(float x, ref float y, float width, List<E> enums, int flag, Predicate<E> exclude = null) where E : struct, IConvertible
		{
			foreach (var e in enums)
			{
				if (exclude != null && exclude(e))
					continue;

				var v = (int)(object)e;
				if ((flag & v) == v)

					WindowUtil.DrawLabel(x + 20, ref y, width, "- " + e.ToString(), 30);
			}
		}

		public static void PlusMinusLabel(
            float x, ref float y, float width, string label, Action add, Action subtract)
        {
            DrawLabel(x, y, width - 80, label, true);
            if (Widgets.ButtonText(new Rect(width - 74, y - 4, 30, 32), "+"))
            {
                add();
            }
            if (Widgets.ButtonText(new Rect(width - 32, y - 4, 30, 32), "-"))
            {
                subtract();
            }
            y += 32;
        }

        public static void PlusMinusLabel<T, U>(
            float x, ref float y, float width, string label,
            FloatOptionsArgs<T> addFloatOptions, 
            FloatOptionsArgs<U> removeFloatOptions)
        {
            PlusMinusLabel(x, ref y, width, label,
                delegate ()
                {
                    if (addFloatOptions.updateItems != null)
                    {
                        if (addFloatOptions.items == null)
                            addFloatOptions.items = new List<T>();
                        addFloatOptions.items = addFloatOptions.updateItems.Invoke();
                    }
					DrawFloatingOptions(addFloatOptions);
                },
                delegate ()
                {
                    if (removeFloatOptions.updateItems != null)
                    {
                        if (removeFloatOptions.items == null)
                            removeFloatOptions.items = new List<U>();
                        removeFloatOptions.items = removeFloatOptions.updateItems.Invoke();
                    }
					DrawFloatingOptions(removeFloatOptions);
                });
        }

		public delegate IEnumerable<T> BeingUsed<T>();
		public delegate bool IsBeingUsed<T>(T t);
		public class PlusMinusArgs<T>
        {
            public IEnumerable<T> allItems;
            public BeingUsed<T> beingUsed;
			public IsBeingUsed<T> isBeingUsed;
            public OnSelect<T> onAdd;
            public OnSelect<T> onRemove;
            public GetDisplayName<T> getDisplayName = null;
			public OnCustomOption onCustomOption = null;

            internal FloatOptionsArgs<T> addArgs = null;
            internal FloatOptionsArgs<T> removeArgs = null;
        }
        public static void PlusMinusLabel<T>(float x, ref float y, float width, string label, PlusMinusArgs<T> args)
        {
            if (args.addArgs == null)
            {
                args.addArgs = new FloatOptionsArgs<T>()
                {
                    getDisplayName = args.getDisplayName,
					onCustomOption = args.onCustomOption,
                    updateItems = delegate()
					{
						List<T> items = new List<T>();
						if (args.beingUsed != null)
						{
							HashSet<T> lookup = new HashSet<T>();
							foreach (var v in args.beingUsed())
								lookup.Add(v);
							foreach (T d in args.allItems)
								if (!lookup.Contains(d))
									items.Add(d);
						}
						else if (args.isBeingUsed != null)
						{
							foreach (T d in args.allItems)
								if (!args.isBeingUsed(d))
									items.Add(d);
						}
                        return items;
                    },
                    onSelect = args.onAdd
                };
                args.removeArgs = new FloatOptionsArgs<T>()
                {
                    getDisplayName = args.getDisplayName,
                    updateItems = delegate ()
					{
						if (args.beingUsed != null)
						{
							var v = args.beingUsed();
							if (v == null)
								v = new List<T>(0);
							return v;
						}

						List<T> l = new List<T>();
						foreach (var v in args.allItems)
							if (args.isBeingUsed(v))
								l.Add(v);
						return l;
					},
                    onSelect = args.onRemove
                };
            }
            PlusMinusLabel(x, ref y, width, label, args.addArgs, args.removeArgs);
        }
    }
}
