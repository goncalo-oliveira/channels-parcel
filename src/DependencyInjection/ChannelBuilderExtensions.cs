using Microsoft.Extensions.DependencyInjection;
using Faactory.Channels.Parcel.Observables;
using Faactory.Channels.Adapters;

namespace Faactory.Channels;

public static class MessageObserverServiceExtensions
{
    /// <summary>
    /// Adds a singleton service for the message observer
    /// </summary>
    public static IChannelBuilder AddMessageObserver( this IChannelBuilder builder )
    {
        builder.Services.AddSingleton<IMessageObserver, MessageObserver>();

        return ( builder );
    }
}
