using UnityEngine;

namespace _Project.Scripts
{
    public class MoveComponent
    {
        private readonly Rigidbody2D _rb;
        private readonly float _speed;

        public MoveComponent(Rigidbody2D rb, float speed = 10)
        {
            _rb = rb;
            _speed = speed;
        }

        public void MoveTo(Vector2 target)
        {
            var dir = (target - _rb.position).normalized;
            _rb.MovePosition(_rb.position + dir * _speed * Time.fixedDeltaTime);
        }

        public void RotateTo(Vector2 target)
        {
            Vector2 direction = target - _rb.position;
            direction.Normalize();

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            _rb.MoveRotation(targetAngle);
        }

        public void MoveAndRotateTo(Vector2 target)
        {
            var dir = (target - _rb.position).normalized;
            Vector2 movement = _rb.position + dir * _speed * Time.fixedDeltaTime;

            Vector2 direction = target - _rb.position;
            direction.Normalize();

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            _rb.MovePositionAndRotation(movement, targetAngle);
        }
    }
}