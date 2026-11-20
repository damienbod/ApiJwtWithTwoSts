var builder = DistributedApplication.CreateBuilder(args);

var identityProvider = builder.AddProject<Projects.IdentityProvider>("identity-provider")
    .WithExternalHttpEndpoints();

var identityProviderT2 = builder.AddProject<Projects.IdentityProviderT2>("identity-provider-t2")
    .WithExternalHttpEndpoints();

var webApi = builder.AddProject<Projects.WebApi>("web-api")
    .WithReference(identityProvider)
    .WaitFor(identityProvider)
    .WithReference(identityProviderT2)
    .WaitFor(identityProviderT2)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.RazorPageOidcClient>("razor-page-oidc-client")
    .WithReference(webApi)
    .WaitFor(webApi)
    .WithReference(identityProvider)
    .WaitFor(identityProvider)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.RazorPageOidcClientT2>("razor-page-oidc-client-t2")
    .WithReference(webApi)
    .WaitFor(webApi)
    .WithReference(identityProviderT2)
    .WaitFor(identityProviderT2)
    .WithReference(identityProvider)
    .WaitFor(identityProvider)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.BlazorClient_Server>("blazor-client")
    .WithReference(identityProvider)
    .WaitFor(identityProvider)
    .WithReference(identityProviderT2)
    .WaitFor(identityProviderT2)
    .WithExternalHttpEndpoints();

builder.Build().Run();
