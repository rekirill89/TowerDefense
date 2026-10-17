using _Project.Scripts;

namespace _Project.Scripts
{
    public interface IDamageable
    {
        public HealthComponent HealthComponent { get; }
        
        public void TakeDamage(int value);
    }
}