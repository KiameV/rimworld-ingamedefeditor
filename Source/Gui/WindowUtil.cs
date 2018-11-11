using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace InGameDefEditor
{
    static class WindowUtil
    {
        public static string DrawInput(float x, ref float y, string label, ref float value, string valueBuffer)
        {
            string s = DrawLabeledInput(x, y, label, valueBuffer);
            if (float.TryParse(s, out float f))
                value = f;
            y += 40;
            return s;
        }

        public static string DrawInput(float x, ref float y, string label, ref int value, string valueBuffer)
        {
            string s = DrawLabeledInput(x, y, label, valueBuffer);
            if (Double.TryParse(s, out double d))
                value = (int)d;
            y += 40;
            return s;
        }
        
        public static void DrawInput(float x, ref float y, float width, string label, int labelWidth, string buttonText, Action onClick, bool isBolded = false)
        {
            DrawLabel(x, y, labelWidth, label, isBolded);
            x = x + labelWidth + 10;
            if (Widgets.ButtonText(new Rect(x, y, width - x - 10, 30), buttonText))
                onClick();
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

        public static void DrawFloatingOptions<T>(
            IEnumerable<T> items, GetDisplayName<T> getDisplayName, OnSelect<T> onSelect, bool includeNullOption = false)
        {
            if (items == null || items.Count() == 0)
                return;

            List<FloatMenuOption> options = new List<FloatMenuOption>();
            if (includeNullOption)
            {
                options.Add(new FloatMenuOption(
                    "<none>", delegate { onSelect(default(T)); },
                    MenuOptionPriority.High, null, null, 0f, null, null));
            }
            foreach (T t in items)
            {
                options.Add(new FloatMenuOption(
                    getDisplayName(t), delegate { onSelect(t); }, 
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
    }
}
