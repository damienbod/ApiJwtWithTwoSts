var builder = DistributedApplication.CreateBuilder(args);

var identityProvider = builder.AddProject<Projects.IdentityProvider>("identity-provider")
    .WithExternalHttpEndpoints();

var identityProviderT2 = builder.AddProject<Projects.IdentityProviderT2>("identity-provider-t2")
    .WithExternalHttpEndpoints();

var webApi = builder.AddProject<Projects.WebApi>("web-api")
    .WithReference(identityProviderT2)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.RazorPageOidcClient>("razor-page-oidc-client")
    .WithReference(webApi)
    .WithReference(identityProvider)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.RazorPageOidcClientT2>("razor-page-oidc-client-t2")
    .WithReference(webApi)
    .WithReference(identityProviderT2)
    .WithReference(identityProvider)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.BlazorClient_Server>("blazor-client")
    .WithExternalHttpEndpoints();

builder.Build().Run();
