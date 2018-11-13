using UnityEngine;
using Verse;
using System.Text;

namespace InGameDefEditor
{
    public class Controller : Mod
    {
        public Controller(ModContentPack content) : base(content) { }

        public override string SettingsCategory()
        {
            return "InGameDefEditor".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            if (Widgets.ButtonText(new Rect (60, 60, 100, 32), "Show Editor"))
            {
                Find.WindowStack.Add(new InGameDefEditorWindow());
            }

            KeyCode a = InGameDefEditorKeyBindingDefOf.ShowInGameDefEditorDialog.defaultKeyCodeA;
            KeyCode b = InGameDefEditorKeyBindingDefOf.ShowInGameDefEditorDialog.defaultKeyCodeB;

            bool hasBoth = a != default(KeyCode) && b != default(KeyCode);

            StringBuilder sb = new StringBuilder("Use ");
            if (hasBoth)
            {
                sb.Append("{a} or {b}");
            }
            else if (a != default(KeyCode))
            {
                sb.Append("{a}");
            }
            else if (b != default(KeyCode))
            {
                sb.Append("{b}");
            }
            sb.Append(" to open the Editor's window at any time");

            sb.Replace("{a}", a.ToString());
            sb.Replace("{b}", b.ToString());

            Listing_Standard l = new Listing_Standard();
            l.Begin(new Rect(60, 100, 400, 300));
            l.Label(sb.ToString());
            l.End();
        }
    }
}