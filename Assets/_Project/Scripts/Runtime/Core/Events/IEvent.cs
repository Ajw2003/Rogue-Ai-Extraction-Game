namespace EventSystems
{
    // The syntax of ISomeFunctionality is reserved for interfaces, which are promises to have
    // some sort of functionality. Interfaces are not classes.
    public interface IEvent
    {
    }

    public interface IEvent<T> : IEvent
    {
        T Value { get; }
    }
}
