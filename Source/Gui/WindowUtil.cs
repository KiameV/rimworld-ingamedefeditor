using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace InGameDefEditor
{
    public static class WindowUtil
    {
        public delegate void OnChange<T>(T t);
        public static string DrawInput(float x, ref float y, string label, float value, OnChange<float> onChange, string valueBuffer)
        {
            string s = DrawLabeledInput(x, y, label, valueBuffer);
            if (float.TryParse(s, out float f))
                onChange(f);
            y += 40;
            return s;
        }

        public static string DrawInput(float x, ref float y, string label, int value, OnChange<int> onChange, string valueBuffer)
        {
            string s = DrawLabeledInput(x, y, label, valueBuffer);
            if (Double.TryParse(s, out double d))
                onChange((int)d);
            y += 40;
            return s;
        }

        public static bool DrawInput(float x, ref float y, string label, bool value, OnChange<bool> onChange)
        {
            DrawLabel(x, y, 240, label);
            
            if (Widgets.ButtonText(new Rect(x + 250, y, 60, 32), value.ToString()))
            {
                value = !value;
                onChange(value);
            }
            y += 40;
            return value;
        }

        public static void DrawInput<T>(float x, ref float y, float width, string label, int labelWidth, string buttonText, DrawFloatOptionsArgs<T> floatingOptionArgs, bool isBolded = false)
        {
            DrawLabel(x, y, labelWidth, label, isBolded);
            x = x + labelWidth + 10;
            if (Widgets.ButtonText(new Rect(x, y, width - x - 10, 30), buttonText))
                DrawFloatingOptions(floatingOptionArgs);
            y += 40;
        }

        public static String DrawLabeledInput(float x, float y, string label, string buffer)
        {
            DrawLabel(x, y, 240, label);
            return Widgets.TextField(new Rect(x + 250, y, 60, 32), buffer);
        }

        public static void DrawLabel(float x, float y, float width, string label, bool bolded = false)
        {
            // 0.15 is about the size of each character
            if (label.Length > width * 0.15)
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

        public delegate string GetDisplayName<T>(T t);
        public delegate void OnSelect<T>(T t);
        public delegate IEnumerable<T> UpdateItems<T>();
        public class DrawFloatOptionsArgs<T>
        {
            public IEnumerable<T> items = null;
            public UpdateItems<T> updateItems = null;
            public GetDisplayName<T> getDisplayName = null;
            public OnSelect<T> onSelect = null;
            public bool includeNullOption = false;
        }
        public static void DrawFloatingOptions<T>(DrawFloatOptionsArgs<T> args)
        {
            if (args.items == null || args.items.Count() == 0)
                return;

            List<FloatMenuOption> options = new List<FloatMenuOption>();
            if (args.includeNullOption)
            {
                options.Add(new FloatMenuOption(
                    "<none>", delegate { args.onSelect(default(T)); },
                    MenuOptionPriority.High, null, null, 0f, null, null));
            }
            foreach (T t in args.items)
            {
                options.Add(new FloatMenuOption(
                    args.getDisplayName(t), delegate { args.onSelect(t); }, 
                    MenuOptionPriority.Default, null, null, 0f, null, null));
            }
            Find.WindowStack.Add(new FloatMenu(options));
        }
        
        public static void PlusMinusLabel(
            float x, ref float y, int labelWidth, string label, Action add, Action subtract)
        {
            DrawLabel(x, y, labelWidth, label, true);
            if (Widgets.ButtonText(new Rect(x + labelWidth + 10, y - 4, 30, 32), "+"))
            {
                add();
            }
            if (Widgets.ButtonText(new Rect(x + labelWidth + 52, y - 4, 30, 32), "-"))
            {
                subtract();
            }
            y += 40;
        }

        public static void PlusMinusLabel<T, U>(
            float x, ref float y, int labelWidth, string label,
            DrawFloatOptionsArgs<T> addFloatOptions, 
            DrawFloatOptionsArgs<U> removeFloatOptions)
        {
            PlusMinusLabel(x, ref y, labelWidth, label,
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
    }
}
