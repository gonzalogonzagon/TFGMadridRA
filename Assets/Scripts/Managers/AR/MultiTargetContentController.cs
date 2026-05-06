using UnityEngine;
using Vuforia;

public class MultiTargetContentController : MonoBehaviour
{
    [SerializeField]
    public GameObject content;

    [SerializeField]
    [Tooltip("List of ImageTargetBehaviour representing the targets to detect")]
    public ImageTargetBehaviour[] targets;

    private ImageTargetBehaviour currentTarget;

    void Start()
    {
        foreach (var target in targets)
        {
            target.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        content.SetActive(false);
    }

    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            if (currentTarget == null)
            {
                currentTarget = (ImageTargetBehaviour)behaviour;
                StartCoroutine(SetContentParentNextFrame(currentTarget.transform));
                content.SetActive(true);
            }
        }
        else if (currentTarget == behaviour)
        {
            content.SetActive(false);
            content.transform.SetParent(null, true);
            currentTarget = null;
        }
    }

    private System.Collections.IEnumerator SetContentParentNextFrame(Transform parent)
    {
        yield return null;
        content.transform.SetParent(parent, true);
    }

    void OnDestroy()
    {
        foreach (var target in targets)
        {
            target.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }
}
