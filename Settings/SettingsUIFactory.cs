using MelonLoader;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PotatoMod.Settings;
public class SettingsUIFactory
{
    private readonly SettingsScreen _settingsScreen;

    public SettingsUIFactory(SettingsScreen settingsScreen)
    {
        _settingsScreen = settingsScreen;
    }

    public UnityEngine.UI.Button CreateButton(string settingsId, string title)
    {
        var buttonsParent = _settingsScreen.gameObject.transform.Find("Buttons")?.gameObject;
        if (buttonsParent == null) return null;

        var displayButton = buttonsParent.transform.Find("Controls")?.gameObject;
        if (displayButton == null) return null;

        var newButton = GameObject.Instantiate(displayButton, buttonsParent.transform);
        newButton.name = settingsId;

        var textComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
            textComponent.text = title;

        return newButton.GetComponent<UnityEngine.UI.Button>();
    }

    public GameObject CreatePanel(string panelName)
    {
        var contentParent = _settingsScreen.gameObject.transform.Find("Content")?.gameObject;
        if (contentParent == null) return null;

        var graphicsPanel = contentParent.transform.Find("Graphics")?.gameObject;
        if (graphicsPanel == null) return null;

        var newPanel = GameObject.Instantiate(graphicsPanel, contentParent.transform);
        newPanel.name = panelName;

        for (int i = 0, count = newPanel.transform.childCount; i < count; i++)
        {
            GameObject.Destroy(newPanel.transform.GetChild(i).gameObject);
        }

        newPanel.SetActive(false);
        return newPanel;
    }
    public Toggle CreateToggle(GameObject panel, string toggleName, string labelText, bool defaultValue, Action<bool> action)
    {
        var graphicsPanel = panel.transform.parent.Find("Graphics")?.gameObject;
        if (graphicsPanel == null) return null;

        var ssaoToggle = graphicsPanel.transform.Find("GodRays")?.gameObject;
        if (ssaoToggle == null) return null;

        var newToggle = GameObject.Instantiate(ssaoToggle, panel.transform);
        newToggle.name = toggleName;

        var label = newToggle.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
            label.text = labelText;

        var background = newToggle.transform.Find("Toggle")?.GetComponent<RectTransform>();
        var labelRect = label?.GetComponent<RectTransform>();
        var toggleRect = newToggle.GetComponent<RectTransform>();

        if (toggleRect == null || background == null || labelRect == null) return null;

        background.anchorMin = new Vector2(0, 0.5f);
        background.anchorMax = new Vector2(0, 0.5f);
        background.pivot = new Vector2(0, 0.5f);
        background.sizeDelta = new Vector2(20, 20);
        background.anchoredPosition = new Vector2(10, 0);

        labelRect.anchorMin = new Vector2(0, 0.5f);
        labelRect.anchorMax = new Vector2(0, 0.5f);
        labelRect.pivot = new Vector2(0, 0.5f);
        labelRect.sizeDelta = new Vector2(200, 30);
        labelRect.anchoredPosition = new Vector2(40, 0);
        var godRaysToggle = newToggle.GetComponentInChildren<GodRaysToggle>();
        UnityEngine.Object.Destroy(godRaysToggle);

        var toggleComponent = newToggle.GetComponentInChildren<Toggle>();
        if (toggleComponent != null)
        {
            toggleComponent.isOn = defaultValue;
            toggleComponent.onValueChanged.RemoveAllListeners();
            toggleComponent.onValueChanged.AddListener(action.ToUnityAction());
        }


        return toggleComponent;
    }

