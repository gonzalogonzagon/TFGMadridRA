using UnityEngine;
using UnityEngine.UI;

public class ShowInfoMapMarker : MonoBehaviour, IInteractable
{
    [Header("Data to show")]
    [SerializeField] private string playerPrefsKey;
    [SerializeField] private string infoTitle;
    [SerializeField] private string infoText;
    [SerializeField] private Sprite infoImage;
    [SerializeField] private string infoTitlePicture;
    [SerializeField] private string infoTextPicture;

    [Header("Prefabs and References")]
    [SerializeField] private GameObject objectToMove;
    [SerializeField] private Image uiImage;
    [SerializeField] private TextMarkerWithTitle infoMarkerScript;
    [SerializeField] private DisplayPicture displayPictureScript;
    [SerializeField] private GameObject warningCanvas;
    
    public void Interact()
    {
        // if (string.IsNullOrEmpty(playerPrefsKey))
        // {
        //     if (warningCanvas != null)
        //         warningCanvas.SetActive(true);
        //     return;
        // }

        // if (PlayerPrefs.GetInt(playerPrefsKey, 0) != 1)
        //     return;

        // Mueve el objeto 2 a la posición X,Z del objeto 1 (este script)
        if (objectToMove != null)
        {
            objectToMove.SetActive(true);
            objectToMove.transform.position = transform.position;
        }

        if (uiImage != null && infoImage != null)
        {
            uiImage.sprite = infoImage;
            uiImage.preserveAspect = true;
        }
        
        displayPictureScript?.SetImage(infoImage);
        displayPictureScript?.setInfoTitlePicture(infoTitlePicture);
        displayPictureScript?.setInfoDescriptionPicture(infoTextPicture);

        infoMarkerScript?.setInfoTitle(infoTitle);
        infoMarkerScript?.setInfoDescription(infoText);
    }

    public bool CanInteract()
    {
        return true;
        //return !string.IsNullOrEmpty(playerPrefsKey) && PlayerPrefs.GetInt(playerPrefsKey, 0) == 1 && enabled && gameObject.activeInHierarchy;
    }
}
