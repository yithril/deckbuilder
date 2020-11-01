namespace NetCoreTemplateAPI.Models
{
    using Amazon.Lambda.APIGatewayEvents;
    using System.Collections.Generic;
    using System.Net;

    public static class APIGatewayProxyResponseBuilder
    {
        public static APIGatewayProxyResponse BuildResponse(this APIGatewayProxyResponse response, HttpStatusCode statusCode, string body)
        {
            response.Body = body;
            response.StatusCode = (int)statusCode;
            response.Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } };
            return response;
        }
    }
}
