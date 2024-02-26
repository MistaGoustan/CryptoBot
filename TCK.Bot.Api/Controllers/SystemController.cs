using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using TCK.Bot.Options;

namespace TCK.Bot.Api.Controllers
{
    [ApiController]
    [Route("/")]
    public class SystemController : ControllerBase
    {
        private readonly IOptions<BearerAuthenticationOptions> _authenticationOptions;

        public SystemController(IOptions<BearerAuthenticationOptions> authenticationOptions)
        {
            _authenticationOptions = authenticationOptions;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string Index()
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress;

            return $"Your ip address is: {remoteIp}";
        }

        [HttpGet("/health-check")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string HealthCheck()
        {
            return "Health check successful.";
        }

        [HttpGet("/ping")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string Ping()
        {
            return "PONG";
        }

        // Azures way of checking if the container is up and running
        [HttpGet("/robots933456.txt")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string GetRobots()
        {
            return "System up and running";
        }

        [HttpPost("sign-in", Name = "SignIn")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/json")]
        public Task<IActionResult> Run(SignInRequest request) => SignInAsync(request);
        //ErrorHandlerAsync(() => SignInAsync(request));

        private async Task<IActionResult> SignInAsync(SignInRequest request)
        {
            if (request.Username == "admin" && request.Password == "TEST1234")
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                       new Claim("Id", Guid.NewGuid().ToString()),
                       new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                       new Claim(JwtRegisteredClaimNames.Email, request.Username),
                       new Claim(JwtRegisteredClaimNames.Jti,
                       Guid.NewGuid().ToString())
                    }),

                    Expires = DateTime.UtcNow.AddMinutes(_authenticationOptions.Value.ExpireTimeInMinutes),
                    Issuer = _authenticationOptions.Value.Issuer,
                    Audience = _authenticationOptions.Value.Audience,
                    SigningCredentials = new(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authenticationOptions.Value.Key)),
                    SecurityAlgorithms.HmacSha512Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return new OkObjectResult(jwtToken);
            }
            return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
        }
    }
}