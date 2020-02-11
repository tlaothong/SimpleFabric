using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using SimpleStateActor.Interfaces;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Flurl;
using Flurl.Http;

namespace SimpleStatelessService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class SimpleStatelessService : StatelessService, ISimpleStatelessService
    {
        public SimpleStatelessService(StatelessServiceContext context)
            : base(context)
        { }

        public async Task<string> GetWebVar(string varName)
        {
            try
            {
                var baseUrl = $"http://localhost:8686/{varName}";
                return await baseUrl.GetStringAsync();
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new FabricTransientException();
            }
            catch (FlurlHttpException ex)
            {
                if (System.Net.HttpStatusCode.ServiceUnavailable == ex.Call.HttpStatus)
                    throw new FabricTransientException();
                throw;
            }
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }
                                                                                                                                                                                                                                                                                                                                                                                                        }
}
