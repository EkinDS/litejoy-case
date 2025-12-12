using System;
using System.Collections.Generic;
using _Game.Features.PlayerWallet;
using UnityEngine;

public sealed class UpgradeManager
{
    private readonly Dictionary<UpgradeStatKey, UpgradeDataSO> _dataByKey = new();
    private readonly Dictionary<UpgradeStatKey, int> _levelByKey = new();
    private readonly List<UpgradeStatKey> _keys = new();

    public IReadOnlyList<UpgradeStatKey> Keys => _keys;

    public UpgradeManager(IReadOnlyList<UpgradeDataSO> upgradeData)
    {
        if (upgradeData == null) throw new ArgumentNullException(nameof(upgradeData));

        for (int i = 0; i < upgradeData.Count; i++)
        {
            var data = upgradeData[i];
            if (data == null) continue;

            _dataByKey[data.statKey] = data;

            if (!_levelByKey.ContainsKey(data.statKey))
                _levelByKey[data.statKey] = 0;

            if (!_keys.Contains(data.statKey))
                _keys.Add(data.statKey);
        }
        
    }

    public bool HasUpgrade(UpgradeStatKey key) => _dataByKey.ContainsKey(key);

    public int GetCurrentLevel(UpgradeStatKey key)
    {
        return _levelByKey.TryGetValue(key, out var lvl) ? lvl : 0;
    }

    public int GetMaxLevel(UpgradeStatKey key)
    {
        return _dataByKey.TryGetValue(key, out var data) ? data.MaxLevel : 0;
    }

    public float GetCurrentValue(UpgradeStatKey key)
    {
        if (!_dataByKey.TryGetValue(key, out var data))
            return 0f;

        var currentLevel = GetCurrentLevel(key);
        if (currentLevel <= 0)
            return 0f;

        var levelData = data.GetLevelData(currentLevel);
        return levelData == null ? 0f : levelData.value;
    }

    public int GetNextCost(UpgradeStatKey key)
    {
        if (!_dataByKey.TryGetValue(key, out var data))
            return -1;

        var nextLevel = GetCurrentLevel(key) + 1;
        var levelData = data.GetLevelData(nextLevel);
        return levelData == null ? -1 : levelData.cost;
    }

    public float GetNextValue(UpgradeStatKey key)
    {
        if (!_dataByKey.TryGetValue(key, out var data))
            return 0f;

        var nextLevel = GetCurrentLevel(key) + 1;
        var levelData = data.GetLevelData(nextLevel);
        return levelData == null ? 0f : levelData.value;
    }

    public bool CanUpgrade(UpgradeStatKey key)
    {
        var cost = GetNextCost(key);
        return cost >= 0 && Wallet.GetCoins() >= cost;
    }

    public bool TryUpgrade(UpgradeStatKey key)
    {
        if (!_dataByKey.TryGetValue(key, out var data))
            return false;

        var currentLevel = GetCurrentLevel(key);
        if (currentLevel >= data.MaxLevel)
            return false;

        var nextCost = GetNextCost(key);
        if (nextCost < 0 || Wallet.GetCoins() < nextCost)
            return false;

        Wallet.AddCoins(-nextCost);
        _levelByKey[key] = currentLevel + 1;
        return true;
    }
}
