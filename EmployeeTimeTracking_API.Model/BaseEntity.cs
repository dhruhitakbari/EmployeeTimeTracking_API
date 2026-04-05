using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.Model
{
    public abstract class BaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public int? CreatedBy { get; set; } // Nullable int for UserId

        public DateTime? UpdatedDate { get; set; } // Nullable, won't exist on creation

        public int? UpdatedBy { get; set; } // Nullable

        public string? UpdatedReason { get; set; } // Nullable

        // This is much clearer for soft delete than "Status"
        public char? Status { get; set; } = 'A';
    }
}
