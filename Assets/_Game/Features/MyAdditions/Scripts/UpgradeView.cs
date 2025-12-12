using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeView : MonoBehaviour
{
    [SerializeField] private List<UpgradeButtonView> buttons;
    [SerializeField] private Button showUpgradesButton;
    [SerializeField] private TextMeshProUGUI showUpgradesText;
    [SerializeField] private GameObject container;

    private bool containerIsVisible;

    public event Action<UpgradeType> UpgradeClicked;

    private void Awake()
    {
        showUpgradesButton.onClick.AddListener(ToggleUpgradeVisibility);

        foreach (var btn in buttons)
        {
            btn.Clicked -= OnButtonClicked;
            btn.Clicked += OnButtonClicked;
        }
    }

    private void ToggleUpgradeVisibility()
    {
        if (containerIsVisible)
        {
            container.SetActive(false);
            containerIsVisible = false;
            showUpgradesText.text = "Show Upgrades";
        }
        else
        {
            container.SetActive(true);
            containerIsVisible = true;
            showUpgradesText.text = "Hide Upgrades";
        }
    }


    public void Setup(IReadOnlyList<UpgradeType> keysToShow)
    {
        foreach (var b in buttons)
            b.SetVisible(false);

        foreach (var k in keysToShow)
        {
            var btn = FindButton(k);
            btn.SetVisible(true);
        }
    }

    public void Render(UpgradeViewModel viewModel)
    {
        var btn = FindButton(viewModel.Type);

        btn.Render(viewModel);
    }

    private void OnButtonClicked(UpgradeType key)
    {
        UpgradeClicked?.Invoke(key);
    }

    private UpgradeButtonView FindButton(UpgradeType key)
    {
        foreach (var btn in buttons)
        {
            if (btn.key.Equals(key))
                return btn;
        }

        return null;
    }
}