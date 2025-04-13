using HarmonyLib;

namespace PotatoMod.PerformanceMods;

[HarmonyPatch(typeof(GrowLight), "Awake")]
public class GrowLightMod
{
    public static void Postfix(GrowLight __instance)
    {
        if (__instance == null)
            return;

        foreach (var item in __instance.Light.lightSources)
        {
            item.gameObject.SetActive(Core.Config.PotLightsEnabled.Value);
        }
    }

    public static void Update(bool enable)
    {
        var growLights = UnityEngine.Object.FindObjectsOfType<GrowLight>();
        foreach (var light in growLights)
        {
            foreach (var item in light?.Light?.lightSources)
            {
                item.gameObject.SetActive(enable);
            }
        }
    }
}