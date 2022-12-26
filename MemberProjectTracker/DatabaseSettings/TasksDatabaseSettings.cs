using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberProjectTracker.DatabaseSettings
{
    public class TasksDatabaseSettings : ITasksDatabaseSettings
    {
        public string TasksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ITasksDatabaseSettings
    {
        public string TasksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
