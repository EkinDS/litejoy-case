public sealed class UpgradeModel
{
    public UpgradeType Type { get; }
    public int CurrentLevel { get; }
    public int NextLevel { get; }
    public float CurrentValue { get; }
    public float NextValue { get; }
    public int NextCost { get; }
    public bool CanUpgrade { get; }

    public UpgradeModel(
        UpgradeType type,
        int currentLevel,
        int nextLevel,
        float currentValue,
        float nextValue,
        int nextCost,
        bool canUpgrade)
    {
        Type = type;
        CurrentLevel = currentLevel;
        NextLevel = nextLevel;
        CurrentValue = currentValue;
        NextValue = nextValue;
        NextCost = nextCost;
        CanUpgrade = canUpgrade;
    }
}