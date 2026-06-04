using UnityEngine;
using TMPro;

public class TextMarker : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvasToShow;
    [SerializeField] private TMP_Text descriptionField;
    [SerializeField] private string infoDescription = "Este es un punto de interés.";
    

    public void Interact()
    {
        if (!CanInteract()) return;

        if (canvasToShow != null)
            canvasToShow.gameObject.SetActive(true);

        if (descriptionField != null)
            descriptionField.text = infoDescription;
    }

    public bool CanInteract() => enabled && gameObject.activeInHierarchy;

    public string InfoDescription
    {
        get => infoDescription;
        set
        {
            infoDescription = value;
            if (descriptionField != null)
                descriptionField.text = infoDescription;
        }
    }
}
