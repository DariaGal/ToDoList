using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Users
{
    public class User
    {
        [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; }

        [BsonElement("Login")]
        public string Login { get; set; }

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("RegisteredAt")]
        public DateTime RegisteredAt { get; set; }

    }
}
