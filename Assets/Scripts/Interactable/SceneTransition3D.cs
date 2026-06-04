using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition3D : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject currentFocusObject;
    [SerializeField] private GameObject nextFocusObject;

    public void Interact()
    {
        if (!CanInteract()) return;
        currentFocusObject?.SetActive(false);
        nextFocusObject?.SetActive(true);
    }

    public bool CanInteract() => enabled && gameObject.activeInHierarchy;
}
