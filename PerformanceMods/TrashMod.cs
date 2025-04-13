using HarmonyLib;
using UnityEngine;

namespace PotatoMod.PerformanceMods;

[HarmonyPatch]
public class TrashMod
{
    public static void Update(bool enable)
    {
        if (!enable)
        {
            if (NetworkSingleton<TrashManager>.Instance != null)
                NetworkSingleton<TrashManager>.Instance.DestroyAllTrash();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TrashGenerator), "GenerateTrash")]
    [HarmonyPatch(typeof(TrashManager), "CreateAndReturnTrashItem")]
    [HarmonyPatch(typeof(TrashManager), nameof(TrashManager.CreateTrashItem),
    [
        typeof(NetworkConnection),
        typeof(string),
        typeof(Vector3),
        typeof(Quaternion),
        typeof(Vector3),
        typeof(NetworkConnection),
        typeof(string),
        typeof(bool)
    ])]
    [HarmonyPatch(typeof(TrashManager), nameof(TrashManager.CreateTrashItem),
    [
        typeof(string),
        typeof(Vector3),
        typeof(Quaternion),
        typeof(Vector3),
        typeof(string),
        typeof(bool)
    ])]
    [HarmonyPatch(typeof(TrashSpawnVolume), nameof(TrashSpawnVolume.SleepStart)
    )]
    public static bool CreateTrashItem2_Patch()
    {
        return Core.Config.TrashEnabled.Value;
    }

}