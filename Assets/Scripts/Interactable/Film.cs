using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class Film : MonoBehaviour, IInteractable
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject playButtonObject;
    [SerializeField] private Canvas canvasToShow;

    private void Awake()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoEnd;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        
        if (videoPlayer == null)
        {
            Debug.LogWarning("VideoPlayer component is not assigned in Film on " + gameObject.name);
            return;
        }

        videoPlayer.frame = 0;
        videoPlayer.Play();

        playButtonObject?.SetActive(false);
        canvasToShow?.gameObject.SetActive(true);
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        canvasToShow?.gameObject.SetActive(false);
        playButtonObject?.SetActive(true);
    }

    public bool CanInteract() => enabled && gameObject.activeInHierarchy && videoPlayer != null;
}
