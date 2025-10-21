using UnityEngine;

public interface IInteractable
{
    // Main method to handle interaction logic
    void Interact();

    // Method to check if interaction is possible
    bool CanInteract();
}
