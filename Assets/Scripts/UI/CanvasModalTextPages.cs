using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CanvasModalTextPages : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textPanel;
    [SerializeField]
    private Button buttonNext;
    [SerializeField]
    private Button buttonBack;
    [TextArea]
    public string longText;
    [SerializeField]
    private PaginationMode paginationMode = PaginationMode.ByPeriod;

    public enum PaginationMode
    {
        ByPeriod,
        ByNewLine
    }

    private List<string> pages = new List<string>();
    private int currentPage = 0;

    void Start()
    {
        PaginateText();
        ShowPage(0);

        buttonNext.onClick.AddListener(NextPage);
        buttonBack.onClick.AddListener(PreviousPage);
    }

    public void PaginateText()
    {
        pages.Clear();
        currentPage = 0;
        if (paginationMode == PaginationMode.ByPeriod)
        {
            string[] sentences = longText.Split(new[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var sentence in sentences)
            {
                string trimmed = sentence.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    pages.Add(trimmed.EndsWith(".") ? trimmed : trimmed + ".");
            }
        }
        else if (paginationMode == PaginationMode.ByNewLine)
        {
            string[] lines = longText.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                string trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    pages.Add(trimmed);
            }
        }
    }

    public void ShowPage(int page)
    {
        if (pages.Count == 0) return;
        if (page < 0 || page >= pages.Count) return;
        textPanel.text = pages[page];
        buttonBack.interactable = page > 0;
        buttonNext.interactable = page < pages.Count - 1;
    }

    void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }
}
