using CadFermaHub.Data.DAL;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using CadFermaHub.MicroserviceServer;
using ProtoMicroservice = CadFermaHub.MicroserviceServer.Microservice;
using EFMicroservice = CadFermaHub.Data.EFModels.Microservice;
using Grpc.Net.Client;
using System.Net.NetworkInformation;
using System.Threading.Channels;

namespace CadFermaHub.Service
{
    public class ApiServiceWork : ApiService.ApiServiceBase
    {
        [Inject] private IDB DB { get; set; }
        [Inject] private IClassConverter ClassConverter { get; set; }
        [Inject] private EndpointsService endpointsService { get; set; }
        public async override Task<SetMicroserviceResponse> SetMicroservice(ProtoMicroservice request, ServerCallContext context)
        {
            EFMicroservice microservice = new();
            ClassConverter.Convert(request, microservice);
            return new SetMicroserviceResponse() { Success = DB.AddMicroservice(microservice) };
        }


        public async override Task<GetMicroserviceResponse> GetMicroservices(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            List<ProtoMicroservice> microservices = new();
            foreach (var item in DB.GetMicroservices())
            {
                ProtoMicroservice microservice = new();
                ClassConverter.Convert(item, microservice);
                microservices.Add(microservice);
            }
            var result =new GetMicroserviceResponse();
            result.Microservices.AddRange(microservices);
            return result;
        }


        public async override Task<EditMicroserviceResponse> EditMicroservice(ProtoMicroservice request, ServerCallContext context)
        {
            EFMicroservice microservice = new();
            ClassConverter.Convert(request, microservice);
            return new EditMicroserviceResponse() { Success = DB.EditMicroservice(microservice) };
        }


        public async override Task<DeleteMicroserviceResponse> DeleteMicroservice(DeleteMicroserviceRequest request, ServerCallContext context)
        {
            return new DeleteMicroserviceResponse() { Success = DB.DeleteMicroservice(Guid.Parse(request.Id)) };
        }


        public async override Task<Response> SendRequest(Request request, ServerCallContext context)
        {
            var microserviceGuid = Guid.Parse(request.MicroserviceGuid);
            Response response = null;

            foreach (var endpoint in endpointsService.EndpointsDict[microserviceGuid])
            {
                try
                {
                    using var channel = GrpcChannel.ForAddress(endpoint);
                    var client = new MicroserviceApi.MicroserviceApiClient(channel);
                    response = await client.SendRequestAsync(request);

                    if (response != null)
                        break;
                }
                catch (RpcException rpcEx)
                {
                    Console.WriteLine($"Ошибка подключения к эндпоинту {endpoint}: {rpcEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Неизвестная ошибка при подключении к эндпоинту {endpoint}: {ex.Message}");
                }
            }

            return response ?? new Response() {ResultData= new MicroserviceServer.Data() { Info = {"Ошибка в подключении" } } };
        }
    }
}