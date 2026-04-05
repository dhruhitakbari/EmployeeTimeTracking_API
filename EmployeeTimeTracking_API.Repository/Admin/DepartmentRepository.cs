using EmployeeTimeTracking_API.DTOs;
using EmployeeTimeTracking_API.IRepository;
using EmployeeTimeTracking_API.Model;
using EmployeeTimeTracking_API.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> SelectAllDepartment()
        {
            // Fetch all departments where Status is NOT 'D' (Deleted)
            return await _context.Departments
                .Where(d => d.Status != 'D')
                .OrderByDescending(d => d.CreatedDate)
                .ToListAsync();
        }
        public async Task<bool> InsertDepartment(Department obj)
        {
            // Insert All Departmen
            var IsExist = false;
            IsExist = await _context.Departments.AnyAsync(d => d.Name.ToLower() == obj.Name.ToLower() && d.Status != 'D');
            if (IsExist)
            {
                return false;
            }

            //if not Duplicate then Insert
            await _context.Departments.AddAsync(obj);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Department> SelectDepartment(int? id)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id && d.Status != 'D');
        }

        public async Task<int> UpdateDepartment(Department obj)
        {
            var IsExist = false;
            // check the same Department Exist or not

            IsExist = await _context.Departments.AnyAsync(d => d.Name.ToLower() == obj.Name.ToLower()
                                                && d.DepartmentId != obj.DepartmentId
                                                && d.Status != 'D');

            if(IsExist)
            {
                return 0;
            }

            // 2. DIRECT UPDATE: No fetching! We use Where() and ExecuteUpdateAsync()
            int rowsAffected = await _context.Departments.Where(d => d.DepartmentId == obj.DepartmentId && d.Status != 'D')
                               .ExecuteUpdateAsync(setters => setters
                                .SetProperty(d => d.Name, obj.Name)
                                .SetProperty(d => d.Description, obj.Description)
                                );

            if(rowsAffected == 0 )
            {
                return 0;
            }
            return 1;
        }
    }
}
