using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCollectable : MonoBehaviour, IInteractable
{
    [SerializeField] private string key;
    [SerializeField] private Sprite imageAsset;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        UpdateVisuals();

        CollectablesManager.OnCollectableCollected += HandleCollectableCollected;
    }

    private void OnDestroy()
    {
        CollectablesManager.OnCollectableCollected -= HandleCollectableCollected;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        CollectablesManager.Instance.TryCollect(key);
    }
    
    public bool CanInteract()
    {
        return !CollectablesManager.Instance.IsCollected(key);
    }

    private void HandleCollectableCollected(string collectedKey)
    {
        if (collectedKey == key)
            UpdateVisuals();
    }
    
    private void UpdateVisuals()
    {
        if (CollectablesManager.Instance.IsCollected(key))
            spriteRenderer.sprite = imageAsset;
    }
}
