using UnityEngine;

[System.Serializable]
public class CollectableItem
{
    [SerializeField] private string key;
    [SerializeField] private string title;
    [SerializeField] private Sprite preview;
    [SerializeField] private Sprite postcard;

    public string Key => key;
    public string Title => title;
    public Sprite Preview => preview;
    public Sprite Postcard => postcard;
}
