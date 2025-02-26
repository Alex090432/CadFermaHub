using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CadFermaHub.Data.EFModels
{
    public class Microservice
    {
        [Key]
        public Guid Id { get; set; } 
        public string Name { get; set; } 
        public List<HttpMethod> HttpMethods { get; set; } 
        public DataType InputType { get; set; } 
        public DataType TypeResult { get; set; } 
        public string Author { get; set; } 
        public string Params { get; set; } 
        public string Description { get; set; } 
        public string Notes { get; set; } 
    }

    // Enum for HTTP methods
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