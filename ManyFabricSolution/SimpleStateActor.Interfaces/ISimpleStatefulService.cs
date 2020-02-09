using Microsoft.ServiceFabric.Services.Remoting;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStateActor.Interfaces
{
    public interface ISimpleStatefulService : IService
    {
        Task<IEnumerable<SimpleItem>> ListItemsAsync();

        Task<SimpleItem> GetItemAsync(string id);

        Task AddItemAsync(SimpleItem item);
    }
}
