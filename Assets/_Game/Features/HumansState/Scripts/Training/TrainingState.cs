using System;
using _Game.Features.Humans;
using _Game.Features.HumansState.Scripts.Combat;
using _Game.Features.HumansState.Scripts.Core;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

public class TrainingState : HumanState
{
    private readonly UpgradeManager _upgradeManager;
    private readonly Vector3 _startingPosition = new(0, -2.72f, 0);

    private bool _isFree = true;
    public override bool HasFreeSlot() => _isFree;

    public TrainingState(
        HumanStateController humanStateController,
        GameObject trainSlotPrefab,
        UpgradeManager upgradeManager
    ) : base(humanStateController)
    {
        _upgradeManager = upgradeManager;
        GameObject.Instantiate(trainSlotPrefab, _startingPosition, quaternion.identity);
    }

    protected override void Enter(HumanView humanView)
    {
        _isFree = false;
        humanView.MoveTo(_startingPosition, _ => StartTraining(humanView));
    }

    private void StartTraining(HumanView humanView)
    {
        float trainingTime =
            _upgradeManager.GetCurrentValue(UpgradeStatKey.TrainingTime);

        if (trainingTime <= 0)
            trainingTime = 1f;

        Observable.Timer(TimeSpan.FromSeconds(trainingTime))
            .Subscribe(_ => FinishTraining(humanView));
    }

    private void FinishTraining(HumanView humanView)
    {
        int healthGain = (int)_upgradeManager.GetCurrentValue(UpgradeStatKey.HumanHealth);
        int damageGain = (int)_upgradeManager.GetCurrentValue(UpgradeStatKey.HumanDamage);

        Debug.Log(healthGain + "," + damageGain);
        humanView.ApplyTrainingResult(healthGain, damageGain);

        _isFree = true;
        humanStateController.TransitionTo<CombatState>(humanView);
    }

}