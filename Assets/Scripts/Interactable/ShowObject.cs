using UnityEngine;

public class ShowObject : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject targetObject; // The object to show when interacted with

    [SerializeField]
    private bool hideObjectOnInteract = true; // Whether to hide the object on interaction

    public void Interact()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            Debug.Log("Interacted with " + gameObject.name + ", showing " + targetObject.name); // Log interaction ////

            if (hideObjectOnInteract)
            {
                gameObject.SetActive(false);
                Debug.Log("Hiding " + gameObject.name + " after interaction."); // ////
            }
        } else
        {
            Debug.LogWarning("Target object is not assigned in ShowObject script on " + gameObject.name);
        }

    }

    public bool CanInteract()
    {
        // Interaction is only possible if the component is enabled and the GameObject is active in the hierarchy.
        return enabled && gameObject.activeInHierarchy;
    }
}
