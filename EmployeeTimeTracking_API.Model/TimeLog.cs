using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.Model
{
    public class TimeLog : BaseEntity
    {
        public int TimeLogId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } // Nullable if the timer is still running
        public string LogType { get; set; } // "Work", "Lunch", "Break", "DayEnd"

        // Foreign Key for User
        public int UserId { get; set; }
        public User User { get; set; }
    }   
}
