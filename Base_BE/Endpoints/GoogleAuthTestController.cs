using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace Base_BE.Endpoints;

[ApiController]
[Route("api/google-auth-test")]
public class GoogleAuthTestController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public GoogleAuthTestController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult TestPage()
    {
        var html = @"
<!DOCTYPE html>
<html>
<head>
    <title>Google OAuth2 Test</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40px; line-height: 1.6; }
        h1 { color: #333; }
        .button { 
            display: inline-block;
            background-color: #4285F4;
            color: white;
            padding: 10px 20px;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
        }
        .container {
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 5px;
        }
        pre {
            background-color: #f5f5f5;
            padding: 10px;
            border-radius: 5px;
            overflow-x: auto;
        }
    </style>
</head>
<body>
    <div class='container'>
        <h1>Google OAuth2 Test</h1>
        <p>Click the button below to test Google login:</p>
        <p><a class='button' href='/api/googleauth/login'>Login with Google</a></p>
        
        <h2>Implementation Details</h2>
        <p>The following endpoints are available:</p>
        <ul>
            <li><code>/api/googleauth/login</code> - Start Google login flow</li>
            <li><code>/api/googleauth/callback</code> - Google callback endpoint</li>
        </ul>
        
        <h3>How it works:</h3>
        <ol>
            <li>User is redirected to Google for authentication</li>
            <li>After successful authentication, Google redirects back to the callback URL</li>
            <li>The user is created/authenticated in your application</li>
            <li>An OpenIddict token is issued for API access</li>
        </ol>
    </div>
</body>
</html>";

        return Content(html, "text/html");
    }
} 