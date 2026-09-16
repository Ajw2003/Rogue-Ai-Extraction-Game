namespace Interfaces
{
    public interface IHealth
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }

        void TakeDamage(float damage);
        void TakeDamage(float damage, float impactVelocity);
    }
}
