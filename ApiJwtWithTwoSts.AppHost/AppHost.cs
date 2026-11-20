var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.IdentityProvider>("identity-provider")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.IdentityProviderT2>("identity-provider-t2")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.WebApi>("web-api")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.RazorPageOidcClient>("razor-page-oidc-client")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.RazorPageOidcClientT2>("razor-page-oidc-client-t2")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.BlazorClient_Server>("blazor-client")
    .WithExternalHttpEndpoints();

builder.Build().Run();
