namespace Faactory.Channels.Parcel.Observables;

/// <summary>
/// An interface to wait for an observable message
/// </summary>
public interface IMessageObservable
{
    /// <summary>
    /// Waits for the observable message.
    /// </summary>
    /// <param name="timeout">The time to wait for the message before timing out. Defaults to 30 seconds.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the wait operation.</param>
    /// <returns>The message that was pushed; null if the wait timed out.</returns>
    Task<Message?> WaitAsync( TimeSpan? timeout = null, CancellationToken cancellationToken = default );
}
