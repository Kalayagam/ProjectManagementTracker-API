using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ManagerProjectTracker.DatabaseSettings;
using MemberProjectTracker;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ManagerProjectTracker.Services
{
    public class TeamMembersService : IPatchMember
    {
        private readonly IMongoCollection<TeamMembers> _teamMembers;
        public TeamMembersService(ITeamMembersDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _teamMembers = database.GetCollection<TeamMembers>(settings.TeamMembersCollectionName);

        }
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
        public List<TeamMembers> GetAsync()
        {
                return _teamMembers.FindAsync(_teamMembers => true).GetAwaiter().GetResult().ToList().OrderByDescending(x => x.YearsofExperience).ToList();
            
        }
        public async Task<TeamMembers> GetAsync(int id) {
            if (id != 0)
                return await _teamMembers.Find(x => x.mID == id).FirstOrDefaultAsync();
            else
            {
                throw new Exception("Bad Request");
            }
        }

        public List<TeamMembers> GetAllocationAsync(int allocation)
        {
            if(allocation!=0 || allocation!=100)
            {
               return _teamMembers.FindAsync(x => x.Allocation == allocation).GetAwaiter().GetResult().ToList().OrderByDescending(x => x.YearsofExperience).ToList();
            }
            else
            {
                throw new Exception("Internal Server Error");
            }
        }

        public async Task CreateAsync(TeamMembers member)
        {

                if ((member.YearsofExperience > 3 && member.SkillSet >= 3 && member.ProjectEndDate > member.ProjectStartDate))
                {
                    member._id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();
                    member.mID = RandomNumber(100000, 999999);
                    await _teamMembers.InsertOneAsync(member);
                }
            
            else
            {
                if (member.ProjectEndDate < member.ProjectStartDate)
                    throw new Exception("Project End date must be greater than Project Start Date");
                else
                    throw new Exception("Bad Request");
            }
            
        }
        [HttpPatch]
        public List<TeamMembers> UpdateAllocationPatchAsync(int allocation)
        {
            try
            {
                var currentdate = DateTime.Today; int all;
                var ProjectEndDate = DateTime.Today;
                List<TeamMembers> getAll = GetAsync();
                List<TeamMembers> IDList = new List<TeamMembers>();
                foreach (var x in getAll) IDList.Add(x);
                foreach (var x in getAll)
                {
                    ProjectEndDate = DateTime.Today;
                    for (int index = 0; index < IDList.Count; index++)
                    {
                        ProjectEndDate = _teamMembers.Find(x => x.mID == IDList[index].mID).SingleOrDefaultAsync().GetAwaiter().GetResult().ProjectEndDate;
                        if (ProjectEndDate != currentdate)
                        {
                            if (ProjectEndDate < currentdate) all = 0;
                            else all = 100;
                            if (all == 0 || all == 100)
                                _teamMembers.UpdateManyAsync<TeamMembers>(x => x.mID == IDList[index].mID, Builders<TeamMembers>.Update.Set(p => p.Allocation, all));
                        }
                    }
                }
                return GetAllocationAsync(allocation);
            }
            catch(Exception e)
            {
                throw new Exception("Bad Request");
            }
        }

    }
    }

    


