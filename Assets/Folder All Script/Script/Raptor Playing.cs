using UnityEngine;
using System.Collections.Generic;

public class RaptorPlaying : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 7f;

    [Header("Teleport Settings")]
    [SerializeField] private List<Transform> teleportPoints = new List<Transform>();
    [SerializeField] private int maxTeleports = 2;

    [Header("VFX")]
    [SerializeField] private ParticleSystem teleportEffect;

    private int teleportCount = 0;
    private bool isTeleporting = false;

    private void Update()
    {
        if (player == null || isTeleporting) return;
        if (teleportCount >= maxTeleports) return;
        if (teleportCount >= teleportPoints.Count) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= detectionRange)
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        isTeleporting = true;

        // Play particle effect at current position
        if (teleportEffect != null)
        {
            Instantiate(teleportEffect, transform.position, Quaternion.identity);
        }

        // Teleport to next point in list
        Transform targetPoint = teleportPoints[teleportCount];
        transform.position = targetPoint.position;

        teleportCount++;

        // Disable after reaching limit
        if (teleportCount >= maxTeleports)
        {
            enabled = false;
        }

        isTeleporting = false;
    }
}