using Faactory.Channels.Adapters;
using Microsoft.Extensions.Logging.Abstractions;

namespace Faactory.Channels.Parcel;

public static class MessageEncodingExtensions
{
    public static ReadOnlyMemory<byte> Encode( this Message message )
    {
        var context = new DetachedContext();
        var adapter = new ParcelEncoderAdapter( NullLoggerFactory.Instance );

        adapter.ExecuteAsync( context, message )
            .GetAwaiter()
            .GetResult();

        return context.Forwarded.Cast<byte[]>().Single();
    }

    public static Message? Decode( this ReadOnlyMemory<byte> buffer )
    {
        var context = new DetachedContext();
        var adapter = new ParcelDecoderAdapter( NullLoggerFactory.Instance );

        adapter.ExecuteAsync( context, buffer.ToArray() )
            .GetAwaiter()
            .GetResult();

        if ( !context.Forwarded.Any() )
        {
            return ( null );
        }

        var messages = context.Forwarded.Single() as Message[];

        if ( messages == null || messages.Length == 0 )
        {
            return ( null );
        }

        if ( messages.Length > 1 )
        {
            throw new InvalidOperationException( "Multiple messages were decoded from the buffer" );
        }

        return messages.Single();
    }
}
