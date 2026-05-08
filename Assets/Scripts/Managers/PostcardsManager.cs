using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class PostcardItem
{
    [SerializeField] private string key;
    [SerializeField] private string title;
    [SerializeField] private Sprite preview;
    [SerializeField] private Sprite postcard;
    [SerializeField] private Sprite pastPicture;

    public string Key => key;
    public string Title => title;
    public Sprite Preview => preview;
    public Sprite Postcard => postcard;
    public Sprite PastPicture => pastPicture;
}

public class PostcardsManager : MonoBehaviour
{
    [SerializeField] private List<PostcardItem> postcards;
    // [SerializeField] private Canvas rewardCanvas;
    // [SerializeField] private Image rewardImageDisplay;
    [SerializeField] private GameObject objectToMove;

    [SerializeField] private string infoTitlePicture;
    [SerializeField] private SpriteRenderer postcardImage;

    [SerializeField] private TextMarkerWithTitle infoMarkerScript;
    [SerializeField] private DisplayPicture displayPictureScript;
    
    public static PostcardsManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public void ShowPostcard(string key, Vector3 positionToMove)
    {
        PostcardItem postcard = GetPostcard(key);
        if (postcard == null)
        {
            Debug.LogWarning("Postcard with key " + key + " not found.");
            return;
        }

        if (objectToMove != null)
        {
            objectToMove.SetActive(true);
            objectToMove.transform.position = positionToMove;
        }

        if (postcardImage != null)
        {
            postcardImage.sprite = postcard.Preview;

            Vector2 size = postcardImage.sprite.bounds.size;

            // Ajustar la escala del objeto para que tenga un tamaño deseado
            float desiredWidth = 0.5f; // 1 unidad en el mundo
            float scale = desiredWidth / size.x;

            postcardImage.transform.localScale = new Vector3(scale, scale, 1f);
        }

    }

    public bool IsCollected(string key) => PlayerPrefs.HasKey(key);

    public PostcardItem GetPostcard(string key) 
        => postcards.Find(p => p.Key == key);

    public List<PostcardItem> GetAllPostcards() => postcards;
}
