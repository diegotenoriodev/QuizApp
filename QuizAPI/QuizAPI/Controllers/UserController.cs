using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuizAPI.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuizAPI.Controllers
{
    public class UserViewModel
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Please inform the username")]
        public string Username { get; set; }

        //[Required(ErrorMessage = "Please inform a valid email.")]
        //[RegularExpression(pattern: "", ErrorMessage = "Please inform the username!")]
        public string Email { get; set; }

        public string CurrentPassword { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class LoginCredential
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Produces("application/json")]
    [EnableCors("SiteCorsPolicy")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private UserManager<Domain.UserQuiz> userManager;
        private SignInManager<Domain.UserQuiz> signManager;
        private Model.UserModel model;

        public UserController(UserManager<Domain.UserQuiz> userManager, SignInManager<Domain.UserQuiz> signManager)
        {
            this.userManager = userManager;
            this.signManager = signManager;
            this.model = new UserModel();
        }

        private string CreateToken(string userId)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.SYMMETRIC_SECURITY_KEY));
            var signingCreadentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(signingCredentials: signingCreadentials, claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        [HttpPost]
        public IActionResult Post([FromBody]UserViewModel entity)
        {
            Domain.UserQuiz user = new Domain.UserQuiz()
            {
                Email = entity.Email,
                UserName = entity.Username
            };

            var result = this.userManager.CreateAsync(user, entity.Password).Result;

            if (!result.Succeeded)
            {
                return Ok(new ResultOperationWithObject(result.Errors.Select(r => r.Description).ToList(), entity));
            }

            this.signManager.SignInAsync(user, isPersistent: false);

            return Ok(new ResultOperationWithObject(true, new { Token = CreateToken(user.Id) }));
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody]UserViewModel entity)
        {
            try
            {
                var user = this.userManager.FindByIdAsync(HttpContext.User.Claims.FirstOrDefault().Value).Result;

                if (user != null)
                {
                    if (entity.Password != null)
                    {
                        var result = this.userManager.ChangePasswordAsync(user, entity.CurrentPassword, entity.Password).Result;

                        if (result.Succeeded)
                        {
                            return Ok(new ResultOperation(true));
                        }
                        else
                        {
                            return Ok(new ResultOperation(false) { Errors = result.Errors.Select(r => r.Description).ToList() });
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Ok(new ResultOperation(ex.Message));
            }

            return Ok(new ResultOperation("User was not found"));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredential credentials)
        {
            Domain.UserQuiz user = await userManager.FindByEmailAsync(credentials.Username);

            if (user != null && await userManager.CheckPasswordAsync(user, credentials.Password))
            {
                await this.signManager.SignInAsync(user, isPersistent: false);

                return Ok(new ResultOperationWithObject(true, new { Token = CreateToken(user.Id) }));
            }

            return Ok(new ResultOperation("Username or password invalid!"));
        }

        [HttpGet]
        [Route("quiz/{idquiz:int}")]
        [Authorize]
        public IActionResult GetUsersByQuiz(int idQuiz)
        {
            return Ok(model.GetListOfUsersForPublication(idQuiz, HttpContext.User.Claims.First().Value));
        }
    }
}