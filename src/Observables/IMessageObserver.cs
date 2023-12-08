using Faactory.Channels.Parcel;

namespace Faactory.Channels.Parcel.Observables;

/// <summary>
/// An interface to observe message identifiers
/// </summary>
public interface IMessageObserver
{
    /// <summary>
    /// Creates an observable that waits for any of the given message identifiers to be pushed
    /// </summary>
    /// <param name="messageIds">The message identifiers to wait for</param>
    /// <returns>An observable instance</returns>
    IMessageObservable Create( params string[] messageIds );

    /// <summary>
    /// Pushes a message through the observer releasing and pending locks
    /// </summary>
    /// <param name="message">The message to deliver</param>
    /// <returns>True if the message was delivered to an observable; false otherwise.</returns>
    bool Push( Message message );
}
