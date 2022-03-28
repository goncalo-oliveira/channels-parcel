using Microsoft.Extensions.DependencyInjection;
using Faactory.Channels.Parcel.Observables;

namespace Faactory.Channels.Parcel;

public static class ClientChannelBuilderExtensions
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
