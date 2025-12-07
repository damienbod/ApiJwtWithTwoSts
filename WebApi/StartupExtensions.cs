using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Security.Cryptography.X509Certificates;

namespace WebApi;

internal static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        var x509Certificate2 = GetCertificate(builder.Environment);

        services.AddSingleton<IAuthorizationHandler, MyApiHandler>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "SchemeStsA";
        })
        .AddJwtBearer("SchemeStsA", options =>
        {
            options.Audience = "rs_scope_aApi";
            options.Authority = "https://localhost:44318";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidAudiences = new List<string> { "rs_scope_aApi" },
                ValidIssuers = new List<string> { "https://localhost:44318" },
            };

        })
        .AddJwtBearer("SchemeStsB", options =>
        {
            options.Audience = "rs_scope_bApi";
            options.Authority = "https://localhost:44367";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidAudiences = new List<string> { "rs_scope_bApi" },
                ValidIssuers = new List<string> { "https://localhost:44367" },
            };
        });

        //.AddJwtBearer(options =>
        //{
        //    options.Audience = "ProtectedApiResource";
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidIssuers = new List<string> { "https://localhost:44318", "https://localhost:44367" },
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new X509SecurityKey(x509Certificate2),
        //        IssuerSigningKeyResolver = 
        //        (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
        //          => new List<X509SecurityKey> { new X509SecurityKey(x509Certificate2) }
        //    };
        //});

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes("SchemeStsA", "SchemeStsB")
                .Build();

            options.AddPolicy("MyPolicy", policy =>
            {
                policy.AddRequirements(new MyApiRequirement());
            });
        });

        //services.AddAuthorization(options =>
        //    options.AddPolicy("protectedScope", policy =>
        //    {
        //        policy.RequireClaim("scope", "scope_used_for_api_in_protected_zone");
        //    })
        //);

        services.AddControllers();

        builder.Services.AddOpenApi(options =>
        {
            //options.UseTransformer((document, context, cancellationToken) =>
            //{
            //    document.Info = new()
            //    {
            //        Title = "My API",
            //        Version = "v1",
            //        Description = "API for Damien"
            //    };
            //    return Task.CompletedTask;
            //});
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

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
            app.UseHsts();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        //app.MapOpenApi(); // /openapi/v1.json
        app.MapOpenApi("/openapi/v1/openapi.json");
        //app.MapOpenApi("/openapi/{documentName}/openapi.json");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1/openapi.json", "v1");
            });
        }

        return app;
    }

    private static X509Certificate2 GetCertificate(IWebHostEnvironment environment)
    {
        var cert = X509CertificateLoader
            .LoadPkcs12FromFile(Path.Combine(environment.ContentRootPath, "sts_dev_cert.pfx"), "1234");

        return cert;
    }
}
