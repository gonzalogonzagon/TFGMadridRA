using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class Film : MonoBehaviour, IInteractable
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject playButtonObject;
    [SerializeField] private Canvas canvasToDisable;

    private void Awake()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void Interact()
    {
        videoPlayer.frame = 0;
        videoPlayer.Play();
        playButtonObject?.SetActive(false);
        canvasToDisable?.gameObject.SetActive(true);
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        canvasToDisable?.gameObject.SetActive(false);
        playButtonObject?.SetActive(true);
    }

    public bool CanInteract()
    {
        return true;
    }
}
