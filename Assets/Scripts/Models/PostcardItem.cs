using UnityEngine;

[System.Serializable]
public class PostcardItem
{
    [SerializeField] private string key;
    [SerializeField] private PictureInfo postcard;
    [SerializeField] private PictureInfo pastPicture;
    [SerializeField] private PictureInfo presentPicture;

    public string Key => key;
    public PictureInfo Postcard => postcard;
    public PictureInfo PastPicture => pastPicture;
    public PictureInfo PresentPicture => presentPicture;

}
