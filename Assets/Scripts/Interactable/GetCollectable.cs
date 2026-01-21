using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCollectable : MonoBehaviour, IInteractable
{
    [SerializeField] private Canvas canvasToShow;
    [SerializeField] private Image canvasImageDisplay;
    [SerializeField] private Sprite imageAsset;
    [SerializeField] private string key;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(key) && imageAsset != null && spriteRenderer != null)
            spriteRenderer.sprite = imageAsset;
    }

    public void Interact()
    {
        if (!CanInteract())
        {
            Debug.Log("Ya has recogido este coleccionable: " + key);
            return;
        }

        PlayerPrefs.SetInt(key, 1);

        if (spriteRenderer != null && imageAsset != null)
        {
            spriteRenderer.sprite = imageAsset;
        }

        canvasToShow?.gameObject.SetActive(true);
        
        if (canvasImageDisplay != null && imageAsset != null)
        {
            canvasImageDisplay.sprite = imageAsset;
            canvasImageDisplay.preserveAspect = true;
        }
    }
    
    public bool CanInteract()
    {
        return !PlayerPrefs.HasKey(key);
    }
}
