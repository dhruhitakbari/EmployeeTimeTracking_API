using AutoMapper;
using Azure.Core;
using Azure;
using EmployeeTimeTracking_API.DTOs;
using EmployeeTimeTracking_API.IRepository;
using EmployeeTimeTracking_API.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeTimeTracking_API.Model;
using System.Data;

namespace EmployeeTimeTracking_API.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<DepartmentDto>>> SelectAllDepartment()
        {
            var response = new ServiceResponse<List<DepartmentDto>>();

            // 1. Get raw entities from Repository
            var departments = await _departmentRepository.SelectAllDepartment();

            // 2. Map Entity -> DTO using AutoMapper
            response.Data = _mapper.Map<List<DepartmentDto>>(departments);

            response.Message = "Departments fetched successfully.";
            return response;
        }

        public async Task<ServiceResponse<string>> InsertDepartment(CreateDepartmentDto obj)
        {
            var response = new ServiceResponse<string>();
            // 1. Map
            var newDeptartment = _mapper.Map<Department>(obj);
            newDeptartment.Status = 'A'; // Active

            bool isCreated = await _departmentRepository.InsertDepartment(newDeptartment);
            if (isCreated)
            {
                response.Success = true;
                response.Data = "Created";
                response.Message = "Department created successfully.";
            }
            else
            {
                response.Success = false;
                response.Message = "Department already exists.";
            }
            return response;
        }

        public async Task<ServiceResponse<DepartmentDto>> SelectDepartment(int? id)
        {
            var response = new ServiceResponse<DepartmentDto>();

            var dept = await _departmentRepository.SelectDepartment(id);

            if (dept == null)
            {
                response.Success = false;
                response.Message = "Department not Found";
                return response;
            }
            else
            {
                response.Success = true;
                //map
                response.Data = _mapper.Map<DepartmentDto>(dept);
                return response;
            }

             
        }

        public async Task<ServiceResponse<string>> UpdateDepartment(DepartmentDto obj)
        {
            var response = new ServiceResponse<string>();
            var deptToUpdate = _mapper.Map<Department>(obj);

            int result = await _departmentRepository.UpdateDepartment(deptToUpdate);

            if (result == 0)
            {
                response.Success = false;
                response.Message = "Another department with this name already exists.";
            }
            else
            {
                response.Success = true;
                response.Data = "Updated";
                response.Message = "Department updated successfully.";
            }
            return response;
        }
    }
}
