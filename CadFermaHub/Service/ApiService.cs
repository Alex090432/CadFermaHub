using CadFermaHub.Data.DAL;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using CadFermaHub.MicroserviceServer;
using ProtoMicroservice = CadFermaHub.MicroserviceServer.Microservice;
using EFMicroservice = CadFermaHub.Data.EFModels.Microservice;
using Grpc.Net.Client;
using System.Net.NetworkInformation;
using System.Threading.Channels;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf;

namespace CadFermaHub.Service
{
    public class ApiServiceWork : ApiService.ApiServiceBase
    {
        private IDB DB { get; set; }
        private EndpointsService EndpointsService { get; set; }

        public ApiServiceWork(IDB dB, EndpointsService endpointsService)
        {
            DB = dB;
            EndpointsService = endpointsService;
        }

        public async override Task<SetMicroserviceResponse> SetMicroservice(ProtoMicroservice request, ServerCallContext context)
        {
            var microservice = ToEFMicroservice(request);
            return new SetMicroserviceResponse() { Success = DB.AddMicroservice(microservice) };
        }

        public async override Task<GetMicroserviceResponse> GetMicroservices(Empty request, ServerCallContext context)
        {
            var result = new GetMicroserviceResponse();
            var microservices = DB.GetMicroservices()
                .Select(m => ToProtoMicroservice(m))
                .Where(m => m != null);
            result.Microservices.AddRange(microservices);
            return result;
        }

        public async override Task<EditMicroserviceResponse> EditMicroservice(ProtoMicroservice request, ServerCallContext context)
        {
            var microservice = ToEFMicroservice(request);
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

            foreach (var endpoint in EndpointsService.EndpointsDict[microserviceGuid])
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
                    Console.WriteLine($"Проблема с подключением {endpoint}: {rpcEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка {endpoint}: {ex.Message}");
                }
            }

            return response ?? new Response() {ResultData= new MicroserviceServer.Data() { Info = {"������ � �����������" } } };
        }

        public EFMicroservice ToEFMicroservice(ProtoMicroservice proto)
        {
            if (proto == null) return null;

            return new EFMicroservice
            {
                Id = string.IsNullOrEmpty(proto.Id) ? Guid.NewGuid() : Guid.Parse(proto.Id),
                Name = proto.Name,
                HttpMethods = proto.HttpMethods.Select(x=> (Data.EFModels.HttpMethod)x).ToList(),
                Endpoints = proto.Endpoints.ToList(),
                InputType = (Data.EFModels.DataType)proto.InputType,
                TypeResult = (Data.EFModels.DataType)proto.TypeResult,
                Author = proto.Author,
                Params = proto.Params,
                Description = proto.Description,
                Notes = proto.Notes
            };
        }

        public ProtoMicroservice ToProtoMicroservice(EFMicroservice ef)
        {
            if (ef == null) return null;

            var proto = new ProtoMicroservice
            {
                Id = ef.Id.ToString(),
                Name = ef.Name,
                Author = ef.Author,
                Params = ef.Params,
                Description = ef.Description,
                Notes = ef.Notes,
                InputType = (MicroserviceServer.DataType)ef.InputType,
                TypeResult = (MicroserviceServer.DataType)ef.TypeResult
            };
            
            proto.HttpMethods.AddRange(ef.HttpMethods.Select(x=> (MicroserviceServer.HttpMethod)x).ToList());
            proto.Endpoints.AddRange(ef.Endpoints);
            
            return proto;
        }
    }
}