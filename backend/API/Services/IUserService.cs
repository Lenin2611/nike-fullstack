using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;

namespace API.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(UserRegisterDto registerDto);
        Task<string> LoginAsync(UserDto userDto);
        Task<List<string>> GetRolsAsync(UserDto userDto);
        Task<string> RefreshToken(string username);
        Task<string> AddRolAsync(UserRolDto userRolDto);
        string GetUsernameTry();
    }
}