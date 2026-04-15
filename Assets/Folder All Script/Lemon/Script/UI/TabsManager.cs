using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManager : MonoBehaviour
{
    [Header("Tabs")]
    public GameObject[] Tabs;
    public Image[] TabButtons;
    public Image[] Icons;

    [Header("Active / Inactive Style")]
    public Color ActiveColor;
    public Color InactiveColor;
    public Color ActiveIcon;
    public Color InactiveIcon;
    public Vector2 ActiveSize = new Vector2(200, 80);
    public Vector2 InactiveSize = new Vector2(160, 60);

    [Header("Animation")]
    public float duration = 0.2f;
    void Start()
    {
        SwitchToTab(0);
    }
    public void SwitchToTab(int tabID)
    {
        if (tabID < 0 || tabID >= Tabs.Length || tabID >= TabButtons.Length)
            return;

        // ===== Switch Content =====
        for (int i = 0; i < Tabs.Length; i++)
            Tabs[i].SetActive(i == tabID);

        // ===== Animate Buttons =====
        for (int i = 0; i < TabButtons.Length; i++)
        {
            bool isActive = (i == tabID);

            Image img = TabButtons[i];
            Image icon= Icons[i];
            SmoothColor(icon,isActive ? ActiveIcon : InactiveIcon,
                duration);
                
            SmoothColor(img,
                isActive ? ActiveColor : InactiveColor,
                duration);

            SmoothSize(img.rectTransform,
                isActive ? ActiveSize : InactiveSize,
                duration);
        }
    }

    // ================================
    // Smooth Color
    // ================================
    void SmoothColor(Image img, Color target, float duration)
    {
        StartCoroutine(FadeColor(img, target, duration));
    }

    IEnumerator FadeColor(Image img, Color target, float duration)
    {
        Color start = img.color;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            img.color = Color.Lerp(start, target, time / duration);
            yield return null;
        }

        img.color = target;
    }

    // ================================
    // Smooth Size
    // ================================
    void SmoothSize(RectTransform rect, Vector2 target, float duration)
    {
        StartCoroutine(Resize(rect, target, duration));
    }

    IEnumerator Resize(RectTransform rect, Vector2 target, float duration)
    {
        Vector2 start = rect.sizeDelta;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            rect.sizeDelta = Vector2.Lerp(start, target, time / duration);
            yield return null;
        }

        rect.sizeDelta = target;
    }
}
