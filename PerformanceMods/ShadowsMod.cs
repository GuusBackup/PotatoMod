using UnityEngine;

namespace PotatoMod.PerformanceMods;

public class ShadowsMod
{
    private static Dictionary<Light, LightShadows> originalShadowSettings = new Dictionary<Light, LightShadows>();

    public static void SceneLoaded()
    {
        originalShadowSettings.Clear();

        var lights = UnityEngine.Object.FindObjectsOfType<Light>();

        foreach (var light in lights)
        {
            if (light != null)
            {
                originalShadowSettings[light] = light.shadows;
            }
        }
    }

    public static void Update(bool enable)
    {
        var lights = UnityEngine.Object.FindObjectsOfType<Light>();

        if (enable)
        {
            foreach (var light in lights)
            {
                if (light != null && originalShadowSettings.TryGetValue(light, out LightShadows originalSetting))
                {
                    light.shadows = originalSetting;
                }
            }
        }
        else
        {
            foreach (var light in lights)
            {
                if (light != null)
                {
                    light.shadows = LightShadows.None;
                }
            }
        }
    }

}