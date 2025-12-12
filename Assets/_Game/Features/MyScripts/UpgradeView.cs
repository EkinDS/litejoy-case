using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeView : MonoBehaviour
{
    [SerializeField] private List<UpgradeButtonView> buttons = new();
    [SerializeField] private Button showUpgradesButton;
    [SerializeField] private TextMeshProUGUI showUpgradesText;
    [SerializeField] private GameObject container;

    private bool containerIsVisible;

    public event Action<UpgradeType> UpgradeClicked;

    private void Awake()
    {
        showUpgradesButton.onClick.AddListener( ToggleUpgradeVisibility);

        for (int i = 0; i < buttons.Count; i++)
        {
            var btn = buttons[i];
            if (btn == null) continue;

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
        for (int i = 0; i < buttons.Count; i++)
            if (buttons[i] != null) buttons[i].SetVisible(false);

        for (int i = 0; i < keysToShow.Count; i++)
        {
            var btn = FindButton(keysToShow[i]);
            if (btn != null) btn.SetVisible(true);
        }
    }

    public void Render(UpgradeModel model)
    {
        var btn = FindButton(model.Type);
        if (btn == null) return;
        btn.Render(model);
    }

    private void OnButtonClicked(UpgradeType key)
    {
        UpgradeClicked?.Invoke(key);
    }

    private UpgradeButtonView FindButton(UpgradeType key)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var btn = buttons[i];
            if (btn != null && btn.key.Equals(key))
                return btn;
        }
        return null;
    }
}