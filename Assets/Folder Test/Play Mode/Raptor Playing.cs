using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class UT_RAPTOR_MINIGAME
{
    private GameObject root;

    [SetUp]
    public void Setup()
    {
        root = new GameObject("ROOT_TEST");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(root);
    }

    // =========================================================
    // UT-Raptor-3: Teleport Count
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Raptor_3_Teleport_Count_Increases()
    {
        var raptorGO = new GameObject("Raptor");
        raptorGO.transform.parent = root.transform;

        var raptor = raptorGO.AddComponent<RaptorPlaying>();

        var player = new GameObject("Player").transform;
        player.position = Vector3.zero;

        // inject private
        typeof(RaptorPlaying).GetField("player", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(raptor, player);

        typeof(RaptorPlaying).GetField("detectionRange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(raptor, 100f);

        var points = new System.Collections.Generic.List<Transform>();
        for (int i = 0; i < 2; i++)
        {
            var p = new GameObject("TP" + i).transform;
            p.position = new Vector3(i + 5, 0, 0);
            points.Add(p);
        }

        typeof(RaptorPlaying).GetField("teleportPoints", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(raptor, points);

        yield return null;

        raptor.SendMessage("Update");
        yield return null;

        int count = (int)typeof(RaptorPlaying).GetField("teleportCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(raptor);

        Assert.Greater(count, 0);
    }

    // =========================================================
    // UT-Raptor-4: Teleport Position
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Raptor_4_Teleport_Position_Changes()
    {
        var raptorGO = new GameObject("Raptor");
        raptorGO.transform.parent = root.transform;

        var raptor = raptorGO.AddComponent<RaptorPlaying>();

        var player = new GameObject("Player").transform;
        player.position = Vector3.zero;

        typeof(RaptorPlaying).GetField("player", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(raptor, player);

        typeof(RaptorPlaying).GetField("detectionRange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(raptor, 100f);

        var tp = new GameObject("TP").transform;
        tp.position = new Vector3(10, 0, 0);

        typeof(RaptorPlaying).GetField("teleportPoints", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(raptor, new System.Collections.Generic.List<Transform> { tp });

        yield return null;

        raptor.SendMessage("Update");
        yield return null;

        Assert.AreEqual(new Vector3(10f, 0f, 0f), raptorGO.transform.position);
    }

    // =========================================================
    // UT-Raptor-5: TV Toggle On/Off
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Raptor_5_TV_Toggle()
    {
        var tvGO = new GameObject("TV");
        tvGO.transform.parent = root.transform;

        var tv = tvGO.AddComponent<TVInteract>();

        var content = new GameObject("Screen");
        content.SetActive(false);

        var audio = tvGO.AddComponent<AudioSource>();

        typeof(TVInteract).GetField("tvContent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(tv, content);

        typeof(TVInteract).GetField("audioSource", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(tv, audio);

        typeof(TVInteract).GetField("clickClip", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(tv, AudioClip.Create("click", 441, 1, 44100, false));

        typeof(TVInteract).GetField("staticClip", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(tv, AudioClip.Create("static", 441, 1, 44100, false));

        yield return null;

        tv.Interact(null);
        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(content.activeSelf);
    }

    // =========================================================
    // UT-Raptor-6: Fishing Success
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Raptor_6_Fishing_Success()
    {
        var go = new GameObject("Fishing");
        go.transform.parent = root.transform;

        var fishing = go.AddComponent<FishingMinigame_Input>();

        var track = new GameObject().AddComponent<RectTransform>();
        track.sizeDelta = new Vector2(100, 200);

        var marker = new GameObject().AddComponent<RectTransform>();
        var zone = new GameObject().AddComponent<RectTransform>();
        zone.sizeDelta = new Vector2(100, 200);

        var text = new GameObject().AddComponent<TextMeshProUGUI>();

        fishing.trackArea = track;
        fishing.marker = marker;
        fishing.successZone = zone;
        fishing.resultText = text;
        fishing.successText = "OK";
        fishing.missText = "FAIL";

        yield return null;

        fishing.StartFishing();

        // force inside zone
        marker.anchoredPosition = zone.anchoredPosition;

        fishing.PressStop();
        yield return null;

        Assert.AreEqual("OK", text.text);
    }

    // =========================================================
    // UT-Raptor-7: Fishing Miss
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Raptor_7_Fishing_Miss()
    {
        var go = new GameObject("Fishing");
        go.transform.parent = root.transform;

        var fishing = go.AddComponent<FishingMinigame_Input>();

        var track = new GameObject().AddComponent<RectTransform>();
        track.sizeDelta = new Vector2(100, 200);

        var marker = new GameObject().AddComponent<RectTransform>();
        var zone = new GameObject().AddComponent<RectTransform>();
        zone.sizeDelta = new Vector2(100, 50);

        var text = new GameObject().AddComponent<TextMeshProUGUI>();

        fishing.trackArea = track;
        fishing.marker = marker;
        fishing.successZone = zone;
        fishing.resultText = text;
        fishing.successText = "OK";
        fishing.missText = "FAIL";

        yield return null;

        fishing.StartFishing();

        // force OUTSIDE zone
        marker.anchoredPosition = new Vector2(0, 999);

        fishing.PressStop();
        yield return null;

        Assert.AreEqual("FAIL", text.text);
    }

    // =========================================================
    // UT-Raptor-8: Retry Trigger
    // =========================================================
    [UnityTest]
    public IEnumerator UT_Raptor_8_Retry_Triggers_Event()
    {
        var go = new GameObject("Fishing");
        go.transform.parent = root.transform;

        var fishing = go.AddComponent<FishingMinigame_Input>();

        bool triggered = false;
        var eventField = typeof(FishingMinigame_Input)
            .GetField("onThreeRetries", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var unityEvent = (UnityEngine.Events.UnityEvent)eventField.GetValue(fishing);

        if (unityEvent == null)
        {
            unityEvent = new UnityEngine.Events.UnityEvent();
            eventField.SetValue(fishing, unityEvent);
        }

        unityEvent.AddListener(() => triggered = true);

        yield return null;

        fishing.Retry();
        fishing.Retry();
        fishing.Retry();

        yield return null;

        Assert.IsTrue(triggered);
    }
}