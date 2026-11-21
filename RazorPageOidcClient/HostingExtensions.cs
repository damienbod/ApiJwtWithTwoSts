using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NetEscapades.AspNetCore.SecurityHeaders.Infrastructure;
using Serilog;

namespace RazorPageOidcClient;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        // Add service defaults & Aspire client integrations.
        builder.AddServiceDefaults();

        builder.Services.AddSecurityHeaderPolicies()
            .SetPolicySelector((PolicySelectorContext ctx) =>
            {
                return SecurityHeadersDefinitions.GetHeaderPolicyCollection(builder.Environment.IsDevelopment());
            });

        builder.Services.AddTransient<ApiService>();
        builder.Services.AddSingleton<ApiTokenInMemoryClient>();
        builder.Services.AddSingleton<ApiTokenCacheClient>();

        builder.Services.AddHttpClient();
        builder.Services.Configure<AuthConfigurations>(configuration.GetSection("AuthConfigurations"));

        var authConfigurations = configuration.GetSection("AuthConfigurations");
        var identityProvider = authConfigurations["IdentityProvider"];

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddOpenIdConnect(options =>
        {
            options.SignInScheme = "Cookies";
            options.Authority = identityProvider;
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

        builder.Services.AddAuthorization();
        builder.Services.AddRazorPages();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        IdentityModelEventSource.ShowPII = true;
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseSecurityHeaders();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        return app;
    }
}
