using Azure.Core;
using EmployeeTimeTracking_API.DTOs;
using EmployeeTimeTracking_API.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTimeTracking_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> SelectAllDepartment()
        {
            var result = await _departmentService.SelectAllDepartment();

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }       
        
        [HttpPost]
        public async Task<IActionResult> InsertDepartment(CreateDepartmentDto obj)
        {
            var result = await _departmentService.InsertDepartment(obj);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> SelectDepartment(int? id)
        {
            var result = await _departmentService.SelectDepartment(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDepartment(DepartmentDto obj)
        {
            var result = await _departmentService.UpdateDepartment(obj);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
