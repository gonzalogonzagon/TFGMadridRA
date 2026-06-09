using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class CollectableData
{
    [SerializeField] private string _key;
    [SerializeField] private bool _collected;

    public string Key => _key;
    public bool IsCollected => _collected;

    public CollectableData() { }

    public CollectableData(string key, bool collected = false)
    {
        _key = key;
        _collected = collected;
    }

    public void MarkAsCollected() => _collected = true;
}

[System.Serializable]
public class SaveData
{
    public List<CollectableData> collectables = new();
}

public class SaveLoadManager : MonoBehaviour
{
    private static SaveLoadManager _instance;
    private SaveData _saveData;
    private string _savePath;
    
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
    
    // Cargar datos desde JSON
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
    
    // Guardar datos a JSON
    public void SaveProgress()
    {
        string json = JsonUtility.ToJson(_saveData, true);
        File.WriteAllText(_savePath, json);
        Debug.Log($"Juego guardado en: {_savePath}");
    }
    
    // Verificar si un coleccionable está recogido
    public bool IsCollected(string key)
    {
        return _saveData.collectables.Find(c => c.Key == key)?.IsCollected ?? false;
    }
    
    // Marcar un coleccionable como recogido
    public bool TryCollect(string key)
    {
        var collectableData = _saveData.collectables.Find(c => c.Key == key);
        
        if (collectableData != null && collectableData.IsCollected)
            return false; // Ya fue recogido
        
        if (collectableData == null)
        {
            collectableData = new CollectableData(key, false);
            _saveData.collectables.Add(collectableData);
        }
        
        collectableData.MarkAsCollected();
        SaveProgress();
        return true;
    }
}
