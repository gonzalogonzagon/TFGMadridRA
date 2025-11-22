using UnityEngine;
using TMPro;
using UIConstants;

public class TextMarkerWithPages : MonoBehaviour, IInteractable
{
    [TextArea] public string infoDescription = "This is a point of interest description.";
    [SerializeField] private Canvas canvasToShow;
    [SerializeField] private CanvasModalTextPages modalTextPages;
    [SerializeField] private PaginationMode paginationMode = PaginationMode.ByPeriod;

    public void Interact()
    {
        if (canvasToShow != null)
            canvasToShow.gameObject.SetActive(true);

        ChangeContent(infoDescription);
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

    public void ChangeContent(string newText)
    {
        if (modalTextPages != null)
        {
            modalTextPages.longText = newText;
            modalTextPages.PaginationMode = paginationMode;
            modalTextPages.PaginateText();
            modalTextPages.ShowPage(0);
        }
        else
        {
            Debug.LogWarning("Please assign the reference to CanvasModalTextPages in TextMarkerWithPages.");
        }
    }
}
