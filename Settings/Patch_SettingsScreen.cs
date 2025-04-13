
using HarmonyLib;
namespace PotatoMod.Settings;

[HarmonyPatch(typeof(SettingsScreen), "Start")]
public class Patch_SettingsScreen
{
    public static void Prefix(SettingsScreen __instance)
    {
        if (__instance == null) return;

        var categories = __instance.Categories.ToList();
        var factory = new SettingsUIFactory(__instance);

        foreach (var module in ModSettings.GetModules())
        {
            var btn = factory.CreateButton(module.Id, module.Title);
            if (btn == null) continue;

            var panel = factory.CreatePanel(module.Id + "Panel");
            if (panel == null) continue;

            foreach (var option in module.Options)
            {
                option.CreateGameObject(panel, factory);
            }

            var category = new SettingsScreen.SettingsCategory
            {
                Button = btn,
                Panel = panel
            };

            categories.Add(category);
        }

        __instance.Categories = categories.ToArray();
    }
}