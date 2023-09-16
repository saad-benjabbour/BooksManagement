using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using booksData.Models.DTOs;
using System.Threading.Tasks;
using System;
using booksData.Models;
using System.Collections.Generic;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using booksData;

namespace BooksManagement.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // for retrieving the data (security related) of the user
        private readonly UserManager<IdentityUser> _userManager;
        // for getting the secret that resides in the settings we should use the IConfiguration Interface
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private ILibraryCard _libraryCard;
        // Dependency Injection 
        public AuthenticationController(UserManager<IdentityUser> userManager, 
            IConfiguration configuration,
            IMapper mapper,
            ILibraryCard libraryCard)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _libraryCard = libraryCard;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] PatronRegistrationDto requestDto)
        {
            // Validate the incoming request
            if (ModelState.IsValid)
            {
                var email_existed = await _userManager.FindByEmailAsync(requestDto.Email);
                if (email_existed != null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>() { "Email already exist" }
                    });
                }
                // creating the user
                string user_name = requestDto.firstName + "" + requestDto.lastName;
                var new_user = new Patron()
                {
                    Email = requestDto.Email,
                    UserName = user_name,
                    firstName = requestDto.firstName,
                    lastName = requestDto.lastName,
                    Address = requestDto.Address,
                    dateOfBirth = requestDto.dateOfBirth,
                    PhoneNumber = requestDto.phoneNumber,
                    libraryCard = createLibraryCard()
                };
                // var libraryCard = createLibraryCard();
                // assign it to the user
                // var userLibraryCard = _libraryCard.GetById(libraryCard.Id);
                // assignLibraryCard(new_user, userLibraryCard);
                // var new_user = _mapper.Map<IdentityUser>(requestDto);
                var createdUser = await _userManager.CreateAsync(new_user, requestDto.Password);
                if(createdUser.Succeeded)
                {
                    var userToken = GenerateJwtToken(new_user);
                    return Ok(new AuthResult()
                    {
                        Result = true,
                        Token = userToken
                    }
                    );
                }
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>() { "Internal Server Error" }
                });
            }
            return BadRequest();
        }

        // when creating the user we should create its libraryCard
        private LibraryCard createLibraryCard()
        {
            var libraryCard = new LibraryCard()
            {
                Fees = 12.20M,
                Created = DateTime.Now
            };
            // Creating the library Card for the user
            _libraryCard.AddLibraryCard(libraryCard);
            return libraryCard;
        }
        // when creating the user we should create its libraryCard
        private void assignLibraryCard(Patron new_user, LibraryCard userLibraryCard)
        {
            new_user.libraryCard.Id = userLibraryCard.Id;
            new_user.libraryCard.Fees = userLibraryCard.Fees;
            new_user.libraryCard.Created = userLibraryCard.Created;
        }

        // Login
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] PatronRequestLoginDto patronLogin)
        {
            if(ModelState.IsValid)
            {
                var existed_patron = await _userManager.FindByEmailAsync(patronLogin.Email);
                if (existed_patron == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>() { "User does not exist" }
                    });
                }
                if (!await _userManager.CheckPasswordAsync(existed_patron, patronLogin.Password))
                {
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>() { "Password does not match" }
                    });
                }
                var patronToken = GenerateJwtToken(existed_patron);
                return Ok(new AuthResult()
                {
                    Result = true,
                    Token = patronToken
                }
                );
            }
            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>() { "Internal Server Error " }
            });
        }

        // generating the token 
        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
            // Token Descriptor (Claims, ExpirationTime, SigningCredentials)
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    // claims are statements about the user
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            // Creating the token
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            // we need to convert to string so we can handle it
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

        

    }
}
