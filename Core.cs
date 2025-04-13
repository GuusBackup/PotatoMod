using MelonLoader;
using PotatoMod.PerformanceMods;
using PotatoMod.Settings;
using UnityEngine;

[assembly: MelonInfo(typeof(PotatoMod.Core), "PotatoMod", "1.0.3", "OhMyGuus", null)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace PotatoMod;

public class Core : MelonMod
{
    public static Config Config;
    public override void OnInitializeMelon()
    {
        Config = Config.Instance;
        LoggerInstance.Msg("Initialized.");
        InitializeMenu();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        ShadowsMod.SceneLoaded();
        ShadowsMod.Update(Config.ShadowsEnabled.Value);
        TrashMod.Update(Config.TrashEnabled.Value);
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(Config.StorageItemsKeybind.Value))
        {
            Config.StorageItemsEnabled.Value = !Config.StorageItemsEnabled.Value;
            StorageRackMod.Update(Config.StorageItemsEnabled.Value);
        }

        if (Input.GetKeyDown(Config.ShadowsKeybind.Value))
        {
            Config.ShadowsEnabled.Value = !Config.ShadowsEnabled.Value;
            ShadowsMod.Update(Config.ShadowsEnabled.Value);
        }
    }

    public void InitializeMenu()
    {
        ModSettings.CreateModule("PotatoModSettings", "Potato Settings")
       .AddToggle("DisablePotLights", "Pot lights", Config.PotLightsEnabled.Value, (value) =>
       {
           Config.PotLightsEnabled.Value = value;
       })
       .AddToggle("ViewStorage", "Storage Items", Config.StorageItemsEnabled.Value, (value) =>
       {
           Config.StorageItemsEnabled.Value = value;
       })
       .AddToggle("Shadow", "Shadows", Config.ShadowsEnabled.Value, (value) =>
        {
            Config.ShadowsEnabled.Value = value;
        })
       .AddToggle("Trash", "Trash", Config.TrashEnabled.Value, (value) =>
       {
           Config.TrashEnabled.Value = value;
       })
       .AddKeyRequest("StorageKeybind", "storage Items", Config.StorageItemsKeybind.Value, (value) =>
        {
            Config.StorageItemsKeybind.Value = value;
        })
       .AddKeyRequest("ShadowKeybind", "Shadows", Config.ShadowsKeybind.Value, (value) =>
        {
            Config.ShadowsKeybind.Value = value;
        });
    }

}


//[HarmonyPatch]
//public static class UnityDebugPatches
//{

//    [HarmonyPatch(typeof(Debug), nameof(Debug.Log), new[] { typeof(Il2CppSystem.Object) })]
//    [HarmonyPatch(typeof(Debug), nameof(Debug.Log), new[] { typeof(Il2CppSystem.Object), typeof(UnityEngine.Object) })]
//    [HarmonyPatch(typeof(DebugLogHandler), nameof(DebugLogHandler.Internal_Log))]
//    [HarmonyPatch(typeof(Logger), nameof(Logger.Log), new[] { typeof(string), typeof(Il2CppSystem.Object) })]
//    [HarmonyPatch(typeof(Logger), nameof(Logger.Log), new[] { typeof(LogType), typeof(string), typeof(Il2CppSystem.Object) })]
//    [HarmonyPatch(typeof(Logger), nameof(Logger.Log), new[] { typeof(LogType), typeof(Il2CppSystem.Object), typeof(UnityEngine.Object) })]
//    [HarmonyPatch(typeof(Logger), nameof(Logger.Log), new[] { typeof(LogType), typeof(Il2CppSystem.Object) })]


//    [HarmonyPrefix]
//    public static bool Prefix()
//    {
//        return false;
//    }
//}