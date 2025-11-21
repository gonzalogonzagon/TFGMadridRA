using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSliderController : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Slider audioSlider;
    [SerializeField]
    private TMP_Text durationText;
    [SerializeField]
    private Button btnPlayPause;
    [SerializeField]
    private Button btnReset;
    [SerializeField]
    private Sprite playIcon;
    [SerializeField]
    private Sprite pauseIcon;

    private bool isDragging = false;
    private bool isPlaying = false;

    void Start()
    {
        if (audioSource.clip != null)
        {
            audioSlider.maxValue = audioSource.clip.length;
            audioSlider.value = 0;
        }

        btnPlayPause.onClick.AddListener(OnPlayPauseClicked);
        btnReset.onClick.AddListener(OnResetClicked);
        audioSource.Stop();
        isPlaying = false;
    }

    void Update()
    {
        if (audioSource.isPlaying && !isDragging)
        {
            audioSlider.value = audioSource.time;
        }

        durationText.text = FormatTime(audioSource.time) + " / " + FormatTime(audioSource.clip.length);
    }

    public void OnSliderValueChanged(float value)
    {
    }

    public void OnSliderPointerDown()
    {
        isDragging = true;
    }

    public void OnSliderPointerUp()
    {
        audioSource.time = audioSlider.value;
        isDragging = false;
    }

    void SetPlayPauseIcon(bool isPlaying)
    {
        Image icon = btnPlayPause.GetComponent<Image>();
        if (icon != null)
            icon.sprite = isPlaying ? pauseIcon : playIcon;
    }

    public void OnPlayPauseClicked()
    {
        if (!isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
            SetPlayPauseIcon(true);
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                SetPlayPauseIcon(false);
            }
            else
            {
                audioSource.Play();
                SetPlayPauseIcon(true);
            }
        }
    }

    public void OnResetClicked()
    {
        audioSource.Stop();
        audioSource.time = 0;
        audioSlider.value = 0;
        isPlaying = false;
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
