using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CanvasCollectablesPages : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private Button buttonBack;
    [SerializeField] private Button buttonNext;

    [Header("Display")]
    [SerializeField] private Image imageDisplay;
    [SerializeField] private TMP_Text titleDisplay;

    [Header("Collect Button")]
    [SerializeField] private Button getCollectableButton;
    [SerializeField] private TMP_Text collectableButtonText;
    [SerializeField] private Image iconButton;
    [SerializeField] private Sprite eyeHintIcon;
    [SerializeField] private Sprite hintCheckIcon;

    private List<CollectableItem> collectableItems;
    private int currentPage = 0;

    void Start()
    {
        collectableItems = CollectablesManager.Instance.GetAllCollectables();

        ShowPage(0);

        buttonNext.onClick.AddListener(NextPage);
        buttonBack.onClick.AddListener(PreviousPage);
        getCollectableButton.onClick.AddListener(GetCollectable);
    }

    private void ShowPage(int page)
    {
        if (page < 0 || page >= collectableItems.Count) return;
        
        imageDisplay.sprite = collectableItems[page].Preview;
        imageDisplay.preserveAspect = true;
        titleDisplay.text = collectableItems[page].Title;

        bool collected = CollectablesManager.Instance.IsCollected(collectableItems[page].Key);

        collectableButtonText.text = collected ? "Ya obtenido" : "Descubrir";
        getCollectableButton.interactable = !collected;
        iconButton.sprite = collected ? hintCheckIcon : eyeHintIcon;

        buttonBack.interactable = page > 0;
        buttonNext.interactable = page < collectableItems.Count - 1;
    }

    private void NextPage()
    {
        if (currentPage < collectableItems.Count - 1)
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

    private void GetCollectable()
    {
        if (CollectablesManager.Instance.IsCollected(collectableItems[currentPage].Key))
            return;

        CollectablesManager.Instance.TryCollect(collectableItems[currentPage].Key);
        ShowPage(currentPage);
    }
}
