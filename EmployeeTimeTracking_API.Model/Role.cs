using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.Model
{
    public class Role : BaseEntity
    {
        public int RoleId { get; set; }
        public string Name { get; set; } // e.g., "Admin", "Employee"

        public ICollection<User> Users { get; set; }
    }
}
