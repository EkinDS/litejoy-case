using System;
using System.Collections.Generic;

public sealed class UpgradePresenter : IDisposable
{
    private readonly UpgradeManager _upgradeManager;
    private readonly UpgradeView _view;

    public UpgradePresenter(UpgradeManager upgradeManager, UpgradeView view)
    {
        _upgradeManager = upgradeManager;
        _view = view;

        _view.UpgradeClicked += OnUpgradeClicked;
        _view.Setup(_upgradeManager.Keys);

        RefreshAll();
    }

    public void Dispose()
    {
        _view.UpgradeClicked -= OnUpgradeClicked;
    }

    private void OnUpgradeClicked(UpgradeType type)
    {
        if (_upgradeManager.TryUpgrade(type))
            RefreshAll();
        else
            RefreshOne(type); // still update button interactable/cost if needed
    }

    private void RefreshAll()
    {
        IReadOnlyList<UpgradeType> keys = _upgradeManager.Keys;
        for (int i = 0; i < keys.Count; i++)
            RefreshOne(keys[i]);
    }

    private void RefreshOne(UpgradeType type)
    {
        var currentLevel = _upgradeManager.GetCurrentLevel(type);
        var nextCost = _upgradeManager.GetNextCost(type);
        var currentValue = _upgradeManager.GetCurrentValue(type);
        var nextValue = _upgradeManager.GetNextValue(type);

        var model = new UpgradeModel(
            type,
            currentLevel,
            nextCost < 0 ? currentLevel : currentLevel + 1,
            currentValue,
            nextValue,
            nextCost,
            _upgradeManager.CanUpgrade(type)
        );

        _view.Render(model);

    }
}