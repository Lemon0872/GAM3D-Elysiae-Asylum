using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UT_UI
{
    private GameObject root;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        root = new GameObject("TestRoot");

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(root);
        foreach (var obj in Object.FindObjectsOfType<GameObject>())
        {
            Object.Destroy(obj);
        }

        yield return null;
    }

    // =========================================
    // UT-UI-2: Menu Button UI
    // =========================================
    [UnityTest]
    public IEnumerator UT_UI_02_Menu_Button_Works()
    {
        var obj = new GameObject();
        var ui = obj.AddComponent<UIManager>();

        ui.mainCanvas = new GameObject();
        ui.settingCanvas = new GameObject();

        var titleObj = new GameObject();
        ui.title = titleObj.AddComponent<TextMeshProUGUI>();

        ui.OpenSetting();
        yield return null;

        Assert.IsFalse(ui.mainCanvas.activeSelf);
        Assert.IsTrue(ui.settingCanvas.activeSelf);

        ui.BackToMenu();
        yield return null;

        Assert.IsTrue(ui.mainCanvas.activeSelf);
        Assert.IsFalse(ui.settingCanvas.activeSelf);
    }

    // =========================================
    // UT-UI-4: Pause Menu (Tabs)
    // =========================================
    [UnityTest]
    public IEnumerator UT_UI_04_Tabs_Switching()
    {
        var obj = new GameObject();
        var tabs = obj.AddComponent<TabsManager>();

        tabs.Tabs = new GameObject[2];
        tabs.TabButtons = new Image[2];
        tabs.Icons = new Image[2];

        for (int i = 0; i < 2; i++)
        {
            tabs.Tabs[i] = new GameObject();
            tabs.TabButtons[i] = new GameObject().AddComponent<Image>();
            tabs.Icons[i] = new GameObject().AddComponent<Image>();
        }

        tabs.SwitchToTab(1);
        yield return null;

        Assert.IsFalse(tabs.Tabs[1].activeSelf);
    }

    // =========================================
    // UT-UI-7: Graphics Tab (Fullscreen)
    // =========================================
    [UnityTest]
    public IEnumerator UT_UI_07_Fullscreen_Toggle()
    {
        var obj = new GameObject();
        var manager = obj.AddComponent<ScreenSettingsManager>();

        var slider = new GameObject().AddComponent<Slider>();
        var dropdown = new GameObject().AddComponent<TMP_Dropdown>();
        var toggle = new GameObject().AddComponent<Toggle>();
        var overlay = new GameObject().AddComponent<Image>();

        // inject private fields
        typeof(ScreenSettingsManager).GetField("brightnessSlider",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(manager, slider);

        typeof(ScreenSettingsManager).GetField("qualityDropdown",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(manager, dropdown);

        typeof(ScreenSettingsManager).GetField("fullscreenToggle",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(manager, toggle);

        typeof(ScreenSettingsManager).GetField("brightnessOverlay",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(manager, overlay);

        manager.SetFullscreen(true);
        yield return null;

        Assert.IsFalse(Screen.fullScreen);

        manager.SetFullscreen(false);
        yield return null;

        Assert.IsFalse(Screen.fullScreen);
    }

    // =========================================
    // UT-UI-11: Game Over UI (HorrorUI)
    // =========================================
    [UnityTest]
    public IEnumerator UT_UI_11_HorrorUI_Update()
    {
        var obj = new GameObject();
        var ui = obj.AddComponent<HorrorUI>();

        ui.hpFill = new GameObject().AddComponent<Image>();
        ui.batteryFill = new GameObject().AddComponent<Image>();
        ui.statusText = new GameObject().AddComponent<TextMeshProUGUI>();

        ui.hp = 0.5f;
        ui.battery = 0.3f;

        yield return null;

        Assert.AreEqual(0.5f, ui.hpFill.fillAmount);
        Assert.AreEqual(0.3f, ui.batteryFill.fillAmount);

        ui.SetHidden(false);
        Assert.AreEqual("DETECTED", ui.statusText.text);

        ui.SetHidden(true);
        Assert.AreEqual("HIDDEN", ui.statusText.text);
    }

    // =========================================
    // UT-UI-12: Whale Minigame UI
    // =========================================
    [UnityTest]
    public IEnumerator UT_UI_12_Whale_Interact_Changes_Color()
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var whale = obj.AddComponent<WhaleChangeColor>();

        var mat = new Material(Shader.Find("Standard"));

        typeof(WhaleChangeColor).GetField("newMaterial",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(whale, mat);

        typeof(WhaleChangeColor).GetField("whaleClean",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(whale, true);

        whale.Interact(null);

        yield return null;

        Assert.IsNotNull(obj.GetComponent<Renderer>().material);
    }

    // =========================================
    // EXTRA: PlayerInteractUI
    // =========================================
    [UnityTest]
    public IEnumerator UT_UI_PlayerInteract_ShowHide()
    {
        var obj = new GameObject();
        var ui = obj.AddComponent<PlayerInteractUI>();

        var container = new GameObject();
        var text = new GameObject().AddComponent<TextMeshProUGUI>();

        typeof(PlayerInteractUI).GetField("containerGameObject",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(ui, container);

        typeof(PlayerInteractUI).GetField("interactTextMeshProUGUI",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(ui, text);

        // fake interactable
        var fake = new GameObject().AddComponent<FakeInteractable>();

        ui.SendMessage("Show", fake);
        yield return null;

        Assert.IsTrue(container.activeSelf);

        ui.SendMessage("Hide");
        yield return null;

        Assert.IsFalse(container.activeSelf);
    }

    class FakeInteractable : MonoBehaviour, IInteractable
    {
        public string GetInteractText() => "Test";
        public Transform GetTransform() => transform;
        public void Interact(Transform t) { }
    }
}