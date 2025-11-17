using System.Collections.Generic;
using UnityEngine;

public class SelectActiveObject : MonoBehaviour
{
    [Tooltip("Lista de objetos que se pueden activar/desactivar")]
    public List<GameObject> objectsToManage;

    // Llama este método desde el botón, pasando el índice correspondiente
    public void ActivateObjectByIndex(int index)
    {
        for (int i = 0; i < objectsToManage.Count; i++)
        {
            if (objectsToManage[i] != null)
                objectsToManage[i].SetActive(i == index);
        }
    }
}
