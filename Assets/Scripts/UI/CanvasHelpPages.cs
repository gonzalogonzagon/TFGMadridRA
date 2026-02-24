using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CanvasHelpPages : MonoBehaviour
{
    [SerializeField] private Button buttonBack;
    [SerializeField] private Button buttonNext;

    [SerializeField] private Image imageDisplay;
    [SerializeField] private TMP_Text textDisplay;

    [SerializeField] private List<string> textPages = new List<string>();
    [SerializeField] private List<Sprite> imagePages = new List<Sprite>();

    private int currentPage = 0;

    void Start()
    {
        ShowPage(0);

        buttonNext.onClick.AddListener(NextPage);
        buttonBack.onClick.AddListener(PreviousPage);
    }

    private void ShowPage(int page)
    {
        if (textPages.Count == 0 || imagePages.Count == 0) return;
        if (page < 0 || page >= textPages.Count) return;
        imageDisplay.sprite = imagePages[page];
        imageDisplay.preserveAspect = true;
        textDisplay.text = textPages[page];
        buttonBack.interactable = page > 0;
        buttonNext.interactable = page < textPages.Count - 1;
    }

    private void NextPage()
    {
        if (currentPage < textPages.Count - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    private void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }
}
