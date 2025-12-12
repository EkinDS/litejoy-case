using System;
using _Game.Features.PlayerWallet;

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

        Wallet.CoinsChanged += RefreshAll;

        RefreshAll();
    }

    public void Dispose()
    {
        _view.UpgradeClicked -= OnUpgradeClicked;
        Wallet.CoinsChanged -= RefreshAll;
    }

    private void OnUpgradeClicked(UpgradeType key)
    {
        _upgradeManager.TryUpgrade(key);
        RefreshAll();
    }

    private void RefreshAll()
    {
        var keys = _upgradeManager.Keys;

        for (int i = 0; i < keys.Count; i++)
        {
            var key = keys[i];

            int currentLevel = _upgradeManager.GetCurrentLevel(key);
            int nextCost = _upgradeManager.GetNextCost(key);

            var vm = new UpgradeViewModel(
                key,
                currentLevel,
                nextCost < 0 ? currentLevel : currentLevel + 1,
                _upgradeManager.GetCurrentValue(key),
                _upgradeManager.GetNextValue(key),
                nextCost,
                _upgradeManager.CanUpgrade(key)
            );

            _view.Render(vm);
        }
    }
}