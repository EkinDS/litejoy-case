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
            _upgradeManager.GetCurrentValue(UpgradeType.LessHumanTrainingTime);

        if (trainingTime <= 0)
            trainingTime = 1f;

        Observable.Timer(TimeSpan.FromSeconds(trainingTime))
            .Subscribe(_ => FinishTraining(humanView));
    }

    private void FinishTraining(HumanView humanView)
    {
        const int baseHealthGain = 10;
        const int baseDamageGain = 10;

        int healthBonus = (int)_upgradeManager.GetCurrentValue(UpgradeType.AdditionalHumanHealth);
        int damageBonus = (int)_upgradeManager.GetCurrentValue(UpgradeType.AdditionalHumanDamage);

        humanView.ApplyTrainingResult(baseHealthGain + healthBonus, baseDamageGain + damageBonus);

        _isFree = true;
        humanStateController.TransitionTo<CombatState>(humanView);
    }

}