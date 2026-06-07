using UnityEngine;
using TMPro;

public class TextMarkerWithTitle : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvasToShow;
    [SerializeField] private TMP_Text titleField;
    [SerializeField] private TMP_Text descriptionField;

    [SerializeField] private string infoTitle = "Información";
    [SerializeField] private string infoDescription = "Este es un punto de interés.";
    

    public void Interact()
    {
        if (!CanInteract()) return;
        
        canvasToShow?.gameObject.SetActive(true);
            
        if (titleField != null)
            titleField.text = infoTitle;
        else
            Debug.LogWarning("No title field assigned to TextMarkerWithTitle.");

        if (descriptionField != null)
            descriptionField.text = infoDescription;
        else
            Debug.LogWarning("No description field assigned to TextMarkerWithTitle.");
    }

    public bool CanInteract() => enabled && gameObject.activeInHierarchy;

    public string InfoDescription
    {
        get => infoDescription;
        private set
        {
            infoDescription = value;
            if (descriptionField != null)
                descriptionField.text = infoDescription;
        }
    }

    public string InfoTitle
    {
        get => infoTitle;
        private set
        {
            infoTitle = value;
            if (titleField != null)
                titleField.text = infoTitle;
        }
    }
}
