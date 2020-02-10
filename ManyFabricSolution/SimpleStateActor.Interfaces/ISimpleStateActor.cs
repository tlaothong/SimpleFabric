using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;
using SharedModels;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace SimpleStateActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface ISimpleStateActor : IActor
    {
        Task<IEnumerable<SimpleItem>> ListItemsAsync(CancellationToken cancellationToken);

        Task<SimpleItem> GetItemAsync(string id);

        Task AddItemAsync(SimpleItem item);

        Task<string> GetWebVar(string varName);
    }
}
