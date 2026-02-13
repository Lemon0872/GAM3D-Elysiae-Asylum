using UnityEngine;

[System.Serializable]
public class SummonEntry
{
    public GameObject prefab;
    public GameObject particle;
    public Vector3 spawnPosition;
    [HideInInspector] public bool hasSummoned;
}
