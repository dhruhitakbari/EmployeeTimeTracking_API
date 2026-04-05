using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.DTOs
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "Department Name is required.")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
