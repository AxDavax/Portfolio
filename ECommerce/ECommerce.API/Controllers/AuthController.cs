using ECommerce.Application.OAuth.UseCases;
using ECommerce.Application.UseCases.Auth.ForgotPassword;
using ECommerce.Application.UseCases.Auth.Login;
using ECommerce.Application.UseCases.Auth.Logout;
using ECommerce.Application.UseCases.Auth.Me;
using ECommerce.Application.UseCases.Auth.Refresh;
using ECommerce.Application.UseCases.Auth.Register;
using ECommerce.Application.UseCases.Auth.ResetPassword;
using ECommerce.Contracts.Auth.ForgotPassword;
using ECommerce.Contracts.Auth.Login;
using ECommerce.Contracts.Auth.Logout;
using ECommerce.Contracts.Auth.Refresh;
using ECommerce.Contracts.Auth.Register;
using ECommerce.Contracts.Auth.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginHandler _loginHandler;
    private readonly RefreshHandler _refreshHandler;
    private readonly MeHandler _meHandler;
    private readonly RegisterHandler _registerHandler;
    private readonly LogoutHandler _logoutHandler;
    private readonly ForgotPasswordHandler _forgotPwdHandler;
    private readonly ResetPasswordHandler _resetPwdHandler;
    private readonly IMediator _mediator;

    public AuthController(
        LoginHandler loginHandler, 
        RefreshHandler refreshHandler, 
        MeHandler meHandler, 
        RegisterHandler registerHandler,
        LogoutHandler logoutHandler,
        ForgotPasswordHandler forgotPwdHandler, 
        ResetPasswordHandler resetPwdHandler,
        IMediator mediator)
    {
        _loginHandler = loginHandler;
        _refreshHandler = refreshHandler;
        _meHandler = meHandler;
        _registerHandler = registerHandler;
        _logoutHandler = logoutHandler;
        _forgotPwdHandler = forgotPwdHandler;
        _resetPwdHandler = resetPwdHandler;
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _registerHandler.HandleAsync(request);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _loginHandler.Handle(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var response = await _refreshHandler.Handle(request);

        if(!response.Success)
            return Unauthorized(response.Message);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier) ??
            User.FindFirst("uid") ??
            User.FindFirst(JwtRegisteredClaimNames.Sub);

        if (userIdClaim == null) return Unauthorized();

        var response = await _meHandler.Handle(new MeRequest
        {
            UserId = Guid.Parse(userIdClaim.Value)
        });

        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var result = await _logoutHandler.HandleAsync(request);
        return Ok(result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var result = await _forgotPwdHandler.HandleAsync(request);
        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _resetPwdHandler.HandleAsync(request);
        return Ok(result);
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

        if(!result.Success)
            return BadRequest(result.ErrorMessage);

        return Ok(result);
    }
}