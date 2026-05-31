using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition2D : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject currentFocusObject;
    [SerializeField] private GameObject nextFocusObject;

    [SerializeField] private float minOrthoSize = 0.5f;
    [SerializeField] private float maxOrthoSize = 3f;
    [SerializeField] private float initialOrthoSize = 3f;

    [SerializeField] private RTouchManager2D touchManager;

    public void Interact()
    {
        currentFocusObject?.SetActive(false);
        nextFocusObject?.SetActive(true);
        
        touchManager?.SetTargetContent(nextFocusObject.transform, minOrthoSize, maxOrthoSize, initialOrthoSize);
    }

    public bool CanInteract()
    {
        return true;
    }

}
