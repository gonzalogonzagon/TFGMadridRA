using Vuforia;
using UnityEngine;
using System.Collections;

public class ContentPlacementController : MonoBehaviour
{
    [SerializeField] private GameObject contentRoot;
    [SerializeField] private GameObject planeFinder;
    [SerializeField] private ContentPositioningBehaviour contentPositioning;

    void Start()
    {
        // When Vuforia places the content, execute OnContentPlaced
        contentPositioning.OnContentPlaced.AddListener(OnContentPlaced);
    }

    public void StartRepositioning()
    {
        contentRoot.SetActive(false);
        planeFinder.SetActive(true);
    }

    private void OnContentPlaced(GameObject placedContent)
    {
        contentRoot.SetActive(true);
        planeFinder.SetActive(false);

        StartCoroutine(AppearAnimation());
    }

    private IEnumerator AppearAnimation()
    {
        // Initial scale is zero
        contentRoot.transform.localScale = Vector3.zero;

        float duration = 0.4f;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            contentRoot.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure final exact scale
        contentRoot.transform.localScale = Vector3.one;
    }
}
