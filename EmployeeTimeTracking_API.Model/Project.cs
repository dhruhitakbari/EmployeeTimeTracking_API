using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.Model
{
    public class Project : BaseEntity
    {
        public int ProjectId { get; set; }
        public string Name { get; set; } // e.g., "Time Tracker App"

        public ICollection<UserProject> UserProjects { get; set; }
    }
}
