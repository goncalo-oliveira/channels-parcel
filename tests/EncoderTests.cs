using Faactory.Channels;
using Faactory.Channels.Adapters;
using Faactory.Channels.Buffers;
using Faactory.Channels.Parcel;
using Microsoft.Extensions.Logging.Abstractions;

namespace tests;

public class EncoderTests
{
    [Fact]
    public async Task SimpleTestAsync()
    {
        var message = new Message
        {
            Id = $"ack:Y-vwjlJCLQzK1tGJyLtr5vt7C4nZasuV974il7cJTkw",
        };

        var context = new DetachedContext();
        IChannelAdapter adapter = new ParcelEncoderAdapter( NullLoggerFactory.Instance );

        await adapter.ExecuteAsync( context, message );

        var buffer = Assert.IsType<byte[]>( context.Forwarded.Single() );

        context.Clear();
        adapter = new ParcelDecoderAdapter( NullLoggerFactory.Instance );

        await adapter.ExecuteAsync( context, new WrappedByteBuffer( buffer ) );

        var result = Assert.Single( Assert.IsType<Message[]>( context.Forwarded.Single() ) );

        Assert.Equal( message.Id, result.Id );
    }

}
