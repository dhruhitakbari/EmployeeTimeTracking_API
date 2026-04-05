using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.DTOs
{
    public class RegisterUserRequestDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress] // Adds email format validation
        public string Email { get; set; }

        [Required]
        [MinLength(6)] // Example of a password rule
        public string Password { get; set; }

        public int? DesignationId { get; set; }
    }
}