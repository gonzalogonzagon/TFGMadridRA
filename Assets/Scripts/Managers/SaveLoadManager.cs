using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private static SaveLoadManager _instance;
    private SaveData _saveData;
    private string _savePath;
    
    public bool IsCollected(string key)
    {
        return _saveData.collectables.Find(c => c.Key == key)?.IsCollected ?? false;
    }
    
    public bool TryCollect(string key)
    {
        var collectableData = _saveData.collectables.Find(c => c.Key == key);
        
        if (collectableData != null && collectableData.IsCollected)
            return false;
        
        if (collectableData == null)
        {
            collectableData = new CollectableData(key, false);
            _saveData.collectables.Add(collectableData);
        }
        
        collectableData.MarkAsCollected();
        SaveProgress();
        return true;
    }

    // Lifecycle methods ------------------------------------------------------------
    public static SaveLoadManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveLoadManager>();
                if (_instance == null)
                {
                    var go = new GameObject("SaveLoadManager");
                    _instance = go.AddComponent<SaveLoadManager>();
                }
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        _savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        LoadProgress();
    }

    // JSON methods ------------------------------------------------------------
    // Load data from JSON
    public void LoadProgress()
    {
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            _saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            _saveData = new SaveData();
        }
    }
    
    // Save data to JSON
    public void SaveProgress()
    {
        string json = JsonUtility.ToJson(_saveData, true);
        File.WriteAllText(_savePath, json);
    }
}
