using CadFermaHub.Data.EFModels;

namespace CadFermaHub.Data.DAL
{
    interface IDB {
        List<Microservice> GetMicroservices();
        bool EditMicroservice(Microservice microservice);
        bool AddMicroservice(Microservice microservice);
        bool DeleteMicroservice(Guid microserviceGuid);
    }
    public class DB : IDB
    {
        private readonly object _lock = new object();
        public bool DeleteMicroservice(Guid microserviceGuid)
        {
            lock (_lock)
            {
                using (var db = new ApplicationContext())
                {
                    var microservice = db.Microservices.FirstOrDefault(x => x.Id == microserviceGuid);
                    if (microservice != null)
                    {
                        db.Microservices.Remove(microservice);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool EditMicroservice(Microservice microservice)
        {
            lock (_lock)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var existingMicroservice = db.Microservices.FirstOrDefault(x => x.Id == microservice.Id);
                    if (existingMicroservice != null)
                    {
                        db.Microservices.Update(microservice);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        public bool AddMicroservice(Microservice microservice)
        {
            lock (_lock)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    if (!db.Microservices.Any(x => x.Id == microservice.Id))
                    {
                        db.Microservices.Add(microservice);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public List<Microservice> GetMicroservices()
        {
            lock (_lock)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    return db.Microservices.ToList();
                }
            }
        }
    }
}
