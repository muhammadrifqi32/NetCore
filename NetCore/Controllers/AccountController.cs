using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetCore.Models;
using NetCore.Repositories.Data;
using NetCore.ViewModels;

namespace NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public IConfiguration _configuration;
        private readonly AccountRepository _repository;

        public AccountController(
            IConfiguration config,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            AccountRepository accountRepository
            )
        {
            _configuration = config;
            _userManager = userManager;
            _signInManager = signInManager;
            this._repository = accountRepository;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User { };
                    user.Email = employeeVM.Email;
                    user.UserName = employeeVM.Name;
                    user.Id = user.Email;
                    user.PasswordHash = employeeVM.Password;

                    var result = await _userManager.CreateAsync(user, employeeVM.Password);
                    if (result.Succeeded)
                    {
                        var post = _repository.InsertUser(employeeVM);
                        if (post > 0)
                        {
                            return Ok("Register succes");
                        }
                        return BadRequest("Failed to Register");
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                //AddErrors(result);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userVM.Username, userVM.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(userVM.Username);
                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn
                            );
                        var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
                        claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
                        return Ok(idtoken + "..." + user.Email + "..." + user.Id);
                    }
                }
                return BadRequest(new { message = "Username or Password is Invalid" });
            }
            else
            {
                return BadRequest("Failed");
            }
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Log Out Success");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("GetAll")]
        public Task<IEnumerable<EmployeeVM>> GetAll()
        {
            return _repository.GetUserData();
        }
    }
}