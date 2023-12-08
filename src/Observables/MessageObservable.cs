namespace Faactory.Channels.Parcel.Observables;

internal sealed class MessageObservable : IMessageObservable
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly Action cleanup;
    private Message? message;

    internal MessageObservable( Action cleanupAction )
    {
        cleanup = cleanupAction;
    }

    public void Set( Message value )
    {
        message = value;
        cancellationTokenSource.Cancel();
    }

    public async Task<Message?> WaitAsync( TimeSpan? timeout, CancellationToken cancellationToken )
    {
        timeout ??= TimeSpan.FromSeconds( 30 );

        try
        {
            await Task.Delay( timeout.Value, cancellationTokenSource.Token );
        }
        catch ( TaskCanceledException )
        {}
        finally
        {
            cleanup();
        }

        return message;
    }
}
