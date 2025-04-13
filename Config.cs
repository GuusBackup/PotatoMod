using MelonLoader;
using PotatoMod.PerformanceMods;
using UnityEngine;

namespace PotatoMod;

public sealed class Config
{
    private static readonly Lazy<Config> _instance = new Lazy<Config>(() => new Config());
    public static Config Instance => _instance.Value;

    private MelonPreferences_Category _category;
    public MelonPreferences_Entry<bool> PotLightsEnabled { get; private set; }
    public MelonPreferences_Entry<bool> StorageItemsEnabled { get; private set; }
    public MelonPreferences_Entry<bool> ShadowsEnabled { get; private set; }
    public MelonPreferences_Entry<KeyCode> StorageItemsKeybind { get; private set; }
    public MelonPreferences_Entry<KeyCode> ShadowsKeybind { get; private set; }
    public MelonPreferences_Entry<bool> TrashEnabled { get; private set; }

    private Config()
    {
        _category = MelonPreferences.CreateCategory("PotatoMod");

        PotLightsEnabled = _category.CreateEntry("PotLightsEnabled", true);
        StorageItemsEnabled = _category.CreateEntry("StorageItemsEnabled", true);
        ShadowsEnabled = _category.CreateEntry("ShadowsEnabled", true);
        StorageItemsKeybind = _category.CreateEntry("StorageItemsKeybind", KeyCode.None);
        ShadowsKeybind = _category.CreateEntry("ShadowsKeybind", KeyCode.None);
        TrashEnabled = _category.CreateEntry("TrashEnabled", true);
        InitializeChanged();
    }

    private void InitializeChanged()
    {
        PotLightsEnabled.OnEntryValueChanged.Subscribe((old, newValue) => GrowLightMod.Update(newValue));
        StorageItemsEnabled.OnEntryValueChanged.Subscribe((old, newValue) => StorageRackMod.Update(newValue));
        ShadowsEnabled.OnEntryValueChanged.Subscribe((old, newValue) => ShadowsMod.Update(newValue));
        TrashEnabled.OnEntryValueChanged.Subscribe((old, newValue) => TrashMod.Update(newValue));
    }
    public void Save()
    {
        MelonPreferences.Save();
    }
}
