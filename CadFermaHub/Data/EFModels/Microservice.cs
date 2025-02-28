using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CadFermaHub.Data.EFModels
{
    public class Microservice
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<HttpMethod> HttpMethods { get; set; } = new();
        public List<string> Endpoints { get; set; } = new();
        public DataType InputType { get; set; } 
        public DataType TypeResult { get; set; } 
        public string Author { get; set; } = string.Empty;
        public string Params { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
    public enum HttpMethod
    {
        HTTP_METHOD_UNSPECIFIED = 0,
        GET = 1,
        POST = 2,
        PUT = 3,
        DELETE = 4,
        PATCH = 5
    }

    public enum DataType {
        TYPE_RESULT_UNSPECIFIED = 0,
        FILES = 1,
        TABLE = 2,
        TEXT = 3
    }   
}