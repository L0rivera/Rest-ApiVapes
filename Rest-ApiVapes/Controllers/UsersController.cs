using Microsoft.AspNetCore.Mvc;
using Rest_ApiVapes.Models;
using Rest_ApiVapes.Data;

namespace Rest_ApiVapes.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly UserData _userData;

        public UsersController(UserData userData)
        {
            _userData = userData;
        }

        [HttpGet]
        [Route("api/users")]
        public IEnumerable<User> Get()
        {
            return _userData.GetUsers();
        }

        [HttpPost]
        [Route("api/user")]
        public IActionResult Post([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Registro Fallido");
            }

            if (user.password.Length < 9)
            {
                return BadRequest($"{user.password} has to be more than 9 characters");
            }

            //Use Bcrypt to hash the password of the user
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);

            //Here we give the method the model with the password modifed 
            _userData.addUser(user);

            return Ok("Registro exitoso");
        }

        [HttpPost]
        [Route("api/login")]
        public IActionResult Post([FromBody] Login login)
        {
            bool isAuthenticated = _userData.login(login.email, login.password);

            if (isAuthenticated)
            {
                string sessionToken = Guid.NewGuid().ToString();

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Path = "/"
                };

                HttpContext.Response.Cookies.Append("Token", sessionToken, cookieOptions);

                return Ok(new { message = "Login exitoso", token = sessionToken });
            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }

        [HttpPost]
        [Route("api/logout")]
        public IActionResult Post()
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            };

            HttpContext.Response.Cookies.Append("Token", "", cookieOptions);

            return Ok(new { message = "Logout exitoso" });
        }
    }
}
