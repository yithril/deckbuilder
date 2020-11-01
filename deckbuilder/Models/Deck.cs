namespace deckbuilder.Models
{
    using Amazon.DynamoDBv2.DataModel;
    
    [DynamoDBTable("deckbuiler-Data-Table-test")]
    public class Deck
    {
        public string Id { get; set; }
        public string DataType { get; set; }
        public string Name { get; set; }
    }
}
