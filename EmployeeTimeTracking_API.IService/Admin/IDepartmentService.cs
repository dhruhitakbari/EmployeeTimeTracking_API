using EmployeeTimeTracking_API.DTOs;
using EmployeeTimeTracking_API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.IService
{
    public interface IDepartmentService
    {
        Task<ServiceResponse<List<DepartmentDto>>> SelectAllDepartment();
        Task<ServiceResponse<string>> InsertDepartment(CreateDepartmentDto obj);
        Task<ServiceResponse<DepartmentDto>> SelectDepartment(int? id);
        Task<ServiceResponse<string>> UpdateDepartment(DepartmentDto obj);
    }
}
