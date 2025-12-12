using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonView : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private TMP_Text costText;


    public event Action<UpgradeType> Clicked;
    public UpgradeType key;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        button.onClick.AddListener(() => Clicked?.Invoke(key));
    }

    public void Render(UpgradeViewModel viewModel)
    {
        titleText.text = viewModel.Type.ToString();

        if (viewModel.NextCost < 0)
        {
            levelText.text = $"Lv {viewModel.CurrentLevel + 1}";
            valueText.text = $"{viewModel.CurrentValue}";
            costText.text = "MAX";
            SetInteractable(false);
            return;
        }

        levelText.text = $"Lv {viewModel.CurrentLevel + 1}";
        valueText.text = $"{viewModel.CurrentValue} → {viewModel.NextValue}";
        costText.text = $"Cost: {viewModel.NextCost}";
        SetInteractable(viewModel.CanUpgrade);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    private void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }
}