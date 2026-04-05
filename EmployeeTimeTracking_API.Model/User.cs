using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.Model
{
    public class User : BaseEntity
    {
        public int UserId { get; set; } // Primary Key
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } // Used for login
        public string PasswordHash { get; set; } // We'll hash the password

        // Foreign Key for Designation
        public int? DesignationId { get; set; }
        public Designation Designation { get; set; }

        // Foreign Key for Role
        public int RoleId { get; set; }
        public Role Role { get; set; }

        // Foreign Key for Department (nullable, in case user is not assigned)
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        // Navigation Properties
        public ICollection<TimeLog> TimeLogs { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }
    }
}
