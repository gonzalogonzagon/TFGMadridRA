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
        if (!CanInteract()) return;

        canvasToShow?.gameObject.SetActive(true);
        ChangeContent(infoDescription);
    }

    public bool CanInteract() => enabled && gameObject.activeInHierarchy;

    public string InfoDescription
    {
        get => infoDescription;
        set => infoDescription = value;
    }

    public void ChangeContent(string newText)
    {
        if (modalTextPages == null)
        {
            Debug.LogWarning("Please assign the reference to CanvasModalTextPages in TextMarkerWithPages.");
            return;
        }

        modalTextPages.LongText = newText;
        modalTextPages.PaginationMode = paginationMode;
        modalTextPages.PaginateText();
        modalTextPages.ShowPage(0);
    }
}
