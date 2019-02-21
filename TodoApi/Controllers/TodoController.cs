using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TodoApi.Services;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TodoController : ControllerBase
    {
        private ITodoServices _todoServices;
        private IMemoryCache _cache;

        public TodoController(ITodoServices todoServices, IMemoryCache memoryCache)
        {
            _todoServices = todoServices;
            _cache = memoryCache;
        }

        // GET: api/Todo
        /// <summary>
        /// Returns the list of all Todo Items.
        /// </summary>
        /// <returns>Returns the list of all items</returns>
        /// <response code="200">Returns the list of all items</response>
        /// <response code="404">If there are no items</response>
        /// <response code="500">On error</response>
        [HttpGet]
        [ProducesResponseType(typeof(TodoItems[]), 200)]
        [ProducesResponseType(500)]
        public /*async Task<ActionResult<IEnumerable<TodoItems>>>*/ string GetTodoItems()
        {
            return "Test";
            /*
            var items = await _todoServices.GetTodoItems();

            if (items == null)
                return NotFound();

            return new ActionResult<IEnumerable<TodoItems>>(items);
            */           
            //return items;
        }

        // GET: api/Todo/5
        /// <summary>
        /// Returns the Todo Item with given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Todo Item with the matching id</returns>
        /// <response code="200">The TodoItem with the matching id</response>
        /// <response code="404">If not elems match the given id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoItems), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TodoItems>> GetTodoItem(int id)
        {
            var item = await _todoServices.GetTodoItem(id);

            if (item == null)
                return NotFound();
                
            return item;
        }

        // POST: api/Todo
        /// <summary>
        /// Creates a Todo Item.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created Todo Item</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(TodoItems), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TodoItems>> PostTodoItem(TodoItems item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
            }

            await _todoServices.PostTodoItem(item);

            // CreatedAtAction return HTTP 201 on successs
            // NB: standard for request that creates a ressource on the server
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        // PUT: api/Todo/5
        /// <summary>
        /// Updates the Todo Item with given id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Todo
        ///     {
        ///        "Id": 1,
        ///        "Name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The TodoItem Id</param>
        /// <param name="item"></param>
        /// <response code="204">Returns nothing</response>
        /// <response code="400">If id or item is invalid</response>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutTodoItem(int id, TodoItems item)
        {
            if (id != item.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
            }

            await _todoServices.PutTodoItem(id, item);

            return NoContent();
        }

        // DELETE: api/Todo/5
        /// <summary>
        /// Deletes a specific Todo Item.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">Returns nothing</response>
        /// <response code="404">If id invalid</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItem = await _todoServices.GetTodoItem(id);

            if (todoItem == null)
                return NotFound();

            await _todoServices.DeleteTodoItem(todoItem);

            return NoContent();
        }
    }
}