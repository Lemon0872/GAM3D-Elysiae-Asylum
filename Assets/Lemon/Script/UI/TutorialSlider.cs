using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialSlider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform pagesRoot;
    [SerializeField] private RawImage videoDisplay;
    [SerializeField] private CanvasGroup tutorialCanvasGroup;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private KeyCode nextKey = KeyCode.E;
    [SerializeField] private KeyCode prevKey = KeyCode.Q;

    [Header("Navigation Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button finishButton;

    [Header("Pagination")]
    [SerializeField] private Transform paginationRoot;
    [SerializeField] private GameObject dotPrefab;

    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color inactiveColor = new Color(1,1,1,0.3f);

    [SerializeField] private float activeScale = 1.3f;

    private List<Image> dots = new List<Image>();
    private List<CanvasGroup> pages = new();
    private List<VideoPlayer> videoPages = new();

    private RenderTexture sharedRT;
    private int currentIndex = 0;
    private bool isTransitioning = false;

    void Awake()
    {
        InitializePages();
        InitializePagination();
        CreateRenderTexture();
        ShowPageInstant(0);
        UpdateNavigationButtons();
    }
    void Start()
    {
        nextButton.onClick.AddListener(NextPage);
        previousButton.onClick.AddListener(PreviousPage);
    }
    void Update()
    {
        if (isTransitioning) return;

        if (Input.GetKeyDown(nextKey))
            Next();

        if (Input.GetKeyDown(prevKey))
            Previous();
    }

    // ===============================
    // INITIALIZATION
    // ===============================
    void InitializePages()
    {
        pages.Clear();
        videoPages.Clear();

        for (int i = 0; i < pagesRoot.childCount; i++)
        {
            Transform page = pagesRoot.GetChild(i);

            CanvasGroup cg = page.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = page.gameObject.AddComponent<CanvasGroup>();

            // Quan trọng: KHÔNG SetActive(false)
            page.gameObject.SetActive(true);

            cg.alpha = 0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;

            pages.Add(cg);

            VideoPlayer vp = page.GetComponentInChildren<VideoPlayer>(true);
            videoPages.Add(vp);
        }
    }
    void InitializePagination()
    {
        dots.Clear();
        finishButton.gameObject.SetActive(false);

        for (int i = 0; i < pages.Count; i++)
        {
            GameObject dot = Instantiate(dotPrefab, paginationRoot);

            Image img = dot.GetComponent<Image>();
            dots.Add(img);
        }

        UpdatePagination(0);
    }

    void CreateRenderTexture()
    {
        sharedRT = new RenderTexture(Screen.width, Screen.height, 0);
        videoDisplay.texture = sharedRT;
        videoDisplay.gameObject.SetActive(false);
    }

    // ===============================
    // PAGE CONTROL
    // ===============================
    public void Next()
    {
        if (currentIndex < pages.Count - 1)
            FadeTo(currentIndex + 1);
    }

    public void Previous()
    {
        if (currentIndex > 0)
            FadeTo(currentIndex - 1);
    }
    public void NextPage()
    {
        if (isTransitioning) return;

        int next = currentIndex + 1;

        if (next >= pages.Count)
            return; // hoặc loop về 0 nếu muốn

        FadeTo(next);
    }

    public void PreviousPage()
    {
        if (isTransitioning) return;

        int prev = currentIndex - 1;

        if (prev < 0)
            return;

        FadeTo(prev);
    }

    public void FadeTo(int index)
    {
        if (index < 0 || index >= pages.Count)
            return;

        if (index == currentIndex)
            return;

        isTransitioning = true;

        CanvasGroup oldPage = pages[currentIndex];
        CanvasGroup newPage = pages[index];

        newPage.alpha = 0f;
        newPage.interactable = false;
        newPage.blocksRaycasts = false;

        newPage.gameObject.SetActive(true);
        HandleVideo(index);
        // Fade out page cũ
        LeanTween.alphaCanvas(oldPage, 0f, fadeDuration)
                .setEase(LeanTweenType.easeInOutQuad);

        oldPage.interactable = false;
        oldPage.blocksRaycasts = false;

        // Fade in page mới
        LeanTween.alphaCanvas(newPage, 1f, fadeDuration)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() =>
                {
                    currentIndex = index;

                    newPage.interactable = true;
                    newPage.blocksRaycasts = true;
                    currentIndex = index;

                    bool isLastPage = currentIndex == pages.Count - 1;
                    finishButton.gameObject.SetActive(isLastPage);

                    UpdatePagination(index);
                    UpdateNavigationButtons();
                    isTransitioning = false;
                });
    }
    void UpdatePagination(int activeIndex)
    {
        for (int i = 0; i < dots.Count; i++)
        {
            bool isActive = i == activeIndex;

            dots[i].color = isActive ? activeColor : inactiveColor;

            LeanTween.scale(dots[i].rectTransform,
                            isActive ? Vector3.one * activeScale : Vector3.one,
                            0.2f)
                    .setEase(LeanTweenType.easeOutBack);
        }
    }
    void UpdateNavigationButtons()
    {
        previousButton.gameObject.SetActive(currentIndex > 0);
        nextButton.gameObject.SetActive(currentIndex < pages.Count - 1);
    }

    void ShowPageInstant(int index)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].alpha = (i == index) ? 1 : 0;
            pages[i].interactable = (i == index);
            pages[i].blocksRaycasts = (i == index);
        }

        currentIndex = index;
        HandleVideo(index);
    }

    // ===============================
    // VIDEO
    // ===============================
    void HandleVideo(int index)
    {
        foreach (var vp in videoPages)
        {
            if (vp != null)
                vp.Stop();
        }

        VideoPlayer currentVP = videoPages[index];

        if (currentVP != null)
        {
            videoDisplay.gameObject.SetActive(true);

            videoDisplay.transform.SetParent(
                pages[index].transform,
                false
            );

            RectTransform rect = videoDisplay.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            currentVP.targetTexture = sharedRT;
            currentVP.time = 0;
            currentVP.Play();
        }
        else
        {
            LeanTween.alpha(videoDisplay.gameObject,0,fadeDuration)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                videoDisplay.gameObject.SetActive(false);
            });
        }
    }

    // ===============================
    // FADE WHOLE TUTORIAL
    // ===============================
    public void ShowTutorial()
    {
        tutorialCanvasGroup.gameObject.SetActive(true);
        LeanTween.alphaCanvas(tutorialCanvasGroup, 1, fadeDuration);
    }

    public void HideTutorial()
    {
        LeanTween.alphaCanvas(tutorialCanvasGroup, 0, fadeDuration)
                 .setOnComplete(() =>
                 {
                     tutorialCanvasGroup.gameObject.SetActive(false);
                 });
    }
}
