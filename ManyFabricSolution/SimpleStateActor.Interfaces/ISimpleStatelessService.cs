using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStateActor.Interfaces
{
    public interface ISimpleStatelessService : IService
    {
        Task<string> GetWebVar(string varName);
    }
}
