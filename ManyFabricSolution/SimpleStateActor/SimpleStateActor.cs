using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using SimpleStateActor.Interfaces;
using SharedModels;

namespace SimpleStateActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class SimpleStateActor : Actor, ISimpleStateActor
    {
        /// <summary>
        /// Initializes a new instance of SimpleStateActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public SimpleStateActor(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId)
        {
        }

        public async Task AddItemAsync(SimpleItem item)
        {
            var sm = this.StateManager;

            var items = await sm.TryGetStateAsync<List<SimpleItem>>("items");
            var listOfItems = items.HasValue ? items.Value : new List<SimpleItem>();
            listOfItems.Add(item);
            await sm.AddOrUpdateStateAsync("items", listOfItems, (stName, _) => listOfItems);
        }

        public async Task<SimpleItem> GetItemAsync(string id)
        {
            var sm = this.StateManager;
            var items = await sm.TryGetStateAsync<List<SimpleItem>>("items");
            if (items.HasValue)
            {
                return items.Value.Find(it => it.Id == id);
            }
            return null;
        }

        public async Task<IEnumerable<SimpleItem>> ListItemsAsync(CancellationToken cancellationToken)
        {
            var sm = this.StateManager;
            var items = await sm.TryGetStateAsync<List<SimpleItem>>("items");
            return items.HasValue ? items.Value : new List<SimpleItem>();
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization

            return this.StateManager.TryAddStateAsync("count", 0);
        }
    }
}
