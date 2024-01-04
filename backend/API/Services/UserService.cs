using System.Security.Claims;
using System.Security.Cryptography;
using API.Dtos;
using API.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _configuration = configuration;
        }

        // REGISTER

        public async Task<string> RegisterAsync(UserRegisterDto registerDto)
        {
            var user = new User
            {
                Username = registerDto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Email = registerDto.Email
            };
            var userExists = await _unitOfWork.Users.GetByUsernameAsync(registerDto.Username);
            if (userExists == null)
            {
                var rolDefault = _unitOfWork.Rols
                                .Find(x => x.Name == AuthorizationHelper.rol_default.ToString())
                                .First();
                try
                {
                    user.Rols.Add(rolDefault);
                    _unitOfWork.Users.Add(user);
                    await _unitOfWork.SaveAsync();
                    return $"User {user.Username} has been successfully registered.";
                }
                catch (Exception ex)
                {
                    return $"Error: {ex.Message}";
                }
            }
            else
            {
                return $"User is already registered.";
            }
        }

        // GENERATE REFRESH TOKEN

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
        }

        // GET ROLS

        public async Task<List<string>> GetRolsAsync(UserDto userDto)
        {
            var user = await _unitOfWork.Users
                        .GetByUsernameAsync(userDto.Username);
            return user.Rols
                        .Select(x => x.Name)
                        .ToList();
        }

        // CREATE TOKEN

        public async Task<string> CreateTokenAsync(User user)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
            var userMapped = _mapper.Map<UserDto>(user);
            var rols = await GetRolsAsync(userMapped);
            foreach (var rol in rols)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // SET REFRESH TOKEN

        private CookieOptions SetRefreshToken(RefreshToken refreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            };
            user.RefreshToken = refreshToken.Token;
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshtoken", refreshToken.Token, cookieOptions);
            user.TokenExpires = refreshToken.Expires;
            user.TokenCreated = refreshToken.Created;
            return cookieOptions;
        }

        // LOGIN

        public async Task<string> LoginAsync(UserDto userDto)
        {
            var userExists = await _unitOfWork.Users
                                    .GetByUsernameAsync(userDto.Username);
            if (userExists == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, userExists.Password))
            {
                return "Check your password and username.";
            }
            var refreshToken = GenerateRefreshToken();
            string token = await CreateTokenAsync(userExists);
            userExists.RefreshToken = refreshToken.Token;
            userExists.TokenCreated = DateTime.Now;
            userExists.TokenExpires = DateTime.Now.AddHours(1);
            SetRefreshToken(refreshToken, userExists);
            _unitOfWork.Users.Update(userExists);
            await _unitOfWork.SaveAsync();
            return token;
        }

        // REFRESH TOKEN

        public async Task<string> RefreshToken(string username)
        {
            var refreshTokenCookie = _httpContextAccessor.HttpContext.Request.Cookies["refreshtoken"];
            var userExists = await _unitOfWork.Users.GetByUsernameAsync(username);
            if (userExists == null)
            {
                return "User not found.";
            }
            var refreshToken = userExists.RefreshToken;
            if (!refreshToken.Equals(refreshTokenCookie) && userExists.TokenExpires >= DateTime.Now)
            {
                return "Invalid Refresh Token.";
            }
            else if (userExists.TokenExpires < DateTime.Now)
            {
                return "Token expired.";
            }
            string token = await CreateTokenAsync(userExists);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, userExists);
            return token;
        }

        // ADD ROL

        public async Task<string> AddRolAsync(UserRolDto userRolDto)
        {
            var userExists = await _unitOfWork.Users.GetByUsernameAsync(userRolDto.Username);
            if (userExists == null)
            {
                return "User not found";
            }
            else
            {
                try
                {
                    var rol = _unitOfWork.Rols
                            .Find(x => x.Name == userRolDto.Rol)
                            .First();
                    userExists.Rols.Add(rol);
                    _unitOfWork.Users.Update(userExists);
                    await _unitOfWork.SaveAsync();
                    return $"Rol {rol.Name} was successfully added to {userExists.Username}";
                }
                catch (Exception ex)
                {
                    return $"Error: {ex.Message}";
                }
            }
        }

        // GET USERNAME TO TRY

        public string GetUsernameTry()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext is not null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return result;
        }
    }
}