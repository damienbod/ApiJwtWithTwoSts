var builder = DistributedApplication.CreateBuilder(args);

var identityProvider = builder.AddDotnetProject("identity-provider", @"..\IdentityProvider\IdentityProvider.csproj")
    .WithExternalHttpEndpoints();

var identityProviderT2 = builder.AddDotnetProject("identity-provider-t2", @"..\IdentityProviderT2\IdentityProviderT2.csproj")
    .WithExternalHttpEndpoints();

var webApi = builder.AddDotnetProject("web-api", @"..\WebApi\WebApi.csproj")
    .WithReference(identityProvider)
    .WaitFor(identityProvider)
    .WithReference(identityProviderT2)
    .WaitFor(identityProviderT2)
    .WithExternalHttpEndpoints();

builder.AddDotnetProject("razor-page-oidc-client", @"..\RazorPageOidcClient\RazorPageOidcClient.csproj")
    .WithEnvironment("AuthConfigurations__ProtectedApiUrl", webApi.GetEndpoint("https"))
    .WithReference(webApi)
    .WaitFor(webApi)
    .WithReference(identityProvider)
    .WaitFor(identityProvider)
    .WithExternalHttpEndpoints();

builder.AddDotnetProject("razor-page-oidc-client-t2", @"..\RazorPageOidcClientT2\RazorPageOidcClientT2.csproj")
    .WithEnvironment("AuthConfigurations__ProtectedApiUrl", webApi.GetEndpoint("https"))
    .WithReference(webApi)
    .WaitFor(webApi)
    .WithReference(identityProviderT2)
    .WaitFor(identityProviderT2)
    .WithReference(identityProvider)
    .WaitFor(identityProvider)
    .WithExternalHttpEndpoints();

builder.AddDotnetProject("blazor-client", @"..\BlazorClient\Server\BlazorClient.Server.csproj")
    .WithReference(identityProvider)
    .WaitFor(identityProvider)
    .WithReference(identityProviderT2)
    .WaitFor(identityProviderT2)
    .WithExternalHttpEndpoints();

builder.Build().Run();
