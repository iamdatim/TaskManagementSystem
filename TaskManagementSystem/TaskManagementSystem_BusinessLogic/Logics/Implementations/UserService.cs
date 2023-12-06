using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem_BusinessLogic.Logics.Interfaces;
using TaskManagementSystem_BusinessLogic.Utilities;
using TaskManagementSystem_DataSource.Entities;
using TaskManagementSystem_DTOs;
using TaskManagementSystem_DTOs.Request;

namespace TaskManagementSystem_BusinessLogic.Logics.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<GenericResponse<string>> RegisterUserAsync(RegisterUserRequestDTO registerRequestDTO)
        {
            var userExist = await _userManager.FindByEmailAsync(registerRequestDTO.Email);
            if (userExist == null)
            {
                ApplicationUser user = new()
                {
                    FirstName = registerRequestDTO.FirstName,
                    LastName = registerRequestDTO.LastName,
                    MiddleName = registerRequestDTO.MiddleName,
                    Email = registerRequestDTO.Email,
                    UserName = registerRequestDTO.Email.Split('@')[0],
                    EmailConfirmed = true
                };

                IdentityResult result = await _userManager.CreateAsync(user, registerRequestDTO.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(Constants.Roles.User))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Constants.Roles.User));
                    }

                    if (await _roleManager.RoleExistsAsync(Constants.Roles.User))
                    {
                        await _userManager.AddToRoleAsync(user, Constants.Roles.User);
                    }
                    return GenericResponse<string>.SuccessResponse("Successful", "User Account Registered Successfully");
                }
                return GenericResponse<string>.ErrorResponse("Account creating failed, please try again");
            }
            return GenericResponse<string>.ErrorResponse("Email Already Exist");
        }

    }
}
