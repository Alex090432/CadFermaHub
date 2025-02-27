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
        [Inject] private IDB dB { get; set; }
        [Inject] private IClassConverter ClassConverter { get; set; }
        public async override Task<SetMicroserviceResponse> SetMicroservice(ProtoMicroservice request, ServerCallContext context)
        {
            EFMicroservice microservice = new();
            ClassConverter.Convert(request, microservice);
            return new SetMicroserviceResponse() { Success = dB.AddMicroservice(microservice) };
        }


        public async override Task<GetMicroserviceResponse> GetMicroservices(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            List<ProtoMicroservice> microservices = new();
            foreach (var item in dB.GetMicroservices())
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
            return new EditMicroserviceResponse() { Success = dB.EditMicroservice(microservice) };
        }


        public async override Task<DeleteMicroserviceResponse> DeleteMicroservice(DeleteMicroserviceRequest request, ServerCallContext context)
        {
            return new DeleteMicroserviceResponse() { Success = dB.DeleteMicroservice(Guid.Parse(request.Id)) };
        }


        public async override Task<Response> SendRequest(Request request, ServerCallContext context)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7229");

            // создаем клиент
            var client = new MicroserviceApi.MicroserviceApiClient(channel);
            Response response = new();
            // отправляем каждое слово сервису для получения перевода

            response = await client.SendRequestAsync(request);

            return response;
        }
    }
}