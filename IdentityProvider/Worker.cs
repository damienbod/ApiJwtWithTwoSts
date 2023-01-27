using System.Globalization;
using OpenIddict.Abstractions;
using OpeniddictServer.Data;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OpeniddictServer;

public class Worker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        await RegisterApplicationsAsync(scope.ServiceProvider);
        await RegisterScopesAsync(scope.ServiceProvider);

        static async Task RegisterApplicationsAsync(IServiceProvider provider)
        {
            var manager = provider.GetRequiredService<IOpenIddictApplicationManager>();

            // API application CC
            if (await manager.FindByClientIdAsync("CC_STS_A") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "CC_STS_A",
                    ClientSecret = "cc_secret",
                    DisplayName = "CC for protected API",
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.Prefixes.Scope + "scope_used_for_api_in_protected_zone",
                        Permissions.Prefixes.Scope + "scope_a"
                    }
                });
            }

            // OIDC Code flow confidential client
            if (await manager.FindByClientIdAsync("codeflowpkceclient") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "codeflowpkceclient",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "OIDC confidential Code Flow PKCE",
                    DisplayNames =
                    {
                        [CultureInfo.GetCultureInfo("fr-FR")] = "Application cliente MVC"
                    },
                    PostLogoutRedirectUris =
                    {
                        new Uri("https://localhost:44360/signout-callback-oidc"),
                        new Uri("https://localhost:5001/signout-callback-oidc"),
                        new Uri("https://localhost:5001/signout-callback-oidc-t1")
                    },
                    RedirectUris =
                    {
                        new Uri("https://localhost:44360/signin-oidc"),
                        new Uri("https://localhost:5001/signin-oidc"),
                        new Uri("https://localhost:5001/signin-oidc-t1")
                    },
                    ClientSecret = "codeflow_pkce_client_secret",
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.Endpoints.Revocation,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "scope_used_for_api_in_protected_zone",
                        Permissions.Prefixes.Scope + "scope_a"
                    },
                    Requirements =
                    {
                        Requirements.Features.ProofKeyForCodeExchange
                    }
                });
            }
        }

        static async Task RegisterScopesAsync(IServiceProvider provider)
        {
            var manager = provider.GetRequiredService<IOpenIddictScopeManager>();

            if (await manager.FindByNameAsync("scope_used_for_api_in_protected_zone") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    DisplayName = "scope_used_for_api_in_protected_zone API access",
                    DisplayNames =
                    {
                        [CultureInfo.GetCultureInfo("fr-FR")] = "Accès à l'API de démo"
                    },
                    Name = "scope_used_for_api_in_protected_zone",
                    Resources =
                    {
                        "rs_scope_used_for_api_in_protected_zoneApi"
                    }
                });
            }

            if (await manager.FindByNameAsync("scope_a") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    DisplayName = "scope_a API access",
                    DisplayNames =
                    {
                        [CultureInfo.GetCultureInfo("fr-FR")] = "Accès à l'API de démo"
                    },
                    Name = "scope_a",
                    Resources =
                    {
                        "rs_scope_aApi"
                    }
                });
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
