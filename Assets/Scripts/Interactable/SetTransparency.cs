using UnityEngine;

public class SetTransparency : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer targetSprite;

    public void SetSpriteAlpha() 
    {
        if (targetSprite != null) {
            Color c = targetSprite.color;
            c.a = (c.a == 1f) ? 0.5f : 1f;
            targetSprite.color = c;
        }
    }

    public void Interact() => SetSpriteAlpha();
    public bool CanInteract() => enabled && gameObject.activeInHierarchy;
}
