using UnityEngine;
using System.Collections.Generic;

public class SegmentedBar : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private int maxSegments = 5;

    public List<GameObject> segments = new();
    private int currentCount = 0;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        ClearSegments();

        for (int i = 0; i < maxSegments; i++)
        {
            GameObject segment = Instantiate(segmentPrefab, transform);
            segment.SetActive(false);
            segments.Add(segment);
        }

        currentCount = 0;
    }

    public void SetValue(int value)
    {
        int newCount = Mathf.Clamp(value, 0, maxSegments);

        if (newCount == currentCount)
            return;

        if (newCount > currentCount)
        {
            // BẬT thêm (index lớn trước)
            for (int i = maxSegments - currentCount - 1;
                 i >= maxSegments - newCount;
                 i--)
            {
                segments[i].SetActive(true);
            }
        }
        else
        {
            // TẮT bớt (index lớn trước)
            for (int i = maxSegments - currentCount;
                 i < maxSegments - newCount;
                 i++)
            {
                segments[i].SetActive(false);
            }
        }

        currentCount = newCount;
    }

    private void ClearSegments()
    {
        foreach (var segment in segments)
        {
            if (segment != null)
                Destroy(segment);
        }

        segments.Clear();
        currentCount = 0;
    }
}