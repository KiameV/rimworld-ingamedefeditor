using HarmonyLib;
using RimWorld;
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
            var harmony = new Harmony("com.ingamedefeditor.rimworld.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message(
                "InGameDefEditor Harmony Patches:" + Environment.NewLine +
                "  Prefix:" + Environment.NewLine +
                "    Page_SelectScenario.BeginScenarioConfiguration" + Environment.NewLine +
                "    SavedGameLoaderNow.LoadGameFromSaveFileNow" + Environment.NewLine +
                "    Root_Play.SetupForQuickTestPlay" + Environment.NewLine +
                "  Postfix:" + Environment.NewLine +
                "    UIRoot.UIRootOnGUI");
        }
    }

    [HarmonyPatch(typeof(Page_SelectScenario), "BeginScenarioConfiguration")]
    static class Patch_Page_SelectScenario_BeginScenarioConfiguration
    {
        [HarmonyPriority(Priority.First)]
        static void Prefix()
        {
            IOUtil.LoadData();
        }
    }

    [HarmonyPatch(typeof(SavedGameLoaderNow), "LoadGameFromSaveFileNow")]
    static class Patch_SavedGameLoaderNow_LoadGameFromSaveFileNow
    {
        [HarmonyPriority(Priority.First)]
        static void Prefix()
        {
            IOUtil.LoadData();
        }
    }

    [HarmonyPatch(typeof(Root_Play), "SetupForQuickTestPlay")]
    static class Patch_Root_Play_SetupForQuickTestPlay
    {
        [HarmonyPriority(Priority.First)]
        static void Prefix()
        {
            IOUtil.LoadData();
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