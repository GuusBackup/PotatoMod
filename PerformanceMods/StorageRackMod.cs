using HarmonyLib;
namespace PotatoMod.PerformanceMods;

[HarmonyPatch(typeof(StorageEntity), "Awake")]
public class StorageRackMod
{
    public static void Postfix(StorageEntity __instance)
    {
        if (__instance == null)
            return;

        var obj = __instance.gameObject.transform.Find("StoredItemContainer");
        if (obj == null) return;
        obj.gameObject.SetActive(Core.Config.StorageItemsEnabled.Value);
    }

    public static void Update(bool enable)
    {
        var storageEntities = UnityEngine.Object.FindObjectsOfType<StorageEntity>();
        foreach (var storageEntity in storageEntities)
        {
            var obj = storageEntity.gameObject.transform.Find("StoredItemContainer");
            if (obj == null) continue;

            obj.gameObject.SetActive(enable);
        }
    }
}