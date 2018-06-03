using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website.API.Models.Register;
using Microsoft.AspNetCore.Identity;
using Website.Model;
using AutoMapper;
using Website.Dal;
using Website.API.Models.User;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Website.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly CoreDbContext db;

        public AuthController(UserManager<User> userManager, IMapper mapper, CoreDbContext db)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.db = db;
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
    }
}
