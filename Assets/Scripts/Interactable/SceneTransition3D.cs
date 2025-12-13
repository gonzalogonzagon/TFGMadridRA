using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition3D : MonoBehaviour, IInteractable
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RTouchManager touchManager;
    [SerializeField] private Vector3 newCameraPosition;
    [SerializeField] private Transform lookAtTarget;

    [Header("Botones a habilitar")]
    [SerializeField] private List<GameObject> buttonsToEnable;
    [Header("Botones a deshabilitar")]
    [SerializeField] private List<GameObject> buttonsToDisable;

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

        foreach (var btn in buttonsToEnable)
        {
            btn?.SetActive(true);
        }
        foreach (var btn in buttonsToDisable)
        {
            btn?.SetActive(false);
        }
    }

    public bool CanInteract()
    {
        return true;
    }
}
