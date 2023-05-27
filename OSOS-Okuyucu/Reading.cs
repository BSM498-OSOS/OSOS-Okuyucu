
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OSOS_Okuyucu
{
    [BsonIgnoreExtraElements]
    public class Reading
    {
        [BsonElement("Obis000")]
        public int Obis000 { get; set; }
        [BsonElement("Obis092")]
        public DateTime Obis092 { get; set; }
        [BsonElement("Obis180")]
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public decimal Obis180 { get; set; }
    }
}