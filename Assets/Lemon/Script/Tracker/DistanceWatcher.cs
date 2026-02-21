using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.Cinemachine;

[System.Serializable]
public class DistancePoint
{
    public string id;

    public Transform pointTransform;
    public Vector3 position;
    public float radius = 3f;

    public bool triggerOnce = true;

    [Header("Combination Settings")]
    public bool useCombination;
    public List<int> combinationIndexes = new List<int>();

    public UnityEvent onEnter;

    [HideInInspector] public bool isInside;
    [HideInInspector] public bool isVisited;
    [HideInInspector] public bool hasTriggered;
}
public class DistanceWatcher : MonoBehaviour
{
    [Header("Target Settings")]
    [TagField]
    public string targetTag = "Player";
    private Transform target;

    [Header("Detection Points")]
    public List<DistancePoint> points = new List<DistancePoint>();

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(targetTag);
        if (obj != null)
            target = obj.transform;
    }

    void Update()
    {
        if (target == null) return;

        for (int i = 0; i < points.Count; i++)
        {
            UpdatePointState(i);
        }

        CheckCombinations();
    }
    void UpdatePointState(int index)
    {
        var point = points[index];

        Vector3 pos = point.pointTransform != null
            ? point.pointTransform.position
            : point.position;

        float sqrDistance = (target.position - pos).sqrMagnitude;
        bool insideNow = sqrDistance <= point.radius * point.radius;

        // Detect entering moment
        if (insideNow && !point.isInside)
        {
            point.isVisited = true;

            // Nếu KHÔNG phải point tổ hợp → trigger độc lập
            if (!point.useCombination)
            {
                TriggerPoint(point);
            }
        }

        point.isInside = insideNow;
    }

    void CheckCombinations()
    {
        for (int i = 0; i < points.Count; i++)
        {
            var point = points[i];

            if (!point.useCombination)
                continue;

            if (point.triggerOnce && point.hasTriggered)
                continue;

            // Bản thân point phải được visit trước
            if (!point.isVisited)
                continue;

            bool allVisited = true;

            foreach (int index in point.combinationIndexes)
            {
                if (index < 0)
                    continue;

                if (!points[index].isVisited)
                {
                    Debug.Log("ch di du point");
                    allVisited = false;
                    break;
                }
            }

            if (allVisited)
            {
                point.onEnter?.Invoke();
                point.hasTriggered = true;
            }
        }
    }

    void TriggerPoint(DistancePoint point)
    {
        if (point.useCombination) return;
        if (point.triggerOnce && point.hasTriggered) return;

        point.onEnter?.Invoke();
        point.hasTriggered = true;
    }
    public void testtracker()
    {
        Debug.Log("da den dich");
    }
}