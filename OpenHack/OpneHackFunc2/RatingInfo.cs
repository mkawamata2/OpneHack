using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace OpneHackFunc2
{
    [JsonObject]
    public class RatingInfo
    {
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [NonSerialized]
        public BsonObjectId _id;

        [DataMember(Name = "id")]
        [BsonElement("id")]
        public Guid Id { get; set; }

        [DataMember(Name = "userId")]
        [BsonElement("userId")]
        public Guid UserId { get; set; }

        [DataMember(Name = "productId")]
        [BsonElement("productId")]
        public Guid ProductId { get; set; }

        [DataMember(Name = "timestamp")]
        [BsonElement("timestamp")]
        public string Timestamp { get; set; }

        [DataMember(Name = "locationName")]
        [BsonElement("locationName")]
        public string LocationName { get; set; }

        [DataMember(Name = "rating")]
        [BsonElement("rating")]
        public int Rating { get; set; }

        [DataMember(Name = "userNotes")]
        [BsonElement("userNotes")]
        public string UserNotes { get; set; }
    }
}
