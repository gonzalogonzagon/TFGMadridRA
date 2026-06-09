using UnityEngine;
using TMPro;

public class TextMarkerPostcard : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvasToShow;
    [SerializeField] private TMP_Text titleField;
    [SerializeField] private TMP_Text descriptionField;
    
    [SerializeField] private PostcardDataEnum dataType = PostcardDataEnum.PresentData;
    [SerializeField] private bool useDataFields = true;
    
    public void Interact()
    {
        if (!CanInteract()) return;

        PostcardItem currentPostcard = PostcardsManager.Instance.GetCurrentPostcard();
        if (currentPostcard == null)
        {
            Debug.LogWarning("No postcard is currently selected");
            return;
        }
        
        var pictureInfo = GetCurrentPostcardData(currentPostcard);
        if (pictureInfo == null)        {
            Debug.LogWarning($"No data found for {dataType} in postcard {currentPostcard.Key}");
            return;
        }
        titleField.text = useDataFields ? pictureInfo.DataTitle : pictureInfo.Title;
        descriptionField.text = useDataFields ? pictureInfo.DataDescription : pictureInfo.Description;
        canvasToShow?.gameObject.SetActive(true);
    }

    public bool CanInteract() => enabled && gameObject.activeInHierarchy;

    private PictureInfo GetCurrentPostcardData(PostcardItem currentPostcard)
    {
        return dataType switch
        {
            PostcardDataEnum.PresentData => currentPostcard.PresentPicture,
            PostcardDataEnum.PastData => currentPostcard.PastPicture,
            PostcardDataEnum.PostcardData => currentPostcard.Postcard,
            _ => null
        };
    }
}
