using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShowObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private ShowInteractionEnum showBehavior = ShowInteractionEnum.ShowWithAnimationAndHide;
    [SerializeField] private float appearDuration = 0.3f;

    public void Interact()
    {
        if (!CanInteract()) return;
        if (targetObject == null)
        {
            Debug.LogWarning("Target object is not assigned in ShowObject on " + gameObject.name);
            return;
        }

        targetObject.SetActive(true);

        switch (showBehavior)
        {
            case ShowInteractionEnum.ShowOnly:
                break;
            case ShowInteractionEnum.ShowWithAnimation:
                StartCoroutine(AppearAnimation(targetObject));
                break;
            case ShowInteractionEnum.ShowWithAnimationAndHide:
                StartCoroutine(AppearAnimationThenHide(targetObject));
                break;
            case ShowInteractionEnum.HideThis:
                gameObject.SetActive(false);
                break;
        }
    }

    // Interaction is only possible if the component is enabled and the GameObject is active in the hierarchy
    public bool CanInteract() => enabled && gameObject.activeInHierarchy;

    private IEnumerator AppearAnimationThenHide(GameObject obj)
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
            rend.enabled = false;

        Image img = GetComponent<Image>();
        if (img != null)
            img.enabled = false;

        yield return StartCoroutine(AppearAnimation(obj));

        gameObject.SetActive(false);
    }
    
    private IEnumerator AppearAnimation(GameObject obj)
    {
        Vector3 originalScale = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero;

        float time = 0f;

        while (time < appearDuration)
        {
            float t = time / appearDuration;
            obj.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = originalScale;
    }
}
