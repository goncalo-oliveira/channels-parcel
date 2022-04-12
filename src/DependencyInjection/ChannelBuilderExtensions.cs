using Microsoft.Extensions.DependencyInjection;
using Faactory.Channels.Parcel.Observables;
using Faactory.Channels.Adapters;

namespace Faactory.Channels;

public static class ParcelChannelBuilderExtensions
{
    /// <summary>
    /// Adds a singleton service for the message observer
    /// </summary>
    public static IClientChannelBuilder AddMessageObserver( this IClientChannelBuilder builder )
    {
        builder.Services.AddSingleton<IMessageObserver, MessageObserver>();

        return ( builder );
    }
}
