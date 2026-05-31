using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Postcard : MonoBehaviour, IInteractable
{
    [SerializeField] private string postcardKey;

    private void Start()
    {
        if (PostcardsManager.Instance.IsCollected(postcardKey))
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = PostcardsManager.Instance.GetPostcard(postcardKey)?.Postcard;
        }
    }
    
    public void Interact()
    {
        PostcardsManager.Instance.ShowPostcard(postcardKey, transform.position);
    }

    public bool CanInteract()
    {
        return true;
    }
}
