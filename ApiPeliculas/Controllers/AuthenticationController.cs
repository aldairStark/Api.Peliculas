using ApiPeliculas.Auth;
using ApiPeliculas.Configuration;
using ApiPeliculas.Model.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiPeliculas.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthenticationController(UserManager<IdentityUser> userManager,
            IOptions<JwtConfig> jwtConfig)
        {
            _userManager = userManager;
            _jwtConfig = jwtConfig.Value;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
          //validamos si existe usuario
            var emailExist = await _userManager.FindByEmailAsync(request.EmailAddress);
            if (emailExist != null)
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Email already exist"
                    }
                });
            
            //usamos la clase identity
            var user = new IdentityUser()
            {
                Email = request.EmailAddress,
                UserName = request.EmailAddress
            };
            //creamos el usuario
            var isCreated = await _userManager.CreateAsync(user,request.Password);
            if (isCreated.Succeeded)
            {
                //generamos token
                var token = GenerateToken(user);

                return Ok(new AuthResult()
                {
                    Result = true,
                    Token = token
                });
            }
            else
            {
                var errors = new List<string>();
                foreach (var err in isCreated.Errors)
                    errors.Add(err.Description);


                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = errors
                });

            }
            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>()
                {
                    "User couldn´t be created"
                }
            });

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //usuario existe
            var existUser = await _userManager.FindByEmailAsync(request.Email);

            if (existUser == null) {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid Payload"
                    }
                });
            }
                
            var checkUserAndPass = await _userManager.CheckPasswordAsync(existUser, request.Password);
            if (!checkUserAndPass)
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid credentials"
                    }
                });
            }
            var token = GenerateToken(existUser);
            return Ok(new AuthResult
            {
                Token = token,
                Result = true
            });


        }
        private string GenerateToken( IdentityUser user)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new ClaimsIdentity( new[]
                    {
                        new Claim("Id",user.Id),
                        new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                        new Claim(JwtRegisteredClaimNames.Email,user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                           new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                    })),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256 )
                   
                };

                var token = jwtTokenHandler.CreateToken(tokenDescriptor);

                return jwtTokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            { 

                throw;
            }
        }
    }
}
