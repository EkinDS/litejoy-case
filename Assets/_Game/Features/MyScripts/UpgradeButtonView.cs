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

    public void Render(UpgradeModel model)
    {
        titleText.text = model.Type.ToString();

        if (model.NextCost < 0)
        {
            levelText.text = $"Lv {model.CurrentLevel + 1}";
            valueText.text = $"{model.CurrentValue}";
            costText.text = "MAX";
            SetInteractable(false);
            return;
        }

        levelText.text = $"Lv {model.CurrentLevel + 1}";
        valueText.text = $"{model.CurrentValue} → {model.NextValue}";
        costText.text = $"Cost: {model.NextCost}";

        SetInteractable(model.CanUpgrade);
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