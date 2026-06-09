using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    [Tooltip("List of objects to toggle visibility")]
    public List<GameObject> objectsToToggle;

    private bool isHidden = false;

    /// <summary>
    /// Toggles the visibility of the selected objects
    /// Method to be called from a button
    /// </summary>
    public void ToggleContent()
    {
        if (objectsToToggle == null || objectsToToggle.Count == 0)
            return;

        isHidden = !isHidden;

        foreach (GameObject obj in objectsToToggle)
        {
            if (obj != null)
                obj.SetActive(!isHidden);
        }
    }
}
