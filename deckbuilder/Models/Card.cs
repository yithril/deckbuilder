namespace deckbuilder.Models
{
    using Amazon.DynamoDBv2.DataModel;
    
    [DynamoDBTable("deckbuiler-Data-Table-test")]
    public class Card
    {
        public string Id { get; set; }
        public string DataType { get; set; }
    }
}
