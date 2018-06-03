﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website.API.Models.Register;
using Microsoft.AspNetCore.Identity;
using Website.Model;
using AutoMapper;
using Website.Dal;
using Website.API.Models.User;
using System.Linq;
using Website.API.Models.Login;
using System.Security.Claims;
using Website.API.Auth;
using Website.API.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Website.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly CoreDbContext db;
        private readonly IJwtFactory jwtFactory;

        public AuthController(UserManager<User> userManager, IMapper mapper, CoreDbContext db, IJwtFactory jwtFactory)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.db = db;
            this.jwtFactory = jwtFactory;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = this.mapper.Map<User>(model);

            var result = await this.userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            this.db.UserRoles.Add(new UserRole()
            {
                UserId = userIdentity.Id,
                RoleId = this.db.Roles.SingleOrDefault(x => x.Name == Roles.Roles.DefaultUser).Id
            });

            this.db.SaveChanges();

            var userViewModel = this.mapper.Map<UserViewModel>(userIdentity);

            return new OkObjectResult(userViewModel);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel login)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await this.GetClaimsIdentity(login.Email, login.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            var response = new AuthResponse()
            {
                id = int.Parse(identity.Claims.Single(c => c.Type == "id").Value),
                access_token = this.jwtFactory.GenerateAccessToken(login.Email, identity),
                refresh_token = this.jwtFactory.GenerateRefreshToken(login.Email)
            };

            return new OkObjectResult(response);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await this.userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await this.userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id.ToString()));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
