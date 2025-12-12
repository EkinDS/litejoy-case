using System;

namespace _Game.Features.Humans
{
    public sealed class HumanModel
    {
        public event Action<float, float> HealthChanged; // current, max
        public event Action Died;

        private int _health;
        private int _maxHealth;
        private int _damage;

        public int Health => _health;
        public int MaxHealth => _maxHealth;
        public int Damage => _damage;

        public bool IsDead => _health <= 0;

        public void Initialize(int baseMaxHealth)
        {
            _maxHealth = Math.Max(1, baseMaxHealth);
            _health = _maxHealth;
            HealthChanged?.Invoke(_health, _maxHealth);
        }

        public void ApplyTrainingResult(int healthGain, int damageGain)
        {
            healthGain = Math.Max(0, healthGain);
            damageGain = Math.Max(0, damageGain);

            _maxHealth += healthGain;
            _health += healthGain;
            _damage += damageGain;

            HealthChanged?.Invoke(_health, _maxHealth);
        }

        public void TakeDamage(int damage)
        {
            if (IsDead) return;

            damage = Math.Max(0, damage);
            _health -= damage;

            HealthChanged?.Invoke(_health, _maxHealth);

            if (_health <= 0)
                Died?.Invoke();
        }

        public bool CanAttackBoss(bool bossAlive)
        {
            return !IsDead && bossAlive;
        }
    }
}