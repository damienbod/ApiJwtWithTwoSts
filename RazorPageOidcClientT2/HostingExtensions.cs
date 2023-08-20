using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;

namespace RazorPageOidcClient;

internal static class HostingExtensions
{
    private static IWebHostEnvironment? _env;

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        _env = builder.Environment;

        services.AddTransient<ApiService>();
        services.AddSingleton<ApiTokenInMemoryClient>();
        services.AddSingleton<ApiTokenCacheClient>();

        services.AddHttpClient();
        services.Configure<AuthConfigurations>(configuration.GetSection("AuthConfigurations"));

        var authConfigurations = configuration.GetSection("AuthConfigurations");
        var stsServer = authConfigurations["StsServer"];

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddOpenIdConnect(options =>
        {
            options.SignInScheme = "Cookies";
            options.Authority = stsServer;
            options.RequireHttpsMetadata = true;
            options.ClientId = "codeflowpkceclient";
            options.ClientSecret = "codeflow_pkce_client_secret";
            options.ResponseType = "code";
            options.UsePkce = true;
            options.Scope.Add("profile");
            options.Scope.Add("offline_access");
            options.SaveTokens = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name"
            };
        });

        services.AddAuthorization();
        services.AddRazorPages();

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        IdentityModelEventSource.ShowPII = true;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        app.UseSerilogRequestLogging();

        if (_env!.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseSecurityHeaders(
            SecurityHeadersDefinitions.GetHeaderPolicyCollection(_env!.IsDevelopment()));

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        return app;
    }
}
