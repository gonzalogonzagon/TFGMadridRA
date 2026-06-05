using UnityEngine;
using UnityEngine.UI;

public class CalleAlcala : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetSprite;
    
    [Range(0f, 1f)]
    [SerializeField] private float invisibleAlpha = 0f;
    [Range(0f, 1f)]
    [SerializeField] private float semiTransparentAlpha = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float fullyVisibleAlpha = 1f;

    private void Start()
    {
        if (targetSprite == null)
        {
            Debug.LogError("No se ha asignado un SpriteRenderer en " + gameObject.name);
            return;
        }

        SetTransparencyInvisible();
    }

    /// <summary>
    /// Makes the sprite completely invisible (alpha = 0)
    /// </summary>
    public void SetTransparencyInvisible()
    {
        SetSpriteAlpha(invisibleAlpha);
    }

    /// <summary>
    /// Makes the sprite semi-transparent (alpha = 0.5)
    /// </summary>
    public void SetTransparencySemiVisible()
    {
        SetSpriteAlpha(semiTransparentAlpha);
    }

    /// <summary>
    /// Makes the sprite completely visible (alpha = 1)
    /// </summary>
    public void SetTransparencyFullyVisible()
    {
        SetSpriteAlpha(fullyVisibleAlpha);
    }

    /// <summary>
    /// Sets the Alpha value of the sprite
    /// </summary>
    private void SetSpriteAlpha(float alpha)
    {
        if (targetSprite == null) return;

        Color newColor = targetSprite.color;
        newColor.a = alpha;
        targetSprite.color = newColor;
    }
}
