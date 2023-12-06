using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem_DTOs.Request;
using TaskManagementSystem_DTOs;

namespace TaskManagementSystem_BusinessLogic.Logics.Interfaces
{
    public interface IUserService
    {
        Task<GenericResponse<string>> RegisterUserAsync(RegisterUserRequestDTO registerRequestDTO);
    }
}
