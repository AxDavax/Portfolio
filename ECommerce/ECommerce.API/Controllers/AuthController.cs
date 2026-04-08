using ECommerce.API.Extensions;
using ECommerce.Application.UseCases.Auth.Login;
using ECommerce.Application.UseCases.Auth.Logout;
using ECommerce.Application.UseCases.Auth.Me;
using ECommerce.Application.UseCases.Auth.Refresh;
using ECommerce.Application.UseCases.Auth.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    public AuthController(
        LoginHandler loginHandler, 
        RefreshHandler refreshHandler, 
        MeHandler meHandler, 
        RegisterHandler registerHandler,
        LogoutHandler logoutHandler)
    {
        _loginHandler = loginHandler;
        _refreshHandler = refreshHandler;
        _meHandler = meHandler;
        _registerHandler = registerHandler;
        _logoutHandler = logoutHandler;
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
        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.GetUserId();

        var response = await _meHandler.Handle(new MeRequest
        {
            UserId = userId
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
}