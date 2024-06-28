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

        [Authorize]
        [HttpPut("Update/{id:Guid}")]        
        [SwaggerOperation(Summary = "Update account", Description = "Update account")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateAccount
        (
            [FromRoute] Guid id,
            [FromBody] AccountUpdateRequestDTO request
        )
        {
            var response = await _accountService.UpdateAccountAsync(id, request);
            return Ok(response);
        }
        
        [Authorize]
        [HttpPost("Unregister")]        
        [SwaggerOperation(Summary = "Unregister account", Description = "Remove and unregister account")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UnregisterAccount()
        {
            await _accountService.UnregisterAsync(Guid.NewGuid());

            return StatusCode(StatusCodes.Status200OK);
        }
    }
}