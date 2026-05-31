using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSceneTransition3D : MonoBehaviour, IInteractable
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RTouchManager touchManager;
    [SerializeField] private Vector3 newCameraPosition;
    [SerializeField] private Transform lookAtTarget;

    [Header("Content to disable")]
    [SerializeField] private GameObject currentFocusObject;
    [Header("Content to enable")]
    [SerializeField] private GameObject nextFocusObject;

    public void Interact()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = newCameraPosition;
            if (lookAtTarget != null)
            {
                Quaternion lookRotation = Quaternion.LookRotation(lookAtTarget.position - mainCamera.transform.position, Vector3.up);
                touchManager?.SetTargetRotation(lookRotation);
            }
        }

        currentFocusObject?.SetActive(false);
        nextFocusObject?.SetActive(true);
    }

    public bool CanInteract()
    {
        return true;
    }
}
