using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BurgerRoyale.Auth.API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Login")]
        [SwaggerOperation(Summary = "User login", Description = "Authenticate user and return access token.")]
        [ProducesResponseType(typeof(AuthenticationResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequestDTO request)
        {
            var response = await _accountService.Authenticate(request);
            return Ok(response);
        }

        [HttpPost("Register")]
        [SwaggerOperation(Summary = "User customer register", Description = "Register customer user")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRequestDTO request)
        {
            var response = await _accountService.RegisterCustomerAsync(request);
            return StatusCode((int) HttpStatusCode.Created, response);
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("Update/{id:Guid}")]        
        [SwaggerOperation(Summary = "Uptade customer user", Description = "Update customer user")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCustomer
        (
            [FromRoute] Guid id,
            [FromBody] CustomerUpdateRequestDTO request
        )
        {
            var response = await _accountService.UpdateCustomerAsync(id, request);
            return Ok(response);
        }
    }
}
