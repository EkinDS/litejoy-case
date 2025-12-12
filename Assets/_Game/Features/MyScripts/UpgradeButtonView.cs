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
    
    public void Initialize()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => Clicked?.Invoke(key));
        }
    }

    public void Render(UpgradeModel model)
    {
        if (titleText != null)
            titleText.text = model.Type.ToString();

        if (model.NextCost < 0)
        {
            if (levelText != null) levelText.text = $"Lv {model.CurrentLevel + 1}";
            if (valueText != null) valueText.text = $"{model.CurrentValue}";
            if (costText != null) costText.text = "MAX";
            SetInteractable(false);
            return;
        }

        if (levelText != null) levelText.text = $"Lv {model.CurrentLevel + 1}";
        if (valueText != null) valueText.text = $"{model.CurrentValue} → {model.NextValue}";
        if (costText != null) costText.text = $"Cost: {model.NextCost}";

        // If you want affordability gating, use model.CanUpgrade here
        SetInteractable(model.CanUpgrade);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    private void SetInteractable(bool interactable)
    {
        if (button != null) button.interactable = interactable;
    }
}