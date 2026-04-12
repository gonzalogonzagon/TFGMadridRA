using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShowObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private bool hideObjectOnInteract = true;
    [SerializeField] private bool playAppearAnimation = true;

    public void Interact()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target object is not assigned in ShowObject on " + gameObject.name);
            return;
        }

        targetObject.SetActive(true);

        if (playAppearAnimation)
            StartCoroutine(AppearAnimation(targetObject));

        if (hideObjectOnInteract)
            HideSelf();
    }

    public bool CanInteract()
    {
        // Interaction is only possible if the component is enabled and the GameObject is active in the hierarchy.
        return enabled && gameObject.activeInHierarchy;
    }

    private void HideSelf()
    {
        //Check if this GameObject has a Canvas component (covers UI panels, etc.)
        if (GetComponent<Canvas>() != null)
        {
            gameObject.SetActive(false);
            Debug.Log("Hiding Canvas " + gameObject.name); ////
            return;
        }

        // Check if it has a Graphic component (covers most UI elements)
        if (GetComponent<Graphic>() != null)
        {
            gameObject.SetActive(false);
            Debug.Log("Hiding UI element " + gameObject.name); ////
            return;
        }

        // Case 3: 3D object - disable Renderer and Collider instead of deactivating the GameObject
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
            rend.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        Debug.Log("Hiding 3D object " + gameObject.name); ////
    }
    
    private IEnumerator AppearAnimation(GameObject obj)
    {
        Vector3 originalScale = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero;

        float appearDuration = 0.3f;
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
