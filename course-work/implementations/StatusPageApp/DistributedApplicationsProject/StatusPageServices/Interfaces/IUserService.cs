using StatusPageServices.RequestDTO.Services;
using StatusPageServices.RequestDTO.Users.StatusPageServices.RequestDTO.Users;
using StatusPageServices.ResponseDTO.Services;
using StatusPageServices.ResponseDTO.User;
using StatusPageData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageServices.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task DeleteAsync(int id);
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserEntity?> FindByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username);
        bool VerifyPassword(string inputPassword, string storedPassword);

    }
}
