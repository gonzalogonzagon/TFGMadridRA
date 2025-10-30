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
        if (canvasToShow != null)
            canvasToShow.gameObject.SetActive(true);
            
        if (titleField != null)
            titleField.text = infoTitle;
        else
            Debug.LogWarning("No se ha asignado el campo de título a InfoMarker1.");

        if (descriptionField != null)
            descriptionField.text = infoDescription;
        else
            Debug.LogWarning("No se ha asignado el campo de descripción a InfoMarker1.");
    }

    public bool CanInteract()
    {
        return true;
    }

    public string getInfoDescription()
    {
        return infoDescription;
    }
    public void setInfoDescription(string description)
    {
        infoDescription = description;
        if (descriptionField != null)
            descriptionField.text = infoDescription;
    }

    public string getInfoTitle()
    {
        return infoTitle;
    }
    public void setInfoTitle(string title)
    {
        infoTitle = title;
        if (titleField != null)
            titleField.text = infoTitle;
    }
}
