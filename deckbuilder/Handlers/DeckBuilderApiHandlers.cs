using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace deckbuilder.Handlers
{
    using Amazon.Lambda.APIGatewayEvents;
    using Autofac;
    using Microsoft.Extensions.Logging;
    using deckbuilder.Models;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using deckbuilder.DataAccess.Interfaces;

    public class DeckBuilderApiHandlers
    {
        ILogger _logger;
        IDeckBuilderRepository _deckBuilderRepository;

        public DeckBuilderApiHandlers()
        {
            var container = DependencyModule.BuildContainer();
            ResolveDependencies(container);
        }

        public DeckBuilderApiHandlers(IContainer container)
        {
            ResolveDependencies(container);
        }

        public APIGatewayProxyResponse Post(APIGatewayProxyRequest request)
        {
            var res = new Response();

            try
            {
                // do stuff here
                //var queryStringKeyNameHere = request.QueryStringParameters.ContainsKey("key");
                //var pathStringPathKeyHere = request.PathParameters.ContainsKey("id");
                var requestBody = request.Body;
                var headers = request.Headers;
                //_logger.LogDebug($"QueryStringParameters: {queryStringKeyNameHere}");
                //_logger.LogDebug($"PathParameters: {pathStringPathKeyHere}");
                _logger.LogInformation($"Body: {requestBody}");
                //_logger.LogDebug($"Headers: {headers}");

                //_logger.LogInformation("I am information being logged yay");
                //return to the api

                var deck = new Deck()
                {
                    Id = Guid.NewGuid().ToString(),
                    DataType = "Deck"
                };

                _deckBuilderRepository.Save(deck);

                res.Deck = deck;
            }
            catch (Exception ex)
            {
                _logger.LogError("I am an error being logged");
                throw new Exception("Error with hello world: ", ex);
            }

            return new APIGatewayProxyResponse().BuildResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(res));
        }

        /// <summary>
        /// DI Helpers
        /// </summary>
        /// <param name="container"></param>
        private void ResolveDependencies(IContainer container)
        {
            _logger = container.Resolve<ILogger<DeckBuilderApiHandlers>>();
            _deckBuilderRepository = container.Resolve<IDeckBuilderRepository<DeckBuilderApiHandlers>>();
        }
    }
}
