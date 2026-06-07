using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    [SerializeField] private List<CollectableItem> collectables;
    [SerializeField] private Canvas rewardCanvas;
    [SerializeField] private Image rewardImageDisplay;
    
    public static CollectablesManager Instance { get; private set; }
    public static event Action<string> OnCollectableCollected;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    
    public bool TryCollect(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return false;
        
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
        
        var item = GetCollectable(key);
        if (item != null && rewardImageDisplay != null)
        {
            rewardImageDisplay.sprite = item.Postcard;
            rewardImageDisplay.preserveAspect = true;
            rewardCanvas?.gameObject.SetActive(true);
        }

        OnCollectableCollected?.Invoke(key);

        return true;
    }
    
    public bool IsCollected(string key) => PlayerPrefs.HasKey(key);
    
    public CollectableItem GetCollectable(string key) 
        => collectables.Find(c => c.Key == key);
    
    public List<CollectableItem> GetAllCollectables() => collectables;
}