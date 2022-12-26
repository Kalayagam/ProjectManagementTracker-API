using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProjectTracker
{
    public class Tasks
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MemberId { get; set; }
        [BsonRequired]
        public int mID { get; set; }

        public string MemberName { get; set; }

        public string TaskName { get; set; }

        public string Deliverables { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime TaskStartDate { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime TaskEndDate { get; set; }

    }

}
