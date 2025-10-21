using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayPicture : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvasToShow;
    [SerializeField] private Image imageDisplay;
    [SerializeField] private Sprite imageAsset;

    public void Interact()
    {
        if (canvasToShow != null)
        {
            canvasToShow.gameObject.SetActive(true);
            
            if (imageDisplay != null && imageAsset != null)
            {
                imageDisplay.sprite = imageAsset;
                imageDisplay.preserveAspect = true;
            }
        }
        else
        {
            Debug.LogError("No se ha asignado un panel de imagen en " + gameObject.name);
        }
    }
    
    public bool CanInteract()
    {
        return enabled && gameObject.activeInHierarchy;
    }
}
