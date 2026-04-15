using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour {
    public float interactRange =3f;

    void Start()
    {
        MouseLock();
        Debug.Log("da lock");
    }

    private void MouseUnLock()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void MouseLock()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() 
    {
        if (Keyboard.current.eKey.wasPressedThisFrame) {
            IInteractable interactable = GetInteractableObject();
            if (interactable != null) {
                interactable.Interact(transform);
            }
        }
        if(Input.GetMouseButtonDown(2)) MouseUnLock();
    }

    public IInteractable GetInteractableObject() 
    {
        List<IInteractable> interactableList = new List<IInteractable>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray) {
            if (collider.TryGetComponent(out IInteractable interactable)) {
                interactableList.Add(interactable);
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList) {
            if (closestInteractable == null) 
            {
                closestInteractable = interactable;
            } 
            else 
            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) < 
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) {
                    // Closer
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }
}