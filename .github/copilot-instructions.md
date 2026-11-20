# Copilot instructions

This repository demonstrates ASP.NET Core authorization with two OpenIddict identity providers, a JWT-protected Web API, two Razor Pages clients, and a Blazor WebAssembly client hosted by ASP.NET Core. The `ApiJwtWithTwoSts.AppHost` project orchestrates the runnable server projects.

## Code guidelines

- Keep changes focused on the project responsible for the behavior: token validation belongs in `WebApi`, identity configuration in `IdentityProvider` or `IdentityProviderT2`, and browser authentication in the client projects.
- Preserve the two-provider distinction and its existing HTTPS ports and issuer values unless the task explicitly changes authentication configuration.
- Follow existing C# conventions: top-level statements for simple `Program.cs` files, file-scoped namespaces, implicit usings, and nullable annotations where already enabled.
- Keep secrets and local certificates out of source changes. Do not edit the unused `BlazorClient - Backup.Client.csproj`.
- Keep Aspire resource definitions in `ApiJwtWithTwoSts.AppHost/AppHost.cs`; do not introduce service-discovery configuration without migrating the existing fixed authority and API URLs.

## Build and test

Run the repository's existing CI commands from the repository root:

```bash
dotnet restore
dotnet build --no-restore
dotnet test --no-build --verbosity normal
```

There are currently no test projects. For Aspire orchestration changes, also build the AppHost project directly and inspect the dashboard/resource endpoints when the local environment supports running Aspire.
