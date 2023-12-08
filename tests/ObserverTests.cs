using Faactory.Channels.Parcel;
using Faactory.Channels.Parcel.Observables;
using Microsoft.Extensions.Logging.Abstractions;

namespace tests;

public class ObserverTests
{
    [Fact]
    public async Task SingleWait()
    {
        var observer = new MessageObserver( NullLoggerFactory.Instance );

        var observable = observer.Create( "test" );  

        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        Task.Run( async () =>
        {
            await Task.Delay( 100 );

            observer.Push(
                new Message
                {
                    Id = "test"
                }
            );
        } );
        #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        var reply = await observable.WaitAsync( TimeSpan.FromSeconds( 1 ) );

        Assert.NotNull( reply );
        Assert.Equal( "test", reply.Id );
    }

    [Fact]
    public async Task SingleTimeout()
    {
        var observer = new MessageObserver( NullLoggerFactory.Instance );

        var observable = observer.Create( "test" );  

        var reply = await observable.WaitAsync( TimeSpan.FromSeconds( 1 ) );

        Assert.Null( reply );
    }

    [Fact]
    public async Task MultipleTimeout()
    {
        var observer = new MessageObserver( NullLoggerFactory.Instance );

        var observable = observer.Create( "test1", "test2" );  

        var reply = await observable.WaitAsync( TimeSpan.FromSeconds( 1 ) );

        Assert.Null( reply );
    }

    [Fact]
    public async Task MultipleWait()
    {
        var observer = new MessageObserver( NullLoggerFactory.Instance );

        var observable = observer.Create( "test1", "test2" );  

        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        Task.Run( async () =>
        {
            await Task.Delay( 100 );

            observer.Push(
                new Message
                {
                    Id = "test1"
                }
            );
        } );
        #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        var reply = await observable.WaitAsync( TimeSpan.FromSeconds( 1 ) );

        Assert.NotNull( reply );
        Assert.Equal( "test1", reply.Id );

        observable = observer.Create( "test1", "test2" );  

        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        Task.Run( async () =>
        {
            await Task.Delay( 100 );

            observer.Push(
                new Message
                {
                    Id = "test2"
                }
            );
        } );
        #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        reply = await observable.WaitAsync( TimeSpan.FromSeconds( 1 ) );

        Assert.NotNull( reply );
        Assert.Equal( "test2", reply.Id );
    }

}

