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
            var harmony = HarmonyInstance.Create("com.InGameDefEditor.rimworld.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message(
                "InGameDefEditor Harmony Patches:" + Environment.NewLine +
                "  Prefix:" + Environment.NewLine +
                "    GameComponentUtility.StartedNewGame" + Environment.NewLine +
                "    GameComponentUtility.LoadedGame");
        }
    }

    [HarmonyPatch(typeof(GameComponentUtility), "StartedNewGame")]
    static class Patch_GameComponentUtility_StartedNewGame
    {
        static void Postfix()
        {
            Equipment.LoadData();
        }
    }

    [HarmonyPatch(typeof(GameComponentUtility), "LoadedGame")]
    static class Patch_GameComponentUtility_LoadedGame
    {
        static void Postfix()
        {
            Equipment.LoadData();
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