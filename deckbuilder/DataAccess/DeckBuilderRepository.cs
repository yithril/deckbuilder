using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using deckbuilder.DataAccess.Interfaces;
using deckbuilder.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace deckbuilder.DataAccess
{
    public class DeckBuilderRepository: IDeckBuilderRepository
    {
        private readonly IDynamoDBContext _dbContext;
        private readonly DynamoDBOperationConfig _dconfig;

        public DeckBuilderRepository(IConfiguration configuration)
        {
            _dbContext = new DynamoDBContext(new AmazonDynamoDBClient(), new DynamoDBContextConfig() { TableNamePrefix = configuration["DynamoDBTablePrefix"] });
            _dconfig = new DynamoDBOperationConfig();
        }

        public void Delete(Deck deck)
        {
            _dbContext.DeleteAsync(deck.Id).Wait();
        }

        public Deck Get(string id, string dataType)
        {
            var result = _dbContext.QueryAsync<Deck>(id, QueryOperator.Equal, new List<object>() { id, dataType });
            return result.GetRemainingAsync().Result.DefaultIfEmpty(null).First();
        }

        public IEnumerable<Deck> ListByType(string dataType)
        {
            var deckList = new List<Deck>();
            _dconfig.IndexName = "DataType_IDX";

            var results = _dbContext.QueryAsync<Deck>(dataType, QueryOperator.Equal, new List<object>() { true }, _dconfig);

            do
            {
                var set = results.GetNextSetAsync().Result;

                if (set != null && set.Any())
                {
                    deckList.AddRange(set);
                }
            } while (!results.IsDone);

            return deckList;
        }

        public void Save(Deck deck)
        {
            _dbContext.SaveAsync(deck).Wait();
        }
    }
}
