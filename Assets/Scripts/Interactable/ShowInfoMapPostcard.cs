using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfoMapPostcard : MonoBehaviour, IInteractable
{
    [SerializeField] private string postcardKey;
    [SerializeField] private Sprite imageAsset;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (!PostcardsManager.Instance.IsCollected(postcardKey))
        {
            spriteRenderer.sprite = imageAsset;
        }
    }

    public void Interact()
    {
        if (!PostcardsManager.Instance.IsCollected(postcardKey))
        {
            Debug.Log("No se ha desbloqueado este marcador de mapa");
            return;
        }

        PostcardsManager.Instance.ShowPostcard(postcardKey, transform.position);
    }

    public bool CanInteract()
    {
        return true;
    }
}
