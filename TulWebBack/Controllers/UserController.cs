using Microsoft.AspNetCore.Authentication.OAuth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TulWebBack.Entities;
using TulWebBack.All_Views;

namespace TulWebBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(Context context) : ControllerBase
    {
        private Context Context { get; set; } = context;

        [HttpGet("fullusers")]
        public ActionResult<List<UserView>> GetUsers()
        {
            return Ok(context.Users.Select(u => new UserView() { Login = u.Login, Password = u.Password, Email=u.Email, Id = u.Id }));
        }

        [HttpGet("orders")]
        public ActionResult<List<UserView>> GetOrders([FromQuery] string Login)
        {
            return Ok(context.Users.FirstOrDefault(u => u.Login == Login).Items.Select(i => new ItemView()
            {
                Brand = i.Props.Brand,
                Description = i.Props.Description,
                Evropallet = i.Props.Evropallet,
                GroupUpakovka = i.Props.GroupUpakovka,
                Id = i.Id,
                Image = i.Image.Image,
                ShtrihCode = i.Props.ShtrihCode,
                Title = i.Props.Title,
                Upakovka = i.Props.Upakovka
            }));
        }



        [HttpPost("reg")]
        public async Task<ActionResult> Registration([FromBody] UserView inputData)
        {
            if (context.Users.Where(u => u.Login == inputData.Login).ToList().Count != 0)
                return BadRequest();

            await context.Users.AddAsync(
                    new User() { Id = Guid.NewGuid(), Login = inputData.Login, Password = inputData.Password, Email = inputData.Email }
                );
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult<Authorize> Login([FromBody] UserView inputData)
        {
            User? foundUser = context.Users.FirstOrDefault(p => p.Login == inputData.Login && p.Password == inputData.Password);

            if (foundUser is null) return Unauthorized();

            var claims = new List<Claim> { new(ClaimTypes.Name, foundUser.Login!) };


            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(60)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


            Authorize response = new() { Token = encodedJwt, Username = foundUser.Login };
            return Ok(response);
        }
    }
}
