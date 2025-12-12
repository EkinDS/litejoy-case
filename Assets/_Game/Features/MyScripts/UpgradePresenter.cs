using System;
using System.Collections.Generic;

public sealed class UpgradePresenter : IDisposable
{
    private readonly UpgradeManager _upgradeManager;
    private readonly UpgradePopupView _view;

    public UpgradePresenter(UpgradeManager upgradeManager, UpgradePopupView view)
    {
        _upgradeManager = upgradeManager;
        _view = view;

        _view.UpgradeClicked += OnUpgradeClicked;
        _view.ShowKeys(_upgradeManager.Keys);

        RefreshAll();
    }

    public void Dispose()
    {
        _view.UpgradeClicked -= OnUpgradeClicked;
    }

    private void OnUpgradeClicked(UpgradeType key)
    {
        if (_upgradeManager.TryUpgrade(key))
            RefreshAll();
    }

    private void RefreshAll()
    {
        IReadOnlyList<UpgradeType> keys = _upgradeManager.Keys;

        for (int i = 0; i < keys.Count; i++)
        {
            var key = keys[i];

            _view.SetEntry(
                key,
                _upgradeManager.GetCurrentLevel(key),
                _upgradeManager.GetCurrentValue(key),
                _upgradeManager.GetNextCost(key)
            );
        }
    }
}