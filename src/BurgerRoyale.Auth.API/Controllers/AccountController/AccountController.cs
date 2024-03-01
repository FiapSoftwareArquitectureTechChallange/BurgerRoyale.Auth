using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BurgerRoyale.Auth.API.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
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
    }
}
