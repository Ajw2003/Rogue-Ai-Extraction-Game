namespace Interfaces
{
    // Lets Item resolve a picked-up creature's own pickup behavior without Items needing a
    // compile-time reference to MonsterStateMachine (which lives in Enemies, downstream of Items).
    public interface ICarryableCreature
    {
        void PickUp();
    }
}
