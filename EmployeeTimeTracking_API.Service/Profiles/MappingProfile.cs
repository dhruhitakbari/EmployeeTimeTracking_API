

using AutoMapper;
using EmployeeTimeTracking_API.DTOs;
using EmployeeTimeTracking_API.Model;

namespace EmployeeTimeTracking_API.Service.Profiles
{
    public class MappingProfile : Profile 
    {
        public MappingProfile() {

            // 1. Map from Database Entity -> Read DTO (Get)
            CreateMap<Department, DepartmentDto>();

            // 2. Map from Create DTO -> Database Entity (Add/Update)
            CreateMap<CreateDepartmentDto, Department>();
        }
    }
}
