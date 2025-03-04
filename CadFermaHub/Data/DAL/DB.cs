using CadFermaHub.Data.EFModels;
using CadFermaHub.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace CadFermaHub.Data.DAL
{
    public interface IDB
    {
        List<Microservice> GetMicroservices();
        bool EditMicroservice(Microservice microservice);
        bool AddMicroservice(Microservice microservice);
        bool DeleteMicroservice(Guid microserviceGuid);
    }
    public class DB : IDB
    {

        private readonly ApplicationContext _dbContext;
        private readonly EndpointsService _endpointsService;
        public DB(ApplicationContext dbContext, EndpointsService endpointsService)
        {
            _dbContext = dbContext;
            _endpointsService = endpointsService;
        }
        public bool DeleteMicroservice(Guid microserviceGuid)
        {
            var microservice = _dbContext.Microservices.FirstOrDefault(x => x.Id == microserviceGuid);
            if (microservice != null)
            {
                _dbContext.Microservices.Remove(microservice);
                _dbContext.SaveChanges();
                _endpointsService.EndpointsDict.TryRemove(microservice.Id, out _);
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool EditMicroservice(Microservice microservice)
        {
            var existingMicroservice = _dbContext.Microservices.FirstOrDefault(x => x.Id == microservice.Id);
            if (existingMicroservice != null)
            {
                _dbContext.Microservices.Update(microservice);
                _dbContext.SaveChanges();
                _endpointsService.EndpointsDict.AddOrUpdate(
                microservice.Id,
                microservice.Endpoints,
                (key, oldValue) => microservice.Endpoints
            );
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddMicroservice(Microservice microservice)
        {
            if (!_dbContext.Microservices.Any(x => x.Id == microservice.Id))
            {
                _dbContext.Microservices.Add(microservice);
                _dbContext.SaveChanges();
                _endpointsService.EndpointsDict.TryAdd(microservice.Id, microservice.Endpoints);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Microservice> GetMicroservices()
        {
            if (_endpointsService.EndpointsDict.IsEmpty)
            {
                var microservices = _dbContext.Microservices;
                foreach (var microservice in microservices)
                {
                    _endpointsService.EndpointsDict.TryAdd(microservice.Id, microservice.Endpoints);
                }
            } 
            return _dbContext.Microservices.ToList();
        }
    }
}
