using Microsoft.Extensions.Logging;
using Faactory.Channels.Buffers;
using Faactory.Channels.Parcel;

namespace Faactory.Channels.Adapters;

/// <summary>
/// Encodes IEnumerable<Message>, forwards a byte array
/// </summary>
public sealed class ParcelEncoderAdapter : ChannelAdapter<IEnumerable<Message>>
{
    private readonly ILogger logger;

    public ParcelEncoderAdapter( ILoggerFactory loggerFactory )
    {
        logger = loggerFactory.CreateLogger<ParcelEncoderAdapter>();
    }

    public override Task ExecuteAsync( IAdapterContext context, IEnumerable<Message> data )
    {
        // compute messages length
        var length = data.Sum( x => x.Length );

        var buffer = new WritableByteBuffer( length + 6 );

        // write Head and Length
        buffer.WriteByte( Constants.Head );
        buffer.WriteUInt32( (uint)length );

        // write Messages
        foreach ( var message in data )
        {
            buffer.WriteMessage( message );
        }

        // write Tail
        buffer.WriteByte( Constants.Tail );

        // forward buffer
        context.Forward( buffer.ToArray() );

        logger.LogDebug( $"Forwarded {buffer.Length} bytes." );

        return  Task.CompletedTask;
    }
}
