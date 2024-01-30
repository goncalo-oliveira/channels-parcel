# Channels - Parcel Protocol

Parcel Protocol implementation for the Channels library.

Learn more about [Parcel Protocol](https://github.com/goncalo-oliveira/parcel-spec).

Learn more about [Channels](https://github.com/goncalo-oliveira/channels);

## Getting Started

Install the package from NuGet

```bash
dotnet add package Faactory.Channels.Parcel
```

To enable decoding or encoding of Parcel Messages on the pipeline, we just need to register the respective adapters with the channel pipeline. It is the same for server or client channels.

```csharp
IChannelBuilder channel = ...;

// This adapter will decode from a byte[] and forward a Parcel.Message[]
channel.AddInputAdapter<ParcelDecoderAdapter>();

// User handler implementation to perform business logic
channel.AddInputHandler<MyMessageHandler>();

// This adapter will encode a Parcel.Message or a Parcel.Message[] into a byte[]
channel.AddOutputAdapter<ParcelEncoderAdapter>();
```

## Observables

It is possible to write to the output channel and then wait for a specific response on the input channel. This is useful if we know that the server will reply with a Parcel Message with a specific identifier.

To make this work, we first need to register the `IMessageObserver` service; this will allow us to retrieve the instance through dependency injection.

```csharp
IChannelBuilder channel = ...;

channel.AddMessageObserver();
```

With the message observer registered, we can now create an observable instance for a particular identifier. Here's an example

```csharp
IChannel channel = ...;
IMessageObserver observer = ...;

/*
Sending the message below to the channel will result
in a reply with the identifier `my-reply`. Therefore,
we create an observable for that identifier.
*/
var observable = observer.CreateObservable( "my-reply" );

// send the message to the channel
await channel.WriteAsync( new Message
{
    // ...
} );

// tell the observable to wait for the reply
var replyMessage = await observable.WaitAsync();

if ( replyMessage == null )
{
    // the response is null if the timeout triggers
}
```

> **Note:** The observable instance is for single use. Once the observable value is set, it is removed from the observer.

This isn't enough though... Because the handling of the messages is done by our pipeline and by our handlers, we need to tell the observer when we receive messages. For this example, we'll do it on our `MyMessageHandler` handler.

```csharp
public class MyMessageHandler : ChannelHandler<Message>
{
    private readonly IMessageObserver observer;

    public MyMessageHandler( IMessageObserver messageObserver )
    {
        // our observer is injected
        observer = messageObserver;
    }

    public override Task ExecuteAsync( IChannelContext context, Message message )
    {
        // ...

        // let the observer know we received a message
        observer.Push( message );

        // ...
    }
}
```
