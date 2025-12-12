using System.Collections.Generic;
using _Game.Features.Bosses;
using _Game.Features.Humans;
using _Game.Features.HumansState.Scripts.Combat;
using _Game.Features.HumansState.Scripts.Portal;
using _Game.Features.HumansState.Scripts.Spawn;
using _Game.Features.HumansState.Scripts.Waiting;
using UnityEngine;

namespace _Game.Features.HumansState.Scripts.Core
{
    public class HumanStateController : MonoBehaviour
    {
        [SerializeField] private GameObject _trainStateView;
        [SerializeField] private HumanView _humanPrefab;
        [SerializeField] private BossView _bossPrefab;

        private UpgradeManager _upgradeManager;
        private List<HumanState> _states;

        public void Initialize(UpgradeManager upgradeManager)
        {
            _upgradeManager = upgradeManager;
        }

        private void Start()
        {
            _states = new List<HumanState>()
            {
                new SpawnState(this, _humanPrefab),
                new PortalState(this),
                new WaitingState(this),
                new TrainingState(this, _trainStateView, _upgradeManager),
                new CombatState(this, _bossPrefab),
            };

            TransitionTo<SpawnState>();
        }

        public void TransitionTo<T>(HumanView humanView = null) where T : HumanState
        {
            for (int i = 0; i < _states.Count; i++)
            {
                if (_states[i].GetType() == typeof(T))
                    _states[i].OnEnter(humanView);
            }
        }

        public bool FreeSlotIn<T>() where T : HumanState
        {
            for (int i = 0; i < _states.Count; i++)
            {
                if (_states[i].GetType() == typeof(T))
                    return _states[i].HasFreeSlot();
            }

            return false;
        }
    }
}