    public GameObject CreateKeyBind(GameObject panel, string keyBindName, string labelText, KeyCode defaultValue, Action<KeyCode> action)
    {
        var controlsPanel = panel.transform.parent.Find("Controls")?.gameObject;
        if (controlsPanel == null)
        {
            MelonLogger.Warning($"Failed to create keybind '{keyBindName}': Controls panel not found in parent of {panel.name}.");
            return null;
        }

        var firstRow = controlsPanel.transform.Find("Row")?.gameObject;
        if (firstRow == null)
        {
            MelonLogger.Warning($"Failed to create keybind '{keyBindName}': Row not found in Controls panel.");
            return null;
        }

        var forwardKeybind = firstRow.transform.Find("Forward")?.gameObject;
        if (forwardKeybind == null)
        {
            MelonLogger.Warning($"Failed to create keybind '{keyBindName}': Interact keybind template not found in Controls panel.");
            return null;
        }

        var newKeyBind = GameObject.Instantiate(forwardKeybind, panel.transform);
        newKeyBind.name = keyBindName;

        var label = newKeyBind.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
            label.text = labelText;
        else
            MelonLogger.Warning($"Failed to set label text for keybind '{keyBindName}': TextMeshProUGUI component not found.");

        var labelRect = label?.GetComponent<RectTransform>();
        var keyButton = newKeyBind.transform.Find("Button")?.GetComponent<RectTransform>();
        var keyBindRect = newKeyBind.GetComponent<RectTransform>();

        if (keyBindRect == null || keyButton == null || labelRect == null)
        {
            if (keyBindRect == null)
                MelonLogger.Warning($"Failed to create keybind '{keyBindName}': RectTransform not found on {newKeyBind.name}.");
            if (keyButton == null)
                MelonLogger.Warning($"Failed to create keybind '{keyBindName}': Button RectTransform not found under {newKeyBind.name}/Button.");
            if (labelRect == null)
                MelonLogger.Warning($"Failed to create keybind '{keyBindName}': RectTransform not found on label for {newKeyBind.name}.");
            return null;
        }

        labelRect.anchorMin = new Vector2(0, 0.5f);
        labelRect.anchorMax = new Vector2(0, 0.5f);
        labelRect.pivot = new Vector2(0, 0.5f);
        labelRect.sizeDelta = new Vector2(100, 30);
        labelRect.anchoredPosition = new Vector2(10, 0);

        keyButton.anchorMin = new Vector2(1, 0.5f);
        keyButton.anchorMax = new Vector2(1, 0.5f);
        keyButton.pivot = new Vector2(1, 0.5f);
        keyButton.sizeDelta = new Vector2(50, 30);
        keyButton.anchoredPosition = new Vector2(-10, 0);


        var keyText = keyButton.GetComponentInChildren<TextMeshProUGUI>();
        if (keyText != null)
            keyText.text = defaultValue.ToString();


        var KeybinderComponent = newKeyBind.GetComponent<Keybinder>();
        UnityEngine.Object.Destroy(KeybinderComponent);

        var RebindActionUIComponent = newKeyBind.GetComponent<RebindActionUI>();
        UnityEngine.Object.Destroy(RebindActionUIComponent);

        var buttonComponent = newKeyBind.GetComponentInChildren<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.onClick.RemoveAllListeners();

            buttonComponent.onClick.AddListener((UnityAction)(() =>
            {
                MelonCoroutines.Start(ListenForKeyPress(buttonComponent, keyText, action));
            }));
        }
        return newKeyBind;
    }

    private IEnumerator ListenForKeyPress(Button button, TextMeshProUGUI keyText, Action<KeyCode> onKeyChanged)
    {
        button.interactable = false;

        KeyCode newKey = KeyCode.None;
        bool keyPressed = false;

        while (!keyPressed)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    newKey = key;
                    keyPressed = true;
                    break;
                }
            }
            yield return null;
        }

        if (keyText != null)
            keyText.text = newKey.ToString();

        onKeyChanged?.Invoke(newKey);
        button.interactable = true;
    }

}

public class KeyPressHandler : MonoBehaviour
{
    private KeyCode keyCode;
    private Action onKeyPress;

    public void Initialize(KeyCode initialKeyCode, Action keyPressAction)
    {
        keyCode = initialKeyCode;
        onKeyPress = keyPressAction;
    }

    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            onKeyPress?.Invoke();
        }
    }
}

public abstract class SettingOption
{
    public string Id { get; set; }
    public string Label { get; set; }

    public abstract GameObject CreateGameObject(GameObject parent, SettingsUIFactory factory);
}

// Toggle setting option
public class ToggleSetting : SettingOption
{
    public bool DefaultValue { get; set; }
    public Action<bool> OnValueChanged { get; set; }

    public override GameObject CreateGameObject(GameObject parent, SettingsUIFactory factory)
    {
        var toggle = factory.CreateToggle(parent, Id, Label, DefaultValue, OnValueChanged);
        return toggle?.gameObject;
    }
}

public class KeyBindSetting : SettingOption
{
    public KeyCode DefaultValue { get; set; }
    public Action<KeyCode> OnValueChanged { get; set; }

    public override GameObject CreateGameObject(GameObject parent, SettingsUIFactory factory)
    {
        var toggle = factory.CreateKeyBind(parent, Id, Label, DefaultValue, OnValueChanged);
        return toggle?.gameObject;
    }
}

// Can add more setting types like sliders, dropdowns, etc.
public class SliderSetting : SettingOption
{
    public float DefaultValue { get; set; }
    public float MinValue { get; set; }
    public float MaxValue { get; set; }
    public Action<float> OnValueChanged { get; set; }

    public override GameObject CreateGameObject(GameObject parent, SettingsUIFactory factory)
    {
        // Implementation would go here when needed
        // For now, return null as a placeholder
        return null;
    }
}

// Defines a module (tab) in the settings menu
public class ModSettingsModule
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<SettingOption> Options { get; set; } = new List<SettingOption>();

    public ModSettingsModule AddToggle(string id, string label, bool defaultValue, Action<bool> onValueChanged)
    {
        Options.Add(new ToggleSetting
        {
            Id = id,
            Label = label,
            DefaultValue = defaultValue,
            OnValueChanged = onValueChanged
        });
        return this;
    }

    public ModSettingsModule AddKeyRequest(string id, string label, KeyCode defaultValue, Action<KeyCode> onValueChanged)
    {
        Options.Add(new KeyBindSetting
        {
            Id = id,
            Label = label,
            DefaultValue = defaultValue,
            OnValueChanged = onValueChanged
        });
        return this;
    }

}

public class ModSettings
{
    private static List<ModSettingsModule> _modules = new List<ModSettingsModule>();

    public static ModSettingsModule CreateModule(string id, string title)
    {
        var module = new ModSettingsModule
        {
            Id = id,
            Title = title
        };
        _modules.Add(module);
        return module;
    }

    public static List<ModSettingsModule> GetModules()
    {
        return _modules;
    }
}