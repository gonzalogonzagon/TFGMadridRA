using UnityEngine;

public class InvokeInteract : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject targetObject;

    [SerializeField]
    private bool hideObjectOnInteract = true; // Whether to hide the object on interaction

    public void Interact()
    {
        if (targetObject != null)
        {
            IInteractable interactable = targetObject.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract())
            {
                if (hideObjectOnInteract)
                {
                    gameObject.SetActive(false);
                    Debug.Log("Hiding " + gameObject.name + " after interaction."); // ////
                }
                interactable.Interact();
                Debug.Log("Invoked Interact on " + targetObject.name + " from " + gameObject.name); // Log interaction ////
            }
            else
            {
                Debug.LogWarning("Target object does not have a valid IInteractable component in InvokeInteract script on " + gameObject.name);
            }
        }
        else
        {
            Debug.LogWarning("Target object is not assigned in InvokeInteract script on " + gameObject.name);
        }
    }

    public bool CanInteract()
    {
        return true;
    }
}
