using UnityEngine;

[System.Serializable]
public class PictureInfo
{
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private string dataTitle;
    [SerializeField] private string dataDescription;
    [SerializeField] private Sprite picture;

    public string Title => title;
    public string Description => description;
    public string DataTitle => dataTitle;
    public string DataDescription => dataDescription;
    public Sprite Picture => picture;
}
