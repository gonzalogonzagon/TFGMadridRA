using UnityEngine;
using UnityEngine.UI;

public class ShowInfoMapMarker : MonoBehaviour, IInteractable
{
    [Header("Data to show")]
    [SerializeField] private string playerPrefsKey;
    [SerializeField] private string infoTitle;
    [SerializeField] private string infoText;
    [SerializeField] private Sprite infoImage;

    [Header("Prefabs and References")]
    [SerializeField] private Transform objectToMove;
    [SerializeField] private Transform objectToMove2;
    [SerializeField] private Image uiImage;
    [SerializeField] private TextMarkerWithTitle infoMarker;
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
            objectToMove.gameObject.SetActive(true);
            objectToMove2.gameObject.SetActive(true);
            Vector3 newPos = objectToMove.position;
            newPos.x = transform.position.x;
            newPos.y = objectToMove.position.y;
            newPos.z = transform.position.z;
            objectToMove.position = newPos;
            newPos.y = objectToMove2.position.y;
            objectToMove2.position = newPos;
        }

        // Asigna la imagen a la UI
        if (uiImage != null && infoImage != null)
        {
            uiImage.sprite = infoImage;
            uiImage.preserveAspect = true;
        }

        // Asigna el texto al InfoMarker1
        if (infoMarker != null)
        {
            infoMarker.setInfoTitle(infoTitle);
            infoMarker.setInfoDescription(infoText);
        }
    }

    public bool CanInteract()
    {
        return true;
        //return !string.IsNullOrEmpty(playerPrefsKey) && PlayerPrefs.GetInt(playerPrefsKey, 0) == 1 && enabled && gameObject.activeInHierarchy;
    }
}
