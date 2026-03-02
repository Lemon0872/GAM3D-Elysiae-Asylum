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
            Debug.LogWarning("Missing Renderer or Material!");
            return;
        }

        objectRenderer.material = newMaterial;

        hasChanged = true;

        Debug.Log("Whale color changed. Puzzle finished!");

        // 🔥 Invoke event
        OnWhaleColorChanged?.Invoke();
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