using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.Model
{
    public class Department : BaseEntity
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } // e.g., "Engineering"
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
