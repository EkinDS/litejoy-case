using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Data")]
public class UpgradeDataSO : ScriptableObject
{
    public UpgradeStatKey statKey;
    public List<UpgradeLevelData> levels;

    public int MaxLevel => levels.Count;

    public UpgradeLevelData GetLevelData(int level)
    {
        if (level <= 0 || level > levels.Count)
            return null;

        return levels[level - 1];
    }
}
