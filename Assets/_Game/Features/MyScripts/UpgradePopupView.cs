using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePopupView : MonoBehaviour
{
    [Serializable]
    private class UpgradeEntryUI
    {
        public UpgradeStatKey key;
        public Button button;
        public TMP_Text levelText;
        public TMP_Text valueText;
        public TMP_Text costText;
    }

    [SerializeField] private List<UpgradeEntryUI> entries = new();

    public event Action<UpgradeStatKey> UpgradeClicked;

    private void Awake()
    {
        for (int i = 0; i < entries.Count; i++)
        {
            var capturedKey = entries[i].key;
            entries[i].button.onClick.RemoveAllListeners();
            entries[i].button.onClick.AddListener(() => UpgradeClicked?.Invoke(capturedKey));
        }
    }

    public void ShowKeys(IReadOnlyList<UpgradeStatKey> keysToShow)
    {
        for (int i = 0; i < entries.Count; i++)
            entries[i].button.gameObject.SetActive(false);

        for (int i = 0; i < keysToShow.Count; i++)
        {
            var entry = FindEntry(keysToShow[i]);
            if (entry != null)
                entry.button.gameObject.SetActive(true);
        }
    }

    public void SetEntry(UpgradeStatKey key, int level, float value, int nextCost)
    {
        var entry = FindEntry(key);
        if (entry == null) return;

        entry.levelText.text = $"Level: {level}";
        entry.valueText.text = $"Value: {value}";
        entry.costText.text = nextCost < 0 ? "MAX" : $"Cost: {nextCost}";
        entry.button.interactable = nextCost >= 0;
    }

    private UpgradeEntryUI FindEntry(UpgradeStatKey key)
    {
        for (int i = 0; i < entries.Count; i++)
            if (entries[i].key == key) return entries[i];

        return null;
    }
}