using EmployeeTimeTracking_API.IRepository;
using EmployeeTimeTracking_API.Repository.Data;
using EmployeeTimeTracking_API.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeTimeTracking_API.IService;

namespace EmployeeTimeTracking_API.Service
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            #region Admin
            services.AddScoped<IDepartmentService, DepartmentService>();
            #endregion


        }
    }
}
