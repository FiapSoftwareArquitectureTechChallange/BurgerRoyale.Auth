using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Enumerators;
using BurgerRoyale.Auth.Domain.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BurgerRoyale.Auth.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get a list of users", Description = "Retrieves a list of users based on the specified type.")]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUsers([FromQuery] UserRole? userType)
        {
            var users = await _userService.GetUsersAsync(userType);
            return Ok(users);
        }

        [HttpGet("{id:Guid}")]
        [SwaggerOperation(Summary = "Get an user by ID", Description = "Retrieves an user by its ID.")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add a new user", Description = "Creates a new user.")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateUser([FromBody] RequestUserDTO user)
        {
            var createdUser = await _userService.CreateAsync(user);

            return StatusCode((int)HttpStatusCode.Created, createdUser);
        }

        [HttpPut("{id:Guid}")]
        [SwaggerOperation(Summary = "Update an user", Description = "Updates an existing user by its ID.")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateUser(
            [FromRoute] Guid id,
            [FromBody] RequestUserDTO user
        )
        {
            var updatedUser = await _userService.UpdateAsync(id, user);

            return Ok(updatedUser);
        }

        [HttpDelete("{id:Guid}")]
        [SwaggerOperation(Summary = "Delete an user by ID", Description = "Deletes an user by its ID.")]
        [ProducesResponseType(typeof(HttpStatusCode), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            await _userService.DeleteAsync(id);

            return NoContent();
        }
    }
}
