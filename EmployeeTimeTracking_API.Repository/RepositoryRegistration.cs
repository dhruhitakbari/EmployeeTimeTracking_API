using EmployeeTimeTracking_API.IRepository;
using EmployeeTimeTracking_API.Repository.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.Repository
{
    public static class RepositoryRegistration
    {
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<AppDbContext>();

            #region Admin
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            #endregion
        }
    }
}
