using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.Model
{
    public class Designation : BaseEntity // Inherit from BaseEntity
    {
        public int DesignationId { get; set; }
        public string Name { get; set; }

        // Navigation property
        public ICollection<User> Users { get; set; }
    }
}
