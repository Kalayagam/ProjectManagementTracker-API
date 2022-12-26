using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ManagerProjectTracker
{
    [BsonIgnoreExtraElements]
    public class TeamMembers
    {
            [BsonRequired]
            public string Name { get; set; }
        [BsonId]
            [BsonRequired]
            [BsonRepresentation(BsonType.ObjectId)]
            public string _id { get; set; }

        [BsonRequired]
        public int mID { get; set; }

        [BsonRequired]
            public int YearsofExperience { get; set; }

            [BsonRequired]
            public string Description { get; set; }

            [BsonRequired]
            [BsonRepresentation(BsonType.DateTime)]
            public DateTime ProjectStartDate { get; set; }

            [BsonRequired]
            [BsonRepresentation(BsonType.DateTime)]
            public DateTime ProjectEndDate { get; set; }

            [BsonRequired]
            public int SkillSet { get; set; }

            [BsonRequired]
            public double Allocation { get; set; }    
    }
}
