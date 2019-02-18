using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using TodoApi.Entities;
using TodoApi.Services;
using TodoApi.Helpers;
using TodoApi.Dtos;

namespace TodoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly DataContext _context;

        public UserController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            DataContext context)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        // GET: api/User/Authenticate
        /// <summary>
        /// Logs in the user.
        /// </summary>
        /// <returns>Returns the User</returns>
        /// <response code="200">Returns the list of all users</response>
        /// <response code="500">On error</response>
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        [ProducesResponseType(typeof(Users), 200)]
        [ProducesResponseType(400)]
        public ActionResult<Users> Authenticate([FromBody]UserDto userDto)
        {
            var user = _userService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            // Initializes an instance of JwtSecurityTokenHandler.
            var tokenHandler = new JwtSecurityTokenHandler();

            // Encodes a set of characters into a sequence of bytes
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            // Place holder for all the attributes related to the issued token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // The ClaimsIdentity class is a concrete implementation of a claims-based identity;
                // that is, an identity described by a collection of claims.
                // A claim is a statement about an entity made by an issuer that describes a property,
                // right, or some other quality of that entity.
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Token = tokenString
            });
        }

        // GET: api/User/Register
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <returns>Returns nothing</returns>
        /// <response code="200">Returns nothing</response>
        /// <response code="400">On creation error</response>
        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<Users>(userDto);

            try
            {
                _userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var userDtos = _mapper.Map<IList<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Update(int id, [FromBody]UserDto userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<Users>(userDto);
            user.Id = id;

            try
            {
                // save 
                _userService.Update(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/User/5
        /// <summary>
        /// Deletes a specific User.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }

        /*
        // GET: api/User/Todo/John%20Doe
        /// <summary>
        /// Returns the Todo Items where User has given name.
        /// </summary>
        /// <param name="name">The User name</param>
        /// <returns>A Todo Item list where User has matching name</returns>
        /// <response code="200">A TodoItem list where User has matching name</response>
        /// <response code="400">If passed parameter is of invalid type</response>
        [HttpGet("Todo/{name}")]
        [ProducesResponseType(typeof(TodoItems), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<TodoItems>>> GetUserTodos(string name)
        {
            var selectTodos = from user in _context.Users
                                join item in _context.TodoItems on user.Id equals item.UserId
                                where user.Username == name
                                select item;

            return await selectTodos.ToListAsync();
        }
        */
    }
}
