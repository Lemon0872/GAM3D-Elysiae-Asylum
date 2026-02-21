using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;

public class TeleportSystem : MonoBehaviour
{
    [Header("Fade Image")]
    public Image fadeImage;

    [Header("Teleport Pairs")]
    public List<TeleportPair> teleportPairs = new();

    private Dictionary<string, Transform> tagCache = new();

    private void Update()
    {
        foreach (var pair in teleportPairs)
        {
            pair.Tick(this);
        }
    }

    [Serializable]
    public class TeleportPair
    {
        [Header("Detection")]
        public string targetTag = "Player";
        public float radius = 2f;
        public bool allowTwoWay = true;
        public float cooldown = 1f;
        private bool wasInsideA;
        private bool wasInsideB;

        [Header("Point A")]
        public Transform pointA;
        public Vector3 pointAPosition;

        [Header("Point B")]
        public Transform pointB;
        public Vector3 pointBPosition;

        [Header("Effect")]
        public GameObject effectPrefab;
        public float fadeDuration = 0.4f;

        [Header("Effect Lifetime")]
        public bool autoDestroyEffect = true;
        public float effectLifetime = 2f;

        private float lastTeleportTime;

        public void Tick(TeleportSystem system)
        {
            Transform target = system.GetTargetByTag(targetTag);
            if (target == null) return;

            Vector3 posA = pointA ? pointA.position : pointAPosition;
            Vector3 posB = pointB ? pointB.position : pointBPosition;

            float distToA = Vector3.Distance(target.position, posA);
            float distToB = Vector3.Distance(target.position, posB);

            bool isInsideA = distToA <= radius;
            bool isInsideB = distToB <= radius;

            bool canTeleport = Time.time > lastTeleportTime + cooldown;

            if (canTeleport)
            {
                if (isInsideA && !wasInsideA)
                {
                    system.StartCoroutine(system.TeleportRoutine(target, posA, posB, this));
                    lastTeleportTime = Time.time;
                    Debug.Log("A qua B");
                }
                else if (allowTwoWay && isInsideB && !wasInsideB)
                {
                    system.StartCoroutine(system.TeleportRoutine(target, posB, posA, this));
                    lastTeleportTime = Time.time;
                    Debug.Log("B qua A");
                }
            }

            // 🔥 LUÔN cập nhật state
            wasInsideA = isInsideA;
            wasInsideB = isInsideB;
        }
    }

    private IEnumerator TeleportRoutine(
        Transform target,
        Vector3 fromPos,
        Vector3 toPos,
        TeleportPair pair)
    {
        Debug.Log("dich chuyen");
        SpawnEffect(pair.effectPrefab, fromPos, pair);

        yield return Fade(1f, pair.fadeDuration);

        CharacterController cc = target.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            target.position = toPos;
            cc.enabled = true;
        }
        else
        {
            target.position = toPos;
        }
        print($"{toPos.x} {toPos.y} {toPos.z}");

        SpawnEffect(pair.effectPrefab, fromPos, pair);

        yield return Fade(0f, pair.fadeDuration);
    }

    private void SpawnEffect(GameObject prefab, Vector3 pos, TeleportPair pair)
    {
        if (prefab == null) return;

        GameObject fx = Instantiate(prefab, pos, Quaternion.identity);

        if (pair.autoDestroyEffect)
        {
            Destroy(fx, pair.effectLifetime);
        }
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        if (fadeImage == null) yield break;

        float start = fadeImage.color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            Color c = fadeImage.color;
            c.a = Mathf.Lerp(start, targetAlpha, time / duration);
            fadeImage.color = c;
            yield return null;
        }

        Color final = fadeImage.color;
        final.a = targetAlpha;
        fadeImage.color = final;
    }

    private Transform GetTargetByTag(string tag)
    {
        if (tagCache.TryGetValue(tag, out Transform cached))
            return cached;

        GameObject obj = GameObject.FindGameObjectWithTag(tag);
        if (obj != null)
        {
            tagCache[tag] = obj.transform;
            return obj.transform;
        }

        return null;
    }
}