using System.Collections.Generic;

public sealed class UpgradeManager
{
    private readonly UpgradeModel _model;

    public UpgradeManager(UpgradeModel model)
    {
        _model = model;
    }

    public IReadOnlyList<UpgradeType> Keys => _model.Keys;

    public int GetCurrentLevel(UpgradeType key) => _model.GetLevel(key);
    public float GetCurrentValue(UpgradeType key) => _model.GetCurrentValue(key);
    public int GetNextCost(UpgradeType key) => _model.GetNextCost(key);
    public float GetNextValue(UpgradeType key) => _model.GetNextValue(key);

    public bool CanUpgrade(UpgradeType key) => _model.CanUpgrade(key);
    public bool TryUpgrade(UpgradeType key) => _model.TryUpgrade(key);
}