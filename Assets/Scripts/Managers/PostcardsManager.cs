using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PostcardsManager : MonoBehaviour
{
    [SerializeField] private List<PostcardItem> postcards;
    
    [SerializeField] private GameObject objectToMove;

    [SerializeField] private PostcardUIDisplay uiDisplay;

    [SerializeField] private Canvas postcardWarningCanvas;
    [SerializeField] private Image warningImageDisplay;
    [SerializeField] private TMP_Text warningTitleText;

    private PostcardItem currentPostcard;
    
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

        currentPostcard = GetPostcard(key);
        uiDisplay.DisplayPostcard(currentPostcard);
        
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

    public PostcardItem GetCurrentPostcard() => currentPostcard;

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
        currentPostcard = GetPostcard(key);

        if (currentPostcard == null)
        {
            Debug.LogWarning($"Postcard with key '{key}' not found");
            return;
        }

        if (warningImageDisplay != null && currentPostcard.PresentPicture != null)
        {
            warningImageDisplay.sprite = currentPostcard.PresentPicture.Picture;
            warningImageDisplay.preserveAspect = true;
        }

        if (warningTitleText != null)
        {
            warningTitleText.text = currentPostcard.Postcard.Title ?? "Postcard no disponible";
        }

        postcardWarningCanvas?.gameObject.SetActive(true);
    }
}
