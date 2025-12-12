using System;
using System.Collections.Generic;
using _Game.Features.Humans;
using UnityEngine;

namespace _Game.Features.Bosses
{
    public sealed class BossModel
    {
        public event Action<List<HumanView>> Defeated;
        public event Action<int, int> HealthChanged;

        private readonly List<HumanView> _attackers = new();

        private float _lastAttackTime;
        private int _currentHp;
        private int _maxHp;
        private float _attackInterval;
        private int _targetsPerAttack;
        private int _damage;

        public bool IsAlive => _currentHp > 0;

        public void Initialize(int hp, float attackInterval, int targetsPerAttack, int damage)
        {
            _maxHp = Math.Max(1, hp);
            _currentHp = _maxHp;

            _attackInterval = attackInterval;
            _targetsPerAttack = targetsPerAttack;
            _damage = damage;
            _lastAttackTime = 0F;

            HealthChanged?.Invoke(_currentHp, _maxHp);
        }

        public bool ShouldAttack(float now)
        {
            return IsAlive && _attackers.Count > 0 && (now - _lastAttackTime) >= _attackInterval;
        }

        public void AttackTick(float now)
        {
            _lastAttackTime = now;

            int count = Mathf.Min(_targetsPerAttack, _attackers.Count);
            var defeatedHumans = new List<HumanView>();

            for (int i = 0; i < count; i++)
            {
                var target = _attackers[i];

                if (target == null)
                {
                    defeatedHumans.Add(target);
                    continue;
                }

                target.TakeDamage(_damage);

                if (target.IsDead())
                {
                    defeatedHumans.Add(target);
                }
            }

            foreach (var t in defeatedHumans)
            {
                _attackers.Remove(t);
            }
        }

        public void TakeDamage(int damage)
        {
            if (!IsAlive)
            {
                return;
            }

            _currentHp = Math.Max(0, _currentHp - Math.Max(0, damage));
            HealthChanged?.Invoke(_currentHp, _maxHp);

            if (_currentHp <= 0)
            {
                Defeated?.Invoke(_attackers);
            }
        }

        public void RegisterAttacker(HumanView humanView)
        {
            if (humanView == null)
            {
                return;
            }

            if (!_attackers.Contains(humanView))
            {
                _attackers.Add(humanView);
            }
        }
    }
}