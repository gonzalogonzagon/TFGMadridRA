using UnityEngine;
using TMPro;

public class TextMarkerPostcard : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvasToShow;
    [SerializeField] private TMP_Text titleField;
    [SerializeField] private TMP_Text descriptionField;
    
    [SerializeField] private PostcardDataEnum dataType = PostcardDataEnum.PresentData;
    
    public void Interact()
    {
        if (!CanInteract()) return;

        PostcardItem currentPostcard = PostcardsManager.Instance.GetCurrentPostcard();
        if (currentPostcard == null)
        {
            Debug.LogWarning("No postcard is currently selected");
            return;
        }

        if (dataType == PostcardDataEnum.PresentData)
        {
            titleField.text = currentPostcard.PresentPicture.DataTitle;
            descriptionField.text = currentPostcard.PresentPicture.DataDescription;
        }
        else if (dataType == PostcardDataEnum.PastData)
        {            
            titleField.text = currentPostcard.PastPicture.DataTitle;
            descriptionField.text = currentPostcard.PastPicture.DataDescription;
        }
        else if (dataType == PostcardDataEnum.PostcardData)
        {
            titleField.text = currentPostcard.Postcard.DataTitle;
            descriptionField.text = currentPostcard.Postcard.DataDescription;
        }
        
        canvasToShow?.gameObject.SetActive(true);
    }

    public bool CanInteract() => enabled && gameObject.activeInHierarchy;
}
