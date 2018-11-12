using Harmony;
using System;
using System.Reflection;
using Verse;

namespace InGameDefEditor
{
    [StaticConstructorOnStartup]
    class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = HarmonyInstance.Create("com.ingamedefeditor.rimworld.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message(
                "InGameDefEditor Harmony Patches:" + Environment.NewLine +
                "  Postfix:" + Environment.NewLine +
                "    UIRoot.UIRootOnGUI" + Environment.NewLine +
                "    GameComponentUtility.StartedNewGame" + Environment.NewLine +
                "    GameComponentUtility.LoadedGame");
        }
    }

    [HarmonyPatch(typeof(GameComponentUtility), "StartedNewGame")]
    static class Patch_GameComponentUtility_StartedNewGame
    {
        [HarmonyPriority(Priority.Last)]
        static void Postfix()
        {
            IOUtil.LoadData();
            Log.Message("InGameDefEditor".Translate() + ": Settings Applied");
        }
    }

    [HarmonyPatch(typeof(GameComponentUtility), "LoadedGame")]
    static class Patch_GameComponentUtility_LoadedGame
    {
        [HarmonyPriority(Priority.Last)]
        static void Postfix()
        {
            IOUtil.LoadData();
            Log.Message("InGameDefEditor".Translate() + ": Settings Applied");
        }
    }

    [HarmonyPatch(typeof(UIRoot), "UIRootOnGUI")]
    static class Patch_UIRoot_UIRootOnGUI
    {
        private static long LastClick = 0;
        static void Postfix()
        {
            if (InGameDefEditorKeyBindingDefOf.ShowInGameDefEditorDialog.JustPressed)
            {
                long now = DateTime.Now.Ticks;
                if (now - LastClick > TimeSpan.TicksPerSecond)
                {
                    LastClick = now;
                    if (!Find.WindowStack.TryRemove(typeof(InGameDefEditorWindow), true))
                    {
                        Find.WindowStack.Add(new InGameDefEditorWindow());
                    }
                }
            }
        }
    }
}