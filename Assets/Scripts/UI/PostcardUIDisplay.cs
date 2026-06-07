using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostcardUIDisplay : MonoBehaviour
{
    [SerializeField] private Image postcardImageRenderer;
    [SerializeField] private Image presentPictureRenderer;
    [SerializeField] private Image pastPictureRenderer;
    
    [SerializeField] private TMP_Text pastPictureTitleText;
    [SerializeField] private TMP_Text pastPictureDescriptionText;
    [SerializeField] private TMP_Text presentPictureTitleText;
    [SerializeField] private TMP_Text presentPictureDescriptionText;
    
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private string defaultTitle = "Sin título";

    public void DisplayPostcard(PostcardItem postcard)
    {
        if (postcard == null) return;
        
        DisplayPictureInfo(presentPictureRenderer, presentPictureTitleText, 
            presentPictureDescriptionText, postcard.PresentPicture);
            
        DisplayPictureInfo(pastPictureRenderer, pastPictureTitleText, 
            pastPictureDescriptionText, postcard.PastPicture);
            
        postcardImageRenderer.sprite = postcard.Postcard?.Picture ?? defaultImage;
    }

    private void DisplayPictureInfo(Image imageRenderer, TMP_Text titleText, 
        TMP_Text descriptionText, PictureInfo pictureInfo)
    {
        imageRenderer.sprite = pictureInfo?.Picture ?? defaultImage;
        titleText.text = !string.IsNullOrEmpty(pictureInfo?.Title) ? pictureInfo.Title : defaultTitle;
        descriptionText.text = pictureInfo?.Description ?? string.Empty;
    }
}