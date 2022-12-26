using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberProjectTracker.DatabaseSettings;
using ManagerProjectTracker.DatabaseSettings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ManagerProjectTracker;
using System.Net.Http;
using System.Net;
using MongoDB.Bson;

namespace MemberProjectTracker.Services
{
    public class TaskService
    {
        private readonly IMongoCollection<TeamMembers> _teamMembers;
        private readonly IMongoCollection<Tasks> _tasks;
        public TaskService(ITasksDatabaseSettings tasksettings, ITeamMembersDatabaseSettings teamsettings)
        {
            var taskclient = new MongoClient(tasksettings.ConnectionString);
            var Taskdatabase = taskclient.GetDatabase(tasksettings.DatabaseName);
            _tasks = Taskdatabase.GetCollection<Tasks>(tasksettings.TasksCollectionName);

            var teamclient = new MongoClient(teamsettings.ConnectionString);
            var Teamdatabase = teamclient.GetDatabase(teamsettings.DatabaseName);
            _teamMembers = Teamdatabase.GetCollection<TeamMembers>(teamsettings.TeamMembersCollectionName);
        }

        public List<Tasks> GetAsync()
        {
            try
            {
                return _tasks.FindAsync(_ => true).GetAwaiter().GetResult().ToList();
            }
            catch(Exception e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                var errormessage = e.Message;
                response.Content = new StringContent(errormessage);
                throw new HttpResponseException(response.Content);
            }
        }

        public List<Tasks> GetIdAsync(int id)
        {
            if(id!=0)
            {
                return _tasks.FindAsync(x => x.mID == id).GetAwaiter().GetResult().ToList();
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                var errormessage = "Bad Request";
                response.Content = new StringContent(errormessage);
                throw new HttpResponseException(response.Content);
            }
        }
        public List<object> GetAsync(int id)
        {
            List<object> memberTaskList = new List<object>();
            TaskandTeamMemberscs tt = new TaskandTeamMemberscs();
            bool idflag = false;
                if (id > 0)
                {
                    tt.mID = _tasks.FindAsync(x => x.mID == id).GetAwaiter().GetResult().SingleOrDefault().mID;
                    idflag = true;
                }
                if (idflag == true)
                {

                    tt.MemberId = _tasks.FindAsync(x => x.mID == id).GetAwaiter().GetResult().SingleOrDefault().MemberId;
                    tt.MemberName = _tasks.FindAsync(x => x.mID == id).GetAwaiter().GetResult().SingleOrDefault().MemberName;
                    tt.TaskName = _tasks.FindAsync(x => x.mID == id).GetAwaiter().GetResult().SingleOrDefault().TaskName;
                    tt.TaskStartDate = _tasks.FindAsync(x => x.mID == id).GetAwaiter().GetResult().SingleOrDefault().TaskStartDate;
                    tt.TaskEndDate = _tasks.FindAsync(x => x.mID == id).GetAwaiter().GetResult().SingleOrDefault().TaskEndDate;
                    tt.Deliverables = _tasks.FindAsync(x => x.mID == id).GetAwaiter().GetResult().SingleOrDefault().Deliverables;
                    tt.ProjectStartDate = _teamMembers.FindAsync(x => x.mID == id).GetAwaiter().GetResult().SingleOrDefault().ProjectStartDate;
                    tt.ProjectEndDate = _teamMembers.FindAsync(x => x.mID == id).GetAwaiter().GetResult().SingleOrDefault().ProjectEndDate;
                    tt.Allocation = _teamMembers.FindAsync(x => x.mID == id).GetAwaiter().GetResult().SingleOrDefault().Allocation;
                    memberTaskList.Add(tt);
                    return memberTaskList;
                }

            else            
            {   
                throw new Exception("Member ID is not assigned with any tasks");
            }
        }


        public async Task CreateAsyncTasks(Tasks task)
        {
            if (task.mID != 0)
            {
                var MemberID = _teamMembers.Find(x => x.mID == task.mID).FirstOrDefaultAsync().GetAwaiter().GetResult()._id;
                var mID = _teamMembers.Find(x => x.mID == task.mID).FirstOrDefaultAsync().GetAwaiter().GetResult().mID;
                var MemberName = _teamMembers.Find(x => x.mID == task.mID).SingleOrDefaultAsync().GetAwaiter().GetResult().Name;
                var ProjectStartDate = _teamMembers.Find(x => x.mID == task.mID).SingleOrDefaultAsync().GetAwaiter().GetResult().ProjectStartDate;
                var ProjectEndDate = _teamMembers.Find(x => x.mID == task.mID).SingleOrDefaultAsync().GetAwaiter().GetResult().ProjectEndDate;
                if (task.TaskEndDate <= ProjectEndDate && task.TaskStartDate >=ProjectStartDate)
                {
                    task.MemberId = MemberID;
                    task.mID = mID; task.MemberName = MemberName;
                    await _tasks.InsertOneAsync(task);
                }
                else
                {
                   if(task.TaskStartDate < ProjectStartDate)
                    throw new Exception("Task Start Date is less than Project End Date");
                   else if (task.TaskEndDate > ProjectEndDate)
                    throw new Exception("Task End Date is higher than Project End Date");
                   else 
                    throw new Exception("Bad Request");
                }
            }
            else
            {
                throw new Exception("Bad Request");
            }
        }
    }

}
