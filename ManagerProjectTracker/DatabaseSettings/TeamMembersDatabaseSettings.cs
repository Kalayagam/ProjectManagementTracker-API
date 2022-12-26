using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerProjectTracker.DatabaseSettings
{
    public class TeamMembersDatabaseSettings : ITeamMembersDatabaseSettings
    {
        public string TeamMembersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

    }


    public interface ITeamMembersDatabaseSettings
    {
        public string TeamMembersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
