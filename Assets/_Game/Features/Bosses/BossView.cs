using System;
using System.Collections.Generic;
using _Game.Features.Humans;
using DG.Tweening;
using UnityEngine;

namespace _Game.Features.Bosses
{
    public class BossView : MonoBehaviour
    {
        public Action<List<HumanView>> BossDefeatedCallback { get; set; }

        [SerializeField] private HealthBarView healthBarView;

        private readonly BossModel _model = new();

        public bool IsAlive() => _model.IsAlive;

        public void Initialize(double hp, float attackInterval, int targetsPerAttack, double damage)
        {
            _model.Defeated -= OnDefeated;
            _model.Defeated += OnDefeated;

            _model.HealthChanged -= OnHealthChanged;
            _model.HealthChanged += OnHealthChanged;

            _model.Initialize(hp, attackInterval, targetsPerAttack, (int)damage);
        }

        private void Update()
        {
            if (!_model.ShouldAttack(Time.time))
                return;

            _model.AttackTick(Time.time);

            transform.DOScale(1.1f, 0.1f).OnComplete(() =>
            {
                transform.DOScale(1f, 0.1f);
            });
        }

        public void TakeDamage(double damage)
        {
            _model.TakeDamage(damage);
        }

        public void RegisterAttacker(HumanView humanView)
        {
            _model.RegisterAttacker(humanView);
        }

        private void OnHealthChanged(float current, float max)
        {
            if (healthBarView != null)
                healthBarView.SetValues(current, max);
        }

        private void OnDefeated(List<HumanView> attackers)
        {
            Debug.Log("Boss Defeated!");
            BossDefeatedCallback?.Invoke(attackers);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _model.Defeated -= OnDefeated;
            _model.HealthChanged -= OnHealthChanged;
        }
    }
}