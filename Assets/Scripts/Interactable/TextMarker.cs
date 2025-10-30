using UnityEngine;
using TMPro;

public class TextMarker : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvasToShow;
    [SerializeField] private TMP_Text descriptionField;
    [SerializeField] private string infoDescription = "Este es un punto de interés.";
    

    public void Interact()
    {
        if (canvasToShow != null)
            canvasToShow.gameObject.SetActive(true);

        if (descriptionField != null)
            descriptionField.text = infoDescription;
        else
            Debug.LogWarning("No se ha asignado el campo de descripción a TextMarker.");
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
}
