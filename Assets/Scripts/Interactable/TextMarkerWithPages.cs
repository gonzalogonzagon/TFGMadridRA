using UnityEngine;
using TMPro;

public class TextMarkerWithPages : MonoBehaviour, IInteractable
{
    [TextArea] public string infoDescription = "Este es un punto de interés.";
    [SerializeField] private Canvas canvasToShow;

    public CanvasModalTextPages modalTextPages;

    public void Interact()
    {
        if (canvasToShow != null)
            canvasToShow.gameObject.SetActive(true);

        CambiarContenido(infoDescription);
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
    }

    public void CambiarContenido(string nuevoTexto)
    {
        if (modalTextPages != null)
        {
            modalTextPages.longText = nuevoTexto;
            modalTextPages.PaginateText();
            modalTextPages.ShowPage(0);
        }
        else
        {
            Debug.LogWarning("No se ha asignado la referencia a CanvasModalTextPages en InfoMarker3.");
        }
    }
}
