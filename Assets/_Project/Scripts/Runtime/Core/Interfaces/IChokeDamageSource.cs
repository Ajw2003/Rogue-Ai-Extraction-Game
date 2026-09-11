namespace Interfaces
{
    // Lets a held creature read its captor's choke-damage rate without Enemies needing a
    // compile-time reference to PlayerStateMachine (which lives in a different assembly and
    // depends back on Items).
    public interface IChokeDamageSource
    {
        float ChokeDamage { get; }
    }
}
