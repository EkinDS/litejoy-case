public class UpgradeModel
{
    public int CurrentLevel { get; private set; }

    public UpgradeModel()
    {
        CurrentLevel = 0;
    }

    public void IncreaseLevel()
    {
        CurrentLevel++;
    }
}