using System;
using UnityEngine;
using UnityEngine.Events;

public class WhaleChangeColor : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText = "E to Change Color";

    [Header("Material Settings")]
    [SerializeField] private Material newMaterial;

    [Header("Puzzle State")]

    [Header("Events")]
    public UnityEvent OnWhaleColorChanged;

    private Renderer objectRenderer;
    private bool hasChanged = false;
    [SerializeField] private GameObject objectToEnable;
    [SerializeField] private float enableDelay = 1f;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    public string GetInteractText()
    {
        if (hasChanged) return "";
        return interactText;
    }

    private void ChangeMaterial()
    {
        if (objectRenderer == null || newMaterial == null)
        {
            return;
        }

        objectRenderer.material = newMaterial;

        hasChanged = true;

        // 🔥 Invoke event
        OnWhaleColorChanged?.Invoke();

        if (objectToEnable != null)
        {
            Invoke(nameof(EnableObject), enableDelay);
        }
    }

    private void EnableObject()
    {
        objectToEnable.SetActive(true);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        if (hasChanged) return;

        ChangeMaterial();
    }

}