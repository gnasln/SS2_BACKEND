using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Base_BE.Domain.Entities;
using System.Security.Claims;
using Base_BE.Domain.Constants;

namespace Base_BE.Endpoints;

[ApiController]
[Route("api/[controller]")]
public class GoogleAuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public GoogleAuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpGet("login")]
    public IActionResult GoogleLogin([FromQuery] string returnUrl = "/")
    {
        // Make sure returnUrl is properly validated to prevent open redirect vulnerabilities
        if (!Url.IsLocalUrl(returnUrl) && !returnUrl.StartsWith("/"))
        {
            returnUrl = "/";
        }

        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleCallback"),
            Items =
            {
                { "returnUrl", returnUrl }
            }
        };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            return BadRequest(new { error = "Google authentication failed", details = result.Failure?.Message });
        }

        var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
        var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
        var googleId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return BadRequest(new { error = "Email not found in Google response" });
        }

        // Check if user exists by email
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            // Create new user with a random username if needed
            string username = email.Split('@')[0] + "_" + Guid.NewGuid().ToString().Substring(0, 6);
            
            // Create new user
            user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                FullName = name,
                EmailConfirmed = true,
                Status = "Active"
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                return BadRequest(new { error = "Failed to create user", details = createResult.Errors.Select(e => e.Description) });
            }

            // Add user to role
            await _userManager.AddToRoleAsync(user, Roles.User);
        }

        // Sign in the user
        await _signInManager.SignInAsync(user, isPersistent: true);

        // Create claims for OpenIddict
        var claims = new List<Claim>
        {
            new Claim(OpenIddictConstants.Claims.Subject, user.Id),
            new Claim(OpenIddictConstants.Claims.Email, user.Email),
            new Claim(OpenIddictConstants.Claims.Name, user.FullName ?? string.Empty)
        };

        // Add roles to claims
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(OpenIddictConstants.Claims.Role, role));
        }

        // Create identity and principal
        var identity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // Get the return URL from the authentication properties
        var returnUrl = result.Properties?.Items["returnUrl"] ?? "/";

        // If client wants JSON response (e.g., for SPA)
        if (Request.Headers.Accept.Any(h => h.Contains("application/json")))
        {
            // Return token in JSON response
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Otherwise redirect to the return URL
        return Redirect(returnUrl);
    }
} 