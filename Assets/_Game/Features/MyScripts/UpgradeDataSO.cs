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
        Debug.Log("level: " + level + "," + "levelsCount: " + levels.Count);
        
        if (level <= 0 || level > levels.Count)
        {
            return null;
        }

        Debug.Log("returning level - 1 as " + level);
        return levels[level - 1];
    }
}