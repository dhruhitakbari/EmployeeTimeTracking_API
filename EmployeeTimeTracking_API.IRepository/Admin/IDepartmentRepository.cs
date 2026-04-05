using EmployeeTimeTracking_API.DTOs;
using EmployeeTimeTracking_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.IRepository
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> SelectAllDepartment();
        Task<bool> InsertDepartment(Department obj);
        Task<Department> SelectDepartment(int? id);
        Task<int> UpdateDepartment(Department obj);
    }
}
