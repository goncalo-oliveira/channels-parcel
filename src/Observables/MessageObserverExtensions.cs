using Faactory.Channels.Parcel;

namespace Faactory.Channels.Parcel.Observables;

public static class MessageObserverExtensions
{
    public static Task<Message?> WaitForAsync( this IMessageObserver observer, string messageId )
        => observer.WaitForAsync( messageId, TimeSpan.FromSeconds( 20 ) );
}
