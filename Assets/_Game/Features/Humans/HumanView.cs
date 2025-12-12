using System;
using _Game.Features.Bosses;
using _Game.Features.PlayerWallet;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace _Game.Features.Humans
{
    public class HumanView : MonoBehaviour
    {
        private readonly CompositeDisposable _moveDisposables = new();

        [SerializeField] private HealthBarView healthBarView;
        [SerializeField] private int baseMaxHealth = 10;

        private HumanModel _model;
        private Tween _tween;

        public bool IsDead() => _model != null && _model.IsDead;

        public void Initialize(int count)
        {
            gameObject.name = $"Human_{count}";

            _model = new HumanModel();
            _model.Died += OnDied;
            _model.HealthChanged += OnHealthChanged;

            _model.Initialize(baseMaxHealth);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void MoveTo(Vector3 targetPosition, Action<HumanView> onReached)
        {
            var speed = 5f;
            var startPosition = transform.position;
            var distance = Vector3.Distance(startPosition, targetPosition);
            var duration = distance / speed;
            var elapsedTime = 0f;

            _moveDisposables.Clear();

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    elapsedTime += Time.deltaTime;
                    transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

                    if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                        return;

                    onReached(this);
                    _moveDisposables.Clear();
                })
                .AddTo(_moveDisposables);
        }

        public void ApplyTrainingResult(int healthGain, int damageGain)
        {
            _model.ApplyTrainingResult(healthGain, damageGain);
        }

        public void StartAttacking(BossView bossView)
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ => AttackBoss(bossView))
                .AddTo(this)
                .AddTo(bossView);
        }

        private void AttackBoss(BossView bossView)
        {
            if (_model == null) return;
            if (bossView == null) return;

            if (!_model.CanAttackBoss(bossView.IsAlive()))
                return;

            int dmg = _model.Damage;

            Wallet.AddCoins(dmg);
            bossView.TakeDamage(dmg);

            _tween =  transform.DOScale(1.1f, 0.1f).OnComplete(() =>
            {
                _tween =    transform.DOScale(1f, 0.1f);
            });
        }

        public void TakeDamage(int damage)
        {
            _model.TakeDamage(damage);
        }

        private void OnHealthChanged(int current, int max)
        {
            if (healthBarView != null)
                healthBarView.SetValues(current, max);
        }

        private void OnDied()
        {
            Debug.Log("Human Dead!");
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _tween.Kill();
            if (_model != null)
            {
                _model.Died -= OnDied;
                _model.HealthChanged -= OnHealthChanged;
            }

            _moveDisposables?.Dispose();
        }
    }
}
