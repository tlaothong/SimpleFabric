using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using SimpleStateActor.Interfaces;
using SharedModels;

namespace SimpleStatefulService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class SimpleStatefulService : StatefulService, ISimpleStatefulService
    {
        public SimpleStatefulService(StatefulServiceContext context)
            : base(context)
        { }

        public async Task AddItemAsync(SimpleItem item)
        {
            var sm = this.StateManager;
            var items = await sm.GetOrAddAsync<IReliableDictionary<string, List<SimpleItem>>>("items");
            using (var tx = sm.CreateTransaction())
            {
                var listItems = await items.TryGetValueAsync(tx, "items");
                var list = listItems.HasValue ? listItems.Value : new List<SimpleItem>();
                list.Add(item);
                await items.AddOrUpdateAsync(tx, "items", _ => list, (k, _) => list);
                await tx.CommitAsync();
            }
        }

        public async Task<SimpleItem> GetItemAsync(string id)
        {
            var sm = this.StateManager;
            var items = await sm.GetOrAddAsync<IReliableDictionary<string, List<SimpleItem>>>("items");
            List<SimpleItem> list;
            using (var tx = sm.CreateTransaction())
            {
                var listItems = await items.TryGetValueAsync(tx, "items");
                list = listItems.HasValue ? listItems.Value : new List<SimpleItem>();
                await tx.CommitAsync();
            }

            return list.Find(it => it.Id == id);
        }

        public async Task<IEnumerable<SimpleItem>> ListItemsAsync()
        {
            var sm = this.StateManager;
            var items = await sm.GetOrAddAsync<IReliableDictionary<string, List<SimpleItem>>>("items");
            List<SimpleItem> list;
            using (var tx = sm.CreateTransaction())
            {
                var listItems = await items.TryGetValueAsync(tx, "items");
                list = listItems.HasValue ? listItems.Value : new List<SimpleItem>();
                await tx.CommitAsync();
            }

            return list;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
