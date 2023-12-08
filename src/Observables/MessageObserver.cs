using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Faactory.Channels.Parcel.Observables;

internal sealed class MessageObserver : IMessageObserver
{
    private readonly ILogger logger;
    private readonly ConcurrentDictionary<string, MessageObservable> observables = new();

    public MessageObserver( ILoggerFactory loggerFactory )
    {
        logger = loggerFactory.CreateLogger<MessageObserver>();
    }

    public IMessageObservable Create( params string[] messageIds )
    {
        var observable = new MessageObservable(
            () =>
            {
                foreach ( var messageId in messageIds )
                {
                    observables.TryRemove( messageId, out _ );
                }
            }
        );

        var failAll = messageIds.Select( messageId => observables.TryAdd( messageId, observable ) )
            .ToArray()
            .All( added => !added );
            ;

        if ( failAll )
        {
            throw new InvalidOperationException( "All message identifiers are already being observed." );
        }

        logger.LogDebug(
            "Created message observable for {MessageIds}",
            string.Join( ", ", messageIds )
        );

        return observable;
    }

    public bool Push( Message message )
    {
        if ( message.Id == null )
        {
            // ignore null identifiers
            return ( false );
        }

        var observable = observables.GetValueOrDefault( message.Id );

        if ( observable == null )
        {
            // observable not found
            return ( false );
        }

        // set the observable value and trigger the pending wait
        observable.Set( message );

        logger.LogDebug(
            "Observable set with {MessageId}",
            message.Id
        );

        return ( true );
    }
}
