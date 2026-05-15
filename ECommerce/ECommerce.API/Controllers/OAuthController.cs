using ECommerce.Application.OAuth.Records;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OAuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public OAuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ---------------------------------------------------------------------//
    // 1. Start External Login (redirect to Google/Facebook/Microsoft/etc.) //
    // ---------------------------------------------------------------------//
    [HttpGet("external/{provider}")]
    public async Task<IActionResult> StartExternalLogin(string provider)
    {
        var result = await _mediator.Send(new StartExternalLoginRequest(provider));

        // result.AuthorizationUrl contains the OAuth URL of the provider
        return Redirect(result.AuthorizationUrl);
    }

    // --------------------------------------------------------------//
    // 2. Callback (provider → API) - handle the provider's response //
    // --------------------------------------------------------------//
    [HttpGet("external/{provider}/callback")]
    public async Task<IActionResult> ExternalLoginCallback(
        string provider,
        [FromQuery] string code,
        [FromQuery] string state)
    {
        var result = await _mediator.Send(
            new ExternalLoginCallbackRequest(provider, code, state)
        );

        if (!result.Success)
            return BadRequest(result.ErrorMessage);

        return Ok(result);
    }
}