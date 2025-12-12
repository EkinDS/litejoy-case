using System.Collections.Generic;
using _Game.Features.HumansState.Scripts.Core;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private List<UpgradeDataSO> upgradeData;
    [SerializeField] private HumanStateController humanStateController;
    [SerializeField] private UpgradeView upgradeView;

    private UpgradeModel _upgradeModel;
    private UpgradeManager _upgradeManager;
    private UpgradePresenter _upgradePresenter;

    private void Awake()
    {
        InitializeUpgradeFeature();

        humanStateController.Initialize(_upgradeManager);
    }

    private void InitializeUpgradeFeature()
    {
        _upgradeModel = new UpgradeModel(upgradeData);
        _upgradeManager = new UpgradeManager(_upgradeModel);
        _upgradePresenter = new UpgradePresenter(_upgradeManager, upgradeView);
    }

    private void OnDestroy()
    {
        _upgradePresenter?.Dispose();
    }
}