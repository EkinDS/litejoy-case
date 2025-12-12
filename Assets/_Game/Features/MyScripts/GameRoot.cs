using System.Collections.Generic;
using _Game.Features.HumansState.Scripts.Core;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private List<UpgradeDataSO> upgradeData;
    [SerializeField] private HumanStateController humanStateController;
    [SerializeField] private UpgradePopupView upgradePopupView;

    private UpgradeManager _upgradeManager;
    private UpgradePresenter _upgradePresenter;

    private void Awake()
    {
        _upgradeManager = new UpgradeManager(upgradeData);

        humanStateController.Initialize(_upgradeManager);

        _upgradePresenter = new UpgradePresenter(_upgradeManager, upgradePopupView);
    }

    private void OnDestroy()
    {
        _upgradePresenter?.Dispose();
    }
}