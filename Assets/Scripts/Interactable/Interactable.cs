using UnityEngine;

public interface IInteractable
{
    void Interact();    // What to do when interacted with?
    bool CanInteract(); // Is it possible to interact now?
}
