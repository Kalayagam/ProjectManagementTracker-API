using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerProjectTracker
{
    public interface IPatchMember
    {
        List<TeamMembers> UpdateAllocationPatchAsync(int allocation);
    }
}
