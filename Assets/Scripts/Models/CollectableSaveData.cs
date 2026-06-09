using System.Collections.Generic;
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

