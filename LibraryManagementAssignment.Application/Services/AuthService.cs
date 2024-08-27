using LibraryManagementAssignment.Application.Dto.Account;
using LibraryManagementAssignment.Application.Interfaces;
using LibraryManagementAssignment.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task<ResponseModel> SignUpAsync(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)

                return new ResponseModel { Status = "Error", Message = "User already exists!" };
            AppUser user = new AppUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return new ResponseModel
            {
                Status = "Error",
                Message = "User creation failed! Please check user details and try again."
            };
            return new ResponseModel { Status = "Success", Message = "User created succesfully!" };
        }
        public async Task<ResponseModel> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole.ToString()));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

                return new ResponseModel
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    ExpiredOn = token.ValidTo,
                    Message = "User successfully login!",
                    Roles = userRoles.ToList(),
                    Status = "Success"
                };
                //return new ResponseModel { Status = "Success", Message="User successfully login"};
            }
            return new ResponseModel { Status = "Error", Message = "Password Not valid!" };

        }
        public async Task<ResponseModel> CreateRoleAsync(string rolename)
        {
            if (!await _roleManager.RoleExistsAsync(rolename)) 
                await _roleManager.CreateAsync(new IdentityRole(rolename));
            return new ResponseModel { Status = "Success", Message ="Role Created successfully!"};
        }

        public async Task<ResponseModel> AssignToRoleAsync(string userName, string rolename)
        {
            var user = await _userManager.FindByNameAsync(userName);
  
            if (await _roleManager.RoleExistsAsync($"{rolename}"))
            {
                await _userManager.AddToRoleAsync (user, rolename);
            }
            return new ResponseModel { Status = "Success", Message = "User created succesfully!" };
        }
    }
}
