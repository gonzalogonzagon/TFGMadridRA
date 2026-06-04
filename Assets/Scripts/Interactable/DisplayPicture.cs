using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayPicture : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvasToShow;
    [SerializeField] private Image imageDisplay;
    [SerializeField] private Sprite imageAsset;

    [SerializeField] private Button moreInfoButton;
    [SerializeField] private TextMarkerWithTitle textMarkerScript;


    public void Interact()
    {
        if (!CanInteract()) return;
        
        if (canvasToShow != null)
        {
            canvasToShow.gameObject.SetActive(true);
            
            if (imageDisplay != null && imageAsset != null)
            {
                imageDisplay.sprite = imageAsset;
                imageDisplay.preserveAspect = true;
            }

            if (textMarkerScript != null && moreInfoButton != null)
            {
                moreInfoButton.onClick.RemoveAllListeners();
                moreInfoButton.onClick.AddListener(textMarkerScript.Interact);
            }
        }
        else
        {
            Debug.LogError("No image panel has been assigned in " + gameObject.name);
        }
    }
    
    public bool CanInteract() => enabled && gameObject.activeInHierarchy;

    public Image ImageDisplay{
        get => imageDisplay;
        set => imageDisplay = value;
    }
}
