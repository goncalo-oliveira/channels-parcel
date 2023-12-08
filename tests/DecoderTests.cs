using Faactory.Channels;
using Faactory.Channels.Adapters;
using Faactory.Channels.Buffers;
using Faactory.Channels.Parcel;
using Microsoft.Extensions.Logging.Abstractions;

namespace tests;

public class DecoderTests
{
    [Fact]
    public async Task MultipleMessagesAsync()
    {
        var hex = "F0000000342F61636B3A592D76776A6C4A434C517A4B3174474A794C74723576743743346E5A61737556393734696C37634A546B7700000000FFF0000000050000000000FF";
        var buffer = new WrappedByteBuffer( Convert.FromHexString( hex ) );

        var context = new DetachedContext();
        IChannelAdapter adapter = new ParcelDecoderAdapter( NullLoggerFactory.Instance );

        await adapter.ExecuteAsync( context, buffer );

        var messages = Assert.IsType<Message[]>( context.Forwarded.Single() );

        Assert.True( messages.Length == 2 );
    }
}
