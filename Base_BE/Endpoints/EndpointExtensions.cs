using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace Base_BE.Endpoints;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        // Find all the EndpointGroupBase derived classes
        var endpointGroupTypes = typeof(EndpointExtensions).Assembly
            .GetExportedTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(EndpointGroupBase)));

        // Create an instance of each endpoint group and call its Map method
        foreach (var type in endpointGroupTypes)
        {
            var instance = Activator.CreateInstance(type) as EndpointGroupBase;
            instance?.Map(app);
        }

        // Add Google login shortcut endpoint
        app.MapGet("/google-login", async context =>
        {
            var callbackUrl = new Uri($"{context.Request.Scheme}://{context.Request.Host}/api/googleauth/callback");
            var properties = new AuthenticationProperties { RedirectUri = callbackUrl.ToString() };
            await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, properties);
        });

        return app;
    }
} 