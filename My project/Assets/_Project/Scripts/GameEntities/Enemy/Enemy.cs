using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        public delegate void Destroyed(Enemy enemy);

        public event Destroyed OnKilled;
        public event Action OnTakeHit;
        public event Action OnTargetAttack;
        public bool IsMove { get; private set; } = false;
        public bool IsAttack { get; private set; } = false;

        public HealthComponent HealthComponent { get; private set; }

        private readonly int _dealingDamage = 10;
        private readonly float _attackSpeed = 0.5f;
        private readonly int _maxDefaultHealth = 30;

        private MoveComponent _moveComponent;
        private Vector2 _target;
        private Rigidbody2D _rb;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private Action<Enemy> _returnDelegate;

        private bool _isActing = false;

        public void ChangeMoveStatus(bool isMove) => IsMove = isMove;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _moveComponent = new MoveComponent(_rb, 2f);
            HealthComponent = new HealthComponent(_maxDefaultHealth);

            HealthComponent.OnHealthZero += OnHealthZeroHandler;
        }

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            HealthComponent.SetHealthOrDefault();
        }

        private void FixedUpdate()
        {
            if (!_isActing)
                return;

            if (!IsMove)
                return;

            _moveComponent.MoveTo(_target);
        }

        private void Update()
        {
            if (!_isActing)
                return;

            RotateToTarget();
        }


        private void OnDisable()
        {
            transform.position = Vector2.zero;
            transform.rotation = Quaternion.identity;
            StopActing();
            OnKilled?.Invoke(this);
            _cts.Cancel();
        }

        private void OnDestroy()
        {
            HealthComponent.OnHealthZero -= OnHealthZeroHandler;

            OnKilled?.Invoke(this);
            _cts.Cancel();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isActing)
                return;

            if (other.TryGetComponent<Stronghold>(out var stronghold))
            {
                ChangeMoveStatus(false);
                AttackAsync(stronghold).Forget();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Stronghold stronghold))
            {
                _cts.Cancel();
            }
        }

        public void Init(Vector2 position, Action<Enemy> returnDelegate)
        {
            transform.position = position;
            _returnDelegate = returnDelegate;

            ChangeMoveStatus(true);
            _isActing = true;
        }

        private void OnHealthZeroHandler()
        {
            RemoveObject();
        }

        public void RemoveObject()
        {
            transform.position = default;
            _returnDelegate?.Invoke(this);
        }

        public void SetTarget(Vector2 target)
        {
            _target = target;
        }

        private void RotateToTarget()
        {
            Vector3 targetPos = new Vector3(_target.x, _target.y, transform.position.z);
            Vector3 direction = targetPos - transform.position;
            direction.z = 0f;

            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

            float deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle);

            transform.Rotate(0f, deltaAngle * Time.deltaTime * 10, 0f);
        }

        public void StopActing()
        {
            _isActing = false;
            IsMove = false;
        }

        private async UniTask AttackAsync(IDamageable target)
        {
            IsAttack = true;
            using var registration = _cts.Token.Register(() => { IsAttack = false; });

            while (_isActing && !_cts.IsCancellationRequested)
            {
                target.TakeDamage(_dealingDamage);

                OnTargetAttack?.Invoke();
                await UniTask.WaitForSeconds(_attackSpeed, cancellationToken: _cts.Token);
            }
        }

        public void TakeDamage(int value)
        {
            HealthComponent.ChangeHealthByValue(-value);
            OnTakeHit?.Invoke();
        }
    }
}