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
    [SerializeField] private Sprite postcard;
    [SerializeField] private Sprite pastPicture;
    [SerializeField] private Sprite presentPicture;

    public string Key => key;
    public string Title => title;
    public Sprite Postcard => postcard;
    public Sprite PastPicture => pastPicture;
    public Sprite PresentPicture => presentPicture;
}

public class PostcardsManager : MonoBehaviour
{
    [SerializeField] private List<PostcardItem> postcards;
    
    [SerializeField] private GameObject objectToMove;

    [SerializeField] private Sprite defaultImage;
    [SerializeField] private string infoTitlePicture;
    [SerializeField] private Image postcardImageRenderer;
    [SerializeField] private Image pastPictureRenderer;
    [SerializeField] private Image presentPictureRenderer;

    [SerializeField] private Canvas postcardWarningCanvas;
    [SerializeField] private Image warningImageDisplay;
    [SerializeField] private TMP_Text warningTitleText;
    
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
        if (!IsCollected(key))
        {
            ShowPostcardWarning(key);
            return;
        }

        PostcardItem postcard = GetPostcard(key);

        presentPictureRenderer.sprite = postcard != null && postcard.PresentPicture != null ? postcard.PresentPicture : defaultImage;
        pastPictureRenderer.sprite = postcard != null && postcard.PastPicture != null ? postcard.PastPicture : defaultImage;
        postcardImageRenderer.sprite = postcard != null && postcard.Postcard != null ? postcard.Postcard : defaultImage;

        if (objectToMove != null)
        {
            objectToMove.SetActive(true);
            objectToMove.transform.position = positionToMove;
        }

        StartCoroutine(AppearAnimation(objectToMove));

    }

    public bool IsCollected(string key) => PlayerPrefs.HasKey(key);

    public PostcardItem GetPostcard(string key) 
        => postcards.Find(p => p.Key == key);

    public List<PostcardItem> GetAllPostcards() => postcards;

    private IEnumerator AppearAnimation(GameObject obj)
    {
        Vector3 originalScale = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero;

        float appearDuration = 0.3f;
        float time = 0f;

        while (time < appearDuration)
        {
            float t = time / appearDuration;
            obj.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = originalScale;
    }

    public void ShowPostcardWarning(string key)
    {
        PostcardItem postcard = GetPostcard(key);

        if (postcard == null)
        {
            Debug.LogWarning($"Postcard with key '{key}' not found");
            return;
        }

        if (warningImageDisplay != null && postcard.PresentPicture != null)
        {
            warningImageDisplay.sprite = postcard.PresentPicture;
            warningImageDisplay.preserveAspect = true;
        }

        if (warningTitleText != null)
        {
            warningTitleText.text = postcard.Title;
        }

        postcardWarningCanvas?.gameObject.SetActive(true);
    }
}
