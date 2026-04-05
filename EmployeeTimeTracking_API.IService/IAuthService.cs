using EmployeeTimeTracking_API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTimeTracking_API.IService
{
    public interface IAuthService
    {
        Task<ServiceResponse<RegisterUserResponseDto>> RegisterUserAsync(RegisterUserRequestDto request);
        Task<ServiceResponse<LoginResponseDto>> LoginUserAsync(LoginRequestDto request);
    }
}
