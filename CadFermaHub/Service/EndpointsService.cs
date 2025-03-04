using CadFermaHub.Data.DAL;
using Microsoft.AspNetCore.Components;
using System.Collections.Concurrent;

namespace CadFermaHub.Service
{
    public class EndpointsService
    {
        public ConcurrentDictionary<Guid, List<string>> EndpointsDict { get; set; } = new();

        
    }
}
