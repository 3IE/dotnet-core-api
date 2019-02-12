﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoContext _context;

        public UserController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/User
        /// <summary>
        /// Returns the list of all Users.
        /// </summary>
        /// <returns>Returns the list of all users</returns>
        /// <response code="200">Returns the list of all users</response>
        /// <response code="500">On error</response>
        [HttpGet]
        [ProducesResponseType(typeof(TodoItem[]), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
        /// <summary>
        /// Returns the User with given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The User with the matching id</returns>
        /// <response code="200">The User with the matching id</response>
        /// <response code="404">If users match the given id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/User
        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /User
        ///     {
        ///        "name": "John"
        ///     }
        ///
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>A newly created User</returns>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the user is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.First());
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // CreatedAtAction return HTTP 201 on successs
            // NB: standard for request that creates a ressource on the server
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // GET: api/Todo/5
        /// <summary>
        /// Returns the Todo Items where User has given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Todo Item list where User has matching id</returns>
        /// <response code="200">A TodoItem list where User has matching id</response>
        /// <response code="400">If passed parameter is of invalid type</response>
        [HttpGet("Todos/{id}")]
        [ProducesResponseType(typeof(TodoItem), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetUserTodos(long id)
        {
            return await _context.TodoItems
                .Where(b => b.UserId == id)
                .ToListAsync(); 
        }
    }
}
