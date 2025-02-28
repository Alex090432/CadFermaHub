using CadFermaHub.Data.DAL;
using Microsoft.AspNetCore.Components;
using System.Collections.Concurrent;

namespace CadFermaHub.Service
{
    public class EndpointsService
    {
        [Inject]
        public IDB DB { get; set; }
        public ConcurrentDictionary<Guid, List<string>> EndpointsDict { get; set; } = new();

        public EndpointsService()
        {
            var microservices = DB.GetMicroservices();
            foreach (var microservice in microservices)
            {
                EndpointsDict.TryAdd(microservice.Id,microservice.Endpoints);
            }
        }
    }
}
