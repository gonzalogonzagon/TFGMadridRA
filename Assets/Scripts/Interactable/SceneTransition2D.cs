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
        // Turn off the current
        if (currentFocusObject != null)
        {
            currentFocusObject.SetActive(false);
        }

        // Turn on the new
        if (nextFocusObject != null)
        {
            nextFocusObject.SetActive(true);

            // Update the target in the touch manager
            touchManager.SetTargetContent(nextFocusObject.transform, minOrthoSize, maxOrthoSize, initialOrthoSize);

        }
    }

    public bool CanInteract()
    {
        return true;
    }

}
