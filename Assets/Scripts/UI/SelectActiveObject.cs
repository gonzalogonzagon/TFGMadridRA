using System.Collections.Generic;
using UnityEngine;

public class SelectActiveObject : MonoBehaviour
{
    [Tooltip("Object list that can be activated/deactivated")]
    public List<GameObject> objectsToManage;

    // Call this method from the button, passing the corresponding index
    public void ActivateObjectByIndex(int index)
    {
        if (objectsToManage == null || index < 0 || index >= objectsToManage.Count)
            return;

        for (int i = 0; i < objectsToManage.Count; i++)
        {
            if (objectsToManage[i] != null)
                objectsToManage[i].SetActive(i == index);
        }
    }
}
