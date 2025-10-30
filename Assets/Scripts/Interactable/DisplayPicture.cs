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
    
    public bool CanInteract()
    {
        // Interaction is allowed only if this component is enabled and the GameObject is active in the hierarchy.
        return enabled && gameObject.activeInHierarchy;
    }

    public Sprite GetImage()
    {
        return imageAsset;
    }
    public void SetImage(Sprite newImage)
    {
        imageAsset = newImage;
    }

    public string getInfoTitlePicture()
    {
        if (textMarkerScript != null)
            return textMarkerScript.getInfoTitle();
        return "";
    }
    public void setInfoTitlePicture(string title)
    {
        if (textMarkerScript != null)
            textMarkerScript.setInfoTitle(title);
    }

    public string getInfoDescriptionPicture()
    {
        if (textMarkerScript != null)
            return textMarkerScript.getInfoDescription();
        return "";
    }
    public void setInfoDescriptionPicture(string description)
    {
        if (textMarkerScript != null)
            textMarkerScript.setInfoDescription(description);
    }

}
