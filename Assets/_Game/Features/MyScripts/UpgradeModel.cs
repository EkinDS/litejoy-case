using System;
using System.Collections.Generic;
using _Game.Features.PlayerWallet;

public sealed class UpgradeModel
{
    private readonly Dictionary<UpgradeType, UpgradeDataSO> _dataByKey = new();
    private readonly Dictionary<UpgradeType, int> _levels = new();
    private readonly List<UpgradeType> _keys = new();

    public IReadOnlyList<UpgradeType> Keys => _keys;

    public UpgradeModel(IReadOnlyList<UpgradeDataSO> upgradeData)
    {
        if (upgradeData == null) throw new ArgumentNullException(nameof(upgradeData));

        for (int i = 0; i < upgradeData.Count; i++)
        {
            var data = upgradeData[i];
            if (data == null) continue;

            _dataByKey[data.type] = data;

            if (!_levels.ContainsKey(data.type))
                _levels[data.type] = 0;

            if (!_keys.Contains(data.type))
                _keys.Add(data.type);
        }
    }

    public int GetLevel(UpgradeType key) => _levels.TryGetValue(key, out var lvl) ? lvl : 0;

    public float GetCurrentValue(UpgradeType key)
    {
        if (!_dataByKey.TryGetValue(key, out var data)) return 0f;

        int lvl = GetLevel(key);
        if (lvl <= 0) return 0f;

        var ld = data.GetLevelData(lvl);
        return ld == null ? 0f : ld.value;
    }

    public int GetNextCost(UpgradeType key)
    {
        if (!_dataByKey.TryGetValue(key, out var data)) return -1;

        int nextLvl = GetLevel(key) + 1;
        var ld = data.GetLevelData(nextLvl);
        return ld == null ? -1 : ld.cost;
    }

    public float GetNextValue(UpgradeType key)
    {
        if (!_dataByKey.TryGetValue(key, out var data)) return 0f;

        int nextLvl = GetLevel(key) + 1;
        var ld = data.GetLevelData(nextLvl);
        return ld == null ? 0f : ld.value;
    }

    public bool CanUpgrade(UpgradeType key)
    {
        int cost = GetNextCost(key);
        return cost >= 0 && Wallet.GetCoins() >= cost;
    }

    public bool TryUpgrade(UpgradeType key)
    {
        if (!_dataByKey.TryGetValue(key, out var data)) return false;

        int lvl = GetLevel(key);
        if (lvl >= data.MaxLevel) return false;

        int cost = GetNextCost(key);
        if (cost < 0 || Wallet.GetCoins() < cost) return false;

        Wallet.AddCoins(-cost);
        _levels[key] = lvl + 1;
        return true;
    }
}
