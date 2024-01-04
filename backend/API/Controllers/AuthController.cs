using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Data;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly NikeContext _context;

        public AuthController(IUnitOfWork unitOfWork, IUserService userService, IConfiguration configuration, NikeContext context)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> RegisterUser(UserRegisterDto userRegisterDto)
        {
            var result = await _userService.RegisterAsync(userRegisterDto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto userDto)
        {
            var result = await _userService.LoginAsync(userDto);
            return Ok(result);
        }

        [HttpPost("addrol")]
        public async Task<ActionResult<string>> AddRol(UserRolDto userRolDto)
        {
            var result = await _userService.AddRolAsync(userRolDto);
            return Ok(result);
        }

        [HttpPost("userrols")]
        public async Task<ActionResult<List<string>>> GetUserRols(UserDto userDto)
        {
            var result = await _userService.GetRolsAsync(userDto);
            return Ok(result);
        }

        [HttpGet("users")]
        public ActionResult GetUsers()
        {
            var query = (from user in _context.Users select new
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            }).ToList();
            return Ok(query);
        }

        [HttpGet("username"), Authorize(Roles = "Person")]
        public ActionResult<string> GetUsername()
        {
            return Ok(_userService.GetUsernameTry());
        }
    }
}